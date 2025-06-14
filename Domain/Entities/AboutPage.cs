using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
	public class AboutPage
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Content { get; set; }
	}
}
