using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHelp.Domain.Validation;

internal class DomainExceptValidation : Exception
{
    public DomainExceptValidation(string error) : base(error)
    {
    }
    public static void When(bool hasError, string error)
    {
        if (hasError)
            throw new DomainExceptValidation(error);
    }
}
