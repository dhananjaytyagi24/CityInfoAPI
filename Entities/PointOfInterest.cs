using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CityInfo.Entities
{
	public class PointOfInterest
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        // By convention a relationship b/w classes/objects will be created if a navicalble property is found in the class
        // Relationships discovered by convention will always target the primary key of the other entity (cityId in this case)
        // Explicitly mentioning that the foreigKey for the navicable property is CityId; by default it will take the navicable prop name with Id at end for foreign key
        [JsonIgnore]
        [ForeignKey("CityId")]
        public City? City { get; set; }

        public int CityId { get; set; }

        public PointOfInterest(string name)
		{
            Name = name;
		}
	}
}

