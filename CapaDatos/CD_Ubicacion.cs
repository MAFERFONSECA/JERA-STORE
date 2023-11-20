using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Ubicacion
    {

        public List<Municipio> ObtenerMunicipio()
        {

            List<Municipio> lista = new List<Municipio>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from MUNICIPIO";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Municipio()
                                {
                                    MUN_ID = dr["MUN_ID"].ToString(),
                                    MUN_DESCRIPCION = dr["MUN_DESCRIPCION"].ToString(),
                                }

                                );
                        }
                    }
                }




            }
            catch
            {
                lista = new List<Municipio>();
            }

            return lista;

        }


        public List<Ciudad> ObtenerCiudad(string idmunicipio)
        {

            List<Ciudad> lista = new List<Ciudad>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from CIUDAD WHERE MUN_ID = @MUN_ID";
     
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@MUN_ID", idmunicipio);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Ciudad()
                                {
                                    CIU_ID = dr["CIU_ID"].ToString(),
                                    CIU_DESCRIPCION = dr["CIU_DESCRIPCION"].ToString(),
                                }

                                );
                        }
                    }
                }




            }
            catch
            {
                lista = new List<Ciudad>();
            }

            return lista;

        }


        public List<Colonia> ObtenerColonia(string idmunicipio, string idciudad)
        {

            List<Colonia> lista = new List<Colonia>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from COLONIA WHERE CIU_ID = @CIU_ID and MUN_ID = @MUN_ID";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@CIU_ID", idciudad);
                    cmd.Parameters.AddWithValue("@MUN_ID", idmunicipio);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Colonia()
                                {
                                    COL_ID = dr["COL_ID"].ToString(),
                                    COL_DESCRIPCION = dr["COL_DESCRIPCION"].ToString(),
                                }

                                );
                        }
                    }
                }




            }
            catch
            {
                lista = new List<Colonia>();
            }

            return lista;

        }

    }
}
