# Order Management System – VINCI Evaluation

Este projeto foi desenvolvido como parte de um desafio técnico para a posição de **Desenvolvedor .NET Sênior**. Ele simula um sistema de gerenciamento de pedidos com escrita assíncrona, leitura performática, mensageria desacoplada e arquitetura moderna baseada em microsserviços.

---

## 🚀 Tecnologias Utilizadas

### Backend
- **.NET 8**
- **Entity Framework Core** – ORM para SQL Server
- **MongoDB.Driver** – leitura performática
- **Redis** – cache para leitura
- **RabbitMQ** – mensageria com exchange de pedidos
- **Serilog** – logging estruturado
- **FluentValidation** – validações de dados
- **Swagger** – documentação interativa da API
- **xUnit**, **FluentAssertions** – testes automatizados
- **Ocelot** – API Gateway leve e configurável

### Frontend
- **React 18**
- **Vite** – empacotador moderno
- **Tailwind CSS** – utilitários CSS
- **shadcn/ui** – biblioteca de UI acessível
- **Fetch API** – requisições HTTP

### Infraestrutura
- **Docker e Docker Compose**
- **SQL Server**, **MongoDB**, **Redis**, **RabbitMQ**
- **OpenTelemetry para observabilidade**, **Grafana**
- **Containers separados para APIs, Worker, Gateway e Frontend**

---

## 🧱 Arquitetura e Padrões

- **Clean Architecture**: separação por responsabilidades (Domain, Application, Infrastructure, API)
- **CQRS**: APIs distintas para comandos (`orders-command-api`) e queries (`orders-query-api`)
- **API Gateway**: uso de Ocelot para centralizar chamadas REST
- **Abstrações por interfaces** para serviços de cache, mensageria e repositórios
- **Cache distribuído** com Redis (read-through pattern)

---

## 📁 Estrutura do Projeto

```
src/
  ├── BuildingBlocks/               # Componentes compartilhados (cache, logs, messaging)
  ├── Gateway/                      # API Gateway com Ocelot
  ├── OrderService.CommandAPI/      # Escrita de pedidos (comando)
  ├── OrderService.QueryAPI/        # Leitura de pedidos (consulta)
  ├── OrderService.QueryAPI.Worker/ # Worker que consome RabbitMQ
frontend/
  └── order-management-system/      # Frontend React com Tailwind
infra/
  └── environment/                  # Docker Compose e configuração de ambiente
```

---

## 🧪 Testes Automatizados

- Testes unitários com `xUnit`, `FluentAssertions`, `FluentValidation.TestHelper`
- Cobertura para validadores e handlers de domínio
- Preparado para testes de integração com banco de dados e mensageria

---

## 🛠️ Como Executar o Projeto

### Pré-requisitos

- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

### Executar com um único comando

```bash
docker compose -f infra/environment/docker-compose.yml up -d
```

### Serviços disponíveis

| Serviço                 | Endereço                                   |
|-------------------------|--------------------------------------------|
| **SQL Server**          | `localhost:1433`                           |
| **MongoDB**             | `localhost:27017`                          |
| **Redis**               | `localhost:6379`                           |
| **RabbitMQ Dashboard**  | [http://localhost:15672](http://localhost:15672) |
| **API de Comando**      | [http://localhost:8080/swagger](http://localhost:8080/swagger) |
| **API de Consulta**     | [http://localhost:8081/swagger](http://localhost:8081/swagger) |
| **API Gateway**         | `http://localhost:8082`                    |
| **Frontend React**      | [http://localhost:3000](http://localhost:3000) |
| **Grafana**         | [http://localhost:3001](http://localhost:3001)  

---

## 📌 Observações Importantes

- O **Swagger** está habilitado nas APIs de comando e consulta para testes interativos.
- O **frontend** consome a API através do **gateway**.
- O **worker** lê mensagens de `RabbitMQ` e grava no MongoDB.
- O uso de Redis é feito via serviço de cache com abstração.
- Os dados de seed SQL são carregados automaticamente via `sql-init`.

---

## 🧠 Considerações Técnicas

Este projeto demonstra domínio técnico nas seguintes competências:

- Arquitetura moderna com foco em escalabilidade e desacoplamento
- Experiência prática com mensageria, cache e infraestrutura em containers
- Boas práticas de Clean Code, testes, versionamento e documentação

---

## 🔒 Variáveis de Ambiente

As variáveis sensíveis (como senhas e credenciais) devem ser definidas em um arquivo `.env` com os seguintes valores:

```env
DB_SA_PASSWORD=Str0ng@Pass123
ACCEPT_EULA=Y

MONGO_USER=root
MONGO_PASS=dev123456

RABBITMQ_USER=appuser
RABBITMQ_PASS=app123456
```

---

## ✅ Status: Pronto para Produção Local

Este projeto está apto para rodar 100% localmente com todas as funcionalidades e infraestrutura simuladas via Docker. Ideal para validação de arquitetura, testes e demonstrações técnicas.
