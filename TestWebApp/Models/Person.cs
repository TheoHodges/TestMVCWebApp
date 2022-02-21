using System;
using System.ComponentModel.DataAnnotations;

namespace TestAPI.Models
{
    public class Person
    {
        public int ID { get; set; }

        [StringLength(60, MinimumLength = 2)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(60, MinimumLength = 2)]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required]
        [StringLength(30)]
        public string PlaceOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        public Boolean Married { get; set; }
    }
}
