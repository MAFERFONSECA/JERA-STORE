using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }


        //+++++++++++++++CATEGORIA++++++++++++++++++++

        #region CATEGORIA

        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> oLista = new List<Categoria>();

            oLista = new CN_Categoria().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.CAT_ID == 0)
            {

                resultado = new CN_Categoria().Registrar(objeto, out mensaje);

            }
            else
            {
                resultado = new CN_Categoria().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Categoria().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        //+++++++++++++++MARCA++++++++++++++++++++

        #region MARCA
        [HttpGet]
        public JsonResult ListarMarca()
        {
            List<Marca> oLista = new List<Marca>();

            oLista = new CN_Marca().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.MAR_ID == 0)
            {

                resultado = new CN_Marca().Registrar(objeto, out mensaje);

            }
            else
            {
                resultado = new CN_Marca().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Marca().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        //+++++++++++++++PRODUCTO++++++++++++++++++++
        #region PRODUCTO

        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> oLista = new List<Producto>();

            oLista = new CN_Producto().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {

            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto);

            decimal precio;

            if (decimal.TryParse(oProducto.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-MX"), out precio))
            {
                oProducto.PRO_PRECIO = precio;

            }
            else
            {
                return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);
            }



            if (oProducto.PRO_ID == 0)
            {

                int idProductoGenerado = new CN_Producto().Registrar(oProducto, out mensaje);

                if (idProductoGenerado != 0)
                {
                    oProducto.PRO_ID = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }

            }
            else
            {
                operacion_exitosa = new CN_Producto().Editar(oProducto, out mensaje);
            }

            if (operacion_exitosa)
            {
                if (archivoImagen != null)
                {
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oProducto.PRO_ID.ToString(), extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        guardar_imagen_exito = false;
                    }


                    if (guardar_imagen_exito)
                    {
                        oProducto.PRO_RUTAIMAGEN = ruta_guardar;
                        oProducto.PRO_NOMBREIMAGEN = nombre_imagen;
                        bool rspta = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardo el producto, pero hubo problemas con la imagen";
                    }
                }
            }



            return Json(new { operacionExitosa = operacion_exitosa, idGenerado = oProducto.PRO_ID, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {

            bool conversion;
            Producto oproducto = new CN_Producto().Listar().Where(p => p.PRO_ID == id).FirstOrDefault();

            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oproducto.PRO_RUTAIMAGEN, oproducto.PRO_NOMBREIMAGEN), out conversion);


            return Json(new
            {
                conversion = conversion,
                textobase64 = textoBase64,
                extension = Path.GetExtension(oproducto.PRO_NOMBREIMAGEN)

            },
             JsonRequestBehavior.AllowGet
            );


        }


        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Producto().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}