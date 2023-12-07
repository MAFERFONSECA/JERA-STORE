using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

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
                    cmd.Parameters.AddWithValue("VEN_DIRECCION", obj.VEN_DIRECCION);
                    cmd.Parameters.AddWithValue("VEN_IDTRANSACCION", obj.VEN_IDTRANSACCION);
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


        public List<DetalleVenta> ListarCompras(int idcliente)
        {

            List<DetalleVenta> lista = new List<DetalleVenta>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    string query = "select * from fn_ListarCompra(@idcliente)";


                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DetalleVenta()
                            {

                                oProducto = new Producto()
                                {

                                    PRO_NOMBRE = dr["PRO_NOMBRE"].ToString(),
                                    PRO_PRECIO = Convert.ToDecimal(dr["PRO_PRECIO"], new CultureInfo("es-MX")),
                                    PRO_RUTAIMAGEN = dr["PRO_RUTAIMAGEN"].ToString(),
                                    PRO_NOMBREIMAGEN = dr["PRO_NOMBREIMAGEN"].ToString(),
                                },
                                DETV_CANTIDAD = Convert.ToInt32(dr["DETV_CANTIDAD"]),
                                DETV_TOTAL = Convert.ToDecimal(dr["DETV_TOTAL"],new CultureInfo("es-MX")),
                                VEN_IDTRANSACCION = dr["VEN_IDTRANSACCION"].ToString(),




                            });
                        }
                    }
                }

            }
            catch
            {
                lista = new List<DetalleVenta>();
            }

            return lista;

        }

    }
}
