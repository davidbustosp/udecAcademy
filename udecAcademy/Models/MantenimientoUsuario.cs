using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace udecAcademy.Models

{
    public class MantenimientoUsuario
    {
        private SqlConnection con;

        public object ViewData { get; private set; }

        private void Conectar()
        {
            string constr = ConfigurationManager.ConnectionStrings["administracion"].ToString();
            con = new SqlConnection(constr);
        }

        public Usuario RecuperaUsuario(int idUser)
        {
            Conectar();
            SqlCommand comando = new SqlCommand("sp_consultaUsuario", con);
            comando.Parameters.AddWithValue("idUsuario", idUser);
            comando.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader registro = comando.ExecuteReader();
            Usuario user = new Usuario();
            if (registro.Read())
            {
                user.IdUsuario = int.Parse(registro["idUsuario"].ToString());
                user.PerfilUsuario = int.Parse(registro["idPerfilUsuario"].ToString());
                user.NombrePerfil = registro["nombrePerfil"].ToString();
                user.LoginUsuario = registro["loginUsuario"].ToString();
                user.IdTipoDocUsuario = int.Parse(registro["idTipoDocUsuario"].ToString());
                user.DocumentoUsuario = registro["documentoUsuario"].ToString();
                user.Nombre1Usuario = registro["nombre1Usuario"].ToString();
                user.Nombre2Usuario = registro["nombre2usuario"].ToString();
                user.Apellido1Usuario = registro["Apellido1Usuario"].ToString();
                user.Apellido2Usuario = registro["Apellido2Usuario"].ToString();
                user.EmailUsuario = registro["emailUsuario"].ToString();
            }
            con.Close();
            return user;
        }

        public int Modificar(Usuario oUsuario)
        {
            bool registrado;
            string mensaje;

            Conectar();
            SqlCommand comando = new SqlCommand("sp_EliminaUsuario");
            comando.Parameters.AddWithValue("IdUsuario", oUsuario.IdUsuario);
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
            int i=comando.ExecuteNonQuery();

            registrado = Convert.ToBoolean(comando.Parameters["Registrado"].Value);
            mensaje = comando.Parameters["Mensaje"].Value.ToString();

            con.Close();
            return i;




        }
        
        public int Eliminar(int codigo)
        {

            bool registrado;
            string mensaje;
            

            Conectar();
            SqlCommand comando=new SqlCommand("sp_EliminaUsuario", con);
            comando.Parameters.AddWithValue("IdUsuario", codigo);

            comando.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

            comando.CommandType = CommandType.StoredProcedure;
            con.Open();
            int i=comando.ExecuteNonQuery();

            registrado = Convert.ToBoolean(comando.Parameters["Registrado"].Value);
            mensaje = comando.Parameters["Mensaje"].Value.ToString();
            

            con.Close();
            return i;
        }


    }
}