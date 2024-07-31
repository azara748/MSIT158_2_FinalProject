using MSIT158_2_FinalProject.Models;

namespace MSIT158_2_FinalProject.Controllers.前台
{
	public class fM通用
	{
		SelectShopContext db = new SelectShopContext();
        // 計算兩數之間的百分比
        public int 二數百分比(int a,int b) { double c = (double)a / (double)b;return (int)c; }
        // 計算特定商品特定評價等級的百分比
        public double 商品評價百分比(int pid, int rid)
		{
			var v = db.TReviews.Where(x => x.ProductId == pid); // 查詢特定商品的評價
            double a = v.Count(); // 總評價數量
            double b = v.Where(x => x.RankId == rid).Count(); // 特定評價等級的數量
            if (b!=0&&a!=0) return (b / a)*100; // 計算百分比
            else return 0; // 若無評價或無特定評價等級，返回0
        }
        // 將字串中的換行符號替換為HTML的換行標籤
        public string 改成HTML換行符號(string a)
		{
			if(!string.IsNullOrEmpty(a))
			a = a.Replace("\r\n", "<br/>").Replace("\n", "<br/>");
			return a;
		}
	}
}
