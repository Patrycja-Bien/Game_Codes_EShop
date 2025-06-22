
# Game Codes EShop

## Project Overview
Game Codes EShop is a comprehensive e-commerce platform designed to manage and sell game codes. The system is built using .NET Core and follows a microservices architecture to ensure scalability, maintainability, and ease of deployment.

## Features
- **Client Details**: Retrieve client information.
- **Products**: Full CRUD (Create, Read, Update, Delete) operations for products.
- **Product Categories**: Full CRUD operations for product categories.
- **Shopping Cart**: Full CRUD operations for the shopping cart.
- **User Authentication and Authorization**: JWT-based authentication, user registration, login, password reset, and role management.
- **Order Processing**: Manage and process orders.
- **Notifications**: Send notifications to users.
- **Invoices**: Generate and send invoices/receipts via email.

## Technologies Used
- **.NET Core**: Backend framework for building web APIs.
- **Entity Framework Core**: ORM for database access.
- **JWT**: Secure authentication and authorization.
- **Docker**: Containerization of services.
- **Kafka**: Message broker for asynchronous communication.
- **MediatR**: Implements the mediator pattern for CQRS.
- **Swagger**: API documentation and testing.
- **Redis**: In-memory data store for caching.
- **AutoMapper**: Object-object mapping.
- **xUnit**: Unit and integration testing framework.

## Microservices Architecture
The system is composed of several independently deployable services:
- **ProductCatalogueService**: Manages products and categories.
- **OrdersService**: Handles order processing.
- **ShoppingCartService**: Manages shopping cart operations.
- **UserService**: Manages user accounts, authentication, and roles.
- **NotificationService**: Sends notifications to users.

Each service has its own database and communicates with others via Kafka.
