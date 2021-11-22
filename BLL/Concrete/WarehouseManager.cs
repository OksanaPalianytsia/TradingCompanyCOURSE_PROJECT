using BLL.Interfaces;
using DAL.Interfaces;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Concrete
{
    public class WarehouseManager : IWarehouseManager
    {
        private readonly IProductDAL _productDAL;
        private readonly IOrderDAL _orderDAL;
        private readonly IContractDAL _contractDAL;
        private readonly IProviderDAL _providerDAL;
        public WarehouseManager(IProductDAL productDAL, IOrderDAL orderDAL, IContractDAL contractDAL, IProviderDAL providerDAL)
        {
            _productDAL = productDAL;
            _orderDAL = orderDAL;
            _contractDAL = contractDAL;
            _providerDAL = providerDAL;
        }
        public void CreateContract(int productID, int providerID, int quantity, bool isActive)
        {
            _contractDAL.CreateContract(productID, providerID, quantity);
        }

        public void DeactivateContractById(int contractId)
        {
            ContractDTO contr = _contractDAL.GetContractById(contractId);
            _productDAL.AddProductSupplies(contr.ProductID, contr.Quantity);
            _contractDAL.DeActivateContractById(contractId);
        }

        public void DeactivateOrderById(int orderId)
        {
            OrderDTO or = _orderDAL.GetOrderById(orderId);
            _productDAL.ExcludeProductSupplies(or.ProductID, or.Quantity);
            _orderDAL.DeActivateOrderById(orderId);
        }

        public List<ContractDTO> ShowActiveContracts()
        {
            return _contractDAL.GetActiveContracts();
        }

        public List<OrderDTO> ShowActiveOrders()
        {
            return _orderDAL.GetActiveOrders();
        }

        public List<ProductDTO> ShowAllProducts()
        {
            return _productDAL.GetAllProducts();
        }

        public List<ProviderDTO> ShowAllProviders()
        {
            return _providerDAL.GetAllProviders();
        }

        public List<ProductDTO> FindProductsByCategories(int categoryId)
        {
            return _productDAL.GetProductsByCategories(categoryId);
        }

        public List<ProductDTO> FindProductsByName(string name)
        {
            return _productDAL.GetProductsByName(name);
        }

        public List<ProductDTO> FindProductsByPrice(int price)
        {
            return _productDAL.GetProductsByPrice(price);
        }

        public List<ProductDTO> SortProductsByCategory()
        {
            return _productDAL.SortProductsByCategory();
        }

        public List<ProductDTO> SortProductsByName()
        {
            return _productDAL.SortProductsByName();
        }

        public List<ProductDTO> SortProductsByQuantity()
        {
            return _productDAL.SortProductsByQuantity();
        }

        public List<ProductDTO> SortProductsByUpdateDate()
        {
            return _productDAL.SortProductsByUpdateDate();
        }

        public void UpdateOrder(int orderID, int productID, int quantity, bool isActive)
        {
            _orderDAL.UpdateOrder(orderID, productID, quantity, isActive);
        }
    }
}
