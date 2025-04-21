using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CotizacionesDomain.Entities;
using CotizacionesDomain.Interfaces;
using System.Net.WebSockets;
using Newtonsoft.Json;
using System.Threading;
using CotizacionesInfrastructure.Helpers;

namespace CotizacionesInfrastructure.Repositories
{
    public class PriceStreamService : IPriceStreamService
    {
        //Esta variable es el cliente WebSocket de .NET, que permite abrir y mantener una conexión WebSocket.
        private readonly ClientWebSocket _webSocket = new ClientWebSocket();

        private readonly Uri _uri;

        //permite cancelar operaciones asincronas, en caso de necesitar cerrar la conexion, esta variable sera usada
        private readonly CancellationTokenSource _cts;

        //private readonly JsonSerializerSettings _jsonOprions = new JsonSerializerSettings();

        //Evento para notificar a otras partes de la app cuando hay nuevos datos disponibles
        public event Action<PriceData> OnPriceRecieved;

        //Constructor
        public PriceStreamService()
        {
            var token = ProfitApiSettingsLoader.LoadToken();
            _uri = new Uri($"wss://api.profit.com/real-time?token={token}");
            _cts = new CancellationTokenSource();
        }


        public async Task SubscribeAsync(IEnumerable<string> tickers)
        {

            try
            {
                if (_webSocket.State != WebSocketState.Open)
                    await _webSocket.ConnectAsync(_uri, _cts.Token);
                // _ = accion --> es una convencion para decir que no importa lo que devuelva quiero ejecutar eso en segundo plano
                _ = RecieveLoopAsync();

                var message = new
                {
                    action = "subscribe",
                    tickers = tickers.ToArray()
                };

                //Convierto el objeto message en JSON para poder enviarlo como mensaje de websocket
                string json = JsonConvert.SerializeObject(message);

                //Codigo el mensaje de json en bytes usando UTF-8. porque SendAsync() require byte y no string o json
                var buffer = Encoding.UTF8.GetBytes(json);

                //Crea un ArraySegment<byte> con el buffer, que es el tipo que espera el método SendAsync.
                //Por qué: Este segmento es lo que se transmite al WebSocket.
                var segment = new ArraySegment<byte>(buffer);

                //envia el mensaje al servidor de websocket
                //WebSocketMessageType.Text -> indica que el mensaje es texto
                //true -> indica que e el ultimo segmento del mensaje
                //_cts.Token -> En caso de ser necesaria una cancelacion, se usa.
                await _webSocket.SendAsync(segment, WebSocketMessageType.Text, true, _cts.Token);
            }
            catch (WebSocketException ex)
            {
                throw new Exception($"Error al conectar o enviar mensaje por WebSocket. Error: {ex.Message}", ex);
            }
            catch (TimeoutException ex)
            {
                throw new Exception($"La operación de WebSocket ha excedido el tiempo de espera. Error: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Error al serializar el mensaje de suscripción. Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado. Error: {ex.Message}", ex);
            }
        }

        public async Task UnsubscribeAsync(IEnumerable<string> tickers)
        {
            try
            {
                var message = new
                {
                    action = "unsubscribe",
                    tickers = tickers.ToArray()
                };

                string json = JsonConvert.SerializeObject(message);
                var buffer = Encoding.UTF8.GetBytes(json);
                var segment = new ArraySegment<byte>(buffer);

                await _webSocket.SendAsync(segment, WebSocketMessageType.Text, true, _cts.Token);
            }
            catch (WebSocketException ex)
            {
                throw new Exception($"Error al conectar o enviar mensaje por WebSocket. Error: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Error al serializar el mensaje de suscripción. Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado. Error: {ex.Message}", ex);
            }
        }

        //Metodo que escucha al websocket permanentemente para estar recibiendo informacion si se esta enviando
        private async Task RecieveLoopAsync()
        {

            try
            {
                //Necesito un espacio donde guardar los datos que llegan, en este caso el mensaje que envie el servidor de websocket
                var buffer = new byte[1024 * 4];

                //Mientras no se haya cancelado el proceso y la conexion siga abierta, se sigue escuchando
                while (!_cts.IsCancellationRequested && _webSocket.State == WebSocketState.Open)
                {
                    //Este es el punto donde se reciben los datos de la api websocket. es decir, espera asincronamente un mensaje y lo guarda en el buffer(buffer = espacio en memoria, en este caso 4KB)
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cts.Token);

                    //Manejo automatico de cierre de conexion (ya que es un bucle infinito mientras se escuche el servidor, siempre estara escuchando atento)
                    //si detecta que el cliente o servidor cerro la conexion, entonces deja de escuchar
                    if (result.MessageType == WebSocketMessageType.Close)
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    break;


                    //Convierte el contenido del buffer en una cadena JSON, usando solo la cantidad de bytes reales recibidos.
                    var json = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    try
                    {
                        var priceData = JsonConvert.DeserializeObject<PriceData>(json); //Verificar mapeo de propiedades (mayusculas y minusculas)
                                                                                        //Si recibe un objeto valido, lanza el evento para notificar el resto de la app que hay datos nuevos
                                                                                        //Aca ocurre la notificacion en tiempo real
                        if (priceData != null)
                            OnPriceRecieved?.Invoke(priceData);
                    }
                    catch (JsonException ex)
                    {
                        throw new Exception($"Error al deserializar los datos de precio recibidos. Error: {ex.Message}", ex);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error inesperado al procesar los datos del WebSocket. Error: {ex.Message}", ex);
                    }
                }
            }
            catch (WebSocketException ex)
            {
                throw new Exception($"Error en la conexión al websocket en la recepción de datos. Error: {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"La operación de recepción fue cancelada. Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado. Error: {ex.Message}", ex);
            }
        }

    }
}
