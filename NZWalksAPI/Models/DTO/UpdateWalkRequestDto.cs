using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class UpdateWalkRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Max Length Should be 3")]
        [MaxLength(5, ErrorMessage = "Max Length Should be 5")]
        public string Name { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Max Length Should be 3")]
        [MaxLength(5, ErrorMessage = "Max Length Should be 5")]
        public string Description { get; set; }
        [Required]
        [Range(0, 50)]
        public string LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }
    }
}
