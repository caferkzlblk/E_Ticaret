using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Ticaret.Models
{
	public class Category
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CategoryID { get; set; }

        public int ParentID { get; set; }

		[StringLength(50, ErrorMessage = "The name can be a maximum of 50 characters. ") ]
		[Required(ErrorMessage ="You must enter Catagory Name ")]

		[DisplayName("Category Name")]
        public string? CategoryName { get; set; }

		[DisplayName("Active/NonActive")]

		
		public bool IsActive { get; set; }





    }
}
