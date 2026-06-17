# 📚 BookStore Management System (Backend - .NET Core)

An advanced yet clean E-Commerce Backend System built for online bookstores. The project focuses on solid software design principles, secure transaction workflows, and accurate inventory tracking.

---

## 🏗️ Project Architecture & Structure

The system uses a structured **Layered Architecture Style** mapped out into three decoupled projects within the Visual Studio Solution. This logical division makes the code easy to maintain and prepares the infrastructure to scale as a **3-Tier Architecture** (multi-server deployment) in the future:

1. **Presentation Layer (`API`):** Manages secure RESTful Endpoints, JWT authentication, and standard JSON responses.
2. **Business Logic Layer (`BLL`):** Executes all core application logic, business rules, and state validations.
3. **Data Access Layer (`DAL`):** Handles communication with **SQL Server** using Entity Framework Core.

---

## 🛠️ Design Patterns Implemented

To maintain flexibility and follow industry standards, the backend integrates the following patterns:

* **Repository & Unit of Work Patterns:** Decouples data access from the business logic and centralizes database operations into unified transactions.
* **Strategy & Factory Patterns:** Dynamically resolves and handles payment processing behaviors (e.g., distinguishing between Cash and Visa logic at runtime).

---

## 🔄 Core Business Workflows

### 🛒 Atomicity & Stock Integrity
The system uses strict database **Transactions (All-or-Nothing)** during checkouts:
* Real-time stock is verified, and book inventory is instantly deducted upon ordering.
* If the payment process fails, a complete **Rollback** is executed to protect inventory data accuracy.

### 📦 Order Lifecycle Management
Orders move through a protected state pipeline to avoid operational errors:
`Pending` ➔ `Confirmed` ➔ `Shipped` ➔ `Delivered` ➔ `Cancelled` 

### 💳 Financial Refunds & Ledger
Canceling or returning an order automatically executes a safe workflow: it updates the transaction record to `Refunded`, changes the order status to `Cancelled`, and securely re-stocks the exact book quantities back to the store inventory.

---

## 🔐 Security & Role-Based Authorization

Endpoints are guarded by **JWT Authentication** and split into two distinct operational roles:

* **User Role:** Authorized to check out from the cart, execute single-book quick purchases, track personal orders, and cancel pending requests.
* **Admin Role:** Authorized to manage the bookstore lifecycle, including confirming pending orders, updating shipping/delivery tracks, executing financial refunds, and auditing overall system logs.