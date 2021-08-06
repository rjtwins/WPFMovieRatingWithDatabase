using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MovieRatingWithDatabase;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Window;
        public IDisplay MainPage;
        public MainWindow()
        {
            InitializeComponent();
            MainWindow.Window = this;
            MainPage = new Page1();
        }

        public void SetController(IController Controller)
        {
            MainPage.SetController(Controller);
        }

        public static void ShowPage(Page page)
        {
            MainWindow.Window.Frame.Navigate(page);
            MainWindow.Window.Window_SizeChanged(null, null);
        }
        
        public static void ClosePage()
        {
            MainWindow.Window.Frame.Content = null;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Debug.WriteLine("Size of window: " + this.Height + ":" + this.Width);
            //Debug.WriteLine("Size of frame: " + Frame.Height + ":" + Frame.Width);
            try
            {
                ///Debug.WriteLine("Size of page: " + (Frame.Content as Page).Height + ":" + (Frame.Content as Page).Width);
                (Frame.Content as Page).Height = this.Rectangle.Height;
                (Frame.Content as Page).Width = this.Rectangle.Width;
            }
            catch(System.NullReferenceException ex)
            {
                Debug.WriteLine("Size of page: NO WINDOW FOUND");
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            Window_SizeChanged(sender, null);
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            Window_SizeChanged(sender, null);
        }
    }

    public class DisplayItem
    {
        public string id { get; set; }
        public string title { get; set; }
        public BitmapImage bitmapImage { get; set; }
    }
}
