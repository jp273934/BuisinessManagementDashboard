# Business Management Dashboard

A full-stack business management web application developed using **ASP.NET Core MVC (.NET 10 LTS), C#, Entity Framework Core 10, and SQL Server**.

This portfolio project demonstrates the development of a secure, database-driven application for managing customers, tracking work orders, and analyzing business performance.

## Key Features

### Dashboard and Reporting
- Real-time statistics displaying total customers, open work orders, completed work orders, and completed revenue.
- Interactive six-month revenue chart powered by Chart.js.
- Recent work orders displayed with status indicators.
- Responsive dashboard layout built with Bootstrap.

### Customer Management
- Create, view, edit, and delete customer records.
- Search customers and navigate paginated results.
- Manage relationships between customers and work orders.
- Prevent deletion of customers with associated work orders.

### Work Order Management
- Create, view, edit, and delete work orders.
- Associate work orders with individual customers.
- Track Open, In Progress, and Completed statuses.
- Automatically record completion dates.
- Search, filter, and paginate work orders.

### Authentication and Security
- ASP.NET Core Identity authentication.
- Secure user login and logout.
- Password hashing and configurable password requirements.
- Administrator role-based authorization.
- Protected customer and work order management.
- Public registration disabled.
- Role-based navigation links.
- Public read-only dashboard.

## Technology Stack

| Component | Technology |
|---|---|
| Programming Language | C# |
| Framework | ASP.NET Core MVC (.NET 10 LTS) |
| Database | SQL Server LocalDB |
| Object-Relational Mapper | Entity Framework Core 10 |
| Authentication | ASP.NET Core Identity |
| Frontend | Razor Views, HTML, CSS, Bootstrap |
| Data Visualization | Chart.js |
| Development Environment | Visual Studio 2026 |
| Version Control | Git and GitHub |

## Application Architecture

The application follows the **Model-View-Controller (MVC)** architectural pattern.

- **Models:** Define customers, work orders, and business data.
- **Views:** Provide responsive interfaces using Razor and Bootstrap.
- **Controllers:** Handle HTTP requests, business operations, and authorization.
- **Entity Framework Core:** Manages database access, relationships, and migrations.
- **ASP.NET Core Identity:** Provides authentication and role-based authorization.

## Security Design

The application separates public dashboard access from administrator-only management functionality.

**Public visitors**
- Can view dashboard statistics and reporting information.
- Cannot access customer or work order management.
- Cannot register new accounts through the standard registration endpoint.

**Administrators**
- Can securely sign in and out.
- Can create, update, and delete customer records.
- Can manage work orders and their statuses.
- Can access protected management functionality.

Development-only database seeding and administrator role assignment are restricted to the Development environment.

## Running the Application Locally

### Prerequisites
- .NET 10 SDK
- Visual Studio 2026 with ASP.NET and web development support
- SQL Server LocalDB

### Setup Instructions

1. Clone the GitHub repository.
2. Open the solution in Visual Studio 2026.
3. Configure the local SQL Server connection string in `appsettings.Development.json`.
4. Restore NuGet packages.
5. Apply Entity Framework Core migrations using `Update-Database` in Package Manager Console.
6. Build and run the application.

The Development environment includes fictional demonstration data for testing and portfolio purposes.

Administrator access requires a separately provisioned account.

**Security note:** Local development settings and credentials should never be committed to the repository.

## Application Screenshots

Screenshots can demonstrate the following features:

- **Dashboard Overview:** Business statistics and recent work orders.
- **Revenue Reporting:** Interactive six-month revenue chart.
- **Customer Management:** Searchable customer records and CRUD operations.
- **Work Order Management:** Status tracking, filtering, and editing.
- **Authentication:** Login and administrator-only functionality.

## Development Highlights

This project demonstrates practical experience with:

- Full-stack C# and ASP.NET Core development
- MVC application architecture
- Relational database design
- Entity Framework Core migrations
- CRUD operations and data validation
- User authentication and role-based authorization
- Secure access to management functionality
- Search, filtering, and pagination
- Interactive data visualization
- Responsive web design
- Git version control

## Future Enhancements

- Dedicated production demonstration database
- Additional dashboard reporting and analytics
- Exportable reports
- Automated unit and integration testing
- Cloud deployment when hosting becomes available

## Project Status

**Functional local portfolio demonstration**

The application is developed and tested locally using Visual Studio 2026 and SQL Server LocalDB.

Source code is maintained in GitHub. Public hosting is planned for a future release.