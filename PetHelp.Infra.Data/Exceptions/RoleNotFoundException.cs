namespace PetHelp.Infra.Data.Exceptions;
using System;
public class RoleNotFoundException : Exception
{
    public RoleNotFoundException(string roleName) : base($"Role '{roleName}' not found.")
    {
    }
}
