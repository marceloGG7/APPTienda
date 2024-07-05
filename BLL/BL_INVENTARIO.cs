using BLL.Validator;
using DAL;
using ENTITIES.DTO;
using ENTITIES.ENTITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BL_INVENTARIO
    {
        private static string Conn = Environment.GetEnvironmentVariable("Prod");    

        public static List<string> ValidarProducto(INVENTARIO PInventario)
        {
            List<string> lstValidaciones = new();

            ValidacionInventario validationRules = new ValidacionInventario();

            var resul = validationRules.Validate(PInventario);

            if (!resul.IsValid)
            {
                lstValidaciones = resul.Errors.Select(x => x.ErrorMessage).ToList();
            }
            if (ValidaDescripDB(PInventario.Descrip))
            {
                lstValidaciones.Add("La descripcion del producto ya se encuentra registrada.");
            }
            return lstValidaciones;
        }

        public static bool ValidaDescripDB(string PDescrip)
        {
            bool Validacion=false;
            var dpParametros = new
            {
                p_Accion = 1,
                p_Descrip= PDescrip
            };
            
            DataTable Dt = Contexto.Funcion_StoreDB(Conn, "spConsulInventario", dpParametros);

            if (Dt.Rows.Count > 0)
            {
                Validacion= true;
            }
            return Validacion;
        }

        public static List<string> GuardarInventario(INVENTARIO PInventario)
        {
            List<string> lstRespuesta = [];

            string SQLScript = "\r\nINSERT INTO INVENTARIO (Descrip,PVenta,Existencias,IdCategoria,IVA,SKU,CB)\r\n\t\t\tVALUES (@P_Descrip,@P_Venta,@P_Existencias,@P_IdCategoria,@P_IVA,@P_SKU,@P_CB)";

            try
            {
                var dpParametro = new
                {
                    P_Descrip = PInventario.Descrip,
                    P_Venta = PInventario.PVenta,
                    P_Existencias = PInventario.Existencias,
                    P_IdCategoria = PInventario.IdCategoria,
                    P_IVA = PInventario.IVA,
                    P_SKU = PInventario.SKU,
                    P_CB = PInventario.CB
                };
                Contexto.Procedimiento_ScriptDB(Conn, SQLScript, dpParametro);
                lstRespuesta.Add("00");
                lstRespuesta.Add("Inventario Registrado");
            }
            catch (Exception ex)
            {
                lstRespuesta.Add("14");
                lstRespuesta.Add(ex.Message);
            }
            return lstRespuesta;
        }

        public static List<DtoRepInventario> ReporteInventario()
        {
            List<DtoRepInventario> lstRepInventario = [];

            string SQLScript = "select ISNULL(INV.SKU,'--') AS SKU,\r\n\t\tISNULL(INV.CB,'--') AS CB,\r\n\t\tINV.Descrip,\r\n\t\tINV.PVenta,\r\n\t\tINV.Existencias,\r\n\t\tCAT.Descrip AS Categoria,\r\n\t\tINV.IVA,\r\n\t\tIIF(INV.IsActivo=1, 'Activo','Inactivo') AS Estatus,\r\n\t\tFORMAT(FecRegistro,'dd/MM/yyyy HH:mm') as FecRegistro\r\nFROM INVENTARIO AS INV\r\nINNER JOIN CATEGORIA AS CAT ON INV.IdCategoria=CAT.IdCategoria";

            var dpParametros = new { };

            DataTable Dt = Contexto.Funcion_ScriptDB(Conn, SQLScript, dpParametros);

            if (Dt.Rows.Count >= 0)
            {
                lstRepInventario = 
                [

                    .. (from item in Dt.AsEnumerable()
                        select new DtoRepInventario
                        {
                            SKU = item.Field<string>("SKU"),
                            CB = item.Field<string>("CB"),
                            Descrip = item.Field<string>("Descrip"),
                            PVenta = item.Field<decimal>("PVenta"),
                            Existencias = item.Field<int>("Existencias"),
                            Categoria = item.Field<string>("Categoria"),
                            IVA = item.Field<decimal>("IVA"),
                            Estatus = item.Field<string>("Estatus"),
                            FecAlta = item.Field<string>("FecRegistro")
                        }),
                ];
            }
            return lstRepInventario;
        }

        public static List<DtoCarrito> ConsultaDatosArticulo(string Articulo)
        {
            List<DtoCarrito> Carrito = [];

            var dpParametros = new
            {
                P_Articulo = Articulo
            };

            DataTable Dt = Contexto.Funcion_StoreDB(Conn, "spConsulArticulo", dpParametros);

            if(Dt.Rows.Count > 0)
            {
                Carrito = [
                    .. (from item in Dt.AsEnumerable()
                        select new DtoCarrito{
                            IdInventario = item.Field<int>("IdInventario"),
                            Descrip = item.Field<string>("Descrip"),
                            Precio = item.Field<decimal>("PVenta"),
                            IVA = item.Field<decimal>("IVA")
                        })
                ];
            }
            return Carrito;
        }
    }
}
    