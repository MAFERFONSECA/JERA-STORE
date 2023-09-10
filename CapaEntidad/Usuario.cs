using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Usuario
    {
        public int USU_ID { get; set; }
        public string USU_NOMBRES { get; set; }
        public string USU_APELLIDOS { get; set; }
        public string USU_CORREO { get; set; }
        public string USU_CLAVE { get; set; }
        public bool USU_REESTABLECER { get; set; }
        public bool USU_ACTIVO { get; set; }
    }
}
