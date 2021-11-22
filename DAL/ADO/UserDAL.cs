using DAL.Interfaces;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ADO
{
    public class UserDAL : IUserDAL
    {
        private string _connStr;

        public UserDAL(string connstr)
        {
            this._connStr = connstr;
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
        public void CreateUser(string login, string email, string password, int roleID)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
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

        public void DeleteUser(int userID)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("delete_user", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                Guid salt = Guid.NewGuid();
                comm.Parameters.AddWithValue("@uUserID", userID);
                int rowsDeleted = comm.ExecuteNonQuery();

                conn.Close();
            }
        }

        public UserDTO GetUserByLogin(string login)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("get_by_login_user", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@uLogin", login);

                SqlDataReader reader = comm.ExecuteReader();
                var user = new UserDTO();
                while (reader.Read())
                {
                    user.UserID = (int)reader["UserID"];
                    user.Login = reader["Login"].ToString();
                    user.Email = reader["Email"].ToString();
                    // user.Password = Encoding.UTF8.GetBytes(reader["Password"].ToString());
                    user.Password = reader["Password"].ToString();
                    user.Salt = Guid.Parse(reader["Salt"].ToString());
                    user.RoleID = (int)reader["RoleID"];
                    user.RowInsertTime = DateTime.Parse(reader["RowInsertTime"].ToString());
                    user.RowUpdateTime = DateTime.Parse(reader["RowUpdateTime"].ToString());
                }
                conn.Close();
                return user;
            }
        }


        public void UpdateUser(int userID, string login, string email, string password, int roleID)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("update_user", conn);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Clear();
                Guid salt = Guid.NewGuid();
                comm.Parameters.AddWithValue("@uUserID", userID);
                comm.Parameters.AddWithValue("@uLogin", login);
                comm.Parameters.AddWithValue("@uEmail", email);
                comm.Parameters.AddWithValue("@uPassword", GetHashString(password + salt.ToString()));
                comm.Parameters.AddWithValue("@uSalt", salt);
                comm.Parameters.AddWithValue("@uRoleID", roleID);

                int rowUpdated = comm.ExecuteNonQuery();
                conn.Close();
            }
        }

        public bool Login(string login, string password)
        {
            UserDTO user_end = new UserDTO();
            user_end = GetUserByLogin(login);
            return user_end.Login != null && user_end.Password == GetHashString(password + user_end.Salt.ToString());
        }

    }
}
