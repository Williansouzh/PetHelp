namespace PetHelp.Application.Adoptions.Commads;

public class UpdateAdoptionCommand : AdoptionCommand
{
    public Guid Id { get; set; }
    public UpdateAdoptionCommand(Guid id)
    {
        Id = id;
    }

}
