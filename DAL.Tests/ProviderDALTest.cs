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
    [TestFixture]
    public class ProviderDALTest
    {
        IProviderDAL temp;
        string connection;
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ProviderDALTest));

        public ProviderDALTest()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [OneTimeSetUp]
        // 
        public void SetupTest()
        {
            connection = ConfigurationManager.ConnectionStrings["ShopTest"].ConnectionString;
            temp = new ProviderDAL(connection);
            _log.Info("[Test] PROVIDERS set up completed");
        }
        public List<ProviderDTO> GetItemsBySqlForTest()
        {
            using (SqlConnection conn = new SqlConnection(connection))
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
        public void InsertItemBySqlForTest(string name)
        {
            using (SqlConnection conn = new SqlConnection(connection))
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
        public void DeleteItemBySqlForTest(int providerId)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("delete_provider", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@pProviderID", providerId);

                int rowsDeleted = comm.ExecuteNonQuery();

                conn.Close();
            }
        }

        [Test]
        public void GetAllProvidersTest()
        {
            var random = new Random();
            ProviderDTO prov_for_test = new ProviderDTO
            {
                ProviderID = random.Next(),
                Name = "Orest",
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };
            InsertItemBySqlForTest(prov_for_test.Name);
            var providers = temp.GetAllProviders();
            Assert.AreEqual(providers[providers.Count() - 1].Name, prov_for_test.Name);
            DeleteItemBySqlForTest(providers[providers.Count() - 1].ProviderID);
            _log.Info(providers[providers.Count() - 1].Name == prov_for_test.Name ? "PASS GetAllProvidersTest()" : "FAIL GetAllProvidersTest()");

        }
        [Test]
        public void UpdateProviderTest()
        {
            var providers = GetItemsBySqlForTest();

            DateTime expected = providers[providers.Count() - 1].RowUpdateTime;
            temp.UpdateProvider(providers[providers.Count() - 1].ProviderID, providers[providers.Count() - 1].Name);


            providers = GetItemsBySqlForTest();
            DateTime actual = providers[providers.Count() - 1].RowUpdateTime;


            Assert.AreNotEqual(expected, actual);
            _log.Info(expected != actual ? "PASS UpdateProviderTest()" : "FAIL UpdateProviderTest()");

        }

        [Test]
        public void CreateProviderTest()
        {
            var random = new Random();
            var providers = GetItemsBySqlForTest();
            ProviderDTO prov_for_test = new ProviderDTO
            {
                ProviderID = random.Next(),
                Name = "Oleksandr",
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            temp.CreateProvider(prov_for_test.Name);
            providers = GetItemsBySqlForTest();

            Assert.IsTrue(providers.Any(m => m.Name == prov_for_test.Name));
            _log.Info(providers.Any(m => m.Name == prov_for_test.Name) == true ? "PASS CreateProviderTest()" : "FAIL CreateProviderTest()");

        }


        [Test]
        public void DeleteProviderTest()
        {
            var providers = GetItemsBySqlForTest();

            ProviderDTO expected_provider = providers[providers.Count() - 1];
            temp.DeleteProvider(providers[providers.Count() - 1].ProviderID);

            providers = GetItemsBySqlForTest();
            ProviderDTO actual_provider = providers[providers.Count() - 1];
            
            Assert.AreNotEqual(expected_provider, actual_provider);
            _log.Info(expected_provider != actual_provider ? "PASS DeleteProviderTest()" : "FAIL DeleteProviderTest()");

        }


        [Test]
        public void GetCreatedItemByID()
        {
            var random = new Random();
            ProviderDTO prov_for_test = new ProviderDTO
            {

                ProviderID = random.Next(),
                Name = "Khrystyna",
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            InsertItemBySqlForTest(prov_for_test.Name);

            var products = GetItemsBySqlForTest();
            ProviderDTO actual_provider = temp.GetProviderById(products[products.Count() - 1].ProviderID);
            DeleteItemBySqlForTest(products[products.Count() - 1].ProviderID);

            Assert.AreEqual(prov_for_test.Name, actual_provider.Name);
            _log.Info(prov_for_test.Name == actual_provider.Name ? "PASS GetCreatedProviderByID()" : "FAIL GetCreatedProviderByID()");

        }
    }
}
