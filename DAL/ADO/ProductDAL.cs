using DAL.Interfaces;
using DTO;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ADO
{
    public class ProductDAL : IProductDAL
    {
        public string _connStr;

        public ProductDAL(string connstr)
        {
            this._connStr = connstr;
        }

        public void AddProductSupplies(int productID, int value)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("add_supplies", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@pProductID", productID);
                comm.Parameters.AddWithValue("@Value", value);

                int rowUpdated = comm.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void CreateProduct(string name, int categoryID, int price, int quantity)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
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
        
        public void DeleteProduct(int productID)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
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

        public void ExcludeProductSupplies(int productID, int value)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("exclude_supplies", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@pProductID", productID);
                comm.Parameters.AddWithValue("@Value", value);

                int rowUpdated = comm.ExecuteNonQuery();

                conn.Close();
            }
        }

        public List<ProductDTO> GetAllProducts()
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
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

        public ProductDTO GetProductById(int productID)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_by_id_product", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@pProductID", productID);

                SqlDataReader reader = comm.ExecuteReader();
                var product = new ProductDTO();
                while (reader.Read())
                {
                    product.ProductID = (int)reader["ProductID"];
                    product.Name = reader["Name"].ToString();
                    product.CategoryID = (int)reader["CategoryID"];
                    product.Price = (int)reader["Price"];
                    product.Quantity = (int)reader["Quantity"];
                    product.RowInsertTime = DateTime.Parse(reader["RowInsertTime"].ToString());
                    product.RowUpdateTime = DateTime.Parse(reader["RowUpdateTime"].ToString());
                }


                conn.Close();
                return product;
            }
        }

        public List<ProductDTO> GetProductsByCategories(int categoryId)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_products_by_categories", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@pCategoryID", categoryId);

                SqlDataReader reader = comm.ExecuteReader();

                var special_id_products = new List<ProductDTO>();

                while (reader.Read())
                {
                    special_id_products.Add(new ProductDTO
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
                return special_id_products;
            }
        }

        public List<ProductDTO> GetProductsByName(string name)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_products_by_name", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@pName", name);

                SqlDataReader reader = comm.ExecuteReader();

                var special_name_products = new List<ProductDTO>();

                while (reader.Read())
                {
                    special_name_products.Add(new ProductDTO
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
                return special_name_products;
            }
        }

        public List<ProductDTO> GetProductsByPrice(int price)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_products_by_price", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@pPrice", price);

                SqlDataReader reader = comm.ExecuteReader();

                var special_price_products = new List<ProductDTO>();

                while (reader.Read())
                {
                    special_price_products.Add(new ProductDTO
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
                return special_price_products;
            }
        }

        public List<ProductDTO> SortProductsByCategory()
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("sort_product_by_category", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = comm.ExecuteReader();

                var products_by_name = new List<ProductDTO>();

                while (reader.Read())
                {
                    products_by_name.Add(new ProductDTO
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
                return products_by_name;
            }
        }

        public List<ProductDTO> SortProductsByName()
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("sort_product_by_name", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = comm.ExecuteReader();

                var products_by_name = new List<ProductDTO>();

                while (reader.Read())
                {
                    products_by_name.Add(new ProductDTO
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
                return products_by_name;
            }
        }

        public List<ProductDTO> SortProductsByQuantity()
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("sort_product_by_quantity", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = comm.ExecuteReader();

                var products_by_name = new List<ProductDTO>();

                while (reader.Read())
                {
                    products_by_name.Add(new ProductDTO
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
                return products_by_name;
            }
        }

        public List<ProductDTO> SortProductsByUpdateDate()
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("sort_product_by_date", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = comm.ExecuteReader();

                var products_by_name = new List<ProductDTO>();

                while (reader.Read())
                {
                    products_by_name.Add(new ProductDTO
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
                return products_by_name;
            }
        }

        public void UpdateProduct(int productID, string name, int categoryID, int price, int quantity)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("update_product", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@pProductID", productID);
                comm.Parameters.AddWithValue("@pName", name);
                comm.Parameters.AddWithValue("@pCategoryID", categoryID);
                comm.Parameters.AddWithValue("@pPrice", price);
                comm.Parameters.AddWithValue("@pQuantity", quantity);

                int rowUpdated = comm.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
