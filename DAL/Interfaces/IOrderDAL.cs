using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IOrderDAL
    {
        void DeleteOrder(int orderID);
        void UpdateOrder(int orderID, int productID, int quantity, bool isActive);
        void CreateOrder(int productID, int quantity);
        List<OrderDTO> GetAllOrders();
        List<OrderDTO> GetActiveOrders();
        OrderDTO GetOrderById(int itemId);
        void DeActivateOrderById(int orderId);
    }
}
