using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Windows.Forms;

namespace Porpose
{
    class WorkDbf
    {
        //Создаем обект OdbcConnection для работы с подключением
        private readonly OdbcConnection _conn = null;

        //Конструктор
        public WorkDbf()
        {
            this._conn = new OdbcConnection();
        }

        // Внутренний метод выполнения, открытие подключения выполнение нужного запроса и возвращение результата
        private DataTable Execute(string command)
        {
            DataTable dt = null;
            if (_conn != null)
            {
                try
                {
                    _conn.Open();
                    dt = new DataTable();
                    OdbcCommand oCmd = _conn.CreateCommand();
                    oCmd.CommandText = command;
                    dt.Load(oCmd.ExecuteReader());
                    _conn.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            return dt;
        }

        /// <summary>
        /// Получить все данные из выбранного DBF файла
        /// </summary>
        /// <param name="dbfFullFileName">DBF файл с которым хотим работать</param>
        /// <returns>Возвращает заполненный данными DataTable</returns>
        public DataTable GetAll(string dbfFullFileName)
        {

            string mpath = Path.GetDirectoryName(dbfFullFileName);
            string mfile = Path.GetFileName(dbfFullFileName);

            _conn.ConnectionString = _conn.ConnectionString = "Driver={Microsoft dBase Driver (*.dbf)};SourceType=DBF;" + ";Exclusive=No; NULL=NO;DELETED=NO;BACKGROUNDFETCH=NO;SourceDB=" + mpath;
            return Execute("SELECT * FROM " + mpath + "\\" + mfile);
        }

        /// <summary>
        /// Получить данные соответствуюшие нашему запросу
        /// </summary>
        /// <param name="dbfFullFileName">DBF файл с которым хотим работать</param>
        /// <param name="query">Текстовый SQL запрос на получение данных из указанного файла</param>
        /// <returns>Возвращает заполненный данными DataTable</returns>
        public DataTable ExecuteQuery(string dbfFullFileName, string query)
        {

            string mpath = Path.GetDirectoryName(dbfFullFileName);
            string mfile = Path.GetFileName(dbfFullFileName);

            _conn.ConnectionString = _conn.ConnectionString = "Driver={Microsoft dBase Driver (*.dbf)};SourceType=DBF;" + ";Exclusive=No; NULL=NO;DELETED=NO;BACKGROUNDFETCH=NO;SourceDB=" + mpath;  
			// ВОТ ТУТ КРОЕТСЯ ОШИБКА ТАК КАК ЗАПРОС ДОПУСТИМ С WHERE ТУТ НЕ ВЫПОЛНИТЬ т.к. он представляет что то вроде SELECT * FROM (FILE PATH)\\(FILENAME) WHERE X=100
			// ЕСЛИ ЕСТЬ ПРЕДЛОЖЕНИЯ КАК ДОРАБОТАТЬ ПИШИТЕ
            return Execute(query + " " + mpath + "\\" + mfile);
        }


    }
}
