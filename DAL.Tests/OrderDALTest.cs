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
    public class OrderDALTest
    {
        IOrderDAL temp;
        string connection;
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(OrderDALTest));
        public OrderDALTest()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [OneTimeSetUp]
        // 
        public void SetupTest()
        {
            connection = ConfigurationManager.ConnectionStrings["ShopTest"].ConnectionString;
            temp = new OrderDAL(connection);
            _log.Info("[Test] ORDERS set up completed");
        }

        public List<OrderDTO> GetItemsBySqlForTest()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_orders", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = comm.ExecuteReader();

                var orders = new List<OrderDTO>();

                while (reader.Read())
                {
                    orders.Add(new OrderDTO
                    {
                        OrderID = (int)reader["OrderID"],
                        ProductID = (int)reader["ProductID"],
                        Quantity = (int)reader["Quantity"],
                        IsActive = (bool)reader["IsActive"],
                        RowInsertTime = DateTime.Parse(reader["RowInsertTime"].ToString()),
                        RowUpdateTime = DateTime.Parse(reader["RowUpdateTime"].ToString())
                    });
                }
                conn.Close();
                return orders;
            }
        }
        public void InsertItemBySqlForTest(int productID, int quantity)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("insert_order", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@oProductID", productID);
                comm.Parameters.AddWithValue("@oQuantity", quantity);
                comm.Parameters.AddWithValue("@oIsActive", true);

                int rowsAffected = comm.ExecuteNonQuery();

                conn.Close();
            }
        }
        public void DeleteItemBySqlForTest(int orderID)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("delete_order", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@oOrderId", orderID);

                int rowsDeleted = comm.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void UpdateItemBySqlForTest(int orderID, int productID, int quantity, bool isActive)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("update_oder", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@oOrderID", orderID);
                comm.Parameters.AddWithValue("@oProductID", productID);
                comm.Parameters.AddWithValue("@oQuantity", quantity);
                comm.Parameters.AddWithValue("@oIsActive", isActive);

                int rowUpdated = comm.ExecuteNonQuery();

                conn.Close();
            }
        }
        [Test]
        public void GetAllOrdersTest()
        {
            var random = new Random();
            OrderDTO order_for_test = new OrderDTO
            {
                OrderID = random.Next(),
                ProductID = 8,
                Quantity = 10,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };
            InsertItemBySqlForTest(order_for_test.ProductID, order_for_test.Quantity);
            var orders = temp.GetAllOrders();
            Assert.AreEqual(orders[orders.Count() - 1].ProductID, order_for_test.ProductID);
            DeleteItemBySqlForTest(orders[orders.Count() - 1].OrderID);
            _log.Info(orders[orders.Count() - 1].ProductID == order_for_test.ProductID ? "PASS GetAllOrdersTest()" : "FAIL GetAllOrdersTest()");

        }

        [Test]
        public void UpdateOrderTest()
        {
            var orders = GetItemsBySqlForTest();

            DateTime expected = orders[orders.Count() - 1].RowUpdateTime;
            temp.UpdateOrder(orders[orders.Count() - 1].OrderID, orders[orders.Count() - 1].ProductID, orders[orders.Count() - 1].Quantity, orders[orders.Count() - 1].IsActive);


            orders = GetItemsBySqlForTest();
            DateTime actual = orders[orders.Count() - 1].RowUpdateTime;

            Assert.AreNotEqual(expected, actual);
            _log.Info(expected != actual ? "PASS UpdateOrderTest()" : "FAIL UpdateOrderTest()");

        }

        [Test]
        public void CreateOrderTest()
        {
            var random = new Random();
            var orders = GetItemsBySqlForTest();
            OrderDTO order_for_test = new OrderDTO
            {
                OrderID = random.Next(),
                ProductID = 29,
                Quantity = 2,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };


            temp.CreateOrder(order_for_test.ProductID, order_for_test.Quantity);
            orders = GetItemsBySqlForTest();

            Assert.IsTrue(orders.Any(m => m.ProductID == order_for_test.ProductID));
            _log.Info(orders.Any(m => m.ProductID == order_for_test.ProductID) == true ? "PASS CreateOrderTest()" : "FAIL CreateOrderTest()");

        }

        [Test]
        public void DeleteOrderTest()
        {
            var orders = GetItemsBySqlForTest();

            OrderDTO expected_order = orders[orders.Count() - 1];
            temp.DeleteOrder(orders[orders.Count() - 1].OrderID);

            orders = GetItemsBySqlForTest();
            OrderDTO actual_order = orders[orders.Count() - 1];

            Assert.AreNotEqual(expected_order, actual_order);
            _log.Info(expected_order != actual_order ? "PASS DeleteOrderTest()" : "FAIL DeleteOrderTest()");

        }


        [Test]
        public void GetCreatedItemByID()
        {
            var random = new Random();
            OrderDTO order_for_test = new OrderDTO
            {
                OrderID = random.Next(),
                ProductID = 29,
                Quantity = 2,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            InsertItemBySqlForTest(order_for_test.ProductID, order_for_test.Quantity);

            var orders = GetItemsBySqlForTest();
            OrderDTO actual_order = temp.GetOrderById(orders[orders.Count() - 1].OrderID);
            DeleteItemBySqlForTest(orders[orders.Count() - 1].OrderID);

            Assert.AreEqual(order_for_test.ProductID, actual_order.ProductID);
            _log.Info(order_for_test.ProductID == actual_order.ProductID ? "PASS GetCreatedOrderByID()" : "FAIL GetCreatedOrderByID()");

        }

        [Test]

        public void GetActiveOrdersTest()
        {
            var random = new Random();
            var orders = GetItemsBySqlForTest();
            OrderDTO order_for_test = new OrderDTO
            {
                OrderID = random.Next(),
                ProductID = 29,
                Quantity = 2,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            InsertItemBySqlForTest(order_for_test.ProductID, order_for_test.Quantity);
            orders = GetItemsBySqlForTest();
            int expected = orders[orders.Count() - 1].OrderID;
            UpdateItemBySqlForTest(orders[orders.Count() - 1].OrderID, orders[orders.Count() - 1].ProductID, orders[orders.Count() - 1].Quantity, false);
            orders = temp.GetActiveOrders();

            Assert.IsFalse(orders.Any(m => m.OrderID == expected));
            _log.Info(orders.Any(m => m.OrderID == expected) == false ? "PASS GetActiveOrdersTest()" : "FAIL GetActiveOrdersTest()");

            orders = GetItemsBySqlForTest();
            DeleteItemBySqlForTest(orders[orders.Count() - 1].OrderID);
        }

        [Test]

        public void DeActivateOrderByIdTest()
        {
            var random = new Random();
            var orders = GetItemsBySqlForTest();
            OrderDTO order_for_test = new OrderDTO
            {
                OrderID = random.Next(),
                ProductID = 29,
                Quantity = 2,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            InsertItemBySqlForTest(order_for_test.ProductID, order_for_test.Quantity);
            orders = GetItemsBySqlForTest();
            temp.DeActivateOrderById(orders[orders.Count() - 1].OrderID);
            orders = GetItemsBySqlForTest();

            Assert.AreEqual(false, orders[orders.Count() - 1].IsActive);
            _log.Info(false == orders[orders.Count() - 1].IsActive ? "PASS DeActivateOrderByIdTest()" : "FAIL DeActivateOrderByIdTest()");

            DeleteItemBySqlForTest(orders[orders.Count() - 1].OrderID);
        }
    }
}
