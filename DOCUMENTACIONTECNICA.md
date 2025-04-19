## Documentación técnica de Cotizaciones API
### Estructura del proyecto
CotizacionesSolution está organizado en las siguientes carpetas:

1. CotizacionesApi/

Contiene la API principal que gestiona las cotizaciones
**Archivos principales:**
- **Program.cs**: Punto de entrada de la aplicación
- **Startup.cs**: Configuracion de los servicios y middlewares.
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

Esta clase es la encargada de los tests de integracion, obteniendo los datos de la api de DolarApi

Esta clase hereda la interface IClassFixture, que es una interfaz que permite crear un servidor en memoria para poder consultar los datos.

1. **private readonly HttpClient _client;** Variable privada de solo lectura que guarda un objeto de tipo HttpClient


Contructor que crea un cliente HTTP para probar la API en un servidor en memoria
```bash
	public MonedaControllerIntegrationTest(WebApplicationFactory<Program> factory)
		{
			_client = factory.CreateClient();
		}
```

- ```WebApplicationFactory<Program>``` le indica cual es el punto de entrada de la aplicación (la clase Program.cs)

3. ```bash
	[Fact]
		public async Task ObtnerDolares_RetornaCotizacionesReales()
		{
			//Act, peticion a la API de aplicacion por el endpoint del controlador
			var response = await _client.GetStringAsync("/api/monedas/dolares");

			//Deserializacion de los objetos JSON de la respuesta de la API, en una LIST<>
			var dolares = JsonConvert.DeserializeObject<List<Moneda>>(response);
			
			//verificaciones de la respuesta - verifica que el JSON no sea una lista vacia
			Assert.NotEmpty(dolares);


		} 
	```

Los siguientes métodos : 
- ObtenerEuro_RetornaCotizacionReal()
- ObtenerReal_RetornaCotizacionReal()
- ObtenerChileno_RetornaCotizacionReal()
- ObtenerUY_RetornaCotizacionReal()

Siguen la misma lógica y estructura, con la diferencia de cada uno hace la peticion a su endpoint correspondiente según está establecido por **MonedaController.cs**


**CotizacionesTests/MonedaControllerTest.cs**

Esta clase es la encargada de las pruebas unitarias de los métodos que obtienen la información de las peticiónes de divisas a la API de DolarApi. Se utilizan la libreria de XUnit para testing en .NET y Moc para la creacion de MOCS que simulan los datos.

- **[Fact]** --> Indica que se trata de una prueba unitaria.

```bash

		[Fact]
		public async Task ObtenerEuro_RetornaValorCorrecto() {

			var mockInterface = new Mock<IMonedaRepository>(); //Mock de la interfaz
			mockInterface.Setup(repo => repo.ObtenerEuros()).ReturnsAsync(new Moneda( //Llamado al metodo que establece la interfaz
				(decimal)125.36,
				(decimal)325.69,
				"Oficial",
				"Euro",
				"EUR", 
				DateTime.Now));

			var controller = new MonedaController(mockInterface.Object); //instancia de la clase que tiene el controlador, inyectando el mock recien creado

			var resultado = await controller.obtenerEuro() as OkObjectResult;

			var moneda = resultado?.Value as Moneda; //extaigo la moneda de la respuesta convirtiendo la respuesta en una instacia de moneda

			//verificaciones de resultados

			Assert.NotNull(resultado);
			Assert.Equal(200, resultado.StatusCode);
			Assert.NotNull(moneda);
			Assert.Equal("Euro", moneda.Nombre);
			Assert.Equal((decimal)125.36, moneda.Compra);
			Assert.Equal((decimal)325.69, moneda.Venta);
		}
```

- ``` var mockInterface = new Mock<IMonedaRepository>();```  Crea una instancia de MOCK y le inyecta la inteface que decalara los metodos a testear

- Configuro el MOCK usando el método Setup para poder acceder al metodo que se debe testear, el cual es asíncrono, entonces con el método **.ReturnsAsync(Moneda)** voy a obtener un objeto Moneda, de manera asíncrona. Al mismo tiempo, Moneda en su constructor recibe la informacion que lo conforma como objeto modelo (entidad), lo que declare en los parametros del constructor serán los datos que devolverá la prueba unitaria 
 ```
  mockInterface.Setup(repo => repo.ObtenerEuros()).ReturnsAsync(new Moneda( //Llamado al metodo que establece la interfaz
				(decimal)125.36,
				(decimal)325.69,
				"Oficial",
				"Euro",
				"EUR", 
				DateTime.Now));
```

 ```bash 
	var controller = new MonedaController(mockInterface.Object); //instancia de la clase que tiene el controlador, inyectando el mock recien creado  
``` 

- La siguiente linea:
```bash 
	var resultado = await controller.obtenerEuro() as OkObjectResult;
``` 
Espera por la respuesta del método del controlador recien creado que tiene el Mock con la información ya inyectado, dicha respuesta es espera como un **OkObjectResult**

- La siguiente linea:
```bash 
	var moneda = resultado?.Value as Moneda;
``` 
Extrae el objeto Moneda de la respuesta, contemplando la posibilidad de que dicha respuesta sea Null (resultado?.Value)

- Finalmente, lo que resta son las verificaciones de la prueba usando la clase Assert y sus métodos.
```bash 
	
			Assert.NotNull(resultado);
			Assert.Equal(200, resultado.StatusCode);
			Assert.NotNull(moneda);
			Assert.Equal("Euro", moneda.Nombre);
			Assert.Equal((decimal)125.36, moneda.Compra);
			Assert.Equal((decimal)325.69, moneda.Venta);
``` 

- El resto de los métodos siguen la misma estructura y lógica, con la diferencia de que **ObtenerDolares_ValoresCorrectos()** lo que retorna asíncronamente es una ```List<Moneda>``` en lugar de devolver un objeto único como el resto de los métodos.

### Sección: Cotizaciones e Instrumentos financieros - Integracion con ProfitAPI
##### Fuente de información: https://api.profit.com/
##### Tipos de datos: 
Instrumentos financieros, índices, cotizaciónes y mercados
##### Frecuencia de actualización: 
Puede variar según el endpoint, generalmente es información semi-estática o actualizable por llamada.
##### Autenticación: 
Se requiere un token de autenticación (obtenido previamente). Este se incluye como query param al iniciar la conexión.

##### Clases e interfaces
###### - ICotizacionesRepository
Define el contrato que implementa el repositorio que gestiona:
- Códigos de mercado
- Indices globales y por país
- Acciones por mercados (códigos de mercados)
- Ultimas cotizaciones de instrumentos (precio de cierre)
- Constituyentes de un índice solicitado

```bash
	public interface ICotizacionesRepository
{
    Task<List<MarketCode>> GetStocksMarketsCode();
    Task<List<MarketCode>> GetIndexesMarketsCode();
    Task<List<FinancialInstrument>> GetAllIndexes();
    Task<List<FinancialInstrument>> GetUsaIndexes();
    Task<List<FinancialInstrument>> GetIndexesMarketCountry(string exchangeCode);
    Task<List<FinancialInstrument>> GetStocksMarketsCountry(string exchangeCode);
    Task<List<IndexConstituent>> GetIndexesConstituens(string symbol);
    Task<LastQuote> GetLastQuote(string symbol);
}

```

##### Entidades:
###### - **FinancialInstrument.cs**
Representa un activo financiero como una acción, bono o un índice

```bash
Propiedad	| Tipo   | Descripción
Ticker		| string | Código de cotización
Symbol		| string | Símbolo único
Name		| string | Nombre completo del activo
Type		| string | Tipo de instrumento (acción, índice)
Currency	| string | Moneda de cotización
Country		| string | País del mercado
Exchange	| string | Bolsa o mercado correspondiente
```

###### - **IndexConstituent.cs**
Representa un activo que forma parte de un índice financiero.

```bash
Propiedad | Tipo   | Descripción
Ticker    | string | Código bursátil del activo
Code      | string | Código interno o alternativo
Exchange  | string | Mercado en el que cotiza
Name      | string | Nombre del activo
Sector    | string | Sector económico
Industry  | string | Industria específica
```

###### - **MarketCode.cs**
Códigos de los mercados disponibles para acciones o índices.

```bash
Propiedad | Tipo   | Descripción
Name      | string | Nombre del mercado
Code      | string | Código único del mercado
```

###### - **PriceData.cs**
Información de cotización

```bash
Propiedad | Tipo    | Descripción
Ticker    | string  | Código del instrumento
Price     | decimal | Último precio conocido
Timestamp | long    | Fecha/hora de la cotización 
Volume    | decimal | Volumen operado
```

###### - **LastQuote.cs**
Última cotización completa de un instrumento (precio de cierre anterior).

```bash
Propiedad             | Tipo        | Descripción
Ticker                | string      | Código del instrumento
Name                  | string      | Nombre del activo
Symbol                | string      | Símbolo bursátil
Price                 | decimal     | Precio actual
PreviousClose         | decimal     | Cierre del día anterior
DailyPriceChange      | decimal     | Diferencia de precio respecto al cierre
DailyPercentageChange | decimal     | Variación porcentual diaria
Timestamp             | long        | Timestamp UNIX
AssetClass            | string      | Clase de activo
Currency              | string      | Moneda
LogoUrl               | string      | URL del logo (si aplica)
Volume                | long        | Volumen operado
Broker                | string      | Corredor o intermediario
OhlcWeek              | OHLC_Weekly | Apertura, cierre, máximo y mínimo
```
###### - **OHLC_Weekly**
Datos semanales del instrumento.
```bash
Propiedad | Tipo    | Descripción
Open      | decimal | Precio de apertura
High      | decimal | Máximo semanal
Low       | decimal | Mínimo semanal
Close     | decimal | Cierre semanal
```

#### Ejemplo de uso
1 - GetStocksMarketsCode()
	
Obtiene los códigos de mercado disponibles para acciones.

2 - GetStocksMarketsCountry("NYSE")

Devuelve todas las acciones que cotizan en la bolsa de NY.

3 - GetLastQuote("AAPL")
Trae la última cotización de Apple Inc.

4 - GetIndexesConstituens("NDX")

Lista todos los activos que componen el índice Nasdaq.



### MarketCodesRepositories.cs - Documentación
Esta es la clase repositorio que implementa el contrato de la Interface ICotizacionesRepository. Su objetivo es conectarse a la **API de Profit** y recuperar información relacionada con: 
- Códigos de mercados (acciones e índices).
- Listados de índices o acciones por mercado.
- Últimas cotizaciones de instrumentos solicitados.
- Componentes (constituyentes) de índices financieros.

#### Dependencias
- **HttpClient:** usado para realizar peticiones HTTP a la API de Profit.

- **Newtonsoft.Json:** para deserializar los JSON que devuelve la API.

- **JObject:** permite acceder a objetos JSON anidados cuando la API envía respuestas con estructuras complejas.

#### Token
El acceso a la API de Profit requiere un token. En esta clase está definido como:
```bash
	private readonly string token = "f89dee4238b641e684301f3973086aaf";
```

#### Constructor
```bash
	public MarketCodesRepositories(HttpClient httpClient)
```
- Inyecta una instancia de HttpClient mediante inyección de dependencias (DI), lo que permite su reutilización y testeo.

###### Métodos

1. **GetStocksMarketsCode()**

Obtiene los mercados disponibles para acciones.

GET: https://api.profit.com/data-api/reference/exchanges?token={token}&type=stocks

Devuelve: List<**MarketCode**>

2. **GetIndexesMarketsCode()**

Obtiene los mercados disponibles para índices.

GET: https://api.profit.com/data-api/reference/exchanges?token={token}&type=indexes

Devuelve: List<**MarketCode**>

3. **GetAllIndexes()**

Obtiene todos los índices financieros disponibles.

GET: https://api.profit.com/data-api/reference/indices?token={token}&skip=0&limit=1000

Devuelve: List<**FinancialInstrument**>

**Observación:** La API devuelve un objeto con una propiedad data, por eso se usa JObject.Parse.

4. **GetUsaIndexes()**

Obtiene índices financieros del mercado estadounidense.

GET: https://api.profit.com/data-api/reference/indices?token={token}&limit=1000&exchange=INDX&country=United%20States&currency=USD&available_data=historical&available_data=fundamental&type=INDEX

Devuelve: List<**FinancialInstrument**>

5. **GetIndexesMarketCountry(string exchangeCode)**

Obtiene índices de un mercado específico, filtrado por su código.

**Parámetro:**

exchangeCode: código del mercado (por ejemplo "INDX")

GET: https://api.profit.com/data-api/reference/indices?token={token}&skip=0&limit=1000&exchange={exchangeCode}&available_data=historical&available_data=fundamental&type=INDEX

Devuelve: List<**FinancialInstrument**>

6. **GetStocksMarketsCountry(string exchangeCode)**

Obtiene acciones correspondientes a un mercado específico.

**Parámetro:**

exchangeCode: código de país del mercado.

GET: https://api.profit.com/data-api/reference/stocks?token={token}&exchange={exchangeCode}&limit=1000

Devuelve: List<**FinancialInstrument**>

7. **GetIndexesConstituens(string symbol)**

Obtiene los constituyentes (acciones/activos) que componen un índice.

**Parámetro:**

symbol: el símbolo del índice (por ejemplo, "GSPC" para S&P 500)

GET: https://api.profit.com/data-api/fundamentals/indexes/index_constituents/{symbol}?token={token}

Devuelve: List<**IndexConstituent**>

8. **GetLastQuote(string symbol)**

Obtiene la última cotización (precio, cambio, volumen, etc.) de un instrumento.

**Parámetro:**

symbol: el símbolo del instrumento financiero (acción o índice)

GET: https://api.profit.com/data-api/market-data/quote/{symbol}?token={token}

Devuelve: LastQuote

#### Entidades utilizadas
```bash
Entidad             | Descripción
MarketCode          | Contiene el código e información del mercado.
FinancialInstrument | Representa una acción o índice financiero.
IndexConstituent    | Representa un activo que forma parte de un índice.
LastQuote           | Representa la última cotización de un instrumento.
```

### CotizacionesController.cs - Documentación

El controlador CotizacionesController se encarga de exponer endpoints HTTP para consultar datos financieros a través de un repositorio (ICotizacionesRepository) que se conecta a la API de Profit.

#### Constructor

```bash
	public CotizacionesController(ICotizacionesRepository repo)
```

- Se inyecta una dependencia del repositorio **ICotizacionesRepository** a través de inyección de dependencias.

- Esto permite desacoplar la lógica de negocio de la lógica HTTP, facilitando pruebas y mantenibilidad.

#### Endpoints disponibles

1. GET /api/markets/stocks-codes

**Descripción:**
Obtiene los códigos de los mercados de acciones disponibles.

**Respuestas:**

**200 OK:** Lista de mercados (List<**MarketCode**>)

**200 OK:** "No hay elementos que mostrar" si la lista está vacía

**404 Not Found:** Si la respuesta es null


2. GET /api/markets/indexes-codes

**Descripción:**
Obtiene los códigos de los mercados de índices disponibles.

**Respuestas:**

**200 OK:** Lista de mercados (List<**MarketCode**>)

**200 OK:** "No hay elementos que mostrar" si la lista está vacía

**404 Not Found:** Si la respuesta es null


3. GET /api/markets/all-indexes

**Descripción:**
Obtiene todos los índices financieros disponibles, sin importar el país o el mercado.

**Respuestas:**

**200 OK:** Lista de mercados (List<**FinancialInstrument**>)

**200 OK:** "No hay elementos que mostrar" si la lista está vacía

**404 Not Found:** Si la respuesta es null


4. GET /api/markets/usa-indexes

**Descripción:**
Obtiene los índices del mercado estadounidense, filtrando por país, moneda y tipo.

**Respuestas:**

**200 OK:** Lista de mercados (List<**FinancialInstrument**>)

**200 OK:** "No hay elementos que mostrar" si la lista está vacía

**404 Not Found:** Si la respuesta es null


5. GET /api/markets/indexes-by-exchange?exchangeCode=XXX

**Descripción:**
Obtiene los índices disponibles en un mercado específico.

**Parametros:**

- exchangeCode: código del mercado (por ejemplo "INDX")

**Respuestas:**

**200 OK:** Lista de mercados (List<**FinancialInstrument**>)

**200 OK:** Lista vacía si no hay resultados

**404 Not Found:** Si la respuesta es null

6. GET /api/markets/stocks-by-exchange?exchangeCode=XXX

**Descripción:**
Obtiene las acciones disponibles en un mercado específico.

**Parametros:**

- exchangeCode: código del mercado (por ejemplo "AAPL")

**Respuestas:**

**200 OK:** Lista de mercados (List<**FinancialInstrument**>)

**200 OK:** Lista vacía si no hay acciones

**404 Not Found:** Si la respuesta es null


7. GET /api/markets/indexes-constituens?symbol=XXX

**Descripción:**
Obtiene los constituyentes (acciones) que componen un índice determinado.

**Parametros:**

- symbol: símbolo del índice (por ejemplo "NDX")

**Respuestas:**

**200 OK:** Lista de mercados (List<**IndexConstituent**>)

**200 OK:** Lista vacía si no hay acciones

**404 Not Found:** Si la respuesta es null

**500 Internal Server Error:** Si ocurre un error en tiempo de ejecución


8. GET /api/markets/last-quote?symbol=XXX

**Descripción:**
Devuelve la última cotización de un instrumento financiero.

**Parametros:**

- symbol: símbolo del instrumento (por ejemplo "AAPL")

**Respuestas:**

**200 OK:** Objeto LastQuote

**404 Not Found:** Si la respuesta es null


### Tests
El proyecto cuenta con dos tipos de pruebas automatizadas: tests unitarios y tests de integración, desarrollados con **xUnit** y utilizando **Moq** para simular las dependencias en los tests unitarios.

#### Tests Unitarios
Los tests unitarios están ubicados en el archivo **StocksInedexesUnitTests.cs** y prueban de forma aislada la lógica de los controladores, simulando las dependencias con Moq.

**Objetivo:** Verificar que los métodos del controlador CotizacionesController respondan correctamente al recibir datos simulados del repositorio **ICotizacionesRepository**.

**Se testean los siguientes métodos:**
```bash
Método del Controlador                | Descripción
GetStocksCodes()                      | Devuelve los códigos de mercados de acciones
GetIndexesCodes()                     | Devuelve los códigos de mercados de índices
GetAllIndexes()                       | Retorna todos los índices disponibles
GetUsaIndexes()                       | Retorna los índices del mercado estadounidense
GetIndexesByExchange(string code)     | Retorna índices según un país o mercado
GetStocksByExchange(string code)      | Retorna acciones según un país o mercado
GetIndexesConstituens(string symbol)  | Retorna los constituyentes de un índice
GetLastQuoteInstrument(string symbol) | Retorna la última cotización de un instrumento
```

#### Tecnologías utilizadas:

- **xUnit** para la estructura del test

- **Moq** para simular dependencias (repositorio)

- **Assert** para verificar resultados esperados


#### Tests de integración

Los tests de integración (ubicados en el archivo **stocksIndexesIntegrationTests.cs**) verifican el comportamiento real del sistema haciendo llamadas HTTP a los endpoints expuestos por la API. Se utiliza un entorno controlado gracias a **WebApplicationFactory**.

**Objetivo:** Validar que los endpoints funcionen correctamente al integrarse todos los componentes (controlador, servicio, repositorio, etc).

**Ejemplo de lo que se testea:**

```bash
Endpoint                             | Descripción
/api/cotizaciones/stocks             | Devuelve todos los instrumentos tipo acción
/api/cotizaciones/indexes            | Devuelve todos los índices disponibles
/api/cotizaciones/lastquote/{symbol} | Devuelve la última cotización de un instrumento
```

#### Tecnologías utilizadas:

- xUnit

- HttpClient

- WebApplicationFactory de ASP.NET Core

#### Cómo ejecutar los tests:

Desde la raíz del proyecto de test, podés ejecutar todos los tests con:

```bash
	dotnet test
```

Esto correrá tanto los tests unitarios como los de integración y te mostrará un resumen con el resultado de cada prueba.


### Servicio de WebSocket: Cotizaciones en Tiempo Real

El sistema cuenta con un servicio que se conecta a una API de WebSocket para obtener precios en tiempo real de activos financieros. A continuación, se detalla cómo funciona este servicio, sus componentes y cómo interactuar con él.

####  Interfaz IPriceStreamService

Esta interfaz define los métodos y eventos que debe implementar cualquier servicio que provea cotizaciones en tiempo real.

```bash
	namespace CotizacionesDomain.Interfaces
{
    public interface IPriceStreamService
    {
        Task SubscribeAsync(IEnumerable<string> Tickers);
        Task UnsubscribeAsync(IEnumerable<string> Tickers);

        event Action<PriceData> OnPriceRecieved;
    }
}

```

- **SubscribeAsync:** permite suscribirse a una lista de tickers, acción necesaria para recibir cotizaciones en tiempo real de los tickers enviados en este metodo.

- **UnsubscribeAsync:** permite dejar de escuchar ciertos tickers.

- **OnPriceRecieved:** evento que se dispara cada vez que se recibe un nuevo precio, para notificar al resto de la aplicación.

##### Clase **PriceData.cs**

Objeto que representa los datos de una cotización.

```bash
	namespace CotizacionesDomain.Entities
{
    public class PriceData
    {
        public string Ticker { get; set; }
        public decimal Price { get; set; }
        public long Timestamp { get; set; }
        public decimal Volume { get; set; }
    }
}

```

#### Implementacion - Repositorio

Ubicada en CotizacionesInfrastructure.Repositories, esta clase implementa la interfaz **IPriceStreamService**.

**Conexión y configuración**

- Se conecta al WebSocket de Profit usando un token que se lee desde appsettings.json.

- Utiliza ClientWebSocket para establecer y mantener la conexión.

**Suscripción**

Envía un mensaje JSON como:

```bash
{
  "action": "subscribe",
  "tickers": ["YPFD", "GGAL"]
}
```

**Desuscripción**

Envía un mensaje JSON como:

```bash
{
  "action": "unsubscribe",
  "tickers": ["YPFD"]
}
```

###### Bucle de escucha
El método **RecieveLoopAsync()** se ejecuta en segundo plano, manteniendo una escucha constante del WebSocket. Cada vez que se recibe un mensaje:

- Se deserializa a un objeto PriceData.

- Se dispara el evento OnPriceRecieved.

#### Configuración de Token

Estructura en appsettings.json:

```bash
{
  "ProfitApi": {
    "Token": "tu_token_aqui"
  }
}
```

#### Clase de configuración:

```bash
public class ProfitApiSettings
{
    public string Token { get; set; }
}

```

#### Helper para cargar el token:


```bash
public static class ProfitApiSettingsLoader
{
    public static string LoadToken()
    {
        var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        var json = File.ReadAllText(jsonPath);
        var jObject = JObject.Parse(json);
        return jObject["ProfitApi"]?["Token"]?.ToString();
    }
}
```
##### Consideraciónes

- La conexión al WebSocket se realiza sólo cuando se realiza una suscripción.

- El sistema escucha mensajes en segundo plano una vez establecida la conexión.

- El evento OnPriceRecieved permite a otras partes del sistema reaccionar en tiempo real ante nuevos precios.