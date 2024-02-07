using Microsoft.AspNetCore.Mvc;
using E_Ticaret.Models;
namespace E_Ticaret.ViewComponents
{
    
    public class Menus : ViewComponent
    {
        ETicaretContext context = new();

        public IViewComponentResult Invoke()
        {
            List<Category> categories = context.Categories.ToList();
            return View(categories);
        }
    }
}
