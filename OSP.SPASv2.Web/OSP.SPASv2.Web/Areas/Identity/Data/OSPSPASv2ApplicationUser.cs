using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OSP.SPASv2.Web.Areas.Identity.Data;

// Add profile data for application users by adding properties to the OSPSPASv2ApplicationUser class
public class OSPSPASv2ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

