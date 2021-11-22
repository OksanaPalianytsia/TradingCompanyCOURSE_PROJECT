using DAL.Interfaces;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ADO
{
    public class CategoryDAL : ICategoryDAL
    {
        private string _connStr;
        public CategoryDAL(string connStr)
        {
            this._connStr = connStr;
        }
        public void CreateCategory(string name)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
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

        public void DeleteCategory(int categoryID)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
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

        public List<CategoryDTO> GetAllCategories()
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
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

        public CategoryDTO GetCategoryById(int categoryId)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_by_id_category", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@cCategoryID", categoryId);

                SqlDataReader reader = comm.ExecuteReader();
                var category = new CategoryDTO();
                while (reader.Read())
                {
                    category.CategoryID = (int)reader["CategoryID"];
                    category.Name = reader["Name"].ToString();
                    category.RowInsertTime = DateTime.Parse(reader["RowInsertTime"].ToString());
                    category.RowUpdateTime = DateTime.Parse(reader["RowUpdateTime"].ToString());
                }

                conn.Close();
                return category;
            }
        }

        public void UpdateCategory(int categoryID, string name)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("update_category", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@cCategoryID", categoryID);
                comm.Parameters.AddWithValue("@cName", name);

                int rowUpdated = comm.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
