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

Esta API implementa exatamente os endpoints solicitados no case: `POST /pedidos`, `GET /pedidos/{id}`, `GET /pedidos` (com `status`, `page` e `pageSize`) e `PUT /pedidos/{id}/cancelar`.
O `StatusPedido.Pago` existe no domínio apenas para suportar a regra de negócio “pedido pago não pode ser cancelado”; por isso não foi criado endpoint de pagamento (fora do escopo do case).
JWT não foi implementado porque não era requisito do case; fica como melhoria futura opcional.

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

## Decisões técnicas

- ValorTotal é calculado no backend e nunca recebido do cliente.
- SQLite foi usado por simplicidade de execução e por ser banco relacional.
- EF Core foi usado pela produtividade com CRUD, relacionamento Pedido/ItemPedido e migrations.
- Clean Architecture foi usada de forma simplificada para separar responsabilidades.
- Middleware global centraliza erros e mantém controllers limpos.
- Não foi criado endpoint de pagamento porque está fora do escopo do case.
- Não foi implementado JWT porque não era requisito obrigatório nem diferencial do PDF.

## Teoria

Perguntas do case: [docs/TEORIA.md](docs/TEORIA.md)
