using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Max Length Should be 3")]
        [MaxLength(5, ErrorMessage = "Max Length Should be 5")]
        public string Code { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Max Length Should be 3")]
        [MaxLength(50, ErrorMessage = "Max Length Should be 50")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
