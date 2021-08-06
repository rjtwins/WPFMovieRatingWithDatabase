using System.Collections.Generic;
using System.Data;

public interface IDatabaseInterface
{
    /// <summary>
    /// Get all (rows) data in database and return it as a ResultDataSet table.
    /// </summary>
    /// <returns>ResultDataSet.resultDataTable table</returns>
    DataTable GetAllFromDatabase();

    /// <summary>
    /// Get a row with a specific key from database and return it as a row in ResultDataSet.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>ResultDataSet.resultRow row</returns>
    DataRow GetFromDatabase(string id);

    /// <summary>
    /// Put a given row in the database.
    /// </summary>
    /// <param name="row"></param>
    void PutInDatabase(object[] itemArray);

    /// <summary>
    /// Search given database with a specific query and return resutls as ResultDataSet table.
    /// </summary>
    /// <param name="q"></param>
    /// <returns>ResultDataSet.resultDataTable table</returns>
    List<DataRow> SearchInDatabase(string q);

    /// <summary>
    /// Remove a (row) data entry from the database with given key.
    /// </summary>
    /// <param name="id"></param>
    void TryRemoveFromDatabase(string id);

    /// <summary>
    /// Update given (row) entry in database
    /// </summary>
    /// <param name="row"></param>
    void UpdateInDatabase(object[] itemArray);
}