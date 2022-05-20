using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISTEMA_DE_FACTURACIÓN_LULY.Clases
{
    public static class AutoCarga
    {
        //para no estar cargando datos cada ves que se inicia elprograma
        public static void Cargarproducto(ref List<Producto> ListaProducto)
        {
            ListaProducto.Add(new Producto()
            {
                IdProducto = 1,
                CodigoProducto = "1",
                CodigoBarraProducto = "659896652",
                DescripcionProducto = "Galleta x100gr",
                PrecCostoProducto = decimal.Parse("10"),
                PrecPublicoProducto = decimal.Parse("15"),
                StockProducto = int.Parse("100")
            });
            ListaProducto.Add(new Producto()
            {
                IdProducto = 2,
                CodigoProducto = "2",
                CodigoBarraProducto = "745278278",
                DescripcionProducto = "Galleta x200gr",
                PrecCostoProducto = decimal.Parse("15"),
                PrecPublicoProducto = decimal.Parse("18"),
                StockProducto = int.Parse("100")
            });
            ListaProducto.Add(new Producto()
            {
                IdProducto = 3,
                CodigoProducto = "3",
                CodigoBarraProducto = "194828947",
                DescripcionProducto = "Ades 1L",
                PrecCostoProducto = decimal.Parse("15"),
                PrecPublicoProducto = decimal.Parse("20"),
                StockProducto = int.Parse("100")
            });
        }

        public static void Cargarcliente(ref List<Cliente> ListaCliente)
        {
            ListaCliente.Add(new Cliente()
                {
                    ClienteId = 1,
                    ClienteNombre = "Lucas",
                    ClienteApellido = "Gonzales",
                    ClienteCuit = "21536948923",
                    ClienteEmail = "juangonz@gmail.com",
                    ClienteCodigo = "1",
                    ClienteCelular = "156896325",
                    ClienteTelefono = "48695231",
                    Haber = 500
                });

             ListaCliente.Add(new Cliente()
            {
                ClienteId = 2,
                ClienteNombre = "Rafael",
                ClienteApellido = "Martinez",
                ClienteCuit = "20879945925",
                ClienteEmail = "Gabriel@gmail.com",
                ClienteCodigo = "2",
                ClienteCelular = "159987162",
                ClienteTelefono = "432586",
                Haber = 100
            });
             ListaCliente.Add(new Cliente()
             {
                 ClienteId = 3,
                 ClienteNombre = "Nicolas",
                 ClienteApellido = "Cepeda",
                 ClienteCuit = "20879379998",
                 ClienteEmail = "Gabriel@gmail.com",
                 ClienteCodigo = "3",
                 ClienteCelular = "151803680",
                 ClienteTelefono = "42569146",
                 Haber = 1000
             });


        }
        public static void CargarEmpleado(ref List<Empleado> ListaEmpleado)
        {
            ListaEmpleado.Add(new Empleado()
            {
                EmpleadoId = 1,
                EmpleadoNombre = "Ezequiel",
                EmpleadoApellido = "Mendoza",
                EmpleadoDNI = "34562358",
                EmpleadoEmail = "eze_mendoza@live.com",
                EmpleadoCodigo = "1",
                EmpleadoCelular = "156982679",
                EmpleadoTelefono = "42308693",
                EmpleadoSueldo = decimal.Parse("3500")
            });
            ListaEmpleado.Add(new Empleado()
            {
                EmpleadoId = 2,
                EmpleadoNombre = "Dario",
                EmpleadoApellido = "Lazarte",
                EmpleadoDNI = "34981240",
                EmpleadoEmail = "dariolazart_90@hotmail.com",
                EmpleadoCodigo = "2",
                EmpleadoCelular = "1562089589",
                EmpleadoTelefono = "423971983",
                EmpleadoSueldo = decimal.Parse("3500")
            });

            ListaEmpleado.Add(new Empleado()
            {
                EmpleadoId = 3,
                EmpleadoNombre = "Mateo",
                EmpleadoApellido = "Martinez",
                EmpleadoDNI = "34671908",
                EmpleadoEmail = "mateu_98@live.com",
                EmpleadoCodigo = "3",
                EmpleadoCelular = "152061893",
                EmpleadoTelefono = "4248160",
                EmpleadoSueldo = decimal.Parse("3500")
            });
        }
    }
}

