using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RelacionadorDeSerie;
using ReneUtiles.Clases.Multimedia.Series.Procesadores.Buscadores.Datos;

namespace Actuelice1.clases.vista.Series
{

    class DatosARecargar_Series
    {
        public bool hayQueCargarSeries;
        public bool hayQueRecargarTodasLasCategorias;
        public HashSet<TipoDeCategoriaPropias> categoriasARecargar;
        public bool hayQueRecargarTodasLasEtiquetas;
        public HashSet<ConjuntoDeEtiquetasDeSerie> etiquetasARecargar;
        public DatosARecargar_Series()
        {
            resetear();
            this.hayQueCargarSeries = true;
            this.hayQueRecargarTodasLasCategorias = true;
            this.hayQueRecargarTodasLasEtiquetas = true;
        }

        public bool hayQueRecargarCategorias() {
            return categoriasARecargar.Count > 0;
        }
        public bool hayQueRecargarEtiquetas()
        {
            return etiquetasARecargar.Count > 0;
        }

        public void setActualizarTodo()
        {
            this.hayQueCargarSeries = true;
            setActualizarTodasLasCategorias();
            setActualizarTodasLasEtiquetas();
        }


        public void setActualizarTodasLasCategorias()
        {
            this.hayQueCargarSeries = true;
            this.hayQueRecargarTodasLasCategorias = true;
        }

        public void setActualizarTodasLasEtiquetas()
        {
            this.hayQueCargarSeries = true;
            this.hayQueRecargarTodasLasEtiquetas = true;
        }

        private bool tieneTodasLasCategorias()
        {
            foreach (TipoDeCategoriaPropias t in TipoDeCategoriaPropias.VALUES)
            {
                bool laEncontro = false;
                foreach (TipoDeCategoriaPropias t_interna in categoriasARecargar)
                {
                    if (t == t_interna)
                    {
                        laEncontro = true;
                        break;
                    }
                }
                if (!laEncontro)
                {
                    return false;
                }

            }
            return true;
        }

        public void add(TipoDeCategoriaPropias t)
        {
            this.hayQueCargarSeries = true;
            if (!this.hayQueRecargarTodasLasCategorias)
            {
                categoriasARecargar.Add(t);
                if (tieneTodasLasCategorias())
                {
                    this.hayQueRecargarTodasLasCategorias = true;
                }
            }

        }
        
        public void add(IEnumerable<TipoDeEtiquetaDeSerie> let)
        {
            this.hayQueCargarSeries = true;

            etiquetasARecargar.Add(new ConjuntoDeEtiquetasDeSerie(let));
            

        }

        public void add(ConjuntoDeEtiquetasDeSerie c)
        {
            this.hayQueCargarSeries = true;

            etiquetasARecargar.Add(c);


        }

        public void resetear()
        {
            this.hayQueCargarSeries = false;
            this.hayQueRecargarTodasLasCategorias = false;
            this.categoriasARecargar = TipoDeCategoriaPropias.getNewHashSet();
            this.hayQueRecargarTodasLasEtiquetas = false;
            this.etiquetasARecargar = ConjuntoDeEtiquetasDeSerie.getNewHashSet();
        }
    }
}
