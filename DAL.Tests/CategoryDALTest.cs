using DAL.ADO;
using DAL.Interfaces;
using DTO;
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
    public class CategoryDALTest
    {
        ICategoryDAL temp;
        string connection;
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ProviderDALTest));

        public CategoryDALTest()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [OneTimeSetUp]
        // 
        public void SetupTest()
        {
            connection = ConfigurationManager.ConnectionStrings["ShopTest"].ConnectionString;
            temp = new CategoryDAL(connection);
            _log.Info("[Test] CATEGORIES set up completed");
        }
        public List<CategoryDTO> GetItemsBySqlForTest()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_categories", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = comm.ExecuteReader();

                var categories = new List<CategoryDTO>();

                while (reader.Read())
                {
                    categories.Add(new CategoryDTO
                    {
                        CategoryID = (int)reader["CategoryID"],
                        Name = reader["Name"].ToString(),
                        RowInsertTime = DateTime.Parse(reader["RowInsertTime"].ToString()),
                        RowUpdateTime = DateTime.Parse(reader["RowUpdateTime"].ToString())
                    });
                }
                conn.Close();
                return categories;
            }
        }
        public void InsertItemBySqlForTest(string name)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("insert_cathegory", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@cName", name);

                int rowsAffected = comm.ExecuteNonQuery();

                conn.Close();
            }
        }
        public void DeleteItemBySqlForTest(int categoryID)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("delete_category", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@cCategoryID", categoryID);

                int rowsDeleted = comm.ExecuteNonQuery();

                conn.Close();
            }
        }
        [Test]
        public void GetAllCategoriesTest()
        {
            var random = new Random();
            CategoryDTO prov_for_test = new CategoryDTO
            {
                CategoryID = random.Next(),
                Name = "Jewelry",
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };
            InsertItemBySqlForTest(prov_for_test.Name);
            var categories = temp.GetAllCategories();
            Assert.AreEqual(categories[categories.Count() - 1].Name, prov_for_test.Name);
            DeleteItemBySqlForTest(categories[categories.Count() - 1].CategoryID);
            _log.Info(categories[categories.Count() - 1].Name == prov_for_test.Name ? "PASS GetAllCategoriesTest()" : "FAIL GetAllCategoriesTest()");

        }

        [Test]
        public void UpdateCategoryTest()
        {
            var categories = GetItemsBySqlForTest();

            DateTime expected = categories[categories.Count() - 1].RowUpdateTime;
            temp.UpdateCategory(categories[categories.Count() - 1].CategoryID, categories[categories.Count() - 1].Name);

            categories = GetItemsBySqlForTest();
            DateTime actual = categories[categories.Count() - 1].RowUpdateTime;

            Assert.AreNotEqual(expected, actual);
            _log.Info(expected != actual ? "PASS UpdateCategoryTest()" : "FAIL UpdateCategoryTest()");

        }

        [Test]
        public void CreateCategoryTest()
        {
            var random = new Random();
            var categories = GetItemsBySqlForTest();
            CategoryDTO cat_for_test = new CategoryDTO
            {

                CategoryID = random.Next(),
                Name = "Technique",
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            temp.CreateCategory(cat_for_test.Name);
            categories = GetItemsBySqlForTest();

            Assert.IsTrue(categories.Any(m => m.Name == cat_for_test.Name));
            _log.Info(categories.Any(m => m.Name == cat_for_test.Name) == true ? "PASS CreateCategoryTest()" : "FAIL CreateCategoryTest()");

        }

        [Test]
        public void DeleteCategoryTest()
        {
            var categories = GetItemsBySqlForTest();

            CategoryDTO expected_category = categories[categories.Count() - 1];
            temp.DeleteCategory(categories[categories.Count() - 1].CategoryID);

            categories = GetItemsBySqlForTest();
            CategoryDTO actual_category = categories[categories.Count() - 1];

            Assert.AreNotEqual(expected_category, actual_category);
            _log.Info(expected_category != actual_category ? "PASS DeleteCategoryTest()" : "FAIL DeleteCategoryTest()");

        }


        [Test]
        public void GetCreatedItemByID()
        {
            var random = new Random();
            CategoryDTO cat_for_test = new CategoryDTO
            {

                CategoryID = random.Next(),
                Name = "Plants",
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            InsertItemBySqlForTest(cat_for_test.Name);

            var categories = GetItemsBySqlForTest();
            CategoryDTO actual_category = temp.GetCategoryById(categories[categories.Count() - 1].CategoryID);
            DeleteItemBySqlForTest(categories[categories.Count() - 1].CategoryID);

            Assert.AreEqual(cat_for_test.Name, actual_category.Name);
            _log.Info(cat_for_test.Name == actual_category.Name ? "PASS GetCreatedItemByID()" : "FAIL GetCreatedItemByID()");

        }
    }
}
