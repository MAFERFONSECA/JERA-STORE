using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class DetalleVenta
    {
        public int DETV_ID { get; set; }
        public int VEN_ID { get; set; }
        public Producto oProducto { get; set; }
        public int DETV_CANTIDAD { get; set; }
        public decimal DETV_TOTAL { get; set; }
        public string VEN_IDTRANSACCION { get; set; }
    }
}
