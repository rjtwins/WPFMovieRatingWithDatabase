
namespace MovieRatingWithDatabase
{
    public interface IController
    {
        /// <summary>
        /// Add a entry with given unique identifier to the bookmarks database and display it.
        /// </summary>
        /// <param name="id"></param>
        void AddIdToBookmarks(string id);

        /// <summary>
        /// Add a entry with given index in the search results display to the bookmarks database and
        /// display it.
        /// </summary>
        /// <param name="index"></param>
        void AddIdToBookmarks(int index);

        /// <summary>
        /// Search web by a search string.
        /// </summary>
        /// <param name="q"></param>
        void HandlePublicSearchString(string q);

        /// <summary>
        /// Search underlying database with search string.
        /// </summary>
        /// <param name="q"></param>
        void HandlPrivateSearchString(string q);

        /// <summary>
        /// Used to notify implimenting class that the details display has been closed and any data
        /// input in there should be processed.
        /// </summary>
        /// <param name="r"></param>
        void NotifyDetailsDisplayClosing(Result r);

        /// <summary>
        /// Remove given Result from the bookmarks and underlaying database.
        /// </summary>
        /// <param name="r"></param>
        void RemoveBookmark(Result r);

        /// <summary>
        /// Set up the display that will show the details of the result with given ID.
        /// </summary>
        /// <param name="id"></param>
        void SetupDetailedDisplay(string id);

        /// <summary>
        /// Used to indicate that a web search for content shoud be stopped
        /// </summary>
        void StopSearch();
    }
}
