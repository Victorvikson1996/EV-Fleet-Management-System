# EV Fleet Management System (C#/.NET)

This project implements an **Electric Vehicle (EV) Fleet Management System** using **C# and .NET 9**. It follows a **distributed, event-driven architecture**, featuring a **Service Registry, UDP Multicast Space, Event Bus, and Nodes** (such as vehicles and charging stations) with FSM (Finite State Machine), Drivers, Sensors, and Actuators.

## 📌 Architecture Diagram

![EV Fleet Management Architecture](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0c8AAAAASUVORK5CYII=)

> _Replace the placeholder with an actual encoded image or a valid URL to the architecture diagram._

## 🚀 Features

- **Service Registry**: Tracks and manages EVs, charging stations, and fleet nodes.
- **UDP Multicast Space**: Simulates real-time event broadcasting (WebSocketSharp is used as an alternative to UDP for reliability).
- **Event Bus**: Publishes and subscribes to events for real-time communication.
- **Nodes**: Implements `VehicleNode`, `ChargingStation`, and `FleetManager` with FSM-based state management.
- **Drivers, Sensors, and Actuators**: Simulates hardware interactions such as GPS, battery monitoring, movement, and charging.
- **Authentication**: Implements JWT-based authentication for fleet management access.
- **Data Storage**: Uses PostgreSQL with Entity Framework Core for persistent storage.

## 📋 Prerequisites

Ensure you have the following installed:

- [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- A compatible IDE: Visual Studio (Windows/Mac), JetBrains Rider, or VS Code with the C# extension

## 🛠 Installation

1. **Clone the repository** or create a new project:

   ```bash
   git clone https://github.com/Victorvikson1996/EV-Fleet-Management-System.git
   cd ev-fleet-management
   ```

2. **Install dependencies:**

   ```bash
   dotnet add package Microsoft.EntityFrameworkCore
   dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
   dotnet add package Stateless
   dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
   dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis
   dotnet add package WebSocketSharp
   ```

3. **Configure `appsettings.json`** with your PostgreSQL credentials and JWT settings.

## ▶️ Running the Project

1. **Ensure PostgreSQL is running:**

   ```bash
   brew install postgresql  # For macOS
   brew services start postgresql
   createdb ev_fleet
   ```

2. **Run the application:**

   ```bash
   dotnet run
   ```

   This starts the server on `https://localhost:5001`. Use the `/api/fleet` endpoints with a JWT token for authenticated access.

## 📡 Usage

- Access fleet status, drive vehicles, or charge vehicles via HTTP requests to `/api/fleet` endpoints.
- The system broadcasts events via WebSocket and updates the **Service Registry**.
- Example actions are triggered through the `FleetController`, such as starting a vehicle or sending it to charge.

## 🤝 Contributing

1. **Fork the repository**.
2. **Create a feature branch**.
3. **Submit a pull request** with tests and documentation.

## 📜 License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

## 💡 Acknowledgments

- Inspired by **EV fleet management architecture**.
- Uses `xstate` for FSM and `dgram` for UDP multicast simulation.

---

🔗 _For any issues or feature requests, please open an [issue](https://github.com/Victorvikson1996/EV-Fleet-Management-System/issues)._
