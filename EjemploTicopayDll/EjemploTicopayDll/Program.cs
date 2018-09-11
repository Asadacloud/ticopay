using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicoPayDll.Clients;
using TicoPayDll.Invoices;
using TicoPayDll.Reports;
using TicoPayDll.Services;
using TicoPayDll.Taxes;

namespace EjemploTicopayDll
{
    class Program
    {
        static void Main(string[] args)
        {
            string token = "";
            string tenancy = "tutorial";
            string user = "tutorial";
            string password = "P@ssw0rd";
            Console.WriteLine("Realizando login en " + tenancy);
            token = Metodos_Seccion_Account.ApiAccount.AutentificarUsuario(tenancy, user, password);
            if (token != "")
            {
                string Opcion = "";
                bool Ejecutando = true;
                Console.WriteLine("Token asignado: " + token);
                Console.WriteLine("Este Token tiene una duracion de 20 min, para extenderlo utilize la funcion RefreshToken");
                Console.ReadKey();
                while (Ejecutando)
                {
                    Console.Clear();
                    Console.WriteLine("Seleccione Operacion a Realizar: ");
                    Console.WriteLine("1: Insertar Cliente Ejemplo ");
                    Console.WriteLine("2: Consultar Clientes ");
                    Console.WriteLine("3: Consultar Impuestos ");
                    Console.WriteLine("4: Consultar Servicios ");
                    Console.WriteLine("5: Crear Servicio Ejemplo ");
                    Console.WriteLine("6: Crear Factura Ejemplo ");
                    Console.WriteLine("7: Consultar Facturas ");
                    Console.WriteLine("8: Consultar Facturas Enviadas a Tribunet ");
                    Console.WriteLine("9: Consultar PDF");
                    Console.WriteLine("S: Salir ");
                    Opcion = Console.ReadKey().KeyChar.ToString();
                    if (Opcion.ToUpper().Contains("1"))
                    {
                        Console.WriteLine("Ejecutando");
                        Client cliente = new Client();
                        cliente.Name = "Pedro";
                        cliente.LastName = "Perez";
                        cliente.IdentificationType = IdentificacionTypeTipo.Cedula_Fisica;
                        cliente.Identification = "923456789";
                        cliente.Email = "Ejemplo@ejemplo.ejm";
                        Client clienteCreado = Metodos_Seccion_Client.ApiClient.CrearCliente(token, cliente);
                        if (cliente != null)
                        {
                            Console.WriteLine("Cliente Creado :");
                            Console.WriteLine(clienteCreado.Name + " " + clienteCreado.LastName + " " + clienteCreado.Identification);
                        }
                        Console.ReadKey();
                    }
                    if (Opcion.ToUpper().Contains("2"))
                    {
                        Console.WriteLine("Ejecutando");
                        Client[] clientes = Metodos_Seccion_Client.ApiClient.BuscarClientes(token);
                        Console.WriteLine(clientes.Length + " encontrados");
                        foreach (Client cliente in clientes)
                        {
                            Console.WriteLine(cliente.Name + " " + cliente.LastName + " " + cliente.Identification);
                        }
                        Console.ReadKey();
                    }
                    if (Opcion.ToUpper().Contains("3"))
                    {
                        Console.WriteLine("Ejecutando");
                        Tax[] impuestos = Metodos_Seccion_Tax.ApiTax.BuscarImpuestos(token);
                        Console.WriteLine(impuestos.Length + " encontrados");
                        foreach (Tax impuesto in impuestos)
                        {
                            Console.WriteLine(impuesto.Name + " " + impuesto.Rate + " " + impuesto.TaxTypes);
                        }
                        Console.ReadKey();
                    }
                    if (Opcion.ToUpper().Contains("4"))
                    {
                        Console.WriteLine("Ejecutando");
                        Service[] servicios = Metodos_Seccion_Service.ApiService.BuscarServicios(token);
                        Console.WriteLine(servicios.Length + " encontrados");
                        foreach (Service servicio in servicios)
                        {
                            Console.WriteLine(servicio.Name + " " + servicio.Price);
                        }
                        Console.ReadKey();
                    }
                    if (Opcion.ToUpper().Contains("5"))
                    {
                        Console.WriteLine("Ejecutando");
                        Tax[] impuestos = Metodos_Seccion_Tax.ApiTax.BuscarImpuestos(token);
                        Service servicio = new Service();
                        servicio.Name = "Eliminacion de Plagas";
                        servicio.Price = 100;
                        servicio.TaxId = impuestos.First().Id;
                        servicio.Quantity = 1;
                        servicio.UnitMeasurement = UnidadMedidaType.Servicios_Profesionales;
                        Service servicioCreado = Metodos_Seccion_Service.ApiService.CrearServicio(token, servicio);
                        if (servicioCreado != null)
                        {
                            Console.WriteLine("Servicio Creado :");
                            Console.WriteLine(servicioCreado.Name + " " + servicioCreado.Price);
                        }
                        Console.ReadKey();
                    }
                    if (Opcion.ToUpper().Contains("6"))
                    {
                        Console.WriteLine("Ejecutando");
                        Invoice factura = CrearFactura(token);
                        if (factura != null)
                        {
                            Console.WriteLine("Factura Creada :");
                            Console.WriteLine(factura.Status + " " + factura.Client.Name);
                        }
                        Console.ReadKey();
                    }
                    if (Opcion.ToUpper().Contains("7"))
                    {
                        Console.WriteLine("Ejecutando");
                        Invoice[] facturas = BuscarFacturas(token);
                        Console.WriteLine(facturas.Length + " encontrados");
                        foreach (Invoice factura in facturas)
                        {
                            Console.WriteLine(factura.Client.Name + " " + factura.Balance + " " + factura.Status);
                        }
                        Console.ReadKey();
                    }
                    if (Opcion.ToUpper().Contains("8"))
                    {
                        Console.WriteLine("Ejecutando");
                        InvoiceSendTribunet[] reporte = ReporteEstatusFacturasTribunet(token);
                        Console.WriteLine(reporte.Length + " encontrados");
                        foreach (InvoiceSendTribunet factura in reporte)
                        {
                            Console.WriteLine(factura.NombreCliente + " " + factura.NumeroFactura + " " + factura.StatusTribunet);
                        }
                        Console.ReadKey();
                    }
                    if (Opcion.ToUpper().Contains("9"))
                    {
                        Console.WriteLine("Ejecutando");
                        PDF reporte = GetInvoicePDF(token, "f8d073a7-d5f0-45c1-a53c-1fb64344f3df").objectResponse;
                        Console.WriteLine(reporte.FileName + " encontrado bytes " + reporte.Data.Length);
                        Console.ReadKey();
                    }
                }
                Console.WriteLine("Hasta luego ");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Error al Conectar con Ticopay");
                Console.ReadKey();
            }

        }
    }
}
