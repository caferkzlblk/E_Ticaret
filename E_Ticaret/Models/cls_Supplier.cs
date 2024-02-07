using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Models
{
	
	public class cls_Supplier
	{
		ETicaretContext context = new();
		public  List<Supplier> SupplierSelect()
		{
			List<Supplier> suppliers = context.Suppliers.ToList();

			return suppliers;


		}
		public static bool SupplierInsert(Supplier supplier)
		{
			//static method oldugu icin context direkt gelmez


			using (ETicaretContext context = new())
			{
				try
				{

					context.Add(supplier);
					context.SaveChanges();
					return true;
				}
				catch (Exception)
				{

					return false;

				}


			}

			




		}

		public async Task<Supplier> SupplierDetails(int? id)
		{

			Supplier? supplier = await context.Suppliers.FirstOrDefaultAsync(s => s.SupplierID == id);

			return supplier;




		}


		public static bool SupplierUpdate(Supplier supplier)
		{
			using (ETicaretContext context = new())
			{
				try
				{
					//if (supplier.PhotoPath == null)
					//{
					//	supplier.PhotoPath = context.Suppliers.FirstOrDefault(s => s.SupplierID == supplier.SupplierID)?.PhotoPath;
					//}

					context.Update(supplier);
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
