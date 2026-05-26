# API de Pedidos — Case Técnico Backend .NET

API REST em .NET 8 para gerenciamento de pedidos (case Wiz Co).

## Stack

- .NET 8, ASP.NET Core Web API
- EF Core + SQLite
- FluentValidation, AutoMapper
- Swagger, Docker
- xUnit, FluentAssertions, Moq
- ILogger

## Arquitetura

Camadas: **Domain**, **Application**, **Infrastructure**, **Api**, **Tests**.

Fluxo: `Controller → Service → Repository → DbContext`

## Estrutura

```
src/Api
src/Application
src/Domain
src/Infrastructure
tests/Tests
docs/TEORIA.md
docker-compose.yml
```

## Requisitos do case

**Obrigatórios:** criar/listar/buscar/cancelar pedidos, regras de total e itens, SQLite, EF Core.

**Diferenciais:** DTOs, AutoMapper, FluentValidation, paginação, logs, testes (14), Docker, middleware de erro, Swagger.

## Endpoints

| Método | Rota |
|--------|------|
| POST | `/pedidos` |
| GET | `/pedidos/{id}` |
| GET | `/pedidos?status=&page=&pageSize=` |
| PUT | `/pedidos/{id}/cancelar` |

Status: `Novo`, `Pago`, `Cancelado`.

## Exemplo — criar pedido

```json
{
  "clienteNome": "João Silva",
  "itens": [
    { "produtoNome": "Notebook", "quantidade": 1, "precoUnitario": 3500.00 }
  ]
}
```

## Executar local

```bash
dotnet restore
dotnet build
dotnet run --project src/Api/WizCo.Orders.Api.csproj
```

Swagger: http://localhost:5200/swagger

Banco local: `src/Api/data/orders.db`

## Docker

```bash
docker compose up --build
```

Swagger: http://localhost:8080/swagger

Banco: `./data/orders.db`

## Testes

```bash
dotnet test
```

14 testes unitários (domínio, validators, service).

## Decisões

- **EF Core + SQLite:** atende o case sem infra extra.
- **Camadas separadas:** domínio testável, sem MediatR/CQRS (poucos endpoints).
- **ValorTotal no domínio:** não confiar no cliente.
- **Middleware de erro:** respostas padronizadas, controller enxuto.
- **JWT:** fora do PDF; pode ser adicionado depois.

## Melhorias futuras

JWT, endpoint de pagamento, testes de integração, versionamento da API, CI/CD.

## Teoria

Perguntas do case: [docs/TEORIA.md](docs/TEORIA.md)
