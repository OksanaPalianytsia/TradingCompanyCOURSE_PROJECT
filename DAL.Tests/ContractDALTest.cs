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
    public class ContractDALTest
    {
        IContractDAL temp;
        string connection;
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ContractDALTest));
        public ContractDALTest()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [OneTimeSetUp]
        // 
        public void SetupTest()
        {
            connection = ConfigurationManager.ConnectionStrings["ShopTest"].ConnectionString;
            temp = new ContractDAL(connection);
            _log.Info("[Test] CONTRACTS set up completed");
        }
        public List<ContractDTO> GetItemsBySqlForTest()
        {
            using (SqlConnection conn = new SqlConnection(connection))
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
        public void InsertItemBySqlForTest(int productID, int providerID, int quantity)
        {
            using (SqlConnection conn = new SqlConnection(connection))
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

        public void UpdateItemBySqlForTest(int contractID, int productID, int providerID, int quantity, bool isActive)
        {
            using (SqlConnection conn = new SqlConnection(connection))
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
        public void DeleteItemBySqlForTest(int contractID)
        {
            using (SqlConnection conn = new SqlConnection(connection))
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
        [Test]
        public void GetAllContractsTest()
        {
            var random = new Random();
            ContractDTO contract_for_test = new ContractDTO
            {
                ContractID = random.Next(),
                ProductID = 9,
                ProviderID = 9,
                Quantity = 10,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };
            InsertItemBySqlForTest(contract_for_test.ProductID, contract_for_test.ProviderID, contract_for_test.Quantity);
            var contracts = temp.GetAllContracts();
            Assert.AreEqual(contracts[contracts.Count() - 1].ProductID, contract_for_test.ProductID);
            DeleteItemBySqlForTest(contracts[contracts.Count() - 1].ContractID);
            _log.Info(contracts[contracts.Count() - 1].ProductID == contract_for_test.ProductID ? "PASS GetAllContractsTest()" : "FAIL GetAllContractsTest()");

        }

        [Test]
        public void UpdateContractTest()
        {
            var contracts = GetItemsBySqlForTest();

            DateTime expected = contracts[contracts.Count() - 1].RowUpdateTime;
            temp.UpdateContract(contracts[contracts.Count() - 1].ContractID, contracts[contracts.Count() - 1].ProductID, contracts[contracts.Count() - 1].ProviderID, contracts[contracts.Count() - 1].Quantity, contracts[contracts.Count() - 1].IsActive);

            contracts = GetItemsBySqlForTest();
            DateTime actual = contracts[contracts.Count() - 1].RowUpdateTime;

            Assert.AreNotEqual(expected, actual);
            _log.Info(expected != actual ? "PASS UpdateContractTest()" : "FAIL UpdateContractTest()");

        }

        [Test]
        public void CreateContractTest()
        {
            var random = new Random();
            var contracts = GetItemsBySqlForTest();
            ContractDTO Contract_for_test = new ContractDTO
            {
                ContractID = random.Next(),
                ProductID = 8,
                ProviderID = 9,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            temp.CreateContract(Contract_for_test.ProductID, Contract_for_test.ProviderID, Contract_for_test.Quantity);
            contracts = GetItemsBySqlForTest();

            Assert.IsTrue(contracts.Any(m => m.ProviderID == Contract_for_test.ProviderID));
            _log.Info(contracts.Any(m => m.ProductID == Contract_for_test.ProductID) == true ? "PASS CreateContractTest()" : "FAIL CreateContractTest()");

        }

        [Test]
        public void DeleteContractTest()
        {
            var contracts = GetItemsBySqlForTest();

            ContractDTO expected_contract = contracts[contracts.Count() - 1];
            temp.DeleteContract(contracts[contracts.Count() - 1].ContractID);

            contracts = GetItemsBySqlForTest();
            ContractDTO actual_contract = contracts[contracts.Count() - 1];

            Assert.AreNotEqual(expected_contract, actual_contract);
            _log.Info(expected_contract != actual_contract ? "PASS DeleteContractTest()" : "FAIL DeleteContractTest()");

        }


        [Test]
        public void GetCreatedItemByID()
        {
            var random = new Random();
            ContractDTO contract_for_test = new ContractDTO
            {
                ContractID = random.Next(),
                ProductID = 9,
                ProviderID = 3,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            InsertItemBySqlForTest(contract_for_test.ProductID, contract_for_test.ProviderID, contract_for_test.Quantity);

            var contracts = GetItemsBySqlForTest();
            ContractDTO actual_Contract = temp.GetContractById(contracts[contracts.Count() - 1].ContractID);
            DeleteItemBySqlForTest(contracts[contracts.Count() - 1].ContractID);

            Assert.AreEqual(contract_for_test.ProviderID, actual_Contract.ProviderID);
            _log.Info(contract_for_test.ProviderID == actual_Contract.ProviderID ? "PASS GetCreatedContractByID()" : "FAIL GetCreatedContractByID()");

        }

        [Test]
        public void GetActiveContractsTest()
        {
            var random = new Random();
            var contracts = GetItemsBySqlForTest();
            ContractDTO Contract_for_test = new ContractDTO
            {
                ContractID = random.Next(),
                ProductID = 8,
                ProviderID = 9,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            InsertItemBySqlForTest(Contract_for_test.ProductID, Contract_for_test.ProviderID, Contract_for_test.Quantity);
            contracts = GetItemsBySqlForTest();
            int expected = contracts[contracts.Count() - 1].ContractID;
            UpdateItemBySqlForTest(contracts[contracts.Count() - 1].ContractID, contracts[contracts.Count() - 1].ProductID, contracts[contracts.Count() - 1].ProviderID, contracts[contracts.Count() - 1].Quantity, false);
            contracts = temp.GetActiveContracts();

            Assert.IsFalse(contracts.Any(m => m.ContractID == expected));
            _log.Info(contracts.Any(m => m.ContractID == expected) == false ? "PASS GetActiveContractsTest()" : "FAIL GetActiveContractsTest()");

            contracts = GetItemsBySqlForTest();
            DeleteItemBySqlForTest(contracts[contracts.Count() - 1].ContractID);
        }

        [Test]

        public void DeActivateContractByIdTest() {
            var random = new Random();
            var contracts = GetItemsBySqlForTest();
            ContractDTO Contract_for_test = new ContractDTO
            {
                ContractID = random.Next(),
                ProductID = 8,
                ProviderID = 9,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            InsertItemBySqlForTest(Contract_for_test.ProductID, Contract_for_test.ProviderID, Contract_for_test.Quantity);
            contracts = GetItemsBySqlForTest();
            temp.DeActivateContractById(contracts[contracts.Count() - 1].ContractID);
            contracts = GetItemsBySqlForTest();

            Assert.AreEqual(false, contracts[contracts.Count() - 1].IsActive);
            _log.Info(false == contracts[contracts.Count() - 1].IsActive ? "PASS DeActivateContractByIdTest()" : "FAIL DeActivateContractByIdTest()");

            DeleteItemBySqlForTest(contracts[contracts.Count() - 1].ContractID);
        }

    }
}
