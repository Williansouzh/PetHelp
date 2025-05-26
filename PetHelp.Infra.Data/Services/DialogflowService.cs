using Google.Apis.Auth.OAuth2;
using Google.Cloud.Dialogflow.V2;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;
using PetHelp.Domain.Interfaces.Services;

namespace PetHelp.Infra.Data.Services;

public class DialogflowService : IDialogFlowService
{
    private readonly SessionsClient _client;
    private readonly string _projectId;

    public DialogflowService(IConfiguration config)
    {
        _projectId = config["Dialogflow:ProjectId"];
        var credentialsPath = config["GoogleCloud:CredentialsPath"];

        // Carrega as credenciais da conta de serviço
        var credential = GoogleCredential
            .FromFile(credentialsPath)
            .CreateScoped(SessionsClient.DefaultScopes);

        // Cria o cliente usando o SessionsClientBuilder e passando as credenciais
        _client = new SessionsClientBuilder
        {
            // Passa as credenciais diretamente aqui
            Credential = credential
        }.Build();
    }

    public async Task<string> SendMessageAsync(string sessionId, string message)
    {
        var sessionName = SessionName.FromProjectSession(_projectId, sessionId);

        var queryInput = new QueryInput
        {
            Text = new TextInput
            {
                Text = message,
                LanguageCode = "pt-BR"
            }
        };

        var response = await _client.DetectIntentAsync(sessionName, queryInput);
        return response.QueryResult.FulfillmentText;
    }
}
