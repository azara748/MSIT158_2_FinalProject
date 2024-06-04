using Microsoft.AspNetCore.Mvc;
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
            return View(datas);
        }
    }
}
