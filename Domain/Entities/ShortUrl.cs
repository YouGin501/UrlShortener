using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	public class ShortUrl
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[Url]
		public string OriginalUrl { get; set; }

		[Required]
		[MaxLength(10)]
		public string ShortCode { get; set; }

		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

		public string CreatedById { get; set; }

		[ForeignKey(nameof(CreatedById))]
		public IdentityUser CreatedBy { get; set; }
	}
}
