# Invoicing API

This API provides endpoints to manage companies, users, and invoices. It supports creating invoices and retrieving sent/received invoices with simple token-based authentication.

## Features

- **Companies & Invoices Management**: Create and view invoices sent or received.
- **Token-based Authentication**: Restricted endpoints require a valid token in the `Authorization` header.
- **Swagger UI**: Easily explore and test the API endpoints.
  
**Example Tokens**:  
- `SoftOne`: `token_softone`  
- `edrink`: `token_edrink`

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker & Docker Compose](https://docs.docker.com/get-docker/)

### Running Locally

1. **Run without Docker**:
    ```
   cd Api
   dotnet run http
    ```
   Swagger UI will be available at: [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)

2. **Run with Docker**:
    ```
    cd Docker
    .\docker-compose-debug.cmd
    ```
    After containers start, the API will be available at [http://localhost:8000/swagger/index.html](http://localhost:8000/swagger/index.html)

### Authentication

Include one of the provided tokens in your request headers:

```
Authorization: token_softone
```

or

```
Authorization: token_edrink
```

### Endpoints

- **POST /invoice**: Create a new invoice (requires token).
- **GET /invoice/sent**: Retrieve invoices sent by authenticated company.
- **GET /invoice/received**: Retrieve invoices received by authenticated company.

Use Swagger UI for detailed documentation and request examples.

## Testing

A basic test suite is included. After installing dependencies, run:

```bash
cd Tests
dotnet test
```
