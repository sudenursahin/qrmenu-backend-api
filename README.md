# QRMenu

QRMenu is a QR-based remote ordering system developed for restaurants and cafes. It is built with modern software development principles and follows a clean, maintainable multi-layered architecture using .NET Core RESTful Web API.

## Features

- Multi-layered architecture (Business, DataAccess, Domain, Core, etc.)
- RESTful API structure
- Custom-built authentication system (no built-in Identity)
- Role-based authorization (Admin, User, etc.)
- Repository and Unit of Work patterns
- Clean and scalable codebase
- Built with .NET Core framework

## Project Layers

| Layer         | Description |
|---------------|-------------|
| Business      | Contains business logic and service implementations |
| DataAccess    | Handles database operations and EF Core context |
| Domain        | Includes entity models and shared types |
| CoreL         | Infrastructure, utilities, and custom result models |
| QRMenu.API    | API layer with controllers and endpoints |

## Authentication & Authorization

The project includes a fully custom-built authentication and authorization system:

- JWT-based token generation
- Login and logout endpoints
- Role-based access control

## Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- MS SQL Server
- JWT (JSON Web Tokens)
- AutoMapper
- RESTful service structure
- Layered software architecture

## Getting Started

```bash
git clone https://github.com/sudenursahin/qrmenu-backend-api.git
cd qrmenu-backend-api
