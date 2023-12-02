using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.USU_CORREO== correo && u.USU_CLAVE == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();


            if (oUsuario == null)
            {

                ViewBag.Error = "Correo o contraseña no correctos";
                return View();
            }
            else
            {

                if (oUsuario.USU_REESTABLECER)
                {

                    TempData["USU_ID"] = oUsuario.USU_ID;
                    return RedirectToAction("CambiarClave");

                }


                FormsAuthentication.SetAuthCookie(oUsuario.USU_CORREO, false);

                ViewBag.Error = null;

                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CambiarClave(string idusuario, string claveactual, string nuevaclave, string confirmarclave)
        {

            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.USU_ID == int.Parse(idusuario)).FirstOrDefault();

            if (oUsuario.USU_CLAVE != CN_Recursos.ConvertirSha256(claveactual))
            {
                TempData["USU_ID"] = idusuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if (nuevaclave != confirmarclave)
            {

                TempData["USU_ID"] = idusuario;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            ViewData["vclave"] = "";


            nuevaclave = CN_Recursos.ConvertirSha256(nuevaclave);


            string mensaje = string.Empty;

            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idusuario), nuevaclave, out mensaje);

            if (respuesta)
            {

                return RedirectToAction("Index");
            }
            else
            {

                TempData["USU_ID"] = idusuario;

                ViewBag.Error = mensaje;
                return View();
            }

        }


        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {

            Usuario ousurio = new Usuario();

            ousurio = new CN_Usuarios().Listar().Where(item => item.USU_CORREO == correo).FirstOrDefault();

            if (ousurio == null)
            {
                ViewBag.Error = "No se encontro un usuario relacionado a ese correo";
                return View();
            }


            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().ReestablecerClave(ousurio.USU_ID, correo, out mensaje);

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

        public ActionResult CerrarSesion()
        {

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");

        }
    }
}