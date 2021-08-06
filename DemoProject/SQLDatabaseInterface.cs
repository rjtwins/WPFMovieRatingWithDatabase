using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace MovieRatingWithDatabase
{
    internal class SQLDatabaseInterface : IDatabaseInterface
    {
        //This is used to dynamically fill and create new rows based in object property names and types.
        public static DataColumnCollection SqlDataColumns;

        private MySqlDataAdapter SQLDataAdaptor;
        private DataTable InternalDataTable = new DataTable();
        private MySqlConnection cnx;

        public SQLDatabaseInterface()
        {
            //Debug.WriteLine(UTILS.Passwords.SQLAccountName);
            //Debug.WriteLine(UTILS.Passwords.SQLDatabase);
            //Debug.WriteLine(UTILS.Passwords.SQLAddress);
            //Debug.WriteLine(UTILS.Passwords.SQLPort);
            //Debug.WriteLine(UTILS.Passwords.SQLPassword);
            //Debug.WriteLine(UTILS.Passwords.xrapidapikey);

            MySqlConnectionStringBuilder bd = new MySqlConnectionStringBuilder
            {
                Server = UTILS.Passwords.SQLAddress,
                Port = uint.Parse(UTILS.Passwords.SQLPort),
                UserID = UTILS.Passwords.SQLAccountName,
                Password = UTILS.Passwords.SQLPassword,
                Database = UTILS.Passwords.SQLDatabase,
                ConnectionTimeout = 60
            };

            ConnectAndSetupAdaptor(bd);
            GetInternalDataTable(bd);
            SetupSQLAdaptorCommands();
        }

        private void ConnectAndSetupAdaptor(MySqlConnectionStringBuilder bd)
        {
            cnx = new MySqlConnection(bd.ToString());
            string sqlCmd = "SELECT * FROM result";
            SQLDataAdaptor = new MySqlDataAdapter(sqlCmd, cnx);
        }

        private void GetInternalDataTable(MySqlConnectionStringBuilder bd)
        {
            SQLDataAdaptor.SelectCommand.CommandType = CommandType.Text;
            SQLDataAdaptor.Fill(InternalDataTable);
            InternalDataTable.PrimaryKey = new DataColumn[] { InternalDataTable.Columns[0] };
            SqlDataColumns = InternalDataTable.Columns;
        }

        private void SetupSQLAdaptorCommands()
        {
            MySqlCommandBuilder cb = new MySqlCommandBuilder(SQLDataAdaptor);
            SQLDataAdaptor.InsertCommand = cb.GetInsertCommand();
            SQLDataAdaptor.DeleteCommand = cb.GetDeleteCommand();
            SQLDataAdaptor.UpdateCommand = cb.GetUpdateCommand();
        }

        public DataTable GetAllFromDatabase()
        {
            return InternalDataTable;
        }

        DataRow IDatabaseInterface.GetFromDatabase(string id)
        {
            if (InternalDataTable.Rows.Contains(id))
            {
                return InternalDataTable.Rows.Find(id);
            }
            return null;
        }

        public void PutInDatabase(object[] itemArray)
        {
            string id = itemArray[0] as string;
            if (this.InternalDataTable.Rows.Contains(id))
            {
                UpdateInDatabase(itemArray);
            }
            InternalDataTable.Rows.Add(itemArray);
            SQLDataAdaptor.Update(InternalDataTable);
        }

        public List<DataRow> SearchInDatabase(string q)
        {
            List<DataRow> resultRows = new List<DataRow>();
            for (int i = 0; i < InternalDataTable.Rows.Count; i++)
            {
                foreach (var column in InternalDataTable.Rows[i].ItemArray)
                {
                    if (resultRowsContainsRow(i))
                    {
                        continue;
                    }
                    if (IsNotString(column))
                    {
                        continue;
                    }
                    string value = column as string;
                    if (value.ToLower().Contains(q.ToLower()))
                    {
                        resultRows.Add(InternalDataTable.Rows[i]);
                    }
                }
            }

            return resultRows;

            bool resultRowsContainsRow(int i)
            {
                return resultRows.Contains(InternalDataTable.Rows[i]);
            }

            bool IsNotString(object column)
            {
                return column.GetType() != typeof(string);
            }
        }

        public void TryRemoveFromDatabase(string id)
        {
            if (!InternalDataTable.Rows.Contains(id))
            {
                return;
            }
            InternalDataTable.Rows.Remove(InternalDataTable.Rows.Find(id));
            SQLDataAdaptor.Update(InternalDataTable);
        }

        public void UpdateInDatabase(object[] itemArray)
        {
            string id = itemArray[0] as string;
            if (!InternalDataTable.Rows.Contains(id))
            {
                throw new Exception("Row with key: " + itemArray[0] as string + " is not present in the datatable!");
            }
            DataRow row = InternalDataTable.Rows.Find(id);
            row.ItemArray = itemArray;
            SQLDataAdaptor.Update(InternalDataTable);
        }
    }
}