
## 🐶 Visão Geral

A API do PetHelp será responsável por gerenciar:

- Cadastro e autenticação de usuários (ONGs e adotantes)
- Cadastro, listagem e adoção de animais
- Envio e gestão de denúncias
- Upload de imagens para o Google Cloud Storage
- Geolocalização de denúncias
- Dashboad para Ongs
- Chat de Assistente virtual

---

## 🧰 Tecnologias Utilizadas

- **.NET 8**
- **C#**
- **ASP.NET Core Web API**
- **Entity Framework Core (EF Core)** – ORM
- **PostgreSQL** – Banco de dados relacional
- **Google Cloud Storage** – Armazenamento de imagens
- **JWT** – Autenticação e autorização
- **FluentValidation** – Validação de dados
- **AutoMapper** – Mapeamento entre DTOs e modelos
- **Swagger (Swashbuckle)** – Documentação interativa da API

---

## 🔒 Regras de Acesso

| Papel | Permissões principais |
| --- | --- |
| **ONG** | Cadastrar/editar/remover animais, visualizar denúncias |
| **Adotante** | Ver lista de pets, enviar pedidos de adoção e denúncias |
| **Admin** (opcional) | Acompanhar métricas e denúncias críticas |

---

## 📦 Estrutura de Projeto

```
PetHelp/
├── .github/                        # Configurações do GitHub (Actions, Workflows etc.)
├── bin/                            # Arquivos binários compilados
├── obj/                            # Arquivos temporários de build

├── PetHelp.API/                    # Camada de Apresentação - Web API
│   ├── Controllers/                # Endpoints da API (ex: AuthControllerller)
│   ├── Middlewares/               # Middlewares personalizados (ex: trat. erros)
│   ├── appsettings.json           # Configurações da aplicação
│   ├── Credentials.json           # Credenciais de serviços externos
│   └── Program.cs                 # Ponto de entrada da aplicação

├── PetHelp.Application/           # Camada de Aplicação - Casos de uso
│   ├── Adoptions/                 # Lógica de adoções
│   ├── Animals/                   # Lógica de animais
│   ├── Reports/                   # Lógica de denúncias
│   ├── DTOs/                      # Objetos de transferência de dados
│   ├── Exceptions/                # Exceções personalizadas
│   ├── Extensions/                # Métodos de extensão
│   ├── Interfaces/                # Interfaces da camada de aplicação
│   ├── Mappings/                  # Perfis do AutoMapper
│   └── Pagination/                # Lógica de paginação
│   └── Services/                  # Lógica de serviços

├── PetHelp.Domain/                # Camada de Domínio - Regras de Negócio
│   ├── Account/                   # Lógica e entidades de conta/usuário
│   ├── Entities/                  # Entidades principais (ex: Animal, User)
│   ├── Enum/                      # Enums do domínio
│   ├── Interfaces/                # Interfaces de repositórios e serviços do domínio
│   └── Validation/                # Validações específicas do domínio

├── PetHelp.Infra.Data/            # Camada de Infraestrutura - Acesso a dados
│   ├── Context/                   # DbContext e conexões
│   ├── EntityConfiguration/       # Configurações do Entity Framework
│   ├── Exceptions/                # Exceções da infraestrutura
│   ├── Factories/                 # Fábricas de recursos
│   ├── Helpers/                   # Funções auxiliares
│   ├── Identity/                  # Autenticação e identidade
│   ├── Migrations/                # Migrations do banco de dados
│   ├── Persistence/               # Persistência de dados
│   ├── Repositories/              # Implementações dos repositórios
│   └── Services/                  # Serviços auxiliares

├── PetHelp.IoC/                   # Injeção de Dependências (IoC/DI)
│   ├── DependencyInjection.cs            # Registro geral de serviços
│   ├── DependencyInjectionJWT.cs         # Configuração de autenticação JWT
│   └── DependencyInjectionSwagger.cs     # Configuração do Swagger

├── docker-compose.yml             # Orquestração com Docker
├── .dockerignore                  # Arquivos ignorados pelo Docker
├── .gitignore                     # Arquivos ignorados pelo Git
├── PetHelp.sln                    # Solução principal do projeto
└── README.md                      # Documentação inicial do projeto
```

---

## 🎯 MVP – Funcionalidades Essenciais

### ✅ Autenticação

- Registro e login com email e senha (ONG/adotante)
- Geração e validação de token JWT
- Middleware para proteger rotas

### ✅ Gestão de Animais

- Cadastro com nome, espécie, idade, porte, descrição, imagem
- Listagem geral e filtrada
- Visualização individual
- Edição/remoção por ONG

### ✅ Adoções

- Pré-cadastro de interessados (formulário)
- ONG recebe info de quem quer adotar

### ✅ Denúncias

- Formulário com descrição, localização e imagem
- Listagem apenas para ONGs/Admins

### ✅ Upload de Imagem

- Upload multipart para Google Cloud Storage
- Retorno da URL pública para salvar no banco

### ✅ Chat bot

- Conectar com dialogFlow
- url do chat

### ✅ Dashboard

- retornar informacoes no dashboard

---

## 🗃️ Schemas e Entidades

### 🧍 Usuário

```csharp
public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
}

```

### 🐾 Animal

```csharp
Animal {
    Guid Id,
    string Name,
    string Species,
    string Breed,
    int Age,
    string Size,  // Pequeno, Médio, Grande
    string Description,
    string ImageUrl,
    Guid OngId, // FK para User
    DateTime CreatedAt
}

```

### 📄 Pedido de Adoção

```csharp
public class Animal : Entity
{
    public string Name { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public Size Size { get; set; }
    public string Description { get; set; }
    public bool IsVaccinated { get; set; }
    public bool IsNeutered { get; set; }
    public string AdoptionRequirements { get; set; }
    public AnimalStatus Status { get; set; }
    public List<string> PhotoUrls { get; set; } = new();
    public string? ImageUrl { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string CreatedByUserId { get; set; }

}

```

### 🚨 Denúncia

```csharp
public class Report : Entity
{
    public Guid Id { get; private set; }

    public string Description { get; private set; }
    public string? ImageUrl { get; private set; }

    public float Latitude { get; private set; }
    public float Longitude { get; private set; }

    public string Address { get; private set; }
    public AnimalType AnimalType { get; private set; }
    public UrgencyLevel UrgencyLevel { get; private set; }

    public Guid? UserId { get; private set; } // Pode ser nulo para denúncia anônima

    public string? Name { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
}
```

---

### 🚨 Adoção

```csharp
public class Adoption : Entity
{
    public Guid AnimalId { get; private set; }
    public string UserId { get; private set; }

    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public string Address { get; private set; }
    public bool HasOtherPets { get; private set; }
    public string HousingType { get; private set; }
    public int NumberOfResidents { get; private set; }
    public string WorkSchedule { get; private set; }
    public string ReasonForAdoption { get; private set; }
    public bool AgreedToTerms { get; private set; }

    public AdoptionStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
}
```

---

## 🔐 Autenticação JWT

- Registro → Criação de hash com `BCrypt`
- Login → Verifica e retorna JWT
- Middleware que verifica token nas rotas privadas
- Roles: Adotante | ONG

---

## 🧪 Regras de Negócio

- Apenas **ONGs autenticadas** podem cadastrar/editar animais
- Um usuário não pode solicitar adoção de um animal mais de uma vez
- Denúncias precisam obrigatoriamente de localização (lat/lng)
- Animais adotados não aparecem na listagem pública
- Upload de imagens é obrigatório para cadastrar animais (mas pode usar placeholder se não for enviado)

---

## 🔄 Endpoints Principais (REST)

| Método | Rota | Descrição |
| --- | --- | --- |
| POST | `/auth/register` | Registro de usuário |
| POST | `/auth/login` | Login e geração de token JWT |
| GET | `/animals` | Lista pública de animais |
| GET | `/animals/{id}` | Detalhes de um animal |
| POST | `/animals` (ONG) | Cadastrar novo animal |
| PUT | `/animals/{id}` (ONG) | Editar animal |
| DELETE | `/animals/{id}` (ONG) | Remover animal |
| POST | `/adoptions` | Enviar pedido de adoção |
| GET | `/dashboard/adoptions` | Ver pedidos de adoção recebidos |
| POST | `/reports` | Enviar denúncia com imagem/mapa |
| GET | `/reports` (ONG/Admin) | Listar denúncias |
| POTS | /chatbot | Chat |
| POTS | /dashboard | Dashboard de ongs |

---

## ☁️ Integração com Google Cloud Storage

- Pacote: `Google.Cloud.Storage.V1`
- Cloud Storage - Armazenamento de imagens
- Google Maps Api - Geolocalização de denúncias
- DialogFlow ES - Assistente virtual para chat
- Cloud Run - Hospedagem da API
- Cloud SQL - Banco de dados PostgreSQL
- Sendgrid - Envio de emails (opcional)
- FireBase - Autenticação e Analytics (opcional)

---

## 🧪 Validações (FluentValidation)

- Email e senha obrigatórios e formatados
- Campos como idade, nome, espécie com limites
- Denúncia precisa de localização válida
- Upload de imagem com extensões válidas (.jpg, .png)

---

## 🧹 Extra: Regras de Clean Code

- Use padrões SOLID nos serviços
- Separe responsabilidades (Controller → Service → Repo)
- Injeção de dependência via `IServiceCollection`
- Crie camadas claras: Domain, Application, Infrastructure

---

## 🧪 Testes (Se der tempo)

- Testes unitários com xUnit
- Testes de serviço e validações

---

## 🛠️ Variáveis de ambiente

```
{
    "ConnectionStrings": {
        "DefaultConnection": "Host=db;Port=5432;Database=pethelp_db;Username=pethelp_user;Password=secret123"
    },
    "Dialogflow": {
        "ProjectId": "pethelp-storage-access-460701"
    },
    "GoogleCloud": {
        "BucketName": "pethelp-images",
        "CredentialsPath": "Credentials.json"
    },
    "Jwt": {
        "Key": "ThisIsASecretKeyForJwtTokenDontShare",
        "Issuer": "YourApp",
        "Audience": "YourAppUsers"
    },
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*"
}

```

---