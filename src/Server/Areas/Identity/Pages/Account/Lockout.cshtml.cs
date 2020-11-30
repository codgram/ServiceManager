using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceManager.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    [EnableCors("Development")]
    public class LockoutModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}
