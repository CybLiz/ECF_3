# BookHub - Deployment


## Prérequis
.NET SDK 8, Docker, Docker Compose et PostgreSQL.

# Lancer la base de données PostgreSQL
docker compose up -d postgres

# Tester un service individuellement, par exemple LoanService
# Le service démarre et expose ses endpoints REST. Swagger est disponible pour tester les API.
cd src/Services/BookHub.LoanService
dotnet run

 Swagger dans Startup.cs :
 app.UseSwagger();
 app.UseSwaggerUI(c =>
 {
     c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookHub Loan Service V1");
 });


# Lancer l’API Gateway
Elle centralise la sécurité, le logging et le routage des requêtes vers les microservices.

cd src/Services/BookHub.ApiGateway
dotnet run

# Tester les endpoints via Swagger
 CatalogService : http://localhost:5001/swagger
 UserService : http://localhost:5002/swagger
 LoanService : http://localhost:5000/swagger

 puis API Gateway : http://localhost:5265/swagger

# Lancer le frontend Blazor pour accéder à l’application côté client
cd src/Web/BookHub.BlazorClient
dotnet run

# Lancer l’API Gateway et le frontend en une seule commande via Docker Compose
docker-compose up --build

# L’application complète est accessible via : http://localhost:5000

# Arrêter tous les containers lancés par Docker Compose
docker-compose down

# Pour supprimer également les volumes persistants et repartir avec une base propre
docker-compose down -v
