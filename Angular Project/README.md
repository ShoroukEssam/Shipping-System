# Volt - Transport System Frontend

## Introduction

Volt is the front-end application for the Transport Company System. It provides a user-friendly interface for administrators, employees, merchants, and agents to manage orders, accounts, branches, provinces, and cities efficiently.

## Purpose

The purpose of Volt is to offer a seamless and intuitive user experience for managing transport company operations. It aims to facilitate tasks such as order management, user management, and data visualization through an interactive dashboard.

## Technology Used

- **Framework**: Angular
- **UI Components**: Angular Material
- **HTTP Client**: HttpClientModule
- **Routing**: Angular Router
- **Development Tools**: Visual Studio Code, Node.js, npm

## Users and Roles

### 1. Admin
- Full access to all functionalities.
- Add, edit, and delete merchants, employees, agents, branches, cities, and provinces.
- Modify permissions for each role.

### 2. Employee
- Accept orders and assign them to agents based on the province.
- Adjust prices based on weight and shipping rates set for the merchant.

### 3. Merchant
- Add and edit orders.

### 4. Agent
- Update the status of orders (delivered, postponed, rejected, etc.).

## Running the System

To run Volt locally:

1. **Clone the repository:**

    ```sh
    https://github.com/Ahmed3llam/Volt-Angular.git
    ```

2. **Navigate to the project directory:**

    ```sh
    cd volt
    ```

3. **Install dependencies:**

    ```sh
    npm install
    ```

4. **Run the development server:**

    ```sh
    ng serve -o
    ```

5. **Access the application:**

    Open your browser and navigate to `http://localhost:4200`.

## Features

- **Dashboard**: Interactive dashboard for merchants and agents to view order statuses.
- **Order Management**: Add, edit, and update order statuses.
- **User Management**: Manage users with different roles and permissions.
- **Search and Filter**: Search and filter orders by various criteria such as agent name, order status, customer name, and date range.
- **Data Management**: Manage branches, cities, provinces, and user data with search functionality.

## Future Features

- Adding detailed analytics and reports.
- Multi-language support.
- Enhanced user interface for better user experience.
- Integration with additional third-party services.

## Conclusion

Volt is designed to provide a robust and intuitive interface for managing transport company operations. By leveraging Angular's powerful features and a well-organized structure, it ensures a smooth and efficient workflow for all users.
