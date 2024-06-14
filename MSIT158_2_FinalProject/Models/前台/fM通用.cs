using MSIT158_2_FinalProject.Models;

namespace MSIT158_2_FinalProject.Controllers.前台
{
	public class fM通用
	{
		SelectShopContext db = new SelectShopContext();
		public int 二數百分比(int a,int b) { double c = (double)a / (double)b;return (int)c; }
		public double 商品評價百分比(int pid, int rid)
		{
			var v = db.TReviews.Where(x => x.ProductId == pid);
			double a = v.Count();
			double b = v.Where(x => x.RankId == rid).Count();
			if(b!=0&&a!=0) return (b / a)*100;
			else return 0;
		}
		public string 改成HTML換行符號(string a)
		{
			if(!string.IsNullOrEmpty(a))
			a = a.Replace("\r\n", "<br/>").Replace("\n", "<br/>");
			return a;
		}
	}
}
