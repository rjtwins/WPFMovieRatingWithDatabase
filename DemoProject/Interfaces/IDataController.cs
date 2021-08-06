using MovieRatingWithDatabase;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;

namespace MovieRatingWithDatabase
{
    public abstract class IDataController
    {
        public IDatabaseInterface DatabaseInterface;
        private protected bool StopSearchFlag = false;

        /// <summary>
        /// Allow furture web search.
        /// </summary>
        public virtual void EnableSearch()
        {
            this.StopSearchFlag = false;
        }

        /// <summary>
        /// Get all entries from database.
        /// </summary>
        /// <returns>List <Result>results</Result></returns>
        public virtual List<Result> GetAllFromDatabase()
        {
            List<Result> results = new List<Result>();
            var dataTable = DatabaseInterface.GetAllFromDatabase();
            foreach (DataRow row in dataTable.Rows)
            {
                Result r = UTILS.RowToResult(row);
                results.Add(r);
            }
            return results;
        }

        /// <summary>
        /// Async get a Bitmap from a given image url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Task<Bitmap> getBitmapTask</Bitmap></returns>
        public async virtual Task<Bitmap> GetBitMapFromURL(string url)
        {
            WebRequest requ = WebRequest.Create(url);
            WebResponse resp = await requ.GetResponseAsync();
            System.IO.Stream respStream = resp.GetResponseStream();
            Bitmap bmp = new Bitmap(respStream);
            respStream.Dispose();
            return bmp;
        }

        /// <summary>
        /// Get a entry from underlaying database by unique id key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Result result</returns>
        public virtual Result GetFromDatabase(string id)
        {
            DataRow row = DatabaseInterface.GetFromDatabase(id);
            return UTILS.RowToResult(row);
        }

        public abstract void LoadPasswordFile();

        /// <summary>
        /// Remove given Result from database.
        /// </summary>
        /// <param name="r"></param>
        public virtual void RemoveFromDataBase(Result r)
        {
            this.DatabaseInterface.TryRemoveFromDatabase(r.id);
        }

        /// <summary>
        /// Search database by given title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns>List<result> results</result></returns>
        public virtual List<Result> SearchDatabase(string title)
        {
            List<Result> results = new List<Result>();
            foreach (DataRow row in DatabaseInterface.SearchInDatabase(title))
            {
                results.Add(UTILS.RowToResult(row));
            }
            return results;
        }

        /// <summary>
        /// Search web by a unique ID (this may only work for cirtain APIs)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Result result</returns>
        public abstract Result SearchWebByID(string id);

        /// <summary>
        /// Search internet by a given title and return as list.
        /// </summary>
        /// <param name="title"></param>
        /// <returns>List<Result> results</Result></returns>
        public abstract List<Result> SearchWebByTitle(string title);

        /// <summary>
        /// Stop current and future websearch.
        /// </summary>
        public virtual void StopSearch()
        {
            this.StopSearchFlag = true;
        }

        /// <summary>
        /// Update-Add a result in the database.
        /// </summary>
        /// <param name="r"></param>
        public virtual void StoreResultInDatabase(Result r)
        {
            DatabaseInterface.PutInDatabase(UTILS.ResultItemArray(r));
        }

        /// <summary>
        /// Update given result in database.
        /// </summary>
        /// <param name="r"></param>
        public virtual void UpdateInDatabase(Result r)
        {
            DatabaseInterface.UpdateInDatabase(UTILS.ResultItemArray(r));
        }
    }
}