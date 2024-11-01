using System.ComponentModel.DataAnnotations;

namespace DogsHouse.Domain.Entities
{
    public class Dog
    {
        //[Key]
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Color { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Tail length must be greater than or equal to 0")]
        public int TailLength { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Weight must be greater than 0")]
        public int Weight { get; set; }

        public Dog() { }
        public Dog(string name, string color, int tailLength, int weight)
        {
            Name = name;
            Color = color;
            TailLength = tailLength;
            Weight = weight;
        }
    }
}