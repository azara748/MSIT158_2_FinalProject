using System.Security.Cryptography;

namespace MSIT158_2_FinalProject.Models.前台
{
	public class fM結帳
	{
		public List<string> 明細(int oid)
		{
            // 建立一個字串列表來存放訂單明細
            List<string> a = new List<string>();
            // 建立資料庫上下文物件
            SelectShopContext db = new SelectShopContext();
            // 從 TPurchases 表中查詢符合訂單 ID 的項目，並連接到 TProducts 表以取得商品的單價和名稱
            var p = db.TPurchases.Where(x => x.OrderId == oid).Join(db.TProducts, x => x.ProductId, y => y.ProductId, (x, y) =>
			new { y.UnitPrice, y.ProductName, x.Qty });
            // 從 TPackageWayDetails 表中查詢符合訂單 ID 的項目，並連接到 TAllPackages 表以取得包裹的價格和名稱
            var b = db.TPackageWayDetails.Where(x => x.OrderId == oid).Join(db.TAllPackages, x => x.PackageId, y => y.PackageId, (x, y) =>
			new { y.Price, y.PackName, x.PackQty });
            // 全部商品查詢結果，將每個商品的名稱和數量加入字串列表
            foreach (var x in p)
			{
				string s = x.ProductName + "x" + x.Qty;
				a.Add(s);
			}
            //全部包裹查詢結果，將每個包裹的名稱和數量加入字串列表
            foreach (var x in b)
			{
				string s = x.PackName + "x" + x.PackQty;
				a.Add(s);
			}
            // 返回包含訂單明細的字串列表
            return a;
		}
	}
}
