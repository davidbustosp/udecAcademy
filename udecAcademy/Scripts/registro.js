function validaRegistro() {
    var clave = document.forms["registro"]["ClaveUsuario"].value;

    if (clave == null || clave= "")
    {
        alert("Debe ingresar una clave");
        return false;
    }
}