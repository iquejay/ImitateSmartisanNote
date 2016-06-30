using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SQLite.Linq;

namespace ImitateSmartisanNote
{
    public class NotesDataProvider
    {
        public void Init()
        {
            try
            {
                //判断表是否存在，不存在则创建
                string tableName = "NotesTable";
                string connectString = @"Data Source=notesData.db;Pooling=true;FailIfMissing=false";
                using (SQLiteConnection conn = new SQLiteConnection(connectString))
                {
                    conn.Open();

                    SQLiteCommand cmd = conn.CreateCommand();

                    cmd.CommandText = string.Format(@"select count(*)  from sqlite_master where type = 'table' and name = '{0}'", tableName);
                    cmd.CommandType = System.Data.CommandType.Text;

                    var nTableCount = (Int64)cmd.ExecuteScalar();
                    if (nTableCount <= 0)
                    {
                        //读取已经存储的空数据库sql语句文件。创建新表
                        var createTablesSQL = System.IO.File.ReadAllText(string.Format(@"backSQL\{0}.sql", tableName));
                        if (!string.IsNullOrWhiteSpace(createTablesSQL))
                        {
                            //创建新表
                            cmd.CommandText = createTablesSQL;
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.ExecuteScalar();
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }

            
        }

        public IEnumerable<NoteItemModel> LoadData(string filterstr="")
        {
            var noteItems = new List<NoteItemModel>();

            string connectString = @"Data Source=notesData.db;Pooling=true;FailIfMissing=false";
            SQLiteConnection conn = new SQLiteConnection(connectString);
            conn.Open();

            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from NotesTable "+filterstr; //表
            cmd.CommandType = System.Data.CommandType.Text;

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    noteItems.Add(new NoteItemModel(DateTime.Parse(reader["DateTime"].ToString()), reader["Note"].ToString()) {
                        IsStared = reader["IsStared"].ToString() == "1" ? true : false,
                        Identity = reader["Indentity"].ToString()
                    });
                }

                noteItems.Sort((a, b) => {
                    int seconds = (b.DateAndTime - a.DateAndTime).Seconds;
                    return seconds>0?1:seconds==0?0:-1; });
            }

            return noteItems;
        }
        public void InsertData(NoteItemModel noteItem)
        {
            string connectString = @"Data Source=notesData.db;Pooling=true;FailIfMissing=false";
            SQLiteConnection conn = new SQLiteConnection(connectString);
            conn.Open();

            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = string.Format("insert into NotesTable(DateTime,Note,IsStared,Indentity) values(\"{0:s}\",\"{1:s}\",{2:d},{3:s})", noteItem.DateAndTime.ToString(),noteItem.Note,noteItem.IsStared?1:0,noteItem.Identity); //表
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.ExecuteScalar();
        }
        public void DeleteData(NoteItemModel noteItem)
        {
            string connectString = @"Data Source=notesData.db;Pooling=true;FailIfMissing=false";
            SQLiteConnection conn = new SQLiteConnection(connectString);
            conn.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = string.Format("delete from NotesTable where Indentity='{0:s}'",noteItem.Identity); //表
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.ExecuteScalar();
        }
        public void UpdateData(NoteItemModel noteItem,bool ContentChanged=false)
        {
            string connectString = @"Data Source=notesData.db;Pooling=true;FailIfMissing=false";
            SQLiteConnection conn = new SQLiteConnection(connectString);
            conn.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            if(!ContentChanged)
                cmd.CommandText = string.Format("update NotesTable set IsStared='{0:d}' where Indentity='{1:s}'", noteItem.IsStared?1:0, noteItem.Identity); //表
            else
                cmd.CommandText = string.Format("update NotesTable set Note='{0:s}',DateTime='{1:s}',IsStared='{2:d}' where Indentity='{3:s}'",noteItem.Note,noteItem.DateAndTime.ToString(),noteItem.IsStared?1:0, noteItem.Identity); //表
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.ExecuteScalar();
        }
    }
}
