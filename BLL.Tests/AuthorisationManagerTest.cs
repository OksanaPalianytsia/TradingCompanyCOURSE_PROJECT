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
    public class AuthorizationManagerTests
    {
        private Mock<IUserDAL> userDAL;

        private AuthorisationManager auth_m;

        [SetUp]
        public void SetUp()
        {
            userDAL = new Mock<IUserDAL>(MockBehavior.Strict);
            auth_m = new AuthorisationManager(userDAL.Object);
        }

        [Test]
        public void GetUserByLoginTest()
        {
            Guid salt = Guid.NewGuid();
            string password = "password";
            UserDTO inuser = new UserDTO
            {
                UserID = 13,
                Login = "login",
                Email = "...@gmail.com",
                Password = password,
                Salt = salt
            };
            bool outuser = true;
            userDAL.Setup(d => d.Login(inuser.Login, password)).Returns(outuser);

            var res = auth_m.Login(inuser.Login, password);
            Assert.IsNotNull(res);
            Assert.AreEqual(outuser, res);
        }
    }
}
