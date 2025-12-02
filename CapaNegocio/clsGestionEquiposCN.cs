using CapaDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class clsGestionEquiposCN
    {
        clsGestionEquipoCD ObjGestionEquipo = new clsGestionEquipoCD();
        
        public void mtdInsertarEquipoCN(int idCreador, string nombre, int idDeporte, string descripcion)
        {
            ObjGestionEquipo.mtdInsertarEquipoCD(idCreador, nombre, idDeporte, descripcion);
        }

        public DataTable mtdCListarDeportesActivosCN()
        {
            return ObjGestionEquipo.mtdCListarDeportesActivosCD();
        }

        public DataTable mtdListarEquiposActivosCN(int idUsuario)
        {
            return ObjGestionEquipo.mtdListarEquiposActivosCD(idUsuario);
        }

        public void mtdEliminarLogicoEquipoCN(int idEquipo)
        {
            ObjGestionEquipo.mtdEliminarLogicoEquipoCD(idEquipo);
        }

        public void mtdModificarEquipoCN(int idEquipo, string nombre, string descripcion)
        {
            ObjGestionEquipo.mtdModificarEquipoCD(idEquipo, nombre, descripcion);
        }

        public DataTable mtdListarIntegrantesCN(int idEquipo)
        {
            return ObjGestionEquipo.mtdListarIntegrantesCD(idEquipo);
        }

        public void mtdInsertarInvitacionCN(int idEquipo, int idUsuarioEmisor, int idUsuarioReceptor)
        {
            ObjGestionEquipo.mtdInsertarInvitacionCD(idEquipo, idUsuarioEmisor, idUsuarioReceptor);
        }

        public DataTable mtdListarUsuariosParaInvitacionCN(string busqueda = null)
        {
            return ObjGestionEquipo.mtdListarUsuariosParaInvitacionCD(busqueda);
        }

        public DataTable mtdListarInvitacionesCN(int idUsuario)
        {
            return ObjGestionEquipo.mtdListarInvitacionesCD(idUsuario);
        }

        public void mtdAceptarInvitacionCN(int idInvitacion, int idUsuarioReceptor)
        {
            ObjGestionEquipo.mtdAceptarInvitacionCD(idInvitacion, idUsuarioReceptor);
        }

        public void mtdRechazarInvitacionCN(int idInvitacion, int idUsuarioReceptor)
        {
            ObjGestionEquipo.mtdRechazarInvitacionCD(idInvitacion, idUsuarioReceptor);
        }

    }
}
