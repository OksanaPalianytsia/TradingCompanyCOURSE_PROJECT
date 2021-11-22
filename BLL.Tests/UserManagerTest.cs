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
    public class UserManagerTest
    {
        private Mock<IProductDAL> productDAL;
        private UserManager user;

        [SetUp]
        public void SetUp()
        {
            productDAL = new Mock<IProductDAL>(MockBehavior.Strict);
            user = new UserManager(productDAL.Object);
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

            var res = user.ShowAllProducts();
            Assert.IsNotNull(res);
            Assert.AreEqual(expected_prod[0], res[0]);
        }
    }
}
