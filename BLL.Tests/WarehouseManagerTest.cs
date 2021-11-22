using BLL.Concrete;
using DAL.Interfaces;
using DTO;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Tests
{
    [TestFixture]
    public class WarehouseManagerTest
    {
        private Mock<IProductDAL> productDAL;
        private Mock<IOrderDAL> orderDAL;
        private Mock<IContractDAL> contractDAL;
        private Mock<IProviderDAL> providerDAL;

        private WarehouseManager manager;

        [SetUp]
        public void SetUp()
        {
            productDAL = new Mock<IProductDAL>(MockBehavior.Strict);
            orderDAL = new Mock<IOrderDAL>(MockBehavior.Strict);
            contractDAL = new Mock<IContractDAL>(MockBehavior.Strict);
            providerDAL = new Mock<IProviderDAL>(MockBehavior.Strict);

            manager = new WarehouseManager(productDAL.Object, orderDAL.Object, contractDAL.Object, providerDAL.Object);
        }


        [Test]
        public void UpdateOrderTest()
        {
            OrderDTO inorder = new OrderDTO
            {
                OrderID = 13,
                ProductID = 8,
                Quantity = 100,
                IsActive = true
            };

            orderDAL.Setup(d => d.UpdateOrder(inorder.OrderID, inorder.ProductID, inorder.Quantity, inorder.IsActive)).Verifiable();
        }


        [Test]
        public void DeactivateOrderByIdTest()
        {
            int orderId = 1;
            orderDAL.Setup(d => d.DeActivateOrderById(orderId)).Verifiable();
        }


        [Test]
        public void CreateContractTest()
        {
            ContractDTO incontract = new ContractDTO
            {
                ProductID = 1,
                ProviderID = 100,
                Quantity = 10
            };

            contractDAL.Setup(d => d.CreateContract(incontract.ProductID, incontract.ProviderID, incontract.Quantity)).Verifiable();
        }


        [Test]
        public void DeactivateContractByIdTest()
        {
            int contractID = 1;
            contractDAL.Setup(d => d.DeActivateContractById(contractID)).Verifiable();
        }


        [Test]
        public void ShowAllProductsTest()
        {
            List<ProductDTO> expected_prod = new List<ProductDTO>();
            expected_prod.Add(new ProductDTO
            {
                ProductID = 1,
                Name = "Product1",
                CategoryID = 2,
                Price = 100,
                Quantity = 15
            });
            expected_prod.Add(new ProductDTO
            {
                ProductID = 2,
                Name = "Product2",
                CategoryID = 2,
                Price = 100,
                Quantity = 20
            });

            productDAL.Setup(d => d.GetAllProducts()).Returns(expected_prod);

            var res = manager.ShowAllProducts();
            Assert.IsNotNull(res);
            Assert.AreEqual(expected_prod[0], res[0]);
        }


        [Test]
        public void ShowAllProviderTest()
        {
            List<ProviderDTO> expected_prov = new List<ProviderDTO>();
            expected_prov.Add(new ProviderDTO
            {
                ProviderID = 1,
                Name = "Provider1"
            });
            expected_prov.Add(new ProviderDTO
            {
                ProviderID = 2,
                Name = "Provider2"
            });

            providerDAL.Setup(d => d.GetAllProviders()).Returns(expected_prov);

            var res = manager.ShowAllProviders();
            Assert.IsNotNull(res);
            Assert.AreEqual(expected_prov[0], res[0]);
        }


        [Test]
        public void ShowActiveOrdersTest()
        {
            List<OrderDTO> expected_order = new List<OrderDTO>();
            expected_order.Add(new OrderDTO
            {
                OrderID = 1,
                ProductID = 1,
                Quantity = 10
            });
            expected_order.Add(new OrderDTO
            {
                OrderID = 2,
                ProductID = 2,
                Quantity = 20
            });

            orderDAL.Setup(d => d.GetActiveOrders()).Returns(expected_order);

            var res = manager.ShowActiveOrders();
            Assert.IsNotNull(res);
            Assert.AreEqual(expected_order[0], res[0]);
        }


        [Test]
        public void ShowActiveContractsTest()
        {
            List<ContractDTO> expected_contract = new List<ContractDTO>();
            expected_contract.Add(new ContractDTO
            {
                ContractID = 1,
                ProductID = 1,
                ProviderID = 1,
                Quantity = 10
            });
            expected_contract.Add(new ContractDTO
            {
                ContractID = 2,
                ProductID = 2,
                ProviderID = 2,
                Quantity = 20
            });

            contractDAL.Setup(d => d.GetActiveContracts()).Returns(expected_contract);

            var res = manager.ShowActiveContracts();
            Assert.IsNotNull(res);
            Assert.AreEqual(expected_contract[0], res[0]);
        }


        [Test]
        public void SortProductsNameTest()
        {
            List<ProductDTO> expected_prod = new List<ProductDTO>();
            expected_prod.Add(new ProductDTO
            {
                ProductID = 1,
                Name = "Product1",
                CategoryID = 2,
                Price = 100,
                Quantity = 15
            });
            expected_prod.Add(new ProductDTO
            {
                ProductID = 2,
                Name = "Product2",
                CategoryID = 2,
                Price = 100,
                Quantity = 20
            });

            productDAL.Setup(d => d.SortProductsByName()).Returns(expected_prod);

            var res = manager.SortProductsByName();
            Assert.IsNotNull(res);
            Assert.AreEqual(expected_prod[0], res[0]);
        }


        [Test]
        public void SortProductsCategoryTest()
        {
            List<ProductDTO> expected_prod = new List<ProductDTO>();
            expected_prod.Add(new ProductDTO
            {
                ProductID = 1,
                Name = "Product1",
                CategoryID = 2,
                Price = 100,
                Quantity = 15
            });
            expected_prod.Add(new ProductDTO
            {
                ProductID = 2,
                Name = "Product2",
                CategoryID = 2,
                Price = 100,
                Quantity = 20
            });

            productDAL.Setup(d => d.SortProductsByCategory()).Returns(expected_prod);

            var res = manager.SortProductsByCategory();
            Assert.IsNotNull(res);
            Assert.AreEqual(expected_prod[0], res[0]);
        }


        [Test]
        public void SortProductsQuantityTest()
        {
            List<ProductDTO> expected_prod = new List<ProductDTO>();
            expected_prod.Add(new ProductDTO
            {
                ProductID = 1,
                Name = "Product1",
                CategoryID = 2,
                Price = 100,
                Quantity = 15
            });
            expected_prod.Add(new ProductDTO
            {
                ProductID = 2,
                Name = "Product2",
                CategoryID = 2,
                Price = 100,
                Quantity = 20
            });

            productDAL.Setup(d => d.SortProductsByQuantity()).Returns(expected_prod);

            var res = manager.SortProductsByQuantity();
            Assert.IsNotNull(res);
            Assert.AreEqual(expected_prod[0], res[0]);
        }


        [Test]
        public void SortProductsUpdateDateTest()
        {
            List<ProductDTO> expected_prod = new List<ProductDTO>();
            expected_prod.Add(new ProductDTO
            {
                ProductID = 1,
                Name = "Product1",
                CategoryID = 2,
                Price = 100,
                Quantity = 15
            });
            expected_prod.Add(new ProductDTO
            {
                ProductID = 2,
                Name = "Product2",
                CategoryID = 2,
                Price = 100,
                Quantity = 20
            });

            productDAL.Setup(d => d.SortProductsByUpdateDate()).Returns(expected_prod);

            var res = manager.SortProductsByUpdateDate();
            Assert.IsNotNull(res);
            Assert.AreEqual(expected_prod[0], res[0]);
        }


        [Test]
        public void FindProductsByNameTest()
        {
            string name = "";
            List<ProductDTO> expected_prod = new List<ProductDTO>();
            expected_prod.Add(new ProductDTO
            {
                ProductID = 1,
                Name = "Product1",
                CategoryID = 2,
                Price = 100,
                Quantity = 15
            });
            expected_prod.Add(new ProductDTO
            {
                ProductID = 2,
                Name = "Product2",
                CategoryID = 2,
                Price = 100,
                Quantity = 20
            });

            productDAL.Setup(d => d.GetProductsByName(name)).Returns(expected_prod);

            var res = manager.FindProductsByName(name);
            Assert.IsNotNull(res);
            Assert.AreEqual(expected_prod[0], res[0]);
        }


        [Test]
        public void FindProductsByCategoriesTest()
        {
            int category = 2;
            List<ProductDTO> expected_prod = new List<ProductDTO>();
            expected_prod.Add(new ProductDTO
            {
                ProductID = 1,
                Name = "Product1",
                CategoryID = 2,
                Price = 100,
                Quantity = 15
            });
            expected_prod.Add(new ProductDTO
            {
                ProductID = 2,
                Name = "Product2",
                CategoryID = 2,
                Price = 100,
                Quantity = 20
            });

            productDAL.Setup(d => d.GetProductsByCategories(category)).Returns(expected_prod);

            var res = manager.FindProductsByCategories(category);
            Assert.IsNotNull(res);
            Assert.AreEqual(expected_prod[0], res[0]);
        }


        [Test]
        public void FindProductsByPriceTest()
        {
            int price = 100;
            List<ProductDTO> expected_prod = new List<ProductDTO>();
            expected_prod.Add(new ProductDTO
            {
                ProductID = 1,
                Name = "Product1",
                CategoryID = 2,
                Price = 100,
                Quantity = 15
            });
            expected_prod.Add(new ProductDTO
            {
                ProductID = 2,
                Name = "Product2",
                CategoryID = 2,
                Price = 100,
                Quantity = 20
            });

            productDAL.Setup(d => d.GetProductsByPrice(price)).Returns(expected_prod);

            var res = manager.FindProductsByPrice(price);
            Assert.IsNotNull(res);
            Assert.AreEqual(expected_prod[0], res[0]);
        }
    }
}
