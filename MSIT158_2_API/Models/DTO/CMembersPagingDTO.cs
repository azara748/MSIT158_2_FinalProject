﻿using MSIT158_2_API.Models.DTO.Member;

namespace MSIT158_2_API.Models.DTO
{
    public class CMembersPagingDTO
    {
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public List<CmemberDetailDTO>? MembersResult { get; set; }
    }
}
