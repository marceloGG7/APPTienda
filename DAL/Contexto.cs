using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class Contexto
    {
        public static DataTable Funcion_StoreDB(string cadena, string P_Sentencia, object P_Parametro)
        {
            DataTable Dt = new();
            try
            {
                using SqlConnection conn = new(cadena);
                var lst = conn.ExecuteReader(P_Sentencia, P_Parametro, commandType: CommandType.StoredProcedure);
                Dt.Load(lst);
            }
            catch (SqlException)
            {
                throw;
            }
            return Dt;
        }

        public static void Procedimiento_StoreDB(string cadena, string P_Sentencia, object P_Parametro)
        {
            try
            {
                using SqlConnection conn = new(cadena);
                conn.Execute(P_Sentencia, P_Parametro, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public static void Procedimiento_ScriptDB(string cadena, string P_Sentencia, object P_Parametro)
        {
            try
            {
                using SqlConnection conn = new(cadena);
                conn.Execute(P_Sentencia, P_Parametro, commandType: CommandType.Text);
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public static DataTable Funcion_ScriptDB(string cadena, string P_Sentencia, object P_Parametro)
        {
            DataTable Dt = new();
            try
            {
                using SqlConnection conn = new(cadena);
                var lst = conn.ExecuteReader(P_Sentencia, P_Parametro, commandType: CommandType.Text);
                Dt.Load(lst);
            }
            catch (SqlException)
            {
                throw;
            }
            return Dt;
        }
    }
}
