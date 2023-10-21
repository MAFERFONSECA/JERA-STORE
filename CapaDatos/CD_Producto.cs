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
    public class CD_Producto
    {
        public List<Producto> Listar()
        {

            List<Producto> lista = new List<Producto>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("select p.PRO_ID, p.PRO_NOMBRE, p.PRO_DESCRIPCION,");
                    sb.AppendLine("m.MAR_ID,m.MAR_DESCRIPCION[DesMarca],");
                    sb.AppendLine("c.CAT_ID,c.CAT_DESCRIPCION[DesCategoria],");
                    sb.AppendLine("p.PRO_PRECIO,p.PRO_STOCK,p.PRO_RUTAIMAGEN,p.PRO_NOMBREIMAGEN,p.PRO_ACTIVO");
                    sb.AppendLine("from PRODUCTO p");
                    sb.AppendLine("inner join MARCA m on m.MAR_ID = p.MAR_ID");
                    sb.AppendLine("inner join CATEGORIA c on c.CAT_ID = p.CAT_ID");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Producto()
                            {
                                PRO_ID = Convert.ToInt32(dr["PRO_ID"]),
                                PRO_NOMBRE = dr["PRO_NOMBRE"].ToString(),
                                PRO_DESCRIPCION = dr["PRO_DESCRIPCION"].ToString(),
                                oMarca = new Marca() { MAR_ID = Convert.ToInt32(dr["MAR_ID"]), MAR_DESCRIPCION = dr["DesMarca"].ToString(), },
                                oCategoria = new Categoria() { CAT_ID = Convert.ToInt32(dr["CAT_ID"]), CAT_DESCRIPCION = dr["DesCategoria"].ToString(), },
                                PRO_PRECIO = Convert.ToDecimal(dr["PRO_PRECIO"], new CultureInfo("es-MX")),
                                PRO_STOCK = Convert.ToInt32(dr["PRO_STOCK"]),
                                PRO_RUTAIMAGEN = dr["PRO_RUTAIMAGEN"].ToString(),
                                PRO_NOMBREIMAGEN = dr["PRO_NOMBREIMAGEN"].ToString(),
                                PRO_ACTIVO = Convert.ToBoolean(dr["PRO_ACTIVO"]),
                            });
                        }
                    }
                }

            }
            catch
            {
                lista = new List<Producto>();
            }

            return lista;

        }


        public int Registrar(Producto obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarProducto", oconexion);
                    cmd.Parameters.AddWithValue("PRO_NOMBRE", obj.PRO_NOMBRE);
                    cmd.Parameters.AddWithValue("PRO_DESCRIPCION", obj.PRO_DESCRIPCION);
                    cmd.Parameters.AddWithValue("MAR_ID", obj.oMarca.MAR_ID);
                    cmd.Parameters.AddWithValue("CAT_ID", obj.oCategoria.CAT_ID);
                    cmd.Parameters.AddWithValue("PRO_PRECIO", obj.PRO_PRECIO);
                    cmd.Parameters.AddWithValue("PRO_STOCK", obj.PRO_STOCK);
                    cmd.Parameters.AddWithValue("PRO_ACTIVO", obj.PRO_ACTIVO);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }
            return idautogenerado;
        }


        public bool Editar(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarProducto", oconexion);
                    cmd.Parameters.AddWithValue("PRO_ID", obj.PRO_ID);
                    cmd.Parameters.AddWithValue("PRO_NOMBRE", obj.PRO_NOMBRE);
                    cmd.Parameters.AddWithValue("PRO_DESCRIPCION", obj.PRO_DESCRIPCION);
                    cmd.Parameters.AddWithValue("MAR_ID", obj.oMarca.MAR_ID);
                    cmd.Parameters.AddWithValue("CAT_ID", obj.oCategoria.CAT_ID);
                    cmd.Parameters.AddWithValue("PRO_PRECIO", obj.PRO_PRECIO);
                    cmd.Parameters.AddWithValue("PRO_STOCK", obj.PRO_STOCK);
                    cmd.Parameters.AddWithValue("PRO_ACTIVO", obj.PRO_ACTIVO);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }



        public bool GuardarDatosImagen(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = String.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "update producto set PRO_RUTAIMAGEN = @PRO_RUTAIMAGEN, PRO_NOMBREIMAGEN = @PRO_NOMBREIMAGEN where PRO_ID = @PRO_ID";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@PRO_RUTAIMAGEN", obj.PRO_RUTAIMAGEN);
                    cmd.Parameters.AddWithValue("@PRO_NOMBREIMAGEN", obj.PRO_NOMBREIMAGEN);
                    cmd.Parameters.AddWithValue("@PRO_ID", obj.PRO_ID);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar la imagen";

                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }



        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarProducto", oconexion);
                    cmd.Parameters.AddWithValue("PRO_ID", id);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

    }
}
