using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Models
{
	public class cls_Category
	{
		ETicaretContext context = new();

		public async Task<List<Category>> CategorySelect()
		{
			List<Category> categories = await context.Categories.ToListAsync();

			return categories;


		}
		public List<Category> CategorySelectMain()
		{
			List<Category> categories = context.Categories.Where(c => c.ParentID == 0).ToList() ;

			return categories;


		}

		public static bool CategoryInsert(Category category)
		{
			//static method oldugu icin context direkt gelmez


			using (ETicaretContext context = new())
			{
				try { 

				context.Add(category);
				context.SaveChanges();
				return true;
				}
				catch (Exception)
				{

					return false;

				}
				

			}

		}

		public async Task<Category> CategoryDetails(int? id)
		{

			Category? category = new();
			category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);


			
			return category;




		}
		public static bool CategoryUpdate(Category category)
		{
			using (ETicaretContext context = new())
			{
				try
				{

					context.Update(category);
					context.SaveChanges();
					return true;
				}
				catch (Exception)
				{

					return false;

				}


			}
		}
       
		
    }
}
