# Solace .NET Pub/Sub + Queue Demo

This project demonstrates how to use the [Solace PubSub+](https://solace.com/products/event-broker/software/) event broker in a .NET 8+ ASP.NET Core application. It includes:
- A topic subscriber (`SolaceSubscriberService`) for direct messaging
- A queue subscriber (`SolaceQueueSubscriberService`) for guaranteed messaging

---

## 🔧 Prerequisites

- [.NET 8 SDK or later](https://dotnet.microsoft.com/en-us/download)
- [Docker](https://www.docker.com/)
- Solace PubSub+ Docker image (used locally)

---

## 🚀 Running Solace with Docker

Start a local Solace broker using Docker (Mac):

```bash
docker run -d --name=solace \
  -p 8080:8080 -p 55554:55555 -p 8008:8008 -p 1883:1883 -p 5672:5672 -p 9000:9000 \
  -e username_admin_globalaccesslevel=admin \
  -e username_admin_password=admin \
  --shm-size=1g \
  solace/solace-pubsub-standard
```
For Windows update the port as `-p 55555:55555`.  

Access the Solace Admin UI at [http://localhost:8080](http://localhost:8080)  
Default credentials: `admin / admin`  
Use the **default** Message VPN.

---

## 🧪 Running the App

```bash
dotnet run
```

This will:
- Subscribe to `tryme/topic` and `tryme2/topic` (direct messages)
- Bind to a queue named `demo.queue` for guaranteed messages

> Note: Ensure `demo.queue` exists in Solace UI and has topic subscriptions (e.g., `demo/topic`).

---

## 💡 Try It Out

### From Solace Try Me UI:
- Go to the **Try Me** tab in the Solace Admin UI
- Use the **Publisher** to send messages to:
  - `tryme/topic`
  - `tryme2/topic`
  - `demo/topic` (if mapped to `demo.queue`)

Messages will appear in the terminal as they're received.

---

## 📂 Project Structure

- `SolaceSubscriberService.cs` — listens to multiple topics
- `SolaceQueueSubscriberService.cs` — binds to a queue and acknowledges messages
- `Program.cs` — registers both services in a hosted ASP.NET Core app

---

## 📝 Notes

- This demo uses **direct messaging** for topics and **client-acknowledged flows** for queues.
- Make sure to **acknowledge queue messages** properly, or they will be redelivered.

---

## 📚 Resources

- [.NET Solace SDK Docs](https://tutorials.solace.dev/dotnet/)
- [Solace PubSub+ Docs](https://docs.solace.com/)
