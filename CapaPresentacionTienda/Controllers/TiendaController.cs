using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;
using System.IO;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
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

    }
}