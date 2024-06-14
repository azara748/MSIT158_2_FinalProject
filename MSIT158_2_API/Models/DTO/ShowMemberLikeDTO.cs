namespace MSIT158_2_API.Models.DTO
{
	public class ShowMemberLikeDTO
	{
		public int? MemeberId { get; set; }
		public int LikeId { get; set; }
		public int? ProductId { get; set; }
		public int? Status { get; set; }
		public string ProductName { get; set; }
		public byte[] Productphoto { get; set; }
		public decimal? UnitPrice { get; set; }
		//public string? MemberName { get; set; }
	}
}
