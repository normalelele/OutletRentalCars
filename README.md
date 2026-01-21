## Sistema de búsqueda y reserva de vehículos

Sistema de búsqueda y reserva de vehículos para outlet By miles CAR RENTAL.

---

## Descripción de la Solución

Este proyecto implementa una **Web API RESTful** que permite:

- **Búsqueda de vehículos disponibles** según localidad, fechas y mercado
- **Creación de reservas** con validación de disponibilidad
- **Gestión de eventos de dominio** (patrón Domain Events)
- **Separación de bases de datos** (transaccional vs catálogo)

## Reglas de Negocio Implementadas

1. **Disponibilidad por Localidad**: Los vehículos deben estar en la localidad de recogida
2. **Disponibilidad por Mercado**: Solo se retornan vehículos habilitados para el país de la localidad
3. **Validación de Fechas**: No se permiten reservas que coincidan con reservas existentes
4. **Estado del Vehículo**: Solo se retornan vehículos con estado "Disponible"

---

## Arquitectura

El proyecto sigue **Clean Architecture** con las siguientes capas:

```
OutletRentalCars/
│
├── OutletRentalCars.Domain/          # Núcleo del negocio
│   ├── Entities/                     # Vehicle, Reservation, Location, Market
│   ├── ValueObjects/                 # DateRange (lógica de solapamiento)
│   ├── Events/                       # VehicleReservedEvent
│   ├── Enums/                        # VehicleStatus
│   └── Interfaces/                   # Contratos de repositorios
│
├── OutletRentalCars.Application/     # Casos de uso (CQRS)
│   ├── Queries/                      # SearchVehiclesQuery + Handler
│   ├── Commands/                     # CreateReservationCommand + Handler
│   ├── DTOs/                         # Objetos de transferencia
│   └── EventHandlers/                # VehicleReservedEventHandler
│
├── OutletRentalCars.Infrastructure/  # Implementaciones técnicas
│   ├── Persistence/                  # EF Core + Repositories (SQL)
│   ├── MongoDB/                      # MongoDB Context + Repository
│   └── Seed/                         # Data inicial
│
├── OutletRentalCars.API/             # Capa de presentación
│   ├── Controllers/                  # VehiclesController, ReservationsController
│   └── Program.cs                    # Configuración e Inyección de Dependencias
│
└── OutletRentalCars.Tests/           # Pruebas
    ├── UnitTests/                    # Tests de dominio y aplicación
    └── IntegrationTests/             # Tests de endpoints
```

## Flujo de Dependencias (Clean Architecture)

```
API → Application → Domain
        ↑
  Infrastructure
```

- **Domain**: No depende de nadie
- **Application**: Solo depende de Domain
- **Infrastructure**: Implementa interfaces de Application
- **API**: Todo usando Dependency Injection

---

## Decisiones Técnicas

## 1. **Bases de Datos In-Memory**
**Decisión**: Usar `EF Core InMemory` + `MongoDB` en lugar de instancias reales.

**Razón**:
- Facilita la ejecución sin instalaciones adicionales
- El evaluador puede ejecutar con `dotnet run` sin configurar nada
- Permite enfocarse en la arquitectura y lógica de negocio
- Tests más rápidos y aislados

**Alternativa para producción**: MySQL + MongoDB reales (configuración lista en código).

---

## 2. **CQRS con MediatR**
**Decisión**: Separar Queries (lectura) de Commands (escritura) usando MediatR.

**Razón**:
- **Separation of Concerns** (principio SOLID - S)
- Facilita testing (handlers aislados)
- Escalabilidad futura (fácil agregar nuevos casos de uso)
- Código más limpio y mantenible

---

## 3. **Value Objects**
**Decisión**: Implementar `DateRange` como Value Object con lógica de solapamiento.

**Razón**:
-  Encapsula regla de negocio crítica (solapamiento de fechas)
-  Reutilizable en todo el dominio
-  Testeable de forma aislada
-  Evita duplicación de lógica (principio DRY)

---

## 4. **Domain Events (In-Memory)**
**Decisión**: Generar `VehicleReservedEvent` al crear una reserva.

**Razón**:
- Desacopla la creación de reserva de acciones secundarias
- Facilita agregar nuevas funcionalidades sin modificar código existente (Open/Closed)

---

## 5. **Repository Pattern**
**Decisión**: Interfaces en Domain, implementaciones en Infrastructure.

**Razón**:
- **Dependency Inversion** (principio SOLID - D)
- Facilita testing con mocks
- Permite cambiar persistencia sin afectar lógica de negocio
- Separación clara de responsabilidades

---

## 6. **Separación SQL/NoSQL**
**Decisión**: MySQL (EF Core) para datos transaccionales, MongoDB para catálogos.

**Razón**:
- **Vehicles & Reservations** (SQL): Requieren transacciones y relaciones
- **Markets** (MongoDB): Datos de configuración sin relaciones
- Escalabilidad según tipo de dato

---

## Cómo Ejecutar el Proyecto

## Prerrequisitos

- **.NET 8 SDK** (o .NET 7 mínimo)
- **Visual Studio 2022**
- **Git**

## Paso 1: Clonar el repositorio

```bash
git clone https://github.com/TU_USUARIO/OutletRentalCars.git
cd OutletRentalCars
```

## Paso 2: Restaurar dependencias

```bash
dotnet restore
```

## Paso 3: Ejecutar la aplicación

```bash
cd OutletRentalCars.API
dotnet run
```

O desde Visual Studio:
- Establecer `OutletRentalCars.API` como proyecto de inicio
- Presionar `F5`

## Paso 4: Acceder a Swagger

Una vez iniciada la aplicación, abre tu navegador en:

```
https://localhost:5001
```

Swagger UI se cargará automáticamente con la documentación interactiva de la API.

---

## Ejemplos de Uso

## 1vBuscar Vehículos Disponibles

**Endpoint**: `GET /api/vehicles/search`

**Parámetros**:
```
pickupLocationId: 1
returnLocationId: 2
pickupDateTime: 2026-02-01T10:00:00
returnDateTime: 2026-02-05T10:00:00
```

**Ejemplo con cURL**:
```bash
curl -X GET "https://localhost:5001/api/vehicles/search?pickupLocationId=1&returnLocationId=2&pickupDateTime=2026-02-01T10:00:00&returnDateTime=2026-02-05T10:00:00"
```

**Respuesta exitosa** (200 OK):
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "brand": "Toyota",
      "model": "Corolla",
      "year": 2023,
      "licensePlate": "ABC123",
      "status": "Available",
      "marketCode": "CO",
      "location": {
        "id": 1,
        "name": "Bogotá Airport",
        "city": "Bogotá",
        "countryCode": "CO"
      }
    }
  ],
  "count": 1
}
```

---

## 2️ Crear una Reserva

**Endpoint**: `POST /api/reservations`

**Body** (JSON):
```json
{
  "vehicleId": 1,
  "pickupLocationId": 1,
  "returnLocationId": 2,
  "pickupDateTime": "2026-02-01T10:00:00",
  "returnDateTime": "2026-02-05T10:00:00",
  "customerName": "John Doe",
  "customerEmail": "john.doe@example.com"
}
```

**Ejemplo con cURL**:
```bash
curl -X POST "https://localhost:5001/api/reservations" \
  -H "Content-Type: application/json" \
  -d '{
    "vehicleId": 1,
    "pickupLocationId": 1,
    "returnLocationId": 2,
    "pickupDateTime": "2026-02-01T10:00:00",
    "returnDateTime": "2026-02-05T10:00:00",
    "customerName": "John Doe",
    "customerEmail": "john.doe@example.com"
  }'
```

**Respuesta exitosa** (201 Created):
```json
{
  "success": true,
  "message": "Reservation created successfully",
  "data": {
    "id": 1,
    "vehicleId": 1,
    "pickupLocationId": 1,
    "returnLocationId": 2,
    "pickupDateTime": "2026-02-01T10:00:00",
    "returnDateTime": "2026-02-05T10:00:00",
    "customerName": "John Doe",
    "customerEmail": "john.doe@example.com",
    "isActive": true
  }
}
```

**Evento generado** (consola):
```
[EVENT] Vehicle 1 reserved from 2026-02-01 10:00:00 to 2026-02-05 10:00:00
```

---

## Ejecutar Tests

## Todos los tests
```bash
dotnet test
```

## Solo Unit Tests
```bash
dotnet test --filter "FullyQualifiedName~UnitTests"
```

## Solo Integration Tests
```bash
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

## Cobertura de tests
- **Unit Tests**: 16 tests (reglas de negocio del dominio)
- **Integration Tests**: 4 tests (endpoint de búsqueda)
- **Total**: 20+ tests

---

## Datos de Prueba (Seed Data)

## Locations (SQL)
| ID | Name              | City      | Country |
|----|-------------------|-----------|---------|
| 1  | Bogotá Airport    | Bogotá    | CO      |
| 2  | Medellín Downtown | Medellín  | CO      |
| 3  | Cartagena Beach   | Cartagena | CO      |
| 4  | Miami Airport     | Miami     | US      |
| 5  | New York JFK      | New York  | US      |

## Vehicles (SQL)
| ID | Brand     | Model    | Year | License Plate | Location | Market |
|----|-----------|----------|------|---------------|----------|--------|
| 1  | Toyota    | Corolla  | 2023 | ABC123        | 1        | CO     |
| 2  | Chevrolet | Spark    | 2024 | DEF456        | 1        | CO     |
| 3  | Mazda     | CX-5     | 2023 | GHI789        | 2        | CO     |
| 4  | Renault   | Logan    | 2024 | JKL012        | 2        | CO     |
| 5  | Ford      | Escape   | 2023 | MNO345        | 3        | CO     |
| 6  | Honda     | Civic    | 2024 | PQR678        | 4        | US     |
| 7  | Tesla     | Model 3  | 2024 | STU901        | 4        | US     |
| 8  | BMW       | X5       | 2023 | VWX234        | 5        | US     |

## Markets (MongoDB)
| Code | Name          | Active |
|------|---------------|--------|
| CO   | Colombia      | true   |
| US   | United States | true   |
| MX   | Mexico        | true   |
| BR   | Brazil        | false  |

---

## Principios SOLID Aplicados

- **S (Single Responsibility)**: Cada clase tiene una única razón para cambiar
  - `Vehicle` solo maneja lógica de vehículos
  - `DateRange` solo valida rangos de fechas
  - Handlers separados por caso de uso

- **O (Open/Closed)**: Abierto a extensión, cerrado a modificación
  - Fácil agregar nuevos handlers sin modificar existentes
  - Domain Events permiten agregar funcionalidad sin cambiar código

- **L (Liskov Substitution)**: Las implementaciones pueden sustituirse sin romper el código
  - Repositorios implementan interfaces correctamente
  - Mocks en tests funcionan igual que implementaciones reales

- **I (Interface Segregation)**: Interfaces específicas, no genéricas
  - `IVehicleRepository`, `IReservationRepository` separados
  - Cada interfaz con métodos específicos a su dominio

- **D (Dependency Inversion)**: Dependencias hacia abstracciones
  - Handlers dependen de `IRepository`, no de implementaciones concretas
  - Domain no conoce Infrastructure

---

## Patrones de Diseño Utilizados

- **Repository Pattern**: Abstracción de persistencia
- **CQRS**: Separación comando/consulta
- **Domain Events**: Desacoplamiento de efectos secundarios
- **Value Objects**: Encapsulación de lógica de validación
- **Dependency Injection**: Inversión de control
- **Unit of Work**: Implícito en EF Core DbContext

---

## Autor

**Jaime Nuñez**

---

## Licencia

Este proyecto fue desarrollado como prueba técnica.

---

Gracias por la oportunidad de participar en este proceso de selección. Este proyecto demuestra mi capacidad para:

Diseñar arquitecturas limpias y escalables  
Aplicar principios SOLID y patrones de diseño  
Implementar CQRS y Domain Events  
Escribir código testeable y mantenible  
Trabajar con múltiples tecnologías (SQL, NoSQL, .NET)  
Entregar soluciones funcionales bajo presión de tiempo