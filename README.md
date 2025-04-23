# Clean Architecture API

A .NET 8 Web API project following Clean Architecture principles.

## Project Structure

The solution is organized into the following projects:

- **CleanArchitecture.API**: The entry point of the application, containing controllers and API configurations
- **CleanArchitecture.Application**: Contains business logic, interfaces, and DTOs
- **CleanArchitecture.Domain**: Contains core business entities and interfaces
- **CleanArchitecture.Infrastructure**: Contains implementations of interfaces and external services

## Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or Visual Studio Code
- Git

## Getting Started

1. Clone the repository:
   ```bash
   git clone [repository-url]
   ```

2. Navigate to the project directory:
   ```bash
   cd CleanArchitecture
   ```

3. Restore dependencies:
   ```bash
   dotnet restore
   ```

4. Run the application:
   ```bash
   dotnet run --project CleanArchitecture.API
   ```

## API Documentation

The API documentation is available through Swagger UI at:
```
http://localhost:5000/swagger
```

### Available Endpoints

#### Weather Forecast
- **GET** `/weatherforecast`
  - Description: Retrieves weather forecast for the next 5 days
  - Response: Array of `WeatherForecast` objects
  - Content-Type: application/json

### WeatherForecast Model
```json
{
  "date": "2024-04-10",
  "temperatureC": 25,
  "temperatureF": 77,
  "summary": "Warm"
}
```

## Development

### Project Configuration

- The API runs on port 5000 by default
- Swagger documentation is enabled in Development environment
- XML documentation is enabled for better API documentation

### Dependencies

- Microsoft.AspNetCore.OpenApi (8.0.14)
- Swashbuckle.AspNetCore (6.6.2)

## Clean Architecture Principles

This project follows Clean Architecture principles:

1. **Dependency Rule**: Dependencies point inward
2. **Separation of Concerns**: Each layer has a specific responsibility
3. **Independence of Frameworks**: Core business logic is independent of frameworks
4. **Testability**: Each layer can be tested independently

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

- API Support: support@example.com
- Project Link: [repository-url]

## Acknowledgments

- Clean Architecture by Robert C. Martin
- ASP.NET Core Documentation
- Swagger/OpenAPI Documentation 