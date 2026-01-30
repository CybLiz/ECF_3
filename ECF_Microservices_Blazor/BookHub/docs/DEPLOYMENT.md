# Deployment

## Prérequis

- .NET SDK 8
- Docker et Docker Compose
- PostgreSQL (via Docker)

## Lancement en local

1. Lancer la base de données :

docker compose up -d postgres

- postgres pour lancer uniquement le service PostgreSQL, pas encore les APIs.

- Permet de tester que la DB fonctionne avant de démarrer les services.

 Resultat:
- Container bookhub-postgres créé et en cours d’exécution.

- Volume persistant bookhub_postgres_data créé.

- Network bookhub-network créé.

Lancer un service :
cd src/Services/BookHub.LoanService
dotnet run

Swagger est disponible en mode développement pour tester les endpoints.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookHub Loan Service V1");
});


Étape 3 : Tester les endpoints avec Swagger

Accédez à l’URL Swagger pour chaque service :

CatalogService : http://localhost:5001/swagger

UserService : http://localhost:5002/swagger

LoanService : http://localhost:5000/swagger

