-Diagramme d'architecture** (C4 Model - niveau Context et Container)

**Context**

┌─────────────────────────────────────────────────────────────┐
│                    Blazor WebAssembly                        │
│                    (Frontend Client)                         │
└─────────────────────┬───────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│                    API Gateway (Ocelot)                      │
│                    Port: 5000                                │
└────┬─────────────────┬─────────────────┬────────────────────┘
     │                 │                 │
     ▼                 ▼                 ▼
┌──────────┐    ┌──────────┐    ┌──────────┐    ┌──────────────┐
│ Catalog  │    │  User    │    │  Loan    │    │ Notification │
│ Service  │    │ Service  │    │ Service  │    │   Service    │
│ :5001    │    │ :5002    │    │ :5003    │    │   (TODO)     │
└────┬─────┘    └────┬─────┘    └────┬─────┘    └──────┬───────┘
     │               │               │                 │
     └───────────────┴───────────────┴─────────────────┘
                           │
                           ▼
              ┌────────────────────────┐
              │   PostgreSQL / RabbitMQ │
              └────────────────────────┘


**Container**

BookHub.BlazorClient
├── Pages
├── Components
└── Services (HttpClient vers ApiGateway)

BookHub.ApiGateway
├── Controllers -> Routes vers microservices
└── Infrastructure/HttpClients vers microservices

BookHub.CatalogService
├── Api/Controllers
├── Application/Services
├── Domain/Entities & Ports
└── Infrastructure/Persistence

BookHub.UserService
├── Api/Controllers
├── Application/Services
├── Domain/Entities & Ports
└── Infrastructure/Security & Persistence

BookHub.LoanService
├── Api/Controllers
├── Application/Services
├── Domain/Entities & Ports
└── Infrastructure/Persistence & HttpClients

BookHub.Shared
├── DTOs -> Partage des modèles de données
└── IntegrationEvents -> Événements pour communication inter-services
