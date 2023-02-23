using System;
using System.Linq;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Actuelice1.clases.util;
using Actuelice1.clases.vista;
using RelacionadorDeSerie;
using System.IO;
using ReneUtiles.Clases.WPF;
using ReneUtiles.Clases.WPF.Copiadores;
using ReneUtiles;

using Actuelice1.clases.vista.Dialogos;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Specialized;

namespace Actuelice1

{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DialogoCargando dlgCargando;
        public MainWindow()
        {
            InitializeComponent();
            //drawerhost.LeftDrawerCloseOnClickAway = false;
            Loaded += (e, v) => {

                Referencia.mngCopiador = new Actuelice1.clases.util.MangerCopiador_CopyFileGeneric(); //new MangerCopiador_Windows();

                Referencia.frame = frame_main_home;

                Referencia.manager = new ManagerRelacionadorDeSerie(new EjecutorDeSubprosesosWPF());

                Referencia.urlReproductorDeVideo = Referencia.manager.TS().getStr("ultimoDir_Reproductor", "");
                if (Referencia.urlReproductorDeVideo.Length == 0)
                {
                    UtilidadesActualize.resetearDireccionDeVideo();

                    string url = Referencia.urlReproductorDeVideo;


                    //provarCopiador();
                }

                Referencia.EA_Configuracion.utilizar_reproductor_seleccionado = Referencia.manager.TS().getBool("cb_UtilizarElReproductorSeleccionado", false) ?? false;

                Referencia.showFrame = setVisibleFrame;

                frame_main_home.Navigate(new Uri("clases/vista/Home.xaml", UriKind.RelativeOrAbsolute));
                
                frame_main_series.ContentRendered += (z1, z2) =>
                {
                    Referencia.ventana_home = (Home)frame_main_home.Content;
                };

                    frame_main_series.Navigate(new Uri("clases/vista/Series.xaml", UriKind.RelativeOrAbsolute));
                frame_main_series.ContentRendered += (z1, z2) => {
                    Referencia.ventana_series = (Series_Vista)frame_main_series.Content;
                    Referencia.ventana_series.inicializar(TipoDeSeccion.SERIES);
                    Referencia.ventana_series.actualizar();
                };
                

                //Referencia.seccionActual = TipoDeSeccion.ANIME;
                frame_main_animes.Navigate(new Uri("clases/vista/Series.xaml", UriKind.RelativeOrAbsolute));
                frame_main_animes.ContentRendered += (z1, z2) => {
                    Referencia.ventana_anime = (Series_Vista)frame_main_animes.Content;
                    Referencia.ventana_anime.inicializar(TipoDeSeccion.ANIME);
                    Referencia.ventana_anime.actualizar();
                };
                    


                frame_main_configuracion.Navigate(new Uri("clases/vista/Configuracion.xaml", UriKind.RelativeOrAbsolute));

                frame_main_configuracion.ContentRendered += (z1, z2) =>
                {
                    Referencia.ventana_Configuracion = (Configuracion)frame_main_configuracion.Content;
                    Referencia.ventana_Configuracion.actualizar();
                };

                frame_main_peliculas.Navigate(new Uri("clases/vista/Peliculas.xaml", UriKind.RelativeOrAbsolute));
                
                frame_main_peliculas.ContentRendered += (z1, z2) =>
                {
                    Referencia.ventana_peliculas = (Peliculas)frame_main_peliculas.Content;
                };


                frame_main_novelas.Navigate(new Uri("clases/vista/Novelas.xaml", UriKind.RelativeOrAbsolute));
                
                frame_main_novelas.ContentRendered += (z1, z2) =>
                {
                    Referencia.ventana_novelas = (Novelas)frame_main_novelas.Content;
                };



            };

            //showDlgCargando();


            //agregarARchivosAlPortapales();
            //provarCopiador3();
           // this.cwl("lo mando a copiar");
            //hideDlgCargando();
        }

        ////[STAThread]
        //public static void agregarARchivosAlPortapales()
        //{
        //    StringCollection paths = new StringCollection();
        //    //paths.Add(@"D:\_Cosas\Temporal\ANÁLISIS del DOCK OFICIAL de STEAM DECK - tiene de TODO pero ¿NECESITAS todo lo que tiene.mp4");
        //    paths.Add(@"F:\Paquete\Series\The Witcher El origen de la sangre - [Temp 1] [Caps.04] [1080p] [Latino 5.1 Dual] [8,19 GB]\S01E01.mkv");
        //    Clipboard.Clear(); //Para borrar lo que esté en el Clipboard
        //    Clipboard.SetFileDropList(paths);
        //}


        //[DllImport("user32.dll")]
        //static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

        //[DllImport("user32.dll", SetLastError = true)]
        //static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        //public static void provarCopiador3()
        //{
        //    //abre una ventana del explorer en la ruta indicada
        //    var procceso = Process.Start("explorer.exe", @"C:\_COSAS\temporal\experimentos\b");
        //    Thread.Sleep(1000); //En mi caso esto no era necesario pero lo puse por las dudas

        //    //En el mejor de los casos se debería haber podido usar procceso.MainWindowHandle, pero no tengo idea porque no resulta
        //    //La solución fue buscar la lista de procesos activos, y determinar por el título de la ventana a cual correspondia
        //    Process[] processes = Process.GetProcessesByName("explorer");

        //    foreach (Process proc in processes)
        //    {
        //        if (proc.MainWindowTitle == "b") //prueba es el título de mi ventana
        //        {
        //            //StringCollection paths = new StringCollection();
        //            ////paths.Add(@"D:\_Cosas\Temporal\ANÁLISIS del DOCK OFICIAL de STEAM DECK - tiene de TODO pero ¿NECESITAS todo lo que tiene.mp4");
        //            //paths.Add(@"F:\Paquete\Series\The Witcher El origen de la sangre - [Temp 1] [Caps.04] [1080p] [Latino 5.1 Dual] [8,19 GB]\S01E01.mkv");
        //            //Clipboard.Clear(); //Para borrar lo que esté en el Clipboard
        //            //Clipboard.SetFileDropList(paths);


        //            //paths = Clipboard.GetFileDropList();
        //            //foreach (var item in paths)
        //            //{
        //            //    Console.WriteLine(item);

        //            //}
        //            //Console.WriteLine("-------------------------");
        //            //paths = Clipboard.GetFileDropList();
        //            //foreach (var item in paths)
        //            //{

        //            //    Console.WriteLine(item);

        //            //}

        //            keybd_event(0xA2, 0x9d, 0, 0); //se mantiene la tecla control presionada
        //            PostMessage(proc.MainWindowHandle, 0x0100, 86, 0); //se envía a la ventana la tecla V
        //            Thread.Sleep(1000); //puede que esto tampoco sea necesario
        //            keybd_event(0xA2, 0x9d, 0x0002, 0); //se suelta la tecla control
        //        }


        //    }
        //}



        private void showDlgCargando() {
            if (dlgCargando==null) {
                dlgCargando= new DialogoCargando();
            }
            dlgCargando.ShowDialog();
        }
        private void hideDlgCargando() {
            dlgCargando.Hide();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            //frame_main.Navigate(new Uri("clases/vista/Home.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            //if (Referencia.ContenidoConfiguracion!=null) {
            //    frame_main.Content = Referencia.ContenidoConfiguracion;
            //    return;
            //}



            //Referencia.EA_Configuracion.index_cb_secciones_configuracion = 0;
            setVisibleFrame(frame_main_configuracion);
            Referencia.ventana_Configuracion.actualizar();
            //frame_main.Navigate(new Uri("clases/vista/Configuracion.xaml", UriKind.RelativeOrAbsolute));

            //Referencia.ContenidoConfiguracion = frame_main.Content;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //if (Referencia.ContenidoSeries != null)
            //{
            //    frame_main.Content = Referencia.ContenidoSeries;
            //    return;
            //}

            //frame_main.Navigate(new Uri("clases/vista/Series.xaml", UriKind.RelativeOrAbsolute));
            // Referencia.seccionActual = TipoDeSeccion.SERIES;
            setVisibleFrame(frame_main_series);
            Referencia.ventana_series.actualizar();
            //this.cwl();
            //Grid g = new Grid();
            
            //frame_main.TransformToVisual(g);
            ////GridPrincipal.
            ////Frame f=new Frame();
            ////f.Navigate()

            //Referencia.ContenidoSeries = frame_main.Content;
            //Console.WriteLine();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

            //frame_main.Navigate(new Uri("clases/vista/Peliculas.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

            //frame_main.Navigate(new Uri("clases/vista/Novelas.xaml", UriKind.RelativeOrAbsolute));
        }

        private void alCerrarLaVentanaPrincipal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string urlReproductorV = Referencia.urlReproductorDeVideo;
            if (urlReproductorV.Trim().Length>0&&Archivos.existeArchivo(urlReproductorV)&&Archivos.getExtencion(urlReproductorV).ToLower()==".exe") {
                Referencia.manager.TS().put("ultimoDir_Reproductor", urlReproductorV);
            }
            
        }

        private void alApretar_B_Anime(object sender, RoutedEventArgs e)
        {
            //frame_main_animes.Navigate(new Uri("clases/vista/Series.xaml", UriKind.RelativeOrAbsolute));
            //Referencia.seccionActual = TipoDeSeccion.ANIME;
            setVisibleFrame(frame_main_animes);
            Referencia.ventana_anime.actualizar();
        }

        private void setVisibleFrame(Page p)
        {
            Frame[] frames = { frame_main_animes, frame_main_configuracion, frame_main_home, frame_main_novelas, frame_main_peliculas, frame_main_series };
            foreach (Frame fa in frames)
            {
                if (fa.Content == p)
                {
                    fa.Visibility = Visibility.Visible;
                }
            }
            foreach (Frame fa in frames)
            {
                if (fa.Content != p)
                {
                    fa.Visibility = Visibility.Hidden;
                }
            }
        }
        private void setVisibleFrame(Frame f) {
            Frame[] frames = { frame_main_animes,frame_main_configuracion,frame_main_home,frame_main_novelas,frame_main_peliculas,frame_main_series};
            foreach (Frame fa in frames)
            {
                if (fa==f) {
                    f.Visibility = Visibility.Visible;
                }
            }
            foreach (Frame fa in frames)
            {
                if (fa != f)
                {
                    fa.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
