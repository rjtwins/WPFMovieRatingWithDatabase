using MovieRatingWithDatabase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private DataObject Data = new DataObject()
        {
            Result = new Result
            {
                id = "",
                title = "",
                year = 0,
                season = 0
            },
            BitmapImage = null,
            IntArray = new int[] { 1, 2, 3, 4, 5 },
            Watched = false,
            DateTimeString = "",
            MovieOrTv = ""
        };


        IController Controller;

        public Page2()
        {
            //public Result Result { get; set; }
            //public BitmapImage BitmapImage { get; set; }
            //public int[] IntArray { get; set; }
            //public bool Watched { get; set; }
            //public string DateTimeString { get; set; }
            //public string MovieOrTv { get; set; }
            InitializeComponent();
            DataContext = this;
        }

        public void AddDetails(Result r)
        {
            Data.BitmapImage = new BitmapImage(new Uri(r.imageUrl));
            Data.Result = r;
            HandleMovieOrTv(r);
            HandleWatchedString(r);
            handleDateTimeString();
            HandleRatingInteger(r);

            Debug.WriteLine("DATA OUTPUT: ");
            Debug.WriteLine(Data.Result.id);
            Debug.WriteLine(Data.Result.title);
            Debug.WriteLine(Data.Result.season);
            Debug.WriteLine(Data.Watched);
            Debug.WriteLine(Data.DateTimeString);
            Debug.WriteLine(Data.MovieOrTv);


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
            //this.RatingMenu.SelectedIndex = r.rating - 1;
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

        //private void OnFormClosingEvent(object sender, FormClosingEventArgs e)
        //{
        //    this.Result.notes = this.textBox1.Text;
        //    HandleWatchedCheckBox();
        //    this.Result.rating = this.ratingComboBox.SelectedIndex + 1;
        //    controller.NotifyDetailsDisplayClosing(this.Result);
        //}

        //private void HandleWatchedCheckBox()
        //{
        //    if (this.checkBox1.Checked)
        //    {
        //        this.Result.watched = "TRUE";
        //        this.Result.watchDate = DateTime.Parse(this.watchedDate.Text);
        //        return;
        //    }
        //    this.Result.watched = "FALSE";
        //}

        //private void checkBox1_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.checkBox1.Checked)
        //    {
        //        this.checkBox1.Enabled = false;
        //        this.watchedDate.Text = DateTime.Now.ToString();
        //        this.Result.watchDate = DateTime.Now;
        //    }
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //    controller.RemoveBookmark(Result);
        //}

        public void Show()
        {

            MainWindow.ShowPage(this);
        }

        public class DataObject
        {
            public Result Result { get; set; }
            public BitmapImage BitmapImage { get; set; }
            public int[] IntArray { get; set; }
            public bool Watched { get; set; }
            public string DateTimeString { get; set; }
            public string MovieOrTv { get; set; }
        }
    }
}
