using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Ticaret.Models
{
	public class User
	{
		[Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int UserId { get; set; }

		[Required]
		[StringLength(100)]
		public string? NameSurname { get; set; }

		[Required]
		[StringLength(100)]
		[DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

		[Required,StringLength(100)]

		[DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? Telephone { get; set; }

        public string? InvoiceAddress { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsActive { get; set; }	



    }
}
