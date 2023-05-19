using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using ReneUtiles.Clases.WPF;
using ReneUtiles.Clases.Multimedia.Series.Procesadores.Buscadores.Datos;
using ReneUtiles;
using ReneUtiles.Clases;
namespace Actuelice1.clases.util
{
    public class ManagerSelectionIndex_CB_Etiquetas : ManagerSelectionIndex
    {
        public string tagAnteriorSeleccionado_Paquete;
        public readonly static string TAG_TODAS="TODAS";
        public SortedSet<string> tagsAnteriores;

        public ConjuntoDeEventos<Action> accionAlActualizarTags; 
        public ManagerSelectionIndex_CB_Etiquetas(SelectionChangedEventHandler evento, Selector seleccionador, SortedSet<string> tagsAnteriores=null, Action accionAlActualizarTags=null) : base(evento, seleccionador)
        {
            if (tagsAnteriores!=null) {
                add(tagsAnteriores);
                //foreach (string tag in tagsAnteriores)
                //{
                //    seleccionador.Items.Add(tag);
                //}
                this.tagsAnteriores = tagsAnteriores;
            }
            this.accionAlActualizarTags = new ConjuntoDeEventos<Action>();

            if (accionAlActualizarTags!=null) {
                this.accionAlActualizarTags.add(accionAlActualizarTags);
            }

            
        }
        public void actualizarTags<E>(IEnumerable<E> l,Func<E,IEnumerable<TipoDeEtiquetaDeSerie>> obtenerEtiquetas) {
            SortedSet<string> hsTags = ComparadorDeEtiquetasStr.getNewSortedSet();
            foreach (E item in l)
            {

                hsTags.Add(UtilidadesActualize.getEtiquetasEnStr(obtenerEtiquetas(item)));
            }
            clear();

            hsTags.Add(TAG_TODAS);
            this.tagsAnteriores = hsTags;
            add(hsTags);
            //this.seleccionador.Items.Add(TAG_TODAS);
            int i = 0;
            bool hayQueSeleccionar = false;
            int indiceEnElQueSeleccionar = -1;
            foreach (string tag in hsTags)
            {

                //this.seleccionador.Items.Add(tag);
                if (tagAnteriorSeleccionado_Paquete != null && tag == tagAnteriorSeleccionado_Paquete)
                {
                    hayQueSeleccionar = true;
                    indiceEnElQueSeleccionar = i;
                }
                i++;
            }
            if (hayQueSeleccionar)
            {
                selectIndex(indiceEnElQueSeleccionar);

            }

            this.accionAlActualizarTags.ejecutar();
        }

        public List<TipoDeEtiquetaDeSerie> getEtiquetas_List()
        {
            List<TipoDeEtiquetaDeSerie> Ltdc = new List<TipoDeEtiquetaDeSerie>();

            Selector cb = this.seleccionador;





            if (cb.SelectedIndex>0)//cb.SelectedIndex != -1&&
            {
                foreach (string tag in Utiles.split(cb.SelectedItem.ToString(), " "))
                {
                    
                    TipoDeEtiquetaDeSerie tdc = TipoDeEtiquetaDeSerie.get(tag);
                    Ltdc.Add(tdc);
                }


            }




            return Ltdc;
        }
        public TipoDeEtiquetaDeSerie[] getEtiquetas()
        {
            return getEtiquetas_List().ToArray();
        }

        public bool estaSeleccionado_Todas() {
            return this.seleccionador.SelectedIndex <= 0;
        }

        //public virtual void selectIndex(int indice)
        //{

        //}
    }

    

    public class ComparadorDeEtiquetasStr : IEqualityComparer<string>, IComparer<string>
    {
        private static readonly ComparadorDeEtiquetasStr comparadorDeIgualdad_EtiquetasStr = new ComparadorDeEtiquetasStr();

        //public static readonly Dictionary<string, int> codigosHash = new Dictionary<string, int>();
        //public static int ultimoHash = 0;
        private Func<string> getPrimera;
        public ComparadorDeEtiquetasStr(Func<string>  getPrimera=null) {
            if (getPrimera != null)
            {
                this.getPrimera = getPrimera;
            }
            else {
                this.getPrimera = () => "todas";
            }
        }

        private string getKey(string obj)
        {
            return obj;
        }
        public bool Equals(string x, string y)
        {
            return x==y; //getKey(x) == getKey(y);
        }
        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
            //string key = getKey(obj);
            //if (codigosHash.ContainsKey(key))
            //{
            //    return codigosHash[key];
            //}
            //int hash = ultimoHash++;
            //codigosHash.Add(key, hash);
            //return hash;
        }
        public int Compare(string x, string y)
        {
            bool esXTodas = x.ToLower() == getPrimera();
            bool esYTodas = y.ToLower() == getPrimera();

            return esXTodas ?(esYTodas?0: -1):(esYTodas ? 1:  x.CompareTo(y) );
        }


        public static HashSet<string> getNewHashSet()
        {
            return new HashSet<string>(comparadorDeIgualdad_EtiquetasStr);
        }
        public static HashSet<string> getNewHashSet(IEnumerable<string> anterior)
        {
            return new HashSet<string>(anterior, comparadorDeIgualdad_EtiquetasStr);
        }

        public static SortedSet<string> getNewSortedSet(IEnumerable<string> anterior)
        {
            return new SortedSet<string>(anterior, comparadorDeIgualdad_EtiquetasStr);
        }
        public static SortedSet<string> getNewSortedSet()
        {
            return new SortedSet<string>(comparadorDeIgualdad_EtiquetasStr);
        }

        public static Dictionary<string, E> getNewDictionary<E>()
        {
            return new Dictionary<string, E>(comparadorDeIgualdad_EtiquetasStr);
        }
    }
}
