using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DAL.Models;
using DAL.Repositories;
using WebSpeakAngular.Models;

namespace WebSpeakAngular
{
    public class AuthHelper
    {

        public bool IsUser(LoginModel user)
        {
            if (user == null)
            {
                return false;
            }

            List<Users> allUsers = new UsersRepository().GetAll().ToList();
            List<string> userNames = allUsers.Select(u => u.UserName).ToList();
            List<string> emails = allUsers.Select(u => u.Email).ToList();
            List<string> passwords = allUsers.Select(u => u.PasswordHash).ToList();

            bool isLogin = userNames.Contains(user.Login) || emails.Contains(user.Login);
            string passwordHash = ComputeSha256Hash(user.Password);
            bool isPassword = passwords.Contains(passwordHash);

            return isLogin && isPassword;
        }

        public string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                foreach (var elem in bytes)
                {
                    builder.Append(elem.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}