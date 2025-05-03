using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHelp.Domain.Account;

public interface ISeedUserRoleInitial
{
    Task SeedRolesAsync();
    Task SeedUsersAsync();
}

