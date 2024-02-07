	namespace E_Ticaret.Models

	{
		public class MainPageModel
		{
			public List<Product>? SliderProducts { get; set; }
			public List<Product>? NewProducts { get; set; }
			public Product ProductOfDay { get; internal set; }

			public List<Product>? SpecialProducts { get; set; }

			public List<Product>? StarProducts { get; set; }

			public List<Product>? OppurtunityProducts { get; set; }

			public List<Product> RemarkableProducts { get; set; }
			public List<Product>? DiscountProducts { get; set; }
			public List<Product>? HighlightedProducts { get; set; }
			public List<Product> TopSellerProducts { get; set; }
            public Product? ProductDetails { get; set; }
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
        public List<Product> RelatedProducts { get; set; }



        //public List<Product> Products { get; set; } //CategoryPage
    }
	}
