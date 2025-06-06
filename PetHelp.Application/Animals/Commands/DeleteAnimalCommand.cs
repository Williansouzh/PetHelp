﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using PetHelp.Domain.Entities;

namespace PetHelp.Application.Animals.Commands;

public class DeleteAnimalCommand : IRequest<Animal>
{
    public Guid Id { get; set; }
}
