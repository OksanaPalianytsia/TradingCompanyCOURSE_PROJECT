using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserDAL
    {
        void DeleteUser(int userID);
        void UpdateUser(int userID, string login, string email, string password, int roleID);
        void CreateUser(string login, string email, string password, int roleID);
        UserDTO GetUserByLogin(string login);
        bool Login(string login, string password);
    }
}
