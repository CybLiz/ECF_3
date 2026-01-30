# Architecture

## Vue d’ensemble

Le projet BookHub est basé sur une architecture microservices avec une séparation des responsabilités.
Chaque service est développé en .NET et communique via des APIs REST.

Les services principaux sont :
- CatalogService
- UserService
- LoanService
- BlazorClient (frontend)
- API Gateway à mettre en place !

## Architecture hexagonale

Chaque microservice suit une architecture hexagonale (Ports & Adapters) organisée autour de :

- Domain : logique métier et entités
- Application : cas d’usage et services applicatifs
- Infrastructure : accès aux données, clients HTTP, implémentations techniques
- Api : exposition des endpoints REST
- Migrations : gestion des migrations de base de données
- Tests : tests unitaires et d’intégration

Cette architecture permet une meilleure testabilité de chaque microservice, et donc une meilleure maintenabilité et évolutivité.
