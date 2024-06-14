namespace MSIT158_2_API.Models.DTO
{
	public class ToClientProductDTO
	{
		public int TotalPages { get; set; }
		public int TotalCount { get; set; }
		public List<ShowProductDTO>? showProducts { get; set; }
	}
}
