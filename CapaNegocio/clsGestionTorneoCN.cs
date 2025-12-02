using CapaDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class clsGestionTorneoCN
    {
        clsGestionTorneoCD ObjTorneo = new clsGestionTorneoCD();

        public void mtdInsertarTorneoCN(int idCreador, string nombre, string descripcion)
        {
            ObjTorneo.mtdInsertarTorneoCD(idCreador, nombre, descripcion);
        }

        public DataTable mtdListarTorneosActivosPorUsuarioCN(int idUsuario)
        {
            return ObjTorneo.mtdListarTorneosActivosPorUsuarioCD(idUsuario);
        }

        public void mtdModificarTorneoCN(int idTorneo, int idUsuario, string nombre, string descripcion)
        {
            ObjTorneo.mtdModificarTorneoCD(idTorneo, idUsuario, nombre, descripcion);
        }

        public void mtdEliminarTorneoCN(int idTorneo, int idUsuario)
        {
            ObjTorneo.mtdEliminarTorneoCD(idTorneo, idUsuario);
        }

        public DataTable mtdBuscarEquipoPorNombreCN(string nombre)
        {
            return ObjTorneo.mtdBuscarEquipoPorNombreCD(nombre);
        }

        public string InvitarEquipo(int idTorneo, int idEquipo, int idUsuarioEmisor)
        {
            return ObjTorneo.InvitarEquipoATorneo(idTorneo, idEquipo, idUsuarioEmisor);
        }

        public DataTable ObtenerInvitaciones(int idUsuario)
        {
            return ObjTorneo.ListarInvitaciones(idUsuario);
        }


    }
}
