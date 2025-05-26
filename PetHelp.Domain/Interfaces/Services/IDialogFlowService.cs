namespace PetHelp.Domain.Interfaces.Services;

public interface IDialogFlowService
{
    Task<string> SendMessageAsync(string sessionId, string message);
}
