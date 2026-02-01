# Architecture

## Vue d’ensemble

Le projet BookHub est basé sur une architecture microservices avec séparation des responsabilités.  
Chaque service est développé en .NET et communique via des APIs REST.

Les services principaux sont :
- CatalogService
- UserService
- LoanService
- API Gateway
- Web (frontend)

## Architecture hexagonale

Chaque microservice suit une architecture hexagonale organisée autour de :

- Domain : logique métier et entités
- Application : cas d’usage
- Infrastructure : accès aux données et clients HTTP
- Api : endpoints REST
- Migrations : gestion des migrations
- Tests : unitaires et intégration

Cette structure facilite tests, maintenance et évolutivité.

## Patterns utilisés

- Hexagonal Architecture
- Repository pour la persistance
- Shared DTOs et Integration Events pour communication inter-services
- Dependency Injection pour modularité et testabilité

## Dépendances entre services

- CatalogService et UserService : autonomes  
- LoanService : dépend de CatalogService et UserService  
- API Gateway : communique avec tous les services  
- Web : communique uniquement avec API Gateway

## Choix techniques

- .NET 8 : moderne et performant  
- Blazor WebAssembly : UI côté client intégrée au backend  
- ApiGateway : point d’entrée unique pour sécurité et routage  
- Integration Events : communication asynchrone et découplée  
- Shared Library : contrats et modèles communs  
- Docker Compose : orchestration simple pour le déploiement
