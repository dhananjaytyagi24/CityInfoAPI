using System;
using System.ComponentModel.DataAnnotations;

namespace CityInfo.Models
{
	public class UpdatePointOfInterestDto
	{
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}

