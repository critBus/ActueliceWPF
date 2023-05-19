using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CopyFileGeneric;
//using CopyFileGeneric.Utiles;
using ReneUtiles;
using ReneUtiles.Clases;
using ReneUtiles.Clases.Copiador;

namespace Actuelice1.clases.util
{
    public class MangerCopiador_CopyFileGeneric:MangerCopiador
    {
        public CopyFileGeneric.MainWindow ventana;

        

        public override void addDirecciones(params Direcciones_Y_Destino[] direcciones) {
            if (ventana == null)
            {
                ventana = new CopyFileGeneric.MainWindow();
                

                ventana.addAlTerminar(() => {
                    vaciarVetana();
                    cwl("termino la copia");
                    //Action a = () => {
                    //    //ventana.Dispose();
                    //    ventana = null; };
                    //try { ventana.Invoke(a); } catch { ventana = null; }




                });
                //ventana.Show(null);
                bool seMostro = false;
                ventana.Shown += (c, v) => {
                    cwl("asd");
                    if (!seMostro) {
                        seMostro = true;
                        ventana.SetSource_And_Destino(direcciones);
                        ventana.StartCopy();
                        ventana.PauseCopy();
                        ventana.ResumeCopy();
                    }
                    
                };

                System.Windows.Forms.Application.Run(ventana);

                

                




                cwl("lo agrego normal");
            }
            else {
                
                ventana.AddSource_R(direcciones);
                cwl("fue append");
            }
        }

        private void vaciarVetana() {
            if (ventana!=null) {
                if (!ventana.IsDisposed) {

                    try {
                        Action a = () => ventana.Close();
                        ventana.Invoke(a);
                    } catch {

                    }
                    
                }
                

                

                
                ventana = null;
            }
            
        }
    }
}
