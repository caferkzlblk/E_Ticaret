using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Ticaret.Models
{
	public class Setting
	{
		[Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]

		public int SettingId { get; set; }

        public string? Telephone { get; set; }

		[EmailAddress]
        public string? Email { get; set; }

        public string? Address { get; set; }

        public int MainpageCount { get; set; }

        public int SubpageCount { get; set; } 




    }
}
