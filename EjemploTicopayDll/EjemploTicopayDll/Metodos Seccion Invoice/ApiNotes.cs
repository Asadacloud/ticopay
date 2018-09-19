using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicoPayDll.Notes;
using TicoPayDll.Response;

namespace EjemploTicopayDll.Metodos_Seccion_Invoice
{
    public class ApiNotes
    {
        public static CompleteNote CrearNote(string token, CompleteNote NotaACrear)
        {
            Response respuestaServicio;
            respuestaServicio = NoteController.CreateNewNote(NotaACrear, token,true).GetAwaiter().GetResult();
            if (respuestaServicio.status == ResponseType.Ok)
            {
                JsonCreateNote invoice = JsonConvert.DeserializeObject<JsonCreateNote>(respuestaServicio.result);
                return invoice.objectResponse;
            }
            else
            {
                Console.WriteLine(respuestaServicio.message);
                return null;
            }
        }

        public static CompleteNote ReversarFacturaOTiquete(string token, string idInvoiceOrTicket, string externalReferenceNumber)
        {
            TicoPayDll.Response.Response respuestaServicio;
            respuestaServicio = TicoPayDll.Notes.NoteController.ReverseInvoiceOrTicket(idInvoiceOrTicket, token, externalReferenceNumber).GetAwaiter().GetResult();
            if (respuestaServicio.status == ResponseType.Ok)
            {
                JsonCreateNote nota = JsonConvert.DeserializeObject<JsonCreateNote>(respuestaServicio.result);
                return nota.objectResponse;
            }
            else
            {
                Console.WriteLine(respuestaServicio.message);
                return null;
            }
        }
    }
}
