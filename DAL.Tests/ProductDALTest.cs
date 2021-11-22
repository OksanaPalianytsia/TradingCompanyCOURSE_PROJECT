using DAL.ADO;
using DAL.Interfaces;
using DTO;
using LOG;
using log4net.Config;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Tests
{
    [TestFixture]
    public class ProductDALTest
    {
        IProductDAL temp;
        string connection;
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ProductDALTest));
        public ProductDALTest()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [OneTimeSetUp]
        // 
        public void SetupTest()
        {

            connection = ConfigurationManager.ConnectionStrings["ShopTest"].ConnectionString;
            temp = new ProductDAL(connection);
            BasicConfigurator.Configure();
            _log.Info("[Test] PRODUCTS set up completed");

        }
        public List<ProductDTO> GetItemsBySqlForTest()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_products", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = comm.ExecuteReader();

                var products = new List<ProductDTO>();

                while (reader.Read())
                {
                    products.Add(new ProductDTO
                    {
                        ProductID = (int)reader["ProductID"],
                        Name = reader["Name"].ToString(),
                        CategoryID = (int)reader["CategoryID"],
                        Price = (int)reader["Price"],
                        Quantity = (int)reader["Quantity"],
                        RowInsertTime = DateTime.Parse(reader["RowInsertTime"].ToString()),
                        RowUpdateTime = DateTime.Parse(reader["RowUpdateTime"].ToString())
                    });
                }
                conn.Close();
                return products;
            }
        }
        public void InsertItemBySqlForTest(string name, int categoryID, int price, int quantity)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("insert_product", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@pName", name);
                comm.Parameters.AddWithValue("@pCategoryID", categoryID);
                comm.Parameters.AddWithValue("@pPrice", price);
                comm.Parameters.AddWithValue("@pQuantity", quantity);

                int rowsAffected = comm.ExecuteNonQuery();

                conn.Close();
            }
        }
        public void DeleteItemBySqlForTest(int productID)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("delete_product", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@pProductID", productID);

                int rowsDeleted = comm.ExecuteNonQuery();

                conn.Close();
            }
        }

        [Test]
        public void GetAllProductsTest()
        {
            var random = new Random();
            ProductDTO prod_for_test = new ProductDTO
            {
                ProductID = random.Next(),
                Name = "Skirt",
                CategoryID = 4,
                Price = 450,
                Quantity = 1,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };
            InsertItemBySqlForTest(prod_for_test.Name, prod_for_test.CategoryID, prod_for_test.ProductID, prod_for_test.Quantity);
            var products = temp.GetAllProducts();
            Assert.AreEqual(products[products.Count() - 1].Name, prod_for_test.Name);
            DeleteItemBySqlForTest(products[products.Count() - 1].ProductID);
            _log.Info(products[products.Count() - 1].Name == prod_for_test.Name ? "PASS GetAllProductsTest()" : "FAIL GetAllProductsTest()");
        }

        [Test]
        public void UpdateProductTest()
        {
            var products = GetItemsBySqlForTest();

            DateTime expected = products[products.Count() - 1].RowUpdateTime;
            temp.UpdateProduct(products[products.Count() - 1].ProductID, products[products.Count() - 1].Name, products[products.Count() - 1].CategoryID, products[products.Count() - 1].Price, products[products.Count() - 1].Quantity);


            products = GetItemsBySqlForTest();
            DateTime actual = products[products.Count() - 1].RowUpdateTime;

            Assert.AreNotEqual(expected, actual);
            _log.Info(expected != actual ? "PASS UpdateProductTest()" : "FAIL UpdateProductTest()");
        }

        [Test]
        public void CreateProductTest()
        {
            var random = new Random();
            var products = GetItemsBySqlForTest();
            ProductDTO prod_for_test = new ProductDTO
            {
                ProductID = random.Next(),
                Name = "Butter",
                CategoryID = 1,
                Price = 45,
                Quantity = 1,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };


            temp.CreateProduct(prod_for_test.Name, prod_for_test.CategoryID, prod_for_test.Price, prod_for_test.Quantity);
            products = GetItemsBySqlForTest();

            Assert.IsTrue(products.Any(m => m.Name == prod_for_test.Name));
            _log.Info(products.Any(m => m.Name == prod_for_test.Name) == true ? "PASS CreateProductTest()" : "FAIL CreateProductTest()");
        }

        [Test]
        public void DeleteProductTest()
        {
            var products = GetItemsBySqlForTest();

            ProductDTO expected_product = products[products.Count() - 1];
            temp.DeleteProduct(products[products.Count() - 1].ProductID);

            products = GetItemsBySqlForTest();
            ProductDTO actual_product = products[products.Count() - 1];

            Assert.AreNotEqual(expected_product, actual_product);
            _log.Info(expected_product != actual_product ? "PASS DeleteProductTest()" : "FAIL DeleteProductTest()");

        }


        [Test]
        public void GetCreatedItemByID()
        {
            var random = new Random();
            ProductDTO prod_for_test = new ProductDTO
            {
                ProductID = random.Next(),
                Name = "Orange",
                CategoryID = 1,
                Price = 52,
                Quantity = 3,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            InsertItemBySqlForTest(prod_for_test.Name, prod_for_test.CategoryID, prod_for_test.Price, prod_for_test.Quantity);

            var products = GetItemsBySqlForTest();
            ProductDTO actual_product = temp.GetProductById(products[products.Count() - 1].ProductID);

            DeleteItemBySqlForTest(products[products.Count() - 1].ProductID);
            Assert.AreEqual(prod_for_test.Name, actual_product.Name);
            _log.Info(prod_for_test.Name == actual_product.Name ? "PASS GetCreatedProductByID()" : "FAIL GetCreatedProductByID()");

        }

        [Test]
        public void GetProductsByNameTest()
        {
            var random = new Random();
            ProductDTO prod_for_test = new ProductDTO
            {
                ProductID = random.Next(),
                Name = "Orange",
                CategoryID = 1,
                Price = 52,
                Quantity = 3,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            InsertItemBySqlForTest(prod_for_test.Name, prod_for_test.CategoryID, prod_for_test.Price, prod_for_test.Quantity);

            var products = GetItemsBySqlForTest();
            List<ProductDTO> actual_product = temp.GetProductsByName(products[products.Count() - 1].Name);

            DeleteItemBySqlForTest(products[products.Count() - 1].ProductID);
            Assert.AreEqual(prod_for_test.Name, actual_product[actual_product.Count() - 1].Name);
            _log.Info(prod_for_test.Name == actual_product[actual_product.Count() - 1].Name ? "PASS GetProductsByNameTest()" : "FAIL GetProductsByNameTest()");

        }

        [Test]
        public void GetProductsByCategoriesTest()
        {
            var random = new Random();
            ProductDTO prod_for_test = new ProductDTO
            {
                ProductID = random.Next(),
                Name = "Orange",
                CategoryID = 1,
                Price = 52,
                Quantity = 3,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            InsertItemBySqlForTest(prod_for_test.Name, prod_for_test.CategoryID, prod_for_test.Price, prod_for_test.Quantity);

            var products = GetItemsBySqlForTest();
            List<ProductDTO> actual_product = temp.GetProductsByCategories(products[products.Count() - 1].CategoryID);

            DeleteItemBySqlForTest(products[products.Count() - 1].ProductID);
            Assert.AreEqual(prod_for_test.CategoryID, actual_product[actual_product.Count() - 1].CategoryID);
            _log.Info(prod_for_test.CategoryID == actual_product[actual_product.Count() - 1].CategoryID ? "PASS GetProductsByCategoriesTest()" : "FAIL GetProductsByCategoriesTest()");

        }

        [Test]
        public void GetProductsByPriceTest()
        {
            var random = new Random();
            ProductDTO prod_for_test = new ProductDTO
            {
                ProductID = random.Next(),
                Name = "Orange",
                CategoryID = 1,
                Price = 52,
                Quantity = 3,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            InsertItemBySqlForTest(prod_for_test.Name, prod_for_test.CategoryID, prod_for_test.Price, prod_for_test.Quantity);

            var products = GetItemsBySqlForTest();
            List<ProductDTO> actual_product = temp.GetProductsByPrice(products[products.Count() - 1].Price);

            DeleteItemBySqlForTest(products[products.Count() - 1].ProductID);
            Assert.AreEqual(prod_for_test.Price, actual_product[actual_product.Count() - 1].Price);
            _log.Info(prod_for_test.Price == actual_product[actual_product.Count() - 1].Price ? "PASS GetProductsByPriceTest()" : "FAIL GetProductsByPriceTest()");

        }

        [Test]
        public void SortProductsByNameTest()
        {
            var products = temp.SortProductsByName();
            Assert.AreEqual(string.Compare(products[products.Count() - 1].Name, products[products.Count() - 2].Name), 1);
            _log.Info(string.Compare(products[products.Count() - 1].Name, products[products.Count() - 2].Name) == 1 ? "PASS SortProductsByNameTest()" : "FAIL SortProductsByNameTest()");

        }

        [Test]
        public void SortProductsByCategoryTest()
        {
            var products = temp.SortProductsByCategory();
            Assert.IsTrue(products[products.Count() - 1].CategoryID >= products[products.Count() - 2].CategoryID);
            _log.Info(products[products.Count() - 1].CategoryID >= products[products.Count() - 2].CategoryID ? "PASS SortProductsByCategoryTest()" : "FAIL SortProductsByCategoryTest()");
        }

        [Test]
        public void SortProductsByQuantityTest()
        {
            var products = temp.SortProductsByQuantity();
            Assert.IsTrue(products[products.Count() - 1].Quantity >= products[products.Count() - 2].Quantity);
            _log.Info(products[products.Count() - 1].Quantity >= products[products.Count() - 2].Quantity ? "PASS SortProductsByQuantityTest()" : "FAIL SortProductsByQuantityTest()");
        }

        [Test]
        public void SortProductsByUpdateDateTest()
        {
            var products = temp.SortProductsByQuantity();
            Assert.AreEqual(DateTime.Compare(products[products.Count() - 1].RowUpdateTime, products[products.Count() - 2].RowUpdateTime), 1);
            _log.Info(DateTime.Compare(products[products.Count() - 1].RowUpdateTime, products[products.Count() - 2].RowUpdateTime) == 1 ? "PASS SortProductsByUpdateDateTest()" : "FAIL SortProductsByUpdateDateTest()");

        }

        [Test]
        public void AddProductSuppliesTest() 
        {
            var random = new Random();
            ProductDTO prod_for_test = new ProductDTO
            {
                ProductID = random.Next(),
                Name = "New_Product",
                CategoryID = 1,
                Price = 10,
                Quantity = 100,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            InsertItemBySqlForTest(prod_for_test.Name, prod_for_test.CategoryID, prod_for_test.Price, prod_for_test.Quantity);
            var products = GetItemsBySqlForTest();
            temp.AddProductSupplies(products[products.Count() - 1].ProductID, 30);
            products = GetItemsBySqlForTest();
            Assert.AreEqual(130, products[products.Count() - 1].Quantity);
            DeleteItemBySqlForTest(products[products.Count() - 1].ProductID);
            _log.Info(products[products.Count() - 1].Quantity == 130 ? "PASS AddProductSuppliesTest()" : "FAIL AddProductSuppliesTest()");

        }

        [Test]
        public void ExcludeProductSuppliesTest()
        {
            var random = new Random();
            ProductDTO prod_for_test = new ProductDTO
            {
                ProductID = random.Next(),
                Name = "New_Product2",
                CategoryID = 1,
                Price = 10,
                Quantity = 100,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            InsertItemBySqlForTest(prod_for_test.Name, prod_for_test.CategoryID, prod_for_test.Price, prod_for_test.Quantity);
            var products = GetItemsBySqlForTest();
            temp.ExcludeProductSupplies(products[products.Count() - 1].ProductID, 140);
            products = GetItemsBySqlForTest();
            Assert.AreEqual(-40, products[products.Count() - 1].Quantity);
            DeleteItemBySqlForTest(products[products.Count() - 1].ProductID);
            _log.Info(products[products.Count() - 1].Quantity == -40 ? "PASS ExcludeProductSuppliesTest()" : "FAIL ExcludeProductSuppliesTest()");

        }
    }
}
