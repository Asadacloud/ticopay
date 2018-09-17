using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicoPayDll.Clients;
using TicoPayDll.Invoices;
using TicoPayDll.Notes;
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
                    Console.WriteLine("6: Crear Factura de Contado con Cliente Registrado y servicio registrado");
                    Console.WriteLine("7: Crear Factura de Contado Sin Registrar el Cliente y sin registrar servicios");
                    Console.WriteLine("8: Crear Factura a Credito con Cliente Registrado");
                    Console.WriteLine("9: Consultar Facturas del ultimo Mes");
                    Console.WriteLine("A: Consultar Facturas Enviadas a Tribunet Hacienda");
                    Console.WriteLine("B: Reversar una factura completa (Mediante una nota de Credito)");
                    Console.WriteLine("C: Reverso Parcial de items de una factura (Mediante una nota de Credito)");
                    Console.WriteLine("S: Salir ");
                    Opcion = Console.ReadKey().KeyChar.ToString();
                    if (Opcion.ToUpper().Contains("1"))
                    {
                        Console.WriteLine("Ejecutando Cliente Ejemplo");
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
                        Console.WriteLine("Ejecutando Consultar Clientes");
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
                        Console.WriteLine("Ejecutando Consultar Impuestos");
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
                        Console.WriteLine("Ejecutando Consultar Servicios");
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
                        Console.WriteLine("Ejecutando Crear Servicio Ejemplo ");
                        Tax[] impuestos = Metodos_Seccion_Tax.ApiTax.BuscarImpuestos(token);
                        Service servicio = new Service();
                        servicio.Name = "Eliminacion de Plagas";
                        servicio.IsRecurrent = false;
                        servicio.Price = 100;
                        servicio.TaxId = impuestos.First().Id;
                        servicio.Quantity = 1;
                        servicio.Id = "00000000-0000-0000-0000-000000000000";
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
                        Console.WriteLine("Ejecutando Crear Factura de Contado con Cliente Registrado y servicio registrado");
                        Client[] clientes = Metodos_Seccion_Client.ApiClient.BuscarClientes(token);
                        Tax[] impuestos = Metodos_Seccion_Tax.ApiTax.BuscarImpuestos(token);
                        Service[] servicios = Metodos_Seccion_Service.ApiService.BuscarServicios(token);
                        CreateInvoice factura = new CreateInvoice();
                        // Se coloca el ID del cliente en Ticopay al que se le realizara la factura
                        factura.ClientId = clientes.First().Id;
                        // Se inicializan la lista de lineas o items de la factura
                        factura.InvoiceLines = new List<ItemInvoice>();
                        // Se crea un Item
                        ItemInvoice lineaFactura = new ItemInvoice();
                        // lineaFactura.Servicio = "Nombre del item a Facturar";
                        lineaFactura.Note = "Nota de detalle del item a facturar";
                        lineaFactura.Cantidad = (decimal) 1.5;
                        lineaFactura.IdService = servicios.First().Id; // Se obtiene el id del servicio previamente creado
                        lineaFactura.Descuento = 10; // % El numero en Porcentaje
                        lineaFactura.Precio = servicios.First().Price; // Se obtiene el Precio del servicio previamente creado
                        lineaFactura.Tipo = LineType.Service; // Tipo de item ( Servicio o Producto )
                        lineaFactura.UnidadMedida = servicios.First().UnitMeasurement; // Se obtiene la unidad de medida del servicio previamente creado                        
                        decimal montoLinea = Decimal.Round(lineaFactura.Cantidad * lineaFactura.Precio,2); // Se calcula el monto base de la linea
                        lineaFactura.TotalDescuento = Decimal.Round((montoLinea * lineaFactura.Descuento) / 100,2); // Se calcula el descuento de la linea                 
                        decimal subTotal = Decimal.Round((montoLinea - lineaFactura.TotalDescuento),2); // Se calcula el subtotal
                        lineaFactura.IdImpuesto = (Guid) servicios.First().TaxId; // Se obtiene el id del impuesto del servicio previamente creado 
                        lineaFactura.Impuesto = Decimal.Round((servicios.First().Tax.Rate * subTotal) / 100,2); // Se calcula el impuesto de la linea apartir de la Tasa del impuesto
                        lineaFactura.Total = Decimal.Round(lineaFactura.Impuesto + subTotal,2); // Se calcula el Total de la factura
                        factura.InvoiceLines.Add(lineaFactura); // Se agrega la linea a la lista de lineas de la factura
                        // Seccion para simular descuento generales en la factura (Ya que hacienda no soporta descuentos generales , esto aplica el descuento a todas las lineas de la factura)
                        factura.DiscountGeneral = null;
                        factura.TypeDiscountGeneral = null;
                        // Se inicializa la lista de Pagos de la factura
                        factura.ListPaymentType = new List<PaymentInvoce>();
                        // Se crea un nuevo pago
                        PaymentInvoce formaPago = new PaymentInvoce();
                        formaPago.TypePayment = 0; // Tipo de Pago (0 Efectivo, 1 Tarjeta de Credito o Debito, 2 Cheque, 3 Deposito o Transferencia)
                        formaPago.Balance = lineaFactura.Total; // Total pagado de esta forma
                        formaPago.Trans = null; // Numero de referencia, en caso de cheque , Tarjeta, Deposito o transferencia                       
                        factura.ListPaymentType.Add(formaPago); // Se agrega la forma de pago a la factura
                        // Se coloca el tipo de moneda de la factura
                        factura.CodigoMoneda = CodigoMoneda.CRC;
                        // Se envia la factura al Api
                        Invoice facturaCreada = Metodos_Seccion_Invoice.ApiInvoice.CrearFactura(token , factura);
                        if (factura != null)
                        {
                            Console.WriteLine("Factura Creada :");
                            // El Api retorna la factura Creada en Ticopay
                            Console.WriteLine(facturaCreada.Status + " " + facturaCreada.Client.Name);
                        }
                        Console.ReadKey();
                    }
                    if (Opcion.ToUpper().Contains("7"))
                    {
                        Console.WriteLine("Crear Factura de Contado Sin Registrar el Cliente y sin registrar servicios");
                        Client[] clientes = Metodos_Seccion_Client.ApiClient.BuscarClientes(token);
                        Tax[] impuestos = Metodos_Seccion_Tax.ApiTax.BuscarImpuestos(token);
                        Service[] servicios = Metodos_Seccion_Service.ApiService.BuscarServicios(token);
                        CreateInvoice factura = new CreateInvoice();
                        // Como no se creara un cliente se coloca el ID del cliente en null
                        factura.ClientId = null;
                        // Se rellenan los datos del cliente directo en la factura
                        factura.ClientIdentificationType = IdentificacionTypeTipo.Cedula_Fisica;
                        factura.ClientIdentification = "123456789";
                        factura.ClientName = "Pedro Perez";
                        factura.ClientEmail = "prueba@correodeprueba.com";
                        // El resto de los campos del cliente (ClientMobilNumber y ClientPhoneNumber, ) son opcionales para la facturacion electronica
                        // Se inicializan la lista de lineas o items de la factura
                        factura.InvoiceLines = new List<ItemInvoice>();
                        // Se crea un Item
                        ItemInvoice lineaFactura = new ItemInvoice();
                        // lineaFactura.Servicio = "Nombre del item a Facturar";
                        lineaFactura.Note = "Nota de detalle del item a facturar";
                        lineaFactura.Servicio = "Nombre del item"; // Al no usar un servicio o producto del sistema debemos rellenar el nombre del item a facturar
                        lineaFactura.Cantidad = (decimal)1.5;
                        lineaFactura.IdService = null; // como facturaremos un item sin registrarlo previamente se coloca en null
                        lineaFactura.Descuento = 10; // % El numero en Porcentaje
                        lineaFactura.Precio = 110; // Se coloca el precio del producto
                        lineaFactura.Tipo = LineType.Service; // Tipo de item ( Servicio o Producto )
                        lineaFactura.UnidadMedida = UnidadMedidaType.Servicios_Profesionales; // Se coloca la unidad de medida del item                        
                        decimal montoLinea = Decimal.Round(lineaFactura.Cantidad * lineaFactura.Precio, 2); // Se calcula el monto base de la linea
                        lineaFactura.TotalDescuento = Decimal.Round((montoLinea * lineaFactura.Descuento) / 100, 2); // Se calcula el descuento de la linea                 
                        decimal subTotal = Decimal.Round((montoLinea - lineaFactura.TotalDescuento), 2); // Se calcula el subtotal
                        lineaFactura.IdImpuesto = impuestos.First().Id; // Se obtiene el id del impuesto de uno de los impuestos previamente creados 
                        lineaFactura.Impuesto = Decimal.Round((impuestos.First().Rate * subTotal) / 100, 2); // Se calcula el impuesto de la linea apartir de la Tasa del impuesto
                        lineaFactura.Total = Decimal.Round(lineaFactura.Impuesto + subTotal, 2); // Se calcula el Total de la factura
                        factura.InvoiceLines.Add(lineaFactura); // Se agrega la linea a la lista de lineas de la factura
                        // Seccion para simular descuento generales en la factura (Ya que hacienda no soporta descuentos generales , esto aplica el descuento a todas las lineas de la factura)
                        factura.DiscountGeneral = null;
                        factura.TypeDiscountGeneral = null;
                        // Se inicializa la lista de Pagos de la factura
                        factura.ListPaymentType = new List<PaymentInvoce>();
                        // Se crea un nuevo pago
                        PaymentInvoce formaPago = new PaymentInvoce();
                        formaPago.TypePayment = 0; // Tipo de Pago (0 Efectivo, 1 Tarjeta de Credito o Debito, 2 Cheque, 3 Deposito o Transferencia)
                        formaPago.Balance = lineaFactura.Total; // Total pagado de esta forma
                        formaPago.Trans = null; // Numero de referencia, en caso de cheque , Tarjeta, Deposito o transferencia                       
                        factura.ListPaymentType.Add(formaPago); // Se agrega la forma de pago a la factura
                        // Se coloca el tipo de moneda de la factura
                        factura.CodigoMoneda = CodigoMoneda.CRC;
                        // Se envia la factura al Api
                        Invoice facturaCreada = Metodos_Seccion_Invoice.ApiInvoice.CrearFactura(token, factura);
                        if (facturaCreada != null)
                        {
                            Console.WriteLine("Factura Creada :");
                            // El Api retorna la factura Creada en Ticopay
                            Console.WriteLine(facturaCreada.Status + " " + facturaCreada.ClientName);
                        }
                        Console.ReadKey();
                    }
                    if (Opcion.ToUpper().Contains("8"))
                    {
                        Console.WriteLine("Ejecutando Crear Factura a Credito con Cliente Registrado");
                        Client[] clientes = Metodos_Seccion_Client.ApiClient.BuscarClientes(token);
                        Tax[] impuestos = Metodos_Seccion_Tax.ApiTax.BuscarImpuestos(token);
                        Service[] servicios = Metodos_Seccion_Service.ApiService.BuscarServicios(token);
                        CreateInvoice factura = new CreateInvoice();
                        // Se coloca el ID del cliente en Ticopay al que se le realizara la factura
                        factura.ClientId = clientes.First().Id;
                        // Se inicializan la lista de lineas o items de la factura
                        factura.InvoiceLines = new List<ItemInvoice>();
                        // Se crea un Item
                        ItemInvoice lineaFactura = new ItemInvoice();
                        // lineaFactura.Servicio = "Nombre del item a Facturar";
                        lineaFactura.Note = "Nota de detalle del item a facturar";
                        lineaFactura.Cantidad = (decimal)1.5;
                        lineaFactura.IdService = servicios.First().Id; // Se obtiene el id del servicio previamente creado
                        lineaFactura.Descuento = 10; // % El numero en Porcentaje
                        lineaFactura.Precio = servicios.First().Price; // Se obtiene el Precio del servicio previamente creado
                        lineaFactura.Tipo = LineType.Service; // Tipo de item ( Servicio o Producto )
                        lineaFactura.UnidadMedida = servicios.First().UnitMeasurement; // Se obtiene la unidad de medida del servicio previamente creado                        
                        decimal montoLinea = Decimal.Round(lineaFactura.Cantidad * lineaFactura.Precio, 2); // Se calcula el monto base de la linea
                        lineaFactura.TotalDescuento = Decimal.Round((montoLinea * lineaFactura.Descuento) / 100, 2); // Se calcula el descuento de la linea                 
                        decimal subTotal = Decimal.Round((montoLinea - lineaFactura.TotalDescuento), 2); // Se calcula el subtotal
                        lineaFactura.IdImpuesto = (Guid)servicios.First().TaxId; // Se obtiene el id del impuesto del servicio previamente creado 
                        lineaFactura.Impuesto = Decimal.Round((servicios.First().Tax.Rate * subTotal) / 100, 2); // Se calcula el impuesto de la linea apartir de la Tasa del impuesto
                        lineaFactura.Total = Decimal.Round(lineaFactura.Impuesto + subTotal, 2); // Se calcula el Total de la factura
                        factura.InvoiceLines.Add(lineaFactura); // Se agrega la linea a la lista de lineas de la factura
                        // Seccion para simular descuento generales en la factura (Ya que hacienda no soporta descuentos generales , esto aplica el descuento a todas las lineas de la factura)
                        factura.DiscountGeneral = null;
                        factura.TypeDiscountGeneral = null;
                        // Debido a que la factura sera a credito , se deja el campo de pagos en null
                        factura.ListPaymentType = null;
                        // Se debe colocar obligatoriamente la cantidad de dias de credito para la factura
                        factura.CreditTerm = 5;
                        // Se coloca el tipo de moneda de la factura
                        factura.CodigoMoneda = CodigoMoneda.CRC;
                        // Se envia la factura al Api
                        Invoice facturaCreada = Metodos_Seccion_Invoice.ApiInvoice.CrearFactura(token, factura);
                        if (factura != null)
                        {
                            Console.WriteLine("Factura Creada :");
                            // El Api retorna la factura Creada en Ticopay
                            Console.WriteLine(facturaCreada.Status + " " + facturaCreada.Client.Name);
                        }
                        Console.ReadKey();
                    }
                    if (Opcion.ToUpper().Contains("9"))
                    {
                        Console.WriteLine("Ejecutando Consultar Facturas del ultimo Mes");
                        Invoice[] facturas = Metodos_Seccion_Invoice.ApiInvoice.BuscarFacturas(token);
                        Console.WriteLine(facturas.Length + " encontrados");
                        foreach (Invoice factura in facturas)
                        {
                            Console.WriteLine(factura.ConsecutiveNumber + " " + factura.Balance + " " + factura.Status);
                        }
                        Console.ReadKey();
                    }
                    if (Opcion.ToUpper().Contains("A"))
                    {
                        Console.WriteLine("Ejecutando Consultar Facturas Enviadas a Tribunet");
                        InvoiceSendTribunet[] reporte = Metodos_Seccion_Invoice.ApiInvoice.ReporteEstatusFacturasTribunet(token);
                        Console.WriteLine(reporte.Length + " encontrados");
                        foreach (InvoiceSendTribunet factura in reporte)
                        {
                            Console.WriteLine(factura.NombreCliente + " " + factura.NumeroFactura + " " + factura.StatusTribunet);
                        }
                        Console.ReadKey();
                    }
                    if (Opcion.ToUpper().Contains("B"))
                    {
                        Console.WriteLine("Ejecutando Reversar una factura completa (Mediante una nota de Credito)");
                        Invoice[] facturas = Metodos_Seccion_Invoice.ApiInvoice.BuscarFacturas(token);                        
                        //facturas.First().Id;
                        Console.WriteLine("");
                        Console.ReadKey();
                    }
                    if (Opcion.ToUpper().Contains("C"))
                    {
                        Console.WriteLine("Ejecutando Reversar una factura completa (Mediante una nota de Credito)");
                        Invoice[] facturas = Metodos_Seccion_Invoice.ApiInvoice.BuscarFacturas(token);
                        Invoice facturaAReversarParcialmente = facturas.Where(f => f.InvoiceLines.Count >= 2).FirstOrDefault();
                        // Creamos una nueva nota de credito
                        CompleteNote notaACrear = new CompleteNote();
                        // Identificamos a que factura o tiquete se le esta aplicando la nota
                        notaACrear.InvoiceId = Guid.Parse(facturaAReversarParcialmente.Id);
                        notaACrear.NumberInvoiceRef = facturaAReversarParcialmente.ConsecutiveNumber;
                        notaACrear.ExternalReferenceNumber = "N/A"; // Numero de referencia externo usando por su sistema
                        // Rellenamos los datos principales de la nota (Basados en la factura que vamos a reversar)                        
                        notaACrear.CodigoMoneda = facturaAReversarParcialmente.CodigoMoneda; // Moneda de la nota
                        notaACrear.NoteType = NoteType.Crédito; // Tipo de nota                        
                        notaACrear.NoteReasons = NoteReason.Corregir_Monto_Factura; // Motivo de la nota
                        notaACrear.TipoFirma = facturaAReversarParcialmente.TipoFirma; // Tipo de firma
                        notaACrear.IsNoteReceptionConfirmed = false;
                        notaACrear.NoteReasonsOthers = null;                        
                        notaACrear.DueDate = DateTime.UtcNow;
                        notaACrear.Status = Status.Completed;
                        notaACrear.ConditionSaleType = TicoPayDll.Notes.FacturaElectronicaCondicionVenta.Otros;
                        notaACrear.CreditTerm = 0;
                        // Inicializamos el objeto para las lineas de la nota
                        notaACrear.NotesLines = new List<NoteLineDto>();
                        NoteLineDto lineaNotaDetalle = new NoteLineDto();
                        int numeroLinea = 1;
                        // Para el ejemplo solo tomamos la primera linea de la factura para realizar la devolucion
                        lineaNotaDetalle.PricePerUnit = facturaAReversarParcialmente.InvoiceLines[0].PricePerUnit; // Precio del item
                        lineaNotaDetalle.SubTotal = facturaAReversarParcialmente.InvoiceLines[0].SubTotal; // Sub total del item
                        lineaNotaDetalle.LineTotal = facturaAReversarParcialmente.InvoiceLines[0].LineTotal; // Sub total de linea
                        lineaNotaDetalle.TaxId = facturaAReversarParcialmente.InvoiceLines[0].TaxId; // Id del impuesto de la linea
                        lineaNotaDetalle.TaxAmount = facturaAReversarParcialmente.InvoiceLines[0].TaxAmount; // Monto del impuesto de la linea
                        lineaNotaDetalle.Total = facturaAReversarParcialmente.InvoiceLines[0].Total; // Total de la linea
                        lineaNotaDetalle.Note = facturaAReversarParcialmente.InvoiceLines[0].Note; // nota descriptiva de la linea
                        lineaNotaDetalle.Title = facturaAReversarParcialmente.InvoiceLines[0].Title; // Nombre del item de la linea
                        lineaNotaDetalle.Quantity = facturaAReversarParcialmente.InvoiceLines[0].Quantity; // Cantidad de items de la linea
                        lineaNotaDetalle.LineType = facturaAReversarParcialmente.InvoiceLines[0].LineType; // Tipo de linea
                        // Si la linea esta basada en un producto o servicio registrado se guarda el id del mismo
                        if (facturaAReversarParcialmente.InvoiceLines[0].ProductId != null)
                        {
                            lineaNotaDetalle.ProductId = facturaAReversarParcialmente.InvoiceLines[0].ProductId;
                        }
                        if (facturaAReversarParcialmente.InvoiceLines[0].ServiceId != null)
                        {
                            lineaNotaDetalle.ServiceId = facturaAReversarParcialmente.InvoiceLines[0].ServiceId;
                        }
                        lineaNotaDetalle.LineNumber = numeroLinea; // Numero de linea
                        lineaNotaDetalle.DescriptionDiscount = null; 
                        lineaNotaDetalle.ExonerationId = null;
                        lineaNotaDetalle.UnitMeasurement = facturaAReversarParcialmente.InvoiceLines[0].UnitMeasurement; // Unidad de medida de la linea
                        if (facturaAReversarParcialmente.InvoiceLines[0].UnitMeasurementOthers != null)
                        {
                            lineaNotaDetalle.UnitMeasurementOthers = facturaAReversarParcialmente.InvoiceLines[0].UnitMeasurementOthers;
                        }
                        else
                        {
                            lineaNotaDetalle.UnitMeasurementOthers = null;
                        }
                        // Calculamos los totales de la nota
                        notaACrear.Amount = lineaNotaDetalle.SubTotal;
                        notaACrear.TaxAmount = lineaNotaDetalle.TaxAmount;
                        notaACrear.DiscountAmount = 0;
                        notaACrear.Total = lineaNotaDetalle.Total;
                        notaACrear.NotesLines.Add(lineaNotaDetalle);
                        CompleteNote notaCreada = Metodos_Seccion_Invoice.ApiNotes.CrearNote(token, notaACrear);
                        if (notaCreada != null)
                        {
                            Console.WriteLine("Nota Parcial Creada :");
                            Console.WriteLine(notaCreada.Status + " " + notaCreada.ConsecutiveNumber);
                        }
                        Console.WriteLine("");
                        Console.ReadKey();
                    }
                    if (Opcion.ToUpper().Contains("S"))
                    {
                        Ejecutando = false;
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
