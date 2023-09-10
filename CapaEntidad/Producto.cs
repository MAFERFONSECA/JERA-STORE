using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Producto
    {
        public int PRO_ID { get; set; }
        public string PRO_NOMBRE { get; set; }
        public string PRO_DESCRIPCION { get; set; }
        public Marca oMarca { get; set; }
        public Categoria oCategoria { get; set; }
        public decimal PRO_PRECIO { get; set; }

        //public string PrecioTexto { get; set; }

        public int PRO_STOCK { get; set; }
        public string PRO_RUTAIMAGEN { get; set; }
        public string PRO_NOMBREIMAGEN { get; set; }
        public bool PRO_ACTIVO { get; set; }

        //public string Base64 { get; set; }

        //public string Extension { get; set; }
    }
}
