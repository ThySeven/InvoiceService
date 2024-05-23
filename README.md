# InvoiceService

InvoiceService is a microservice responsible for managing invoices within our Auction Core Services architecture. It handles tasks such as generating, sending, validating, and updating invoices. The service is designed to integrate seamlessly with other essential services including Mail Service, RabbitMQ, MongoDB, and external payment systems.

## Table of Contents

- [Setup](#setup)
- [Configuration](#configuration)
- [Architecture](#architecture)
- [Dependencies](#dependencies)
- [Endpoints](#endpoints)
- [Security](#security)
- [Monitoring & Logging](#monitoring--logging)
- [Continuous Deployment](#continuous-deployment)
- [License](#license)

## Setup

### Prerequisites

- Docker
- Docker Compose
- Nginx (for reverse proxy)
- HashiCorp Vault
- RabbitMQ
- Prometheus, Loki, and Grafana for monitoring

### Local Installation

1. Clone the repository:

    ```bash
    git clone https://github.com/yourusername/InvoiceService.git
    cd InvoiceService
    ```

2. Run the service using Docker Compose:

    ```bash
    docker-compose up --build
    ```

### Production Setup

For a production setup, ensure that you have configured Nginx, Vault, RabbitMQ, and monitoring systems accurately. You can deploy the service using your CI/CD pipeline.

## Configuration

### Environment Variables

- `LokiEndpoint`: Endpoint for Loki.
- `RabbitMQHostName`: Hostname for the RabbitMQ server.
- `VAULT_IP`: Address of the HashiCorp Vault service.
- `VAULT_SECRET`: Secret for accessing Vault.
- `MongoDBConnectionString`: Connection string for MongoDB.
- `GrafanaHostname`: Hostname for Grafana (default is `invoiceservice`).
- `RabbitMQQueueName`: Queue name for RabbitMQ (default is `MailQueue`).
- `PublicIP`: Public IP address (default is `http://localhost:3005`).
- `ASPNETCORE_URLS`: URL for the ASP.NET Core service (default is `http://+:3005`).

Rename the file to `.env` and fill in the necessary values.

## Architecture

InvoiceService follows a microservice architecture within the broader Auction Core Services ecosystem. It interacts closely with the following components:

- **Mail Service**: For sending invoice-related emails.
- **RabbitMQ**: For sending notifications and updates.
- **MongoDB**: For data persistence.
- **Nginx**: For managing API traffic and load balancing.

[Architecture Diagram](https://s.icepanel.io/mB4kr95xX1FRKO/cCvU)

## Dependencies

- MongoDB
- RabbitMQ
- HashiCorp Vault

## Endpoints

### API Endpoints

| Method | Endpoint                | Description                         |
|--------|-------------------------|-------------------------------------|
| GET    | /invoices               | Retrieve all invoices               |
| POST   | /invoices               | Create a new invoice                |
| GET    | /invoices/:id           | Get details of a specific invoice   |
| PUT    | /invoices/:id           | Update an existing invoice          |
| DELETE | /invoices/:id           | Delete a specific invoice           |
| POST   | /invoices/:id/validate  | Validate a specific invoice         |

## Security

InvoiceService utilizes HashiCorp Vault to manage secrets securely. Secrets, especially JWT secrets, are stored in Vault and fetched dynamically, ensuring that sensitive information is handled with care.

## Monitoring & Logging

InvoiceService is integrated with Prometheus, Loki, and Grafana for monitoring and logging. 

### Logging

Logs are sent to Loki via the `LokiEndpoint` for centralized logging.

### Metrics

Application metrics are exposed and can be collected by Prometheus. Grafana is used for visualizing these metrics.

## Continuous Deployment

The CI/CD pipeline configuration will handle automatic deployments via platforms such as Jenkins, GitHub Actions, or GitLab CI. Be sure to set your secrets and configurations in your respective CI/CD environment.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.
