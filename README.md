# PassR 🧭  
**A lightweight, flexible mediator library for .NET applications — designed with Clean Architecture in mind.**

---

## ✨ Overview

**PassR** is a minimal, fast, and extensible .NET library that enables clean separation of concerns using the Mediator pattern.  
Inspired by MediatR, PassR adds support for:

- ✅ Request/response handling (`IRequest`, `IRequestHandler`)
- ✅ Fire-and-forget notifications (`INotification`, `INotificationHandler`)
- ✅ Middleware pipeline behaviors (`IPipelineBehavior`)
- ✅ Clean and testable architecture, built on top of dependency injection
- ✅ Command & Query separation via `ICommand`, `IQuery` abstractions

---

## 📦 Installation

```bash
dotnet add package PassR
```

---

## 🛠 Setup

Register PassR in your `Program.cs`:

```csharp
builder.Services.AddPassR(options =>
{
    options.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly);
    options.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
```

---

## 📐 Basic Usage

### 1. Define a Request

```csharp
public record GetUserQuery(Guid UserId) : IQuery<UserDto>;
```

### 2. Create a Handler

```csharp
public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserDto>
{
    public async ValueTask<Result<UserDto>> HandleAsync(GetUserQuery request, CancellationToken cancellationToken)
    {
        // simulate user retrieval
        return Result.Success(new UserDto(request.UserId, "deniz@example.com"));
    }
}
```

### 3. Send the Request

```csharp
var result = await mediator.SendAsync(new GetUserQuery(Guid.NewGuid()));
```

---

## 🔁 Notification Handling

```csharp
public record UserCreated(Guid UserId) : INotification;

public class SendWelcomeEmailHandler : INotificationHandler<UserCreated>
{
    public ValueTask HandleAsync(UserCreated notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Sending email to user: {notification.UserId}");
        return ValueTask.CompletedTask;
    }
}
```

Publish with:

```csharp
await mediator.PublishAsync(new UserCreated(userId));
```

---

## 🧩 Pipeline Behaviors

```csharp
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async ValueTask<TResponse> HandleAsync(
        TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        Console.WriteLine($"[START] {typeof(TRequest).Name}");
        var response = await next();
        Console.WriteLine($"[END] {typeof(TRequest).Name}");
        return response;
    }
}
```

---

## 💡 Design Philosophy

- Separation of concerns
- Minimal magic, maximum flexibility
- Strong typing over string keys or attributes
- Developer-friendly — predictable structure, IDE-friendly IntelliSense

---

## 📚 Interfaces Overview

| Interface                     | Purpose                                    |
|------------------------------|--------------------------------------------|
| `IRequest<TResponse>`        | Represents a request with a response       |
| `IRequestHandler<,>`         | Handles a request                          |
| `INotification`              | Represents a fire-and-forget event         |
| `INotificationHandler<>`     | Handles a notification                     |
| `IPipelineBehavior<,>`       | Middleware logic around requests           |
| `ICommand`, `IQuery`         | CQRS-style abstractions                    |
| `Result`, `Error`, `ValidationError` | Functional result pattern           |

---

## 🧪 Example Projects

Check out `/examples/WebAPI` for how to:
- Wire up `PassR` into a minimal API project
- Use commands, queries, notifications
- Add pipeline logging
- Validate using a behavior layer

---

## 📄 License

MIT © [Burak Besli](https://github.com/bbesli)