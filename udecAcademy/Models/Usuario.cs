using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;




namespace udecAcademy.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int PerfilUsuario { get; set; }
        public string LoginUsuario { get; set; }
        public int IdTipoDocUsuario { get; set; }
        public string DocumentoUsuario { get; set; }
        public string Nombre1Usuario { get; set; }
        public string Nombre2Usuario { get; set; }
        public string Apellido1Usuario { get; set; }
        public string Apellido2Usuario { get; set; }
        public string ClaveUsuario { get; set; }
        public string EmailUsuario { get; set; }

        public string ConfirmarClave { get; set; }
        

    }
}