using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

//using FirebirdSql.Data.Common;

namespace FirebirdTest
{
    public partial class FormMain : Form
    {
        //FirebirdSql.Data.FirebirdClient.
        FbConnection fb;

        public FormMain()
        {
            InitializeComponent();
        }
     
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            //формируем connection string для последующего соединения с нашей базой данных
            FbConnectionStringBuilder fb_con = new FbConnectionStringBuilder();
            fb_con.Charset = "WIN1251"; //используемая кодировка
            fb_con.UserID = textBoxLogin.Text; //логин
            fb_con.Password = textBoxPass.Text; //пароль
            fb_con.Database = textBoxDBPath.Text; //путь к файлу базы данных
            fb_con.ServerType = 0; //указываем тип сервера (0 - "полноценный Firebird" (classic или super server), 1 - встроенный (embedded))

            //создаем подключение
            fb = new FbConnection(fb_con.ToString()); //передаем нашу строку подключения объекту класса FbConnection

            fb.Open(); //открываем БД

            FbDatabaseInfo fb_inf = new FbDatabaseInfo(fb); //информация о БД

            //пока у объекта БД не был вызван метод Open() - никакой информации о БД не получить, будет только ошибка
            MessageBox.Show("Info: " + fb_inf.ServerClass + "; " + fb_inf.ServerVersion); //выводим тип и версию используемого сервера Firebird
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            //так проверять состояние соединения (активно или не активно)
            if (fb.State == ConnectionState.Closed)
                fb.Open();


            FbTransaction fbt = fb.BeginTransaction(); //стартуем транзакцию; стартовать транзакцию можно только для открытой базы (т.е. мутод Open() уже был вызван ранее, иначе ошибка)


            FbCommand SelectSQL = new FbCommand("SELECT * FROM SPISOK_ST", fb); //задаем запрос на выборку


            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
            FbDataReader reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()


            string select_result = ""; //в эту переменную будем складывать результат запроса Select
            //textBoxResult.Clear();


            try
            {
                while (reader.Read()) //пока не прочли все данные выполняем...
                {
                    // select_result = select_result + reader.GetInt32(0).ToString() + ", " + reader.GetString(1) + "\n";
                    select_result = select_result + reader.GetString(0) + " " + reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3) + "\n";
                    //textBoxResult.Text += reader.GetString(0) + " " + reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3) + "\n";
                }
            }
            finally
            {
                //всегда необходимо вызывать метод Close(), когда чтение данных завершено
                reader.Close();
                fb.Close(); //закрываем соединение, т.к. оно нам больше не нужно
            }
            MessageBox.Show(select_result); //выводим результат запроса
            SelectSQL.Dispose(); //в документации написано, что ОЧЕНЬ рекомендуется убивать объекты этого типа, если они больше не нужны
        }

        private void buttonSaveXML_Click(object sender, EventArgs e)
        {
            xmlPackageGenerator xmlGen = new xmlPackageGenerator();
            xmlGen.CreatePackageData("..\\..\\..\\sample.xml");
        }
    }
}
