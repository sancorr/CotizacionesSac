# Cotizaciones SAC API
## Descripción
Este proyecto es una API en .NET 5 que obtiene las cotizaciones de diversas monedas e instrumentos financieros y expone los endpoints para acceder a éstos valores.
## Documentacion API divisas -> Dolar API : https://dolarapi.com/docs/

## Tecnologias utilizadas en este proyecto
- C# con .NET 5
- ASP.NET Core Web Api
- Xunit (Testing)
- Moq (Mocking para pruebas unitarias)
- Test server (pruebas de integración)

## Estructura del proyecto
Este proyecto sigue una arquitectura en capas de tipo "Clean Architectecture" con el objetivo de separar las responsabilidades/tareas en diferentes capas bien definidas con reglas de interacción claras entre cada una de ellas.

|-- CotizacionesSolution/

|   |--CotizacionesApi/  -> Proyecto principal con controladores y punto de entrada de la aplicacion (clases Program.cs y Startup.cs)

|   |--CotizacionesDomain/  -> Libreria de clases que contiene las entidades (clases modelo de los objetos que la aplicación manipulará) y las interfaces que definen la implementación de los métodos necesarios para obtener la informacion desde la API.

|   |--CotizacionesInfrastructure/  -> Libreria de clases encargada de contener toda la logica de negocio encargada del desarrollo e implementación de los metodos que la interfaz define que se deben implementar.

|   |--CotizacionesTest/  -> Proyecto de Xunit que tambien implementa la libreria Moc, cuyo propósito es el de realizar las pruebas unitarias de los metodos que se encargan de hacer solicitudes GET a la API y deserializar los objetos JSON en codigo que el compilador pueda entender, y tambien pruebas de integracion END to END consumiendo la informacion real proveniente de la API.

## Instalación y uso
1. Clonar el repositorio
   ```bash
	git clone https://github.com/sancorr/CotizacionesSac.git
2.
	```bash
	cd tu-repositorio
3.
	```bash
	dotnet restore
4.
	```bash
	dotnet build
5.
	```bash
	ejecutar aplicacion 
#### ¿Porque son necesarios ésta serie de pasos?
Cuando trabajas con un proyecto en .NET, éste usa paquetes NuGet como dependencias externas (librerías de terceros o del propio framework). Estos paquetes no se almacenan dentro del repositorio del proyecto (para evitar archivos innecesarios y reducir el tamaño del repo).

**dotnet restore** se encarga de descargar e instalar automáticamente todas las dependencias listadas en los archivos **.csproj** para que el proyecto pueda compilar y ejecutarse sin problemas.
##### Lo que hace:
1.	Lee el archivo CotizacionesSolution.csproj y busca todas las dependencias declaradas en las secciones **PackageReference**.

2.	Descarga los paquetes NuGet desde nuget.org.

3.	Guarda los paquetes en la carpeta de caché de NuGet.

4.	Registra las referencias de las librerías dentro del proyecto para que sean accesibles durante la compilación.
