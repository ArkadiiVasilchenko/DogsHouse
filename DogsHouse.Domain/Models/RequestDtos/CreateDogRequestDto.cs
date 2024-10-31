namespace DogsHouse.Domain.Models.RequestDtos
{
    public class CreateDogRequestDto
    {     
        public string Name { get; set; }  
        public string Color { get; set; }
        public int TailLength { get; set; }
        public int Weight { get; set; }

        public bool IsEmpty =>
            string.IsNullOrEmpty(Name) &&
            string.IsNullOrEmpty(Color) &&
            TailLength == default &&
            Weight == default;
    }
}
