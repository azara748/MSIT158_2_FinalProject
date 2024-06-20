using System.Security.Cryptography;

namespace MSIT158_2_FinalProject.Models.前台
{
	public class fM結帳
	{
		public List<string> 明細(int oid)
		{
			List<string> a = new List<string>();
			SelectShopContext db = new SelectShopContext();
			var p = db.TPurchases.Where(x => x.OrderId == oid).Join(db.TProducts, x => x.ProductId, y => y.ProductId, (x, y) =>
			new { y.UnitPrice, y.ProductName, x.Qty });
			var b = db.TPackageWayDetails.Where(x => x.OrderId == oid).Join(db.TAllPackages, x => x.PackageId, y => y.PackageId, (x, y) =>
			new { y.Price, y.PackName, x.PackQty });
			foreach (var x in p)
			{
				string s = x.ProductName + "x" + x.Qty;
				a.Add(s);
			}
			foreach (var x in b)
			{
				string s = x.PackName + "x" + x.PackQty;
				a.Add(s);
			}
			return a;
		}
	}
}
