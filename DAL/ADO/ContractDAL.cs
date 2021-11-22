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
    public class ContractDAL : IContractDAL
    {
        private string _connStr;
        public ContractDAL(string connStr)
        {
            this._connStr = connStr;
        }
        public void CreateContract(int productID, int providerID, int quantity)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("insert_contract", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@cProductID", productID);
                comm.Parameters.AddWithValue("@cProviderID", providerID);
                comm.Parameters.AddWithValue("@cQuantity", quantity);
                comm.Parameters.AddWithValue("@cIsActive", true);

                int rowsAffected = comm.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void DeActivateContractById(int contractId)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("deactivate_contract_by_id", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@cContractID", contractId);

                int rowUpdated = comm.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void DeleteContract(int contractID)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("delete_contract", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@cContractId", contractID);

                int rowsDeleted = comm.ExecuteNonQuery();

                conn.Close();
            }
        }

        public List<ContractDTO> GetActiveContracts()
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_active_contracts", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = comm.ExecuteReader();

                var active_contracts = new List<ContractDTO>();

                while (reader.Read())
                {
                    active_contracts.Add(new ContractDTO
                    {
                        ContractID = (int)reader["ContractID"],
                        ProductID = (int)reader["ProductID"],
                        ProviderID = (int)reader["ProviderID"],
                        Quantity = (int)reader["Quantity"],
                        IsActive = (bool)reader["IsActive"],
                        RowInsertTime = DateTime.Parse(reader["RowInsertTime"].ToString()),
                        RowUpdateTime = DateTime.Parse(reader["RowUpdateTime"].ToString())
                    });
                }
                conn.Close();
                return active_contracts;
            }
        }

        public List<ContractDTO> GetAllContracts()
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_contracts", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = comm.ExecuteReader();

                var contracts = new List<ContractDTO>();

                while (reader.Read())
                {
                    contracts.Add(new ContractDTO
                    {
                        ContractID = (int)reader["ContractID"],
                        ProductID = (int)reader["ProductID"],
                        ProviderID = (int)reader["ProviderID"],
                        Quantity = (int)reader["Quantity"],
                        IsActive = (bool)reader["IsActive"],
                        RowInsertTime = DateTime.Parse(reader["RowInsertTime"].ToString()),
                        RowUpdateTime = DateTime.Parse(reader["RowUpdateTime"].ToString())
                    });
                }
                conn.Close();
                return contracts;
            }
        }

        public ContractDTO GetContractById(int contractId)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_by_id_contract", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@cContractID", contractId);

                SqlDataReader reader = comm.ExecuteReader();
                var contract = new ContractDTO();
                while (reader.Read())
                {
                    contract.ContractID = (int)reader["ContractID"];
                    contract.ProductID = (int)reader["ProductID"];
                    contract.ProviderID = (int)reader["ProviderID"];
                    contract.Quantity = (int)reader["Quantity"];
                    contract.IsActive = (bool)reader["IsActive"];
                    contract.RowInsertTime = DateTime.Parse(reader["RowInsertTime"].ToString());
                    contract.RowUpdateTime = DateTime.Parse(reader["RowUpdateTime"].ToString());
                }

                conn.Close();
                return contract;
            }
        }

        public void UpdateContract(int contractID, int productID, int providerID, int quantity, bool isActive)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("update_contract", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@cContractID", contractID);
                comm.Parameters.AddWithValue("@cProductID", productID);
                comm.Parameters.AddWithValue("@cProviderID", providerID);
                comm.Parameters.AddWithValue("@cQuantity", quantity);
                comm.Parameters.AddWithValue("@cIsActive", isActive);

                int rowUpdated = comm.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
