using System.ComponentModel.DataAnnotations;

namespace ModelBinding.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(1, 100)]
        public int Age { get; set; }
    }
}
