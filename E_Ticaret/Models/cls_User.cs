using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Text;
using XSystem.Security.Cryptography;


namespace E_Ticaret.Models
{
    public class cls_User
    {
        ETicaretContext context = new();

        public async Task<User> loginControl(User user)
        {
            string md5sifrele = MD5Sifrele(user.Password);
            User? usr = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == md5sifrele && u.IsAdmin == true && u.IsActive == true);
            return usr;
        }

        public static User? SelectUserInfo(string Email)
        {
            using (ETicaretContext context = new())
            {
                User? user = context.Users.FirstOrDefault(u => u.Email == Email);

                return user;
            }
        }

        public static string AddUser(User user)
        {
            using (ETicaretContext context = new())
            {
                try
                {
                    User? usr = context.Users.FirstOrDefault(u => u.Email == user.Email);
                    if (usr != null)
                    {
                        //bu email zaten kayıtlı
                        return "Email zaten kayıtlı";
                    }
                    else
                    {

                        user.IsActive = true;
                        user.IsAdmin = false;
                        user.Password = MD5Sifrele(user.Password);
                        context.Users.Add(user);
                        context.SaveChanges();
                        return "Başarılı";
                    }
                }
                catch (Exception)
                {

                    return "Başarısız";
                }
            }
        }

        public static string MD5Sifrele(string value)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] btr = Encoding.UTF8.GetBytes(value);
            btr = md5.ComputeHash(btr);

            StringBuilder sb = new StringBuilder();
            foreach (byte item in btr)
            {
                sb.Append(item.ToString("x2").ToLower());
            }
            return sb.ToString();
        }

        public static string UserControl(User user)
        {
            using (ETicaretContext context = new())
            {
                string answer = "";

                try
                {
                    string md5sifrele = MD5Sifrele(user.Password);
                    User? usr = context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == md5sifrele && u.IsActive == true);
                    if (usr == null)
                    {
                        //Email veya şifre yanlıs veya her ikisi de
                        answer = "yanlis";
                    }
                    else
                    {
                        //email ve şifre doğru 
                        // admin mi
                        if (usr.IsAdmin)
                        {
                            answer = "admin";
                        }
                        else
                        {
                            answer = usr.Email;
                        }

                    }


                }
                catch (Exception)
                {

                    answer = "Error";
                }
                return answer;


            }

        }

        public static string Send_Sms(string OrderGroupGUID)
        {
            Order? order;
            User? user;
            using (ETicaretContext context = new ETicaretContext())
            {

                order = context.Orders.FirstOrDefault(o => o.OrderGroupGUID == OrderGroupGUID);
                user = context.Users.FirstOrDefault(u => u.UserId == order.UserID);

                //sayın cafer kızılbulak, 20.01.2024 tarihinde 13131354163 nolu siparişiniz alınmıştır.

                string content = $"Sayın {user?.NameSurname}, {order?.OrderDate} tarihinde , {OrderGroupGUID} nolu siparişiniz alınmıştır.";

                string ss = "";
                ss += "<?xml version = '1.0' encoding='UTF-8'>";
                ss += "<mainbody>";
                ss += "<header>";
                ss += "<company dil ='TR'> NETGSM</company>";
                ss += "<usercode>5359315900</usercode>";   //servisin sağladığı usercode
                ss += "<password>blabla</password>";  //servisin sağladığı password
                ss += "<type>1:n</type>";
                ss += "<msgheader>8503334455</msgheader>";
                ss += "</header>";
                ss += "<body>";
                ss += $"<msg><![CDATA[{content}]]></msg><no>0{user?.Telephone}</no>";
                ss += "</body>";
                ss += "</mainbody>";



                return XMLPOST("https://api.netgsm.com.tr/sms/send/xml", ss);
            }

        }
        static string XMLPOST(string PostAdress, string xmlData)
        {
            try
            {
                WebClient wUpload = new WebClient();
                HttpWebRequest request = WebRequest.Create(PostAdress) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                Byte[] bPostArray = Encoding.UTF8.GetBytes(xmlData);
                Byte[] bResponse = wUpload.UploadData(PostAdress, "POST", bPostArray);
                Char[] sReturnChars = Encoding.UTF8.GetChars(bResponse);
                string sWebPage = new string(sReturnChars);
                return sWebPage;
            }
            catch
            {
                return "-1";

            }
        }

        public static void Send_Email(string OrderGroupGUID)
        {

            using (ETicaretContext context = new())
            {
                Order order = context.Orders.FirstOrDefault(o => o.OrderGroupGUID == OrderGroupGUID);
                MailMessage mailMessage = new MailMessage();
                string subject = "Siparişiniz hakkında.";
                mailMessage.From = new MailAddress("info@eticaret.com", "Sipariş Bilgisi");
                User? user = context.Users.FirstOrDefault(u => u.UserId == order.UserID);
                mailMessage.To.Add(user.Email);
                string content = $"Sayın {user.NameSurname},{order.OrderDate} tarihinde, {OrderGroupGUID} nolu siparişiniz alınmıştır.";
                mailMessage.Body = content;
                mailMessage.Subject = subject;

                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential("Login Bilgisi burada olacak","sifre bilgisi burada olacak");
                smtp.Port = 587;
                smtp.Host = "smtp.caferkzlblk";

                try
                {
                    smtp.Send(mailMessage);
                }
                catch (Exception)
                {

                    //email gönderilemedi...
                }



            }



        }

    }
}
