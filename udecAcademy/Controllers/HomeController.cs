using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using udecAcademy.Models;
using udecAcademy.Permisos;
using System.Configuration;

namespace udecAcademy.Controllers
{
    [ValidarSesion]
    public class HomeController : Controller
    {
        private SqlConnection con;
        private void Conectar()
        {
            string constr = ConfigurationManager.ConnectionStrings["administracion"].ToString();
            con = new SqlConnection(constr);
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult CerrarSesion()
        {
            Session["usuario"] = null;
            return RedirectToAction("Login", "Acceso");
        }
        public ActionResult Registrar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registrar(Usuario oUsuario)
        {
            bool registrado;
            string mensaje;
            if (oUsuario.ClaveUsuario == oUsuario.ConfirmarClave)
            {
                //oUsuario.ClaveUsuario = GetSHA256(oUsuario.ClaveUsuario);
                oUsuario.ClaveUsuario = oUsuario.ClaveUsuario;

            }
            else
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }
            Conectar();
            SqlCommand comando = new SqlCommand("sp_registraUsuario", con);
            comando.Parameters.AddWithValue("IdPerfilUsuario", oUsuario.PerfilUsuario);
            comando.Parameters.AddWithValue("LoginUsuario", oUsuario.LoginUsuario);
            comando.Parameters.AddWithValue("IdTipoDocUsuario", oUsuario.IdTipoDocUsuario);
            comando.Parameters.AddWithValue("DocumentoUsuario", oUsuario.DocumentoUsuario);
            comando.Parameters.AddWithValue("Nombre1Usuario", oUsuario.Nombre1Usuario);
            comando.Parameters.AddWithValue("Nombre2Usuario", oUsuario.Nombre2Usuario);
            comando.Parameters.AddWithValue("Apellido1Usuario", oUsuario.Apellido1Usuario);
            comando.Parameters.AddWithValue("Apellido2usuario", oUsuario.Apellido2Usuario);
            comando.Parameters.AddWithValue("ClaveUsuario", oUsuario.ClaveUsuario);
            comando.Parameters.AddWithValue("EmailUsuario", oUsuario.EmailUsuario);

            comando.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            comando.CommandType = CommandType.StoredProcedure;
            con.Open();
            comando.ExecuteNonQuery();

            registrado = Convert.ToBoolean(comando.Parameters["Registrado"].Value);
            mensaje = comando.Parameters["Mensaje"].Value.ToString();

            ViewData["Mensaje"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }


        }
        
        /*public ActionResult Registrar()
        {
            Session["usuario"] = null;
            return RedirectToAction("Registrar", "Acceso");
        }*/


    }
}