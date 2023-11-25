using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;
using System.IO;
using System.Web.Services.Description;
using System.Runtime.InteropServices;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetalleProducto(int idproducto = 0)
        {
            Producto oProducto = new Producto();
            bool conversion;
            oProducto = new CN_Producto().Listar().Where(p => p.PRO_ID == idproducto).FirstOrDefault();

            if(oProducto != null)
            {
                oProducto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.PRO_RUTAIMAGEN, oProducto.PRO_NOMBREIMAGEN), out conversion);
                oProducto.Extension = Path.GetExtension(oProducto.PRO_NOMBREIMAGEN);
            }

            return View(oProducto);
        }

        [HttpGet] 
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista= new List<Categoria>();


            lista = new CN_Categoria().Listar();

            return Json(new {data= lista}, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ListarMarcaporCategoria( int idcategoria)
        {
            List<Marca> lista = new List<Marca>();


            lista = new CN_Marca().ListarMarcaporCategoria(idcategoria);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca)
        {
            List<Producto> lista = new List<Producto>();

            bool conversion;

            lista= new CN_Producto().Listar().Select(p => new Producto()
            {
                PRO_ID=p.PRO_ID,
                PRO_NOMBRE=p.PRO_NOMBRE,
                PRO_DESCRIPCION=p.PRO_DESCRIPCION,
                oMarca=p.oMarca,
                oCategoria=p.oCategoria,
                PRO_PRECIO=p.PRO_PRECIO,
                PRO_STOCK=p.PRO_STOCK,
                PRO_RUTAIMAGEN=p.PRO_RUTAIMAGEN,
                Base64= CN_Recursos.ConvertirBase64(Path.Combine(p.PRO_RUTAIMAGEN, p.PRO_NOMBREIMAGEN), out conversion),
                Extension= Path.GetExtension(p.PRO_NOMBREIMAGEN),
                PRO_ACTIVO=p.PRO_ACTIVO,
            }).Where(p=> 
            p.oCategoria.CAT_ID == (idcategoria == 0 ? p.oCategoria.CAT_ID: idcategoria) &&
            p.oMarca.MAR_ID == (idmarca == 0 ? p.oMarca.MAR_ID : idmarca) &&
            p.PRO_STOCK>0 && p.PRO_ACTIVO == true
            ).ToList();


            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength=int.MaxValue;
            return jsonresult;

        }


        [HttpPost]
        public JsonResult AgregarCarrito (int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).CLI_ID;

            bool existe = new CN_Carrito().ExisteCarrito(idcliente, idproducto);

            bool respuesta = false;

            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                respuesta = new CN_Carrito().OperacionCarrito(idcliente,idproducto,true, out mensaje);
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).CLI_ID;
            int cantidad = new CN_Carrito().CantidadEnCarrito(idcliente);
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult ListarProductosCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).CLI_ID;

            List<Carrito> oLista = new List<Carrito>();

            bool conversion;

            oLista = new CN_Carrito().ListarProducto(idcliente).Select(oc => new Carrito()
            {
                oProducto = new Producto()
                {
                    PRO_ID = oc.oProducto.PRO_ID,
                    PRO_NOMBRE = oc.oProducto.PRO_NOMBRE,
                    oMarca = oc.oProducto.oMarca,
                    PRO_PRECIO = oc.oProducto.PRO_PRECIO,
                    PRO_RUTAIMAGEN = oc.oProducto.PRO_RUTAIMAGEN,
                    Base64 = CN_Recursos.ConvertirBase64( Path.Combine(oc.oProducto.PRO_RUTAIMAGEN, oc.oProducto.PRO_NOMBREIMAGEN), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.PRO_NOMBREIMAGEN)
                },
                CARR_CANTIDAD = oc.CARR_CANTIDAD

            }).ToList();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto, bool sumar)
        {
            int idcliente = ((Cliente)Session["Cliente"]).CLI_ID;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, sumar, out mensaje);


            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).CLI_ID;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CN_Carrito().EliminarCarrito(idcliente, idproducto);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ObtenerMunicipio()
        {
            List<Municipio> oLista = new List<Municipio>();
            oLista = new CN_Ubicacion().ObtenerMunicipio();
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ObtenerCiudad(string IdMunicipio)
        {
            List<Ciudad> oLista = new List<Ciudad>();
            oLista = new CN_Ubicacion().ObtenerCiudad(IdMunicipio);
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerColonia(string IdMunicipio, string IdCiudad)
        {
            List<Colonia> oLista = new List<Colonia>();
            oLista = new CN_Ubicacion().ObtenerColonia(IdMunicipio, IdCiudad);
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Carrito()
        {
            return View();
        }
    }
}