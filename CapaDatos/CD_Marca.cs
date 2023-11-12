using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CapaDatos
{
    public class CD_Marca
    {
        public List<Marca> Listar()
        {

            List<Marca> lista = new List<Marca>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT MAR_ID,MAR_DESCRIPCION,MAR_ACTIVO FROM Marca ";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Marca()
                            {
                                MAR_ID = Convert.ToInt32(dr["MAR_ID"]),
                                MAR_DESCRIPCION = dr["MAR_DESCRIPCION"].ToString(),
                                MAR_ACTIVO = Convert.ToBoolean(dr["MAR_ACTIVO"])
                            });
                        }
                    }
                }

            }
            catch
            {
                lista = new List<Marca>();
            }

            return lista;

        }


        public int Registrar(Marca obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarMarca", oconexion);
                    cmd.Parameters.AddWithValue("MAR_DESCRIPCION", obj.MAR_DESCRIPCION);
                    cmd.Parameters.AddWithValue("MAR_ACTIVO", obj.MAR_ACTIVO);
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


        public bool Editar(Marca obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarMarca", oconexion);
                    cmd.Parameters.AddWithValue("MAR_ID", obj.MAR_ID);
                    cmd.Parameters.AddWithValue("MAR_DESCRIPCION", obj.MAR_DESCRIPCION);
                    cmd.Parameters.AddWithValue("MAR_ACTIVO", obj.MAR_ACTIVO);
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


        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarMarca", oconexion);
                    cmd.Parameters.AddWithValue("MAR_ID", id);
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


        public List<Marca> ListarMarcaporCategoria( int idcategoria)
        {

            List<Marca> lista = new List<Marca>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("select distinct m.MAR_ID,m.MAR_DESCRIPCION from PRODUCTO p");
                    sb.AppendLine("inner join CATEGORIA c on c.CAT_ID = p.CAT_ID");
                    sb.AppendLine("inner join MARCA m on m.MAR_ID = p.MAR_ID and m.MAR_ACTIVO = 1");
                    sb.AppendLine("where c.CAT_ID = iif(@CAT_ID = 0, c.CAT_ID, @CAT_ID)");


                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@CAT_ID",idcategoria);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Marca()
                            {
                                MAR_ID = Convert.ToInt32(dr["MAR_ID"]),
                                MAR_DESCRIPCION = dr["MAR_DESCRIPCION"].ToString()
                            });
                        }
                    }
                }

            }
            catch
            {
                lista = new List<Marca>();
            }

            return lista;

        }
    }
}
