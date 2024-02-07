using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using PagedList.Core;
using System.Net;
using System.Text;

namespace E_Ticaret.Models
{

    public class cls_Order
    {
        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public string? Sepet { get; set; } //10=1 & 20=1
        public Decimal UnitPrice { get; set; }

        public int Kdv { get; set; }

        public string? PhotoPath { get; set; }

        public string? ProductName { get; set; }

        //Sepete Ekle

        ETicaretContext context = new();

        public bool AddToCart(string id)
        {
            bool varmi = false;
            if (Sepet == "")
            {
                //sepete ilk defa ürün ekliyor
                Sepet = id + "=1";

            }
            else
            {

                //daha önceden sepete eklenmiş ürün veya ürünler var
                string[] sepetdizi = Sepet.Split("&");

                for (int i = 0; i < sepetdizi.Length; i++)
                {
                    //10=1
                    //20=1
                    string[] sepetdizi2 = sepetdizi[i].Split("=");

                    if (sepetdizi2[0] == id)
                    {
                        //bu ürün zaten sepette var
                        //sepetdizi[0] = ProductID
                        //sepetdizi[1] = Adet
                        //sepetdizi[1] + 1
                        //int deger = Convert.ToInt32(sepetdizi[1]) + 1;
                        //Sepet = Sepet + "&" + id + "=" + deger.ToString() ;//ürün adedini 1 arttırmak

                        varmi = true;
                    }


                }
                if (varmi == false)
                {
                    //ürün sepete daha önce eklenmemiş
                    Sepet = Sepet + "&" + id + "=1";
                }

            }

            return varmi;
        }


        //projede sağ üst köşedeki sepet sayfası ve sil butonu tıklanınca yüklenecek olan sayfa bu methodu çağıracak
        //List<cls_Order>  propertyleri döndürcek
        //siparişi onaylama methodu çağıracak
        public List<cls_Order> SelectMyCart()
        {
            List<cls_Order> list = new List<cls_Order>();

            string[] sepetdizi = Sepet.Split('&'); //ürünleri ayırdık 10=1 20=1 gibi (id ve sayısı)

            if (sepetdizi[0] != "")
            {
                for (int i = 0; i < sepetdizi.Length; i++)
                {
                    string[] sepetdizi2 = sepetdizi[i].Split('=');
                    int sepetid = Convert.ToInt32(sepetdizi2[0]);// product id
                    int adet = Convert.ToInt32(sepetdizi2[1]);// adet bilgisi

                    Product? product = context.Products.FirstOrDefault(p => p.ProductID == sepetid);

                    cls_Order p = new cls_Order();

                    p.ProductID = product.ProductID; //veritabaından gelen bilgileri propertiylere gönderiyoruz
                    p.Quantity = adet;
                    p.ProductName = product.ProductName;
                    p.UnitPrice = product.UnitPrice;
                    p.Kdv = product.Kdv;
                    p.PhotoPath = product.PhotoPath;
                    list.Add(p);//Listeye ekleme
                }
            }
            return list;
        }



        public void DeleteFromMyCart(string id)
        {
            string[] sepetdizi = Sepet.Split('&'); // ürünleri ayır
            string yenisepet = "";
            for (int i = 0; i < sepetdizi.Length; i++)
            {
                string[] sepetdizi2 = sepetdizi[i].Split('=');
                int adet = Convert.ToInt32(sepetdizi2[1]); // adeti aldık
                if (sepetdizi2[0] != id)
                {
                    //silinicek ürün hariçleri ekletiyor
                    if (yenisepet == "")
                    {
                        yenisepet = sepetdizi2[0] + "=" + adet;
                    }
                    else
                    {
                        yenisepet = yenisepet + "&" + sepetdizi2[0] + "=" + adet;
                    }
                }
            }
            Sepet = yenisepet;
        }


        public string WriteFromCookieToOrderTable(string Email)
        {
            string OrderGroupGUID = DateTime.Now.ToString().Replace(".", "").Replace(":", "").Replace(" ", "");
            DateTime OrderDate = DateTime.Now;

            List<cls_Order> orders = SelectMyCart();
            foreach (var item in orders)
            {
                Order order = new Order();
                order.OrderDate = OrderDate;
                order.OrderGroupGUID = OrderGroupGUID;
                order.UserID = context.Users.FirstOrDefault(u => u.Email == Email).UserId;
                order.ProductID = item.ProductID;
                order.Quantity = item.Quantity;
                context.Orders.Add(order);
                context.SaveChanges();

            }

            return OrderGroupGUID;
        }


        public List<VW_MyOrders> SelectMyOrders(string Email)
        {
            int? UserID = context.Users.FirstOrDefault(u => u.Email == Email)?.UserId;
            List<VW_MyOrders> orders = context.vw_MyOrders.Where(o => o.UserId == UserID).ToList();
            return orders;
        }


        public List<cls_Order> Select_Products_DetailsSearch(string query)
        {
            List<cls_Order> products = new List<cls_Order>();

            SqlConnection sqlcon = connection.baglanti;
            SqlCommand sqlcmd = new SqlCommand(query,sqlcon);
            sqlcon.Open();
            SqlDataReader sqlDataReader = sqlcmd.ExecuteReader();
            while (sqlDataReader.Read())
            {
                cls_Order p = new cls_Order();
                p.ProductID = Convert.ToInt32(sqlDataReader["ProductID"]);
                p.ProductName = (sqlDataReader["ProductName"]).ToString();
                p.UnitPrice = Convert.ToDecimal(sqlDataReader["UnitPrice"]);
                p.PhotoPath = (sqlDataReader["ProductID"]).ToString();
                products.Add(p);
            }
            sqlcon.Close();
            return products;
        }



    }
}
