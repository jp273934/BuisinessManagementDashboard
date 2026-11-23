Business Management Dashboard

A full-stack business management web application built with ASP.NET Core MVC (.NET 10), C#, Entity Framework Core 10, and SQL Server.

This portfolio project demonstrates the development of a secure, database-driven application for managing customers, tracking work orders, and analyzing business performance.

Key Features

Dashboard and Reporting

Live statistics for total customers, open work orders, completed work orders, and completed revenue.

Interactive six-month revenue chart using Chart.js.

Recent work orders with status indicators.

Responsive dashboard built with Bootstrap.

Customer Management

Create, view, edit, and delete customer records.

Search customers and navigate paginated results.

Maintain relationships between customers and work orders.

Prevent deletion of customers with associated work orders.

Work Order Management

Create and manage work orders associated with customers.

Track Open, In Progress, and Completed statuses.

Automatically record completion dates.

Search, filter, and paginate work orders.

Authentication and Security

ASP.NET Core Identity authentication.

Secure login and logout.

Password hashing and configurable password requirements.

Administrator role-based authorization.

Protected customer and work order management.

Restricted public registration.

Navigation links displayed according to user permissions.

Public read-only dashboard.

Technology Stack

Component

Technology

Programming Language

C#

Framework

ASP.NET Core MVC (.NET 10 LTS)

Database

SQL Server LocalDB

Data Access

Entity Framework Core 10

Authentication

ASP.NET Core Identity

Frontend

Razor Views, HTML, CSS, Bootstrap

Reporting

Chart.js

Development Environment

Visual Studio 2026

Version Control

Git and GitHub

Application Architecture

The application follows the Model-View-Controller (MVC) architectural pattern.

Models: Define customers, work orders, and business data.

Views: Provide responsive user interfaces using Razor and Bootstrap.

Controllers: Handle application requests, business operations, and authorization.

Entity Framework Core: Provides database access, relationships, and migrations.

ASP.NET Core Identity: Manages user authentication and role-based authorization.

Security Design

The application separates public dashboard access from administrator-only management functionality.

Visitors can view demonstration statistics and reporting information without signing in.

Administrator accounts can access customer and work order management. Non-administrator users cannot access these protected areas.

Public account registration is disabled through a custom Identity registration-page override.

Development-only database seeding and administrator role assignment are restricted to the Development environment.

Running the Application Locally

Prerequisites

.NET 10 SDK

Visual Studio 2026 with ASP.NET and web development support

SQL Server LocalDB

Setup

Clone the repository.

Open the solution in Visual Studio 2026.

Configure a local SQL Server connection string.

Restore NuGet packages.

Apply Entity Framework Core migrations using Update-Database or dotnet ef database update.

Run the application.

The Development environment includes fictional demonstration data for testing and portfolio purposes.

Administrator access requires a separately provisioned account.

Screenshots

Add screenshots of the following application features:

Dashboard overview

Revenue chart

Customer management

Work order management

Authentication and authorization

Development Highlights

This project demonstrates practical experience with:

Full-stack C# web development

Relational database design

Entity Framework Core migrations

CRUD functionality

Authentication and authorization

Secure role-based access

Data visualization

Responsive web design

Git-based version control

Future Enhancements

Dedicated production demonstration database

Enhanced dashboard reporting

Exportable reports

Automated testing

Cloud deployment when hosting becomes available

Project Status

Functional local portfolio demonstration.

The application runs locally and is maintained in GitHub. Public hosting is planned for a future release.