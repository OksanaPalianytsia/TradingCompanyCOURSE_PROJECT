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
    public class AuthorisationManager : IAuthorisationManager
    {
        private readonly IUserDAL _userDal;

        public AuthorisationManager(IUserDAL userDal)
        {
            _userDal = userDal;
        }
        public UserDTO GetUserByLogin(string login)
        {
            return _userDal.GetUserByLogin(login);
        }

        public bool Login(string login, string password)
        {
            return _userDal.Login(login, password);
        }
    }
}
