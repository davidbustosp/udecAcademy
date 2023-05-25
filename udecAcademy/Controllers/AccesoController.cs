using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using udecAcademy.Permisos;

using udecAcademy.Models;



namespace udecAcademy.Controllers
{
    //[ValidarSesion]
    public class AccesoController : Controller
    {
        // GET: Acceso
        //Definición de la cadena de conexión
        private SqlConnection con;
        private void Conectar()
        {
            string constr = ConfigurationManager.ConnectionStrings["administracion"].ToString();
            con = new SqlConnection(constr);
        }
        //Fin conexión


        public ActionResult Login()
        {
            return View();
        }
        /*public ActionResult Registrar()
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
            mensaje = comando.Parameters["Mensaje"].ToString();

            ViewData["Mensaje"] =mensaje;

            if (registrado)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                return View();
            }

            
        }*/
        //Login Usuarios
        [HttpPost]
        public ActionResult Login(Usuario oUsuario)
        {
            //oUsuario.ClaveUsuario = GetSHA256(oUsuario.ClaveUsuario);
            if(oUsuario.LoginUsuario==null || oUsuario.ClaveUsuario==null)
            {
                oUsuario.LoginUsuario = "INCORRECTO";
                oUsuario.ClaveUsuario = "INCORECTO";
            }
            Conectar();
            SqlCommand comando = new SqlCommand("sp_validausuario", con);
            comando.Parameters.AddWithValue("LoginUsuario", oUsuario.LoginUsuario);
            comando.Parameters.AddWithValue("ClaveUsuario", oUsuario.ClaveUsuario);
            comando.CommandType = CommandType.StoredProcedure;

            con.Open();

            oUsuario.IdUsuario= Convert.ToInt32(comando.ExecuteScalar().ToString());

            if (oUsuario.IdUsuario != 0)
            {
                Session["usuario"] = oUsuario;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "Usuario no Encontrado";
                return View();
            }

            
        }

        private string GetSHA256(string texto)
        {
            var Sb = new StringBuilder();
            using (SHA256 hash=SHA256Managed.Create())
            {
                Encoding enc=Encoding.UTF8;
                byte[] result=hash.ComputeHash(enc.GetBytes(texto));
                foreach(byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}