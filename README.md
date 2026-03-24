# Employee Management API

API REST construida con **ASP.NET Core (.NET 10)** y **Entity Framework Core** para gestionar empleados, departamentos y designaciones de una empresa. Utiliza **SQL Server** como base de datos.

---

## ¿Qué hace?

La API expone endpoints para realizar operaciones CRUD sobre tres entidades principales:

| Entidad | Tabla SQL |
|---|---|
| Empleados | `employeeTbl` |
| Departamentos | `departmentTbl` |
| Designaciones | `designationTbl` |

---

## Endpoints disponibles

### Empleados — `api/EmployeeMaster`

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/EmployeeMaster` | Obtener todos los empleados |
| GET | `/api/EmployeeMaster/{id}` | Obtener empleado por ID |
| GET | `/api/EmployeeMaster/filter` | Filtrar, ordenar y paginar empleados |
| POST | `/api/EmployeeMaster` | Crear un nuevo empleado |
| PUT | `/api/EmployeeMaster/{id}` | Actualizar empleado existente |
| DELETE | `/api/EmployeeMaster/{id}` | Eliminar empleado |

### Departamentos — `api/DepartmentMaster`

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/DepartmentMaster/GetAllDepartments` | Obtener todos los departamentos |
| POST | `/api/DepartmentMaster/AddDepartment` | Agregar departamento |
| PUT | `/api/DepartmentMaster/UpdateDepartment` | Actualizar departamento |
| DELETE | `/api/DepartmentMaster/DeleteDepartment/{id}` | Eliminar departamento |

### Designaciones — `api/DesignationMaster`

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/DesignationMaster` | Obtener todas las designaciones |
| GET | `/api/DesignationMaster/{id}` | Obtener designación por ID |
| POST | `/api/DesignationMaster` | Crear designación |
| PUT | `/api/DesignationMaster/{id}` | Actualizar designación |
| DELETE | `/api/DesignationMaster/{id}` | Eliminar designación |

---

## Tecnologías utilizadas

- [.NET 10](https://dotnet.microsoft.com/)
- ASP.NET Core Web API
- Entity Framework Core 10 (SQL Server)
- SQL Server / SQL Server Express

---

## Requisitos previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server o SQL Server Express instalado localmente
- (Opcional) [SQL Server Management Studio](https://aka.ms/ssmsfullsetup) para gestionar la base de datos

---

## Configuración local

### 1. Clonar el repositorio

```bash
git clone https://github.com/GlendyT/EmployeeManagmentApp.git
cd Employee.api
```

### 2. Configurar la cadena de conexión

Edita el archivo `appsettings.json` y ajusta la cadena de conexión según tu instancia de SQL Server:

```json
"ConnectionStrings": {
  "empCon": "Server=localhost\\SQLEXPRESS;Database=employeeManageDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

> Si usas una instancia con nombre diferente o autenticación por usuario/contraseña, modifica `Server`, `User Id` y `Password` según corresponda.

### 3. Crear la base de datos con migraciones

```bash
dotnet ef database update
```

> Si no tienes la herramienta `dotnet-ef` instalada, ejecuta primero:
> ```bash
> dotnet tool install --global dotnet-ef
> ```

### 4. Ejecutar la API

```bash
dotnet run
```

La API estará disponible en:

```
http://localhost:{puerto}/api/...
```

El puerto exacto se puede ver en la consola al iniciar, o configurarlo en `Properties/launchSettings.json`.

---

## Ejemplo de uso

### Crear un empleado (POST `/api/EmployeeMaster`)

```json
{
  "name": "Juan Pérez",
  "contactNo": "1234567890",
  "email": "juan.perez@email.com",
  "city": "Bogotá",
  "state": "Cundinamarca",
  "pincode": "110111",
  "address": "Calle 123 #45-67",
  "designationId": 1,
  "designationName": "Developer",
  "role": "employee"
}
```

### Filtrar empleados (GET `/api/EmployeeMaster/filter`)

Parámetros query disponibles:

| Parámetro | Tipo | Descripción |
|---|---|---|
| `search` | string | Búsqueda por nombre o email |
| `designationId` | int | Filtrar por designación |
| `sortBy` | string | Campo de ordenamiento (default: `name`) |
| `sortDir` | string | Dirección: `asc` o `desc` |

---

## Estructura del proyecto

```
Employee.api/
├── Controllers/
│   ├── DepartmentMasterController.cs
│   ├── DesignationMasterController.cs
│   └── EmployeeMasterController.cs
├── Model/
│   ├── Department.cs
│   ├── Designation.cs
│   ├── EmployeeDbContext.cs
│   └── EmployeeModel.cs
├── Properties/
│   └── launchSettings.json
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
└── Employee.api.csproj
```
