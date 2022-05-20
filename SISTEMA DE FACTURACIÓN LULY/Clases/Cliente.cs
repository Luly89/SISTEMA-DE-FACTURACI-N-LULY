using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISTEMA_DE_FACTURACIÓN_LULY.Clases
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string ClienteCodigo { get; set; }
        public string ClienteApellido { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteApellidoyNombre { get { return ClienteApellido + " " + ClienteNombre; } }
        public string ClienteANyCuit { get {return ClienteApellido + " " + ClienteNombre + " Cuit " + ClienteCuit;} }
        public string ClienteCuit { get; set; }
        public string ClienteTelefono { get; set; }
        public string ClienteEmail { get; set; }
        public string ClienteCelular { get; set; }
        public decimal Haber { get; set; }
        public decimal Deuda { get; set; }


    }
}
