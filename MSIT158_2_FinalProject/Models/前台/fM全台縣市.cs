using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MSIT158_2_FinalProject.Models.前台
{
    public class fM全台縣市
    {
        public List<data> data { get; set; }
    }
    public class data
    {
        public List<districts> districts { get; set; }
        public string name { get; set; }
    }
    public class districts
    {
        public string name { get; set; }
        public string zip { get; set; }
    }
}
