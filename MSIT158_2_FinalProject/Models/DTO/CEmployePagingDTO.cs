using MSIT158_2_FinalProject.Models.DTO.Employee;

namespace MSIT158_2_FinalProject.Models.DTO
{
    public class CEmployePagingDTO
    {
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public List<CemployeeDetailDTO>? EmployeesResult { get; set; }
    }
}
