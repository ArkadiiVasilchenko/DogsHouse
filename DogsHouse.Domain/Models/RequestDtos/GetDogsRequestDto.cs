namespace DogsHouse.Domain.Models.RequestDtos
{
    public class GetDogsRequestDto
    {
        public string? Attribute { get; set; }
        public string? Order { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public bool IsEmpty =>
            string.IsNullOrEmpty(Attribute) &&
            string.IsNullOrEmpty(Order) &&
            PageNumber == default &&
            PageSize == default;
    }
}