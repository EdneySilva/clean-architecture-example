# Clean Architecture Vault App

## Introduction
In the development of the Clean Architecture Vault App, I placed a strong focus on meeting the project's requirements while adhering to the principles of clean architecture. Each iteration was guided by user stories, which allowed for the addition of features and continuous improvement of the architecture.

## Architectural Overview
The application is designed with a clear separation of concerns and is divided into the following layers:

1. **Presentation Layer (Endpoints)**
2. **Domain Layer (Business Logic)**
3. **Application Layer (Orchestrations) - .NET Library Project**
4. **Infrastructure Layer (Data Access Layer) - .NET Library Project**

Now, let's delve into each layer in more detail.

## Domain Layer - Business Logic
This layer, represented as a .NET Library Project, serves as the core of the application. It houses the following components:
- **Core Entities**: Entities such as "User" and "Secret."
- **Commands**: Definitions for commands like "CreateSecretCommand," each accompanied by its respective validator.
- **Events**: Event definitions like "SecretCreated," "SecretDeleted," and others.
- **Queries**: Definitions for queries like "UserSecretBySecretNameQuery" and "UserSecretsQuery."
- **Validators**: Validators for commands like "CreateSecretValidator" and "AuthenticateUserValidator."
- Core structures like "Result" and "Metadata" for sharing additional domain object information.

This layer encapsulates the application's core business logic and validation rules.

## Application Layer - Orchestrations
In this .NET Library Project, we find adapters responsible for orchestrating domain validations and applying various business flows. Key components include:
- **Handlers**: Command and event handlers, e.g., "CreateSecretCommandHandler," "AuthenticationUserCommandHandler."
- **Event Handlers**: Handlers such as "SecretHasher."
- **Behaviours**: Components like "ValidationBehaviour."
- Services required by adapters, like "SecretHasher."

The Application Layer manages the execution of use cases, ensuring validations and flows are applied correctly.

## Infrastructure Layer - Data Access Layer
In this .NET Library Project, adapters are responsible for handling data access and interacting with external storage systems, such as a SQL Server Database accessed via Dapper. Key components include:
- **Handlers**: Command and query handlers, e.g., "RegisterUserCommandHandler," "UserQueryCommandHandler."
- Abstractions of commands to perform on storage, such as "CreateUserSecretSqlQuery," "UpdateSecretSqlQuery."

This layer provides a level of abstraction for interacting with storage systems, making it possible to change the internal repository (e.g., SqlDapperRepository) without impacting other layers.

## Presentation Layer - Endpoints
The ASP.NET API Project serves as the Presentation Layer. It implements endpoints that can be used by client applications. This layer adheres to REST protocols and dispatches commands (ports) to be processed by the respective adapters. Key components include:
- Controllers like "SecretsController" and "UsersController."

The Presentation Layer handles HTTP requests and translates results into appropriate HTTP responses.

## Development Approach
- Features were developed incrementally via pull requests (PRs).
- Unit tests were developed for each layer, focusing on the core design.
- End-to-end tests were implemented in the API layer to demonstrate real-world usage.

## Mediator Pattern
The project extensively utilizes the Mediator Pattern to decouple abstractions from implementations. This choice simplifies the orchestration of flows and reduces dependencies between layers, even at the reference project level.

Placing storage at the end of the pipeline ensures that all rules are applied in the correct layers, without dependencies on data access or other infrastructure capabilities. This approach enhances flexibility and maintainability.

Additionally, the project demonstrates event propagation, enabling the sending of notifications or data to be materialized in a different storage, audited, or sent via webhooks.

In conclusion, the Clean Architecture Vault App embodies clean architecture principles, offering flexibility, testability, and maintainability. It leverages the Mediator Pattern to decouple and streamline application flows while enabling event-driven functionality and easy adaptation to changing requirements.
