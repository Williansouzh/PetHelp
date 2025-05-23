﻿using MediatR;
using PetHelp.Domain.Entities;
namespace PetHelp.Application.Animals.Commands;
public abstract class AnimalCommand : IRequest<Animal>
{
    public string Name { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public int Age { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string CreatedByUserId { get; set; }
}
