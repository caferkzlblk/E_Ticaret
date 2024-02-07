using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Ticaret.Models
{
	public class Product
	{
		[Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }


		[StringLength(100)]
		[Required]
        public string? ProductName { get; set; }

        public decimal UnitPrice { get; set; }


		[DisplayName("Category")]
		public int CategoryID { get; set; }

        
        

        [DisplayName("Brand")]
        public int SupplierID { get; set; }

        public int Stock{ get; set; }

        public int Discount { get; set; }

		[DisplayName("Status")]
        public int StatusID { get; set; }

        public DateTime AddDate { get; set; }

        public string? KeyWords { get; set; }
     
        private int _Kdv { get; set; }
		public int Kdv {
			get { return _Kdv; }

			set { _Kdv = Math.Abs(value); }
		}

        public int HighLighted { get; set; } // Önce Cıkanlar

        public int TopSeller { get; set; } //Cok satanlar

        public int Related { get; set; } //Buna bakanlar

        public string? Notes { get; set; }

        public string? PhotoPath { get; set; }

        public bool IsActive { get; set; }




    }
		
	






	}

