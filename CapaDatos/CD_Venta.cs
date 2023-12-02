using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Venta
    {
        public bool Registrar(Venta obj,DataTable DetalleVenta, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarVenta", oconexion);
                    cmd.Parameters.AddWithValue("CLI_ID", obj.CLI_ID);
                    cmd.Parameters.AddWithValue("VEN_TOTALPRODUCTO", obj.VEN_TOTALPRODUCTO);
                    cmd.Parameters.AddWithValue("VEN_MONTOTOTAL", obj.VEN_MONTOTOTAL);
                    cmd.Parameters.AddWithValue("VEN_CONTACTO", obj.VEN_CONTACTO);
                    cmd.Parameters.AddWithValue("COL_ID", obj.COL_ID);
                    cmd.Parameters.AddWithValue("VEN_TELEFONO", obj.VEN_TELEFONO);
                    cmd.Parameters.AddWithValue("VEN_DIRECCION", obj.VEN_IDTRANSACCION);
                    cmd.Parameters.AddWithValue("VEN_IDTRANSACCION", obj.COL_ID);
                    cmd.Parameters.AddWithValue("DETALLEVENTA", DetalleVenta);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }

            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }
            return respuesta;
        }

    }
}
