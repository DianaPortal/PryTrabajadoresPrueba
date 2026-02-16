# PryTrabajadoresPrueba


Proyecto para el Mantenimiento de Trabajadores, desarrollado en .NET 8.


# Sistema de Mantenimiento de Trabajadores 

![.NET Core](https://img.shields.io/badge/.NET%20Core-8.0-purple) ![SQL Server](https://img.shields.io/badge/SQL%20Server-Stored%20Procedures-red) ![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-green) ![Testing](https://img.shields.io/badge/Testing-xUnit-blue)

Este proyecto implementa un módulo CRUD completo para la gestión de trabajadores, utilizando **Clean Architecture**, **Database First** y buenas prácticas.

##  Características Principales


Requerimientos funcionales y técnicos:

* **Arquitectura Limpia (Clean Architecture):** Separación estricta de responsabilidades (Domain, Application, Infrastructure, Web).
* **Database First & Stored Procedures:** Toda la lógica de persistencia (Listado, Insertar, Actualizar, Eliminar) se maneja a través de procedimientos almacenados en SQL Server.
* **Frontend Interactivo:** Uso de Modales con **AJAX** y **jQuery** para una experiencia de usuario fluida (SPA feeling) sin recargas innecesarias.


### Implementados 

* **Unit Testing:** Pruebas unitarias implementadas con **xUnit** y **Moq** para validar reglas de negocio críticas.
* **UX Visual:** Filas coloreadas dinámicamente (Azul: Masculino / Naranja: Femenino).
* **Gestión de Archivos:** Integración con API externa (**ImgBB**) para almacenamiento de fotos, manteniendo el servidor *stateless*.
* **Paginación y Filtros:** Búsqueda por nombre y sexo con paginación optimizada.

---

## Arquitectura del Proyecto

La solución sigue el patrón de **Clean Architecture** para garantizar mantenibilidad y testabilidad:

1.  **📂 PryTrabajadoresPrueba.Domain**
    * *Núcleo del sistema.* Contiene las entidades (`Trabajador`). No tiene dependencias externas.
2.  **📂 PryTrabajadoresPrueba.Application**
    * *Capa de Lógica.* Contiene los Servicios (`TrabajadorService`), Interfaces (`ITrabajadorRepository`, `IFotoService`) y DTOs.
3.  **📂 PryTrabajadoresPrueba.Infrastructure**
    * *Acceso a Datos.* Implementa los repositorios usando **Entity Framework Core** (Database First) y servicios externos (`FotoService`).
4.  **📂 PryTrabajadoresPrueba.Web**
    * *Capa de Presentación.* Aplicación ASP.NET Core MVC. Contiene Controladores, Vistas Razor y scripts JS.
5.  **🧪 PryTrabajadoresPrueba.Tests**
    * *Aseguramiento de Calidad.* Pruebas unitarias para validar la lógica de la capa Application.

---

## Tecnologías Utilizadas

* **Backend:** .NET 8 (C#), Entity Framework Core.
* **Base de Datos:** SQL Server.
* **Frontend:** ASP.NET Core MVC, Bootstrap 5, jQuery.
* **Librerías Adicionales:**
    * `SweetAlert2` (Alertas interactivas).
    * `FontAwesome` (Iconografía).
    * `Moq` & `xUnit` (Pruebas).

---

## Configuración e Instalación

### 1. Base de Datos
Ejecutar el script `Script_BD_Trabajadores.sql` (adjunto en la carpeta `Docs/`) en tu instancia de SQL Server para crear la base de datos `TrabajadoresPrueba`, las tablas y los Stored Procedures.

### 2. Configuración de Conexión
Abre el archivo `appsettings.json` en el proyecto `PryTrabajadoresPrueba.Web` y actualiza la cadena de conexión:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=TrabajadoresPrueba;Trusted_Connection=True;TrustServerCertificate=True;"
}