using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class UpdateRegionRequestDto
    {

        [Required]
        [MinLength(3, ErrorMessage = "Max Length Should be 3")]
        [MaxLength(5, ErrorMessage = "Max Length Should be 5")]
        public string Code { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Max Length Should be 3")]
        [MaxLength(5, ErrorMessage = "Max Length Should be 5")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
