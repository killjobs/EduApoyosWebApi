# EduApoyos
## Descripción
Sistema para la gestión y seguimiento de solicitudes de apoyo estudiantil, permitiendo la creación, consulta, actualización y control del flujo de estados de cada solicitud.

---

## Arquitectura BackEnd
La solución se encuentra estructurada utilizando una arquitectura por capas:
- EduApoyosWebApi
- EduApoyosApplication
- EduApoyosInfrastructure
- EduApoyosDomain
- EduApoyosCommon
- EduApoyosTest

### Patrones de Diseño Aplicados
#### Repository Pattern
Se implementó el patrón Repository mediante interfaces y repositorios concretos para abstraer el acceso a datos.
Ejemplos:
- ISolicitudRepository
- SolicitudRepository
- IAuthRepository
- AuthRepository
#### Justificación
Permite desacoplar la lógica de negocio de Entity Framework Core, facilitando: Modificaciones posteriores, creación y ejecución de pruebas unitarias

---
#### Dependency Injection
Se utilizó Inyección de Dependencias para resolver las dependencias entre capas.
Ejemplos:
- ISolicitudApplication
- IAuthApplication
- ISolicitudRepository
- IJwtService
- IPasswordService
##### Justificación
Permite reducir el acoplamiento y facilita la definición de mocks para la realización de pruebas unitarias.

---
#### Service Pattern
La lógica de negocio fue centralizada en la capa Application.
Ejemplos:
- SolicitudApplication
- AuthApplication
##### Justificación
Permite encapsular reglas de negocio y mantener los controladores enfocados únicamente en la exposición de endpoints.

## Ejecución Manual
1. Crear la base de datos.
2. Configurar las variables de entorno o User Secrets.
3. Ejecutar las migraciones.
4. Ejecutar el script de datos iniciales.
5. Ejecutar la API:
```bash
dotnet run
```

## Arquitectura Frontend
La aplicación cliente fue desarrollada utilizando Angular como framework principal para la construcción de interfaces de usuario y Angular Material para la implementación de componentes visuales.
La solución sigue una organización por responsabilidades con el objetivo de facilitar el mantenimiento, la escalabilidad y la reutilización de código.
### Tecnologías Utilizadas
- Angular
- TypeScript
- Angular Material
- RxJS
- Reactive Forms
- Angular Router
- JWT Authentication
---
### Estructura General
```text
src/
│
├── pipes/
├── guards/
├── interceptors/
├── models/
├── pages/
├── services/
├── components/
├── utils/
├── app.routes.ts
└── app.config.ts
```
---
### Organización de Componentes
#### Pages
Contienen las pantallas principales de la aplicación y representan cada flujo funcional.
Ejemplos:
- Login
- Panel del Asesor
- Panel del Estudiante
#### Components
Contienen componentes reutilizables y modales utilizados por las páginas principales.
Ejemplos:
- Crear Solicitud
- Detalle de Solicitud y confirmación de cambio de estado
#### Services
Centralizan la comunicación con la API REST mediante HttpClient.
Ejemplos:
- AuthService
- SolicitudService
- EstudianteService
#### Models
Definen los contratos de datos utilizados para el intercambio de información entre el frontend y el backend.
#### Pipes
Permiten transformar información para mejorar su visualización.
Ejemplos:
- Conversión de estados numéricos a texto.
#### Guards
Protegen las rutas de la aplicación validando autenticación y permisos según el rol del usuario.
#### Interceptors
Permiten interceptar solicitudes HTTP para agregar automáticamente el token JWT en las peticiones autenticadas.

---
### Autenticación y Autorización
La autenticación se implementó mediante JSON Web Tokens (JWT).
El flujo utilizado es:
1. El usuario realiza el proceso de autenticación.
2. El backend genera un token JWT.
3. El token se almacena en el navegador.
4. El interceptor agrega automáticamente el token a las solicitudes protegidas.
5. Los guards validan la autenticación y los permisos de acceso a cada módulo.
---
### Manejo de Roles
La aplicación implementa autorización basada en roles.
Roles disponibles:
- Asesor
- Estudiante
Cada rol posee acceso únicamente a las funcionalidades autorizadas por el sistema.
---
### Formularios Reactivos
Se utilizaron Reactive Forms para la construcción de formularios.
Beneficios:
- Validaciones centralizadas.
- Control detallado del estado de los campos.
- Manejo sencillo de errores.
- Escalabilidad para formularios complejos.
Ejemplos implementados:
- Inicio de sesión.
- Registro de solicitudes.
---
### Experiencia de Usuario
Se implementaron mecanismos orientados a mejorar la experiencia del usuario:
- Indicadores visuales de carga.
- Deshabilitación de acciones durante operaciones en proceso.
- Confirmaciones antes de ejecutar acciones críticas.
- Notificaciones mediante Snackbar.
- Modales para operaciones de creación y consulta.
- Paginación de resultados.
- Filtros por estado de solicitud.
---
### Componentes Material Utilizados
Entre los componentes principales utilizados se encuentran:
- MatCard
- MatTable
- MatPaginator
- MatDialog
- MatFormField
- MatInput
- MatSelect
- MatButton
- MatSnackBar
- MatProgressSpinner
---
## Ejecución del Frontend
Ubicarse en la carpeta del proyecto Angular:
```bash
cd eduApoyos-app
```
Instalar dependencias:
```bash
npm install
```
Ejecutar la aplicación:
```bash
ng serve
```
La aplicación estará disponible en:
```text
http://localhost:4200
```
Para el correcto funcionamiento de la aplicación es necesario que la API se encuentre ejecutándose y accesible desde la configuración de entorno correspondiente.

---
## Configuración de Base de Datos
### 1. Crear la Base de Datos
Crear una base de datos SQL Server denominada:
```sql
CREATE DATABASE EduApoyos;
GO
```
---
### 2. Crear Usuario de Aplicación
Crear el usuario que será utilizado por la API para conectarse a la base de datos.
Ejemplo:
```sql
CREATE LOGIN adminEduApoyos 
WITH PASSWORD = 'Test1234';
GO

USE EduApoyos;
GO

CREATE USER adminEduApoyos 
FOR LOGIN adminEduApoyos;
GO

ALTER ROLE db_owner ADD MEMBER adminEduApoyos;
GO
```
---
### 3. Configurar la Cadena de Conexión
Actualizar el archivo `appsettings.json` o las variables de entorno correspondientes:
```json
{
  "ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=EduApoyos;User Id=adminEduApoyos;Password=Test1234;TrustServerCertificate=True;"
  }
}
```
---
### 4. Ejecutar Migraciones
Ejecutar las migraciones utilizando Entity Framework Core:
```bash
dotnet ef database update
```
Este comando creará:

- Tablas
- Relaciones
- Restricciones
- Índices definidos mediante migraciones
---
### 5. Ejecutar Scripts Complementarios
Una vez creada la estructura de la base de datos, ejecutar los scripts ubicados en la carpeta:
```text
Database/
```
#### Scripts incluidos

| Script | Descripción |
|----------|----------|
| 01_SolicitudesPendientes.sql | Consulta solicitudes pendientes con más de 5 días sin actualización |
| 02_EstadisticasUltimoMes.sql | Consulta solicitudes agrupadas por estado y tipo de apoyo durante el último mes |
| 03_Indice_Solicitudes.sql | Creación de índice no agrupado para optimizar consultas |
| 04_SeedData.sql | Inserción de datos iniciales para pruebas |
---
### 6. Cargar Datos Iniciales
Ejecutar:
```text
Database/04_SeedData.sql
```
Este script crea:
- Usuarios
- Estudiantes
- Solicitudes
- Historial de estados
necesarios para validar el funcionamiento de la aplicación.
---
## Datos de Prueba
### Usuario Asesor
| Campo | Valor |
|---------|---------|
| Correo | jdgm1234@gmail.com |
| Contraseña | Test1234 |

### Usuario Estudiante

| Campo | Valor |
|---------|---------|
| Correo | test1@gmail.com |
| Contraseña | Test1 |
> Las contraseñas corresponden a los hashes incluidos en el script `04_SeedData.sql`.
---
## Pruebas Unitarias
Ejecutar:
```bash
dotnet test
```
Para generar cobertura:
```bash
dotnet test --collect:"XPlat Code Coverage"
```
Generar reporte HTML:
```bash
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"CoverageReport" -reporttypes:Html
```
---
## Estructura de Scripts SQL
```text
Database
│
├── 01_SolicitudesPendientes.sql
├── 02_EstadisticasUltimoMes.sql
├── 03_Indice_Solicitudes.sql
└── 04_SeedData.sql
```
## Configuración de la Aplicación
La solución utiliza User Secrets para almacenar configuraciones sensibles durante el desarrollo.
Las siguientes configuraciones son necesarias para ejecutar correctamente la aplicación:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=DESKTOP-KLTCPOM\\MSSQLSERVER01; Database=EduApoyos;User Id=adminEduApoyos;Password=Test1234;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Jwt": {
    "SecretKey": "7a515c258aa0d4d9d67e1c326f12586281036f8b0a9dfdf038c026deb988e4e8",
    "Issuer": "EduApoyos",
    "Audience": "EduApoyosUsers",
    "ExpirationMinutes": 15
  }
}
```
---
## Servicios Azure Propuestos
La siguiente propuesta corresponde a una posible estrategia de despliegue para la solución utilizando servicios administrados de Microsoft Azure.
La selección fue realizada considerando la naturaleza de la aplicación desarrollada, la integración con tecnologías .NET y las buenas prácticas comúnmente utilizadas en proyectos empresariales.
> **Nota**
> La información presentada en esta sección se basa en una investigación realizada como complemento a la prueba técnica y en experiencia previa utilizando servicios del ecosistema Azure como repositorios, gestión de tareas, bases de datos, gestión de secretos y distribución de paquetes.
> No corresponde a una definición formal de arquitectura cloud ni a un proceso de dimensionamiento de infraestructura productiva. La selección final de capacidades, costos y configuraciones debería realizarse a partir de métricas reales de utilización, volumen de datos y requerimientos operativos específicos.
---
### Azure App Service
**Uso propuesto**
Hospedaje de la API ASP.NET Core y de la aplicación Frontend Angular.
**Justificación**
Azure App Service permite desplegar aplicaciones web sin necesidad de administrar servidores o infraestructura subyacente.
Entre sus principales beneficios se encuentran:
- Despliegue simplificado de aplicaciones web.
- Integración con GitHub y Azure DevOps.
- Escalabilidad administrada.
- Certificados SSL integrados.
- Monitoreo y diagnóstico mediante Application Insights.
**Consideración**
Para una etapa inicial podría utilizarse un plan básico de bajo costo. La selección definitiva del tier dependerá del número de usuarios concurrentes, consumo de recursos y necesidades de escalabilidad identificadas durante la operación de la solución.
---
### Azure SQL Database
**Uso propuesto**
Almacenamiento de la información relacional del sistema:
- Usuarios.
- Estudiantes.
- Solicitudes de apoyo.
- Historial de estados.
**Justificación**
Azure SQL Database ofrece una alternativa administrada compatible con SQL Server y Entity Framework Core, reduciendo tareas operativas relacionadas con mantenimiento de infraestructura.
Beneficios principales:
- Copias de seguridad automáticas.
- Alta disponibilidad administrada.
- Monitoreo integrado.
- Escalabilidad según demanda.
- Integración nativa con aplicaciones .NET.
**Consideración**
La selección del nivel de servicio dependerá del volumen de información, frecuencia de consultas y carga transaccional observada una vez la solución se encuentre en producción.
---
### Azure Blob Storage
**Uso propuesto**
Almacenamiento de documentos asociados a las solicitudes de apoyo.
Ejemplos:
- Documentos de identidad.
- Certificados académicos.
- Soportes financieros.
- Archivos PDF.
- Imágenes.
**Justificación**
Aunque la funcionalidad de carga de documentos no fue implementada dentro del alcance de esta versión, Azure Blob Storage sería una alternativa adecuada para almacenar archivos de forma desacoplada de la base de datos.
Beneficios principales:
- Bajo costo de almacenamiento.
- Escalabilidad prácticamente ilimitada.
- Integración sencilla con aplicaciones .NET.
- Gestión eficiente de archivos de gran tamaño.
- Posibilidad de compartir archivos mediante accesos temporales seguros.
---
### Azure Key Vault
**Uso propuesto**
Administración centralizada de información sensible.
Ejemplos:
- Cadenas de conexión.
- Secretos JWT.
- Credenciales de servicios externos.
- Certificados digitales.
**Justificación**
Actualmente la solución utiliza configuraciones locales para facilitar la evaluación técnica.
En un entorno productivo se recomienda utilizar Azure Key Vault para evitar el almacenamiento de información sensible dentro del código fuente o archivos de configuración.
Beneficios principales:
- Mayor seguridad en la gestión de credenciales.
- Centralización de secretos.
- Control de acceso mediante identidades administradas.
- Rotación segura de credenciales.
- Integración con aplicaciones desplegadas en Azure.
---
### Conclusión
La propuesta presentada busca priorizar simplicidad operativa, integración con el ecosistema Microsoft y capacidad de crecimiento futuro.
Los servicios seleccionados representan una base adecuada para una posible evolución de la solución hacia un entorno productivo, manteniendo un equilibrio entre facilidad de administración, seguridad y escalabilidad.

## Conclusiones y Trabajo Futuro

Durante el desarrollo de esta solución se priorizó la construcción de una arquitectura organizada por capas, buscando mantener una separación clara entre la lógica de negocio, el acceso a datos y la exposición de servicios mediante API REST.

Se definen como decisiones más relevantes tomadas durante la implementación:
- Utilización de una arquitectura por capas para facilitar el mantenimiento y la escalabilidad de la solución.
- Implementación del patrón Repository para desacoplar la lógica de negocio del acceso a datos mediante Entity Framework Core.
- Uso de Inyección de Dependencias para mejorar la mantenibilidad y facilitar la creación de pruebas unitarias.
- Implementación de autenticación basada en JWT para la protección de los endpoints.
- Crear una respuesta general para las solicitudes y que esta integre genericos, para poder sin importar el tipo de consulta, generar una respuesta estructurada.
- Crear una respuesta general paginada para que implemente su estructura con detalles utiles para controlar la paginación desde el FrontEnd, de igual manera usando genericos para poder estructurar las respuestas correctamente.

Si se dispusiera de más tiempo para mejorar la solución, se considerarían las siguientes actividades:
- Completar la documentación de la API mediante Swagger/OpenAPI, incorporando descripciones funcionales de cada endpoint, documentación de parámetros, respuestas posibles y ejemplos de integración para facilitar el consumo por parte de clientes externos.
- convertir Enumeraciones tales como Roles, Tipos de Apoyos, Sesiones y entre otros en tablas estructuradas de la base de datos para que su gestión no tenga un impacto directo con la aplicación si se requiere incluir, modificar o eliminar alguno.
- Crear excepciones personalizadas para tener un mayor control y trasabilidad al momento de evaluar escenarios inesperados.
- Implementar auditoría centralizada para registrar acciones de usuarios y eventos relevantes del sistema ya sea en logs de tipo archivo o a nivel de base de datos.
- Crear un set de pruebas unitarias que evaluen al menos cada una de las capas de gran impacto, no solo la de aplicación.
- Crear Dtos para respuestas donde el objeto resultante puede ser complejo y lleve consigo datos innecesarios para el proceso en especifico.
- Separar la logica del jwt para que no sea completamente dependiente de servicio de autenticación.
- Incorporar pruebas unitarias para componentes, servicios, guards e interceptors del frontend.
- Implementar una estrategia de manejo global de errores para centralizar la presentación de mensajes al usuario.
- Definir constantes para centralizar la presentación de mensajes de todos los tipos.
- Definir pipes personalizadas para todas las opciones de transformación de los datos.
- Realizar la pantalla completa para la visualización del estudiante.
- Estructurar las consultas para cuando se visualizan las solicitudes, asi como para la creación de las mismas se presente completa la información de los estudiantes seleccionados.
- Mejorar la experiencia responsive para dispositivos móviles y tabletas.
## Pipeline CI/CD
Como parte de la solución se incluye un ejemplo de pipeline de Integración Continua utilizando Azure DevOps.
El archivo se encuentra ubicado en la raíz del proyecto:
```text
eduapoyo-pipelines.yml
```
### Objetivo
Automatizar las validaciones básicas de calidad antes de integrar cambios a una rama principal.
### Flujo implementado
1. Instalación del SDK de .NET 8.
2. Restauración de dependencias mediante `dotnet restore`.
3. Compilación de la solución en modo Release mediante `dotnet build`.
4. Ejecución de pruebas unitarias mediante `dotnet test`.
5. Publicación de la API mediante `dotnet publish`.
6. Generación de un artefacto listo para despliegue.
### Beneficios
La automatización de estas tareas permite:

- Detectar errores de compilación de forma temprana.
- Verificar la ejecución correcta de las pruebas unitarias.
- Validar que la solución pueda ser publicada correctamente.
- Reducir errores manuales durante procesos de integración.
- Estandarizar el proceso de validación de cambios.

### Alcance de la propuesta
La definición del pipeline incluida en esta solución tiene fines demostrativos para cumplir los requerimientos de la prueba técnica.
Su construcción se basa en experiencia previa trabajando con pipelines ya existentes dentro de proyectos empresariales, así como en consulta de documentación oficial de .NET y Azure DevOps para comprender la estructura mínima necesaria.
Si bien no se cuenta con experiencia especializada en diseño de arquitecturas completas de CI/CD desde cero, se entiende el propósito de las etapas principales de integración continua y su aporte al ciclo de desarrollo de software. Por esta razón, la propuesta presentada busca representar una implementación básica, coherente y alineada con buenas prácticas comúnmente utilizadas en proyectos .NET.
