using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Registrar(Cliente objeto)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["CLI_NOMBRES"] = string.IsNullOrEmpty(objeto.CLI_NOMBRES) ? "" : objeto.CLI_NOMBRES;
            ViewData["CLI_APELLIDOS"] = string.IsNullOrEmpty(objeto.CLI_APELLIDOS) ? "" : objeto.CLI_APELLIDOS;
            ViewData["CLI_CORREO"] = string.IsNullOrEmpty(objeto.CLI_CORREO) ? "" : objeto.CLI_CORREO;

            if (objeto.CLI_CLAVE != objeto.ConfirmarClave)
            {

                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }


            resultado = new CN_Cliente().Registrar(objeto, out mensaje);

            if (resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }

        }


        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Cliente oCliente = null;

            oCliente = new CN_Cliente().Listar().Where(item => item.CLI_CORREO == correo && item.CLI_CLAVE == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();


            if (oCliente == null)
            {
                ViewBag.Error = "Correo o contraseña no son correctos";
                return View();

            }
            else
            {

                if (oCliente.CLI_REESTABLECER)
                {
                    TempData["CLI_ID"] = oCliente.CLI_ID;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {

                    FormsAuthentication.SetAuthCookie(oCliente.CLI_CORREO, false);

                    Session["Cliente"] = oCliente;

                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");


                }

            }
        }


        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {


            Cliente cliente = new Cliente();

            cliente = new CN_Cliente().Listar().Where(item => item.CLI_CORREO == correo).FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "No se encontro un cliente relacionado a ese correo";
                return View();
            }


            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().ReestablecerClave(cliente.CLI_ID, correo, out mensaje);

            if (respuesta)
            {

                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");

            }
            else
            {

                ViewBag.Error = mensaje;
                return View();
            }


        }


        [HttpPost]
        public ActionResult CambiarClave(string idcliente, string claveactual, string nuevaclave, string confirmaclave)
        {

            Cliente oCliente = new Cliente();

            oCliente = new CN_Cliente().Listar().Where(u => u.CLI_ID == int.Parse(idcliente)).FirstOrDefault();

            if (oCliente.CLI_CLAVE != CN_Recursos.ConvertirSha256(claveactual))
            {
                TempData["CLI_ID"] = idcliente;
                ViewData["vclave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if (nuevaclave != confirmaclave)
            {

                TempData["CLI_ID"] = idcliente;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            ViewData["vclave"] = "";


            nuevaclave = CN_Recursos.ConvertirSha256(nuevaclave);


            string mensaje = string.Empty;

            bool respuesta = new CN_Cliente().CambiarClave(int.Parse(idcliente), nuevaclave, out mensaje);

            if (respuesta)
            {

                return RedirectToAction("Index");
            }
            else
            {

                TempData["CLI_ID"] = idcliente;

                ViewBag.Error = mensaje;
                return View();
            }

        }

        public ActionResult CerrarSesion()
        {
            //Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");

        }
    }
}