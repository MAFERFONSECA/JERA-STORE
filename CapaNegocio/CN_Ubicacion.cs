using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Ubicacion
    {
        private CD_Ubicacion objCapaDato = new CD_Ubicacion();


        public List<Municipio> ObtenerMunicipio()
        {
            return objCapaDato.ObtenerMunicipio();
        }

        public List<Ciudad> ObtenerCiudad(string idmunicipio)
        {
            return objCapaDato.ObtenerCiudad(idmunicipio);
        }

        public List<Colonia> ObtenerColonia(string idmunicipio, string idciudad)
        {
            return objCapaDato.ObtenerColonia(idmunicipio, idciudad);
        }

    }
}
