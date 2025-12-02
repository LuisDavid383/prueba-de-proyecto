using CapaDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class clsLoginCN
    {
        clsLoginCD ObjLogin = new clsLoginCD();

        public DataTable mtdLoginCN(string dato, string contraseña)
        {
            DataTable dt = ObjLogin.mtdLoginCD(dato);

            if (dt.Rows.Count == 0)
                return null;

            string hashBD = dt.Rows[0]["Contraseña"].ToString();

            bool valido = BCrypt.Net.BCrypt.Verify(contraseña, hashBD);

            if (!valido)
                return null;

            return dt;
        }

        public void mtdRegistrarUsuarioCN(string Nombres, string ApellidoPaterno, string ApellidoMaterno,
                                     int IDTipoDocumento, string Documento, DateTime FechaNacimiento,
                                     string Telefono, string Correo, string Genero, string NombreUsuario,
                                     string Contraseña)
        {
            // Hash a la contraseña
            string Hash = BCrypt.Net.BCrypt.HashPassword(Contraseña);


            ObjLogin.mtdRegistrarUsuarioCD(
                Nombres,
                ApellidoPaterno,
                ApellidoMaterno,
                IDTipoDocumento,
                Documento,
                FechaNacimiento,
                Telefono,
                Correo,
                Genero,
                NombreUsuario,
                Hash
            );
        }

        public DataTable ListarActivos()
        {
            return ObjLogin.ListarActivosDT();
        }
    }
}
