using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace MovieRatingWithDatabase
{
    public static class UTILS
    {
        public static Passwords Passwords;

        public static void DisplayFullBitmap(Bitmap img)
        {
            using (Form form = new Form())
            {
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Size = img.Size;

                PictureBox pb = new PictureBox();
                pb.Dock = DockStyle.Fill;
                pb.Image = img;

                form.Controls.Add(pb);
                form.ShowDialog();
            }
        }

        public static int LimitToRange(this int value, int inclusiveMinimum, int inclusiveMaximum)
        {
            if (value < inclusiveMinimum) { return inclusiveMinimum; }
            if (value > inclusiveMaximum) { return inclusiveMaximum; }
            return value;
        }

        public static object[] ResultItemArray(Result result)
        {
            if (SQLDatabaseInterface.SqlDataColumns == null)
            {
                throw new Exception("SQLSchema was not loaded yet please make sure to load that first.");
            }
            if (SQLDatabaseInterface.SqlDataColumns.Count == 0)
            {
                throw new Exception("SQLSchema countains no columns.");
            }

            int nrColumns = SQLDatabaseInterface.SqlDataColumns.Count;
            object[] itemArray = new object[nrColumns];

            for (int i = 0; i < nrColumns; i++)
            {
                string columnName = SQLDatabaseInterface.SqlDataColumns[i].ColumnName;
                var propInfo = result.GetType().GetProperty(columnName);
                if (propInfo == null)
                {
                    continue;
                }
                var value = propInfo.GetValue(result);
                if (value != DBNull.Value && value != null)
                {
                    itemArray[i] = value;
                    continue;
                }
                if (propInfo.PropertyType == typeof(string))
                {
                    itemArray[i] = "";
                    continue;
                }
                if (propInfo.PropertyType == typeof(int))
                {
                    itemArray[i] = 0;
                    continue;
                }
            }
            return itemArray;
        }

        public static Result RowToResult(DataRow row)
        {
            Result result = new Result();
            string[] propertyNames = new string[row.Table.Columns.Count];

            for (int i = 0; i < propertyNames.Length; i++)
            {
                string columnName = row.Table.Columns[i].ColumnName;
                var propInfo = result.GetType().GetProperty(columnName);
                //Debug.WriteLine("Matching: " + columnName + " value: " + row[i]);
                if (propInfo == null)
                {
                    continue;
                }
                var value = row[i];
                if (value != DBNull.Value && value != null)
                {
                    propInfo.SetValue(result, row[i], null);
                    continue;
                }
                if (propInfo.PropertyType == typeof(string))
                {
                    propInfo.SetValue(result, "", null);
                    continue;
                }
                if (propInfo.PropertyType == typeof(int))
                {
                    propInfo.SetValue(result, 0, null);
                    continue;
                }
            }
            return result;
        }
    }
}