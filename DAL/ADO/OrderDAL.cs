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
    public class OrderDAL : IOrderDAL
    {
        private string _connStr;
        public OrderDAL(string connStr)
        {
            this._connStr = connStr;
        }
        public void CreateOrder(int productID, int quantity)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
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

        public void DeActivateOrderById(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("deactivate_order_by_id", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@oOrderID", orderId);

                int rowUpdated = comm.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void DeleteOrder(int orderID)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
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

        public List<OrderDTO> GetActiveOrders()
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_active_orders", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = comm.ExecuteReader();

                var active_orders = new List<OrderDTO>();

                while (reader.Read())
                {
                    active_orders.Add(new OrderDTO
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
                return active_orders;
            }
        }

        public List<OrderDTO> GetAllOrders()
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
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

        public OrderDTO GetOrderById(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_by_id_oder", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@oOrderID", orderId);

                SqlDataReader reader = comm.ExecuteReader();
                var order = new OrderDTO();
                while (reader.Read())
                {
                    order.OrderID = (int)reader["OrderID"];
                    order.ProductID = (int)reader["ProductID"];
                    order.Quantity = (int)reader["Quantity"];
                    order.IsActive = (bool)reader["IsActive"];
                    order.RowInsertTime = DateTime.Parse(reader["RowInsertTime"].ToString());
                    order.RowUpdateTime = DateTime.Parse(reader["RowUpdateTime"].ToString());
                }

                conn.Close();
                return order;
            }
        }

        public void UpdateOrder(int orderID, int productID, int quantity, bool isActive)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
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
    }
}
