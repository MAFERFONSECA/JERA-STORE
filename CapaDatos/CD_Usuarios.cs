using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;

namespace CapaDatos
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {

            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select USU_ID,USU_NOMBRES,USU_APELLIDOS,USU_CORREO,USU_CLAVE,USU_REESTABLECER,USU_ACTIVO from USUARIO";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Usuario()
                                {
                                    USU_ID = Convert.ToInt32(dr["USU_ID"]),
                                    USU_NOMBRES = dr["USU_NOMBRES"].ToString(),
                                    USU_APELLIDOS = dr["USU_APELLIDOS"].ToString(),
                                    USU_CORREO = dr["USU_CORREO"].ToString(),
                                    USU_CLAVE = dr["USU_CLAVE"].ToString(),
                                    USU_REESTABLECER = Convert.ToBoolean(dr["USU_REESTABLECER"]),
                                    USU_ACTIVO = Convert.ToBoolean(dr["USU_ACTIVO"])
                                }

                                );
                        }
                    }
                }

            }
            catch
            {
                lista = new List<Usuario>();
            }

            return lista;

        }
        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("USU_NOMBRES", obj.USU_NOMBRES);
                    cmd.Parameters.AddWithValue("USU_APELLIDOS", obj.USU_APELLIDOS);
                    cmd.Parameters.AddWithValue("USU_CORREO", obj.USU_CORREO);
                    cmd.Parameters.AddWithValue("USU_CLAVE", obj.USU_CLAVE);
                    cmd.Parameters.AddWithValue("USU_ACTIVO", obj.USU_ACTIVO);
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

        public bool Editar(Usuario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("USU_ID", obj.USU_ID);
                    cmd.Parameters.AddWithValue("USU_NOMBRES", obj.USU_NOMBRES);
                    cmd.Parameters.AddWithValue("USU_APELLIDOS", obj.USU_APELLIDOS);
                    cmd.Parameters.AddWithValue("USU_CORREO", obj.USU_CORREO);
                    cmd.Parameters.AddWithValue("USU_ACTIVO", obj.USU_ACTIVO);
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
                    SqlCommand cmd = new SqlCommand("delete top (1) from usuario where USU_ID = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
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
