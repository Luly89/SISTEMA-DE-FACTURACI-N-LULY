using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISTEMA_DE_FACTURACIÓN_LULY.Clases
{
    public class Empleado
    {
        public int EmpleadoId { get; set; }
        public string EmpleadoCodigo { get; set; }
        public string EmpleadoApellido { get; set; }
        public string EmpleadoNombre { get; set; }
        public string EmpleadoApellidoyNombre { get { return EmpleadoApellido + " " + EmpleadoNombre; } }
        public string EmpleadoDNI { get; set; }
        public string EmpleadoANyDNI { get { return EmpleadoApellido + " " + EmpleadoNombre + " DNI " + EmpleadoDNI; } }
        public string EmpleadoTelefono { get; set; }
        public string EmpleadoEmail { get; set; }
        public string EmpleadoCelular { get; set; }
        public decimal EmpleadoSueldo { get; set; }


    }
}
