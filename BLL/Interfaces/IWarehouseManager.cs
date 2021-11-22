using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IWarehouseManager
    {
        List<ProductDTO> SortProductsByName();
        List<ProductDTO> SortProductsByCategory();
        List<ProductDTO> SortProductsByQuantity();
        List<ProductDTO> SortProductsByUpdateDate();
        List<ProductDTO> ShowAllProducts();
        List<ProductDTO> FindProductsByName(string name);
        List<ProductDTO> FindProductsByCategories(int categoryId);
        List<ProductDTO> FindProductsByPrice(int price);

        List<ProviderDTO> ShowAllProviders();

        List<OrderDTO> ShowActiveOrders();
        void UpdateOrder(int orderID, int productID, int quantity, bool isActive);
        void DeactivateOrderById(int orderId);

        List<ContractDTO> ShowActiveContracts();
        void CreateContract(int productID, int providerID, int quantity, bool isActive);
        void DeactivateContractById(int contractId);
    }
}
