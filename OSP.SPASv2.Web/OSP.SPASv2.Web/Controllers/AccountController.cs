using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OSP.SPASv2.Web.Areas.Identity.Data;
using SPASv2.Models;
using System.Threading.Tasks;

namespace OSP.SPASv2.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<OSPSPASv2ApplicationUser> _signInManager;
        private readonly UserManager<OSPSPASv2ApplicationUser> _userManager;

        public AccountController(SignInManager<OSPSPASv2ApplicationUser> signInManager, UserManager<OSPSPASv2ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var loggeduserid = _userManager.GetUserId(this.User);


            if (user != null)
            {
                if (user.Id != loggeduserid)
                {
                    return Json(new { success = false });
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }

        public IActionResult LoadAuthenticateModal(string AuthenticateModal)
        {
            AccountViewModel model = new AccountViewModel();
            model.AuthentiCate = JsonConvert.DeserializeObject<AuthentiCate>(AuthenticateModal);

            return PartialView("_AuthenticateModal", model);
        }


    }
}
