using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Actuelice1.clases.util;
using System.Windows.Forms;
using ReneUtiles;
using RelacionadorDeSerie;
using RelacionadorDeSerie.Representaciones;
//using System.IO;
using ReneUtiles.Clases.WPF;
using ReneUtiles.Clases.Subprocesos;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ReneUtiles.Clases.Multimedia.Series.Procesadores.Buscadores.Datos;
using ReneUtiles.Clases.Almacenamiento;
using ReneUtiles.Clases.Tipos;
using Actuelice1.clases.vista.Series;
using Delimon.Win32.IO;
namespace Actuelice1.clases.vista
{
    /// <summary>
    /// Interaction logic for Series.xaml
    /// </summary>
    public partial class Series_Vista : Page
    {
        //ConjuntoDeEtiquetasDeSerie variables;



        //public event PropertyChangedEventHandler PropertyChanged;

        //public void OnPropertyChanged(string nombrePropiedad)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombrePropiedad));
        //}


        ManagerSelectionIndex mngSI_CB_Categoria_General_Propias;
        ManagerSelectionIndex mngSI_CB_Categoria_Detalles_Propias;
        ManagerSelectionIndex mngSI_CB_Categoria_Paquetes;
        ManagerSelectionIndex mngSI_CB_TX_o_FINALIZADAS_Paquetes;
        ManagerSelectionIndex_CB_Etiquetas mngSI_CB_Etiquetas_Paquetes;
        TipoDeSeccion seccionActual = null;

        public bool seInicio = false;

        private ConjuntoDeVariables_VistaSerie cnv;

        public Series_Vista()
        {
            InitializeComponent();



            //bool hayQueCargar = false;
            //if (Referencia.EA_Series.variables == null)
            //{
            //    Referencia.EA_Series.variables = new ConjuntoDeVariables_VistaSerie();

            //    //hayQueCargar = true;
            //}

            //cnv = Referencia.EA_Series.variables;




            //Loaded += (o, v) =>
            //{
            //    //this.cwl();
            //    //cargarSeriesDeSerNecesario();
            //    //this.cwl();
            //    cargar_TodasLasSeries();
            //};
            //cargar_TodasLasSeries();

            //Vis
        }



        private void Series_Vista_Loaded(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void alApretar_B_Configuracion_Series_Generales_Propias(object sender, System.Windows.RoutedEventArgs e)
        {
            //Referencia.EA_Configuracion.index_cb_secciones_configuracion = seccionActual==TipoDeSeccion.SERIES?1:2;
            //Referencia.EA_Configuracion.aplicar_indice_categoria_propia = true;
            //Referencia.EA_Configuracion.indice_categoria_propia = cbCategoriaSG.SelectedIndex;
            //irAConfiguracion();

            //alApretar_B_Actualizar_Series_Generales_Propias
            alApretar_B_Configuracion_Series_Propias(cbCategoriaSG);
        }



        private void alEscribir_Buscar_Series_General(object sender, System.Windows.Input.KeyEventArgs e)
        {
            filtrarLista_Series_Propias_Generales_ATravesDeTextoBuscar();

        }

        private void tbbuscardetalle_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            filtrarLista_Series_Propias_Detalles_ATravesDeTextoBuscar();
            //if (tbbuscardetalle.Text.Length > 0)
            //{
            //    string texto = tbbuscardetalle.Text;
            //    series_detalle_Filtradas.Clear();

            //    series_detalle_Filtradas = seriesdetalle.FindAll(sd => sd.id.Contains(texto)
            //    || sd.nombres_serie.Contains(texto) || sd.nombre_capitulo.Contains(texto)
            //    || sd.temporada.Contains(texto));

            //    dgdetalle.ItemsSource = series_detalle_Filtradas;
            //    dgdetalle.Items.Refresh();
            //}
            //else
            //{
            //    dgdetalle.ItemsSource = seriesdetalle;
            //    dgdetalle.Items.Refresh();
            //}
        }
        private void alApretar_B_BorrarTextoFiltrar(System.Windows.Controls.TextBox tb, params Action[] _filtrar)
        {
            if (tb.Text.Trim().Length > 0)
            {
                tb.Text = "";
                foreach (var item in _filtrar)
                {
                    item();
                }

            }

        }

        private void alApretar_B_BorrarTextoFiltrar_Series_Generales_Propias(object sender, RoutedEventArgs e)
        {
            alApretar_B_BorrarTextoFiltrar(tbbuscargeneral, filtrarLista_Series_Propias_Generales_ATravesDeTextoBuscar);
            // tbbuscargeneral.Text = "";
            //lbgeneral.ItemsSource = series_generales;
            //lbgeneral.Items.Refresh();
        }

        private void alApretar_B_BorrarTextoFiltrar_Series_Detalles_Propias(object sender, RoutedEventArgs e)
        {
            alApretar_B_BorrarTextoFiltrar(tbbuscardetalle, filtrarLista_Series_Propias_Detalles_ATravesDeTextoBuscar);
            //tbbuscardetalle.Text = "";
            //dgdetalle.ItemsSource = seriesdetalle;
            //dgdetalle.Items.Refresh();
        }

        private void alApretar_B_Configuracion_Series_Detalles_Propias(object sender, RoutedEventArgs e)
        {
            //Referencia.EA_Configuracion.index_cb_secciones_configuracion = 1;

            //Referencia.frame.Navigate(new Uri("clases/vista/Configuracion.xaml", UriKind.RelativeOrAbsolute));
            alApretar_B_Configuracion_Series_Propias(cbCategoriaSD);
        }

        private void alApretar_B_Actualizar_Series_Detalles_Propias(object sender, RoutedEventArgs e)
        {

            alApretar_B_Actualizar_Propias(cbCategoriaSD);

        }
        //private void Button_Click_4(object sender, RoutedEventArgs e)
        //{
        //    //cargar_TodasLasSeries();
        //    //actualizarListas_Series();
        //    //dialoghost1.IsOpen = true;
        //    //Referencia.manager.actualizar(TipoDeSeccion.SERIES, new EventosEnSubproceso(
        //    //        () =>
        //    //        {
        //    //            ActualizarListaDetalle();
        //    //            dialoghost1.IsOpen = false;
        //    //        },
        //    //        ex =>
        //    //        {
        //    //            Console.WriteLine("jdkjdjkdkjdjk");
        //    //        }
        //    //        ));



        //}

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            var result = sfd.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                System.Windows.MessageBox.Show(sfd.FileName);
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            var result = sfd.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                System.Windows.MessageBox.Show(sfd.FileName);
            }
        }

        private void alApretar_B_Actualizar_Series_Generales_Propias(object sender, RoutedEventArgs e)
        {
            alApretar_B_Actualizar_Propias(cbCategoriaSG);
            //if (cbCategoriaSG.SelectedIndex > 0)
            //{
            //    cargar_LasCategoriasNecesarias(TipoDeCategoriaPropias.getNewHashSet(getCategoria(0)));
            //}
            //else {
            //    cargar_TodasLasCategorias();
            //}


        }






        public TipoDeCategoriaPropias getCategoria(int pos = 0)
        {
            TipoDeCategoriaPropias tdc = null;

            System.Windows.Controls.ComboBox cb = null;


            if (pos == 0)
            {
                cb = cbCategoriaSG;
            }
            else if (pos == 1)
            {
                cb = cbCategoriaSD;
            }

            if (cb.SelectedIndex != 0 && cb.SelectedIndex != -1)
            {
                tdc = TipoDeCategoriaPropias.VALUES[cb.SelectedIndex - 1];
            }
            return tdc;
        }
        public void MostrarDialogo()
        {
            //StackPanel sp = new StackPanel();
            //System.Windows.Controls.ProgressBar pb = new System.Windows.Controls.ProgressBar();

            //pb.Width = 100;
            //pb.Height = 30;

            //sp.Children.Add(pb);

            //dialoghost.DialogContent = sp;
            dialoghost.IsOpen = true;

            //dialoghost1.DialogContent = sp;
            //dialoghost1.IsOpen = true;
        }

        private void chb2_Checked(object sender, RoutedEventArgs e)
        {
            chb1.IsChecked = false;
        }

        private void chb1_Checked(object sender, RoutedEventArgs e)
        {
            chb2.IsChecked = false;
        }
        public TipoDeFiltroPropio? getTipoFiltro()
        {
            return getTipoFiltro(cbfiltros, chb1, chb2);
            //cbfiltros
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            //dialoghost1.IsOpen = false;
        }



        //--------------------------------Rene ----1 ----------------------------------------
        public void inicializar(TipoDeSeccion seccion)
        {
            this.seccionActual = seccion;
            cnv = seccion == TipoDeSeccion.SERIES ? Referencia.EA_Series.variables : Referencia.EA_SeriesAnimes.variables;
            Inicializar_Parte_SeriesPropias();
            Inicializar_Parte_Paquete();




            seInicio = true;
        }

        public void actualizar()//TipoDeSeccion seccion
        {

            cargarSeriesDeSerNecesario();
        }
        private void alApretar_B_Configuracion_Series_Propias(System.Windows.Controls.ComboBox cb)
        {
            Referencia.EA_Configuracion.index_cb_secciones_configuracion = seccionActual == TipoDeSeccion.SERIES ? 1 : 2;
            Referencia.EA_Configuracion.aplicar_indice_categoria_propia = true;
            Referencia.EA_Configuracion.aplicar_indice_etiquetas_paquete = false;
            Referencia.EA_Configuracion.indice_categoria_propia = cb.SelectedIndex;
            irAConfiguracion();
        }
        private void alApretar_B_Actualizar_Propias(System.Windows.Controls.ComboBox cb)
        {
            if (cb.SelectedIndex > 0)
            {
                cargar_LasCategoriasNecesarias(TipoDeCategoriaPropias.getNewHashSet(getCategoria(cb == cbCategoriaSG ? 0 : 1)));
            }
            else
            {
                cargar_TodasLasCategorias();
            }


        }
        private void irAConfiguracion()
        {
            //Referencia.frame.Navigate(new Uri("clases/vista/Configuracion.xaml", UriKind.RelativeOrAbsolute));
            Referencia.ventana_Configuracion.actualizar();
            Referencia.showFrame(Referencia.ventana_Configuracion);
        }
        public TipoDeFiltroPropio? getTipoFiltro(
            System.Windows.Controls.ComboBox cb_Faltantes
            , System.Windows.Controls.CheckBox chb_Faltantes
            , System.Windows.Controls.CheckBox chb_UltimosCapitulos)
        {
            TipoDeFiltroPropio? tfp = null;

            if (chb_Faltantes.IsChecked == true && cb_Faltantes.SelectedIndex > -1)
            {
                if (cb_Faltantes.SelectedIndex == 0)
                {
                    tfp = TipoDeFiltroPropio.FALTANTES_TODO;
                }
                else
                {
                    tfp = TipoDeFiltroPropio.FALTANTES_ULTIMO_TRAMO;
                }
            }
            else if (chb_UltimosCapitulos.IsChecked == true)
            {
                tfp = TipoDeFiltroPropio.ULTIMOS_CAPITULOS;
            }

            return tfp;
        }

        public TipoDeFiltroPropio? getTipoFiltro_Paquete()
        {
            return getTipoFiltro(cb_Filtros_Faltantes_Paquetes, chb_Filtro_Faltantes_Paquetes, chb_Filtro_UltimosCapitulos_Paquetes);
        }
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

        public void Inicializar_Parte_SeriesPropias()
        {
            mngSI_CB_Categoria_General_Propias = new ManagerSelectionIndex(alCambiar_CB_Categoria_General_Propias, cbCategoriaSG);
            mngSI_CB_Categoria_Detalles_Propias = new ManagerSelectionIndex(alCambiar_CB_Categoria_Detalles_Propias, cbCategoriaSD);


            lbgeneral.ItemsSource = cnv.series_general_Filtradas;
            dgdetalle.ItemsSource = cnv.series_detalle_Filtradas;

            tbbuscardetalle.Text = cnv.filtro_detalles_propia;
            tbbuscargeneral.Text = cnv.filtro_generales_propia;
            tb_buscarEnSeriesPaquete.Text = cnv.filtro_paquete;

        }


        public void Inicializar_Parte_Paquete()
        {
            //cnv.series_generales_Paquete = new List<SerieGeneral>();
            //cnv.series_generales_Filtrada_Paquete = new ObservableCollection<SerieGeneral>();
            //cnv.seriesdetalle_Paquete = new List<SerieDetalle>();
            //cnv.seriesdetalle_Filtrada_Paquete = new ObservableCollection<SerieDetalle>();
            mngSI_CB_TX_o_FINALIZADAS_Paquetes = new ManagerSelectionIndex(alCambiar_CB_TX_O_Finalizada_Paquete, cb_TX_O_Finalizada_Paquete);
            mngSI_CB_TX_o_FINALIZADAS_Paquetes.selectIndex(1);
            mngSI_CB_Categoria_Paquetes = new ManagerSelectionIndex(alCambiar_CB_Categoria_Paquete, cbCategoria_Paquete);
            mngSI_CB_Etiquetas_Paquetes = new ManagerSelectionIndex_CB_Etiquetas(alCambiar_CB_Etiquetas_Paquete, cbEtiquetasPaquete, cnv.tagsAnteriores_Paquete
                , () => { cnv.tagsAnteriores_Paquete = mngSI_CB_Etiquetas_Paquetes.tagsAnteriores; });


            lbgeneralPaquete.ItemsSource = cnv.series_generales_Filtrada_Paquete;
            dgdetallePaqueteSeries.ItemsSource = cnv.seriesdetalle_Filtrada_Paquete;

            mngSI_CB_Categoria_Paquetes.selectIndex(cnv.indice_categoria_paquete);
            mngSI_CB_Etiquetas_Paquetes.selectIndex(cnv.indice_etiqueta_paquete);

        }

        public void actualizarListas_Series()
        {
            actualizarListaDetalle_Propias();
            actualizarListaGeneral_Propias();
            actualizarListas_Paquetes();

        }


        private void cargarSeriesDeSerNecesario()
        {

            DatosARecargar_Series dr = cnv.datosDeRecarga;
            if (dr.hayQueCargarSeries)
            {
                if (dr.hayQueRecargarTodasLasCategorias && dr.hayQueRecargarTodasLasEtiquetas)
                {
                    cargar_TodasLasSeries();
                }
                else
                {
                    if (dr.hayQueRecargarTodasLasCategorias)
                    {
                        cargar_TodasLasCategorias();
                    }
                    else if (dr.hayQueRecargarCategorias())
                    {
                        cargar_LasCategoriasNecesarias(cnv.datosDeRecarga.categoriasARecargar);
                    }


                    if (dr.hayQueRecargarTodasLasEtiquetas)
                    {
                        cargar_TodasLasSeriesDelPaquete();
                    }
                    else if (dr.hayQueRecargarEtiquetas())
                    {
                        cargar_LasEtiquetasNecesarias(cnv.datosDeRecarga.etiquetasARecargar.ToArray());
                    }

                }









                dr.resetear();
            }
            else
            {
                //dgdetalle.Items.Refresh();
                //lbgeneral.Items.Refresh();
                //dgdetallePaqueteSeries.Items.Refresh();
                //lbgeneralPaquete.Items.Refresh();


            }
        }


        public void cargar_TodasLasSeries()
        {
            MostrarDialogo();
            Referencia.manager.actualizarSeries_EnPaquete_Y_Propias(seccionActual, getEV_Dlg(
                () =>
                {
                    this.cwl("se llamo!!!!!!!!!!1");
                    actualizarTags_CB_Paquete();
                    actualizarListas_Series();




                }
                ));
        }


        public void cargar_LasEtiquetasNecesarias(params ConjuntoDeEtiquetasDeSerie[] etiquetas)
        {
            MostrarDialogo();
            Referencia.manager.actualizarSeries_EnPaquete(seccionActual
                , etiquetas//cnv.datosDeRecarga.etiquetasARecargar.ToArray()
                , getEV_Dlg(
                    () =>
                    {
                        actualizarTags_CB_Paquete();
                        actualizarListas_Paquetes();
                    })

                    );
        }

        public void cargar_LasCategoriasNecesarias(HashSet<TipoDeCategoriaPropias> categoriasARecargar)
        {
            MostrarDialogo();
            Referencia.manager.actualizar_CategoriasPropias(seccionActual,
                categoriasARecargar//cnv.datosDeRecarga.categoriasARecargar
                , getEV_Dlg(
               () =>
               {
                   actualizarListas_Series();



               }
               ));
        }

        public void cargar_TodasLasCategorias()
        {
            MostrarDialogo();
            Referencia.manager.actualizar(seccionActual, getEV_Dlg(
               () =>
               {
                   actualizarListas_Series();



               }
               ));
        }

        public void cargar_TodasLasSeriesDelPaquete()
        {
            MostrarDialogo();
            Referencia.manager.actualizarSeries_EnPaquete(seccionActual, getEV_Dlg(
                    () =>
                    {
                        actualizarTags_CB_Paquete();
                        actualizarListas_Paquetes();
                    }));
        }

        public void actualizarListas_Paquetes()
        {
            actualizarListaDetalle_Paquete();
            actualizarListaGeneral_Paquete();

        }

        private void actualizarTags_CB_Paquete()
        {
            bool? tx = null;
            if (cb_TX_O_Finalizada_Paquete.SelectedIndex != 0)
            {
                tx = cb_TX_O_Finalizada_Paquete.SelectedIndex == 1;
            }
            mngSI_CB_Etiquetas_Paquetes.actualizarTags(Referencia.manager.getEtiquetasDeSerie_EnPaquete(
                seccionActual, tx
                ), v => v.etiquetas);
        }
        public bool estaSelecionadoCategoriaExtrenosPaquete()
        {

            return cbCategoria_Paquete.SelectedIndex == 1;
        }
        public TipoDeCategoriaPropias getCategoria_Paquete()
        {
            if (estaSelecionadoCategoriaExtrenosPaquete())
            {
                return null;
            }

            TipoDeCategoriaPropias tdc = null;

            System.Windows.Controls.ComboBox cb = cbCategoria_Paquete;




            if (cb.SelectedIndex > 1)
            {
                tdc = TipoDeCategoriaPropias.VALUES[cb.SelectedIndex - 2];
            }
            return tdc;
        }
        private void actualizarLista(ItemsControl ic)
        {
            //ic.Items.Refresh();
        }

        public void actualizarListaDetalle_Paquete()
        {
            TipoDeCategoriaPropias tcp = getCategoria_Paquete();
            TipoDeFiltroPropio? tipoDeFiltro = getTipoFiltro_Paquete();

            List<TipoDeEtiquetaDeSerie> lt = mngSI_CB_Etiquetas_Paquetes.getEtiquetas_List();
            if (cb_TX_O_Finalizada_Paquete.SelectedIndex != 0)
            {
                lt.Add(cb_TX_O_Finalizada_Paquete.SelectedIndex == 1 ? TipoDeEtiquetaDeSerie.TX : TipoDeEtiquetaDeSerie.FINALIZADAS);
            }
            TipoDeEtiquetaDeSerie[] tags = lt.ToArray();
            List<Capitulo_R> lcr;
            cnv.seriesdetalle_Paquete.Clear();
            if (estaSelecionadoCategoriaExtrenosPaquete())
            {
                lcr = Referencia.manager.getCapitulos_Extrenos_DelPaquete(seccionActual, tipoDeFiltro, tags);

            }
            else
            {
                lcr = Referencia.manager.getCapitulos_EnPaquete(seccionActual, tcp, tipoDeFiltro, tags);
            }

            cnv.seriesdetalle_Paquete.AddRange(SerieDetalle.parse(lcr));
            filtrarLista_Series_Paquete_Detalles_ATravesDeTextoBuscar();

            //dgdetallePaqueteSeries.Items.Refresh(); //actualizarLista();
            actualizarLista(dgdetallePaqueteSeries);
        }

        public void actualizarListaGeneral_Paquete()
        {
            TipoDeFiltroPropio? tipoDeFiltro = getTipoFiltro_Paquete();

            TipoDeEtiquetaDeSerie[] tags = mngSI_CB_Etiquetas_Paquetes.getEtiquetas();
            TipoDeCategoriaPropias tcp = getCategoria_Paquete();
            List<Serie_R> lcr;
            cnv.series_generales_Paquete.Clear();
            if (estaSelecionadoCategoriaExtrenosPaquete())
            {
                lcr = Referencia.manager.getSeries_Extrenos_EnPaquete(seccionActual, tipoDeFiltro, tags);

            }
            else
            {
                lcr = Referencia.manager.getSeries_EnPaquete(seccionActual, tcp, tipoDeFiltro, tags);
            }



            cnv.series_generales_Paquete.AddRange(SerieGeneral.parse(lcr));
            filtrarLista_Series_Paquete_Generales_ATravesDeTextoBuscar();
            lbgeneralPaquete.Items.Refresh();
            actualizarLista(lbgeneralPaquete);
        }



        public void actualizarListaDetalle_Propias()
        {
            TipoDeCategoriaPropias tcp = getCategoria(1);
            List<Capitulo_R> lcr;
            cnv.seriesdetalle.Clear();
            if (tcp == null)
            {
                lcr = Referencia.manager.getCapitulos_Detalles_Propios(seccionActual, getTipoFiltro());

            }
            else
            {
                lcr = Referencia.manager.getCapitulos_Detalles_Propios(seccionActual, tcp, getTipoFiltro());
            }

            cnv.seriesdetalle.AddRange(SerieDetalle.parse(lcr));
            filtrarLista_Series_Propias_Detalles_ATravesDeTextoBuscar();
            //dgdetalle.Items.Refresh();
            actualizarLista(dgdetalle);
        }

        public void actualizarListaGeneral_Propias()
        {
            TipoDeCategoriaPropias tcp = getCategoria(0);
            List<Serie_R> lcr;
            cnv.series_generales.Clear();
            if (tcp == null)
            {
                lcr = Referencia.manager.getSeries_General_Propios(seccionActual);

            }
            else
            {
                lcr = Referencia.manager.getSeries_General_Propios(seccionActual, tcp);
            }

            cnv.series_generales.AddRange(SerieGeneral.parse(lcr));
            filtrarLista_Series_Propias_Generales_ATravesDeTextoBuscar();
            //lbgeneral.Items.Refresh();
            actualizarLista(lbgeneral);
        }



        private void filtrarLista_Series_Propias_Generales_ATravesDeTextoBuscar()
        {
            UtilidadesActualize.aplicarFiltroText(lbgeneral, cnv.series_generales, cnv.series_general_Filtradas, tbbuscargeneral, (sd, texto) =>
            Utiles.containsOR_AcontP_arreglarA(sd.nombres_serie, texto));
        }

        private void filtrarLista_Series_Propias_Detalles_ATravesDeTextoBuscar()
        {
            UtilidadesActualize.aplicarFiltroText(dgdetalle, cnv.seriesdetalle, cnv.series_detalle_Filtradas, tbbuscardetalle, (sd, texto) =>
            Utiles.containsOR_AcontP_arreglarA(sd.nombres_serie, texto)
            || Utiles.containsOR_AcontP_arreglarA(texto, sd.nombrecarpeta, sd.formato, sd.nombre_capitulo, sd.temporada));
        }

        private void filtrarLista_Series_Paquete_Generales_ATravesDeTextoBuscar()
        {
            UtilidadesActualize.aplicarFiltroText(lbgeneralPaquete, cnv.series_generales_Paquete, cnv.series_generales_Filtrada_Paquete, tb_buscarEnSeriesPaquete, (sd, texto) =>
            Utiles.containsOR_AcontP_arreglarA(sd.nombres_serie, texto));
        }


        private void filtrarLista_Series_Paquete_Detalles_ATravesDeTextoBuscar()
        {
            UtilidadesActualize.aplicarFiltroText(dgdetallePaqueteSeries, cnv.seriesdetalle_Paquete, cnv.seriesdetalle_Filtrada_Paquete, tb_buscarEnSeriesPaquete, (sd, texto) =>
            Utiles.containsOR_AcontP_arreglarA(sd.nombres_serie, texto)
            || Utiles.containsOR_AcontP_arreglarA(texto, sd.nombrecarpeta, sd.formato, sd.nombre_capitulo, sd.temporada));
        }

        private void alCambiar_CB_Categoria_General_Propias(object sender, SelectionChangedEventArgs e)
        {
            if (seInicio)
            {
                if (cbCategoriaSG.SelectedIndex != -1)
                {
                    cnv.indice_categoria_generales_propia = cbCategoriaSG.SelectedIndex;
                    actualizarListaGeneral_Propias();
                }

            }

        }

        private void alCambiar_CB_Categoria_Detalles_Propias(object sender, SelectionChangedEventArgs e)
        {
            if (seInicio)
            {
                if (cbCategoriaSD.SelectedIndex != -1)
                {
                    cnv.indice_categoria_detalles_propia = cbCategoriaSD.SelectedIndex;
                    actualizarListaDetalle_Propias();
                }

            }
        }

        private void alCambiar_CB_Etiquetas_Paquete(object sender, SelectionChangedEventArgs e)
        {
            if (seInicio)
            {
                if (cbEtiquetasPaquete.SelectedIndex != -1)
                {
                    cnv.indice_etiqueta_paquete = cbEtiquetasPaquete.SelectedIndex;
                    actualizarListas_Paquetes();
                }
            }
        }

        private void alCambiar_CB_Categoria_Paquete(object sender, SelectionChangedEventArgs e)
        {
            if (seInicio)
            {
                if (cbEtiquetasPaquete.SelectedIndex != -1)
                {
                    cnv.indice_categoria_paquete = cbCategoria_Paquete.SelectedIndex;
                    actualizarListas_Paquetes();
                }
                //cnv.indice_categoria_paquete = cbCategoria_Paquete.SelectedIndex;
                //filtrarLista_Series_Paquete_Generales_ATravesDeTextoBuscar();
                //filtrarLista_Series_Paquete_Detalles_ATravesDeTextoBuscar();
            }
        }

        private void KeyUp_TB_SeriesPaquete(object sender, System.Windows.Input.KeyEventArgs e)
        {
            filtrarLista_Series_Paquete_Detalles_ATravesDeTextoBuscar();
            filtrarLista_Series_Paquete_Generales_ATravesDeTextoBuscar();

        }

        private void alApretar_B_Actualizar_Paquete(object sender, RoutedEventArgs e)
        {
            if (cbEtiquetasPaquete.SelectedIndex > 0)
            {
                cargar_LasEtiquetasNecesarias(new ConjuntoDeEtiquetasDeSerie(mngSI_CB_Etiquetas_Paquetes.getEtiquetas_List()));
            }
            else
            {
                cargar_TodasLasSeriesDelPaquete();
            }
        }

        private void alApretar_B_Configuracion_Series_Paquete(object sender, RoutedEventArgs e)
        {
            Referencia.EA_Configuracion.index_cb_secciones_configuracion = seccionActual == TipoDeSeccion.SERIES ? 1 : 2;
            Referencia.EA_Configuracion.aplicar_indice_etiquetas_paquete = true;
            Referencia.EA_Configuracion.etiquetas_paquete = new ConjuntoDeEtiquetasDeSerie(mngSI_CB_Etiquetas_Paquetes.getEtiquetas_List());
            irAConfiguracion();
        }

        private void alApretar_B_VisualizarCapitulo_Detalles_Paquete(object sender, RoutedEventArgs e)
        {
            if (dgdetallePaqueteSeries.SelectedIndex != -1)
            {
                SerieDetalle s = (SerieDetalle)dgdetallePaqueteSeries.SelectedItem;
                if (s.puedeVisualizarse)
                {
                    if (s.urlVisualizacion != null)
                    {

                        FileInfo fi = new FileInfo(s.urlVisualizacion);
                        if (fi.Exists)
                        {

                            if (Referencia.EA_Configuracion.utilizar_reproductor_seleccionado)
                            {
                                if (Referencia.urlReproductorDeVideo != null)
                                {
                                    FileInfo fr = new FileInfo(Referencia.urlReproductorDeVideo);
                                    if (fr.Exists)
                                    {
                                        Utiles.ejecutarCMD(Referencia.urlReproductorDeVideo, "\"" + s.urlVisualizacion + "\"");

                                    }
                                    else
                                    {
                                        this.showDlg_Info("La dirección del reproductor de video  es incorrecta no existe\n" + Referencia.urlReproductorDeVideo);
                                    }
                                }
                                else
                                {
                                    this.showDlg_Info("La dirección del reproductor de video  es incorrecta");
                                }
                            }
                            else
                            {
                                UtilesNavegador.abrirArchivo(s.urlVisualizacion);
                            }



                        }
                        else
                        {
                            this.showDlg_Info("La dirección no existe\n" + s.urlVisualizacion);
                        }
                    }
                    else
                    {
                        this.showDlg_Info("La dirección es incorrecta");
                    }

                }
                else
                {
                    this.showDlg_Info("No es un video correcto");
                }
            }
            else
            {
                this.showDlg_Info("Seleccione una fila");
            }
        }

        private void alApretar_B_BorrarTextoFiltrar_Paquete(object sender, RoutedEventArgs e)
        {
            alApretar_B_BorrarTextoFiltrar(tb_buscarEnSeriesPaquete
                , filtrarLista_Series_Paquete_Detalles_ATravesDeTextoBuscar
                , filtrarLista_Series_Paquete_Generales_ATravesDeTextoBuscar);
        }

        private void alApretar_B_LocalizarCapitulo_Detalles_Paquete(object sender, RoutedEventArgs e)
        {
            if (dgdetallePaqueteSeries.SelectedIndex != -1)
            {
                SerieDetalle s = (SerieDetalle)dgdetallePaqueteSeries.SelectedItem;
                if (validar_Localizacion(s))
                {
                    UtilesNavegador.abrirRutaConArchivoSeleccionado(s.urlLocalizacion);
                }
            }
            else
            {
                this.showDlg_Info("Seleccione una fila");
            }
        }

        private void alApretar_B_Copiar_Serie_Detalles_Paquete(object sender, RoutedEventArgs e)
        {
            if (dgdetallePaqueteSeries.SelectedIndex != -1)
            {
                SerieDetalle s = (SerieDetalle)dgdetallePaqueteSeries.SelectedItem;
                if (validar_Localizacion(s))
                {
                    DirectoryInfo d = getCarpetaDestino_Y_validar();
                    if (d != null)
                    {
                        if (validarEspacioDisponible_Y_DialogoMensaje(d, s))
                        {
                            Referencia.mngCopiador.addDirecciones(new ReneUtiles.Clases.Copiador.Direcciones_Y_Destino(
                            d.ToString(), s.urlLocalizacion
                            ));
                        }


                    }


                    return;
                }
            }
            else
            {
                this.showDlg_Info("Seleccione una fila");
            }
        }

        private bool validarEspacioDisponible_Y_DialogoMensaje(DirectoryInfo destino, params SerieDetalle[] seleccionadas)
        {


            string discoDestino = Utiles.subs(destino.ToString(), 0, 3);
            System.IO.DriveInfo[] allDrives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo d in allDrives)
            {
                if (d.Name == discoDestino)
                {
                    double size = 0;
                    foreach (SerieDetalle s in seleccionadas)
                    {
                        size += s.tamanno;
                    }


                    if (size < d.AvailableFreeSpace)
                    {
                        return true;
                    }
                    EspacioEnAlmacenamiento espacioDisponible = new EspacioEnAlmacenamiento(d.AvailableFreeSpace);
                    EspacioEnAlmacenamiento espacioQueOcupa = new EspacioEnAlmacenamiento(size);
                    EspacioEnAlmacenamiento espacioNecesario = new EspacioEnAlmacenamiento(size - d.AvailableFreeSpace);
                    this.showDlg_Info("No tiene espacio disponible en la unidad " + d.Name
                        + "\n Espacio disponible: " + espacioDisponible.cantidadDeEsteTipoDeEspacio + " " + espacioDisponible.tipoDeEspacio
                        + "\n Espacio necesario : " + espacioNecesario.cantidadDeEsteTipoDeEspacio + " " + espacioNecesario.tipoDeEspacio
                        );
                    return false;
                }
            }
            this.showDlg_Info("No se encontró este disco duro ");
            return false;

        }

        private DirectoryInfo getCarpetaDestino_Y_validar()
        {
            try
            {
                List<DireccionDeActualizadorPropia> ld = Referencia.manager.getDireccionesPropias(seccionActual, TipoDeCategoriaPropias.QUE_TENGO);
                if (ld.Count() > 0)
                {
                    foreach (var d in ld)
                    {
                        if (Archivos.esCarpeta(d.url))
                        {
                            return new DirectoryInfo(d.url);
                        }
                    }

                    this.showDlg_Info("No hay ninguna dirección de una carpeta que exista en la sección “Que tengo” ");
                }
                else
                {
                    this.showDlg_Info("No hay ninguna dirección en la sección  “Que tengo” ");
                }
            }
            catch
            {
                this.showDlg_Info("Hay un error en su dirección");
            }
            return null;
        }

        //private bool validar_HayCarpetaDestino() {

        //}

        private bool validar_Localizacion(SerieDetalle s)
        {

            if (s.puedeLocalizarse)
            {
                string url = s.urlLocalizacion;
                if (url != null)
                {


                    if (Archivos.existeArchivo(url) || Archivos.existeCarpeta(url))
                    {

                        return true;
                        //UtilesNavegador.abrirRutaConArchivoSeleccionado(url);




                    }
                    else
                    {
                        this.showDlg_Info("La dirección no existe\n" + url);
                    }
                }
                else
                {
                    this.showDlg_Info("La dirección es incorrecta");
                }

            }
            else
            {
                this.showDlg_Info("No es una direccion correcta");
            }
            return false;
        }

        private void alApretar_B_Copiar_Todo_Series_Paquete(object sender, RoutedEventArgs e)
        {
            MostrarDialogo();

            DirectoryInfo d = getCarpetaDestino_Y_validar();
            if (d != null)
            {
                List<SerieDetalle> seriesSeleccionadas = cnv.seriesdetalle_Paquete.FindAll(v => v.seleccionado);
                if (seriesSeleccionadas.Count > 0)
                {
                    foreach (SerieDetalle s in seriesSeleccionadas)
                    {
                        if (!validar_Localizacion(s)) { dialoghost.IsOpen = false; return; }
                    }
                    if (validarEspacioDisponible_Y_DialogoMensaje(d, seriesSeleccionadas.ToArray()))
                    {
                        dialoghost.IsOpen = false;
                        Referencia.mngCopiador.addDirecciones(new ReneUtiles.Clases.Copiador.Direcciones_Y_Destino(
                        d.ToString(), (from s in seriesSeleccionadas select s.urlLocalizacion).ToArray()//s.urlLocalizacion
                        ));
                        return;
                    }
                }
                else
                {
                    this.showDlg_Info("No hay series seleccionadas");
                }

            }
            dialoghost.IsOpen = false;
        }

        private void alApretar_TB_MarcarTodas_Series_Detalles_Paquete_Checked(object sender, RoutedEventArgs e)
        {
            alApretar_TB_MarcarTodas_Series_Detalles_Paquete(true);
        }

        private void alApretar_TB_MarcarTodas_Series_Detalles_Paquete_Unchecked(object sender, RoutedEventArgs e)
        {
            alApretar_TB_MarcarTodas_Series_Detalles_Paquete(false);
        }

        private void Alseleccionar_Serie_Detalles_Paquete_Checked(object sender, RoutedEventArgs e)
        {
            Alseleccionar_Serie_Detalles_Paquete();
        }

        private void Aldeseleccionar_Serie_Detalles_Paquete_Checked(object sender, RoutedEventArgs e)
        {
            Alseleccionar_Serie_Detalles_Paquete();
        }

        private bool actualizar_desde_CB_series_seleccionada_detalle_paquete = true;
        private bool actualizar_ToB_SeleccionarTodasLasSeries_paquete = true;

        private void Alseleccionar_Serie_Detalles_Paquete()
        {
            if (actualizar_desde_CB_series_seleccionada_detalle_paquete)
            {
                actualizar_TB_espacio_que_ocupan_las_series_del_paquete_seleccionadas2();
                actualizar_ToB_SeleccionarTodasLasSeries_Paquete_deSerNecesario();
            }

        }


        private void actualizar_TB_espacio_que_ocupan_las_series_del_paquete_seleccionadas2()
        {

            double size = 0;
            cnv.seriesdetalle_Paquete.FindAll(v => v.seleccionado).ForEach(v => size += v.tamanno);
            string texto = "0.00 MB";
            if (size > 0)
            {
                EspacioEnAlmacenamiento espacioQueOcupa = new EspacioEnAlmacenamiento(size);
                texto = espacioQueOcupa.cantidadDeEsteTipoDeEspacio_numeroStr + espacioQueOcupa.tipoDeEspacio.medida_mini_Str2;

            }
            tb_espacio_total_copia_paquete.Text = texto;



        }
        private void actualizar_ToB_SeleccionarTodasLasSeries_Paquete_deSerNecesario()
        {
            bool estado = true;
            foreach (SerieDetalle s in cnv.seriesdetalle_Paquete)
            {
                if (!s.seleccionado)
                {
                    estado = false;
                    break;
                }
            }
            if (estado != TB_MarcarTodas_Series_Detalles_Paquete.IsChecked)
            {
                actualizar_ToB_SeleccionarTodasLasSeries_paquete = false;
                TB_MarcarTodas_Series_Detalles_Paquete.IsChecked = estado;
                actualizar_ToB_SeleccionarTodasLasSeries_paquete = true;
            }
        }

        private void alApretar_TB_MarcarTodas_Series_Detalles_Paquete(bool chequed)
        {
            if (actualizar_ToB_SeleccionarTodasLasSeries_paquete)
            {
                actualizar_desde_CB_series_seleccionada_detalle_paquete = false;
                cnv.seriesdetalle_Paquete.ForEach(v =>
                {
                    if (v.seleccionado != chequed)
                    {
                        v.seleccionado = chequed;
                    }
                });
                actualizar_TB_espacio_que_ocupan_las_series_del_paquete_seleccionadas2();
                actualizar_desde_CB_series_seleccionada_detalle_paquete = true;
            }

        }

        private void alCambiar_CB_TX_O_Finalizada_Paquete(object sender, SelectionChangedEventArgs e)
        {
            if (cb_TX_O_Finalizada_Paquete.SelectedIndex != -1)
            {
                if (seInicio)
                {
                    actualizarTags_CB_Paquete();


                    cnv.indice_tx_o_finalizada_paquete = cb_TX_O_Finalizada_Paquete.SelectedIndex;
                    actualizarListas_Paquetes();

                }
            }
        }
    }




    public class SerieDetalle : ViewModelBase
    {

        private string _id;
        private string _idserie;
        private int _index;
        private string _nombre_serie;
        private List<string> _nombres_serie;
        private string _nombre_capitulo;
        private string _temporada;
        private string _capitulo;
        private string _formato;
        private string _nombrecarpeta;
        private string _direccion;

        private bool _tieneMasDeUnNombreLaSerie;

        private bool _puedeVisualizarse;


        private string _urlVisualizacion;


        private double _tamanno;


        private string _urlLocalizacion;

        private bool _puedeLocalizarse;

        private string _tamannoStr;
        public string tamannoStr
        {
            get
            {
                return _tamannoStr;
            }
            set
            {
                _tamannoStr = value;
                OnPropertyChanged(nameof(tamannoStr));
            }
        }

        private bool _seleccionado;
        public bool seleccionado
        {
            get
            {
                return _seleccionado;
            }
            set
            {
                _seleccionado = value;
                OnPropertyChanged(nameof(seleccionado));
            }
        }
        public bool puedeLocalizarse
        {
            get
            {
                return _puedeLocalizarse;
            }
            set
            {
                _puedeLocalizarse = value;
                OnPropertyChanged(nameof(puedeLocalizarse));
            }
        }
        public string urlLocalizacion
        {
            get
            {
                return _urlLocalizacion;
            }
            set
            {
                _urlLocalizacion = value;
                OnPropertyChanged(nameof(urlLocalizacion));
            }
        }
        public double tamanno
        {
            get
            {
                return _tamanno;
            }
            set
            {
                _tamanno = value;
                OnPropertyChanged(nameof(tamanno));
            }
        }
        public string urlVisualizacion
        {
            get
            {
                return _urlVisualizacion;
            }
            set
            {
                _urlVisualizacion = value;
                OnPropertyChanged(nameof(urlVisualizacion));
            }
        }
        public bool puedeVisualizarse
        {
            get
            {
                return _puedeVisualizarse;
            }
            set
            {
                _puedeVisualizarse = value;
                OnPropertyChanged(nameof(puedeVisualizarse));
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
        public string idserie
        {
            get
            {
                return _idserie;
            }
            set
            {
                _idserie = value;
                OnPropertyChanged(nameof(idserie));
            }
        }
        public int index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                OnPropertyChanged(nameof(index));
            }
        }
        public string nombre_serie
        {
            get
            {
                return _nombre_serie;
            }
            set
            {
                _nombre_serie = value;
                OnPropertyChanged(nameof(nombre_serie));
            }
        }
        public List<string> nombres_serie
        {
            get
            {
                return _nombres_serie;
            }
            set
            {
                _nombres_serie = value;
                OnPropertyChanged(nameof(nombres_serie));
            }
        }
        public string nombre_capitulo
        {
            get
            {
                return _nombre_capitulo;
            }
            set
            {
                _nombre_capitulo = value;
                OnPropertyChanged(nameof(nombre_capitulo));
            }
        }
        public string temporada
        {
            get
            {
                return _temporada;
            }
            set
            {
                _temporada = value;
                OnPropertyChanged(nameof(temporada));
            }
        }
        public string capitulo
        {
            get
            {
                return _capitulo;
            }
            set
            {
                _capitulo = value;
                OnPropertyChanged(nameof(capitulo));
            }
        }
        public string formato
        {
            get
            {
                return _formato;
            }
            set
            {
                _formato = value;
                OnPropertyChanged(nameof(formato));
            }
        }
        public string nombrecarpeta
        {
            get
            {
                return _nombrecarpeta;
            }
            set
            {
                _nombrecarpeta = value;
                OnPropertyChanged(nameof(nombrecarpeta));
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


        public bool tieneMasDeUnNombreLaSerie
        {
            get
            {
                return _tieneMasDeUnNombreLaSerie;
            }
            set
            {
                _tieneMasDeUnNombreLaSerie = value;
                OnPropertyChanged(nameof(tieneMasDeUnNombreLaSerie));
            }
        }
        //public void setSeleccionado_sinProperty(bool seleccionado) {
        //    this._seleccionado
        //}

        public static SerieDetalle parse(Capitulo_R c)
        {
            bool puedeVisualizarce = false;
            string ulrVisualizacion = "";

            string nomc = "";
            if (c.url != null && c.url.Trim().Length > 0)
            {
                DirectoryInfo di = new DirectoryInfo(c.url);
                if (di.Parent != null)
                {
                    nomc = di.Parent.Name;



                }
            }




            bool puedeVerse = c.listaDeVideos.Count() > 0 && c.listaDeVideos[0].Exists;
            bool puedeLocalizarse = false;
            try { puedeLocalizarse = Archivos.esArchivo(c.url) || Archivos.esCarpeta(c.url); } catch { }

            EspacioEnAlmacenamiento espacioQueOcupa = new EspacioEnAlmacenamiento(c.size);

            return new SerieDetalle()
            {
                id = c.id,
                idserie = c.idDeSerie,
                nombres_serie = c.nombres_de_serie,
                nombre_serie = c.nombres_de_serie[0],//c.nombres_de_serie[0],
                nombre_capitulo = c.nombre_capitulo,
                temporada = ((c.temporada < 1) ? 1 : c.temporada) + "",
                capitulo = c.capitulo,
                formato = c.formato,
                nombrecarpeta = nomc,
                direccion = c.url,
                //visibilidad_columna_comboBox_nombresDeSerie = (c.nombres_de_serie.Count > 1) ? "Visible" : "Hidden",
                //visibilidad_columna_label_nombreDeSerie = (c.nombres_de_serie.Count > 1) ? "Hidden" : "Visible",
                tieneMasDeUnNombreLaSerie = c.nombres_de_serie.Count > 1,
                puedeVisualizarse = puedeVerse,
                urlVisualizacion = puedeVerse ? c.listaDeVideos[0].ToString() : "",
                tamanno = c.size,
                puedeLocalizarse = puedeLocalizarse,
                urlLocalizacion = puedeLocalizarse ? c.url : "",
                seleccionado = false,
                tamannoStr = espacioQueOcupa.cantidadDeEsteTipoDeEspacio_numeroStr + " " + espacioQueOcupa.tipoDeEspacio




            };
        }//PopupContentClassicTemplate

        public static List<SerieDetalle> parse(List<Capitulo_R> series)
        {
            List<SerieDetalle> list = new List<SerieDetalle>();
            foreach (Capitulo_R item in series)
            {
                list.Add(SerieDetalle.parse(item));
            }
            return list;
        }
    }

    public class SerieGeneral : ViewModelBase
    {

        private string _id;
        private string _nombre_serie;
        private List<string> _nombres_serie;
        private List<Temporada> _data;
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
        public string nombre_serie
        {
            get
            {
                return _nombre_serie;
            }
            set
            {
                _nombre_serie = value;
                OnPropertyChanged(nameof(nombre_serie));
            }
        }
        public List<string> nombres_serie
        {
            get
            {
                return _nombres_serie;
            }
            set
            {
                _nombres_serie = value;
                OnPropertyChanged(nameof(nombres_serie));
            }
        }
        public List<Temporada> data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                OnPropertyChanged(nameof(data));
            }
        }

        public static SerieGeneral parse(Serie_R s)
        {
            List<Temporada> data1 = new List<Temporada>();
            foreach (var item in s.temporadas)
            {
                data1.Add(Temporada.parse(item));
            }
            return new SerieGeneral()
            {
                id = s.id,
                nombre_serie = s.nombres_de_serie[0],
                nombres_serie = s.nombres_de_serie,
                data = data1
            };
        }

        public static List<SerieGeneral> parse(List<Serie_R> series)
        {
            List<SerieGeneral> list = new List<SerieGeneral>();
            foreach (Serie_R item in series)
            {
                list.Add(SerieGeneral.parse(item));
            }
            return list;
        }

    }

    public class Temporada
    {
        public string id { get; set; }
        public string temporada { get; set; }
        public string num_capitulos { get; set; }
        public List<Capitulo> data1 { get; set; }

        public static Temporada parse(Temporada_R t)
        {
            List<Capitulo> data = new List<Capitulo>();
            foreach (var item in t.capitulos)
            {
                data.Add(Capitulo.parse(item));
            }
            return new Temporada()
            {
                id = t.id,
                temporada = t.numeroTemporada + "",
                num_capitulos = t.cantidadDeCapitulos_distintos + "",
                data1 = data

            };
        }
    }

    public class Capitulo
    {
        public string id { get; set; }
        public string nombre_capitulo { get; set; }
        public string capitulo { get; set; }
        public string formato { get; set; }

        public static Capitulo parse(Capitulo_R c)
        {
            return new Capitulo()
            {
                id = c.id,
                nombre_capitulo = c.nombre_capitulo,
                capitulo = c.capitulo,
                formato = c.formato
            };

        }
    }
}



//private bool validarEspacioDisponible_Y_DialogoMensaje(SerieDetalle s, DirectoryInfo destino)
//{
//    string discoDestino = Utiles.subs(s.urlLocalizacion, 0, 3);
//    System.IO.DriveInfo[] allDrives = System.IO.DriveInfo.GetDrives();
//    foreach (System.IO.DriveInfo d in allDrives)
//    {
//        if (d.Name == discoDestino)
//        {

//            if (s.tamanno < d.AvailableFreeSpace)
//            {
//                return true;
//            }
//            EspacioEnAlmacenamiento espacioDisponible = new EspacioEnAlmacenamiento(d.AvailableFreeSpace);
//            EspacioEnAlmacenamiento espacioQueOcupa = new EspacioEnAlmacenamiento(s.tamanno);
//            EspacioEnAlmacenamiento espacioNecesario = new EspacioEnAlmacenamiento(s.tamanno - d.AvailableFreeSpace);
//            this.showDlg_Info("No tiene espacio disponible en la unidad " + d.Name
//                + "\n Espacio disponible: " + espacioDisponible.cantidadDeEsteTipoDeEspacio + " " + espacioDisponible.tipoDeEspacio
//                + "\n Espacio necesario : " + espacioNecesario.cantidadDeEsteTipoDeEspacio + " " + espacioNecesario.tipoDeEspacio
//                );
//            return false;
//        }
//    }
//    this.showDlg_Info("No se encontró este disco duro ");
//    return false;

//}