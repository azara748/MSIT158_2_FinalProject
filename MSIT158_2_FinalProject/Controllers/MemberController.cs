using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MSIT158_2_FinalProject.Models;
using MSIT158_2_FinalProject.ViewModel;

namespace MSIT158_2_FinalProject.Controllers
{
    public class MemberController : Controller
    {
        private readonly SelectShopContext _context;
        public MemberController(SelectShopContext context) 
        {
            _context = context;
        }
        public IActionResult List(CKeywordViewModel vm)
        {
            IEnumerable<TMember> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from m in _context.TMembers
                        select m;
            else
                datas = _context.TMembers.Where(r => r.MemberName.Contains(vm.txtKeyword) ||
                r.Cellphone.Contains(vm.txtKeyword) ||
                r.EMail.Contains(vm.txtKeyword) ||
                r.Address.Contains(vm.txtKeyword) ||
                r.Sex.Contains(vm.txtKeyword));
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Create(TMember p)
        //{
        //    _context.TMembers.Add(p);
        //    _context.SaveChanges();
        //    return RedirectToAction("List");
        //}

        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                TMember r = _context.TMembers.FirstOrDefault(x => x.MemberId == id);
                if (r != null)
                {
                    _context.TMembers.Remove(r);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            TMember r = _context.TMembers.FirstOrDefault(x => x.MemberId == id);
            if (r == null)
                return RedirectToAction("List");
            return View(r);
        }
        //[HttpPost]
        //public IActionResult Edit(TMember memberIn)
        //{
        //    TMember memberDb = _context.TMembers.FirstOrDefault(x => x.MemberId == memberIn.MemberId);
        //    if (memberDb != null)
        //    {
        //        memberDb.MemberName = memberIn.MemberName;
        //        memberDb.Address = memberIn.Address;
        //        memberDb.Cellphone = memberIn.Cellphone;
        //        memberDb.Birthday = memberIn.Birthday;
        //        memberDb.Sex = memberIn.Sex;
        //        memberDb.Password = memberIn.Password;
        //        memberDb.Points = memberIn.Points;
        //        memberDb.Vipid = memberIn.Vipid;
        //        memberDb.Wallet = memberIn.Wallet;
        //        _context.SaveChanges();
        //    }
        //    return RedirectToAction("List");
        //}
    }
}
