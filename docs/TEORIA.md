# Respostas teóricas — Case Wiz Co

## 1. Lógica / Arquitetura

### API lenta — como investigar?

Verificaria logs, métricas e tempo de resposta por endpoint. Depois analisaria consultas ao banco (queries lentas, índices, N+1 no EF), consumo de CPU/memória e chamadas externas. Também verificaria se o problema começou após alguma mudança ou deploy.

### Endpoint muito chamado, mesma resposta, query pesada

Usaria cache (memória ou Redis) com TTL, já que o resultado não muda com frequência. Também avaliaria otimização da query e índices. Dependendo do cenário, poderia usar uma tabela materializada para leitura.

### Operação crítica não executar duas vezes

Usaria idempotency key, constraint única no banco ou controle de status para processar apenas uma vez. Em cenários distribuídos, poderia usar lock ou fila com processamento idempotente.

### Concorrência em atualização

Usaria concorrência otimista com `RowVersion` no EF Core. Em alguns casos específicos poderia usar lock pessimista, mas normalmente possui maior custo.

### Endpoint com 10x mais tráfego

Escalaria horizontalmente com mais instâncias e load balancer, adicionaria cache e monitoramento, além de otimizar consultas ao banco. Processamentos pesados poderiam ir para filas.

### CORS no browser, Postman ok

CORS é uma política aplicada pelo navegador; o Postman não valida isso. Normalmente ocorre por configuração incorreta de origem permitida ou preflight `OPTIONS`.

---

## 2. .NET / C#

### Task, ValueTask e async void

* **Task:** padrão para métodos assíncronos.
* **ValueTask:** útil quando a operação frequentemente completa de forma síncrona, mas deve ser usado apenas quando existe ganho real.
* **async void:** recomendado apenas para eventos; exceções ficam mais difíceis de tratar.

### Dependency Injection

Permite registrar interfaces e implementações no container, reduzindo acoplamento e facilitando testes e manutenção.

### Transient, Scoped, Singleton

* **Transient:** nova instância a cada solicitação.
* **Scoped:** uma instância por request.
* **Singleton:** uma instância durante toda a aplicação.

`DbContext` normalmente deve ser `Scoped`.

### Middleware

São componentes do pipeline HTTP que interceptam requests e responses. Exemplos: autenticação, logs e tratamento global de erros.

---

## 3. Dapper

### Quando usar em vez de EF Core

Quando preciso de SQL mais complexo ou controle maior sobre consultas e performance. Para CRUD e relacionamentos, EF Core normalmente oferece maior produtividade.

### Evitar SQL Injection

Usar consultas parametrizadas (`@Id`) e nunca concatenar entrada do usuário diretamente na query.

---

## 4. REST API

### PUT vs PATCH

**PUT** substitui o recurso inteiro. **PATCH** altera apenas campos específicos.

### Versionar API

Pode ser feito por URL (`/api/v1/pedidos`), headers ou query string. URL costuma ser a abordagem mais simples e comum.

### Códigos HTTP

* **200** — sucesso
* **201** — recurso criado
* **400** — erro de validação
* **409** — conflito de regra de negócio
* **401** — não autenticado
* **403** — sem permissão
* **404** — recurso não encontrado
* **500** — erro interno

---

## 5. Banco de Dados

### INNER JOIN vs LEFT JOIN

**INNER JOIN:** retorna apenas registros com correspondência nas duas tabelas.

**LEFT JOIN:** retorna todos os registros da tabela esquerda e correspondências da direita, usando `NULL` quando não existir.

### Índice

Índices aceleram buscas e ordenações. Em excesso, podem aumentar custo de armazenamento e reduzir desempenho em INSERT e UPDATE.

### Relacional vs não relacional

**Relacional:** tabelas, relacionamentos e transações; indicado quando existe consistência forte.

**Não relacional:** estrutura mais flexível e fácil escalabilidade horizontal; indicado para documentos, eventos e grandes volumes de dados.
