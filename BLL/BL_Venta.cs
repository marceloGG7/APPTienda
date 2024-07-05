using DAL;
using ENTITIES.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BL_Venta
    {
        private static string Conn = Environment.GetEnvironmentVariable("Prod");

        public static List<string> ValidacionVenta(decimal PMontoPago, decimal PPago, List<DtoCarrito> PlstCarrito)
        {
            List<string> lstValidacion = [];

            if (PPago < PMontoPago)
            {
                lstValidacion.Add("El pago debe ser mayor al monto");
            }
            foreach (var lst in PlstCarrito)
            {
                if (ValidaExistenciasArticulo(lst.IdInventario, lst.Cantidad))
                {
                    lstValidacion.Add($"Articulo {lst.Descrip} no se cuenta con existencias");
                }
            }
            return lstValidacion;
        }

        private static bool ValidaExistenciasArticulo(int PIdInventario, int PCantidad)
        {
            bool Validacion = false;

            string SQLScript = "SELECT 1 AS RESULTADO\r\nFROM INVENTARIO\r\nwhere IdInventario = @P_IdInventario AND \r\n\t\tExistencias >= @P_Cantidad";

            var dpParametros = new
            {
                P_IdInventario = PIdInventario,
                P_Cantidad = PCantidad
            };

            DataTable Dt = Contexto.Funcion_ScriptDB(Conn,SQLScript, dpParametros);

            if(Dt.Rows.Count == 0)
            {
                Validacion = true;
            }

            return Validacion;
        }

        public static List<string> Venta(List<DtoCarrito> PlstCarrito, decimal PMontoPago, decimal PFeria)
        {
            List<string> lstDatos = [];

            try
            {
                var dpParametros = new
                {
                    P_Accion = 1,
                    P_MontoPagado = PMontoPago,
                    P_Feria = PFeria
                };

                DataTable Dt = Contexto.Funcion_StoreDB(Conn, "spVenta", dpParametros);

                if (Dt.Rows.Count > 0)
                {

                    int idVenta = (int)Dt.Rows[0][0];

                    if (VentaDetalle(PlstCarrito, idVenta))
                    {
                        lstDatos.Add("00");
                        lstDatos.Add("Venta realizada con éxito");
                    }
                }
                else
                {
                    lstDatos.Add("14");
                    lstDatos.Add("Venta no realizada");
                }

            }
            catch (Exception e)
            {
                lstDatos.Add("14");
                lstDatos.Add(e.Message);
            }

            return lstDatos;

        }

        private static bool VentaDetalle(List<DtoCarrito> PlstCarrito, int PIdVenta)
        {
            bool Validacion = true;
            try
            {
                foreach (var lst in PlstCarrito)
                {
                    var dpParametros = new
                    {
                        P_Accion = 2,
                        P_IdVenta = PIdVenta,
                        P_IdInventario = lst.IdInventario,
                        P_Cantidad = lst.Cantidad,
                        P_PVenta = lst.Precio,
                        P_IVA = lst.IVA
                    };

                    Contexto.Procedimiento_StoreDB(Conn, "spVenta", dpParametros);
                }

            }
            catch (Exception)
            {
                Validacion = false;
            }
            return Validacion;
        }

        public static List<DtoRepVentas> ReporteVenta()
        {
            List<DtoRepVentas> lstVentas = [];

            string SQLScript = "SELECT VEN.IdVenta AS Ticket,\r\n\t\tVDE.Total,\r\n\t\tVEN.MontoPagado as Pago,\r\n\t\tVEN.Feria as Cambio,\r\n\t\tFORMAT(VEN.FecVenta,'dd/MM/yyyy HH:mm:ss') AS FechaVenta\r\nFROM VENTA AS VEN\r\n\tINNER JOIN (SELECT IdVenta,\r\n\t\t\t\t\tSUM(((PVenta * (1+(IVA/100)))*Cantidad)) AS Total\r\n\t\t\t\t\tfrom VENTA_DETALLE\r\n\t\t\t\t\tgroup by IdVenta) AS VDE ON VEN.IdVenta = VDE.IdVenta";

            var dpParametros = new { };

            DataTable dt = Contexto.Funcion_ScriptDB(Conn, SQLScript, dpParametros);

            if (dt.Rows.Count > 0)
            {
                lstVentas = [..
                    dt.AsEnumerable().Select(item => new DtoRepVentas
                    {
                        Ticket = item.Field<int>("Ticket"),
                        Total = item.Field<decimal>("Total"),
                        Pago = item.Field<decimal>("Pago"),
                        Cambio = item.Field<decimal>("Cambio"),
                        FecVenta = item.Field<string>("FechaVenta")
                    })];
            }

            foreach (var lst in lstVentas)
            {
                lst.VentaDetalle = ReporteVentaDetalle(lst.Ticket);
            }
            
            return lstVentas;
        }

        private static List<DtoRepVentaDetalle> ReporteVentaDetalle(int PIdVenta)
        {
            List <DtoRepVentaDetalle>  lstVentaDetalle = [];

            string SQLScript = "SELECT INV.Articulo AS Articulo,\r\n\t\tCantidad,\r\n\t\tPVenta,\r\n\t\tIVA,((PVenta * (1+(IVA/100))) * Cantidad) as Total\r\n\t\tFROM VENTA_DETALLE AS VDE\r\n\t\tINNER JOIN (SELECT IdInventario,\r\n\t\t\t\tUPPER(Descrip) AS Articulo FROM INVENTARIO) AS INV ON VDE.IdInventario = INV.IdInventario\r\n\r\nwhere IdVenta = @P_IdVenta";

            var dpParametros = new 
            {
                P_IdVenta = PIdVenta,
            };

            DataTable Dt = Contexto.Funcion_ScriptDB(Conn, SQLScript, dpParametros);

            if(Dt.Rows.Count > 0)
            {
                lstVentaDetalle = [
                    .. Dt.AsEnumerable().Select(item => new DtoRepVentaDetalle
                    {
                        Articulo = item.Field<string>("Articulo"),
                        Cantidad = item.Field<int>("Cantidad"),
                        Pventa = item.Field<decimal>("Pventa"),
                        IVA = item.Field<decimal>("IVA"),
                        Total = item.Field<decimal>("Total"),
                    })];
            }
            return lstVentaDetalle;
        }

    }
}
