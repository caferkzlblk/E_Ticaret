using Microsoft.AspNetCore.Mvc;
using E_Ticaret.Models;
using PagedList.Core;
using Microsoft.AspNetCore.Http;
using XAct.Users;
using User = E_Ticaret.Models.User;
using System.Collections.Specialized;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
namespace E_Ticaret.Controllers

{
	public class HomeController : Controller
	{
		ETicaretContext context = new();
		MainPageModel mpm = new();
		cls_Product cp = new();
		cls_Order o = new();
		cls_Category c = new();
		cls_Supplier s = new();

		public IActionResult Index()
		{

			mpm.SliderProducts = cp.ProductSelect("SliderProducts", "index", 0);
			mpm.NewProducts = cp.ProductSelect("NewProducts", "index", 0);
			mpm.ProductOfDay = cp.ProductSelect_OfDay();
			mpm.SpecialProducts = cp.ProductSelect("SpecialProducts", "index", 0);
			mpm.StarProducts = cp.ProductSelect("StarProducts", "index", 0);
			mpm.OppurtunityProducts = cp.ProductSelect("OppurtunityProducts", "index", 0);
			mpm.RemarkableProducts = cp.ProductSelect("RemarkableProducts", "index", 0);
			mpm.DiscountProducts = cp.ProductSelect("DiscountProducts", "index", 0);
			mpm.HighlightedProducts = cp.ProductSelect("HighlightedProducts", "index", 0);
			mpm.TopSellerProducts = cp.ProductSelect("TopSellerProducts", "index", 0);
			return View(mpm);
		}
		public IActionResult NewProducts()
		{
			mpm.NewProducts = cp.ProductSelect("NewProducts", "topmenu", 0); //alt sayfa
			return View(mpm);
		}
		public PartialViewResult _PartialNewProducts(string nextpagenumber)
		{
			int pagenumber = Convert.ToInt32(nextpagenumber);
			//nextpagenumber *4 = kaçıncı üründen başlayacak Skip
			mpm.NewProducts = cp.ProductSelect("NewProducts", "topmenuajax", pagenumber); //alt sayfa daha fazla butonuna tıklanınca 

			return PartialView(mpm);
		}

		public IActionResult SpecialProducts()
		{
			mpm.SpecialProducts = cp.ProductSelect("SpecialProducts", "topmenu", 0); //alt sayfa
			return View(mpm);
		}
		public PartialViewResult _PartialSpecialProducts(string nextpagenumber)
		{
			int pagenumber = Convert.ToInt32(nextpagenumber);
			//nextpagenumber *4 = kaçıncı üründen başlayacak Skip
			mpm.SpecialProducts = cp.ProductSelect("SpecialProducts", "topmenuajax", pagenumber); //alt sayfa daha fazla butonuna tıklanınca 

			return PartialView(mpm);
		}
		public IActionResult DiscountProducts()
		{
			HttpContext.Session.SetString("url", "http://localhost:5249/Home/DiscountProducts");
			mpm.DiscountProducts = cp.ProductSelect("DiscountProducts", "topmenu", 0); //alt sayfa
			return View(mpm);
		}
		public PartialViewResult _PartialDiscountProducts(string nextpagenumber)
		{
			int pagenumber = Convert.ToInt32(nextpagenumber);
			//nextpagenumber *4 = kaçıncı üründen başlayacak Skip
			mpm.DiscountProducts = cp.ProductSelect("DiscountProducts", "topmenuajax", pagenumber); //alt sayfa daha fazla butonuna tıklanınca 

			return PartialView(mpm);
		}
		public IActionResult TopSellerProducts(int page = 1, int pageSize = 4)
		{
			PagedList<Product> model = new PagedList<Product>(context.Products.OrderByDescending(p => p.TopSeller), page, pageSize); ;
			return View("TopSellerProducts", model);
		}

		public IActionResult HighlightedProducts(string nextpagenumber)
		{
			mpm.HighlightedProducts = cp.ProductSelect("HighlightedProducts", "topmenu", 0);
			return View(mpm);
		}
		public PartialViewResult _HighlightedProducts(string nextpagenumber)
		{
			int pagenumber = Convert.ToInt32(nextpagenumber);
			mpm.HighlightedProducts = cp.ProductSelect("HighlightedProducts", "topmenuajax", pagenumber);
			return PartialView(mpm);

        }


        public IActionResult CategoryPage(int id)
		{
			List<Product> products = cp.ProductsSelectByCategoryID(id);
			return View(products);
		}
		//SupplierPage
		public IActionResult SupplierPage(int id)
		{
			List<Product> products = cp.ProductsSelectBySupplierID(id);
			return View(products);
		}


		public IActionResult CartProccess(int id)
		{
			cp.HigLightedPlus(id);
			o.ProductID = id;
			o.Quantity = 1;

			var cookieOptions = new CookieOptions();

			//çerez politikası = cookies  = tarayıcada tutulur

			var cookie = Request.Cookies["sepetim"]; //tarayıcıda sepetim isminde bir cookie var mı ?

			if (cookie == null)
			{
				//Kullanıcı sisteme ilk defa ürün ekleyecek
				cookieOptions = new CookieOptions();
				cookieOptions.Expires = DateTime.Now.AddDays(1); // 1 günlük cooki değeri    
				cookieOptions.Path = "/";
				o.Sepet = "";
				o.AddToCart(id.ToString()); // Sepete ekle methodu
				Response.Cookies.Append("sepetim", o.Sepet, cookieOptions); //tanımladıgımız cerezi tarayıcıya gönderdik.
				HttpContext.Session.SetString("Message", "Ürün sepetinize eklendi.");
				TempData["Message"] = "Ürün sepetinize eklendi.";

			}
			else
			{
				//kullanıcı daha önceden sepetine ürün eklemiş. Sepetin içeriğine yeni şeyler ekleyecek
				// aynı ürün daha önceden eklenmiş mi ?
				o.Sepet = cookie;
				if (o.AddToCart(id.ToString()) == false)
				{
					//Eklenmemiş

					Response.Cookies.Append("sepetim", o.Sepet, cookieOptions);
					cookieOptions.Expires = DateTime.Now.AddDays(1);
					//HttpContext.Session.SetString("Message", "Ürün sepetinize eklendi.");
					TempData["Message"] = "Ürün sepetinize eklendi.";
				}
				else
				{

					TempData["Message"] = "Bu ürün sepetinizde zaten var";
				}

			}
			//ürünü sepete ekledikten sonra aynı sayfaya geri götürür.
			//string url = Request.Headers["Refferer"].ToString();
			//return Redirect(url);
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Details(int id)
		{
			//efcore
			//mpm.ProductDetails = context.Products.FirstOrDefault(p => p.ProductID == id);
			
			//select * from Products where ProductID = id ---> ado.net

			//linq - 4 nolu ürünün bütün kolon bilgileri getir
			mpm.ProductDetails = (from p in context.Products where p.ProductID == id select p).FirstOrDefault();

            //linq 
            mpm.CategoryName = (from p in context.Products
                                join c in context.Categories on p.CategoryID equals c.CategoryID
                                where p.ProductID == id
                                select c.CategoryName).FirstOrDefault();




            //linq 
            mpm.BrandName = (from p in context.Products join s in context.Suppliers on p.SupplierID equals s.SupplierID where p.SupplierID == id select s.BrandName).FirstOrDefault();

			//select * from Products where Related = 2 and ProductID != 4

			mpm.RelatedProducts = context.Products.Where(p => p.Related == mpm.ProductDetails!.Related && p.ProductID != id).ToList();

			cp.HigLightedPlus(id);

            ViewBag.Categories = await c.CategorySelect();
            ViewBag.Suppliers = s.SupplierSelect();
            return View(mpm);



		}

		// sağ üst köşedeki sepet sayfasına tıkladıgında calısacak
		public IActionResult Cart(int id)
		{
			if (HttpContext.Request.Query["scid"].ToString() != "")
			{
				//sayafada ürün silerken sil butonu tıklanınca scid göndereceğiz
				//sepetim cookisinden ürün silerek tekrar cart.cshtml sayfası yüklenecek

				int scid = Convert.ToInt32(HttpContext.Request.Query["scid"].ToString());
				o.Sepet = Request.Cookies["sepetim"]; // Tarayıcıdan cookiyi aldık propa koyduk
				o.DeleteFromMyCart(scid.ToString());
				var cookieOptions = new CookieOptions();
				cookieOptions = new CookieOptions();
				cookieOptions.Expires = DateTime.Now.AddDays(1); // 1 günlük cooki değeri
				Response.Cookies.Append("sepetim", o.Sepet, cookieOptions); //tanımladıgımız cerezi tarayıcıya gönderdik.
				TempData["Message"] = "Ürün sepetinizden silindi.";

				//cart.cshtml ürünleri foreach ile dönüp gösterecek bilgiler hazır olmalı
				List<cls_Order> sepet = o.SelectMyCart();
				ViewBag.Sepetim = sepet;
				ViewBag.sepet_tablo_detay = sepet;

			}
			else
			{
				// sağ üst köşedeki sepet sayfasına tıkladıgında calısacak
				//sepetim cookisinden ürün silerek tekrar cart.cshtml sayfası yüklenecek

				var cookie = Request.Cookies["sepetim"];
				List<cls_Order> sepet;
				var cookieOptions = new CookieOptions();
				if (cookie == null)
				{
					
					o.Sepet = "";
					sepet = o.SelectMyCart();
					ViewBag.Sepetim = sepet;
					ViewBag.sepet_tablo_detay = sepet;

				}
				else
				{
					o.Sepet = Request.Cookies["sepetim"];
					sepet = o.SelectMyCart();
					ViewBag.Sepetim = sepet;
					ViewBag.sepet_tablo_detay = sepet;
				}

			}
			return View();
		}


		[HttpGet]
		public IActionResult Order()
		{


            //kullanıcı giriş yapmış mı
            if (HttpContext.Session.GetString("Email")!= null)
			{
				//giriş yapmış, Email isminde bir oturum almış
				User? user = cls_User.SelectUserInfo(HttpContext.Session.GetString("Email"));	

				return View(user);
			}
			else
			{
				string url = Request.Headers["Referer"].ToString();
                HttpContext.Session.SetString("url", url);

                return RedirectToAction("Login");
			}
		}
		[HttpPost]
		public IActionResult Order(Order order,IFormCollection frm)
		{
			if (Request.Form["txt_tckimlikno"] != "")
			{
				string? tckimlikno = Request.Form["txt_tckimlikno"];



            }
             if (frm["txt_vergino"] != "")
            {
                string? vergino = Request.Form["txt_vergino"];

            }
			string? kredikartno = Request.Form["kredikartno"];

            string? kredikartay = Request.Form["kredikartay"];

            string? kredikartyil = Request.Form["kredikartyil"];

            string? kredikartcvv = Request.Form["kredikartcvv"];
			/*

			NameValueCollection data = new NameValueCollection();

			string payu_url = "https://www.isimsoyisim.com/backref";
			data.Add("BACK_REF", payu_url);
			data.Add("CC_CVV", kredikartcvv);
			data.Add("CC_NUMBER", kredikartno);
			data.Add("CC_MONTH", kredikartay);
			data.Add("CC_YEAR", kredikartyil);

			var deger = "";

			foreach (var item in data)
			{
				var value = item as string;
				var byteCount = Encoding.UTF8.GetByteCount(data.Get(value));
				deger += byteCount + data.Get(value);
			}

			var signatureKey = "size verilen SECRET_KEY";
			var hash = HashWithSignature(deger, signatureKey);
			data.Add("ORDER_HASH", hash);
			var x = POSTFormPayu("https://secure.payu.com.tr/order",data);

			if (x.Contains("<STATUS>SUCCESS</STATUS>")&& x.Contains("<RETURN_CODE>3DS_ENROLLED</RETURN_CODE>"))
			{
				//sanal kart ile alışveriş yaptı ok
			}
			else 
			{
				if (x.Contains("<STATUS>SUCCESS</STATUS>") && x.Contains("<RETURN_CODE>AUTHORIZED</RETURN_CODE>"))
				{
					//gercek kart ile alısveris yaptı ok

				}

            }
			*/

            return RedirectToAction("backref");
		}

        public string POSTFormPayu(string url, NameValueCollection data)
        {
            return "";
        }
        public string HashWithSignature(string deger, string signatureKey)
		{
			return "";
		}


        public IActionResult backref()
		{
			OrderConfirm();

			return RedirectToAction("Confirm");
		}
		
		public static string OrderGroupGUID = ""; //20240120211350
		public static string cevap = ""; //Netgsmden dönen cevap için değişken

        public IActionResult OrderConfirm()
		{
            //cookie sepetindeki siparişi orders tablosuna yazıcaz,sepeti sileceğiz

            var cookieOptions = new CookieOptions();

            //çerez politikası = cookies  = tarayıcada tutulur

            var cookie = Request.Cookies["sepetim"]; //tarayıcıda sepetim isminde bir cookie var mı ?

            if (cookie != null)
            {
                o.Sepet = cookie;

				OrderGroupGUID = o.WriteFromCookieToOrderTable(HttpContext.Session.GetString("Email").ToString());
                cookieOptions.Expires = DateTime.Now.AddDays(1);
				Response.Cookies.Delete("sepetim");
                cevap =  cls_User.Send_Sms(OrderGroupGUID);
                cls_User.Send_Email(OrderGroupGUID);


            }

			return RedirectToAction("Confirm");

        }
		public IActionResult Confirm()
		{
			ViewBag.OrderGroupGUID = OrderGroupGUID;

            return View();
		}


        [HttpGet]
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
        public IActionResult Login(User user)
        {
			string answer = cls_User.UserControl(user);

			if (answer == "yanlis")
			{
                TempData["Message"] = "Error, Try Again.";
				return View();
            }
			else if(answer == "admin")
			{
                HttpContext.Session.SetString("Admin", answer);
                return RedirectToAction("Login", "Admin");

            }
            else if (answer == "Error")
            {
                TempData["Message"] = "Error, Try Again.";
				return View();
            }
            else
			{
				HttpContext.Session.SetString("Email", answer);
				if (HttpContext.Session.GetString("url") != null)
				{
					string? url = (HttpContext.Session.GetString("url"));

                    return Redirect(url);
				}
                return RedirectToAction("Index", "Home");
            }



            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
			return View();
        }
		[HttpPost]
        public IActionResult Register(User user)
        {    
            string answer = cls_User.AddUser(user);
            if (answer == "Başarılı")
            {
                TempData["Message"] = "Bilgileriniz başarıyla kaydedildi.";
                return RedirectToAction("Login");
            }
            else if (answer == "Email zaten kayıtlı")
            {
                TempData["Message"] = "Bu Email Zaten Kayıtlı.";
                return RedirectToAction("Login");
            }

            TempData["Message"] = "Error";


            return View();
        }
		public IActionResult Logout() 
		{
			HttpContext.Session.Remove("Email");
			HttpContext.Session.Remove("Admin");

            return RedirectToAction("Login");		
		}

		public IActionResult MyOrders()
		{
			if (HttpContext.Session.GetString("Email") != null)
			{
                List <VW_MyOrders> orders= o.SelectMyOrders(HttpContext.Session.GetString("Email").ToString());
				return View(orders);
			}
			return RedirectToAction("Login");
		}

		public async Task<IActionResult> DetailedSearch()
		{
			ViewBag.Categories = await c.CategorySelect();
			ViewBag.Suppliers =  s.SupplierSelect();
			return View();

		}


		public IActionResult DpProducts(int CategoryID, string[] SupplierID, string price, string isInStock)
		{
			price = price.Replace(" ", "").Replace("TL", ""); // *200-50000
			string[] PriceArray = price.Split("-");
			string startmoney = PriceArray[0];
			string endmoney = PriceArray[1];

			string sign = ">";
			if (isInStock == "0")
			{
				sign = ">=";
			}

			int count = 0;

			string SupplierValue = "";

			for (int i = 0; i < SupplierID.Length; i++)
			{
				if (count == 0)
				{
					//tek marka
					//Select * From Products Where (SupplierID = 2)
					SupplierValue = "SupplierID = " + SupplierID[i];
					count++;
				}
				else
				{
					//Birden fazla marka
					//Select * From Products Where (SupplierID = 2 or SupplierID = 3)
					SupplierValue += " or SupplierID = " + SupplierID[i];

                }
            }

			if (SupplierValue != "")
			{
				SupplierValue = $"({SupplierValue}) and";
			}

			string query = $"select * from Products where CategoryID = {CategoryID} and {SupplierValue}(UnitPrice > {startmoney} and UnitPrice < {endmoney}) and  Stock {sign} 0 order by ProductName";

			ViewBag.Products = o.Select_Products_DetailsSearch(query);

            return View();
		}


		public PartialViewResult gettingProducts(string id)
		{
			id = id.ToUpper(new System.Globalization.CultureInfo("tr-TR"));
			List<sp_search> uList = cls_Product.gettingSearchProducts(id);
			string json = JsonConvert.SerializeObject(uList);
			var response = JsonConvert.DeserializeObject<List<Search>>(json);

			return PartialView(response);
		}

    }
}

