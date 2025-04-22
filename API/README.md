# VOLT Transport System API

## Introduction

This project is a management system for a transport company, featuring a multi-user system with different interfaces and permissions for each user. The system can manage orders, accounts, branches, provinces, and cities, and includes various features to facilitate the daily work of company employees, agents, and merchants.

## Purpose

The purpose of this system is to provide a unified platform to efficiently manage all company operations, from receiving orders and distributing them to agents to delivering them to customers, with detailed and accurate reporting for different users.

## Technology Used

- **Backend**: ASP.NET Core Web API
- **Database**: SQL Server
- **Frontend**: Angular
- **Authentication**: JWT
- **Development Tools**: Visual Studio, Postman

## Users and Roles

### 1. Admin
- Has all permissions.
- Add, edit, and delete merchants, employees, agents, branches, cities, and provinces.
- Modify permissions for each role.

### 2. Employee
- Accept orders and distribute them to agents based on the province.
- Adjust prices based on weight and shipping rates set for the merchant.

### 3. Merchant
- Add and edit orders.

### 4. Agent
- Update the status of orders (delivered, postponed, rejected, etc.).

## Running the System

To run the system locally:

1. **Clone the repository:**

    ```sh
    git clone https://github.com/Ahmed3llam/Volt-Api.git
    ```

2. **Update the database:**

    ```sh
    update-database
    ```


## Future Features

- Automated notifications (SignalR)
- Adding detailed reports for order operations.
- Multi-language support.
- Enhancing the user interface for a better user experience.
- Integration with multiple payment systems.

## Conclusion

This transport company management system is designed to streamline and improve the daily operations of the company, providing a unified interface for different users and saving time and effort while increasing efficiency and accuracy in operations.
