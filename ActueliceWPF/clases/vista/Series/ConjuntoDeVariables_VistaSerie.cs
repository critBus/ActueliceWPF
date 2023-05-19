using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReneUtiles.Clases.WPF;
using System.Collections.ObjectModel;
using Actuelice1.clases.util;

namespace Actuelice1.clases.vista.Series
{
    class ConjuntoDeVariables_VistaSerie:ViewModelBase
    {
        private List<SerieGeneral> _series_generales;
        private ObservableCollection<SerieGeneral> _series_general_Filtradas;
        private List<SerieDetalle> _seriesdetalle;
        private ObservableCollection<SerieDetalle> _series_detalle_Filtradas;
        private List<SerieGeneral> _series_generales_Paquete;
        private ObservableCollection<SerieGeneral> _series_generales_Filtrada_Paquete;
        private List<SerieDetalle> _seriesdetalle_Paquete;
        private ObservableCollection<SerieDetalle> _seriesdetalle_Filtrada_Paquete;
        public List<SerieGeneral> series_generales
        {
            get
            {
                return _series_generales;
            }
            set
            {
                _series_generales = value;
                OnPropertyChanged(nameof(series_generales));
            }
        }
        public ObservableCollection<SerieGeneral> series_general_Filtradas
        {
            get
            {
                return _series_general_Filtradas;
            }
            set
            {
                _series_general_Filtradas = value;
                OnPropertyChanged(nameof(series_general_Filtradas));
            }
        }
        public List<SerieDetalle> seriesdetalle
        {
            get
            {
                return _seriesdetalle;
            }
            set
            {
                _seriesdetalle = value;
                OnPropertyChanged(nameof(seriesdetalle));
            }
        }
        public ObservableCollection<SerieDetalle> series_detalle_Filtradas
        {
            get
            {
                return _series_detalle_Filtradas;
            }
            set
            {
                _series_detalle_Filtradas = value;
                OnPropertyChanged(nameof(series_detalle_Filtradas));
            }
        }
        public List<SerieGeneral> series_generales_Paquete
        {
            get
            {
                return _series_generales_Paquete;
            }
            set
            {
                _series_generales_Paquete = value;
                OnPropertyChanged(nameof(series_generales_Paquete));
            }
        }
        public ObservableCollection<SerieGeneral> series_generales_Filtrada_Paquete
        {
            get
            {
                return _series_generales_Filtrada_Paquete;
            }
            set
            {
                _series_generales_Filtrada_Paquete = value;
                OnPropertyChanged(nameof(series_generales_Filtrada_Paquete));
            }
        }
        public List<SerieDetalle> seriesdetalle_Paquete
        {
            get
            {
                return _seriesdetalle_Paquete;
            }
            set
            {
                _seriesdetalle_Paquete = value;
                OnPropertyChanged(nameof(seriesdetalle_Paquete));
            }
        }
        public ObservableCollection<SerieDetalle> seriesdetalle_Filtrada_Paquete
        {
            get
            {
                return _seriesdetalle_Filtrada_Paquete;
            }
            set
            {
                _seriesdetalle_Filtrada_Paquete = value;
                OnPropertyChanged(nameof(seriesdetalle_Filtrada_Paquete));
            }
        }

        public int indice_categoria_paquete;
        public int indice_etiqueta_paquete;
        public int indice_tx_o_finalizada_paquete;

        public int indice_categoria_detalles_propia;
        public int indice_categoria_generales_propia;

        public string filtro_detalles_propia;
        public string filtro_generales_propia;
        public string filtro_paquete;

        public SortedSet<string> tagsAnteriores_Paquete;



        public DatosARecargar_Series datosDeRecarga;

        public ConjuntoDeVariables_VistaSerie() {
            series_generales_Paquete = new List<SerieGeneral>();//Referencia.EA_Series.variables.
            series_generales_Filtrada_Paquete = new ObservableCollection<SerieGeneral>();
            seriesdetalle_Paquete = new List<SerieDetalle>();
            seriesdetalle_Filtrada_Paquete = new ObservableCollection<SerieDetalle>();

            seriesdetalle = new List<SerieDetalle>();
            series_detalle_Filtradas = new ObservableCollection<SerieDetalle>();

            series_generales = new List<SerieGeneral>();
            series_general_Filtradas = new ObservableCollection<SerieGeneral>();

            this.indice_categoria_paquete = this.indice_etiqueta_paquete 
                =this.indice_categoria_detalles_propia=this.indice_categoria_generales_propia
                =this.indice_tx_o_finalizada_paquete = 0;
            this.filtro_paquete = this.filtro_detalles_propia = this.filtro_generales_propia="";


            this.datosDeRecarga = new DatosARecargar_Series();

            this.tagsAnteriores_Paquete = ComparadorDeEtiquetasStr.getNewSortedSet();
            //this.hayQueCargarSeries = true;
        }
    }
}
