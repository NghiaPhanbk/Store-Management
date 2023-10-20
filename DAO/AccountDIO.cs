using Quanlyquancafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Quanlyquancafe.DAO
{
    public class AccountDIO
    {
        private static AccountDIO instance;
        public static AccountDIO Instance
        {
            get { if (instance == null) instance = new AccountDIO(); return instance; }
            private set { instance = value; }
        }
        private AccountDIO() { }
        
        public bool login(string username, string password)
        {
/*            byte[] temp = ASCIIEncoding.ASCII.GetBytes(password);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);
            string hasPass = "";
            foreach (byte item in hasData)
            {

            }*/    
            /*var stringlist = hasData.ToString();
            stringlist.Reverse();*/

            string query = "USP_Login @username , @password";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] {username, password});
            return result.Rows.Count>0;
        }

        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT DisplayName, UserName, Type FROM dbo.Account");
        }
        public bool UpdateAccount(string userName, string password, string newpassword, string displayname )
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC USP_UpdateAccount @userName , @displayName , @password , @newPassword", new object[] {userName,displayname,password,newpassword });
            return result > 0;
        }
        public Account GetAccountByUser(string username)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Account WHERE userName = '" + username + "'");
            foreach (DataRow row in data.Rows)
            {
                return new Account(row);
            }
            return null;
        }

        public bool InsertAccount(string username, string displayname, float type)
        {
            string query = string.Format("INSERT dbo.Account ( UserName, DisplayName, Type, PassWord ) VALUES (N'{0}', N'{1}', {2} , N'12345678')", username, displayname, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool UpdateAccount(string username, string displayname, int type)
        {
            string query = string.Format("UPDATE dbo.Account SET DisplayName = N'{1}', Type = {2} WHERE UserName = N'{0}'", username, displayname, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool DeleteAccount(string name)
        {
            string query = string.Format("DELETE dbo.Account WHERE UserName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool ResetPassword(string name)
        {
            string query = string.Format("Update dbo.Account SET PassWord = N'12345678' WHERE UserName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
