using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cliente
    {
        public int CLI_ID { get; set; }
        public string CLI_NOMBRES { get; set; }
        public string CLI_APELLIDOS { get; set; }
        public string CLI_CORREO { get; set; }
        public string CLI_CLAVE { get; set; }
        public string ConfirmarClave { get; set; }
        public bool CLI_REESTABLECER { get; set; }
    }
}
