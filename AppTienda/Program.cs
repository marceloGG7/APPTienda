using ENTITIES.ENTITY;
using System.Reflection.Emit;
using ENTITIES.DTO;
using BLL;
using System.Collections.Generic;

Console.Write("Ingrese el usuario: ");
string Usuario = Console.ReadLine();

Console.Write("Ingrese la contraseña: ");
string Contraseña = Console.ReadLine();

if (ValidaUsuario(Usuario, Contraseña))
{
    int Opc =0;

    while (Opc!=5)
    {
        Console.WriteLine("");
        Console.WriteLine("=Sistema de punto de venta=");
        Console.WriteLine();
        Console.WriteLine("Bienvenido al sistema selecciona la opcion a realizar");
        Console.WriteLine("1.- Reporte de Inventario");
        Console.WriteLine("2.- Reporte de Venta");
        Console.WriteLine("3.- Venta");
        Console.WriteLine("4.- Agregar Producto");
        Console.WriteLine("5.- Salir");
        Console.Write("Ingrese la opcion a realizar: ");
        Opc = Convert.ToInt32(Console.ReadLine());

        if (Opc == 1)
        {
            // Reporte de inventario.
            Console.WriteLine("");
            ReporteInventario();
        }
        else if (Opc == 2)
        {
            // Reporte de venta.
            ReporteVenta();
        }
        else if (Opc == 3)
        {
            // Venta
            Console.WriteLine("");
            Venta();
            Console.WriteLine("");
        }
        else if (Opc == 4)
        {
            // Agregar producto.
            Console.WriteLine();   
            AgregarInventario();
        }
        else if (Opc == 5)
        {
            Console.WriteLine();
            Console.WriteLine("Saliendo del sistema....");
        }
    }
}
else
{
    Console.WriteLine("Acceso denegado......");
    Console.WriteLine("Saliendo del sistrema.....");
}

Console.ReadKey();

static void AgregarInventario()
{
    INVENTARIO Inventario = new();

    Console.Write("Ingrese la descripcion del inventario: ");
    Inventario.Descrip = Console.ReadLine();

    while (true)
    {
        Console.WriteLine("1.- Captura de SKU.");
        Console.WriteLine("2.- Captura de codigo de barras.");
        Console.WriteLine("Indique que valor desea capturar: ");
        int Opc = Convert.ToInt32(Console.ReadLine());

        if (Opc == 1)
        {
            Console.Write("Ingrese el SKU: ");
            Inventario.SKU = Console.ReadLine();
            break;
        }
        else if (Opc == 2)
        {
            Console.Write("Ingrese el codigo de barras: ");
            Inventario.CB = Console.ReadLine();
            break;
        }
        else
        {
            Console.WriteLine("Opciòn no valida, ingrese una opcion correcta.");
            Console.WriteLine();
        }
    }
    Console.Write("Ingrese el precio de venta: ");
    Inventario.PVenta = Convert.ToDecimal(Console.ReadLine());

    Console.WriteLine("Ingrese las existencias: ");
    Inventario.Existencias = Convert.ToInt32(Console.ReadLine());

    List<DtoCatCategoria> lstCategorias = BL_CATEGORIA.ListarCategorias();

    foreach(var lst in lstCategorias)
    {
        Console.WriteLine($"{lst.IdCategoria}-{lst.Categoria}");
    }
    Console.Write("Seleccione una opción: ");
    Inventario.IdCategoria = Convert.ToInt32(Console.ReadLine());

    Console.Write("Ingrese el IVA aplicado: ");
    Inventario.IVA = Convert.ToDecimal(Console.ReadLine());

    List<string> lstValidaciones = BL_INVENTARIO.ValidarProducto(Inventario);

    if (lstValidaciones.Count==0)
    {
        List<string> lstRespuesta = BL_INVENTARIO.GuardarInventario(Inventario);
        if (lstRespuesta[0] == "00")
        {
            Console.WriteLine("");
            Console.WriteLine(lstRespuesta[1]);
        }
        else
        {
            Console.WriteLine("");
            Console.WriteLine(lstRespuesta[1]);
        }
    }
    else
    {
        Console.WriteLine("");
        Console.WriteLine($"Error: {lstValidaciones[0]}");
    }
}


static void ReporteVenta()
{
    List<DtoRepVentas> lstReporteVenta = BL_Venta.ReporteVenta();

    foreach (var lstVenta in lstReporteVenta)
    {
        Console.WriteLine();
        Console.WriteLine("--------");
        Console.WriteLine($"No. Ticket: {lstVenta.Ticket}");
        Console.WriteLine($"Total: {lstVenta.Total.ToString("c")}   Pago: {lstVenta.Pago.ToString("c")}   Cambio: {lstVenta.Cambio.ToString("c")}   Fecha Venta: {lstVenta.FecVenta}");

        Console.WriteLine();

        Console.WriteLine("Articulo  |  Cantidad  |  P. Venta  |  IVA  |  Total ");

        foreach (var lstVD in lstVenta.VentaDetalle)
        {
            Console.WriteLine($"{lstVD.Articulo}  |  {lstVD.Cantidad}  |  {lstVD.Pventa.ToString("c")}  |  {lstVD.IVA.ToString("c")}  |  {lstVD.Total.ToString("c")} ");
        }
    }
    Console.WriteLine("--------");

}

static void ReporteInventario()
{
    List<DtoRepInventario> lstRepInventario = BL_INVENTARIO.ReporteInventario();

    Console.WriteLine($"SKU || CB || Descrip || PVenta || Existencias || Categoria || IVA || Estatus");

    foreach(var lst in lstRepInventario)
    {
        Console.WriteLine($"{lst.SKU} || {lst.CB} || {lst.Descrip} || {lst.PVenta} || {lst.Existencias} || {lst.Categoria} || {lst.IVA} || {lst.Estatus}");
    }
}

static bool ValidaUsuario(string PUsuario, string PContraseña)
{
    bool Resultado = false;
    string Usuario = "admin", Contraseña = "admin";

    if (PUsuario == Usuario && PContraseña == Contraseña)
    {
        Resultado = true;
    }
    return Resultado;
}

static void Venta()
{
    List<DtoCarrito> lstCarrito = [];

    while (true)
    {
        DtoCarrito InfoArticulo = new DtoCarrito();

        Console.Write("Ingrese el SKU / CB del artículo: ");
        string Articulo = Console.ReadLine();

        List<DtoCarrito> lstDatos = BL_INVENTARIO.ConsultaDatosArticulo(Articulo);

        if (lstDatos.Count > 0)
        {
            Console.Write("Ingrese la cantidad a comprar: ");
            int Cantidad = Convert.ToInt32(Console.ReadLine());

            InfoArticulo.Descrip = lstDatos[0].Descrip;
            InfoArticulo.IdInventario = lstDatos[0].IdInventario;
            InfoArticulo.Cantidad = Cantidad;
            InfoArticulo.Precio = lstDatos[0].Precio;
            InfoArticulo.Subtotal = (lstDatos[0].Precio * Cantidad);
            InfoArticulo.Total = (InfoArticulo.Subtotal * ((lstDatos[0].IVA / 100) + 1));
            InfoArticulo.IVA = lstDatos[0].IVA;

            lstDatos = null;

            lstCarrito.Add(InfoArticulo);
            //InfoArticulo = null;

            Console.WriteLine();
            Console.WriteLine("=Carrito=");
            Console.WriteLine($"Artículo | Cantidad | Precio | SubTotal | Total");
            foreach (var lst in lstCarrito)
            {
                Console.WriteLine($"{lst.Descrip} | {lst.Cantidad} | {lst.Precio} | {lst.Subtotal} | {lst.Total}");
            }

            Console.WriteLine();
            Console.WriteLine($"SubTotal: {lstCarrito.Sum(lst => lst.Subtotal).ToString("c")}");
            Console.WriteLine($"Total: {lstCarrito.Sum(lst => lst.Total).ToString("c")}");

            Console.WriteLine();

            Console.WriteLine("1.- Pagar");
            Console.WriteLine("2.- Seguir agregado");
            Console.WriteLine("3.- Cancelar Venta");
            Console.Write("Ingrese la opción a realizar: ");
            int Opc = Convert.ToInt32(Console.ReadLine());

            if (Opc == 1)
            {

                while (true)
                {
                    Console.Write("Ingrese el pago: ");
                    decimal Pago = Convert.ToDecimal(Console.ReadLine());

                    List<string> lstValidacion = BL_Venta.ValidacionVenta(lstCarrito.Sum(lst => lst.Total), Pago, lstCarrito);

                    if (lstValidacion.Count == 0)
                    {

                        List<string> lstDatosVenta = BL_Venta.Venta(lstCarrito, Pago, Pago - lstCarrito.Sum(lst => lst.Total));

                        if (lstDatosVenta[0] == "00")
                        {
                            Console.WriteLine();
                            Console.WriteLine(lstDatosVenta[1]);
                            Console.WriteLine();
                            Console.WriteLine($"Cambio: {Pago - lstCarrito.Sum(lst => lst.Total):c}");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine(lstDatosVenta[1]);
                            Console.WriteLine();
                        }

                    }
                    else
                    {
                        Console.WriteLine("Errores");
                        foreach (var lst in lstValidacion)
                        {
                            Console.WriteLine($"* {lst}");
                        }
                        Console.WriteLine();
                        Console.WriteLine("Venta cancelada..");
                        break;
                    }
                    break;
                }
                break;
            }
            else if (Opc == 3)
            {
                Console.WriteLine("");
                Console.WriteLine("Venta cancelada..");
                break;
            }
        }
        else
        {
            Console.WriteLine("El artículo no fue encontrado");
            Console.WriteLine();
        }
    }
}