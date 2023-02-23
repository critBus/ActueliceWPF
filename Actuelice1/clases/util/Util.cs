using System.Windows.Controls;
using Actuelice1.clases.vista;
using RelacionadorDeSerie;
using System;
using System.Collections.Generic;
using ReneUtiles.Clases.Multimedia.Series.Procesadores.Buscadores.Datos;
using Actuelice1.clases.vista.Series;
using System.Collections.ObjectModel;
using ReneUtiles;
using System.Windows;
using System.Windows.Threading;
using ReneUtiles.Clases.Copiador;

namespace Actuelice1.clases.util
{
    //
    class Referencia
    {
        public static Frame frame;
        public static ManagerRelacionadorDeSerie manager;

        public static MangerCopiador mngCopiador;

        public static Series_Vista ventana_series;
        public static Series_Vista ventana_anime;
        public static Configuracion ventana_Configuracion;
        public static Home ventana_home;
        public static Peliculas ventana_peliculas;
        public static Novelas ventana_novelas;

        public static Action<Page> showFrame;

        public static string urlReproductorDeVideo;

        //public static TipoDeSeccion seccionActual;


        //----------------Rene ----------------
        public static class EA_Configuracion {
            // ++++++++ Roilan
            //Configuracion
            public static int index_cb_secciones_configuracion;
            // +++++++++ End Roilan

            //----------------Rene ----------------
            public static bool aplicar_indice_categoria_propia = false;
            public static int indice_categoria_propia;

            public static bool aplicar_indice_etiquetas_paquete = false;
            public static ConjuntoDeEtiquetasDeSerie etiquetas_paquete;

            public static bool utilizar_reproductor_seleccionado = false;
        }



        public static class EA_Series
        {
            public static ConjuntoDeVariables_VistaSerie variables= new ConjuntoDeVariables_VistaSerie();

        }

        public static class EA_SeriesAnimes
        {
            public static ConjuntoDeVariables_VistaSerie variables = new ConjuntoDeVariables_VistaSerie();

        }


        //public static void subpVisual(Control contrl, Action act)
        //{

        //}
    }
    public abstract class UtilidadesActualize {

        public static void resetearDireccionDeVideo() {
            string[] urlReproductoresDefault = {
                @"C:\Program Files\DAUM\PotPlayer\PotPlayerMini64.exe"
                ,@"C:\Program Files (x86)\K-Lite Codec Pack\MPC-HC64\mpc-hc64.exe"
            };
            foreach (string url in urlReproductoresDefault)
            {
                if (Archivos.existeArchivo(url)) {
                    Referencia.urlReproductorDeVideo = url;
                    return;
                }
            }
            Referencia.urlReproductorDeVideo = "";
        }
        public static string getEtiquetasEnStr(IEnumerable<TipoDeEtiquetaDeSerie> etiquetas)
        {
            string categoria = "";//item.categoria.getValor();
            SortedSet<string> st_tags = new SortedSet<string>();
            foreach (TipoDeEtiquetaDeSerie tag in etiquetas)
            {
                st_tags.Add(tag.nombreTag);
                // dir.categoria +=" "+ tag.nombreTag;
            }

            Action<TipoDeEtiquetaDeSerie> ponerDePrimero = t => {
                if (st_tags.Contains(t.nombreTag))
                {
                    categoria += (categoria.Length > 0 ? " " : "") + t.nombreTag;
                    st_tags.Remove(t.nombreTag);
                }
            };
            ponerDePrimero(TipoDeEtiquetaDeSerie.FINALIZADAS);
            ponerDePrimero(TipoDeEtiquetaDeSerie.TX);
            ponerDePrimero(TipoDeEtiquetaDeSerie.CLASICAS);
            ponerDePrimero(TipoDeEtiquetaDeSerie.DOBLADAS);
            ponerDePrimero(TipoDeEtiquetaDeSerie.SUBTITULADAS);
            foreach (string tag in st_tags)
            {
                categoria += (categoria.Length > 0 ? " " : "") + tag;
            }
            return categoria;
        }

        private static Dictionary<TextBox,int> hs_tbFiltrandose=new Dictionary<TextBox,int>();
        public static void aplicarFiltroText<E>(
            ItemsControl dg
            , List<E> ldOriginal
            , ObservableCollection<E> lo
            , System.Windows.Controls.TextBox tb
            , Func<E, string, bool> filtro
            )
        {
            if (!hs_tbFiltrandose.ContainsKey(tb)) {
                hs_tbFiltrandose.Add(tb,0);
            }
            int indiceActual = hs_tbFiltrandose[tb]+1;
            hs_tbFiltrandose[tb] = indiceActual;

            List<E> ldP = ldOriginal;

            lo.Clear();
            if (tb.Text.Length > 0)
            {
                string texto = tb.Text;
                texto = Utiles.arreglarPalabra(texto.Trim().ToLower());
                ldP = ldOriginal.FindAll(v => filtro(v, texto));
            }


            //ldP.ForEach(v => lo.Add(v));
            //ldP.ForEach(v => Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,new Action( ()=> lo.Add(v))));
            try {
                foreach (E v in ldP)
                {
                    if (hs_tbFiltrandose[tb]!= indiceActual) {
                        break;
                    }
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => lo.Add(v)));
                    
                }
            } catch { }
            
            //Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, AddItem);
            // dg.Items.Refresh();
        }


        //Frame[] frames = { ventana_series, ventana_anime, ventana_Configuracion, ventana_home, ventana_peliculas, ventana_novelas };
    }
    //class Evento : EventosEnSubproceso
    //{
    //    public Evento(Action alTerminar, Action<Exception> siDaError) : base(alTerminar, siDaError)
    //    {
    //        this.alTerminar = () =>
    //        {
    //            subpVisual(alTerminar);
    //        };

    //        this.siDaError = (e) =>
    //        {
    //            subpVisual(siDaError, e);
    //        };
    //    }

    //    public static void subpVisual(Action metodo)
    //    {
    //        System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
    //        {
    //            metodo();
    //        }));
    //    }

    //    public static void subpVisual(Action<Exception> metodo, Exception e)
    //    {
    //        System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
    //        {
    //            metodo(e);
    //        }));
    //    }
    //}

}
