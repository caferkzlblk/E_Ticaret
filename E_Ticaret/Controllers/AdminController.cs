using E_Ticaret.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Controllers
{

    public class AdminController : Controller
    {
        cls_User u = new();
        cls_Category c = new();
        cls_Supplier s = new();
        ETicaretContext context = new();
        cls_Status st = new();
        cls_Product p = new();

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password,NameSurname")] User user)
        {
            if (ModelState.IsValid)
            {
                User? usr = await u.loginControl(user);
                if (usr != null)
                {
                    return RedirectToAction("Index");
                }

            }
            else
            {
                ViewBag.error = "Email or Password is wrong";
            }

            return View();
        }

        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> CategoryIndex()
        {
            List<Category> categories = await c.CategorySelect();

            return View(categories);

        }
        [HttpGet]
        public IActionResult CategoryCreate()
        {
            CategoryFill();

            return View();

        }

        void CategoryFill()
        {
            List<Category> categories = c.CategorySelectMain();
            ViewData["categoryList"] = categories.Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryID.ToString()
            });

        }
         void SupplierFill()
        {
            List<Supplier> suppliers =  s.SupplierSelect();
        	ViewData["supplierList"] = suppliers.Select(s => new SelectListItem { Text = s.BrandName, Value = s.SupplierID.ToString() });
        }
		 void  StatusFill()
		{
			List<Status> statuses =   st.StatusSelect();
			ViewData["statusList"] = statuses.Select(s => new SelectListItem { Text = s.StatusName, Value = s.StatusID.ToString() });
		}
		[HttpPost]

        public IActionResult CategoryCreate(Category category)
        {

            bool answer = cls_Category.CategoryInsert(category);

            if (answer)
            {
                TempData["Message"] = "Category Added";
                //return RedirectToAction(nameof(CategoryCreate));
            }
            else
            {
                TempData["Message"] = "Error";

            }
            //CateforyFill();
            //return View();

            return RedirectToAction(nameof(CategoryCreate));

        }

        [HttpGet]
        public async Task<IActionResult> CategoryEdit(int? id)
        {
            CategoryFill();

            if (id == null || context.Categories == null)
            {
                return NotFound();
            }

            var category = await c.CategoryDetails(id);
            return View(category);

        }

        [HttpPost]
        public IActionResult CategoryEdit(Category category)
        {
            bool answer = cls_Category.CategoryUpdate(category);

            if (answer)
            {
                TempData["Message"] = "Category Updated";
                return RedirectToAction(nameof(CategoryIndex));
            }
            else
            {
                TempData["Message"] = "Error";

            }
            //CateforyFill();
            //return View();

            return RedirectToAction(nameof(CategoryEdit));

        }

        public async Task<IActionResult> CategoryDetails(int? id)
        {

            var category = await c.CategoryDetails(id);
            ViewBag.Category = category.CategoryName;
            return View(category);

        }

        [HttpGet]
        public async Task<IActionResult> CategoryDelete(int? id)
        {
            if (id == null || context.Categories == null)
            {
                return NotFound();

            }
            else
            {
                var category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);
                if (category == null)
                {
                    return NotFound();

                }
                return View(category);
            }

        }
        [HttpPost]
        public async Task<IActionResult> CategoryDelete(int id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);
            category.IsActive = false;
            //subcategory Disactive false
            List<Category> categories = context.Categories.Where(c => c.ParentID == id).ToList();
            foreach (var item in categories)
            {
                item.IsActive = false;

            }

            await context.SaveChangesAsync();

            return RedirectToAction("CategoryIndex");

        }

        public  IActionResult SupplierIndex()
        {
            List<Supplier> suppliers =  s.SupplierSelect();

            return View(suppliers);

        }

        [HttpGet]
        public IActionResult SupplierCreate()
        {

            return View();

        }

        [HttpPost]

        public IActionResult SupplierCreate(Supplier supplier)
        {

            bool answer = cls_Supplier.SupplierInsert(supplier);

            if (answer)
            {
                TempData["Message"] = "Brand Added";

            }
            else
            {
                TempData["Message"] = "Error";

            }

            return RedirectToAction(nameof(SupplierCreate));

        }

        [HttpGet]
        public async Task<IActionResult> SupplierEdit(int? id)
        {

            if (id == null || context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await s.SupplierDetails(id);
            return View(supplier);

        }

        [HttpPost]
        public IActionResult SupplierEdit(Supplier supplier)
        {

            if (supplier.PhotoPath == null)
            {
                string? Photopath = context.Suppliers.FirstOrDefault(s => s.SupplierID == supplier.SupplierID)?.PhotoPath;
                supplier.PhotoPath = Photopath;

            }
            bool answer = cls_Supplier.SupplierUpdate(supplier);

            if (answer)
            {
                TempData["Message"] = supplier?.BrandName.ToUpper() + "Category Updated";
                return RedirectToAction(nameof(SupplierIndex));
            }
            else
            {
                TempData["Message"] = "Error";

            }
            //CateforyFill();
            //return View();

            return RedirectToAction(nameof(SupplierIndex));

        }
        static int? staticID = 0;
        public async Task<IActionResult> SupplierDetails(int? id)
        {
            if (id != null)
            {
                staticID = id;

            }
            if (id == null)
            {
                id = staticID;
            }

            var supplier = await s.SupplierDetails(id);
            ViewBag.supplier = supplier?.BrandName;
            return View(supplier);

        }
        [HttpGet]
        public async Task<IActionResult> SupplierDelete(int? id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();

            }
            else
            {
                var supplier = await context.Suppliers.FirstOrDefaultAsync(c => c.SupplierID == id);
                if (supplier == null)
                {
                    return NotFound();

                }
                return View(supplier);
            }

        }
        [HttpPost]
        public async Task<IActionResult> SupplierDelete(int id)
        {
            var supplier = await context.Suppliers.FirstOrDefaultAsync(c => c.SupplierID == id);
            if (supplier == null)
            {
                return NotFound();

            }
            else
            {
                supplier.IsActive = false;
            }

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(SupplierIndex));

        }

        public  IActionResult StatusIndex()
        {
            List<Status> statuses =  st.StatusSelect();

            return View(statuses);

        }

        [HttpGet]
        public IActionResult StatusCreate()
        {

            return View();

        }

        [HttpPost]

        public IActionResult StatusCreate(Status status)
        {

            bool answer = cls_Status.StatusInsert(status);

            if (answer)
            {
                TempData["Message"] = "Status Not Updated";

            }
            else
            {
                TempData["Message"] = "Error";

            }

            return RedirectToAction(nameof(StatusIndex));

        }
        [HttpGet]
        public async Task<IActionResult> StatusEdit(int? id)
        {

            if (id == null || context.Statuses == null)
            {
                return NotFound();
            }

            var status = await st.StatusDetails(id);
            return View(status);

        }

        [HttpPost]
        public IActionResult StatusEdit(Status status)
        {

            bool answer = cls_Status.StatusUpdate(status);

            if (answer)
            {
                TempData["Message"] = status?.StatusName.ToUpper() + "Status Updated";
                return RedirectToAction(nameof(StatusIndex));
            }
            else
            {
                TempData["Message"] = "Error";
                return RedirectToAction(nameof(StatusIndex));

            }

        }

        [HttpGet]
        public async Task<IActionResult> StatusDelete(int? id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();

            }
            else
            {
                var status = await context.Statuses.FirstOrDefaultAsync(st => st.StatusID == id);
                if (status == null)
                {
                    return NotFound();

                }
                return View(status);
            }

        }
        [HttpPost]
        public async Task<IActionResult> StatusDelete(int id)
        {
            var status = await context.Statuses.FirstOrDefaultAsync(st => st.StatusID == id);
            if (status == null)
            {
                return NotFound();

            }
            else
            {
                status.IsActive = false;
            }

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(StatusIndex));

        }

        public async Task<IActionResult> StatusDetails(int? id)
        {
            if (id != null)
            {
                staticID = id;

            }
            if (id == null)
            {
                id = staticID;
            }

            var status = await st.StatusDetails(id);
            ViewBag.supplier = status?.StatusName;
            return View(status);

        }

		public async Task<IActionResult> ProductIndex()
		{
            List<Product> products = await p.ProductSelect();

			return View(products);

		}


		[HttpGet]
		public IActionResult ProductCreate()
		{
			CategoryFill();
			SupplierFill();
			StatusFill();

			return View();

		}
		[HttpPost]
		public IActionResult ProductCreate(Product product)
		{

            if (ModelState.IsValid)
            {
				bool answer = cls_Product.ProductInsert(product);

				if (answer)
				{
					TempData["Message"] = "Product Added";
					
				}
				else
				{
					TempData["Message"] = "Error";

				}
				
			}
            else
            {
				TempData["Message"] = "Error";
			}


			return RedirectToAction(nameof(ProductCreate));

		}


        [HttpGet]
		public async Task<IActionResult> ProductEdit(int? id)
		{
            CategoryFill();
            SupplierFill();
            StatusFill();


			if (id == null || context.Products == null)
			{
				return NotFound();
			}

			var product = await p.ProductDetails(id);
			return View(product);

		}
        [HttpPost]
		public IActionResult ProductEdit(Product product)
		{
            DateTime addDate;

			if (product.PhotoPath == null)
			{
				string? Photopath = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID)?.PhotoPath;
				product.PhotoPath = Photopath;

			}
            addDate = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID).AddDate;
            product.AddDate = addDate;
			bool answer = cls_Product.ProductUpdate(product);

			if (answer)
			{
				TempData["Message"] = product?.ProductName?.ToUpper() + "Product Updated";
				return RedirectToAction(nameof(ProductIndex));
			}
			else
			{
				TempData["Message"] = "Error";

			}
			
			return RedirectToAction(nameof(ProductIndex));

		}

		public async Task<IActionResult> ProductDetails(int? id)
		{
			if (id != null)
			{
				staticID = id;

			}
			if (id == null)
			{
				id = staticID;
			}

			var product = await p.ProductDetails(id);
			ViewBag.supplier = product?.ProductName;
			return View(product);

		}


		[HttpGet]
		public async Task<IActionResult> ProductDelete(int? id)
		{
			if (id == null || context.Products == null)
			{
				return NotFound();

			}
			else
			{
				var product = await context.Products.FirstOrDefaultAsync(p => p.ProductID == id);
				if (product == null)
				{
					return NotFound();

				}
				return View(product);
			}

		}
		[HttpPost]
		public async Task<IActionResult> ProductDelete(int id)
		{
			var product = await context.Products.FirstOrDefaultAsync(p => p.ProductID == id);
			if (product == null)
			{
				return NotFound();

			}
			else
			{
				product.IsActive = false;
			}

			await context.SaveChangesAsync();

			return RedirectToAction(nameof(ProductIndex));

		}
	}

}