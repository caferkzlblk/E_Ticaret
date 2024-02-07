using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Models
{
    public class cls_Status
    {
        ETicaretContext context = new();
        public  List<Status> StatusSelect()
        {
            List<Status> statuses =  context.Statuses.ToList();

            return statuses;


        }
		public static bool StatusInsert(Status status)
		{
			//static method oldugu icin context direkt gelmez


			using (ETicaretContext context = new())
			{
				try
				{

					context.Add(status);
					context.SaveChanges();
					return true;
				}
				catch (Exception)
				{

					return false;

				}


			}

		}

		public async Task<Status> StatusDetails(int? id)
		{
			

			Status? status = await context.Statuses.FirstOrDefaultAsync(st => st.StatusID == id);
			
		
				return status;
			

		




		}




		public static bool StatusUpdate(Status status)
		{
			using (ETicaretContext context = new())
			{
				try
				{
					//if (supplier.PhotoPath == null)
					//{
					//	supplier.PhotoPath = context.Suppliers.FirstOrDefault(s => s.SupplierID == supplier.SupplierID)?.PhotoPath;
					//}

					context.Update(status);
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
