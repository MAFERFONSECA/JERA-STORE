using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDato = new CD_Usuarios();

        public List<Usuario> Listar()
        {
            return objCapaDato.Listar();
        }


        public int Registrar(Usuario obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.USU_NOMBRES) || string.IsNullOrWhiteSpace(obj.USU_NOMBRES))
            {
                Mensaje = "El nombre del usuario no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.USU_APELLIDOS) || string.IsNullOrWhiteSpace(obj.USU_APELLIDOS))
            {
                Mensaje = "El apellido del usuario no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.USU_CORREO) || string.IsNullOrWhiteSpace(obj.USU_CORREO))
            {
                Mensaje = "El correo del usuario no puede ser vacío";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {

                string clave = "test123";

                //string clave = CN_Recursos.GenerarClave();

                //string asunto = "Creacion de Cuenta";
                //string mensaje_correo = "<h3> Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es: !clave!</p>";
                //mensaje_correo = mensaje_correo.Replace("!clave!", clave);

                //bool respuesta = CN_Recursos.EnviarCorreo(obj.Correo, asunto, mensaje_correo);

                //if (respuesta)
                //{
                obj.USU_CLAVE = CN_Recursos.ConvertirSha256(clave);
                return objCapaDato.Registrar(obj, out Mensaje);
                //}
                //else
                //{
                //   Mensaje = "No se puede enviar el correo";
                //   return 0;
                //}

            }
            else
            {
                return 0;
            }



        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.USU_NOMBRES) || string.IsNullOrWhiteSpace(obj.USU_NOMBRES))
            {
                Mensaje = "El nombre del usuario no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.USU_APELLIDOS) || string.IsNullOrWhiteSpace(obj.USU_APELLIDOS))
            {
                Mensaje = "El apellido del usuario no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.USU_CORREO) || string.IsNullOrWhiteSpace(obj.USU_CORREO))
            {
                Mensaje = "El correo del usuario no puede ser vacío";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }

        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }
    }
}
