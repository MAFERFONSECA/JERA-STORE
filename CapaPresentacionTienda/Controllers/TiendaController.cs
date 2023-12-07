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
using System.Threading.Tasks;
using System.Data;
using System.Globalization;

using CapaEntidad.Paypal;
using Newtonsoft.Json.Linq;
using CapaPresentacionTienda.Filter;

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
        [ValidarSession]
        [Authorize]
        public ActionResult Carrito()
        {
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> oListaCarrito, Venta oVenta)
        {
            decimal total = 0;

            DataTable detalle_venta = new DataTable();
            detalle_venta.Locale = new CultureInfo("es-MX");
            detalle_venta.Columns.Add("PRO_ID", typeof(string));
            detalle_venta.Columns.Add("DETV_CANTIDAD", typeof(int));
            detalle_venta.Columns.Add("DETV_TOTAL", typeof(decimal));

            List<Item>oListaItem = new List<Item>();


            foreach(Carrito oCarrito in oListaCarrito)
            {
                decimal subtotal = Convert.ToDecimal(oCarrito.CARR_CANTIDAD.ToString()) * oCarrito.oProducto.PRO_PRECIO;

                total += subtotal;
                oListaItem.Add(new Item
                {
                    name = oCarrito.oProducto.PRO_NOMBRE,
                    quantity = oCarrito.CARR_CANTIDAD.ToString(),
                    unit_amount = new UnitAmount()
                    {
                        currency_code = "MXN",
                        value = oCarrito.oProducto.PRO_PRECIO.ToString("G",new CultureInfo("es-MX"))
                    }
                });

                detalle_venta.Rows.Add(new object[]
                {
                    oCarrito.oProducto.PRO_ID,
                    oCarrito.CARR_CANTIDAD,
                    subtotal
                });


            }


            PurchaseUnit purchaseUnit = new PurchaseUnit()
            {
                amount = new Amount()
                {
                    currency_code = "MXN",
                    value = total.ToString("G", new CultureInfo("es-MX")),
                    breakdown = new Breakdown()
                    {
                        item_total = new ItemTotal()
                        {
                            currency_code = "MXN",
                            value = total.ToString("G", new CultureInfo("es-MX")),
                        }
                    }


                },
                description = "Compra de articulo de Jeras Store",
                items = oListaItem
            };

            Checkout_Order oCheckOutOrder = new Checkout_Order()
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit>() { purchaseUnit },
                application_context = new ApplicationContext()
                {
                    brand_name = "Jerasstore.com",
                    landing_page = "NO_PREFERENCE",
                    user_action = "PAY_NOW",
                    return_url = "https://localhost:44307/Tienda/PagoEfectuado",
                    cancel_url = "https://localhost:44307/Tienda/Carrito"
                }
            };


            oVenta.VEN_MONTOTOTAL = total;
            oVenta.CLI_ID = ((Cliente)Session["Cliente"]).CLI_ID;

            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_venta;



            CN_Paypal opaypal = new CN_Paypal();

            Response_Paypal<Response_Checkout> response_Paypal = new Response_Paypal<Response_Checkout>();

            response_Paypal = await opaypal.Crearsolicitud(oCheckOutOrder);


            return Json(response_Paypal, JsonRequestBehavior.AllowGet);
        }

        [ValidarSession]
        [Authorize]
        public async Task<ActionResult> PagoEfectuado()
        {
            string token = Request.QueryString["token"];

            CN_Paypal opaypal = new CN_Paypal();
            Response_Paypal<Response_Capture> response_Paypal = new Response_Paypal<Response_Capture>();
            response_Paypal = await opaypal.AprobarPago(token);

            ViewData["Status"] = response_Paypal.Status;

            if (response_Paypal.Status)
            {
                Venta oVenta = (Venta)TempData["Venta"];

                DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];

                oVenta.VEN_IDTRANSACCION = response_Paypal.Response.purchase_units[0].payments.captures[0].id;

                string mensaje = string.Empty;

                bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out mensaje);

                ViewData["IdTransaccion"] = oVenta.VEN_IDTRANSACCION;
            }
            return View();
        }


        [ValidarSession]
        [Authorize]
        public ActionResult Miscompras()
        {
            int idcliente = ((Cliente)Session["Cliente"]).CLI_ID;

            List<DetalleVenta> oLista = new List<DetalleVenta>();

            bool conversion;

            oLista = new CN_Venta().ListarCompras(idcliente).Select(oc => new DetalleVenta()
            {
                oProducto = new Producto()
                {
                    PRO_NOMBRE = oc.oProducto.PRO_NOMBRE,
                    PRO_PRECIO = oc.oProducto.PRO_PRECIO,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.oProducto.PRO_RUTAIMAGEN, oc.oProducto.PRO_NOMBREIMAGEN), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.PRO_NOMBREIMAGEN)
                },
                DETV_CANTIDAD = oc.DETV_CANTIDAD,
                DETV_TOTAL = oc.DETV_TOTAL,
                VEN_IDTRANSACCION = oc.VEN_IDTRANSACCION,


            }).ToList();

            return View(oLista);

        }

    }
}