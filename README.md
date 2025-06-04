# 💰 Wallet System API

A simple wallet service designed for sports betting platforms to manage user funds (create wallets, deposit, withdraw, and query balance) following Clean Architecture principles and implemented as a .NET Framework 4.8 Web API.

---

## 📋 Description

This project implements a fault-tolerant and concurrent-safe wallet system for users on a betting platform. Users can deposit or withdraw funds via RESTful APIs, with safeguards against overspending and double-spending.

---

## ✅ Features

- Create a new wallet
- Add (deposit) funds to a wallet
- Remove (withdraw) funds from a wallet
- Query wallet balance
- Prevent negative balances
- Prevent duplicate transactions
- REST API with Swagger UI

---

## 🔧 Technology Stack

- .NET Framework 4.8
- ASP.NET Web API
- Entity Framework Core 7+ (InMemory provider)
- Clean Architecture (Domain, Application, Infrastructure, API layers)
- Swagger (Swashbuckle.WebApi)
- Logging (ConsoleLogger via Microsoft.Extensions.Logging)

---

## 🚀 Prerequisites

- Visual Studio 2022
- .NET Framework 4.8 SDK
- NuGet packages:
  - Microsoft.EntityFrameworkCore
  - Microsoft.EntityFrameworkCore.InMemory
  - Microsoft.Extensions.DependencyInjection
  - Microsoft.Extensions.Logging.Console
  - Swashbuckle.Core (for .NET Framework)

---

## 🏁 Steps to Run Locally

1. Clone https://github.com/arturbagdasaryan/WalletSystem.git
2. Open the .sln file in Visual Studio 2022.
3. Restore NuGet packages (right-click solution > Restore NuGet Packages).
4. Set the WebAPI project (e.g., WalletSystem.Api) as the startup project.
5. Press F5 or Ctrl+F5 to run.

Swagger will be available at: https://localhost:44312/swagger

## 🔌 API Endpoints

Base URL: http://localhost:44312/api/wallet

| Method | Endpoint      | Description                        | Body (JSON)                                                    |
|--------|---------------|------------------------------------|----------------------------------------------------------------|
| POST   | /create       | Creates a new wallet               | —                                                              |
| GET    | /{walletId}   | Returns wallet balance             | —                                                              |
| POST   | /add          | Deposits funds                     | { "walletId": "GUID", "amount": 100, "transactionId": "GUID" } |
| POST   | /remove       | Withdraws funds (if balance valid) | { "walletId": "GUID", "amount": 50, "transactionId": "GUID" }  |
