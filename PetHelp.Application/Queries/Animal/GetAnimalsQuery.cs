using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace PetHelp.Application.Queries.Animal;

public class GetAnimalsQuery : IRequest<List<Domain.Entities.Animal>>
{
}
