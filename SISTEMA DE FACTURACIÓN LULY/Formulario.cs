using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SISTEMA_DE_FACTURACIÓN_LULY.Clases;

namespace SISTEMA_DE_FACTURACIÓN_LULY
{
    public partial class Formulario : Form
    {
        //declaracion de variables

        public List<Producto> ListaProducto;
        public List<Empleado> ListaEmpleado;
        public List<Cliente> ListaCliente;
        public List<DetalleFactura> ListaDetalleFactura;
        public List<Factura> ListaFacturaEmpleado;
        public List<Factura> ListaFactura;
        public List<CtaCorriente> ListaCtaCorriente;
        public List<CtaCorriente> ListaClienteCtaCorriente;

        //lista para filtrar por fecha marcada las facturas de la ListaFactura
        public List<Factura> ventas;
        public List<CtaCorriente> ListaCtaFiltro;
        int NumFactura = 0;
        public string EmpleadoActualApellidoyNombre;
        public string EmpleadoActualDNI;
        public string EmpleadoActualCodigo;
        public string ClienteActualApellido;
        public string ClienteActualCuil;
        public decimal DeudaTotal;
        public decimal DebeActual;
        public decimal HaberActual;
        //se usa para calcular la comision del empleado elegido en ventas
        public decimal ComisionActual;
        //se usa para poder obtener el sueldo del empleado  elegido en ventas;
        public decimal SueldoEmpleadoActual;
        // variable bool para saber si, el empleado pude cobrar un premio
        public bool Premio;
        // se utiliza para calcular el sueldo Total
        public bool ModoRapido;
        public decimal DeudaFactura;
        public decimal nuevoSaldo;
        public decimal SueldoTotal;
        public Producto ProducActual;
        // para validar si el producto esta cargado o no
        public string ProductoActual;
        public Factura FacturaNueva;
        public CtaCorriente CtaCorrienteNueva;
        public bool debeFactura = true;
        public int contadorClienteId = 3;
        public int contadorEmpleadoId = 3;
        public int ContadorIDProducto = 3;
        public int contadorCtaId = 1;
        public int contadorFacturaId = 1;
        //sirve para guardar el costo de los productos a pesar de que no exista stock
        public decimal CostoProductoAux;
        DateTime fecha1;



        public Formulario()
        {

            InitializeComponent();

            ListaProducto = new List<Producto>();

            //inicializa las variables
            ListaProducto = new List<Producto>();
            ListaEmpleado = new List<Empleado>();
            ListaCliente = new List<Cliente>();
            ListaDetalleFactura = new List<DetalleFactura>();
            ListaFactura = new List<Factura>();
            ListaFacturaEmpleado = new List<Factura>();
            ListaCtaCorriente = new List<CtaCorriente>();
            ListaClienteCtaCorriente = new List<CtaCorriente>();
            ventas = new List<Factura>();
            ListaCtaFiltro = new List<CtaCorriente>();
            ExiteProductosListaFact();
            DebeActual = 0;
            fecha1 = DateTime.Now;
            ModoRapido = false;
            checkBox1.Checked = false;

            this.txtFechaFactura.Text = fecha1.ToString();
        }
        // el loader

        private void Formulario_Load(object sender, EventArgs e)
        {
            AutoCarga.Cargarproducto(ref ListaProducto);
            cargargrillaproducto();
            AutoCarga.Cargarcliente(ref ListaCliente);
            cargargrillaCliente();
            AutoCarga.CargarEmpleado(ref ListaEmpleado);
            CargarComboEmpleado();
            cargargrillaempleado();
            CargarComboCliente();
            cargargrillaproducto();
            cargarGrillaVentaEmpleado();
            CargarComboEmpleado();
            cargarGrillaCtaCte();
            CargarGrillaCtacteClient();
            CalcularGastos();
            FacturaAoB();
            txtNroFactura.Text = "1";
            ExiteProductosListaFact();
            //para que la dtpFecha de ventas de Fecha hasta, sea como el minimo el valor de fecha desde
      }
        // funcion para que al hacer clic en el boton agregar producto, este se agregue
        private void btnAgregarProductoLista_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoProducto.Text) || string.IsNullOrEmpty(txtCodBarraProducto.Text)
                || string.IsNullOrEmpty(txtDescripciónProducto.Text) || nudPrecCostoProducto.Value <= 0 || nudPrecPublicoProducto.Value <= 0 || nudStockProducto.Value <= 0)
            {
                MessageBox.Show("Debe completar todos los campos!");
            }
            else
            {

                if (ExisteProdValidacion(txtCodigoProducto.Text, txtCodBarraProducto.Text))
                {
                    MessageBox.Show("El codigo o codigo de barra ingresado no esta disponible");
                }
                else
                {
                    Producto ObjProducto = new Producto();

                    ObjProducto.IdProducto = ContadorIDProducto;
                    ObjProducto.CodigoProducto = txtCodigoProducto.Text;
                    ObjProducto.CodigoBarraProducto = txtCodBarraProducto.Text;
                    ObjProducto.DescripcionProducto = txtDescripciónProducto.Text;
                    ObjProducto.PrecCostoProducto = nudPrecCostoProducto.Value;
                    ObjProducto.PrecPublicoProducto = nudPrecPublicoProducto.Value;
                    ObjProducto.StockProducto = Convert.ToInt32(nudStockProducto.Value);

                    ListaProducto.Add(ObjProducto);
                    cargargrillaproducto();
                    limpiarProducto();
                    ContadorIDProducto++;
                    MessageBox.Show("Se han guardado los datos");

                }
            }
        }
        private void btnSalirProducto_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Funcion del keypress para buscar un producto cuando hacemos clic en enter

        private void txtBuscarCodFact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

                if (string.IsNullOrEmpty(this.txtBuscarCodFact.Text))
                {
                    MessageBox.Show("Ingrese un codigo, descripcion o codigo de barra para agregar a la factura");
                }
                else
                {
                    if (!(string.IsNullOrEmpty(this.txtBuscarCodFact.Text)))
                    {
                        ProducActual = new Producto();
                        ProducActual = BuscarProd(Convert.ToString(txtBuscarCodFact.Text));

                        if (ProducActual != null)
                        {
                            txtCodigoProductoFactura.Text = Convert.ToString(ProducActual.CodigoProducto);
                            txtDescripcionProductoFactura.Text = ProducActual.DescripcionProducto;
                            nudPrecioProducto.Value = ProducActual.PrecPublicoProducto;

                        }


                        if (string.IsNullOrEmpty(this.txtCodigoProductoFactura.Text) || string.IsNullOrEmpty(txtDescripcionProductoFactura.Text)
                           || nudPrecioProducto.Value <= 0)
                        {
                            MessageBox.Show("El producto ingresado NO EXISTE!");
                        }
                    }
                    txtBuscarCodFact.Clear();
                }
            }
        }
        private void btnAgregarfactura_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtCodigoProductoFactura.Text))
            {
                MessageBox.Show("Ingrese un codigo, descripcion o codigo de barra para agregar a la factura");
            }
            else
            {

                AgregarDF(ProducActual, Convert.ToInt32(nudCantidadDetallesFAct.Value));
                cargargrillaproducto();
                nudCantidadDetallesFAct.Value = 1;
                FacturaAoB();
                ExiteProductosListaFact();
                btnGuardarFactura.Enabled = true;
                btnCtaCteFactura.Enabled = true;
                nudImporteFact.Enabled = true;
            }
        }
        //-------------------------------------------------------------------
        // calcular Subtotal detalle factura
        private void CalculaTotalFact()
        {
            decimal TotalFact = 0;
            foreach (var busc in ListaDetalleFactura)
            {
                TotalFact += busc.SubTotal;
            }
            txtSubTotalFactura.Text = TotalFact.ToString();
        }
        // calcular iva
        public void CalcularIvaDF()
        {
            decimal _iva = 0;
            decimal _suma = 0;
            foreach (var busc in ListaDetalleFactura)
            {
                _suma += busc.SubTotal;
                _iva = (_suma - (_suma / 1.21m));

            }
            txtIvaFact.Text = string.Format("{00:0.00}", _iva);

        }

        // calcular total detalle factura
        public void CalcularTotalDF()
        {
            decimal _total = 0;
            decimal _subtotal = 0;
            decimal _descuento = 0;
            decimal _iva = 0;
            if (nudDescuentoFactura.Value > 0)
            {
                _subtotal = decimal.Parse(txtSubTotalFactura.Text);
                _descuento = nudDescuentoFactura.Value;
                _iva = decimal.Parse(txtIvaFact.Text);
                _total = ((_subtotal + _iva) - ((_subtotal * _descuento) / 100));
                txtTOTALFactura.Text = string.Format("{00:0.00}", _total);
            }
            else
            {
                if (ckbFacturaB.Checked == true)
                {
                    _total = Convert.ToDecimal(txtSubTotalFactura.Text);
                    _subtotal = decimal.Parse(txtSubTotalFactura.Text);
                    _descuento = nudDescuentoFactura.Value;

                    _total = (_subtotal + _iva - ((_subtotal * _descuento) / 100));
                    txtTOTALFactura.Text = string.Format("{00:0.00}", _total);
                }
                else if (ckbFacturaA.Checked == true)
                {
                    _total = Convert.ToDecimal(txtSubTotalFactura.Text);
                    _subtotal = decimal.Parse(txtSubTotalFactura.Text);
                    _descuento = nudDescuentoFactura.Value;
                    _iva = decimal.Parse(txtIvaFact.Text);
                    _total = ((_subtotal + _iva) - ((_subtotal * _descuento) / 100));
                    txtTOTALFactura.Text = string.Format("{00:0.00}", _total);
                }
            }
        }

        //Funcion Agregar detalle Factura, con validación y demás

        public void AgregarDF(Producto codiProd, int cantidadProd)
        {
            // verifica  si existe un producto en la lista de detalles, si encuentra suma la cantidad y verifica el stock
            if (ExisteProducto(codiProd))
            {
                // verifica el stock
                if (nudCantidadDetallesFAct.Value <= ProducActual.StockProducto)
                {
                    foreach (var busc in ListaDetalleFactura)
                    {
                        // ahora verifica que cod de la lista es igual al del producto a agregar, y le suma la cantidad y calcula nuevamente el subtotal
                        if (busc.Codigo == ProducActual.CodigoProducto)
                        {
                            busc.Cantidad += cantidadProd;
                            busc.SubTotal = busc.Cantidad * busc.Precio;
                            ProducActual.StockProducto = ProducActual.StockProducto - cantidadProd;
                            FacturaAoB();
                        }
                        cargarGrillaFact();
                    }
                }
                else if (nudCantidadDetallesFAct.Value > ProducActual.StockProducto)
                {
                    MessageBox.Show("No hay Stock del producto");
                }
            }
            else
            // en caso de que el producto sea nuevo en la lista de detalles
            {
                // verifica el stock
                if (nudCantidadDetallesFAct.Value <= ProducActual.StockProducto)
                {
                    DetalleFactura ObjDetalle = new DetalleFactura();

                    ObjDetalle.Codigo = txtCodigoProductoFactura.Text;
                    ObjDetalle.Descripcion = txtDescripcionProductoFactura.Text;
                    ObjDetalle.Precio = nudPrecioProducto.Value;
                    ObjDetalle.Cantidad = Convert.ToInt32(nudCantidadDetallesFAct.Value);
                    ObjDetalle.SubTotal = ObjDetalle.Precio * ObjDetalle.Cantidad;
                    ProducActual.StockProducto = ProducActual.StockProducto - Convert.ToInt32(nudCantidadDetallesFAct.Value);
                    ListaDetalleFactura.Add(ObjDetalle);
                    cargarGrillaFact();
                    cargargrillaproducto();
                }
                else if (nudCantidadDetallesFAct.Value > ProducActual.StockProducto)
                { MessageBox.Show("No hay Stock del producto"); }
            }
        }
        //btnGuardarFactura: aumenta en 1 el número de factura a medida que se lo presiona, chekea que el tipo 
        //de factura sea, asi lo agrega a los datos de la factura.
        //
        private void btnGuardarFactura_Click(object sender, EventArgs e)
        {
            DateTime fecha = DateTime.Now;
            txtFechaFactura.Text = fecha.ToString();
            if (ckbFacturaA.Checked == false && ckbFacturaB.Checked == false)
            {
                MessageBox.Show("Debe legir un tipo de factura antes de guardar la factura");
            }
            else
            {
                NumFactura += 1;
                txtNroFactura.Text = NumFactura.ToString();
                string _tipofactura = null;

                if (ckbFacturaA.Checked == true)
                {
                    _tipofactura = "A";
                }
                else if (ckbFacturaB.Checked == true)
                {
                    _tipofactura = "B";
                }

                FacturaNueva = new Factura();

                string EmpleadoCodigo = null;
                EmpleadoActualApellidoyNombre = null;
                EmpleadoActualCodigo = null;

                FacturaNueva.Codigo = txtNroFactura.Text;
                EmpleadoCodigo = Convert.ToString(this.cmbEmpleadoFactura.SelectedValue);
                ObtenerEmpleado(EmpleadoCodigo);
                FacturaNueva.EmpleadoCodigo = EmpleadoActualCodigo;
                FacturaNueva.NumeroFactura = txtNroFactura.Text;
                FacturaNueva.ApellidoyNombre = EmpleadoActualApellidoyNombre;
                FacturaNueva.DNI = EmpleadoActualDNI;
                FacturaNueva.Fecha = Convert.ToDateTime(txtFechaFactura.Text);
                FacturaNueva.TipoFactura = _tipofactura;
                FacturaNueva.SubTotal = Convert.ToDecimal(txtSubTotalFactura.Text);
                FacturaNueva.Iva = Convert.ToDecimal(txtIvaFact.Text);
                FacturaNueva.Descuento = nudDescuentoFactura.Value;
                FacturaNueva.Total = Convert.ToDecimal(txtTOTALFactura.Text);
                FacturaNueva.FormaDePago = "Efectivo";

                ListaFactura.Add(FacturaNueva);
                ListaDetalleFactura.Clear();
                cargarGrillaFact();
                txtSubTotalFactura.Clear();
                txtTOTALFactura.Clear();
                txtIvaFact.Clear();
                nudImporteFact.Value = 0;

                txtVueltoFAct.Clear();
                ExiteProductosListaFact();
                nudDescuentoFactura.Value = 0;
                cargarGrillaVentas();

                //para que el boton agregar no se active cuando esta activado el modo rapido
                if (checkBox1.Checked == true)
                {
                    btnAgregarfactura.Enabled = false;
                }
                
                else 
                {
                    btnAgregarfactura.Enabled = true;
                }
                nudImporteFact.Enabled = false;
                btnGuardarFactura.Enabled = false;
                btnCtaCteFactura.Enabled = false;
            }
        }
        private void btnCtaCteFactura_Click(object sender, EventArgs e)
        {
            DateTime fecha = DateTime.Now;
            txtFechaFactura.Text = fecha.ToString();
            if (ckbFacturaA.Checked == false && ckbFacturaB.Checked == false)
            {
                MessageBox.Show("Debe elegir un tipo de factura antes de guardar la factura");
            }
            else
            {
                string codigClit = null;
                codigClit = Convert.ToString(cmbClienteFactura.SelectedValue);


                if (ExisteDeuda(codigClit))
                {
                    MessageBox.Show("El cliente actual posee una deuda, debe regularizar su situacion.");
                }
                else
                {
                    NumFactura += 1;

                    txtNroFactura.Text = NumFactura.ToString();
                    string _tipoFact = null;

                    if (ckbFacturaA.Checked == true)
                    {
                        _tipoFact = "A";
                    }
                    else if (ckbFacturaB.Checked == true)
                    {
                        _tipoFact = "B";
                    }
                    FacturaNueva = new Factura();
                    CtaCorrienteNueva = new CtaCorriente();

                    string codEmpleado = null;
                    EmpleadoActualApellidoyNombre = null;
                    EmpleadoActualCodigo = null;
                    //Venta
                    FacturaNueva.Codigo = txtNroFactura.Text;
                    codEmpleado = Convert.ToString(this.cmbEmpleadoFactura.SelectedValue);
                    ObtenerEmpleado(codEmpleado);
                    FacturaNueva.EmpleadoCodigo = EmpleadoActualCodigo;
                    FacturaNueva.NumeroFactura = txtNroFactura.Text;
                    FacturaNueva.ApellidoyNombre = EmpleadoActualApellidoyNombre;
                    FacturaNueva.DNI = EmpleadoActualDNI;
                    FacturaNueva.Fecha = Convert.ToDateTime(txtFechaFactura.Text);
                    FacturaNueva.TipoFactura = _tipoFact;
                    FacturaNueva.FormaDePago = "Cta Corriente";
                    FacturaNueva.SubTotal = Convert.ToDecimal(txtSubTotalFactura.Text);
                    FacturaNueva.Iva = Convert.ToDecimal(txtIvaFact.Text);
                    FacturaNueva.Descuento = nudDescuentoFactura.Value;
                    FacturaNueva.Total = Convert.ToDecimal(txtTOTALFactura.Text);

                    //CTaCte
                    codigClit = Convert.ToString(this.cmbClienteFactura.SelectedValue);
                    CtaCorrienteNueva.Codigo = txtNroFactura.Text;
                    codigClit = Convert.ToString(this.cmbClienteFactura.SelectedValue);
                    CtaCorrienteNueva.Codigoclit = codigClit;
                    obtenercliente(codigClit);
                    CtaCorrienteNueva.NumFactura = txtNroFactura.Text;
                    CtaCorrienteNueva.EstadoFactura = "Pendiente";
                    CtaCorrienteNueva.ApellidoyNombre = ClienteActualApellido;
                    CtaCorrienteNueva.Cuil = ClienteActualCuil;
                    CtaCorrienteNueva.Fecha = Convert.ToDateTime(txtFechaFactura.Text);
                    CtaCorrienteNueva.TipoFactura = _tipoFact;
                    CtaCorrienteNueva.SubTotal = Convert.ToDecimal(txtSubTotalFactura.Text);
                    CtaCorrienteNueva.Iva = Convert.ToDecimal(txtIvaFact.Text);
                    CtaCorrienteNueva.Descuento = nudDescuentoFactura.Value;
                    CtaCorrienteNueva.Total = Convert.ToDecimal(txtTOTALFactura.Text);

                    ListaFactura.Add(FacturaNueva);
                    ListaCtaCorriente.Add(CtaCorrienteNueva);
                    ListaDetalleFactura.Clear();
                    cargarGrillaFact();
                    cargarGrillaCtaCte();
                    txtSubTotalFactura.Clear();
                    txtTOTALFactura.Clear();
                    txtIvaFact.Clear();
                    nudImporteFact.Value = 0;
                    txtVueltoFAct.Clear();
                    ExiteProductosListaFact();
                    nudDescuentoFactura.Value = 0;
                    cargarGrillaVentas();

                    // para el que boton agregar noc active cuando esta activado el modo rapido
                    if (checkBox1.Checked == true)
                    {
                        btnAgregarfactura.Enabled = false;
                        btnCtaCteFactura.Enabled = false;
                    }
                    else { btnAgregarfactura.Enabled = true; }

                    nudImporteFact.Enabled = false;
                    btnGuardarFactura.Enabled = false;
                    btnCtaCteFactura.Enabled = false;
                }
            }
        }

        //funcion btnSalirFactura

        private void btnSalirFactura_Click(object sender, EventArgs e)
        {
            this.Close();
        }  

        //Valida la no existencia de codigo, DNI, celular 
        private bool ExisteDatosEmpleado(string cod, string DNI, string celular, string telefono, string email)
        {
            foreach (var item in ListaEmpleado)
            {
                if (item.EmpleadoCodigo == cod || item.EmpleadoCelular == celular || item.EmpleadoDNI == DNI || item.EmpleadoTelefono == telefono || item.EmpleadoEmail == email)
                {
                    return true;
                }

            }
            return false;
        }
        // función para obtener un empleado asi colocar su apellido en la factura
        private void ObtenerEmpleado(string _codigoEmple)
        {
            foreach (var item in ListaEmpleado)
            {
                if (item.EmpleadoCodigo.ToString() == Convert.ToString(_codigoEmple))
                {
                    EmpleadoActualApellidoyNombre = item.EmpleadoApellidoyNombre;
                    EmpleadoActualCodigo = item.EmpleadoCodigo;
                    EmpleadoActualDNI = item.EmpleadoDNI;
                    break;
                }
            }
        }

        //btn Agregar Empleado
        private void btnAgregarEmpleado_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoEmpleado.Text) || string.IsNullOrEmpty(txtNombreEmpleado.Text) ||
                           string.IsNullOrEmpty(txtApellidoEmpleado.Text) || string.IsNullOrEmpty(txtDNIEmpleado.Text) ||
                           string.IsNullOrEmpty(txtEmailEmpleado.Text) || string.IsNullOrEmpty(txtCelularEmpleado.Text) ||
                           string.IsNullOrEmpty(txtTelefonoEmpleado.Text) || nudSueldoEmpleado.Value <= 0)
            {
                MessageBox.Show("Complete todos los campos!");
            }
            else
            {
                if (ExisteDatosEmpleado(txtCodigoEmpleado.Text, txtDNIEmpleado.Text, txtCelularEmpleado.Text, txtTelefonoEmpleado.Text, txtEmailCliente.Text))
                {
                    MessageBox.Show("Algunos datos ya han sido utilizados");
                }
                else
                {

                    Empleado empleadoObj = new Empleado();

                    empleadoObj.EmpleadoId = contadorEmpleadoId;
                    empleadoObj.EmpleadoCodigo = txtCodigoEmpleado.Text;
                    empleadoObj.EmpleadoApellido = txtApellidoEmpleado.Text;
                    empleadoObj.EmpleadoNombre = txtNombreEmpleado.Text;
                    empleadoObj.EmpleadoDNI = txtDNIEmpleado.Text;
                    empleadoObj.EmpleadoEmail = txtEmailEmpleado.Text;
                    empleadoObj.EmpleadoTelefono = txtTelefonoEmpleado.Text;
                    empleadoObj.EmpleadoCelular = txtCelularEmpleado.Text;
                    empleadoObj.EmpleadoSueldo = nudSueldoEmpleado.Value;

                    ListaEmpleado.Add(empleadoObj);
                    cargargrillaempleado();
                    CargarComboEmpleado();
                    contadorEmpleadoId++;
                    limpiarEmpleado();
                }
            }
        }

        //funcion borrar empleado

        private void btnBorrarEmpleado_Click(object sender, EventArgs e)
        {
            limpiarEmpleado();
        }
        // btn Salir empleado

        private void btnSalirEmpleado_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtCodigoEmpleado_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            { 
                //el resto de teclas pulsadas se desactivan
                e.Handled = true;
            }
        }

        private void txtApellidoEmpleado_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            else
                if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = false;
                } 
        }
        private void txtNombreEmpleado_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            else
                if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = false;
                }
        }

        private void txtDNIEmpleado_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            
                if (char.IsControl(e.KeyChar))//permitir teclas de control como retroceso
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            
        }
        private void txtTelefonoEmpleado_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void txtCelularEmpleado_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
                if (char.IsControl(e.KeyChar))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
        }
       
        // Funcion para actualizar las listas en las grillas
        public void cargargrillaproducto()
        {
            dgvListaProducto.DataSource = ListaProducto.ToList();
            formatearGrillaProducto();
        }

        public void cargargrillaempleado()
        {
            dgvEmpleado.DataSource = ListaEmpleado.ToList();
            formatearGrillaEmpleado();
        }

        private void cargarGrillaVentaEmpleado()
        {
            dgvListaVentaEmpleado.DataSource = ListaFacturaEmpleado.ToList();
            formatearGrillaVentaEmpleado();
        }

        public void cargargrillaCliente()
        {
            dvgListaCliente.DataSource = ListaCliente.ToList();
            formatearGrillaCliente();
        }

        public void cargarGrillaFact()
        {
            dgvGrillaFactura.DataSource = ListaDetalleFactura.ToList();
            formatearGrillaFact();
        }

        public void cargarGrillaVentas()
        {
            dvgVentas.DataSource = ListarVentas(ref ListaFactura, this.dtpFechaDesdeVentas.Value, this.dtpFechaHastaVentas.Value);
            formatearGrillaVentas();
        }

        public void CargarGrillaCtacteClient()
        {
            dgvCtaCteCliente.DataSource = ListaClienteCtaCorriente.ToList();
            formatearGrillaCtaCteCliente();
        }
        public void cargarGrillaCtaCte()
        {
            ListaCtaFiltro.Clear();
            ListarCtas(ref ListaCtaCorriente, this.dtpFechaDesdeCta.Value, this.dtpFechaHastaCta.Value);
            dgvCtaCorriente.DataSource = ListaCtaFiltro.ToList();

            formatearGrillaCtaCorriente();
        }

        //funcion para formatear las grillas
        public void formatearGrillaProducto()
        {
            this.dgvListaProducto.Columns["IdProducto"].Visible = false;
            this.dgvListaProducto.Columns["CodigoProducto"].HeaderText = "Codigo";
            this.dgvListaProducto.Columns["CodigoProducto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvListaProducto.Columns["CodigoBarraProducto"].HeaderText = "Codigo de barra";
            this.dgvListaProducto.Columns["CodigoBarraProducto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvListaProducto.Columns["DescripcionProducto"].HeaderText = "Descripcion";
            this.dgvListaProducto.Columns["DescripcionProducto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvListaProducto.Columns["PrecCostoProducto"].HeaderText = "Precio Costo";
            this.dgvListaProducto.Columns["PrecCostoProducto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvListaProducto.Columns["PrecPublicoProducto"].HeaderText = "Precio Publico";
            this.dgvListaProducto.Columns["PrecPublicoProducto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvListaProducto.Columns["StockProducto"].HeaderText = "Stock";
            this.dgvListaProducto.Columns["StockProducto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        public void formatearGrillaEmpleado()
        {
            this.dgvEmpleado.Columns["EmpleadoId"].Visible = false;
            this.dgvEmpleado.Columns["EmpleadoANyDNI"].Visible = false;
            this.dgvEmpleado.Columns["EmpleadoApellidoyNombre"].Visible = false;
            this.dgvEmpleado.Columns["EmpleadoCodigo"].HeaderText = "Codigo";
            this.dgvEmpleado.Columns["EmpleadoCodigo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvEmpleado.Columns["EmpleadoApellido"].HeaderText = "Apellido";
            this.dgvEmpleado.Columns["EmpleadoNombre"].HeaderText = "Nombre";
            this.dgvEmpleado.Columns["EmpleadoDNI"].HeaderText = "DNI";
            this.dgvEmpleado.Columns["EmpleadoTelefono"].HeaderText = "Telefono";
            this.dgvEmpleado.Columns["EmpleadoEmail"].HeaderText = "Email";
            this.dgvEmpleado.Columns["EmpleadoCelular"].HeaderText = "Celular";
            this.dgvEmpleado.Columns["EmpleadoSueldo"].HeaderText = "Sueldo";
            this.dgvEmpleado.Columns["EmpleadoApellido"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvEmpleado.Columns["EmpleadoNombre"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvEmpleado.Columns["EmpleadoDNI"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvEmpleado.Columns["EmpleadoTelefono"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvEmpleado.Columns["EmpleadoEmail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvEmpleado.Columns["EmpleadoCelular"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvEmpleado.Columns["EmpleadoSueldo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        public void formatearGrillaVentaEmpleado()
        {
            this.dgvListaVentaEmpleado.Columns["FacturaId"].Visible = false;
            this.dgvListaVentaEmpleado.Columns["ApellidoyNombre"].HeaderText = "Apellido y Nombre";
            this.dgvListaVentaEmpleado.Columns["EmpleadoCodigo"].Visible = false;
            this.dgvListaVentaEmpleado.Columns["Codigo"].Visible = false;
            this.dgvListaVentaEmpleado.Columns["Fecha"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvListaVentaEmpleado.Columns["NumeroFactura"].Width = 60;
            this.dgvListaVentaEmpleado.Columns["NumeroFactura"].HeaderText = "NºFactura";
            this.dgvListaVentaEmpleado.Columns["TipoFactura"].HeaderText = "Tipo";
            this.dgvListaVentaEmpleado.Columns["TipoFactura"].Width = 35;
            this.dgvListaVentaEmpleado.Columns["FormaDePago"].HeaderText = "Forma de Pago";
            this.dgvListaVentaEmpleado.Columns["FormaDePago"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvListaVentaEmpleado.Columns["Total"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvListaVentaEmpleado.Columns["Iva"].Width = 40;
            this.dgvListaVentaEmpleado.Columns["SubTotal"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvListaVentaEmpleado.Columns["Descuento"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        public void formatearGrillaCliente()
        {
            this.dvgListaCliente.Columns["ClienteId"].Visible = false;
            this.dvgListaCliente.Columns["ClienteApellidoyNombre"].Visible = false;
            this.dvgListaCliente.Columns["ClienteANyCuit"].Visible = false;
            this.dvgListaCliente.Columns["ClienteCodigo"].HeaderText = "Codigo";
            this.dvgListaCliente.Columns["ClienteApellido"].HeaderText = "Apellido";
            this.dvgListaCliente.Columns["ClienteNombre"].HeaderText = "Nombre";
            this.dvgListaCliente.Columns["ClienteCuit"].HeaderText = "Cuit";
            this.dvgListaCliente.Columns["ClienteTelefono"].HeaderText = "Telefono";
            this.dvgListaCliente.Columns["ClienteEmail"].HeaderText = "Email";
            this.dvgListaCliente.Columns["ClienteCelular"].HeaderText = "Celular";
            this.dvgListaCliente.Columns["ClienteCodigo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dvgListaCliente.Columns["ClienteApellido"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dvgListaCliente.Columns["ClienteNombre"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dvgListaCliente.Columns["ClienteCuit"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dvgListaCliente.Columns["ClienteTelefono"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dvgListaCliente.Columns["ClienteEmail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dvgListaCliente.Columns["ClienteCelular"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dvgListaCliente.Columns["Haber"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        public void formatearGrillaFact()
        {
            this.dgvGrillaFactura.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        public void formatearGrillaVentas()
        {
            this.dvgVentas.Columns["FacturaId"].Visible = false;
            this.dvgVentas.Columns["ApellidoYNombre"].HeaderText = "Apellido y Nombre";
            this.dvgVentas.Columns["EmpleadoCodigo"].Visible = false;
            this.dvgVentas.Columns["Codigo"].Visible = false;
            this.dvgVentas.Columns["Fecha"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dvgVentas.Columns["NumeroFactura"].Width = 60;
            this.dvgVentas.Columns["TipoFactura"].HeaderText = "Tipo";
            this.dvgVentas.Columns["TipoFactura"].Width = 35;
            this.dvgVentas.Columns["FormaDePago"].HeaderText = "Forma de Pago";
            this.dvgVentas.Columns["FormaDePago"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dvgVentas.Columns["Total"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dvgVentas.Columns["Iva"].Width = 40;
            this.dvgVentas.Columns["SubTotal"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dvgVentas.Columns["Descuento"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        public void formatearGrillaCtaCteCliente()
        {
            this.dgvCtaCteCliente.Columns["CtaID"].Visible = false;
            this.dgvCtaCteCliente.Columns["ApellidoYNombre"].HeaderText = "Apellido y Nombre";
            this.dgvCtaCteCliente.Columns["NumFactura"].HeaderText = "NºFactura";
            this.dgvCtaCteCliente.Columns["TipoFactura"].HeaderText = "Tipo";
            this.dgvCtaCteCliente.Columns["EstadoFactura"].HeaderText = "Estado";
            this.dgvCtaCteCliente.Columns["Codigo"].Visible = false;
            this.dgvCtaCteCliente.Columns["Codigoclit"].Visible = false;
            this.dgvCtaCteCliente.Columns["Haber"].Visible = false;
            this.dgvCtaCteCliente.Columns["Debe"].Visible = false;
            this.dgvCtaCteCliente.Columns["NumFactura"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCtaCteCliente.Columns["ApellidoYnombre"].Width = 85;
            this.dgvCtaCteCliente.Columns["TipoFactura"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCtaCteCliente.Columns["Debe"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCtaCteCliente.Columns["SubTotal"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCtaCteCliente.Columns["Iva"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCtaCteCliente.Columns["Descuento"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCtaCteCliente.Columns["Total"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCtaCteCliente.Columns["EstadoFactura"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        
        }
        public void formatearGrillaCtaCorriente()
        {
            this.dgvCtaCorriente.Columns["CtaID"].Visible = false;
            this.dgvCtaCorriente.Columns["ApellidoyNombre"].HeaderText = "Apellido y Nombre";
            this.dgvCtaCorriente.Columns["NumFactura"].HeaderText = "NºFactura";
            this.dgvCtaCorriente.Columns["TipoFactura"].HeaderText = "Tipo";
            this.dgvCtaCorriente.Columns["EstadoFactura"].HeaderText = "Estado";
            this.dgvCtaCorriente.Columns["Codigo"].Visible = false;
            this.dgvCtaCorriente.Columns["Haber"].Visible = false;
            this.dgvCtaCorriente.Columns["Debe"].Visible = false;
            this.dgvCtaCorriente.Columns["Codigoclit"].Visible = false;
            this.dgvCtaCorriente.Columns["NumFactura"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCtaCorriente.Columns["ApellidoyNombre"].Width = 85;
            this.dgvCtaCorriente.Columns["TipoFactura"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCtaCorriente.Columns["Debe"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCtaCorriente.Columns["SubTotal"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCtaCorriente.Columns["Iva"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCtaCorriente.Columns["Descuento"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCtaCorriente.Columns["Total"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCtaCorriente.Columns["EstadoFactura"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        
        
        //validacion para q no se ingresen codigo o codig de barra existentes
        private bool ExisteProdValidacion(string codProd, string codBarra)
        {

            foreach (var item in ListaProducto)
            {
                if (item.CodigoProducto == codProd || item.CodigoBarraProducto == codBarra)
                {
                    return true;
                }
            }
            return false;
        }
        
        // funcion Buscar producto
        public Producto BuscarProd(string buscar)
        {
            Producto producto = new Producto();
            // el forich buscar en la lista de producto el dato que le pasamos por "buscar"
            foreach (var prod in ListaProducto)
            {
                if (Convert.ToString(prod.CodigoProducto) == buscar ||
                    Convert.ToString(prod.CodigoBarraProducto) == buscar ||
                    prod.DescripcionProducto == buscar)
                {
                    producto = prod;
                    break;
                }
            }
            return producto;
        }
        private void btnBorrarProducto_Click(object sender, EventArgs e)
        {
            limpiarProducto();
        }

        // funcion para cargar los comboBox
        public void CargarComboEmpleado()
        {

            cmbEmpleadoFactura.DataSource = ListaEmpleado.ToList();
            cmbEmpleadoFactura.DisplayMember = "EmpleadoANyDNI";
            cmbEmpleadoFactura.ValueMember = "EmpleadoCodigo";
            cmbEmpleadoVenta.DataSource = ListaEmpleado.ToList();
            cmbEmpleadoVenta.DisplayMember = "EmpleadoANyDNI";
            cmbEmpleadoVenta.ValueMember = "EmpleadoCodigo";
        }
        public void CargarComboCliente()
        {

            cmbClienteFactura.DataSource = ListaCliente.ToList();
            cmbClienteFactura.DisplayMember = "ClienteANyCuit";
            cmbClienteFactura.ValueMember = "ClienteCodigo";
            cmbClienteCta.DataSource = ListaCliente.ToList();
            cmbClienteCta.DisplayMember = "ClienteANyCuit";
            cmbClienteCta.ValueMember = "ClienteCodigo";
        }

        //-------------------------------------------------------------------
        //Funcion para limpiar los campos
        public void limpiarProducto()
        {
            this.txtCodigoProducto.Clear();
            this.txtCodBarraProducto.Clear();
            this.txtDescripciónProducto.Clear();
            nudPrecPublicoProducto.Value = 0;
            nudPrecCostoProducto.Value = 0;
            nudStockProducto.Value = 0;
        }
        public void limpiarEmpleado()
        {
            this.txtCodigoEmpleado.Clear();
            this.txtNombreEmpleado.Clear();
            this.txtApellidoEmpleado.Clear();
            this.txtDNIEmpleado.Clear();
            this.txtTelefonoEmpleado.Clear();
            this.txtCelularEmpleado.Clear();
            this.txtEmailEmpleado.Clear();
            nudSueldoEmpleado.Value = 0;
        }
        public void limpiarCliente()
        {
            this.txtCodigoCliente.Clear();
            this.txtApellidoCliente.Clear();
            this.txtNombreCliente.Clear();
            this.txtCuitCliente.Clear();
            this.txtTelefonoCliente.Clear();
            this.txtEmailCliente.Clear();
            this.txtCelularCliente.Clear();
            nudHaberCliente.Value = 0;
        }
        //función para agregar un cliente a la lista
        private void btnAgregarCliente_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCelularCliente.Text) || string.IsNullOrEmpty(txtCodigoCliente.Text) || string.IsNullOrEmpty(txtCuitCliente.Text)
                || string.IsNullOrEmpty(txtEmailCliente.Text) || string.IsNullOrEmpty(txtApellidoCliente.Text) || string.IsNullOrEmpty(txtNombreCliente.Text)
                || string.IsNullOrEmpty(txtTelefonoCliente.Text) || nudHaberCliente.Value <= 0)
            {
                MessageBox.Show("Complete todos los campos!");
            }
            else
            {
                if (existenDatosCliente(txtCodigoCliente.Text, txtCuitCliente.Text, txtCelularCliente.Text, txtEmailCliente.Text, txtTelefonoCliente.Text))
                {
                    MessageBox.Show("Algunos datos ya han sido utilizados");

                }
                else
                {
                    Cliente clientObjt = new Cliente();

                    clientObjt.ClienteId = contadorClienteId;
                    clientObjt.ClienteCodigo = txtCodigoCliente.Text;
                    clientObjt.ClienteApellido = txtApellidoCliente.Text;
                    clientObjt.ClienteNombre = txtNombreCliente.Text;
                    clientObjt.ClienteCuit = txtCuitCliente.Text;
                    clientObjt.ClienteEmail = txtEmailCliente.Text;
                    clientObjt.ClienteTelefono = txtTelefonoCliente.Text;
                    clientObjt.ClienteCelular = txtCelularCliente.Text;
                    clientObjt.Haber = nudHaberCliente.Value;

                    ListaCliente.Add(clientObjt);
                    cargargrillaCliente();
                    CargarComboCliente();
                    contadorClienteId++;
                    limpiarCliente();
                }
            }
        }
        //btn Salir cliente

        private void btnSalirCliente_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnBorrarCliente_Click(object sender, EventArgs e)
        {
            limpiarCliente();
        }

        private bool existenDatosCliente(string cod, string cuil, string celular, string email, string telefono)
        {
            foreach (var item in ListaCliente)
            {
                if (item.ClienteCodigo == cod || item.ClienteCuit == cuil || item.ClienteCelular == celular || item.ClienteEmail == email || item.ClienteTelefono == telefono)
                {
                    return true;
                }
            }
            return false;
        }

        private void txtCodigoCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
                if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                } 
        }
        private void txtApellidoCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            else
                if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = false;
                }
        }

        private void txtNombreCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            else
                if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = false;
                }
        }

        private void txtCuitCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
                if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                } 
        }

        private void txtTelefonoCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
                if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                } 
        }
        private void txtCelularCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
                if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                } 
        }
        private bool ExisteDeuda(string codClit)
        {
            foreach (var item in ListaCliente)
            {
                if (item.ClienteCodigo == codClit)
                {
                    if (item.Deuda > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        
        //funcion para ver si hay productos en la lista Detalles
        public bool ExisteProducto(Producto factura)
        {
            foreach (var buscar in ListaDetalleFactura)
            {
                if (buscar.Codigo == factura.CodigoProducto)
                {
                    return true;
                }
              
            }
            return false;
        }
        // Evento del checkA estando en true

        bool FactA = true;
        private void ckbFacturaA_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbFacturaA.Checked == true)
            {
                ckbFacturaB.Enabled = false;
                FactA = true;
                FacturaAoB();

            }
            else if (ckbFacturaA.Checked == false)
            {
                FactA = false;
                txtSubTotalFactura.Clear();
                txtTOTALFactura.Clear();
                txtIvaFact.Clear();
                nudDescuentoFactura.Value = 0;
                ckbFacturaB.Enabled = true;
            }
        }

        // evento que se produce cuando el checkB true
        bool FactB = false;
        private void ckbFacturaB_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbFacturaB.Checked == true)
            {
                ckbFacturaA.Enabled = false;
                txtIvaFact.Clear();
                FactB = true;
                FacturaAoB();
            }
            else
                if (ckbFacturaB.Checked == false)
                {
                    FactB = false;
                    txtSubTotalFactura.Clear();
                    txtTOTALFactura.Clear();
                    txtIvaFact.Clear();
                    nudDescuentoFactura.Value = 0;
                    ckbFacturaA.Enabled = true;
                }
        }
        //funcion para poder calcular la factura dependiendo del tipo de factura ya sea AoB
        private void FacturaAoB()
        {
            if (FactA == true)
            {
                ckbFacturaB.Enabled = false;
                CalculaTotalFact();
                CalcularIvaDF();
                CalcularTotalDF();
            }
            else if (FactB == true)
            {
                ckbFacturaA.Enabled = false;
                txtIvaFact.Text = "0";
                CalculaTotalFact();
                CalcularTotalDF();
            }
        }
        // cancela todo y devuelve el stock a la lista de productos
        private void btnCancelarFactura_Click(object sender, EventArgs e)
        {
            //lo mas complejo que se hizo en el primer foreach va de fila en fila
            //y dentro de este hay otro foreach que se fija que el codigo sean iguales
            // si es asi, le devuelve la cantidad de stock que tenia detalles factura
            foreach (var itemDetalle in ListaDetalleFactura)
            {
                foreach (var itemProducto in ListaProducto)
                {
                    if (itemDetalle.Codigo == itemProducto.CodigoProducto)
                    {
                        itemProducto.StockProducto += itemDetalle.Cantidad;
                    }
                }
            }

            txtSubTotalFactura.Text = "0";
            txtTOTALFactura.Text = "0";
            txtIvaFact.Text = "0";
            nudImporteFact.Value = 0;
            txtVueltoFAct.Text = "0";
            ListaDetalleFactura.Clear();
            // pequeño if para controlar que no activar el boton de agregar cuando el cheked de modo rapido esta activado 
            // al precionar el boton cancelar, caso que no es correcto
            if (checkBox1.Checked == true)
            {
                btnAgregarfactura.Enabled = false;
            }
            else { btnAgregarfactura.Enabled = true; }

            cargargrillaproducto();
            cargarGrillaFact();
        }

        //evento que se produce cuando cambiamos el valor del nud con los botones
        private void nudDescuentoFactura_ValueChanged(object sender, EventArgs e)
        {
            FacturaAoB();
        }

        
        //-------------------------------------------------------------------------------
        //evento keypress para calcular el vuelto
        private void nudImporteFact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                decimal _total = Convert.ToDecimal(txtTOTALFactura.Text);

                decimal _importe = Convert.ToDecimal(nudImporteFact.Value);
                btnAgregarfactura.Enabled = false;
                if (_importe >= _total)
                {
                    decimal _vuelto = _total - _importe;
                    // para que no sea negativo el vuelto
                    _vuelto = -(_vuelto);
                    txtVueltoFAct.Text = _vuelto.ToString();
                    nudImporteFact.Enabled = false;
                }
                else
                {
                    MessageBox.Show("$$ ingresado es insuficiente");
                }
            }
        }

        //funcion para ver si existe un producto en la lista de factura
        private void ExiteProductosListaFact()
        {
            string _total = txtTOTALFactura.Text;
            if (_total == "0,00")
            {
                nudImporteFact.Enabled = false;
            }
            else
            {
                nudImporteFact.Enabled = true;
            }
        }

        //lista la grilla ventas con la condicion q las facturass de la lista coinsidan con la de la fecha marcada
        private object ListarVentas(ref List<Factura> _facturas, DateTime _fechaDesde, DateTime _fechaHasta)
        {
            List<Factura> ventas = new List<Factura>();

            foreach (var item in _facturas)
            {
                //recorre la lista de facturas para buscar la que 
                //coinciden con el id del empleado y con las fechas marcadas
                if ((item.Fecha.Date >= _fechaDesde.Date
                    && item.Fecha.Date <= _fechaHasta.Date))
                {
                    ventas.Add(item);
                }
            }
            return ventas;
        }

        private void ListarCtas(ref List<CtaCorriente> _facturas, DateTime _fechaDesde, DateTime _fechaHasta)
        {
            //List<CtaCorriente> CtaCte = new List<CtaCorriente>();

            foreach (var item in _facturas)
            {
                //recorre la lista de facturas para buscar la que 
                //coinciden con el id del empleado y con las fechas marcadas
                if ((item.Fecha.Date >= _fechaDesde.Date
                    && item.Fecha.Date <= _fechaHasta.Date))
                {
                    ListaCtaFiltro.Add(item);
                    //this.dgvCtaCorriente.DataSource = CtaCte.ToList();
                }
            }
        }

        private void dtpFechaDesde_ValueChanged(object sender, EventArgs e)
        {
            //Carga las ventas realizadas
            this.dtpFechaHastaVentas.MinDate = this.dtpFechaDesdeVentas.Value;
            this.dtpFechaHastaVentas.Value = this.dtpFechaDesdeVentas.Value;
            cargarGrillaVentas();

            //cargar las ventas de los empleados (lo mismo del evento change de cmb)
            ListaFacturaEmpleado.Clear();
            string codigoEmpleado = "";
            codigoEmpleado = Convert.ToString(cmbEmpleadoVenta.SelectedValue);
            obtenerEmpleadoVentas(codigoEmpleado, this.dtpFechaDesdeVentas.Value, dtpFechaHastaVentas.Value);
            cargarGrillaVentaEmpleado();
        }
        private void dtpFechaHastaVentas_ValueChanged_1(object sender, EventArgs e)
        {
            //Carga las ventas realizadas
            this.dtpFechaHastaVentas.MinDate = this.dtpFechaDesdeVentas.Value;
            cargarGrillaVentas();

            //cargar las ventas de los empleados (lo mismo del evento change de cmb)
            ListaFacturaEmpleado.Clear();
            string codigoEmpleado = "";
            codigoEmpleado = Convert.ToString(cmbEmpleadoVenta.SelectedValue);
            obtenerEmpleadoVentas(codigoEmpleado, this.dtpFechaDesdeVentas.Value, dtpFechaHastaVentas.Value);
            cargarGrillaVentaEmpleado();
        }
        //-----------------------------------------------------------------------------------
        //este vento se produce cuando se hace clic en el cobox de ventas
        private void cmbEmpleadoVentas_TextChanged(object sender, EventArgs e)
        {
            cargarGrillaVentaEmpleado();
            ListaFacturaEmpleado.Clear();
        }
        //---------------------------------------------------------------------------------
        //obtiene las ventas del empleado elegido
        private void obtenerEmpleadoVentas(string EmpleadoCodigo, DateTime fechaDesde, DateTime fechaHasta)
        {
            //Factura VentaEmpleado = new Factura();
            foreach (var item in ListaFactura)
            {
                if (EmpleadoCodigo == item.EmpleadoCodigo && item.Fecha.Date >= fechaDesde.Date
                    && item.Fecha.Date <= fechaHasta.Date)
                {
                    ListaFacturaEmpleado.Add(item);
                    cargarGrillaVentaEmpleado();
                }
            }
        }
        //--------------------------------------------------------------------------------
        //el evento que ocurre cuando se hace clic en el combox de ventas y selecionamos un empleado
        private void cmbEmpleadoVentas_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListaFacturaEmpleado.Clear();
            string EmpleadoCodigo = "";
            EmpleadoCodigo = Convert.ToString(cmbEmpleadoVenta.SelectedValue);
            obtenerEmpleadoVentas(EmpleadoCodigo, this.dtpFechaDesdeVentas.Value, dtpFechaHastaVentas.Value);
            cargarGrillaVentaEmpleado();
        }
        //-----------------------------------------------------------------------------------
        //Calcular _Comicion
        private void CalcularComicion()
        {
            decimal _TotalVentaFactura = 0;
            ComisionActual = 0;

            foreach (var item in ListaFacturaEmpleado)
            {
                _TotalVentaFactura += item.Total;
            }
            ComisionActual = (_TotalVentaFactura * nudComicionVenta.Value) / 100;
        }
        //-----------------------------------------------------------------------------------
        //Obtiene el sueldo del empleado
        private void ObtenerSueldo()
        {
            // se obtiene el codigo del empelado del del cmbempleadoVenta yy se lo pasa al foreach para que compare si a que empleado es igual
            // y cuando lo obitne usa el saldoAtual para guardar hay el saldo del empleado encontrado.
            string EmpleadoCodigo = "";
            EmpleadoCodigo = Convert.ToString(cmbEmpleadoVenta.SelectedValue);
            SueldoEmpleadoActual = 0;
            foreach (var item in ListaEmpleado)
            {
                if (item.EmpleadoCodigo == EmpleadoCodigo)
                {
                    SueldoEmpleadoActual = item.EmpleadoSueldo;
                    break;
                }
            }
        }
        //-----------------------------------------------------------------------------------
        //Hay o no premio. Suma todo los total de las facturas y si supero al valor establecido el premio es true
        private void PremioSioNo()
        {
            Premio = false;
            decimal TotalVendido = 0;
            // foreach se usa para sumar los totales de las facturas en TotalVendido
            foreach (var item in ListaFacturaEmpleado)
            {
                TotalVendido += item.Total;
            }
            // if que pregunta si el TotalVendido es mayor a lo que hay en VentaPremio. si es asi Premio es true y si no es false
            decimal VentaPremio = 0;
            VentaPremio = Convert.ToDecimal(txtVentaPremio.Text);
            if (TotalVendido > VentaPremio)
            {
                Premio = true;
            }
            else { Premio = false; }
            if (Premio)
            {
                txtPremioEmpleado.Text = "SI";
            }
            else
            {
                txtPremioEmpleado.Text = "No";
            }
        }
        //-----------------------------------------------------------------------------------
        //Calcula el sueldo total en base a la comicionde lo vendido, el sueldo y si tiene premio o no
        private void CalcularSueldoTotal()
        {
            if (Premio)
            {
                SueldoTotal = SueldoEmpleadoActual + ComisionActual + nudPremioVenta.Value;
            }
            else { SueldoTotal = SueldoEmpleadoActual + ComisionActual; }
        }
        //---------------------------------------------------------------------------------
        //btn calcular sueldo de los empleados
        private void btnCalcularVentas_Click(object sender, EventArgs e)
        {
            CalcularComision();
            txtComicionVentas.Text = ComisionActual.ToString();
            ObtenerSueldo();
            txtSueldoVentas.Text = SueldoEmpleadoActual.ToString();
            PremioSioNo();
            CalcularSueldoTotal();
            txtSuedoTotalVentas.Text = SueldoTotal.ToString();
        }
        //el evento que ocurre cuando se hace clic en comboBox de ventas y seleccionamos un empleado
        private void cmbEmpleadoVenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListaFacturaEmpleado.Clear();
            string EmpleadoCodigo = "";
            EmpleadoCodigo = Convert.ToString(cmbEmpleadoVenta.SelectedValue);
            obtenerEmpleadoVentas(EmpleadoCodigo, this.dtpFechaDesdeVentas.Value, dtpFechaHastaVentas.Value);
            cargarGrillaVentaEmpleado();
        }
        // este evento se produce cuando se hace clic en el cmbBox de ventas
        private void cmbEmpleadoVenta_TextChanged(object sender, EventArgs e)
        {
            cargarGrillaVentaEmpleado();
            ListaFacturaEmpleado.Clear();
        }
        //Calcular _Comision
        private void CalcularComision()
        {
            decimal _TotalVentaFactura = 0;
            ComisionActual = 0;

            foreach (var item in ListaFacturaEmpleado)
            {
                _TotalVentaFactura += item.Total;
            }
            ComisionActual = (_TotalVentaFactura * nudComicionVenta.Value) / 100;
        }
        //el checkBox del modo rapido
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                ModoRapido = true;
                btnAgregarfactura.Enabled = false;
            }
            if (checkBox1.Checked == false)
            {
                ModoRapido = false;
                btnAgregarfactura.Enabled = true;
            }
        }

        //Calcula las ganancias y los gastos
        private void btnCalcularGanancia_Click(object sender, EventArgs e)
        {
            //los txt en 0 sino salta error 
            txtGanancia.Text = "0";
            txtIngresoBrutoVenta.Text = "0";
            CalcularIngresoBruto();
            CalcularGanancias();
        }
        //Calcula los gaston de en productos
        private void CalcularGastos()
        {
            txtCostoProductoVenta.Clear();
            decimal _total = 0;
            foreach (var item in ListaProducto)
            {
                _total = (item.PrecCostoProducto * item.StockProducto) + _total;
            }
            CostoProductoAux = _total + CostoProductoAux;
            txtCostoProductoVenta.Text = CostoProductoAux.ToString();
        }
        //------------------------------------------------------------------------------------
        //Calcula los gaston de en productos
        private void CalcularIngresoBruto()
        {
            txtIngresoBrutoVenta.Clear();
            decimal _IngresoBruto = 0;
            foreach (var item in ListaFactura)
            {
                _IngresoBruto += item.Total;
            }
            txtIngresoBrutoVenta.Text = _IngresoBruto.ToString();
        }
        //------------------------------------------------------------------------------------
        //Calcula Las ganancias
        private void CalcularGanancias()
        {
            decimal ganancia = 0;
            decimal ingresobruto = 0;
            decimal gasto = 0;
            txtGanancia.Clear();
            ingresobruto = Convert.ToDecimal(txtIngresoBrutoVenta.Text);
            gasto = Convert.ToDecimal(txtCostoProductoVenta.Text);
            ganancia = ingresobruto - gasto;
            txtGanancia.Text = ganancia.ToString();
        }
        //Obtiene la lista de facturas del cliente con cuenta corriente
        private void obtenerFactClient(string ClienteCodigo)
        {
            foreach (var item in ListaCtaFiltro)
            {
                if (item.Codigoclit == ClienteCodigo)
                {
                    ListaClienteCtaCorriente.Add(item);
                    CargarGrillaCtacteClient();
                }
            }
        }
        //funcion para obtener el haber dle cliente seleccionado
        private void obtenerHaber(string ClienteCodigo)
        {
            foreach (var item in ListaCliente)
            {
                if (item.ClienteCodigo == ClienteCodigo)
                {
                    HaberActual = item.Haber;
                    DebeActual = item.Deuda;
                    break;
                }
            }

            txtHaberCtaCte.Text = string.Format("{00:0.00}", HaberActual);
            txtDebeCtaCte.Text = string.Format("{00:0.00}", -(DebeActual));
        }
        private void cmbClienteCta_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDebeCtaCte.Clear();
            txtHaberCtaCte.Clear();
            txtSaldoNuevoCTa.Clear();
            nudDepositoCta.Enabled = true;
            ListaClienteCtaCorriente.Clear();

            string ClienteCodigo = null;
            ClienteCodigo = Convert.ToString(cmbClienteCta.SelectedValue);
            obtenerFactClient(ClienteCodigo);
            ObtenerDeudaClient();
            obtenerDeudaFact();
            obtenerHaber(ClienteCodigo);
            nudDepositoCta.Enabled = true;
            CargarGrillaCtacteClient();
        }
        //Funcion para obtene el nombre y apellido del cliente
        private void obtenercliente (string ClienteCodigo)
        {
        foreach (var item in ListaCliente)
	{
        if (item.ClienteCodigo == ClienteCodigo)
        {
            ClienteActualApellido = item.ClienteApellidoyNombre;
           
            ClienteActualCuil = item.ClienteCuit;
            
            break;
        }

	}
       
        }
        private void ObtenerDeudaClient()
        {
            DeudaFactura = 0;
            foreach (var item in ListaClienteCtaCorriente)
            {
                if (item.EstadoFactura == "Pendiente")
                {
                    DeudaFactura += item.Total;
                }
                else
                {
                    txtSaldoNuevoCTa.Text = string.Format("{00:0.00}", (DeudaFactura));
                }
                txtSaldoNuevoCTa.Text = string.Format("{00:0.00}", (DeudaFactura));

            }
        }

        //----------------------------------------------------------------------------
        //ObtenerDeudaFAct
        private void obtenerDeudaFact()
        {
            decimal DeudaFact = 0;
            foreach (var item in ListaClienteCtaCorriente)
            {
                if (item.EstadoFactura == "Pendiente")
                {
                    DeudaFact += item.Total;
                }
            }
            txtDebeCtaCte.Text = string.Format("{00:0.00}", (DeudaFact));
        }
        //funcion para calcular el total a pagar del cliente CTaCTE
        private void CalcularDeudaTotal()
        {
            decimal _total = 0;
            foreach (var item in ListaClienteCtaCorriente)
            {
                if (item.EstadoFactura == "Pendiente")
                {
                    _total += item.Total;

                }
                else
                    if (item.EstadoFactura == "Pagado")
                    {

                    }
            }
            DeudaTotal = _total;

        }

        //realiza el deposito al cliente
        private void RealizarDeposito(string ClienteCodigo, decimal Guita)
        {
            foreach (var item in ListaCliente)
            {
                if (ClienteCodigo == item.ClienteCodigo)
                {
                    item.Haber = Guita + item.Haber;
                    break;
                }
            }
        }
        //Cambia el estado de la factura a pagado
        private void ActualizarEstadoFactura()
        {
            foreach (var item in ListaClienteCtaCorriente)
            {
                if (item.EstadoFactura == "Pendiente")
                {
                    item.EstadoFactura = "Pagado";
                }
            }
        }

        //Actualza el haber y el debe del cliente si debe sera incluido en el total de la cuenta cte y si no debe quiere decir que tiene saldo a favor y se suma el resto del saldo
        private void ActualizarHaberCliente(string ClienteCodigo)
        {
            foreach (var item in ListaCliente)
            {
                if (ClienteCodigo == item.ClienteCodigo)
                {
                    if (nuevoSaldo >= 0)
                    {
                        item.Haber = nuevoSaldo;
                        item.Deuda = 0;
                    }
                    else if (nuevoSaldo < 0)
                    {
                        item.Deuda = -(nuevoSaldo);
                        item.Haber = 0;
                    }
                }
            }
        }
        private void btnActualizarCtaCorriente_Click(object sender, EventArgs e)
        {
            if ((nudDepositoCta.Value) < 0)
            {
                nudDepositoCta.Value = 0;
            }
            string ClienteCodigo = null;
            ClienteCodigo = Convert.ToString(cmbClienteCta.SelectedValue);
            decimal guitita = Convert.ToDecimal(nudDepositoCta.Value);
            RealizarDeposito(ClienteCodigo, guitita);
            obtenerHaber(ClienteCodigo);
            CalcularDeudaTotal();
            CargarGrillaCtacteClient();
            nuevoSaldo = 0;
            //decimal haber = HaberActual;
            //decimal debe = DebeActual;
            
            // la resta quedaba haber -(-debe) asi que para solucionar tuve que poner -deuda cuando lo pongo en la lista :D
            nuevoSaldo = HaberActual - DebeActual - DeudaTotal;
            txtHaberCtaCte.Text = string.Format("{00:0.00}", nuevoSaldo);
            if (nuevoSaldo >= 0)
            {
                txtDebeCtaCte.Text = "0";
                txtHaberCtaCte.Text = string.Format("{00:0.00}", nuevoSaldo);
            }
            else if (nuevoSaldo < 0)
            {
                nuevoSaldo = nuevoSaldo - 0;
                txtDebeCtaCte.Text = string.Format("{00:0.00}", -(nuevoSaldo));
                txtHaberCtaCte.Text = "0";
            }
            ActualizarHaberCliente(ClienteCodigo);
            ActualizarEstadoFactura();
            cargargrillaCliente();
            CargarGrillaCtacteClient();
            nudDepositoCta.Value = 0;
            txtSaldoNuevoCTa.Text = "0";
        }
        

        private void btnSalirCtaCorriente_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnSalirVenta_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void dtpFechaDesdeCta_ValueChanged(object sender, EventArgs e)
        {
            //cargar la grillaCta
            this.dtpFechaHastaCta.MinDate = this.dtpFechaDesdeCta.Value;
            this.dtpFechaHastaCta.Value = this.dtpFechaDesdeCta.Value;
            cargarGrillaCtaCte();


            //Hace lo mismo que el evento change de cmb para q quede mas bonito
            txtDebeCtaCte.Clear();
            txtHaberCtaCte.Clear();
            txtSaldoNuevoCTa.Clear();
            nudDepositoCta.Enabled = true;
            ListaClienteCtaCorriente.Clear();

            string ClienteCodigo = null;
            ClienteCodigo = Convert.ToString(cmbClienteCta.SelectedValue);
            obtenerFactClient(ClienteCodigo);
            ObtenerDeudaClient();
            obtenerDeudaFact();
            obtenerHaber(ClienteCodigo);
            nudDepositoCta.Enabled = true;
            CargarGrillaCtacteClient();
        }

        private void dtpFechaHastaCta_ValueChanged(object sender, EventArgs e)
        {
            this.dtpFechaHastaCta.MinDate = this.dtpFechaDesdeCta.Value;
            cargarGrillaCtaCte();

            //Hace lo mismo que el evento change de cmb para q quede mas bonito
            txtDebeCtaCte.Clear();
            txtHaberCtaCte.Clear();
            txtSaldoNuevoCTa.Clear();
            nudDepositoCta.Enabled = true;
            ListaClienteCtaCorriente.Clear();

            string ClienteCodigo = null;
            ClienteCodigo = Convert.ToString(cmbClienteCta.SelectedValue);
            obtenerFactClient(ClienteCodigo);
            ObtenerDeudaClient();
            obtenerDeudaFact();
            obtenerHaber(ClienteCodigo);
            nudDepositoCta.Enabled = true;
            CargarGrillaCtacteClient();
        }

        

        

        

       
    }

        
       



        


        

        

        
    
    
    
    
    
}


