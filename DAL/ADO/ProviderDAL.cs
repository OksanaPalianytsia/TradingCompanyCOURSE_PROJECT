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
    public class ProviderDAL : IProviderDAL
    {
        private string _connStr;
        public ProviderDAL(string connStr)
        {
            this._connStr = connStr;
        }
        public void CreateProvider(string name)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("insert_provider", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@pName", name);

                int rowsAffected = comm.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void DeleteProvider(int providerID)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("delete_provider", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@pProviderID", providerID);

                int rowsDeleted = comm.ExecuteNonQuery();

                conn.Close();
            }
        }

        public List<ProviderDTO> GetAllProviders()
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_providers", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = comm.ExecuteReader();

                var providers = new List<ProviderDTO>();

                while (reader.Read())
                {
                    providers.Add(new ProviderDTO
                    {
                        ProviderID = (int)reader["ProviderID"],
                        Name = reader["Name"].ToString(),
                        RowInsertTime = DateTime.Parse(reader["RowInsertTime"].ToString()),
                        RowUpdateTime = DateTime.Parse(reader["RowUpdateTime"].ToString())
                    });
                }
                conn.Close();
                return providers;
            }
        }

        public ProviderDTO GetProviderById(int providerID)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_by_id_provider", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@pProviderID", providerID);

                SqlDataReader reader = comm.ExecuteReader();
                var provider = new ProviderDTO();
                while (reader.Read())
                {
                    provider.ProviderID = (int)reader["ProviderID"];
                    provider.Name = reader["Name"].ToString();
                    provider.RowInsertTime = DateTime.Parse(reader["RowInsertTime"].ToString());
                    provider.RowUpdateTime = DateTime.Parse(reader["RowUpdateTime"].ToString());
                }

                conn.Close();
                return provider;
            }
        }

        public void UpdateProvider(int providerID, string name)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("update_provider", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@pProviderID", providerID);
                comm.Parameters.AddWithValue("@pName", name);

                int rowUpdated = comm.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
