using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PetHelp.Infra.Data.Identity;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Role { get; set; }
}
