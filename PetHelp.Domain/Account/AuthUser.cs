using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHelp.Domain.Account;

public class AuthUser
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public bool Success { get; set; } = true;
    public IEnumerable<string> Roles { get; set; } = new List<string>();
}
