using DAL.ADO;
using DAL.Interfaces;
using DTO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Tests
{
    [TestFixture]
    public class UserDALTest
    {
        IUserDAL temp;
        string connection;
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(UserDALTest));
        public UserDALTest()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [OneTimeSetUp] 
        public void SetupTest()
        {
            connection = ConfigurationManager.ConnectionStrings["ShopTest"].ConnectionString;
            temp = new UserDAL(connection);
            _log.Info("[Test] USERS set up completed");
        }

        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public List<UserDTO> GetItemsBySqlForTest()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_users", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = comm.ExecuteReader();

                var users = new List<UserDTO>();

                while (reader.Read())
                {
                    users.Add(new UserDTO
                    {
                        UserID = (int)reader["UserID"],
                        Login = reader["Login"].ToString(),
                        Email = reader["Email"].ToString(),
                        // user.Password = Encoding.UTF8.GetBytes(reader["Password"].ToString()),            
                        // Password = Encoding.UTF8.GetString(Convert.FromBase64String(Base64EncodeObject(reader["Password"]))),
                        Password = reader["Password"].ToString(),
                        Salt = Guid.Parse(reader["Salt"].ToString()),
                        RoleID = (int)reader["RoleID"],
                        RowInsertTime = DateTime.Parse(reader["RowInsertTime"].ToString()),
                        RowUpdateTime = DateTime.Parse(reader["RowUpdateTime"].ToString())
                    });
                }
                conn.Close();
                return users;
            }
        }
        public void InsertItemBySqlForTest(string login, string email, string password, int roleID)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("insert_user", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                Guid salt = Guid.NewGuid();
                comm.Parameters.AddWithValue("@uLogin", login);
                comm.Parameters.AddWithValue("@uEmail", email);
                comm.Parameters.AddWithValue("@uPassword", GetHashString(password + salt.ToString()));
                comm.Parameters.AddWithValue("@uSalt", salt);
                comm.Parameters.AddWithValue("@uRoleID", roleID);

                int rowsAffected = comm.ExecuteNonQuery();

                conn.Close();
            }
        }
        public void DeleteItemBySqlForTest(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("delete_user", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@uUserID", userId);
                int rowsDeleted = comm.ExecuteNonQuery();

                conn.Close();
            }
        }
        public bool Login(string login, string password)
        {
            UserDTO user_end = new UserDTO();
            user_end = temp.GetUserByLogin(login);
            return user_end.Login != null && user_end.Password == GetHashString(password + user_end.Salt.ToString());
        }


        [Test]
        public void UpdateUserTest()
        {
            var users = GetItemsBySqlForTest();

            DateTime expected = users[users.Count() - 1].RowUpdateTime;
            temp.UpdateUser(users[users.Count() - 1].UserID, users[users.Count() - 1].Login, users[users.Count() - 1].Email, "admin", users[users.Count() - 1].RoleID);


            users = GetItemsBySqlForTest();
            DateTime actual = users[users.Count() - 1].RowUpdateTime;

            Assert.AreNotEqual(expected, actual);
            _log.Info(expected != actual ? "PASS UpdateUserTest()" : "FAIL UpdateUserTest()");

        }

        [Test]
        public void CreateUserTest()
        {
            var random = new Random();
            var users = GetItemsBySqlForTest();
            Guid salt = Guid.NewGuid();
            string password = "test";
            UserDTO user_for_test = new UserDTO
            {
                UserID = random.Next(),
                Login = "User_1",
                Email = "test@icloud.com",
                Password = GetHashString(password + salt.ToString()),
                Salt = salt,
                RoleID = 1,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };

            temp.CreateUser(user_for_test.Login, user_for_test.Email, password, user_for_test.RoleID);
            users = GetItemsBySqlForTest();

            Assert.IsTrue(users.Any(m => m.Login == user_for_test.Login));
            _log.Info(users.Any(m => m.Login == user_for_test.Login) == true ? "PASS CreateUserTest()" : "FAIL CreateUserTest()");

        }

        [Test]
        public void DeleteUserTest()
        {
            var users = GetItemsBySqlForTest();

            UserDTO expected_user = users[users.Count() - 1];
            temp.DeleteUser(users[users.Count() - 1].UserID);

            users = GetItemsBySqlForTest();
            UserDTO actual_user = users[users.Count() - 1];

            Assert.AreNotEqual(expected_user, actual_user);
            _log.Info(expected_user != actual_user ? "PASS DeleteUserTest()" : "FAIL DeleteUserTest()");

        }

        [Test]
        public void GetUserByLoginTest()
        {
            var random = new Random();

            Guid salt = Guid.NewGuid();
            string password = "test";
            UserDTO user_for_test = new UserDTO
            {
                UserID = random.Next(),
                Login = "User_Test",
                Email = "test@icloud.com",
                Password = GetHashString(password + salt.ToString()),
                Salt = salt,
                RoleID = 1,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };


            InsertItemBySqlForTest(user_for_test.Login, user_for_test.Email, password, user_for_test.RoleID);

            var users = GetItemsBySqlForTest();

            UserDTO actual_user = temp.GetUserByLogin(users[users.Count() - 1].Login);

            DeleteItemBySqlForTest(users[users.Count() - 1].UserID);
            Assert.AreEqual(user_for_test.Login, actual_user.Login);
            _log.Info(user_for_test.Login == actual_user.Login ? "PASS GetUserByLoginTest()" : "FAIL GetUserByLoginTest()");

        }

        [Test]
        public void LoginTest()
        {
            var random = new Random();

            Guid salt = Guid.NewGuid();
            string password = "test";
            UserDTO user_for_test = new UserDTO
            {
                UserID = random.Next(),
                Login = "User_Test",
                Email = "test@icloud.com",
                Password = GetHashString(password + salt.ToString()),
                Salt = salt,
                RoleID = 1,
                RowInsertTime = DateTime.UtcNow,
                RowUpdateTime = DateTime.UtcNow
            };


            InsertItemBySqlForTest(user_for_test.Login, user_for_test.Email, password, user_for_test.RoleID);
            var users = GetItemsBySqlForTest();

            Assert.IsTrue(temp.Login("User_Test", "test"));
            Assert.IsFalse(temp.Login("User_Test", "test123"));
            _log.Info(temp.Login("User_Test", "test") == true && temp.Login("User_Test", "test123") == false ? "PASS LoginTest()" : "FAIL LoginTest()");

            DeleteItemBySqlForTest(users[users.Count() - 1].UserID);
        }
    }
}
