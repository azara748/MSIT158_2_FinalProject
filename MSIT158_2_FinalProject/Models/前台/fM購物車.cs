using MSIT158_2_FinalProject.Models;
using System.Net;
using System.Security.Cryptography;

namespace MSIT158_2_FinalProject.Controllers.前台;

public class fM購物車
{
	public void add購物車(int pid, int Qty, int mid = 2)
	{
		SelectShopContext db = new SelectShopContext();
		var b = db.TCarts.FirstOrDefault(x => x.ProductId == pid&&x.MemberId==mid);
		if (b == null)
		{
			TCart cart = new TCart();
			cart.ProductId = pid;
			cart.Qty = Qty;
			cart.MemberId = mid;
			db.Add(cart);
		}
		else b.Qty += Qty;
		db.SaveChanges();
	}
	public void add購物車2(TCart a)
	{
		SelectShopContext db = new SelectShopContext();
		var b = db.TCarts.FirstOrDefault(x => x.ProductId == a.ProductId && x.MemberId == a.MemberId);
		if (b == null) db.Add(a);
		else b.Qty += a.Qty;
		db.SaveChanges();
	}
    public void add包裝(TPackageCart a)
    {
        SelectShopContext db = new SelectShopContext();
        var b = db.TPackageCarts.FirstOrDefault(x => x.PackageId == a.PackageId && x.MemberId == a.PackageId);
        if (b == null) db.Add(a);
        else b.Qty += a.Qty;
        db.SaveChanges();
    }
    public void plus購物車(int id)
	{
		SelectShopContext db = new SelectShopContext();
		var a = db.TCarts.FirstOrDefault(x => x.CartId == id);
		a.Qty++;
		db.SaveChanges();
	}
    public void plus購物車2(int id)
    {
        SelectShopContext db = new SelectShopContext();
        var a = db.TPackageCarts.FirstOrDefault(x => x.PackageCartId == id);
        a.Qty++;
        db.SaveChanges();
    }
    public void minus購物車(int id)
	{
		SelectShopContext db = new SelectShopContext();
		var a = db.TCarts.FirstOrDefault(x => x.CartId == id);
		a.Qty--;
		db.SaveChanges();
	}
    public void minus購物車2(int id)
    {
        SelectShopContext db = new SelectShopContext();
        var a = db.TPackageCarts.FirstOrDefault(x => x.PackageCartId == id);
        a.Qty--;
        db.SaveChanges();
    }
    public void delete購物車(int id)
	{
		SelectShopContext db = new SelectShopContext();
		var a = db.TCarts.FirstOrDefault(x => x.CartId == id);
		db.Remove(a);
		db.SaveChanges();
	}
    public void delete購物車2(int id)
    {
        SelectShopContext db = new SelectShopContext();
        var a = db.TPackageCarts.FirstOrDefault(x => x.PackageCartId == id);
        db.Remove(a);
        db.SaveChanges();
    }
    //  public void delete所選(int mid)
    //  {
    //      SelectShopContext db = new SelectShopContext();
    //      var a = db.TCarts;
    //foreach (var x in a)
    //{
    //	if((bool)x.Checking)db.Remove(x);
    //}
    //      db.SaveChanges();
    //  }
    public void 結帳(TOrder o)
    {
        SelectShopContext db = new SelectShopContext();      
        db.TOrders.Add(o);
        db.SaveChanges();    
		int lo = db.TOrders.OrderBy(x=>x.OrderId).FirstOrDefault().OrderId;
        var a = db.TCarts.Where(x=>x.MemberId==o.MemberId);
		foreach (var x in a)
		{
			TPurchase p = new TPurchase();
			p.OrderId = lo;
			p.ProductId = x.ProductId;
			p.Qty = x.Qty;
			db.TPurchases.Add(p);
            db.TCarts.Remove(x);
        }		
        db.SaveChanges();
    }
}

