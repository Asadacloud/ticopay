using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicoPayDll.Response;
using TicoPayDll.Services;
using static TicoPayDll.Services.ServiceController;

namespace EjemploTicopayDll.Metodos_Seccion_Service
{
    public class ApiService
    {
        public static Service CrearServicio(string token , Service servicioACrear)
        {
            Response respuestaServicio;            
            respuestaServicio = TicoPayDll.Services.ServiceController.CreateNewService(servicioACrear, token).GetAwaiter().GetResult();
            if (respuestaServicio.status == ResponseType.Ok)
            {
                JsonCreateService service = JsonConvert.DeserializeObject<JsonCreateService>(respuestaServicio.result);
                return service.objectResponse;
            }
            else
            {
                Console.WriteLine(respuestaServicio.message);
                return null;
            }
        }

        public static Service[] BuscarServicios(string token)
        {
            Response respuestaServicio;
            respuestaServicio = TicoPayDll.Services.ServiceController.GetServices(token).GetAwaiter().GetResult();
            if (respuestaServicio.status == ResponseType.Ok)
            {
                JsonServices impuestos = JsonConvert.DeserializeObject<JsonServices>(respuestaServicio.result);
                return impuestos.listObjectResponse;
            }
            else
            {
                Console.WriteLine(respuestaServicio.message);
                return null;
            }
        }
    }
}
