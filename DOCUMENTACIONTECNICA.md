## Documentación técnica de Cotizaciones API
### Estructura del proyecto
CotizacionesSolution está organizado en las siguientes carpetas:

1. CotizacionesApi/

Contiene la API principal que gestiona las cotizaciones
**Archivos principales:**
- **Program.cs**: Punto de entrada de la aplicación
- **Starup.cs**: Configuracion de los servicios y middlewares.
- **Controllers/MonedaController.cs**: Controlador de tipo tipo API que maneja las solicitudes HTTP a la API, cuya ruta especificada es "api/monedas/".

2. CotizacionesDomain/

**Entities**:

Contiene las entidades (modelos de dominio), las cuales son clases de C# que modelan los objetos que la aplicacion manipulará durante su ejecución.

**Interfaces**:

Contiene las interfaces de C# que definen que métodos se van a implementar para solicitar y procesar la información obtenida desde la API mediante solicitudes HTTP GET

3. CotizacionesInfrastructure/

**Repositories**:

Aqui esta toda la lógica de negocio. Es aqui donde están las clases repositorio donde se escriben, desarrollan e implementan los métodos que define la interfaz que deben aplicarse. Dicha clase repositorio hereda la interfaz correspondiente a cada objetivo (Divisas, acciones, etc)

4. CotizacionesTests/

- **MonedaControllerTest.cs**: archivo de pruebas unitarias con moc
- **MonedaControllerIntegration.cs**: Puebas de integracion para el controlador


# Explicación del código fuente
En **Starup.cs**
```bash
public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllers();
			//inyeccion de dependencia
			services.AddHttpClient<IMonedaRepository, MonedaRepository>();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "CotizacionesApi", Version = "v1" });
			});
		}

```
la linea: 

	services.AddHttpClient<IMonedaRepository, MonedaRepository>();

Es la que inyecta la dependencia en el servicio, entonces cuanto la aplicacion se ejecute va a ir a buscar esa interfaz y ese repositorio los cuales la proveerán con información.

**CotizacionesApi/Controllers**

- MonedaController.cs

El controlador es el primer componente al que la API responde cuando se le hace una solicitud GET

[ApiController] : 

Valida que sea una controlador y valida los datos de entradas
	
[Route("api/monedas")] :

Define la ruta base del controlador

**public class MonedaController : ControllerBase**

La clase hereda de ControllerBase, la cual maneja solicitudes HTTP para aplicaciones sin vista.

1. private readonly IMonedaRepository _monedaRepository;

Variable privada del tipo Interfaz que define los metodos para obtener las divisas

2. public MonedaController(IMonedaRepository monedaRepositorio)
		{
			_monedaRepository = monedaRepositorio;
		}

Contructor que inicializa automaticamente la interfaz en la variable del punto 1 cada vez que la clase es llamada

3. 
```
[HttpGet("dolares")]
		public async Task<IActionResult> obtenerDolares()
		{
			try
			{
				var monedas = await _monedaRepository.ObtenerDolaresAsync();

				if (monedas == null)
					return NotFound();


				return Ok(monedas);

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
```

Este es un método que hace una petición HttpGet en la ruta "monedas/api/dolares", el cual es una Task (clase que permite el manejo operaciónes asincrónicas) y devuelve un IActionResult el cual es un codigo de estado, 200 si es exitoso ó 404 si no obtuvo respuesta del servidor. Espera por la respuesta del método en la clase de repositorio el cual estará mas abajo en la documentación.

Los siguientes métodos: 

- obtenerEuro()
- obtenerReal()
- obtenerChileno()
- obtenerUruguayo()

hacen la misma acción y siguen la misma lógica, cada uno tiene su ruta a la cual hacen la petición Http Get y cada uno espera por la respuesta de su metodo en la clase de repositorio.


**CotizacionesDomain/Entities**

- Moneda.cs

Define la estructura del objeto que va a manipular.

```
public class Moneda
	{
		public int IdMoneda { get; set; }
		public decimal Compra { get; set; }
		public decimal Venta { get; set; }
		public string Casa { get; set; }
		public string Nombre { get; set; }
		public string Codigo { get; set; }
		public DateTime FechaActualizacion { get; set; }

		//Constructor
		public Moneda(decimal compra, decimal venta, string casa, string nombre, string codigo, DateTime fecha)
		{
			Compra = compra;
			Venta = venta;
			Casa = casa;
			Nombre = nombre;
			Codigo = codigo;
			FechaActualizacion = fecha;
		}
	}
```



**CotizacionesDomain/Interfaces**

- IMonedaRepository.cs

```
	public interface IMonedaRepository 
	{
		//DOLARES- todos los tipos de cambio
		Task<IEnumerable<Moneda>> ObtenerDolaresAsync();

		//Task<Moneda> ObtenerPorCodigoAsync(string codigo);
		//EUROS
		Task<Moneda> ObtenerEuros();
		//REALES
		Task<Moneda> ObtenerReal();
		//CHILENO
		Task<Moneda> ObtenerChileno();
		//URUGUAYO
		Task<Moneda> ObtenerUruguayo();

	}
```

Define los metodos que por contrato se deben implementar para poder obtener la informacion de la API, procesarla y exponerla en los endpoints del controlador.

El objeto que le dará formato a dicha informacion es Moneda.cs

En el caso de todas las cotizaciones de dólares es un IEnumerable porque la respuesta JSON de dicha solicitud devuelve más de un objeto, devuelve todas las cotizaciones de los distintos tipos de cambio en dólares.

El hecho de que se trabaje de manera asíncrona hace que el uso de la clase **Task** sea la opcion mas apropiada, ya que permite el manejo de tareas asincronas, de esta manera la aplicacion puede manejar solicitudes en paralelo y no bloquea la aplicacion, de lo contrario al hacer una solicitud, la misma deberia terminar para luego poder ejecutar otra y ésto ocasionaria una gran deficiencia en la experiencia del usuario.


**CotizacionesInfrastructure/Repositories**

- MonedaRepository.cs

Ésta es la capa de negocio, es decir, donde se desarrolla toda la lógica y el codigo que se encarga de implemntar los métodos que la interfaz obliga a implementar.

Aqui ocurren los llamados a los endpoints de la API.

**public class MonedaRepository : IMonedaRepository**

Esta clase hereda la interfaz para poder acceder a los métodos que se deben implementar.

**private readonly HttpClient _httpClient;**

Declara una variable privada de tipo HttpClient para poder manejar las solicitudes HTTP

```
	public MonedaRepository(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
```

Inicializa dicha variable en su constructor para que esté disponible automaticamente cada vez que se la solicite.


###### Llamados a la Api de cotizaciones de divisas

```
	public async Task<IEnumerable<Moneda>> ObtenerDolaresAsync()
		{
			try
			{
				string response = await _httpClient.GetStringAsync("https://dolarapi.com/v1/dolares");

				var monedas = JsonConvert.DeserializeObject<List<Moneda>>(response);
				return monedas;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
```
Este metodo asíncrono devuelve un IEnumerable de **Moneda**

- Esta linea:

```bash
	string response = await _httpClient.GetStringAsync("https://dolarapi.com/v1/dolares");
```

Usa el la variable de tipo _httpClient, la cual en su interior tiene un objeto de HttpClient para hacer un llamado a la API y guardar su respuesta en una variable string. Para ello se usa el método **.GetStringAsync** que obtiene la respuesta JSON de la API y la lée en formato string.

- Siguiente bloque de lineas:

```bash
	var monedas = JsonConvert.DeserializeObject<List<Moneda>>(response);
				return monedas;
```

Deserializa el/los objetos JSON provenientes de la respuesta en una List de C#. Para ésto se usa la libreria newtonsoft de .NET para deserializar objetos JSON, cuya clase **JsonConvert** tiene el método **.DeserializeObject** que recibe como parámetro un string que sera la variable anterior, y luego simplemente lo retorna.

Los siguientes Métodos:

- ObetenerEuros()
- ObtenerReal()
- ObtenerChileno()
- ObtenerUruguayo()

Tiene la misma estructura y funcionalidad, la unica diferencia es que no deserializan el objeto en una List como en el ejemplo anterior, ya que al recibir un único objeto JSON como respuesta, hay solamente un objeto para deserializar. Entonces se deserializa un objeto **Moneda**

```bash
	var ejemplo = JsonConvert.DeserializeObject<Moneda>(response);
```


**CotizacionesTests/MonedaControllerIntegrationTest.cs**

