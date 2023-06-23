﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CityInfo.Models;

namespace CityInfo.Entities
{
	public class City
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		// Above annotation specifies how a value will be generated by the db provider
		public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<PointOfInterest>? PointsOfInterest { get; set; }
            = new List<PointOfInterest>();

        [MaxLength(200)]
        public string? Description { get; set; }

        // Not initializing Name to string.Empty cause we want to be explicit that name is a required field for the creation of City
        public City(string name)
		{
			Name = name;
		}
	}
}

