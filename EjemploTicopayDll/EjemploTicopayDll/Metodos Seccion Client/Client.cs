using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicoPayDll.Clients;
using TicoPayDll.Response;
using static TicoPayDll.Clients.ClientController;

namespace EjemploTicopayDll.Metodos_Seccion_Client
{
    public class ApiClient
    {
        /// <summary>
        /// Crea un Cliente con los datos proporcionados.
        /// </summary>
        /// <param name="token">Token de Sesión.</param>
        /// <param name="clienteACrear">Cliente a crear.</param>
        /// <returns>Cliente Creado</returns>
        public static Client CrearCliente(string token, Client clienteACrear)
        {
            Response respuestaServicio;            
            respuestaServicio = TicoPayDll.Clients.ClientController.CreateNewClient(clienteACrear, token).GetAwaiter().GetResult();
            if (respuestaServicio.status == ResponseType.Ok)
            {
                JsonCreateClient clientes = JsonConvert.DeserializeObject<JsonCreateClient>(respuestaServicio.result);
                return clientes.objectResponse;
            }
            else
            {
                Console.WriteLine(respuestaServicio.message);
                return null;
            }

        }

        /// <summary>
        /// Obtiene la lista de todos los clientes del Tenant en Ticopay.
        /// </summary>
        /// <param name="token">Token de Sesión.</param>
        /// <returns>Lista de Clientes</returns>
        public static Client[] BuscarClientes(string token)
        {
            Response respuestaServicio;
            respuestaServicio = TicoPayDll.Clients.ClientController.GetClients(token, false).GetAwaiter().GetResult();
            if (respuestaServicio.status == ResponseType.Ok)
            {
                JsonClients clientes = JsonConvert.DeserializeObject<JsonClients>(respuestaServicio.result);
                return clientes.listObjectResponse;
            }
            else
            {
                Console.WriteLine(respuestaServicio.message);
                return null;
            }
        }

        /// <summary>
        /// Obtiene un cliente, buscando por el numero de identificación.
        /// </summary>
        /// <param name="token">Token de Sesión.</param>
        /// <param name="identificacion">Numero de Identificación del Cliente.</param>
        /// <returns>Cliente especifico</returns>
        public static Client BuscarCliente(string token, string identificacion)
        {
            Response respuestaServicio = new Response();
            respuestaServicio = SearchClients(token, true, identificacion).GetAwaiter().GetResult();
            if (respuestaServicio.status == ResponseType.Ok)
            {
                JsonSearchClient clientes = JsonConvert.DeserializeObject<JsonSearchClient>(respuestaServicio.result);
                return clientes.objectResponse;
            }
            else
            {
                Console.WriteLine(respuestaServicio.message);
                return null;
            }
        }
    }
}
