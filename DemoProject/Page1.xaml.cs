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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page, IDisplay
    {
        Dictionary<string, DisplayItem> BookmarkDict = new Dictionary<string, DisplayItem>();
        Dictionary<string, DisplayItem> SearchDict = new Dictionary<string, DisplayItem>();
        static System.Windows.Forms.Timer SearchTimer = new System.Windows.Forms.Timer();
        private IController Controller;
        private String SearchString;
        private bool PublicSearch = true;

        public Page1()
        {
            InitializeComponent();
            this.BookmarksListView.ItemsSource = BookmarkDict;
            this.SearchListView.ItemsSource = SearchDict;
            SearchTimer.Interval = 1000;
            SearchTimer.Tick += new EventHandler(SearchTimerTick);
            SearchTimer.Enabled = false;
            SearchTimer.Stop();

            MainWindow.ShowPage(this);
        }

        public void Show()
        {
            MainWindow.ShowPage(this);
        }

        private void SearchTimerTick(object sender, EventArgs e)
        {
            SearchTimer.Stop();
            SearchTimer.Interval = 1000;

            if (string.IsNullOrEmpty(SearchString) || string.IsNullOrWhiteSpace(SearchString))
            {
                return;
            }
            if (PublicSearch)
            {
                Controller.HandlePublicSearchString(SearchString);
                return;
            }
            Controller.HandlPrivateSearchString(SearchString);

        }

        public void AddSearchResultToPrivateResult(string id)
        {
            this.Controller.AddIdToBookmarks(id);
        }

        public void DisplayPrivateResults(List<Result> resultList)
        {
            this.BookmarksListView.ItemsSource = null;

            foreach (Result r in resultList)
            {
                if (BookmarkDict.ContainsKey(r.id))
                {
                    continue;
                }

                DisplayItem d = new DisplayItem()
                {
                    id = r.id,
                    title = r.title,
                    bitmapImage = new BitmapImage(new Uri(r.imageUrl))
                };
                BookmarkDict.Add(r.id, d);
            }
            this.BookmarksListView.ItemsSource = BookmarkDict;
        }

        public void DisplayPublicResults(List<Result> resultList)
        {
            this.SearchListView.ItemsSource = null;

            foreach (Result r in resultList)
            {
                Debug.WriteLine(r.id + r.title + r.imageUrl);

                if (SearchDict.ContainsKey(r.id))
                {
                    continue;
                }
                if (string.IsNullOrEmpty(r.imageUrl) || string.IsNullOrWhiteSpace(r.imageUrl))
                {
                    r.imageUrl = @"https://www.publicdomainpictures.net/pictures/280000/velka/not-found-image-15383864787lu.jpg";
                }
                DisplayItem d = new DisplayItem()
                {
                    id = r.id,
                    title = r.title,
                    bitmapImage = new BitmapImage(new Uri(r.imageUrl))
                };

                SearchDict.Add(r.id, d);
            }

            this.SearchListView.ItemsSource = SearchDict;
        }

        public void RemovePrivateResult(string id)
        {
            if (!this.BookmarkDict.ContainsKey(id))
            {
                return;
            }
            this.BookmarkDict.Remove(id);
        }

        public void SetController(IController controller)
        {
            this.Controller = controller;
        }

        private void SearchListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string id = ((dynamic)this.SearchListView.SelectedItem).Key;
            AddSearchResultToPrivateResult(id);
        }

        public void SearchStringChanged(string s)
        {
            this.SearchString = s;
            if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
            {
                SearchTimer.Stop();
                SearchTimer.Interval = 1000;
                return;
            }
            if (SearchTimer.Enabled)
            {
                SearchTimer.Stop();
                SearchTimer.Interval = 1000;
                SearchTimer.Enabled = true;
                SearchTimer.Start();
                return;
            }
            SearchTimer.Interval = 1000;
            SearchTimer.Enabled = true;
            SearchTimer.Start();

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textBox = sender as System.Windows.Controls.TextBox;
            SearchStringChanged(textBox.Text);
        }

        private void BookmarksListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Debug.WriteLine("Double Mouse Click on BookmarksListView");
            //Debug.WriteLine("Selected Index: " + this.BookmarksListView.SelectedIndex);
            try
            {
                string id = ((dynamic)this.BookmarksListView.SelectedItem).Key;
                Controller.SetupDetailedDisplay(id);
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                //do nothing
            }
      
        }

        public void DisplayDetailsDisplay(IDetailedDisplay DetailsDisplay)
        {
            //
        }
    }
}
