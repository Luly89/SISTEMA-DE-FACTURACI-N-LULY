using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISTEMA_DE_FACTURACIÓN_LULY.Clases
{
    public class CtaCorriente
    {
        public int CtaID { get; set; }
        public string Codigo { get; set; }
        public string Codigoclit { get; set; }
        public string NumFactura { get; set; }
        public string ApellidoyNombre { get; set; }
        public string Cuil { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Haber { get; set; }
        public decimal Debe { get; set; }
        public string TipoFactura { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public string EstadoFactura { get; set; }

    }
}
