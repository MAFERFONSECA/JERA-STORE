using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private CD_Producto objCapaDato = new CD_Producto();

        public List<Producto> Listar()
        {
            return objCapaDato.Listar();
        }


        public int Registrar(Producto obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.PRO_NOMBRE) || string.IsNullOrWhiteSpace(obj.PRO_NOMBRE))
            {
                Mensaje = "El nombre del producto no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.PRO_DESCRIPCION) || string.IsNullOrWhiteSpace(obj.PRO_DESCRIPCION))
            {
                Mensaje = "La descripción del producto no puede ser vacía";
            }
            else if (obj.oMarca.MAR_ID == 0)
            {
                Mensaje = "Debe seleccionar una marca";
            }
            else if (obj.oCategoria.CAT_ID == 0)
            {
                Mensaje = "Debe seleccionar una categoría";
            }
            else if (obj.oCategoria.CAT_ID == 0)
            {
                Mensaje = "Debe seleccionar una categoría";
            }
            else if (obj.PRO_PRECIO == 0)
            {
                Mensaje = "Debe ingresar el precio del producto";
            }
            else if (obj.PRO_STOCK == 0)
            {
                Mensaje = "Debe ingresar el stock del producto";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.Registrar(obj, out Mensaje);

            }
            else
            {
                return 0;
            }

        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.PRO_NOMBRE) || string.IsNullOrWhiteSpace(obj.PRO_NOMBRE))
            {
                Mensaje = "El nombre del producto no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.PRO_DESCRIPCION) || string.IsNullOrWhiteSpace(obj.PRO_DESCRIPCION))
            {
                Mensaje = "La descripción del producto no puede ser vacía";
            }
            else if (obj.oMarca.MAR_ID == 0)
            {
                Mensaje = "Debe seleccionar una marca";
            }
            else if (obj.oCategoria.CAT_ID == 0)
            {
                Mensaje = "Debe seleccionar una categoría";
            }
            else if (obj.oCategoria.CAT_ID == 0)
            {
                Mensaje = "Debe seleccionar una categoría";
            }
            else if (obj.PRO_PRECIO == 0)
            {
                Mensaje = "Debe ingresar el precio del producto";
            }
            else if (obj.PRO_STOCK == 0)
            {
                Mensaje = "Debe ingresar el stock del producto";
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



        public bool GuardarDatosImagen(Producto obj, out string Mensaje)
        {
            return objCapaDato.GuardarDatosImagen(obj, out Mensaje);
        }



        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }
    }
}
