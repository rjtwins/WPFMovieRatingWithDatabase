using MovieRatingWithDatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI
{

    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page, IDetailedDisplay
    {
        IController Controller;

        public DataObject Data = new DataObject()
        {
            Result = new Result
            {
                id = "",
                title = "",
                year = 0,
                season = 0
            },
            BitmapImage = null,
            ComboList = new List<string>() { "1", "2", "3", "4", "5" },
            Watched = false,
            DateTimeString = "",
            MovieOrTv = ""
        };

        public Page2()
        {
            InitializeComponent();
            this.DataContext = this.Data;
        }

        public void AddDetails(Result r)
        {
            Data.BitmapImage = new BitmapImage(new Uri(r.imageUrl));
            Data.Result = r;
            HandleMovieOrTv(r);
            HandleWatchedString(r);
            handleDateTimeString();
            HandleRatingInteger(r);

            //Debug.WriteLine("DATA OUTPUT: ");
            //Debug.WriteLine(Data.Result.id);
            //Debug.WriteLine(Data.Result.title);
            //Debug.WriteLine(Data.Result.season);
            //Debug.WriteLine(Data.Watched);
            //Debug.WriteLine(Data.DateTimeString);
            //Debug.WriteLine(Data.MovieOrTv);
            //Debug.WriteLine(Data.ComboList[0]);

        }

        private void handleDateTimeString()
        {
            if (!Data.Watched)
            {
                Data.DateTimeString = "---";
                return;
            }
            Data.DateTimeString = Data.Result.watchDate.ToString();
        }

        private void HandleRatingInteger(Result r)
        {
            r.rating = UTILS.LimitToRange(r.rating, 1, 5);
            this.RatingBox.SelectedIndex = r.rating -1;
        }

        private void HandleWatchedString(Result r)
        {
            if (r.watched.Trim(' ') == "TRUE")
            {
                Data.Watched = true;
                return;
            }
            Data.Watched = false;
        }

        private void HandleMovieOrTv(Result r)
        {
            if (r.titleType == "movie")
            {
                Data.MovieOrTv = "Movie";
                return;
            }
            Data.MovieOrTv = "TV Show";
        }

        public void RegisterController(IController controller)
        {
            this.Controller = controller;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HandleWatchedCheckBox();
            this.Data.Result.rating = UTILS.LimitToRange(this.RatingBox.SelectedIndex + 1, 1, 5);
            Controller.NotifyDetailsDisplayClosing(this.Data.Result);
        }

        private void HandleWatchedCheckBox()
        {
            if (Data.Watched)
            {
                this.Data.Result.watched = "TRUE";
                this.Data.Result.watchDate = DateTime.Parse(this.Data.DateTimeString);
                return;
            }
            this.Data.Result.watched = "FALSE";
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("Checkbox Checked");
            WatchedCheckBox.IsEnabled = false;
            this.Data.Result.watchDate = DateTime.Now;
            this.Data.DateTimeString = DateTime.Now.ToString();
            //Debug.WriteLine(Data.DateTimeString);
        }

        public void Show()
        {

            MainWindow.ShowPage(this);
        }

        public class DataObject : INotifyPropertyChanged
        {
            private string _DateTimeString;
            public Result Result { get; set; }
            public BitmapImage BitmapImage { get; set; }
            public List<string> ComboList { get; set; }
            public bool Watched { get; set; }
            public string DateTimeString
            {
                get { return _DateTimeString; }
                set
                {
                    _DateTimeString = value;
                    OnPropertyChanged();
                }
            }
            public string MovieOrTv { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        public class ComboData
        {
           public string Id { get; set; }
           public string Value { get; set; }
        }
    }
}