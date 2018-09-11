using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicoPayDll.Response;
using TicoPayDll.Taxes;
using static TicoPayDll.Taxes.TaxesController;

namespace EjemploTicopayDll.Metodos_Seccion_Tax
{
    public class ApiTax
    {
        public static Tax[] BuscarImpuestos(string token)
        {
            Response respuestaServicio;
            respuestaServicio = TicoPayDll.Taxes.TaxesController.Gettaxes(token).GetAwaiter().GetResult();
            if (respuestaServicio.status == ResponseType.Ok)
            {
                JsonTaxes impuestos = JsonConvert.DeserializeObject<JsonTaxes>(respuestaServicio.result);
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
