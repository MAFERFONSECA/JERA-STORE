using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Cliente
    {
        private CD_Cliente objCapaDato = new CD_Cliente();

        public int Registrar(Cliente obj, out string Mensaje)
        {

            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.CLI_NOMBRES) || string.IsNullOrWhiteSpace(obj.CLI_NOMBRES))
            {
                Mensaje = "El nombre del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.CLI_APELLIDOS) || string.IsNullOrWhiteSpace(obj.CLI_APELLIDOS))
            {
                Mensaje = "El apellido del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.CLI_CORREO) || string.IsNullOrWhiteSpace(obj.CLI_CORREO))
            {
                Mensaje = "El correo del cliente no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                obj.CLI_CLAVE = CN_Recursos.ConvertirSha256(obj.CLI_CLAVE);
                return objCapaDato.Registrar(obj, out Mensaje);

            }
            else
            {

                return 0;
            }



        }

        public List<Cliente> Listar()
        {
            return objCapaDato.Listar();
        }


        public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
        {

            return objCapaDato.CambiarClave(idcliente, nuevaclave, out Mensaje);
        }


        public bool ReestablecerClave(int idcliente, string correo, out string Mensaje)
        {

            Mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDato.ReestablecerClave(idcliente, CN_Recursos.ConvertirSha256(nuevaclave), out Mensaje);

            if (resultado)
            {

                string asunto = "Contraseña Reestablecida";
                string mensaje_correo = "<h3>Su cuenta fue reestablecida correctamente</h3></br><p>Su contraseña para acceder ahora es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevaclave);


                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensaje_correo);

                if (respuesta)
                {

                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return false;
                }

            }
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña";

                return false;
            }


        }
    }
}
