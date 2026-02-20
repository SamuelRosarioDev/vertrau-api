# ApiVertrau

API RESTful para gerenciamento de usuários, desenvolvida com ASP.NET Core (.NET 10), seguindo os princípios de Clean Architecture com separação em múltiplos projetos.

---

## 🏗️ Arquitetura

O projeto segue a arquitetura em camadas, dividido em 4 projetos principais:

```
task-vertrau/
├── ApiVertrau.Domain/          # Entidades, enums e exceções de domínio
├── ApiVertrau.Application/     # DTOs, mappers, interfaces e serviços
├── ApiVertrau.Infrastructure/  # Repositórios, migrations e type handlers
├── ApiVertrau.API/             # Controllers, middlewares, configurações e Program.cs
└── ApiVertrau.Tests/           # Testes unitários
```

### Fluxo de dependências

```
API → Application → Domain
Infrastructure → Application → Domain
Tests → Application → Domain
```

---

## 🚀 Tecnologias

| Tecnologia | Versão | Uso |
|---|---|---|
| .NET | 10 | Framework principal |
| ASP.NET Core | 10 | Web API |
| Dapper | 2.1.66 | ORM leve para SQL |
| SQLite | 10.0.3 | Banco de dados |
| FluentMigrator | 8.0.1 | Migrations de banco de dados |
| Swashbuckle (Swagger) | 6.9.0 | Documentação da API |
| xUnit | 2.9.3 | Testes unitários |
| Moq | 4.20.72 | Mocks para testes |

---

## 📋 Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Git

---

## ⚙️ Configuração

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/task-vertrau.git
cd task-vertrau
```

### 2. Configure a connection string

Edite o arquivo `ApiVertrau.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=database.db"
  }
}
```

### 3. Execute o projeto

```bash
cd ApiVertrau.API
dotnet run
```

As migrations são executadas automaticamente na inicialização.

---

## 📖 Documentação da API

Com o projeto em execução, acesse o Swagger em:

```
http://localhost:5165/swagger
```

### Endpoints disponíveis

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/v1/users` | Lista todos os usuários |
| `GET` | `/api/v1/users/{id}` | Busca usuário por ID |
| `POST` | `/api/v1/users` | Cria novo usuário |
| `PUT` | `/api/v1/users/{id}` | Atualiza usuário completo |
| `PATCH` | `/api/v1/users/{id}` | Atualiza campos parciais |
| `DELETE` | `/api/v1/users/{id}` | Remove usuário |

### Exemplo de requisição — POST /api/v1/users

```json
{
  "nome": "João",
  "sobrenome": "Silva",
  "email": "joao@vertrau.com.br",
  "genero": 0,
  "dataNascimento": "1990-01-15"
}
```

### Valores do campo `genero`

| Valor | Descrição |
|---|---|
| `0` | Masculino |
| `1` | Feminino |
| `2` | Outro |

### Exemplo de resposta — 201 Created

```json
{
  "id": 1,
  "nome": "João",
  "sobrenome": "Silva",
  "email": "joao@vertrau.com.br",
  "genero": "MASCULINO",
  "dataNascimento": "1990-01-15"
}
```

### Códigos de resposta

| Código | Descrição |
|---|---|
| `200` | Sucesso |
| `201` | Criado com sucesso |
| `204` | Sem conteúdo (atualização/remoção) |
| `400` | Dados inválidos |
| `404` | Recurso não encontrado |
| `409` | Conflito (e-mail já cadastrado) |
| `500` | Erro interno no servidor |

---

## 🧪 Testes

### Executar os testes

```bash
cd ApiVertrau.Tests
dotnet test
```

### Cobertura

Os testes cobrem os casos principais do `UserService`:

| Método | Casos testados |
|---|---|
| `CreateAsync` | Criação com sucesso, e-mail duplicado |
| `GetByIdAsync` | Usuário encontrado, não encontrado |
| `GetAllAsync` | Lista com dados, lista vazia |
| `UpdateAsync` | Atualização com sucesso, não encontrado, e-mail conflitante |
| `PatchAsync` | Atualização parcial, não encontrado |
| `DeleteAsync` | Remoção com sucesso, não encontrado |

---

## 📁 Estrutura detalhada

```
ApiVertrau.Domain/
├── Entities/
│   └── User.cs                        # Entidade principal com validações de domínio
├── Enums/
│   └── Gender.cs                      # Enum de gênero
└── Exceptions/
    ├── ConflictException.cs
    ├── DomainException.cs
    └── NotFoundException.cs

ApiVertrau.Application/
├── DTOs/
│   └── UserDTO.cs                     # CreateUsuarioDTO, UsuarioResponseDTO, etc.
├── Interfaces/
│   ├── IUserRepository.cs             # Contrato do repositório
│   └── IUsersServices.cs             # Contrato do serviço
├── Mappers/
│   └── UserMapper.cs                  # Conversão entre Domain e DTO
└── Services/
    └── UserService.cs                 # Implementação das regras de negócio

ApiVertrau.Infrastructure/
├── Migrations/
│   └── 001_CreateUsersTable.cs        # Migration da tabela de usuários
├── Repositories/
│   └── UserRepository.cs             # Implementação com Dapper + SQLite
└── TypeHandlers/
    └── SqliteDateOnlyHandler.cs       # Handler para DateOnly no SQLite

ApiVertrau.API/
├── Configs/
│   ├── DbConfig.cs                    # Configuração do FluentMigrator
│   └── DependencyInjectionConfig.cs   # Registro de dependências
├── Controllers/
│   └── UsersController.cs            # Endpoints da API
├── Middlewares/
│   └── ExceptionMiddleware.cs        # Tratamento global de exceções
└── Program.cs

ApiVertrau.Tests/
└── Users/
    └── UserServiceTests.cs           # 14 testes unitários
```

---

## 🔒 Validações

Todas as entradas são validadas antes de chegar ao domínio:

- `Nome` e `Sobrenome`: obrigatórios, mínimo 2 caracteres
- `Email`: obrigatório, formato válido, único no sistema
- `Genero`: obrigatório, valor deve existir no enum
- `DataNascimento`: opcional, não pode ser data futura

---

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`)
3. Commit suas mudanças (`git commit -m 'feat: adiciona nova feature'`)
4. Push para a branch (`git push origin feature/nova-feature`)
5. Abra um Pull Request