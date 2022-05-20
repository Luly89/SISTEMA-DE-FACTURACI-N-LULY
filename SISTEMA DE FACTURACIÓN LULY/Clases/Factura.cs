using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISTEMA_DE_FACTURACIÓN_LULY.Clases
{
    public class Factura
    {
        public int FacturaId { get; set; }
        public string Codigo { get; set; }
        public string EmpleadoCodigo { get; set; }
        public string ApellidoyNombre { get; set; }
        public string DNI { get; set; }
        public string NumeroFactura { get; set; }
        public string FormaDePago { get; set; }
        public string TipoFactura { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
    }
}
