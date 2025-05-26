using Microsoft.AspNetCore.Mvc;
using PetHelp.Application.DTOs.Chat;
using PetHelp.Domain.Interfaces.Services;
using PetHelp.Infra.Data.Services;

namespace PetHelp.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IDialogFlowService _dialogflow;

    public ChatController(IDialogFlowService dialogflow)
    {
        _dialogflow = dialogflow;
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ChatRequest request)
    {
        var reply = await _dialogflow.SendMessageAsync(request.SessionId, request.Message);
        return Ok(new { reply });
    }
}
