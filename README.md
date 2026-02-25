# 🎟️ TicketAzure

Cloud-native ticket sales platform built with **.NET 9** following **Clean Architecture** principles and integrated with **Microsoft Azure services**.

The system allows users to purchase event tickets via PIX payment, using asynchronous processing and cloud-native components.

---

## 🚀 Tech Stack

- .NET 9
- ASP.NET Core Web API
- Clean Architecture
- Azure Cosmos DB
- Azure Blob Storage
- Azure Service Bus
- Background Worker Service
- External PIX Payment Gateway
- Repository Pattern
- Result Pattern
- Dependency Injection

---

## 🏗️ Architecture

The solution is structured into the following layers:

- **Core**
  - Entities
  - Enums
  - Interfaces
  - Domain rules

- **Application**
  - Business services
  - DTOs
  - Use cases
  - Result pattern for error handling

- **Infrastructure**
  - Azure integrations
  - Cosmos DB repositories
  - Blob Storage integration
  - Service Bus messaging
  - Payment gateway integration

- **API**
  - REST endpoints
  - Dependency injection setup
  - Request/response handling

- **Worker**
  - Background processing
  - Service Bus message consumption
  - Ticket generation workflow

---

## ☁️ Azure Services Used

- **Azure Cosmos DB**  
  NoSQL storage for events and payments.

- **Azure Blob Storage**  
  Storage for generated ticket files.

- **Azure Service Bus**  
  Asynchronous messaging between API and background worker.

- **Background Worker Service**  
  Processes payment events and handles ticket generation asynchronously.

---

## 💳 Payment Flow

1. Client requests ticket purchase.
2. API validates the event and creates the payment.
3. External PIX gateway generates the payment.
4. Payment is stored in Cosmos DB.
5. A message is published to Azure Service Bus.
6. Background worker processes the message.
7. Ticket is generated and stored in Azure Blob Storage.

---

## 📌 Key Features

- Asynchronous and event-driven architecture
- Cloud-native design
- Scalable NoSQL database
- Decoupled services via messaging
- Secure file storage using SAS tokens
- Clean and maintainable architecture

---

## 🔐 Configuration

Sensitive configurations such as:

- Cosmos DB connection string
- Service Bus connection string
- Blob Storage connection string
- Payment gateway credentials

Should be managed via environment variables or secure configuration providers.

---# TicketWaveAzure
Projeto de venda de tickets utilizando ferramentas do aicrosoft Azure, como CosmosDB, Blob Storage, Service Bus.
