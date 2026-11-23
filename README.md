# Business Management Dashboard

A full-stack business management application developed using **ASP.NET Core MVC (.NET 10), C#, Entity Framework Core, and SQL Server**.

## Project Overview

Business Management Dashboard is a web application designed to manage customer records, track work orders, and visualize business performance.

The application demonstrates full-stack .NET development, relational database design, server-side data processing, and interactive reporting.

## Features

### Customer Management
- Create, view, update, and delete customer records
- Search customers by name or email
- Server-side pagination
- Prevent deletion of customers with existing work orders

### Work Order Management
- Create and assign work orders to customers
- Track Open, In Progress, and Completed statuses
- Record service descriptions and pricing
- Search by customer or service
- Filter work orders by status
- Server-side pagination
- Track work order completion dates

### Business Dashboard
- Live customer statistics
- Open and completed work order counts
- Total completed work order value
- Monthly completed work order value chart
- Recent work orders overview

### Database Features
- Entity Framework Core with SQL Server
- One-to-many customer/work order relationships
- Code-first database migrations
- Development database seeding with fictional demonstration records
- Asynchronous database operations

## Technology Stack

| Technology | Purpose |
|---|---|
| C# | Backend development |
| ASP.NET Core MVC (.NET 9) | Web application framework |
| Razor Views | Server-rendered user interface |
| Entity Framework Core | Database access |
| SQL Server | Relational database |
| Bootstrap | Responsive layout |
| Chart.js | Interactive reporting |
| JavaScript | Client-side functionality |
| Git and GitHub | Version control |

## Application Screenshots

### Business Dashboard

![Business Dashboard](screenshots/dashboard.png)

### Customer Management

![Customer Management](screenshots/customers.png)

### Work Order Management

![Work Order Management](screenshots/workorders.png)

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server or SQL Server LocalDB
- Visual Studio or another compatible development environment


### Installation

1. Clone the repository.
2. Navigate to the MVC project directory.
3. Configure the `DefaultConnection` connection string in your local development settings.
4. Restore dependencies:

   `dotnet restore`

5. Install the EF Core CLI tool if needed, using a version compatible with the project's EF Core packages.
6. Apply database migrations:

   `dotnet ef database update`

7. Run the application:

   `dotnet run`

### Demonstration Data

When running in the Development environment against an empty database, the application automatically seeds five fictional customers and ten work orders.

The sample records include different work order statuses, prices, and completion dates to demonstrate the dashboard's reporting functionality.

Existing databases containing customer or work order records are not overwritten by the seeder.

## Architecture

The application follows the Model-View-Controller architectural pattern.

- **Models:** Define customer and work order entities and dashboard view models.
- **Views:** Provide Razor-based user interfaces.
- **Controllers:** Handle HTTP requests, validation, and application workflows.
- **Data:** Contains the Entity Framework Core database context and development seeder.
- **Migrations:** Maintain the database schema.

## Project Purpose

This application was developed as a professional portfolio project demonstrating practical experience with ASP.NET Core MVC, C#, SQL Server, Entity Framework Core, database relationships, and business application development.

The project uses fictional demonstration data and does not contain proprietary employer code.

## Future Enhancements

- User authentication and authorization
- Additional business reporting
- Improved validation and error handling
- Automated testing
- Cloud deployment

## License

No license has been specified. All rights are reserved unless a license is added to the repository.
