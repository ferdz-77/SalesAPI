README.md - SalesAPI
SalesAPI
A SalesAPI é uma API desenvolvida com .NET 8.0, focada na gestão de vendas e integração com bancos de dados relacionais e NoSQL. O projeto utiliza o Entity Framework Core para interagir com o banco de dados PostgreSQL e MongoDB para armazenamento de dados não relacionais.

Tecnologias Usadas
.NET 8.0: Framework para desenvolvimento da API.
Entity Framework Core: ORM para comunicação com o banco de dados PostgreSQL.
MongoDB: Banco de dados NoSQL para armazenar informações relacionadas a eventos e logs.
Automapper: Mapeamento de objetos entre DTOs e entidades.
MediatR: Implementação do padrão Mediator para separação de preocupações.
xUnit: Framework de testes unitários.
NSubstitute: Framework para criação de mocks nos testes.
Configuração do Ambiente
1. Clone o Repositório
Clone o repositório para sua máquina local:

bash
Copiar
Editar
git clone <URL_DO_SEU_REPOSITORIO>
2. Instale as Dependências
Navegue até o diretório do projeto e instale as dependências usando o comando:

bash
Copiar
Editar
dotnet restore
3. Configuração do Banco de Dados
PostgreSQL
Adicione as configurações de conexão com o banco PostgreSQL no arquivo appsettings.json:

json
Copiar
Editar
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Database=salesdb;Username=postgres;Password=yourpassword"
  }
}
MongoDB
Adicione as configurações de conexão com o banco MongoDB no arquivo appsettings.json:

json
Copiar
Editar
{
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017/salesdb"
  }
}
4. Execute a Aplicação
Para rodar a API localmente, execute o comando:

bash
Copiar
Editar
dotnet run
A aplicação estará disponível em http://localhost:5000.

Funcionalidades
Gerenciamento de Vendas: Criar, atualizar e cancelar vendas.
Eventos de Vendas: Publicar eventos quando uma venda é criada, modificada ou cancelada.
Logs de Atividades: Armazenar eventos de vendas no MongoDB para auditoria e histórico.
Testes
Execute os testes utilizando o comando:

bash
Copiar
Editar
dotnet test
