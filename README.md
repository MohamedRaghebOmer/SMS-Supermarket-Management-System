# 🛒 SMS - Supermarket Management System

### Enterprise Backend Architecture

A highly robust, scalable, and relentlessly secure Backend API meticulously engineered to handle complex Supermarket, Inventory, and Financial Ledger operations.

> This is not a standard CRUD application.
>
> It is the result of over **three months of intensive architectural planning and backend development**.
>
> The project strictly adheres to **Clean Architecture**, shifts business logic away from controllers, and implements enterprise-grade database security, dynamic Resource-Based Authorization, and a comprehensive Audit Logging pipeline.

---
# 🎥 Project Overview & Deep Dive

> 🎬 **Coming Soon:** A comprehensive YouTube walkthrough covering the system architecture, implementation details, business logic, and database design.

---

# 🔥 Engineering Highlights

This project goes far beyond basic framework usage by implementing advanced software engineering principles focused on:

- Performance
- Security
- Scalability
- Maintainability
- Observability
- Clean Separation of Concerns

---

## 1️⃣ Impenetrable Database Architecture & Performance

### 📜 The 14,000-Line Database Script

The entire database structure—including:

- Tables
- Constraints
- Indexes
- Views
- Triggers
- Stored Procedures

is contained inside a single:

```text
Database-Script.sql
```

located at the repository root.

---

### 🛡️ Schema-Level Security (`app` vs `dbo`)

The application never directly accesses business tables.

Instead:

- Direct querying of business tables is restricted.
- The application connects exclusively through a dedicated `app` schema.
- All operations are executed through predefined Stored Procedures.

This approach provides:

- Better security
- Controlled access
- Centralized business rules
- Easier auditing

---

### ⚡ 100% Stored Procedures + ADO.NET

Unlike many applications that rely heavily on ORMs:

- No inline SQL queries
- No Entity Framework
- No direct table manipulation

Every database operation is performed through highly optimized Stored Procedures using:

```text
ADO.NET
```

Benefits:

- Maximum performance
- Better execution plan reuse
- Reduced attack surface
- Stronger control over business logic
- SQL Injection protection

---

## 2️⃣ Dynamic Resource-Based Authorization (RBAC)

Roles are not hardcoded strings.

The system implements a flexible permission matrix powered by:

### Roles

- Admin
- Manager
- Cashier
- Inventory Manager

And any future role can be created dynamically through dedicated endpoints.

---

### Permissions Engine

Permissions are mapped dynamically using:

```text
RoleEntityPermissions
```

and

```text
SystemEntities
```

This allows granular control over:

- Read
- Create
- Update
- Delete

permissions for every resource in the system.

---

## 3️⃣ Ultra-Thin Controllers & CQRS

Using:

- CQRS
- Service Layer Abstractions
- Middleware Pipelines
- Clean Architecture

more than **99% of controllers contain only a few lines of code**.

Controllers are responsible only for:

- Receiving HTTP requests
- Returning HTTP responses

while all business logic is delegated to the Application layer.

---

# 🛡️ Security & Middleware Pipeline

Security and traceability are deeply integrated into the architecture.

---

## AuditLoggingMiddleware

A highly intelligent auditing system.

### Standard Users

Logs sensitive operations such as:

- Login
- Refresh Token
- Create
- Update
- Delete

### Admin Users

The system follows a zero-trust philosophy.

Every single action performed by an Admin is audited regardless of sensitivity.

---

## ExceptionHandlingMiddleware

Provides centralized exception handling.

Responsibilities:

- Catch unhandled exceptions
- Log failures securely
- Return standardized API responses
- Hide internal implementation details

No sensitive stack traces are exposed to clients.

---

## CorrelationIdMiddleware

Injects a unique Correlation ID into every request.

Benefits:

- Easier debugging
- Request tracing
- Log correlation
- Transaction tracking

---

## Authentication & Authorization

Implemented using:

- JWT Bearer Authentication
- Refresh Tokens
- Token Revocation
- Role-Based Authorization
- Resource-Based Authorization
- Policies

---

## Rate Limiting & HTTPS

The API is protected through:

- HTTPS Enforcement
- Sliding Rate Limiting

to mitigate:

- Brute-force attacks
- Abuse
- Excessive requests

---

## Environment-Based Configuration

Sensitive configuration values are never hardcoded.

Examples:

- Database Connection Strings
- JWT Security Keys

are loaded exclusively from:

```text
Environment Variables
```

---

# ⚙️ Core Domain Capabilities

---

## 👥 People, Customers & Access Control

### People Management

Stores:

- National IDs
- Personal Information
- Contact Details
- Nationality Data
- Images

---

### Customer Ledger System

Supports advanced financial tracking through:

```text
CustomerLedger
```

Features include:

- Credit Sales
- Grace Periods
- Minimum Payment Rules
- Dynamic Credit Limits
- Overdue Customer Blocking

---

### User Management

Provides:

- Secure Account Management
- Password Hashing
- Role Assignment
- Login Tracking

---

## 📦 Inventory & Product Lifecycle

### Product Catalog

Supports:

- SKU Management
- Categories
- Units
- Product Images
- Cost Price
- Sell Price
- Discounts

---

### Stock Control Engine

Powered by:

```text
ProductStock
```

Features:

- Real-Time Inventory Tracking
- Decimal Quantity Support
- Reorder Levels
- Stock Monitoring

---

### Flexible Unit System

Supports:

- Count-Based Items
- Weight-Based Products
- Volume-Based Products

through configurable Units.

---

## 💳 Sales, Financials & Returns

### Sales Processing

Handles:

- Sales
- Sale Items
- Discounts
- Change Calculations
- Payment Tracking

---

### Returns Management

Supports:

- Full Return Lifecycle
- Inventory Restocking
- Return History
- Financial Recalculations

based on the exact unit price at the time of sale.

---

# 🏗️ Architectural Blueprint

The solution follows a strict Clean Architecture implementation.

---

## SMS.API

Presentation Layer

Contains:

- Controllers
- Authorization
- Middleware
- Configurations
- File Storage

---

## SMS.Application

Business Logic Layer

Contains:

- CQRS Handlers
- Services
- Interfaces
- Validation
- Mapping

---

## SMS.Contracts

Communication Layer

Contains:

- Request DTOs
- Response DTOs

---

## SMS.Domain

Core Business Layer

Contains:

- Entities
- Enums
- Domain Rules

---

## SMS.Infrastructure

Infrastructure Layer

Contains:

- ADO.NET Implementations
- Repositories
- Data Access Logic

---

## SMS.Shared

Shared Utilities Layer

Contains:

- Constants
- Guards
- Shared Enums
- Common Utilities

---

# 📂 Project Structure

```text
+---SMS.API
|   +---Authorization
|   |   +---Handlers
|   |   \---Requirements
|   +---Configurations
|   +---Controllers
|   +---CustomAttributes
|   +---Helpers
|   +---Interfaces
|   +---Middlewares
|   +---Properties
|   \---Storage
|       +---people
|       \---products

+---SMS.Application
|   +---Common
|   +---Exceptions
|   +---Helpers
|   +---Interfaces
|   +---Mapping
|   \---Services

+---SMS.Contracts
|   +---Requests
|   \---Responses

+---SMS.Domain
|   \---Entities

+---SMS.Infrastructure
|   +---Data
|   +---Helpers
|   \---Repositories

\---SMS.Shared
    +---Common
    +---Constants
    +---Enums
    \---Guards
```

---

# 📄 API Standards & Optimizations

## High-Performance Data Retrieval

More than **90%** of endpoints that return collections support:

- Pagination
- Sorting
- Filtering
- Searching

performed directly at the database level.

---

## Optimized File Storage

To avoid database bloat:

- Product Images are stored on disk.
- Person Images are stored on disk.

Only metadata is stored within the database.

---

### Standardized File Response

```csharp
namespace SMS.Contracts.Responses
{
    public sealed record FileResponse
    {
        public byte[] Bytes { get; init; } = [];
        public string FileExtension { get; init; } = string.Empty;
        public string ContentType { get; init; } = string.Empty;
    }
}
```

---

# 🚀 Future Roadmap

## 🎨 Frontend Application

Develop a modern frontend using:

- React
- Angular
- Blazor

---

## 📊 Advanced Structured Logging

Integrate:

- Serilog
- NLog

with platforms such as:

- Seq
- Elasticsearch
- Logstash
- Kibana

---

## ⚡ Distributed Caching

Introduce Redis for caching:

- Countries
- System Settings
- Frequently Accessed Data

---

## 🐳 Containerization & DevOps

Planned additions:

- Docker
- Docker Compose
- CI/CD Pipelines
- Cloud Deployment

---

# ❤️ Final Note

This repository represents significantly more than a CRUD API.

It demonstrates:

- Clean Architecture
- CQRS
- Advanced Authorization
- Enterprise Database Design
- Audit Logging
- Secure Authentication
- High-Performance Data Access
- Production-Oriented Engineering Practices

---

### Built with passion, caffeine, and strict adherence to Clean Code principles. ☕
