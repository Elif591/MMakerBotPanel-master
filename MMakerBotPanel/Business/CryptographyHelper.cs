namespace MMakerBotPanel.Business
{
    using MMakerBotPanel.Models;
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class CryptographyHelper
    {
        public static string GetHMAC256(string text, string key, RETURNTYPE returnType)
        {
            key = key ?? "";
            using (HMACSHA256 hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(text));

                StringBuilder hex = new StringBuilder(hash.Length * 2);
                switch (returnType)
                {
                    case RETURNTYPE.HEX:
                        return ByteArrayToString(hash).ToLower();
                    case RETURNTYPE.BASE64:
                        return Convert.ToBase64String(hash);
                    default:
                        return ByteArrayToString(hash).ToLower();
                }
            }
        }

        public static string GetHMACSHA384(string text, string key)
        {
            key = key ?? "";

            using (HMACSHA384 hmacsha384 = new HMACSHA384(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hash = hmacsha384.ComputeHash(Encoding.UTF8.GetBytes(text));
                return ByteArrayToString(hash);
            }
        }

        public static string GetHMAC512(string text, string key, RETURNTYPE returnType)
        {
            key = key ?? "";

            using (HMACSHA512 hmacsha512 = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hash = hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(text));
                StringBuilder hex = new StringBuilder(hash.Length * 2);
                switch (returnType)
                {
                    case RETURNTYPE.HEX:
                        return ByteArrayToString(hash).ToLower();
                    case RETURNTYPE.BASE64:
                        return Convert.ToBase64String(hash);
                    default:
                        return ByteArrayToString(hash).ToLower();
                }
            }
        }
        public static string CreateSHA256(string strData, RETURNTYPE returnType)
        {
            byte[] message = Encoding.UTF8.GetBytes(strData);
            using (SHA256 alg = SHA256.Create())
            {
                byte[] hashValue = alg.ComputeHash(message);
                switch (returnType)
                {
                    case RETURNTYPE.HEX:
                        return ByteArrayToString(hashValue).ToLower();
                    case RETURNTYPE.BASE64:
                        return Convert.ToBase64String(hashValue);
                    default:
                        return ByteArrayToString(hashValue).ToLower();
                }

            }
        }

        public static string CreateSHA512(string strData)
        {
            byte[] message = Encoding.UTF8.GetBytes(strData);
            using (SHA512 alg = SHA512.Create())
            {
                byte[] hashValue = alg.ComputeHash(message);
                return ByteArrayToString(hashValue).ToLower();
            }
        }
        public static string CreateMD5Hash(string input)
        {
            // Step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return ByteArrayToString(hashBytes);
        }


        public static string ByteArrayToStringBybit(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);

            foreach (byte b in ba)
            {
                _ = hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }


        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }




    }
}