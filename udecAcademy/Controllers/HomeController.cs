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
using System.Net.Http.Headers;
using System.Web.Services.Description;
using System.Security.Cryptography.X509Certificates;
using System.EnterpriseServices.Internal;

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

        public ActionResult ConsultaUsuarios()
        {
            return View(ListarUsuarios());
        }
       
        public List<Usuario> ListarUsuarios()
        {
            Conectar();
            List<Usuario> usuarios=new List<Usuario>();
            SqlCommand comando = new SqlCommand("sp_consultaUsuarios",con);
            con.Open();
            SqlDataReader registros=comando.ExecuteReader();
            while(registros.Read())
            {
                Usuario user = new Usuario
                {
                    IdUsuario = int.Parse(registros["idUsuario"].ToString()),
                    PerfilUsuario = int.Parse(registros["idPerfilUsuario"].ToString()),
                    NombrePerfil = registros["nombrePerfil"].ToString(),
                    LoginUsuario = registros["loginUsuario"].ToString(),
                    IdTipoDocUsuario = int.Parse(registros["idTipoDocUsuario"].ToString()),
                    NombreTipDoc = registros["nombreTipDoc"].ToString(),
                    DocumentoUsuario = registros["documentoUsuario"].ToString(),
                    Nombre1Usuario = registros["nombre1Usuario"].ToString(),
                    Nombre2Usuario = registros["nombre2Usuario"].ToString(),
                    Apellido1Usuario = registros["apellido1Usuario"].ToString(),
                    Apellido2Usuario = registros["apellido2Usuario"].ToString(),
                    EmailUsuario = registros["emailUsuario"].ToString()
                };
                usuarios.Add(user);
            }
            con.Close();
            return usuarios;
        }
        
        

        public ActionResult ModificarUsuario( int cod)
        {
            MantenimientoUsuario ma = new MantenimientoUsuario();
            Usuario user = ma.RecuperaUsuario(cod);
            return View(user);
        }
        [HttpPost]

        public ActionResult ModificarUsuario(Usuario oUsuario)
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
            SqlCommand comando = new SqlCommand("sp_modificaUsuario", con);
            comando.Parameters.AddWithValue("IdUsuario", oUsuario.IdUsuario);
            comando.Parameters.AddWithValue("IdPerfilUsuario", oUsuario.PerfilUsuario);
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
        public ActionResult EliminaUsuario(int cod)
        {
            return View();
        }
       


    }
}