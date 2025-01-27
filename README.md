# **Projeto de Gerenciamento de Vendas**

Este é um sistema para gerenciar vendas, com endpoints para criar, consultar, atualizar e deletar vendas, além de aplicar regras de negócio, como validação de descontos.

## **Requisitos**

Antes de começar, certifique-se de ter os seguintes requisitos instalados:

- [.NET SDK](https://dotnet.microsoft.com/download) (versão mínima 6.0 ou superior)
- [PostgreSQL](https://www.postgresql.org/download/)
- Ferramenta para gerenciar o banco de dados, como [pgAdmin](https://www.pgadmin.org/) ou [DBeaver](https://dbeaver.io/)
- [Git](https://git-scm.com/)

## **Como Baixar e Executar o Projeto**

### 1. Clone o Repositório
Abra o terminal ou o Git Bash e execute o comando:

```bash
git clone https://github.com/seu-usuario/nome-do-repositorio.git
```

Substitua `seu-usuario` e `nome-do-repositorio` pelo caminho correto do repositório.

### 2. Navegue até a Pasta do Projeto
```bash
cd nome-do-repositorio
```

### 3. Configure o Banco de Dados
1. Crie um banco de dados no PostgreSQL com o nome desejado, por exemplo: `vendasdb`.
2. Atualize a string de conexão no arquivo `appsettings.json`, localizado na pasta `src/API`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=vendasdb;Username=seu-usuario;Password=sua-senha"
}
```

3. Execute as migrations para criar as tabelas no banco de dados:
```bash
dotnet ef database update
```

### 4. Restaure as Dependências
No diretório do projeto, restaure as dependências com o seguinte comando:

```bash
dotnet restore
```

### 5. Execute o Projeto
Para iniciar o servidor, execute:

```bash
dotnet run --project src/API
```

O projeto estará disponível em [http://localhost:5000](http://localhost:5000).

### 6. Teste os Endpoints
- Acesse o Swagger em [http://localhost:5000/swagger](http://localhost:5000/swagger) para testar os endpoints diretamente na interface.
- Ou utilize uma ferramenta como o [Postman](https://www.postman.com/) para enviar requisições aos endpoints.

---

## **Principais Endpoints**

### **Vendas**
| Método | Endpoint          | Descrição                              |
|--------|-------------------|----------------------------------------|
| GET    | `/api/Vendas`     | Lista todas as vendas.                |
| POST   | `/api/Vendas`     | Cria uma nova venda.                  |
| GET    | `/api/Vendas/{id}`| Consulta uma venda pelo ID.           |
| PUT    | `/api/Vendas/{id}`| Atualiza uma venda existente.         |
| DELETE | `/api/Vendas/{id}`| Remove uma venda pelo ID.             |

---

## **Contribuindo**
1. Faça um fork do repositório.
2. Crie uma branch para sua feature: `git checkout -b minha-feature`.
3. Realize suas alterações e faça um commit: `git commit -m 'Minha nova feature'`.
4. Envie para seu fork: `git push origin minha-feature`.
5. Abra um Pull Request no repositório original.

---

## **Licença**
Este projeto está licenciado sob a [MIT License](LICENSE).

---

Se precisar de mais ajuda ou suporte, fique à vontade para abrir uma issue no repositório.
