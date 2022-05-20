using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISTEMA_DE_FACTURACIÓN_LULY.Clases
{
   public class Producto
    {
       public int IdProducto { get; set; }
        public string CodigoProducto { get; set; }
        public string CodigoBarraProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public decimal PrecCostoProducto { get; set; }
        public decimal PrecPublicoProducto { get; set; }
        public int StockProducto { get; set; }

    }
}
