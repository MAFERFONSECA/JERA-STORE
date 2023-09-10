using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Venta
    {
        public int VEN_ID { get; set; }
        public int CLI_ID { get; set; }
        public int VEN_TOTALPRODUCTO { get; set; }
        public decimal VEN_MONTOTOTAL { get; set; }
        public string VEN_CONTACTO { get; set; }
        public string COL_ID { get; set; }
        public string VEN_TELEFONO { get; set; }
        public string VEN_DIRECCION { get; set; }
        public string FechaTexto { get; set; }
        public string VEN_IDTRANSACCION { get; set; }
        public List<DetalleVenta> oDetalleVenta { get; set; }
    }
}
