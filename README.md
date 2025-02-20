# **Projeto de Gerenciamento de Vendas**

# **Sales API** 📦💰

### **📌 Sobre o Projeto**  
Esta API gerencia registros de vendas, permitindo criar, consultar, atualizar e excluir vendas. Segue os princípios de **DDD (Domain-Driven Design)** e **Clean Architecture**, garantindo uma estrutura modular e escalável.

---

## **🚀 Tecnologias Utilizadas**
- **Linguagem:** C# (.NET 7 ou 8)  
- **Banco de Dados:** PostgreSQL + MongoDB  
- **Arquitetura:** DDD + Clean Architecture  
- **Logging:** Serilog  
- **Testes:** XUnit, Fluent Assertions, Bogus  
- **Containerização:** Docker  

---

## **📥 Instalação e Configuração**  

### **1️⃣ Clonar o repositório**  
```sh
git clone https://github.com/seu-usuario/sales-api.git
cd sales-api
```

### **2️⃣ Configurar Variáveis de Ambiente**  
Crie um arquivo `.env` e configure as credenciais do PostgreSQL e MongoDB:  
```env
POSTGRES_USER=postgresql_salesapi_user
POSTGRES_PASSWORD=senha123
POSTGRES_DB=postgresql_salesapi
MONGO_CONNECTION_STRING=mongodb+srv://usuario:senha@cluster.mongodb.net/salesapi
```

### **3️⃣ Subir os containers com Docker**  
```sh
docker-compose up -d
```

### **4️⃣ Rodar a API localmente**  
```sh
dotnet run --project src/SalesAPI
```

---

## **📌 Endpoints Principais**
### **Criar uma Venda**
```http
POST /api/vendas
```
**Request Body:**
```json
{
  "cliente": "João Silva",
  "filial": "Loja Centro",
  "produtos": [
    { "id": 1, "quantidade": 5, "precoUnitario": 100.00 }
  ]
}
```
**Response:**
```json
{
  "id": 1,
  "numeroVenda": "202402191234",
  "valorTotal": 450.00,
  "status": "Ativa"
}
```

---

## **🛠 Testes**
Rodar testes unitários e de integração:  
```sh
dotnet test

