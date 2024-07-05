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
    public class BL_CATEGORIA
    {
        private static string Conn = Environment.GetEnvironmentVariable("Prod");

        public static List<DtoCatCategoria> ListarCategorias()
        {
            List<DtoCatCategoria> lstCategorias = [];

            string SQLScript = "SELECT IdCategoria,\r\n\t\tDescrip AS Categoria\r\nFROM CATEGORIA";

            var dpParametros = new { };

            DataTable Dt = Contexto.Funcion_ScriptDB(Conn,SQLScript,dpParametros);

            if (Dt.Rows.Count>0)
            {
                lstCategorias = 
                    [
                        .. (from item in Dt.AsEnumerable()
                            select new DtoCatCategoria
                            {
                                IdCategoria = item.Field<int>("IdCategoria"),
                                Categoria=item.Field<string>("Categoria")
                            }),
                ];
            }
            return lstCategorias;
        }
    }
}
