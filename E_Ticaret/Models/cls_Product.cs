using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace E_Ticaret.Models
{
	public class cls_Product
	{
		ETicaretContext context = new();
		public async Task<List<Product>> ProductSelect()
		{
			List<Product> products = await context.Products.ToListAsync();

			return products;


		}
		public static bool ProductInsert(Product product)
		{
			//static method oldugu icin context direkt gelmez


			using (ETicaretContext context = new())
			{
				try
				{
					//product.ProductName = product.ProductName.Trim();
					if (product.Notes == null)
					{
						product.Notes = string.Empty;

					}
					product.AddDate = DateTime.Now;
					bool exists = context.Products.Any(p => p.ProductName.ToLower().Equals(product.ProductName.ToLower().Trim()));
					if (!exists)
					{

						context.Add(product);
						context.SaveChanges();
						return true;
					}
					return false;



				}
				catch (Exception)
				{

					return false;

				}


			}

		}


		public async Task<Product> ProductDetails(int? id)
		{

			Product? product = await context.Products.FirstOrDefaultAsync(p => p.ProductID == id);

			return product;




		}
		public static bool ProductUpdate(Product product)
		{
			using (ETicaretContext context = new())
			{
				try
				{


					context.Update(product);
					context.SaveChanges();
					return true;
				}
				catch (Exception)
				{

					return false;

				}


			}
		}
		public List<Product> ProductSelect(string mainPageName, string subPageName, int pagenumber)
		{
			List<Product>? productsList;
			if (mainPageName == "SliderProducts")
			{
				//slider ürünleri getiren sorgu
				productsList = context.Products.Where(p => p.StatusID == 1).Take(8).ToList();


			}
			else if (mainPageName == "NewProducts")
			{
				if (subPageName == "index")
				{
					//home indexten geliyorsa
					productsList = context.Products.OrderByDescending(p => p.AddDate).Take(8).ToList();
				}
				else
				{
					// home/newproducts ---- ikinci parametre

					if (subPageName == "topmenu")
					{
						productsList = context.Products.OrderByDescending(p => p.AddDate).Take(4).ToList();

					}
					else
					{
						//ajax
						productsList = context.Products.OrderByDescending(p => p.AddDate).Skip(pagenumber * 4).Take(4).ToList();
					}

				}

				//yeni ürünleri getiren sorgu

			}
			else if (mainPageName == "SpecialProducts")
			{
				//productsList = context.Products.Where(p => p.StatusID == 2).Take(8).ToList();
				if (subPageName == "topmenu")
				{
					productsList = context.Products.Where(p => p.StatusID == 2).Take(4).ToList();
				}
				else
				{
					productsList = context.Products.Where(p => p.StatusID == 2).Skip(pagenumber * 4).Take(4).ToList();
				}


			}
			else if (mainPageName == "DiscountProducts")
			{
				if (subPageName == "index")
				{
					productsList = context.Products.OrderByDescending(p => p.Discount).Take(8).ToList();

				}
				else
				{
					if (subPageName == "topmenu")
					{
						productsList = context.Products.OrderByDescending(p => p.Discount).Take(4).ToList();
					}
					else
					{
						productsList = context.Products.OrderByDescending(p => p.Discount).Skip(pagenumber * 4).Take(4).ToList();
					}
				}




			}
			else if (mainPageName == "StarProducts")
			{
				productsList = context.Products.Where(p => p.StatusID == 3).Take(8).ToList();

			}
			else if (mainPageName == "OppurtunityProducts")
			{
				productsList = context.Products.Where(p => p.StatusID == 4).Take(8).ToList();

			}
			else if (mainPageName == "RemarkableProducts")
			{
				productsList = context.Products.Where(p => p.StatusID == 5).Take(8).ToList();

			}

			else if (mainPageName == "HighlightedProducts")
			{
				productsList = context.Products.OrderByDescending(p => p.HighLighted).Take(8).ToList();

			}
			else if (mainPageName == "TopSellerProducts")
			{
				productsList = context.Products.OrderByDescending(p => p.TopSeller).Take(8).ToList();

			}
			else
			{
				productsList = null;

			}


			return productsList;


		}

		public Product ProductSelect_OfDay()
		{
			Product? product = context.Products.FirstOrDefault(p => p.StatusID == 6);

			return product;
		}
		public List<Product> ProductsSelectByCategoryID(int id)
		{
			List<Product> products = context.Products.Where(p => p.CategoryID == id).OrderBy(p => p.ProductName).ToList();

			return products;


		}

		public List<Product> ProductsSelectBySupplierID(int id)
		{
			List<Product> products = context.Products.Where(s => s.SupplierID == id).OrderBy(p => p.ProductName).ToList();

			return products;


		}


		public void HigLightedPlus(int? id)
		{

			//Products tablosunda highleted kolonunu 1 arttır
			// veritabanından ürünü bulma
			// o ürünğn higleted 1 arttır
			// savechanges

			using (ETicaretContext context = new())
			{


				Product product = context.Products.FirstOrDefault(c => c.ProductID == id);
				product.HighLighted = product.HighLighted + 1;
				context.Update(product);
				context.SaveChanges();




			}



		}


		public static List<sp_search> gettingSearchProducts (string id)
		{
			using(ETicaretContext context = new ETicaretContext()) 
			{
				var products = context.sp_Searches.FromSql($"sp_search {id}").ToList();
				return products;
			}


			
		}


    }
}
