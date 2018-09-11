using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicoPayDll.Invoices;
using TicoPayDll.Reports;
using TicoPayDll.Response;

namespace EjemploTicopayDll.Metodos_Seccion_Invoice
{
    public class ApiInvoice
    {
        public static Invoice CrearFactura(string token, CreateInvoice facturaACrear)
        {
            Response respuestaServicio;            
            respuestaServicio = TicoPayDll.Invoices.InvoiceController.CreateNewInvoice(facturaACrear, token).GetAwaiter().GetResult();
            if (respuestaServicio.status == ResponseType.Ok)
            {
                JsonCreateInvoice invoice = JsonConvert.DeserializeObject<JsonCreateInvoice>(respuestaServicio.result);
                return invoice.objectResponse;
            }
            else
            {
                Console.WriteLine(respuestaServicio.message);
                return null;
            }
        }

        public static Invoice[] BuscarFacturas(string token)
        {
            TicoPayDll.Response.Response respuestaServicio;
            InvoiceSearchConfiguration parametrosBusqueda = new InvoiceSearchConfiguration();
            parametrosBusqueda.ClientId = null;
            parametrosBusqueda.InvoiceId = null;
            parametrosBusqueda.Status = InvoiceStatus.Pagada;
            parametrosBusqueda.EndDueDate = null;
            parametrosBusqueda.StartDueDate = null;
            respuestaServicio = TicoPayDll.Invoices.InvoiceController.GetInvoices(parametrosBusqueda, token).GetAwaiter().GetResult();
            if (respuestaServicio.status == ResponseType.Ok)
            {
                JsonInvoices facturas = JsonConvert.DeserializeObject<JsonInvoices>(respuestaServicio.result);
                return facturas.listObjectResponse;
            }
            else
            {
                Console.WriteLine(respuestaServicio.message);
                return null;
            }
        }

        public List<InvoiceSendTribunet> ObtenerFacturaTicoPayHacienda(ReportInvoicesSentToTribunetSearchInput parameter, string token)
        {
            Response respuestaServicio = new Response();
            respuestaServicio = TicoPayDll.Reports.ReportsController.GetInvoicesSendToTribunet(parameter, token).GetAwaiter().GetResult();
            if (respuestaServicio.status == ResponseType.Ok)
            {
                JsonInvoicesSendToTribunet invoiceSendTribunet = JsonConvert.DeserializeObject<JsonInvoicesSendToTribunet>(respuestaServicio.result);
                return invoiceSendTribunet.invoices.ToList();
            }
            else
            {
                Console.WriteLine(respuestaServicio.message);
                return null;
            }
        }

        public static InvoiceSendTribunet[] ReporteEstatusFacturasTribunet(string token)
        {
            TicoPayDll.Response.Response respuestaServicio;
            ReportInvoicesSentToTribunetSearchInput parametrosBusqueda = new ReportInvoicesSentToTribunetSearchInput();
            parametrosBusqueda.StatusTribunet = TicoPayDll.Reports.StatusTaxAdministration.Rechazado;
            parametrosBusqueda.RecepcionConfirmada = true;
            parametrosBusqueda.type = "json";
            respuestaServicio = TicoPayDll.Reports.ReportsController.GetInvoicesSendToTribunet(parametrosBusqueda, token).GetAwaiter().GetResult();
            if (respuestaServicio.status == ResponseType.Ok)
            {
                JsonInvoicesSendToTribunet facturas = JsonConvert.DeserializeObject<JsonInvoicesSendToTribunet>(respuestaServicio.result);
                return facturas.invoices;
            }
            else
            {
                Console.WriteLine(respuestaServicio.message);
                return null;
            }
        }

        public static JsonInvoicePDF GetInvoicePDF(string token, string id)
        {

            Response respuestaServicio = new Response();
            respuestaServicio = TicoPayDll.Invoices.InvoiceController.GetInvoicePDF(id, token).GetAwaiter().GetResult();
            if (respuestaServicio.status == ResponseType.Ok)
            {
                //JsonInvoicePDF getInvoicePDF = JsonInvoicePDF(respuestaServicio.result);
                //return getInvoicePDF.objectResponse;
                if (respuestaServicio.result != null)
                {
                    var JsonInvoicePDF = JsonConvert.DeserializeObject<JsonInvoicePDF>(respuestaServicio.result);
                    File.WriteAllBytes(JsonInvoicePDF.objectResponse.FileName + ".pdf", JsonInvoicePDF.objectResponse.Data);
                    return JsonInvoicePDF;
                }
                return null;
            }
            else
            {
                Console.WriteLine(respuestaServicio.message);
                return null;
            }
        }
    }
}
