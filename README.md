Conciliadora – Sistema de Gestão de Estacionamento
Sistema simples para controle de clientes, veículos, mensalistas, faturamento e vagas de estacionamento.

⚙️ Tecnologias e Arquitetura
.NET 8 + ASP.NET Core: Base do backend e APIs REST.

PostgreSQL: Banco de dados relacional escolhido pela robustez e compatibilidade com EF Core.

Entity Framework Core: ORM escolhido por integração nativa com o .NET e facilidade na criação de migrations.

Clean-ish architecture: Separação entre camadas de domínio, infraestrutura e WebApi.

DTOs: Criação de objetos de transporte para evitar vazamento de entidades.

Swagger (Swashbuckle): Documentação automática da API, acessível via /api-docs.

PM2 + Nginx (produção): Gerenciamento de processo e proxy reverso para publicação Linux.

📁 Estrutura
Domain: Entidades de domínio (Cliente, Veiculo, Mensalista, Vaga, etc).

Infrastructure: Repositórios, contexto do banco (DbContext) e configurações de EF Core.

WebApi: Controllers, DTOs e configuração do pipeline da aplicação.

Migrations: Geração e controle de versões do banco via EF Core.

💡 Decisões Técnicas
Relacionamentos entre entidades modelados com EF Core (1:N, 1:1).

Campos como Ativo padronizados com valor default true via .HasDefaultValue(true).

Utilização de Include para carregamento explícito de dados relacionados.

Evitada a referência circular no JSON usando [JsonIgnore].

Controle básico de erros com try/catch e return Problem(...) nos controllers.

Upload via CSV implementado para facilitar cadastros em massa de clientes e veículos.

API pensada para ser RESTful, com métodos claros e bem definidos (GET, POST, PUT, etc).

🚀 Como rodar localmente
Pré-requisitos: .NET 8 SDK, PostgreSQL, Visual Studio/Rider/VSCode

Clonar o projeto

bash
Copy
Edit
git clone https://github.com/VitorSantosH/Conciliadora.git
cd Conciliadora
Configurar conexão
Edite appsettings.json com a string de conexão do PostgreSQL:

json
Copy
Edit
"ConnectionStrings": {
  "Default": "Host=localhost;Port=5432;Database=ConciliadoraDb;Username=postgres;Password=12345"
}
Executar as migrations

bash
Copy
Edit
dotnet ef database update
Rodar a aplicação

bash
Copy
Edit
dotnet run --project TesteConciliadora
Acessar Swagger
http://localhost:5000/api-docs

📌 Observações
Sistema aceita criação e gerenciamento de mensalistas com simulação de faturamento.

Repositórios utilizam GenericRepository para padronizar operações básicas.

🌐 Ambiente de Teste
Subi o projeto em um servidor próprio para facilitar os testes e validação da API:

🔗 https://testeconciliadora.vitorwebdev.com.br/api-docs/index.html

