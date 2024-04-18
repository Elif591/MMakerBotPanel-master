namespace MMakerBotPanel.Business
{
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Database.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Security;

    public static class UserHelper
    {
        public static bool Login(User users)
        {
            users.Password = GetPassHash(users.Password);
            User user;
            using (ModelContext db = new ModelContext())
            {
                user = db.Users.FirstOrDefault(m => m.Email == users.Email && m.Password == users.Password);
            }

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Email, true);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool LogOut()
        {
            FormsAuthentication.SignOut();
            return true;
        }

        public static bool Register(User user)
        {
            try
            {
                string tmpPassword = user.Password;
                user.Password = GetPassHash(user.Password);
                user.PassaportAgain = "";
                user.Role = "Member";
                user.LastLogin = DateTime.Now;
                using (ModelContext db = new ModelContext())
                {
                    _ = db.Users.Add(user);
                    _ = db.SaveChanges();
                }
                user.Password = tmpPassword;
                return Login(user);


            }
            catch (Exception)
            {

                return false;
            }
        }

        public static string GetPassHash(string pass)
        {
            string sifreli_pass = "";
            List<char> karekter = pass.ToCharArray().ToList();
            for (int i = 0; i < karekter.Count(); i++)
            {
                sifreli_pass += (char)(karekter[i] + karekter.Count());
            }
            byte[] data = UTF8Encoding.UTF8.GetBytes(sifreli_pass);
            SHA256Managed sha = new SHA256Managed();
            byte[] result = sha.ComputeHash(data);
            StringBuilder output = new StringBuilder("");
            for (int i = 0; i < result.Length; i++)
            {
                _ = output.Append(result[i].ToString("X2"));
            }
            return output.ToString();

        }

        public static int GetUserID(string userName)
        {
            using (ModelContext db = new ModelContext())
            {
                User user = db.Users.First(x => x.Email == userName);
                return user != null ? user.UserID : 0;
            }
        }

        public static bool ResetPass(string email)
        {
            try
            {
                using (ModelContext db = new ModelContext())
                {
                    User user = db.Users.FirstOrDefault(x => x.Email == email);
                    if (user != null && user.Email != null)
                    {
                        string newPass = GetNewPass();
                        user.Password = GetPassHash(newPass);
                        db.SaveChanges();

                        string MailSubject = "Reset Password -  Cosmeta Support";
                        string MailBody = newPass;
                        Mailler.GetMailler().SendMail(MailSubject, MailBody, user.Email, user.Name);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                
                }

            }
            catch (Exception ex)
            {
                return false;
            }
#pragma warning restore CS0168 // 'ex' değişkeni ifade edilir ancak hiçbir zaman kullanılmaz

        }

        public static bool PasswordCheck(string password, int UserID)
        {
            try
            {
                password = GetPassHash(password);
                using (ModelContext db = new ModelContext())
                {
                    User user = db.Users.FirstOrDefault(x => x.UserID == UserID);
                    return user.Password == password;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static bool PasswordSave(string password, int UserID)
        {
            try
            {
                password = GetPassHash(password);
                using (ModelContext db = new ModelContext())
                {
                    User user = db.Users.FirstOrDefault(x => x.UserID == UserID);
                    user.Password = password;
                    _ = db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string GetNewPass()
        {
            Random rastgele = new Random();
            string harfler = "ABCDEFGHIJKLMNOPRSTUVYZWXQabcdefghıjklmnoprstuvyzwxq0123456789_-/:?&!";
            string newPass = "";
            for (int i = 0; i < 15; i++)
            {
                newPass += harfler[rastgele.Next(harfler.Length)];
            }
            return newPass;
        }

    }
}