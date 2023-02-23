using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Actuelice1.clases.util;
using RelacionadorDeSerie;
//using System.IO;
using System.Windows.Media;
using ReneUtiles;
using ReneUtiles.Clases;
using ReneUtiles.Clases.Subprocesos;
using ReneUtiles.Clases.WPF;
using System.Collections.ObjectModel;
using ReneUtiles.Clases.Multimedia.Series.Procesadores.Buscadores.Datos;
using Delimon.Win32.IO;
namespace Actuelice1.clases.vista
{
    /// <summary>
    /// Interaction logic for Configuracion.xaml
    /// </summary>
    public partial class Configuracion : Page
    {
        

        List<Direccion> direcciones;
        ObservableCollection<Direccion> direcciones_filtradas;

        List<Direccion> direccionesPaquete;
        ObservableCollection<Direccion> direccionesPaquete_Filtradas;

        ManagerEtiquetasDialogoBuscar mngEtiquetasDialogoBuscar_DireccionesPaquete;
        ManagerSelectionIndex_CB_Etiquetas mngSI_CB_EtiquetasPaquete;
        ManagerSelectionIndex mngSI_CB_SeccionesPaquete;
        ManagerSelectionIndex mngSI_CB_SeccionesPropias;
        ManagerSelectionIndex mngSI_CB_CategoriasPropias;

        MangerColumnaChB<Direccion> mngChBTodas_Propias;
        MangerColumnaChB<Direccion> mngChBTodas_Paquete;

        bool seInicializo = false;
        //string tagAnteriorSeleccionado_Paquete;

        public Configuracion()
        {
            



            InitializeComponent();


            inicializar();
            


            //inicializarSeccionPaquete();

            seInicializo = true;
        }
        


        public TipoDeSeccion getSeccion(int pos = 0)
        {
            TipoDeSeccion tds = null;
            System.Windows.Controls.ComboBox cb = null;

            int num = 0;
            if (pos == 0)
            {
                cb = cbSecciones;
            }
            else if (pos == 1)
            {
                num = 1;
                cb = comboboxSecciones;
            }
            else if (pos == 2)
            {
                num = 1;
                cb = comboboxSecciones_TXT;
            }

            if (cb.SelectedIndex == 1 - num)
            {
                tds = TipoDeSeccion.SERIES;
            }
            else if (cb.SelectedIndex == 2 - num)
            {
                tds = TipoDeSeccion.ANIME;
            }

            return tds;
        }
        public TipoDeCategoriaPropias getCategoria(int pos = 0)
        {
            TipoDeCategoriaPropias tdc = null;

            System.Windows.Controls.ComboBox cb = null;


            if (pos == 0)
            {
                cb = cbCategoria;

                if (cb.SelectedIndex != 0 && cb.SelectedIndex != -1)
                {
                    tdc = TipoDeCategoriaPropias.VALUES[cb.SelectedIndex - 1];
                }

            }
            else if (pos == 1)
            {
                cb = comboboxCategoria;


                if (cb.SelectedIndex != -1)
                {
                    tdc = TipoDeCategoriaPropias.VALUES[cb.SelectedIndex];
                }

            }
            else if (pos == 2)
            {
                cb = comboboxCategoria_TXT;


                if (cb.SelectedIndex != -1)
                {
                    tdc = TipoDeCategoriaPropias.VALUES[cb.SelectedIndex];
                }

            }


            return tdc;
        }

        public List<DireccionDeActualizadorPropia> getlistr()
        {
            List<DireccionDeActualizadorPropia> direcciones_r = null;

            TipoDeSeccion tds = getSeccion();

            TipoDeCategoriaPropias tdc = getCategoria();

            if (tds != null && tdc != null)
            {
                direcciones_r = Referencia.manager.getDireccionesPropias(tds, tdc);
            }
            else if (tds != null && tdc == null)
            {
                direcciones_r = Referencia.manager.getDireccionesPropias(tds);
            }
            else if (tds == null && tdc == null)
            {
                direcciones_r = Referencia.manager.getDireccionesPropias();
            }
            else if (tds == null && tdc != null)
            {
                direcciones_r = Referencia.manager.getDireccionesPropias(tdc);
            }

            return direcciones_r;
        }

        public void ActualizarLista()
        {
            List<DireccionDeActualizadorPropia> listr = getlistr();
            direcciones.Clear();
            foreach (var item in listr)
            {
                if (item != null)
                {
                    Direccion dir = new Direccion();
                    dir.id = item.id + "";
                    //dir.nombre=item.nom
                    DirectoryInfo d = new DirectoryInfo(item.url);
                    if (d.Parent != null)
                    {
                        dir.nombre = d.Name;
                    }

                    dir.seccion = item.seccion.getValor();
                    dir.seleccionar = item.seleccioniada;
                    dir.unidad = item.letra;
                    dir.direccion = item.url;
                    dir.categoria = item.categoria.getValor();

                    if (!Directory.Exists(dir.direccion))
                        dir.color_dir = Brushes.OrangeRed;
                    else dir.color_dir = Brushes.Black;

                    List<string> listcb = new List<string>(Directory.GetLogicalDrives());

                    dir.unidades = listcb;

                    direcciones.Add(dir);
                }
            }
            filtrar_direcciones_propias();


        }


        private void alApretar_B_BuscarDirecciones_Propias(object sender, RoutedEventArgs e)
        {
            string dir = "";

            //OpenFileDialog openfile = new OpenFileDialog();
            //var result = openfile.ShowDialog();
            //if (result.ToString() != string.Empty)
            //{
            //    direccionFile.Text = openfile.FileName;
            //}



            //FolderBrowserDialog folderdialog = new FolderBrowserDialog();
            //var result = folderdialog.ShowDialog();
            //if (result.ToString() != string.Empty)
            //{
            //    dir = folderdialog.SelectedPath;
            //    direccionFile.Text = dir;

            //    string[] arrsplit = dir.Split(new string[] { @"\" }, 20, System.StringSplitOptions.None);
            //    nombrecarpeta.Text = arrsplit[arrsplit.Length - 1];
            //    nombreunidad.Text = arrsplit[0];
            //}

            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            string dirAnterior = Referencia.manager.TS().getStr("ultimoDir", "");
            if (dirAnterior.Length > 0)
            {
                dialog.SelectedPath = dirAnterior;
            }
            bool? success = dialog.ShowDialog();
            if (success == true)
            {

                dir = dialog.SelectedPath;
                Referencia.manager.TS().put("ultimoDir", dir);

                direccionFile.Text = dir;
                string[] arrsplit = dir.Split(new string[] { @"\" }, 20, System.StringSplitOptions.None);
                nombrecarpeta.Text = arrsplit[arrsplit.Length - 1];
                nombreunidad.Text = arrsplit[0];
            }
        }

        private void alApretar_B_Aceptar_AlSeleccionarUnaCarpeta(object sender, RoutedEventArgs e)
        {
            if (direccionFile.Text.Length == 0)
            {
                System.Windows.MessageBox.Show("Es necesario seleccionar una carpeta");
            }
            else if (comboboxSecciones.SelectedIndex == -1)
            {
                System.Windows.MessageBox.Show("Es necesario seleccionar una Sección");
            }
            else if (comboboxCategoria.SelectedIndex == -1)
            {
                System.Windows.MessageBox.Show("Es necesario seleccionar una Categoría");
            }
            else
            {
                ComboBoxItem cbi = (ComboBoxItem)comboboxSecciones.SelectedItem;
                StackPanel sp = (StackPanel)cbi.Content;
                TextBlock tbseccion = (TextBlock)sp.Children[1];

                ComboBoxItem cbc = (ComboBoxItem)comboboxCategoria.SelectedItem;
                string _categoria = cbc.ContentStringFormat;

                //Direccion dir = new Direccion() { id = IdGenerate() + "", seleccionar = (bool)seleccionartb.IsChecked, nombre = nombrecarpeta.Text, direccion = direccionFile.Text, unidad = nombreunidad.Text, seccion = tbseccion.Text, categoria = _categoria };

                TipoDeSeccion tds = getSeccion(1);
                TipoDeCategoriaPropias tdc = getCategoria(1);

                Referencia.manager.agregarDireccionPropia(tds, tdc, direccionFile.Text, (bool)seleccionartb.IsChecked);
                if (tds == TipoDeSeccion.SERIES)
                {
                    Referencia.EA_Series.variables.datosDeRecarga.add(tdc);
                }
                if (tds == TipoDeSeccion.ANIME)
                {
                    Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(tdc);
                }
                //cbSecciones.SelectedIndex = comboboxSecciones.SelectedIndex + 1;
                //cbCategoria.SelectedIndex = comboboxCategoria.SelectedIndex + 1;
                mngSI_CB_SeccionesPropias.selectIndex(comboboxSecciones.SelectedIndex + 1);
                mngSI_CB_CategoriasPropias.selectIndex(comboboxCategoria.SelectedIndex + 1);

                ActualizarLista();

                //dgDireccion.Items.Refresh();

                dialoghost.IsOpen = false; //dialoghost_configuracion

                Reset();
            }

        }

        //private void showDialogoCargando_Configuracion() {MostrarDialogo();}
        //private void hideDialogoCargando_Configuracion(){dialoghost_configuracion.IsOpen = false;}

        private void alApretar_B_Cancelar_DlgAgregarDireccion_Propia(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            direccionFile.Text = nombrecarpeta.Text = nombreunidad.Text = "";
            comboboxSecciones.SelectedIndex = comboboxCategoria.SelectedIndex = -1;
        }

        private void alApretar_B_AliminarDireccionSeleccionada_Propia(object sender, RoutedEventArgs e)
        {

            if (dgDireccion.SelectedIndex > -1)
            {
                var mens = System.Windows.MessageBox.Show("Está seguro que desea eliminar la fila?", "Confirmación", MessageBoxButton.YesNoCancel);
                if (mens == MessageBoxResult.Yes)
                {
                    Direccion dir = (Direccion)dgDireccion.SelectedItem;
                    TipoDeSeccion tds = TipoDeSeccion.get(dir.seccion);
                    TipoDeCategoriaPropias tdc = TipoDeCategoriaPropias.get(dir.categoria);

                    direcciones.Remove(dir);
                    //dgDireccion.Items.Refresh();

                    DireccionDeActualizadorPropia dap = new DireccionDeActualizadorPropia(
                    id: Convert.ToInt32(dir.id),
                    categoria: tdc,
                    seccion: tds,
                    seleccioniada: dir.seleccionar,
                    url: dir.direccion
                    );

                    Referencia.manager.eliminarDireccionPropia(dap);

                    if (tds == TipoDeSeccion.SERIES)
                    {
                        Referencia.EA_Series.variables.datosDeRecarga.add(tdc);
                    }
                    else if (tds == TipoDeSeccion.ANIME)
                    {
                        Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(tdc);
                    }
                    direcciones_filtradas.Remove(dir);

                    //filtrar_direcciones_propias();
                }
            }
            else System.Windows.MessageBox.Show("Debe seleccionar una fila de la tabla");
        }

        private void alArpetar_B_AbrirDilagoAgregarDireccion_Propia(object sender, RoutedEventArgs e)
        {
            if (cbSecciones.SelectedIndex > -1)
            {
                comboboxSecciones.SelectedIndex = cbSecciones.SelectedIndex - 1;
                // mngSI_CB_SeccionesPropias.selectIndex(cbSecciones.SelectedIndex - 1);
            }
            if (cbCategoria.SelectedIndex > -1)
            {
                comboboxCategoria.SelectedIndex = cbCategoria.SelectedIndex - 1;
                //mngSI_CB_CategoriasPropias.selectIndex(cbCategoria.SelectedIndex - 1);
            }
        }

        private void alApretar_B_Volver_Propias(object sender, RoutedEventArgs e)
        {
            
        }

        private void alApretar_B_Volver() {
            //Referencia.EA_Configuracion.index_cb_secciones_configuracion
            if (cbSecciones.SelectedIndex == 1)
            {
                //Referencia.frame.Navigate(new Uri("clases/vista/Series.xaml", UriKind.RelativeOrAbsolute));
                Referencia.showFrame(Referencia.EA_Configuracion.index_cb_secciones_configuracion == 1 ? Referencia.ventana_series : Referencia.ventana_anime);
            }
        }

        private void T_Buscar_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            filtrar_direcciones_propias();
            //if (tbbuscar.Text.Length > 0)
            //{
            //    string texto = tbbuscar.Text;

            //    direcciones_filtradas.Clear();

            //    direcciones_filtradas = direcciones.FindAll(d => d.id.Contains(texto) || (d.seleccionar + "").Contains(texto)
            //    || d.nombre.Contains(texto) || d.unidad.Contains(texto));

            //    dgDireccion.ItemsSource = direcciones_filtradas;
            //    dgDireccion.Items.Refresh();
            //}
            //else
            //{
            //    dgDireccion.ItemsSource = direcciones;
            //    dgDireccion.Items.Refresh();
            //}

        }

        private void alApretar_B_EliminarTexto_Buscar_DireccionesPropias(object sender, RoutedEventArgs e)
        {
            tbbuscar.Text = "";
            filtrar_direcciones_propias();
            //dgDireccion.ItemsSource = direcciones;
            //dgDireccion.Items.Refresh();
        }

        private void alApretar_B_ImportarDirecciones_Propias(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog openfile = new OpenFileDialog();
            //var result = openfile.ShowDialog();
            //if (result.ToString() != string.Empty)
            //{
            //    direccionFile.Text = openfile.FileName;
            //}

            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "SQLite (*.acconf)|*.*";

            DialogResult result = ofd.ShowDialog();
            if (result== DialogResult.OK)
            {
                MostrarDialogo();


                Referencia.manager.importarDireccionesPropias_Subproseso(new FileInfo(ofd.FileName), new EventosEnSubproceso(
                    () =>
                    {
                        dialoghost.IsOpen = false;
                        ActualizarLista();

                        //dgDireccion.Items.Refresh();

                        Referencia.EA_Series.variables.datosDeRecarga.setActualizarTodasLasCategorias();
                        Referencia.EA_SeriesAnimes.variables.datosDeRecarga.setActualizarTodasLasCategorias();

                    },
                    ex =>
                    {

                    }
                    )
                    );


            }
        }

        private void alApretar_B_ExportarDirecciones_Propias(object sender, RoutedEventArgs e)
        {
            //SaveFileDialog sfd = new SaveFileDialog();
            //var result = sfd.ShowDialog();
            //if (result.ToString() != string.Empty)
            //{
            //    System.Windows.MessageBox.Show(sfd.FileName);
            //}

            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "SQLite (*.acconf)|*.*";

            var result = sfd.ShowDialog();

            if (result.ToString() != string.Empty)
            {
                MostrarDialogo();

                Console.WriteLine(sfd.FileName);
                FileInfo f = new FileInfo(Archivos.setExtencionStr(sfd.FileName, ".acconf"));
                Referencia.manager.exportarDireccionesPropias_Subproseso(f, new EventosEnSubproceso(
                    () =>
                    {
                        dialoghost.IsOpen = false;
                    },
                    ex =>
                    {

                    }
                    )
                    );


            }
        }

        public void MostrarDialogo()
        {
            StackPanel sp = new StackPanel();
            System.Windows.Controls.ProgressBar pb = new System.Windows.Controls.ProgressBar();

            pb.Width = 100;
            pb.Height = 30;

            sp.Children.Add(pb);

            dialoghost.DialogContent = sp;
            dialoghost.IsOpen = true;
        }

        private void cbSecciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (estaInicializado())
            {
                ActualizarLista();
                //dgDireccion.Items.Refresh();
            }

        }


        private void cbCategoria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (estaInicializado())
            {
                ActualizarLista();
                //dgDireccion.Items.Refresh();
            }
        }


        private void tbdirecciones_Checked(object sender, RoutedEventArgs e)
        {
            mngChBTodas_Propias.alApretar_TB_MarcarTodos(true);
            //foreach (Direccion dir in direcciones_filtradas)
            //{
            //    if (!dir.getSeleccionar())
            //    {
            //        dir.setSeleccionar(true);
            //        Referencia.manager.updateDireccionesPropias(Parse(dir));

            //        TipoDeSeccion tds = TipoDeSeccion.get(dir.seccion);
            //        TipoDeCategoriaPropias tdc = TipoDeCategoriaPropias.get(dir.categoria);

            //        if (tds == TipoDeSeccion.SERIES)
            //        {
            //            Referencia.EA_Series.variables.datosDeRecarga.add(tdc);
            //        }
            //        else if (tds == TipoDeSeccion.ANIME)
            //        {
            //            Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(tdc);
            //        }

            //    }
            //}

            //dgDireccion.Items.Refresh();
        }

        private void tbdirecciones_Unchecked(object sender, RoutedEventArgs e)
        {
            //ponerlosEnFalse();
            mngChBTodas_Propias.alApretar_TB_MarcarTodos(false);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            //Chequear(true);
            mngChBTodas_Propias.Alseleccionar_Elemento();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            mngChBTodas_Propias.Alseleccionar_Elemento();
            // Chequear(false);
        }

        //public void Chequear(bool var)
        //{

         //   if (dgDireccion.SelectedIndex > -1)
        //    {

        //        Direccion dir = (Direccion)dgDireccion.SelectedItem;

        //        if (dir.seleccionar != var)
        //        {


        //            dir.seleccionar = var;

        //            DireccionDeActualizadorPropia dap = Parse(dir);
        //            //System.Windows.MessageBox.Show(dap.seleccioniada + "");
        //            Referencia.manager.updateDireccionesPropias(dap);

        //            tbdirecciones.IsChecked = hayQuePonerenTrue(getIdSeleccionado(), var);


        //            TipoDeSeccion tds = TipoDeSeccion.get(dir.seccion);
        //            TipoDeCategoriaPropias tdc = TipoDeCategoriaPropias.get(dir.categoria);

        //            if (tds == TipoDeSeccion.SERIES)
        //            {
        //                Referencia.EA_Series.variables.datosDeRecarga.add(tdc);
        //            }
        //            else if (tds == TipoDeSeccion.ANIME)
        //            {
        //                Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(tdc);
        //            }

        //            filtrar_direcciones_propias();
        //        }

        //    }
        //}

        public DireccionDeActualizadorPropia Parse(Direccion dir)
        {

            DireccionDeActualizadorPropia dap = new DireccionDeActualizadorPropia(
                id: Convert.ToInt32(dir.id),
                categoria: TipoDeCategoriaPropias.get(dir.categoria),
                seccion: TipoDeSeccion.get(dir.seccion),
                seleccioniada: dir.seleccionar,
                url: dir.direccion
                );

            return dap;


        }

        private void alApretar_B_EliminarTodasLasDirecciones_Propias(object sender, RoutedEventArgs e)
        {
            //HashSet<TipoDeSeccion> hs_sc = TipoDeSeccion.getNewHashSet();
            foreach (var dir in direcciones_filtradas)
            {
                TipoDeSeccion tds = TipoDeSeccion.get(dir.seccion);
                TipoDeCategoriaPropias tdc = TipoDeCategoriaPropias.get(dir.categoria);


                DireccionDeActualizadorPropia dap = new DireccionDeActualizadorPropia(
                id: Convert.ToInt32(dir.id),
                categoria: tdc,
                seccion: tds,
                seleccioniada: dir.seleccionar,
                url: dir.direccion
                );

                Referencia.manager.eliminarDireccionPropia(dap);


                if (tds == TipoDeSeccion.SERIES)
                {
                    Referencia.EA_Series.variables.datosDeRecarga.add(tdc);
                }
                else if (tds == TipoDeSeccion.ANIME)
                {
                    Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(tdc);
                }
                direcciones.Remove(dir);
            }

            direcciones_filtradas.Clear();
            //dgDireccion.Items.Refresh();

            
        }

        private void ComboBox_SelectionChanged_UnidadDiscoDuro_Propia(object sender, SelectionChangedEventArgs e)
        {
            if (dgDireccion.SelectedIndex > -1)
            {
                System.Windows.Controls.ComboBox cb = (System.Windows.Controls.ComboBox)sender;
                if (cb.SelectedItem!=null) {
                    Direccion dir = (Direccion)dgDireccion.SelectedItem;

                    Referencia.manager.supr(getEV_Dlg(() => {
                        dgDireccion.SelectedIndex = -1;
                        //dgDireccion.Items.Refresh();
                    }), () => {


                        string sj = cb.SelectedItem.ToString()[0] + dir.direccion.Substring(1);
                        dir.direccion = sj;
                        Referencia.manager.updateDireccionesPropias(Parse(dir));
                        dir.unidad = (string)cb.SelectedValue;

                        if (!Directory.Exists(dir.direccion))
                            dir.color_dir = Brushes.OrangeRed;
                        else dir.color_dir = Brushes.Black;



                        TipoDeSeccion tds = TipoDeSeccion.get(dir.seccion);
                        TipoDeCategoriaPropias tdc = TipoDeCategoriaPropias.get(dir.categoria);
                        if (tds == TipoDeSeccion.SERIES)
                        {
                            Referencia.EA_Series.variables.datosDeRecarga.add(tdc);
                        }
                        else if (tds == TipoDeSeccion.ANIME)
                        {
                            Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(tdc);
                        }
                    });

                }








            }

        }

        public string getIdSeleccionado()
        {
            //Console.WriteLine(dgDireccion);
            //Console.WriteLine(dgDireccion.SelectedItem);
            //Console.WriteLine(((Direccion)dgDireccion.SelectedItem).id);
            return ((Direccion)dgDireccion.SelectedItem).id;
        }

        //public bool hayQuePonerenTrue(string id, bool fueCheked)
        //{

        //    foreach (Direccion dir in direcciones_filtradas)
        //    {
        //        bool estaSeleccionado = dir.getSeleccionar();
        //        if (dir.id == id)
        //        {
        //            estaSeleccionado = fueCheked;
        //            dir.setSeleccionar(estaSeleccionado);


        //        }
        //        if (!estaSeleccionado)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;

        //}


        //public void ponerlosEnFalse()
        //{
            
        //    bool todosEstanFalse=true;

        //    foreach (Direccion s in direcciones_filtradas)
        //    {
        //        bool estaSeleccionado = s.getSeleccionar();

        //        if (estaSeleccionado)
        //        {
        //            todosEstanFalse=false;
        //            break;
        //            //return;
        //        }
        //    }
        //    if (!todosEstanFalse) {
        //        foreach (Direccion dir in direcciones_filtradas)
        //        {
        //            if (dir.getSeleccionar())
        //            {
        //                dir.setSeleccionar(false);
        //                Referencia.manager.updateDireccionesPropias(Parse(dir));


        //                TipoDeSeccion tds = TipoDeSeccion.get(dir.seccion);
        //                TipoDeCategoriaPropias tdc = TipoDeCategoriaPropias.get(dir.categoria);

        //                if (tds == TipoDeSeccion.SERIES)
        //                {
        //                    Referencia.EA_Series.variables.datosDeRecarga.add(tdc);
        //                }
        //                else if (tds == TipoDeSeccion.ANIME)
        //                {
        //                    Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(tdc);
        //                }

        //            }


        //        }
        //    }

            


        //    dgDireccion.Items.Refresh();
        //    //return true;

        //}




            //---------------------René
            

        //------------------------------------ Paquete  René



        private EventosEnSubproceso getEV_Dlg(Action a = null)
        {
            return getEV(() =>
            {

                a?.Invoke();
                dialoghost.IsOpen = false;
                //dialoghost1.IsOpen = false;
            });
        }

        private EventosEnSubproceso getEV(Action a = null)
        {
            return new EventosEnSubproceso(
                () =>
                {
                    a?.Invoke();
                }
                , ex => { System.Windows.MessageBox.Show("Error!!!", "Error"); }
                );
        }


        public void filtrar_direcciones_propias() {
            UtilidadesActualize.aplicarFiltroText<Direccion>(
                dgDireccion, direcciones, direcciones_filtradas, tbbuscar
                , (d, texto) => d.id.Contains(texto) || (d.seleccionar + "").Contains(texto)
                || d.nombre.Contains(texto) || d.unidad.Contains(texto));
            
        }
        //private string getStrings_TiposDeEtiquetas(IEnumerable<>) {

        //}
        private void actualizarTags_CB_DireccionesPaquete(List<DireccionDePaquete> todasLasDireccionesDeLaSeccion)
        {
            mngSI_CB_EtiquetasPaquete.actualizarTags(todasLasDireccionesDeLaSeccion, dir => dir.etiquetas.etiquetas);
            //SortedSet<string> hsTags = new SortedSet<string>();
            //foreach (DireccionDePaquete dir in todasLasDireccionesDeLaSeccion)
            //{

            //    hsTags.Add(UtilidadesActualize.getEtiquetasEnStr(dir.etiquetas.etiquetas));
            //}
            //mngSI_CB_EtiquetasPaquete.clear();


            //int i = 0;
            //bool hayQueSeleccionar = false;
            //int indiceEnElQueSeleccionar = -1;
            //foreach (string tag in hsTags)
            //{

            //    cbCategoria_Paquete.Items.Add(tag);
            //    if (tagAnteriorSeleccionado_Paquete != null && tag == tagAnteriorSeleccionado_Paquete)
            //    {
            //        hayQueSeleccionar = true;
            //        indiceEnElQueSeleccionar = i;
            //    }
            //    i++;
            //}
            //if (hayQueSeleccionar)
            //{
            //    mngSI_CB_EtiquetasPaquete.selectIndex(indiceEnElQueSeleccionar);

            //}
        }

        //private bool hayQuePonerenTrue_Paquete(string id, bool fueCheked)
        //{

        //    foreach (Direccion dir in direccionesPaquete_Filtradas)
        //    {
        //        bool estaSeleccionado = dir.getSeleccionar();
        //        if (dir.id == id)
        //        {
        //            estaSeleccionado = fueCheked;
        //            dir.setSeleccionar(estaSeleccionado);


        //        }
        //        if (!estaSeleccionado)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;

        //}
        private string getIdSeleccionado_Paquete()
        {

            return ((Direccion)dgDireccion_Paquete.SelectedItem).id;
        }
        //private void Chequear_Paquete(bool var)
        //{
        //    if (dgDireccion_Paquete.SelectedIndex > -1)
        //    {

        //        Direccion dir = (Direccion)dgDireccion_Paquete.SelectedItem;

        //        dir.seleccionar = var;

        //        DireccionDePaquete dp = Parse_A_DireccionDePaquete(dir);
        //        Referencia.manager.updateDireccionesDePaquete(dp);

        //        tbdirecciones_Paquete.IsChecked = hayQuePonerenTrue_Paquete(getIdSeleccionado_Paquete(), var);


        //        TipoDeSeccion tds = dp.seccion;
        //        if (tds == TipoDeSeccion.SERIES)
        //        {
        //            Referencia.EA_Series.variables.datosDeRecarga.add(dp.etiquetas);
        //        }
        //        else if (tds == TipoDeSeccion.ANIME)
        //        {
        //            Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(dp.etiquetas);
        //        }
        //        //if (dir.getSeleccionar()!=var) {

        //        //}

        //    }
        //}

        //private void ponerlosEnFalse_DireccionesdelPaquete()
        //{


        //    bool todosEstanEnFalse = true;
        //    foreach (Direccion s in direccionesPaquete_Filtradas)
        //    {
        //        bool estaSeleccionado = s.getSeleccionar();

        //        if (estaSeleccionado)
        //        {
        //            todosEstanEnFalse=false;
        //            //return;
        //        }
        //    }

        //    if (!todosEstanEnFalse) {

        //        foreach (Direccion s in direccionesPaquete_Filtradas)
        //        {
        //            if (s.getSeleccionar())
        //            {
        //                s.setSeleccionar(false);
        //                DireccionDePaquete dp = Parse_A_DireccionDePaquete(s);
        //                Referencia.manager.updateDireccionesDePaquete(dp);

        //                TipoDeSeccion tds = dp.seccion;
        //                if (tds == TipoDeSeccion.SERIES)
        //                {
        //                    Referencia.EA_Series.variables.datosDeRecarga.add(dp.etiquetas);
        //                }
        //                else if (tds == TipoDeSeccion.ANIME)
        //                {
        //                    Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(dp.etiquetas);
        //                }
        //            }

        //        }


        //        dgDireccion_Paquete.Items.Refresh();
        //    }

            
        //    //return true;

        //}

        private DireccionDePaquete Parse_A_DireccionDePaquete(Direccion dir)
        {

            DireccionDePaquete dap = new DireccionDePaquete(
               id: Convert.ToInt32(dir.id),

               seccion: TipoDeSeccion.get(dir.seccion),
               seleccionada: dir.seleccionar,
               url: dir.direccion
               , etiquetas: dir.categoria.Length > 0 ? new ConjuntoDeEtiquetasDeSerie((from tag in Utiles.split(dir.categoria, " ") select TipoDeEtiquetaDeSerie.get(tag))) : new ConjuntoDeEtiquetasDeSerie()
               );


            return dap;


        }
        private void Reset_Dlg_AgregarDireccionPaquete()
        {
            direccionFile_Paquete.Text = nombrecarpeta_Paquete.Text = nombreunidad_Paquete.Text = "";
            CB_Secciones_AgregarDireccion_Paquete.SelectedIndex = -1;

            //comboboxSecciones.SelectedIndex = comboboxCategoria.SelectedIndex = -1;
        }
        
        private TipoDeSeccion getSeccion_Paquete(int pos = 0)
        {
            TipoDeSeccion tds = null;
            System.Windows.Controls.ComboBox cb = null;

            int num = 0;
            if (pos == 0)
            {
                cb = cbSecciones_Paquete;
            }
            else if (pos == 1)
            {
                num = 1;
                cb = CB_Secciones_AgregarDireccion_Paquete;
            }

            if (cb.SelectedIndex == 1 - num)
            {
                tds = TipoDeSeccion.SERIES;
            }
            else if (cb.SelectedIndex == 2 - num)
            {
                tds = TipoDeSeccion.ANIME;
            }

            return tds;
        }

        private List<DireccionDePaquete> getlistaDireccionesPaquete()
        {
            List<DireccionDePaquete> direcciones_r = null;

            TipoDeSeccion tds = getSeccion_Paquete();

            TipoDeEtiquetaDeSerie[] tdc = mngSI_CB_EtiquetasPaquete.getEtiquetas(); //getEtiquetas_Paquete();

            direcciones_r = Referencia.manager.getDireccionesDePaquete(tds, tdc);



            return direcciones_r;
        }

        private List<DireccionDePaquete> getlistaDireccionesPaquete_TodasEnSeccion()
        {
            List<DireccionDePaquete> direcciones_r = null;

            TipoDeSeccion tds = getSeccion_Paquete();



            direcciones_r = Referencia.manager.getDireccionesDePaquete(tds);



            return direcciones_r;
        }

        private void ActualizarLista_Paquete(
            //bool actualizarTags=true
            )
        {
            List<DireccionDePaquete> ldp = getlistaDireccionesPaquete_TodasEnSeccion();
            actualizarTags_CB_DireccionesPaquete(ldp);

            List<DireccionDePaquete> listr = getlistaDireccionesPaquete();
            direccionesPaquete.Clear();
            List<string> listcb = new List<string>(Directory.GetLogicalDrives());
            foreach (DireccionDePaquete item in listr)
            {
                if (item != null)
                {
                    Direccion dir = new Direccion();
                    dir.id = item.id + "";
                    //dir.nombre=item.nom
                    DirectoryInfo d = new DirectoryInfo(item.url);
                    if (d.Parent != null)
                    {
                        dir.nombre = d.Name;
                    }

                    dir.seccion = item.seccion.getValor();
                    dir.seleccionar = item.seleccioniada;
                    dir.unidad = item.letra;
                    dir.direccion = item.url;

                    dir.categoria = UtilidadesActualize.getEtiquetasEnStr(item.etiquetas.etiquetas);
                    //dir.categoria = "";//item.categoria.getValor();
                    //SortedSet<string> st_tags = new SortedSet<string>();
                    //foreach (TipoDeEtiquetaDeSerie tag in item.etiquetas.etiquetas)
                    //{
                    //    st_tags.Add(tag.nombreTag);
                    //   // dir.categoria +=" "+ tag.nombreTag;
                    //}

                    //Action<TipoDeEtiquetaDeSerie> ponerDePrimero = t => {
                    //    if (st_tags.Contains(t.nombreTag))
                    //    {
                    //        dir.categoria += (dir.categoria.Length > 0 ? " " : "") + t.nombreTag;
                    //        st_tags.Remove(t.nombreTag);
                    //    }
                    //};
                    //ponerDePrimero(TipoDeEtiquetaDeSerie.FINALIZADAS);
                    //ponerDePrimero(TipoDeEtiquetaDeSerie.TX);
                    //ponerDePrimero(TipoDeEtiquetaDeSerie.CLASICAS);
                    //ponerDePrimero(TipoDeEtiquetaDeSerie.DOBLADAS);
                    //ponerDePrimero(TipoDeEtiquetaDeSerie.SUBTITULADAS);
                    //foreach (string tag in st_tags)
                    //{
                    //    dir.categoria += (dir.categoria.Length>0?" ":"")+ tag;
                    //}
                    if (!Directory.Exists(dir.direccion))
                        dir.color_dir = Brushes.OrangeRed;
                    else dir.color_dir = Brushes.Black;



                    dir.unidades = listcb;

                    direccionesPaquete.Add(dir);
                }
            }

            
            //if (actualizarTags) {
            

            filtrarListaDireccionesPaqueteATravesDeTextoBuscar();
            //}



        }

        private class ManagerEtiquetasDialogoBuscar
        {
            //private ConjuntoDeEventos<Action> metodoAgregarStringsAListaOrigen = new ConjuntoDeEventos<Action>();
            //private ConjuntoDeEventos<Action> metodoAgregarStringsAListaDestino = new ConjuntoDeEventos<Action>();

            public string[] StringsAListaOrigen;
            public string[] StringsAListaDestino;

            private Action resetear;

            public System.Windows.Controls.ListBox lb_EtiquetasOrigen, lb_EtiquetasDestino;


            public ManagerEtiquetasDialogoBuscar(System.Windows.Controls.ListBox lb0, System.Windows.Controls.ListBox lb1
                // , System.Windows.Controls.Button Bclear
                )
            {
                resetear = () =>
                {
                    lb0.Items.Clear();
                    UtilesWPF.addStrings(lb0, StringsAListaOrigen);
                    lb1.Items.Clear();
                    UtilesWPF.addStrings(lb1, StringsAListaDestino);
                };



                Action<System.Windows.Controls.ListBox, System.Windows.Controls.ListBox> seEventoIntercambio = (a, b) =>
                {
                    a.MouseUp += (s, e) =>
                    {
                        if (a.SelectedIndex > -1)
                        {
                            b.Items.Add(a.SelectedItem);
                            a.Items.RemoveAt(a.SelectedIndex);
                        }
                    };
                };
                seEventoIntercambio(lb0, lb1);
                seEventoIntercambio(lb1, lb0);
                //Bclear.Click += (s, e) => resetear();

                this.lb_EtiquetasDestino = lb1;
                this.lb_EtiquetasOrigen = lb0;
            }
            private List<TipoDeEtiquetaDeSerie> getEtiquetas(System.Windows.Controls.ListBox lb)
            {
                List<TipoDeEtiquetaDeSerie> lt = new List<TipoDeEtiquetaDeSerie>();
                foreach (var item in lb.Items)
                {
                    lt.Add(TipoDeEtiquetaDeSerie.get(item + ""));
                }
                return lt;
                //return (from item in lb.Items.SourceCollection select TipoDeEtiquetaDeSerie.get(item+""));
            }
            public List<TipoDeEtiquetaDeSerie> getEtiquetasOrigen()
            {
                return getEtiquetas(lb_EtiquetasOrigen);
            }
            public List<TipoDeEtiquetaDeSerie> getEtiquetasDestino()
            {
                return getEtiquetas(lb_EtiquetasDestino);
            }

            public void ejecutar(string[] StringsAListaOrigen, string[] StringsAListaDestino)
            {
                this.StringsAListaOrigen = StringsAListaOrigen;
                this.StringsAListaDestino = StringsAListaDestino;

                this.resetear();
            }
            public void ejecutar(TipoDeEtiquetaDeSerie[] etiquetas)
            {
                HashSet<TipoDeEtiquetaDeSerie> tagsParaOrigen = TipoDeEtiquetaDeSerie.getNewHashSet(TipoDeEtiquetaDeSerie.ETIQUETAS_DE_CLASIFICACION_EXTRA);
                foreach (TipoDeEtiquetaDeSerie tag in etiquetas)
                {
                    tagsParaOrigen.Remove(tag);
                }
                StringsAListaOrigen = (from tag in tagsParaOrigen select tag.nombreTag).ToArray();
                StringsAListaDestino = (from tag in etiquetas select tag.nombreTag).ToArray();
                ejecutar(StringsAListaOrigen, StringsAListaDestino);
            }
        }

        private bool estaInicializado()
        {
            return seInicializo;
        }

        private void alApretar_B_EtiquetaPrincipal(object sender, RoutedEventArgs e)
        {
            B_EtiquetaPrincipal.Content = B_EtiquetaPrincipal.Content.ToString() == "TX" ? "FINALIZADAS" : "TX";

        }


        private void alApretar_B_Volver_Paquete(object sender, RoutedEventArgs e)
        {
            if (cbSecciones.SelectedIndex == 1)
            {
                Referencia.frame.Navigate(new Uri("clases/vista/Series.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void cbSecciones_SelectionChanged_Paquete(object sender, SelectionChangedEventArgs e)
        {
            if (estaInicializado())
            {
                ActualizarLista_Paquete();
            }

            //dgDireccion_Paquete.Items.Refresh();
        }

        private void cbCategoria_SelectionChanged_Paquete(object sender, SelectionChangedEventArgs e)
        {
            if (estaInicializado())
            {

                if (cbCategoria_Paquete.SelectedIndex !=-1)
                {
                    mngSI_CB_EtiquetasPaquete.tagAnteriorSeleccionado_Paquete = cbCategoria_Paquete.SelectedItem.ToString();
                }
                else {
                    mngSI_CB_EtiquetasPaquete.tagAnteriorSeleccionado_Paquete = "";
                }
                ActualizarLista_Paquete();
            }

            //dgDireccion_Paquete.Items.Refresh();
        }

        private void filtrarListaDireccionesPaqueteATravesDeTextoBuscar()
        {
            List<Direccion> ldP = direccionesPaquete;
            direccionesPaquete_Filtradas.Clear();
            if (tbbuscar_Paquete.Text.Length > 0)
            {
                string texto = tbbuscar.Text;



                ldP = direccionesPaquete.FindAll(d => d.id.Contains(texto) || (d.seleccionar + "").Contains(texto)
                   || d.nombre.Contains(texto) || d.unidad.Contains(texto));




            }


            ldP.ForEach(v => direccionesPaquete_Filtradas.Add(v));
            //dgDireccion_Paquete.Items.Refresh();
        }

        private void TextBox_KeyUp_Paquete(object sender, System.Windows.Input.KeyEventArgs e)
        {
            filtrarListaDireccionesPaqueteATravesDeTextoBuscar();

        }

        private void alApretar_B_EliminarTexto_Paquete(object sender, RoutedEventArgs e)
        {
            tbbuscar_Paquete.Text = "";
            filtrarListaDireccionesPaqueteATravesDeTextoBuscar();
            //dgDireccion_Paquete.ItemsSource = direcciones;
            //dgDireccion_Paquete.Items.Refresh();
        }

        private void alApretar_B_AbrirDialogo_Agregar_Paquete(object sender, RoutedEventArgs e)
        {
            if (cbSecciones_Paquete.SelectedIndex > 0)
            {
                CB_Secciones_AgregarDireccion_Paquete.SelectedIndex = cbSecciones_Paquete.SelectedIndex - 1;
            }
            var l = mngSI_CB_EtiquetasPaquete.getEtiquetas_List();//getEtiquetas_Paquete_List();
            B_EtiquetaPrincipal.Content = !l.Contains(TipoDeEtiquetaDeSerie.FINALIZADAS) ? TipoDeEtiquetaDeSerie.TX.nombreTag : TipoDeEtiquetaDeSerie.FINALIZADAS.nombreTag;
            l.RemoveAll(v => v == TipoDeEtiquetaDeSerie.TX || v == TipoDeEtiquetaDeSerie.FINALIZADAS);
            TipoDeEtiquetaDeSerie[] tags = l.ToArray();

            mngEtiquetasDialogoBuscar_DireccionesPaquete.ejecutar(tags);
            //if (cbCategoria_Paquete.SelectedIndex > -1) {
            //comboboxCategoria.SelectedIndex = cbCategoria.SelectedIndex - 1;
            //}
        }

        private void alApretar_B_BuscarCarpeta_Paquete(object sender, RoutedEventArgs e)
        {
            string dir = "";
            string key = "ultimoDir_Paquete";


            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            string dirAnterior = Referencia.manager.TS().getStr(key, "");
            if (dirAnterior.Length > 0)
            {
                dialog.SelectedPath = dirAnterior;
            }
            bool? success = dialog.ShowDialog();
            if (success == true)
            {
                this.cwl("Entro en agregar carpeta");

                dir = dialog.SelectedPath;
                Referencia.manager.TS().put(key, dir);

                direccionFile_Paquete.Text = dir;
                string[] arrsplit = dir.Split(new string[] { @"\" }, 20, System.StringSplitOptions.None);
                nombrecarpeta_Paquete.Text = arrsplit[arrsplit.Length - 1];
                nombreunidad_Paquete.Text = arrsplit[0];
            }
        }

        private void alApretar_B_Acepar_BuscarCarpeta_Paquete(object sender, RoutedEventArgs e)
        {
            if (direccionFile_Paquete.Text.Length == 0)
            {
                System.Windows.MessageBox.Show("Es necesario seleccionar una carpeta");
            }
            else if (CB_Secciones_AgregarDireccion_Paquete.SelectedIndex == -1)
            {
                System.Windows.MessageBox.Show("Es necesario seleccionar una Sección");
            }
            //else if (comboboxCategoria.SelectedIndex == -1)
            //{
            //    System.Windows.MessageBox.Show("Es necesario seleccionar una Categoría");
            //}
            else
            {
                ComboBoxItem cbi = (ComboBoxItem)CB_Secciones_AgregarDireccion_Paquete.SelectedItem;
                StackPanel sp = (StackPanel)cbi.Content;
                TextBlock tbseccion = (TextBlock)sp.Children[1];

                //ComboBoxItem cbc = (ComboBoxItem)comboboxCategoria.SelectedItem;
                //string _categoria = cbc.ContentStringFormat;

                List<TipoDeEtiquetaDeSerie> ltags = mngEtiquetasDialogoBuscar_DireccionesPaquete.getEtiquetasDestino();

                ltags.Add(B_EtiquetaPrincipal.Content.ToString() == "TX" ? TipoDeEtiquetaDeSerie.TX : TipoDeEtiquetaDeSerie.FINALIZADAS);

                //Direccion dir = new Direccion() { id = IdGenerate() + "", seleccionar = (bool)seleccionartb.IsChecked, nombre = nombrecarpeta.Text, direccion = direccionFile.Text, unidad = nombreunidad.Text, seccion = tbseccion.Text, categoria = _categoria };

                TipoDeSeccion tds = getSeccion_Paquete(1);
                //TipoDeCategoriaPropias tdc = getCategoria(1);
                Referencia.manager.agregarDireccionesDePaquete(
                    seccion: tds
                    , etiquetas: ltags.ToArray()
                    , url: direccionFile_Paquete.Text
                    , estaSeleccionada: (bool)seleccionartb.IsChecked
                    );


                if (tds==TipoDeSeccion.SERIES) {
                    Referencia.EA_Series.variables.datosDeRecarga.add(ltags);
                } else if (tds == TipoDeSeccion.ANIME) {
                    Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(ltags);
                }
                

                ActualizarLista_Paquete();

                

                dialoghost.IsOpen = false;

                Reset_Dlg_AgregarDireccionPaquete();
            }
        }

        private void alApretar_B_Cancelar_BuscarCarpeta_Paquete(object sender, RoutedEventArgs e)
        {
            Reset_Dlg_AgregarDireccionPaquete();
        }

        private void alApretar_B_Delete_Direccion_Paquete(object sender, RoutedEventArgs e)
        {
            if (dgDireccion_Paquete.SelectedIndex > -1)
            {
                var mens = System.Windows.MessageBox.Show("Está seguro que desea eliminar la fila?", "Confirmación", MessageBoxButton.YesNoCancel);
                if (mens == MessageBoxResult.Yes)
                {
                    Direccion dir = (Direccion)dgDireccion_Paquete.SelectedItem;
                    direccionesPaquete.Remove(dir);
                    direccionesPaquete_Filtradas.Remove(dir);
                    //dgDireccion_Paquete.Items.Refresh();

                    actualizarTags_CB_DireccionesPaquete(getlistaDireccionesPaquete_TodasEnSeccion());


                    DireccionDePaquete dp = Parse_A_DireccionDePaquete(dir);

                    Referencia.manager.eliminarDireccionesDePaquete(dp);


                    TipoDeSeccion tds = dp.seccion;
                    if (tds == TipoDeSeccion.SERIES)
                    {
                        Referencia.EA_Series.variables.datosDeRecarga.add(dp.etiquetas);
                    }
                    else if (tds == TipoDeSeccion.ANIME)
                    {
                        Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(dp.etiquetas);
                    }
                }
            }
            else System.Windows.MessageBox.Show("Debe seleccionar una fila de la tabla");
        }

        private void alApretar_B_DeleteTodo_Direccion_Paquete(object sender, RoutedEventArgs e)
        {
            foreach (var dir in direccionesPaquete_Filtradas)
            {

                DireccionDePaquete dp = Parse_A_DireccionDePaquete(dir);
                Referencia.manager.eliminarDireccionesDePaquete(dp);
                direccionesPaquete.Remove(dir);

                


                TipoDeSeccion tds = dp.seccion;
                if (tds == TipoDeSeccion.SERIES)
                {
                    Referencia.EA_Series.variables.datosDeRecarga.add(dp.etiquetas);
                }
                else if (tds == TipoDeSeccion.ANIME)
                {
                    Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(dp.etiquetas);
                }
            }

            //direccionesPaquete.Clear();
            direccionesPaquete_Filtradas.Clear();
            //dgDireccion_Paquete.Items.Refresh();

            actualizarTags_CB_DireccionesPaquete(getlistaDireccionesPaquete_TodasEnSeccion());
        }

        private void alApretar_B_ImportarDirecciones_Paquete(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "SQLite (*.acconf)|*.*";

            var result = ofd.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                MostrarDialogo();


                Referencia.manager.importarDireccionesPaquetes_Subproseso(new FileInfo(ofd.FileName), new EventosEnSubproceso(
                    () =>
                    {
                        dialoghost.IsOpen = false;

                        ActualizarLista_Paquete();

                        
                            Referencia.EA_Series.variables.datosDeRecarga.setActualizarTodasLasEtiquetas();
                        
                            Referencia.EA_SeriesAnimes.variables.datosDeRecarga.setActualizarTodasLasEtiquetas();
                        
                    },
                    ex =>
                    {

                    }
                    )
                    );

                

                //dgDireccion_Paquete.Items.Refresh();
            }
        }

        private void alApretar_B_ExportarDirecciones_Paquete(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "SQLite (*.acconf)|*.*";

            var result = sfd.ShowDialog();

            if (result.ToString() != string.Empty)
            {
                MostrarDialogo();

                Console.WriteLine(sfd.FileName);
                FileInfo f = new FileInfo(Archivos.setExtencionStr(sfd.FileName, ".acconf"));
                Referencia.manager.exportarDireccionesPaquete_Subproseso(f, new EventosEnSubproceso(
                    () =>
                    {
                        dialoghost.IsOpen = false;
                    },
                    ex =>
                    {

                    }
                    )
                    );


            }
        }

        private void TB_Direcciones_SelectTodo_Checked_Paquete(object sender, RoutedEventArgs e)
        {
            mngChBTodas_Paquete.alApretar_TB_MarcarTodos(true);
            //foreach (var element in direccionesPaquete_Filtradas)
            //{
            //    if (!element.getSeleccionar())
            //    {
            //        element.setSeleccionar(true);
            //        DireccionDePaquete dp = Parse_A_DireccionDePaquete(element);
            //        Referencia.manager.updateDireccionesDePaquete(dp);

            //        TipoDeSeccion tds = dp.seccion;
            //        if (tds == TipoDeSeccion.SERIES)
            //        {
            //            Referencia.EA_Series.variables.datosDeRecarga.add(dp.etiquetas);
            //        }
            //        else if (tds == TipoDeSeccion.ANIME)
            //        {
            //            Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(dp.etiquetas);
            //        }

            //    }

            //}

            //dgDireccion_Paquete.Items.Refresh();


        }

        private void TB_Direcciones_SelectTodo_Unchecked_Paquete(object sender, RoutedEventArgs e)
        {
            // ponerlosEnFalse_DireccionesdelPaquete();
            mngChBTodas_Paquete.alApretar_TB_MarcarTodos(false);
        }

        private void ChB_Direcciones_Select_Checked_Paquete(object sender, RoutedEventArgs e)
        {
            mngChBTodas_Paquete.Alseleccionar_Elemento();
            // Chequear_Paquete(true);
        }

        private void ChB_Direcciones_Select_Unchecked_Paquete(object sender, RoutedEventArgs e)
        {
            mngChBTodas_Paquete.Alseleccionar_Elemento();
            //Chequear_Paquete(false);
        }

        private void CB_Disco_AlCambiar_Paquete(object sender, SelectionChangedEventArgs e)
        {


           

            if (dgDireccion_Paquete.SelectedIndex > -1)
            {
                System.Windows.Controls.ComboBox cb = (System.Windows.Controls.ComboBox)sender;
                if (cb.SelectedItem != null)
                {
                    Referencia.manager.supr(getEV_Dlg(() => {
                        dgDireccion_Paquete.SelectedIndex = -1;
                        //dgDireccion_Paquete.Items.Refresh();
                    }), () => {
                        Direccion dir = (Direccion)dgDireccion_Paquete.SelectedItem;

                        //System.Windows.Controls.ComboBox cb = (System.Windows.Controls.ComboBox)sender;

                        string sj = cb.SelectedItem.ToString()[0] + dir.direccion.Substring(1);
                        dir.direccion = sj;

                        DireccionDePaquete dp = Parse_A_DireccionDePaquete(dir);
                        Referencia.manager.updateDireccionesDePaquete(dp);
                        dir.unidad = (string)cb.SelectedValue;

                        if (!Directory.Exists(dir.direccion))
                            dir.color_dir = Brushes.OrangeRed;
                        else dir.color_dir = Brushes.Black;




                        TipoDeSeccion tds = dp.seccion;
                        if (tds == TipoDeSeccion.SERIES)
                        {
                            Referencia.EA_Series.variables.datosDeRecarga.add(dp.etiquetas);
                        }
                        else if (tds == TipoDeSeccion.ANIME)
                        {
                            Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(dp.etiquetas);
                        }
                    });
                }

                    

                
            }
        }

        private void alApretar_B_CargarPaquete(object sender, RoutedEventArgs e)
        {
            string dir = "";
            string key = "ultimoDir_DireccionCarpetaPaquete_Paquete";


            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            string dirAnterior = Referencia.manager.TS().getStr(key, "");
            if (dirAnterior.Length > 0)
            {
                dialog.SelectedPath = dirAnterior;
            }
            bool? success = dialog.ShowDialog();
            if (success == true)
            {
                //this.cwl("Entro en agregar carpeta");

                dir = dialog.SelectedPath;
                Referencia.manager.TS().put(key, dir);

                direccionFile_Paquete.Text = dir;
                string[] arrsplit = dir.Split(new string[] { @"\" }, 20, System.StringSplitOptions.None);
                nombrecarpeta_Paquete.Text = arrsplit[arrsplit.Length - 1];
                nombreunidad_Paquete.Text = arrsplit[0];

                MostrarDialogo();
                Referencia.manager.cargarPaquete(new DirectoryInfo(dir), new EventosEnSubproceso(() =>
                {
                    ActualizarLista_Paquete();
                    dialoghost.IsOpen = false;

                    Referencia.EA_Series.variables.datosDeRecarga.setActualizarTodasLasEtiquetas();
                    Referencia.EA_SeriesAnimes.variables.datosDeRecarga.setActualizarTodasLasEtiquetas();

                }, ex =>
                {
                    this.cwl("errorrrr");
                }));
            }

        }

        //private void AlApretar_B_Buscar_Archivo_direcciones_Propias(object sender, RoutedEventArgs e)
        //{

        //}




        private void alApretar_B_BuscarDirecciones_Propias_TXT(object sender, RoutedEventArgs e)
        {
            string dir = "";

            OpenFileDialog openfile = new OpenFileDialog();

            //openfile.Filter = "SQLite (*.acconf)|*.*";

            string dirAnterior = Referencia.manager.TS().getStr("ultimoDir_TXT", "");
            if (dirAnterior.Length > 0)
            {
                openfile.InitialDirectory = dirAnterior;
            }
            var result = openfile.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                //direccionFile_TXT.Text = openfile.FileName;


                dir = openfile.FileName;
                try {
                    if (Archivos.esTXT(new FileInfo(dir)))
                    {
                        Referencia.manager.TS().put("ultimoDir_TXT", dir);

                        direccionFile_TXT.Text = dir;
                        string[] arrsplit = dir.Split(new string[] { @"\" }, 20, System.StringSplitOptions.None);
                        nombrecarpeta_TXT.Text = arrsplit[arrsplit.Length - 1];
                        nombreunidad_TXT.Text = arrsplit[0];
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Es necesario un archivo txt con los nombres de las series");
                    }
                } catch (Exception ex) {
                    System.Windows.MessageBox.Show("Es necesario un archivo txt correcto con los nombres de las series");
                }

                

                

            }



            //FolderBrowserDialog folderdialog = new FolderBrowserDialog();
            //var result = folderdialog.ShowDialog();
            //if (result.ToString() != string.Empty)
            //{
            //    dir = folderdialog.SelectedPath;
            //    direccionFile.Text = dir;

            //    string[] arrsplit = dir.Split(new string[] { @"\" }, 20, System.StringSplitOptions.None);
            //    nombrecarpeta.Text = arrsplit[arrsplit.Length - 1];
            //    nombreunidad.Text = arrsplit[0];
            //}

            //var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            //string dirAnterior = Referencia.manager.TS().getStr("ultimoDir", "");
            //if (dirAnterior.Length > 0)
            //{
            //    dialog.SelectedPath = dirAnterior;
            //}
            //bool? success = dialog.ShowDialog();
            //if (success == true)
            //{

            //    dir = dialog.SelectedPath;
            //    Referencia.manager.TS().put("ultimoDir", dir);

            //    direccionFile.Text = dir;
            //    string[] arrsplit = dir.Split(new string[] { @"\" }, 20, System.StringSplitOptions.None);
            //    nombrecarpeta.Text = arrsplit[arrsplit.Length - 1];
            //    nombreunidad.Text = arrsplit[0];
            //}
        }

        private void alApretar_B_Aceptar_AlSeleccionarUnaCarpeta_TXT(object sender, RoutedEventArgs e)
        {
            if (direccionFile_TXT.Text.Length == 0)
            {
                System.Windows.MessageBox.Show("Es necesario seleccionar una carpeta");
            }
            else if (comboboxSecciones_TXT.SelectedIndex == -1)
            {
                System.Windows.MessageBox.Show("Es necesario seleccionar una Sección");
            }
            else if (comboboxCategoria_TXT.SelectedIndex == -1)
            {
                System.Windows.MessageBox.Show("Es necesario seleccionar una Categoría");
            }
            else
            {
                ComboBoxItem cbi = (ComboBoxItem)comboboxSecciones_TXT.SelectedItem;
                StackPanel sp = (StackPanel)cbi.Content;
                TextBlock tbseccion = (TextBlock)sp.Children[1];

                ComboBoxItem cbc = (ComboBoxItem)comboboxCategoria_TXT.SelectedItem;
                string _categoria = cbc.ContentStringFormat;

                //Direccion dir = new Direccion() { id = IdGenerate() + "", seleccionar = (bool)seleccionartb.IsChecked, nombre = nombrecarpeta.Text, direccion = direccionFile.Text, unidad = nombreunidad.Text, seccion = tbseccion.Text, categoria = _categoria };

                TipoDeSeccion tds = getSeccion(2);
                TipoDeCategoriaPropias tdc = getCategoria(2);

                Referencia.manager.agregarDireccionPropia(tds, tdc, direccionFile_TXT.Text, (bool)seleccionartb.IsChecked);
                if (tds == TipoDeSeccion.SERIES)
                {
                    Referencia.EA_Series.variables.datosDeRecarga.add(tdc);
                }
                if (tds == TipoDeSeccion.ANIME)
                {
                    Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(tdc);
                }
                //cbSecciones.SelectedIndex = comboboxSecciones.SelectedIndex + 1;
                //cbCategoria.SelectedIndex = comboboxCategoria.SelectedIndex + 1;
                mngSI_CB_SeccionesPropias.selectIndex(comboboxSecciones_TXT.SelectedIndex + 1);
                mngSI_CB_CategoriasPropias.selectIndex(comboboxCategoria_TXT.SelectedIndex + 1);

                ActualizarLista();

                //dgDireccion.Items.Refresh();

                dialoghost.IsOpen = false;

                Reset_TXT();
            }

        }

        private void alApretar_B_Cancelar_DlgAgregarDireccion_Propia_TXT(object sender, RoutedEventArgs e)
        {
            Reset_TXT();
        }

        private void Reset_TXT()
        {
            direccionFile_TXT.Text = nombrecarpeta_TXT.Text = nombreunidad_TXT.Text = "";
            comboboxSecciones_TXT.SelectedIndex = comboboxCategoria_TXT.SelectedIndex = -1;
        }

        private void alArpetar_B_AbrirDilagoAgregarDireccion_Propia_TXT(object sender, RoutedEventArgs e)
        {
            if (cbSecciones.SelectedIndex > -1)
            {
                 comboboxSecciones_TXT.SelectedIndex = cbSecciones.SelectedIndex - 1;
                //mngSI_CB_SeccionesPropias.selectIndex(cbSecciones.SelectedIndex - 1);
            }
            if (cbCategoria.SelectedIndex > -1)
            {
                comboboxCategoria_TXT.SelectedIndex = cbCategoria.SelectedIndex - 1;
                //mngSI_CB_CategoriasPropias.selectIndex(cbCategoria.SelectedIndex - 1);
            }
        }

        private void alEscribir_TB_DireccionReproductor(object sender, System.Windows.Input.KeyEventArgs e)
        {
            UtilesWPF.ponerColorExisteArchivo(TB_DireccionReproductorDeVideo
                , f => Referencia.urlReproductorDeVideo = f.ToString()
                , f => Archivos.getExtencion(f).ToLower() == ".exe"
                );
        }

        

        private void alApretar_B_BuscarDireccionReproductor(object sender, RoutedEventArgs e)
        {

            UtilesWPF.buscarArchivo(
                () => Referencia.manager.TS().getStr("ultimoDir_Reproductor", "")
                , f =>
                {
                    string dir = f.ToString();
                    Referencia.manager.TS().put("ultimoDir_Reproductor", dir);

                    TB_DireccionReproductorDeVideo.Text = dir;

                    Referencia.urlReproductorDeVideo = dir;

                }
                , ".exe"
                );
        }

        private void AlApretar_B_BuscarRestearDireccionReproductorDeVideo(object sender, RoutedEventArgs e)
        {
            UtilidadesActualize.resetearDireccionDeVideo();
            Referencia.manager.TS().put("ultimoDir_Reproductor", Referencia.urlReproductorDeVideo);
            TB_DireccionReproductorDeVideo.Text = Referencia.urlReproductorDeVideo;
            //            =
        }

        private void alApretar_True_CB_UtilizarElReproductorSeleccionado(object sender, RoutedEventArgs e)
        {
            AlChekear_CB_UtilizarElReproductorSeleccionado(true);
        }

        private void alApretar_False_CB_UtilizarElReproductorSeleccionado(object sender, RoutedEventArgs e)
        {
            AlChekear_CB_UtilizarElReproductorSeleccionado(false);
        }
        private void AlChekear_CB_UtilizarElReproductorSeleccionado(bool selected) {
            if (seInicializo) {
                Referencia.manager.TS().put("cb_UtilizarElReproductorSeleccionado", selected);
            }
            
        }


        public void inicializar()
        {
            direcciones = new List<Direccion>();

            mngSI_CB_CategoriasPropias = new ManagerSelectionIndex(cbCategoria_SelectionChanged, cbCategoria);
            mngSI_CB_SeccionesPropias = new ManagerSelectionIndex(cbSecciones_SelectionChanged, cbSecciones);

            direcciones_filtradas = new ObservableCollection<Direccion>();
            dgDireccion.ItemsSource = direcciones_filtradas;

            Action<Direccion> upadateDireccionPropia = dir => {
                DireccionDeActualizadorPropia dap = Parse(dir);
                Referencia.manager.updateDireccionesPropias(dap);
                TipoDeSeccion tds = TipoDeSeccion.get(dir.seccion);
                TipoDeCategoriaPropias tdc = TipoDeCategoriaPropias.get(dir.categoria);

                if (tds == TipoDeSeccion.SERIES)
                {
                    Referencia.EA_Series.variables.datosDeRecarga.add(tdc);
                }
                else if (tds == TipoDeSeccion.ANIME)
                {
                    Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(tdc);
                }
            };

            mngChBTodas_Propias = new MangerColumnaChB<Direccion>(direcciones,dgDireccion, tbdirecciones, Direccion.getNombreProperty_seleccionar()
                ,e=> {
                    if (e != null) {
                        upadateDireccionPropia(e);


                    } else {

                        Dictionary<int, DireccionDeActualizadorPropia> dh_bd = new Dictionary<int, DireccionDeActualizadorPropia>();
                        getlistr().ForEach(v => { if (v.id != null) dh_bd.Add(v.id ?? -1, v); });

                        Dictionary<int, Direccion> dh = new Dictionary<int, Direccion>();
                        direcciones.ForEach(v => { if (v.id != null && v.id.Trim().Length > 0) dh.Add(Utiles.inT(v.id), v); });

                        foreach (KeyValuePair<int, Direccion> par in dh)
                        {
                            int id = par.Key;
                            Direccion dir = par.Value;
                            if (dh_bd.ContainsKey(id))
                            {
                                DireccionDeActualizadorPropia da = dh_bd[id];
                                if (da.seleccioniada != dir.seleccionar)
                                {
                                    upadateDireccionPropia(dir);
                                    //Referencia.manager.updateDireccionesPropias(Parse(dir));


                                    //TipoDeSeccion tds = TipoDeSeccion.get(dir.seccion);
                                    //TipoDeCategoriaPropias tdc = TipoDeCategoriaPropias.get(dir.categoria);

                                    //if (tds == TipoDeSeccion.SERIES)
                                    //{
                                    //    Referencia.EA_Series.variables.datosDeRecarga.add(tdc);
                                    //}
                                    //else if (tds == TipoDeSeccion.ANIME)
                                    //{
                                    //    Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(tdc);
                                    //}
                                }
                            }
                        }
                    }

                    

                    
                });

            

            inicializarSeccionPaquete();


        }

        public void actualizar()
        {
            mngSI_CB_SeccionesPropias.selectIndex(Referencia.EA_Configuracion.index_cb_secciones_configuracion);
            if (Referencia.EA_Configuracion.aplicar_indice_categoria_propia && Referencia.EA_Configuracion.indice_categoria_propia > 1)
            {
                mngSI_CB_CategoriasPropias.selectIndex(Referencia.EA_Configuracion.indice_categoria_propia);
            }
            ActualizarLista();


            if (Referencia.EA_Configuracion.aplicar_indice_etiquetas_paquete && Referencia.EA_Configuracion.etiquetas_paquete != null)
            {
                TabConfigu.SelectedIndex = 1;
                mngSI_CB_EtiquetasPaquete.tagAnteriorSeleccionado_Paquete = UtilidadesActualize.getEtiquetasEnStr(Referencia.EA_Configuracion.etiquetas_paquete.etiquetas);
                Referencia.EA_Configuracion.aplicar_indice_etiquetas_paquete = false;
            }
            else {
                TabConfigu.SelectedIndex = 0;
            }
            mngSI_CB_SeccionesPaquete.selectIndex(Referencia.EA_Configuracion.index_cb_secciones_configuracion);
            ActualizarLista_Paquete();

            TB_DireccionReproductorDeVideo.Text = Referencia.urlReproductorDeVideo;
            CB_UtilizarElReproductorSeleccionado.IsChecked = Referencia.EA_Configuracion.utilizar_reproductor_seleccionado;
        }

        private void inicializarSeccionPaquete()
        {
            mngSI_CB_EtiquetasPaquete = new ManagerSelectionIndex_CB_Etiquetas(cbCategoria_SelectionChanged_Paquete, cbCategoria_Paquete);



            mngSI_CB_SeccionesPaquete = new ManagerSelectionIndex(cbSecciones_SelectionChanged_Paquete, cbSecciones_Paquete);

            direccionesPaquete = new List<Direccion>();
            direccionesPaquete_Filtradas = new ObservableCollection<Direccion>();
            dgDireccion_Paquete.ItemsSource = direccionesPaquete_Filtradas;

            Action<Direccion> upadateDireccionPaquete = dir => {
                DireccionDePaquete dp = Parse_A_DireccionDePaquete(dir);
                Referencia.manager.updateDireccionesDePaquete(dp);


                TipoDeSeccion tds = dp.seccion;
                if (tds == TipoDeSeccion.SERIES)
                {
                    Referencia.EA_Series.variables.datosDeRecarga.add(dp.etiquetas);
                }
                else if (tds == TipoDeSeccion.ANIME)
                {
                    Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(dp.etiquetas);
                }
            };

            mngChBTodas_Paquete = new MangerColumnaChB<Direccion>(direccionesPaquete, dgDireccion_Paquete, tbdirecciones_Paquete, Direccion.getNombreProperty_seleccionar()
                , e => {
                    if (e != null)
                    {
                        upadateDireccionPaquete(e);


                    }
                    else
                    {
                        Dictionary<int, DireccionDePaquete> dh_bd = new Dictionary<int, DireccionDePaquete>();
                        getlistaDireccionesPaquete().ForEach(v => { if (v.id != null) dh_bd.Add(v.id ?? -1, v); });

                        Dictionary<int, Direccion> dh = new Dictionary<int, Direccion>();
                        direccionesPaquete.ForEach(v => { if (v.id != null && v.id.Trim().Length > 0) dh.Add(Utiles.inT(v.id), v); });

                        foreach (KeyValuePair<int, Direccion> par in dh)
                        {
                            int id = par.Key;
                            Direccion dir = par.Value;
                            if (dh_bd.ContainsKey(id))
                            {
                                DireccionDePaquete da = dh_bd[id];
                                if (da.seleccioniada != dir.seleccionar)
                                {
                                    upadateDireccionPaquete(dir);
                                    //DireccionDePaquete dp = Parse_A_DireccionDePaquete(dir);
                                    //Referencia.manager.updateDireccionesDePaquete(dp);


                                    //TipoDeSeccion tds = dp.seccion;
                                    //if (tds == TipoDeSeccion.SERIES)
                                    //{
                                    //    Referencia.EA_Series.variables.datosDeRecarga.add(dp.etiquetas);
                                    //}
                                    //else if (tds == TipoDeSeccion.ANIME)
                                    //{
                                    //    Referencia.EA_SeriesAnimes.variables.datosDeRecarga.add(dp.etiquetas);
                                    //}
                                }
                            }
                        }
                    }



                });


            

            mngEtiquetasDialogoBuscar_DireccionesPaquete = new ManagerEtiquetasDialogoBuscar(LB_NoSeleccionado_EtiquetaSerie, LB_Seleccionado_EtiquetaSerie);
            mngEtiquetasDialogoBuscar_DireccionesPaquete.ejecutar(
                StringsAListaOrigen: new string[] { }
                , StringsAListaDestino: new string[] { }
                );

        }


        //string dir = "";

        //OpenFileDialog openfile = new OpenFileDialog();

        //openfile.Filter = "SQLite (*.acconf)|*.*";

        //string dirAnterior = Referencia.manager.TS().getStr("ultimoDir_TXT", "");
        //if (dirAnterior.Length > 0)
        //{
        //    openfile.InitialDirectory = dirAnterior;
        //}
        //var result = openfile.ShowDialog();
        //if (result.ToString() != string.Empty)
        //{
        //    //direccionFile_TXT.Text = openfile.FileName;


        //    dir = openfile.FileName;
        //    try
        //    {
        //        if (Archivos.esTXT(new FileInfo(dir)))
        //        {
        //            Referencia.manager.TS().put("ultimoDir_TXT", dir);

        //            direccionFile_TXT.Text = dir;
        //            string[] arrsplit = dir.Split(new string[] { @"\" }, 20, System.StringSplitOptions.None);
        //            nombrecarpeta_TXT.Text = arrsplit[arrsplit.Length - 1];
        //            nombreunidad_TXT.Text = arrsplit[0];
        //        }
        //        else
        //        {
        //            System.Windows.MessageBox.Show("Es necesario un archivo txt con los nombres de las series");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.MessageBox.Show("Es necesario un archivo txt correcto con los nombres de las series");
        //    }




        //}
    }







    public class Direccion: ViewModelBase
    {

        private bool _seleccionar;
        private string _id;
        private string _nombre;
        private Brush _color_dir;
        private string _direccion;
        private string _unidad;
        private List<string> _unidades;
        private string _seccion;
        private string _categoria;
        public bool seleccionar
        {
            get
            {
                return _seleccionar;
            }
            set
            {
                _seleccionar = value;
                OnPropertyChanged(nameof(seleccionar));
            }
        }
        public string id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(id));
            }
        }
        public string nombre
        {
            get
            {
                return _nombre;
            }
            set
            {
                _nombre = value;
                OnPropertyChanged(nameof(nombre));
            }
        }
        public Brush color_dir
        {
            get
            {
                return _color_dir;
            }
            set
            {
                _color_dir = value;
                OnPropertyChanged(nameof(color_dir));
            }
        }
        public string direccion
        {
            get
            {
                return _direccion;
            }
            set
            {
                _direccion = value;
                OnPropertyChanged(nameof(direccion));
            }
        }
        public string unidad
        {
            get
            {
                return _unidad;
            }
            set
            {
                _unidad = value;
                OnPropertyChanged(nameof(unidad));
            }
        }
        public List<string> unidades
        {
            get
            {
                return _unidades;
            }
            set
            {
                _unidades = value;
                OnPropertyChanged(nameof(unidades));
            }
        }
        public string seccion
        {
            get
            {
                return _seccion;
            }
            set
            {
                _seccion = value;
                OnPropertyChanged(nameof(seccion));
            }
        }
        public string categoria
        {
            get
            {
                return _categoria;
            }
            set
            {
                _categoria = value;
                OnPropertyChanged(nameof(categoria));
            }
        }


        public static string getNombreProperty_seleccionar() {
            return nameof(seleccionar);
        }
        //public bool seleccionar
        //{
        //    get { return _seleccionar; }
        //    set { _seleccionar = value; }
        //}

        //public void setSeleccionar(bool selec)
        //{
        //    _seleccionar = selec;
        //}
        //public bool getSeleccionar()
        //{
        //    return _seleccionar;
        //}
    }
}

//private void setIntercambioList(System.Windows.Controls.ListBox lb0, System.Windows.Controls.ListBox lb1, System.Windows.Controls.Button Bclear, params string[] etiquetas)
//{
//    Action resetear = () => { lb0.Items.Clear(); this.addStrings(lb0, etiquetas); lb1.Items.Clear(); };
//    resetear();
//    Action<System.Windows.Controls.ListBox, System.Windows.Controls.ListBox> seEventoIntercambio = (a, b) =>
//    {
//        a.MouseUp += (s, e) =>
//        {
//            if (a.SelectedIndex > -1)
//            {
//                b.Items.Add(a.SelectedItem);
//                a.Items.RemoveAt(a.SelectedIndex);
//            }
//        };
//    };
//    seEventoIntercambio(lb0, lb1);
//    seEventoIntercambio(lb1, lb0);
//    Bclear.Click += (s, e) => resetear();



//}


//private void actualizarTags_CB_DireccionesPaquete(List<Direccion> todasLasDireccionesDeLaSeccion) {

//    SortedSet<string> hsTags = new SortedSet<string>();
//    foreach (Direccion dir in todasLasDireccionesDeLaSeccion)
//    {
//        //string tags = "";

//        //foreach (string tag in Utiles.split(dir.categoria, " ").OrderBy(v => v))
//        //{
//        //    tags += (tag.Length > 0 ? " " : "") + tag;
//        //}

//        //hsTags.Add(tags);
//        hsTags.Add(dir.categoria);
//    }
//    //cbCategoria_Paquete.Items.Clear();
//    mngSI_CB_EtiquetasPaquete.clear(); 
//    //this.cwl("cantidad:"+hsTags.Count());


//    int i = 0;
//    bool hayQueSeleccionar = false;
//    int indiceEnElQueSeleccionar = -1;
//    foreach (string tag in hsTags)
//    {

//        cbCategoria_Paquete.Items.Add(tag);
//        if (tagAnteriorSeleccionado_Paquete != null&&tag== tagAnteriorSeleccionado_Paquete) {
//            hayQueSeleccionar = true;
//            indiceEnElQueSeleccionar = i;
//        }
//        i++;
//    }
//    if (hayQueSeleccionar) {
//        mngSI_CB_EtiquetasPaquete.selectIndex(indiceEnElQueSeleccionar);
//        //cbCategoria_Paquete.SelectedIndex = indiceEnElQueSeleccionar;
//    }
//}
//private List<TipoDeEtiquetaDeSerie> getEtiquetas_Paquete_List()
//{
//    List<TipoDeEtiquetaDeSerie> Ltdc = new List<TipoDeEtiquetaDeSerie>();

//    System.Windows.Controls.ComboBox cb = cbCategoria_Paquete;





//    if ( cb.SelectedIndex != -1)
//    {
//        foreach (string tag in Utiles.split(cb.SelectedItem.ToString(), " "))
//        {
//            TipoDeEtiquetaDeSerie tdc = TipoDeEtiquetaDeSerie.get(tag);
//            Ltdc.Add(tdc);
//        }


//    }




//    return Ltdc;
//}
//private TipoDeEtiquetaDeSerie[] getEtiquetas_Paquete() {
//    return getEtiquetas_Paquete_List().ToArray();
//}
//private TipoDeEtiquetaDeSerie[] getEtiquetas_DilagoAgregarCarpeta_Paquete()
//{
//    List<TipoDeEtiquetaDeSerie> Ltdc = new List<TipoDeEtiquetaDeSerie>();
//    return Ltdc.ToArray();
//}
