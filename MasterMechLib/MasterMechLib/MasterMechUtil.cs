using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MasterMechLib
{
    public class MasterMechUtil
    {
        public enum OPMode
        {
            New,
            Open,
            Delete
        }

        public static string msConString = "Integrated Security = SSPI; Persist Security Info = false; Initial Catalog = MasterMech; Data Source = (localdb)\\ProjectsV13";

        public static string sUserID;
        public static string sUserType;

        //Financial Year of the logged in
        private static string sFinYear;
        public static string BusinessState = "Jharkhand";
        public static string BusinessName = "Bappi AutoMotiv";
        public static string BusinessAddress = "Chandani Chowk, Chhota Govindpur, Jamshedpur, Jharkhand - 831015";
        public static string BusinessGST = "20AAFFP6534FZ";

        public static string sFY
        {
            set
            {
                if (Regex.IsMatch(value, @"^(\d{4}-\d{2}$)", RegexOptions.IgnoreCase))
                    sFinYear = value;
                else
                    throw new ArgumentException(String.Format("{0} is not a valid value for", value), "sFY");
            }
            get
            {
                return sFinYear;
            }
        }

        public static string[] FYList()
        {
            string[] lsFYList = new string[10];
            int lnCount = 0;
            int lnCurrYear = DateTime.Now.Year;
            int lnYear = lnCurrYear - 5;

            for(lnCount = 0; lnCount<10; lnCount++)
            {
                lsFYList[lnCount] = lnYear.ToString() + "-" + (lnYear++ + 1).ToString().Substring(2);
            }
            return lsFYList;
        }

        public static string CurrFY()
        {
            if (DateTime.Now.Month >= 4)
                return (DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString().Substring(2));
            else
                return ((DateTime.Now.Year-1).ToString() + "-" + DateTime.Now.Year.ToString().Substring(2));
        }

        public static string Encrypt(string encryptString)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }


    }
}
