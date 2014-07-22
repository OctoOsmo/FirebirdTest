using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FirebirdSql.Data.FirebirdClient;

namespace FirebirdTest
{
    class xmlPackageGenerator
    {

        //FbConnection fb_main, fb;
        FbConnection fb;

        string login, password, db_path;

        //public xmlPackageGenerator(FbConnection fb)
        //{
        //    this.fb = fb;
        //}

        private FbConnection CreateFbConnection()
        {
            FbConnection fbc;
            //формируем connection string для последующего соединения с нашей базой данных
            FbConnectionStringBuilder fb_con = new FbConnectionStringBuilder();
            fb_con.Charset = "WIN1251"; //используемая кодировка
            fb_con.UserID = login; //логин
            fb_con.Password = password; //пароль
            fb_con.Database = db_path; //путь к файлу базы данных
            fb_con.ServerType = 0; //указываем тип сервера (0 - "полноценный Firebird" (classic или super server), 1 - встроенный (embedded))

            //создаем подключение
            fbc = new FbConnection(fb_con.ToString()); //передаем нашу строку подключения объекту класса FbConnection

            fbc.Open(); //открываем БД
            return fbc;
        }

        public xmlPackageGenerator(string login, string password, string db_path)
        {
            this.login = login;
            this.password = password;
            this.db_path = db_path;
            this.fb = CreateFbConnection();
        }

        private XmlNode InsertTag(XmlDocument document, XmlNode parent, string nodeName, string node_value)
        {
            if (node_value.Length == 0)
            {
                XmlNode Node = document.CreateElement(nodeName);// даём имя
                parent.AppendChild(Node);// и указываем кому принадлежит
                return Node;
            }
            else
            {
                XmlNode Node = document.CreateElement(nodeName);// даём имя
                Node.InnerText = node_value;// и значение
                parent.AppendChild(Node);// и указываем кому принадлежит
                return Node;
            }
        }

        private void AddAuthData(XmlDocument document)
        {
            XmlNode AuthData = document.CreateElement("AuthData"); // даём имя
            document.DocumentElement.AppendChild(AuthData); // и указываем кому принадлежит
            InsertTag(document, AuthData, "Login", "at@vsma.ac.ru");
            InsertTag(document, AuthData, "Pass", "BRalQhv");       
        }

        private string GetRegistrationDate(int nom_ab)
        {
            DateTime reg_date = DateTime.Now;
            //проверка состояния соединения (активно или не активно)
            if (this.fb.State == System.Data.ConnectionState.Closed)
                this.fb.Open();
            FbTransaction fbt = fb.BeginTransaction(); //стартуем транзакцию; стартовать транзакцию можно только для открытой базы (т.е. мутод Open() уже был вызван ранее, иначе ошибка)            
            string sql_text = "select KOPIA, DAT_PZ, USER_KOPIA_DAT_NA_0 from ABIT where NOM_AB=" + nom_ab.ToString();
            FbCommand SelectSQL = new FbCommand(sql_text, fb); //задаем запрос на выборку
            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
            FbDataReader regDateReader = SelectSQL.ExecuteReader();
            try
            {
                regDateReader.Read();
                if (0 == regDateReader.GetInt32(0))
                {
                    if ("" != regDateReader.GetString(2))
                    {
                        reg_date = regDateReader.GetDateTime(2);
                    }
                }
                else reg_date = regDateReader.GetDateTime(1);
            }
            finally
            {
                regDateReader.Close();
                fb.Close();
            }
            regDateReader.Close();
            SelectSQL.Dispose();
            return reg_date.ToString();
        }

        private string GetName(int nom_ab)
        {
            string name = "";
            //проверка состояния соединения (активно или не активно)
            if (this.fb.State == System.Data.ConnectionState.Closed)
                this.fb.Open();
            FbTransaction fbt = fb.BeginTransaction(); //стартуем транзакцию; стартовать транзакцию можно только для открытой базы (т.е. мутод Open() уже был вызван ранее, иначе ошибка)            
            string sql_text = "Select NAM from ABIT where NOM_AB=" + nom_ab.ToString();
            FbCommand SelectSQL = new FbCommand(sql_text, fb); //задаем запрос на выборку
            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
            FbDataReader Reader = SelectSQL.ExecuteReader();
            try
            {
                Reader.Read();
                name = Reader.GetString(0);
            }
            finally
            {
                Reader.Close();
                fb.Close();
            }
            Reader.Close();
            SelectSQL.Dispose();
            return name;
        }

        private string GetMiddleName(int nom_ab)
        {
            string MiddleName = "";
            //проверка состояния соединения (активно или не активно)
            if (this.fb.State == System.Data.ConnectionState.Closed)
                this.fb.Open();
            FbTransaction fbt = fb.BeginTransaction(); //стартуем транзакцию; стартовать транзакцию можно только для открытой базы (т.е. мутод Open() уже был вызван ранее, иначе ошибка)            
            string sql_text = "Select OTCH from ABIT where NOM_AB=" + nom_ab.ToString();
            FbCommand SelectSQL = new FbCommand(sql_text, fb); //задаем запрос на выборку
            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
            FbDataReader Reader = SelectSQL.ExecuteReader();
            try
            {
                Reader.Read();
                MiddleName = Reader.GetString(0);
            }
            finally
            {
                Reader.Close();
                fb.Close();
            }
            Reader.Close();
            SelectSQL.Dispose();
            return MiddleName;
        }

        private string GetLastName(int nom_ab)
        {
            string LastName = "";
            //проверка состояния соединения (активно или не активно)
            if (this.fb.State == System.Data.ConnectionState.Closed)
                this.fb.Open();
            FbTransaction fbt = fb.BeginTransaction(); //стартуем транзакцию; стартовать транзакцию можно только для открытой базы (т.е. мутод Open() уже был вызван ранее, иначе ошибка)            
            string sql_text = "Select FAM from ABIT where NOM_AB=" + nom_ab.ToString();
            FbCommand SelectSQL = new FbCommand(sql_text, fb); //задаем запрос на выборку
            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
            FbDataReader Reader = SelectSQL.ExecuteReader();
            try
            {
                Reader.Read();
                LastName = Reader.GetString(0);
            }
            finally
            {
                Reader.Close();
                fb.Close();
            }
            Reader.Close();
            SelectSQL.Dispose();
            return LastName;
        }

        private void AddEntrant(XmlDocument document, XmlNode parent, int nom_ab)
        {
            XmlNode Entrant = InsertTag(document, parent, "Entrant", "");
            InsertTag(document, Entrant, "UID", "2014" + nom_ab.ToString());
            InsertTag(document, Entrant, "FirstName", GetName(nom_ab));
            InsertTag(document, Entrant, "MiddleName", GetMiddleName(nom_ab));
            InsertTag(document, Entrant, "LastName", GetLastName(nom_ab));
        }

        private void AddApplication(XmlDocument document, XmlNode parent, int nom_af, int nom_ab)
        {
            string reg_date = GetRegistrationDate(nom_ab);//дата регистрации заявления
            XmlNode application = InsertTag(document, parent, "application", "");
            InsertTag(document, application, "UID", "20142014" + nom_af.ToString());
            InsertTag(document, application, "ApplicationNumber", "2014" + (nom_af + 1).ToString());
            AddEntrant(document, application, nom_ab);
            InsertTag(document, application, "RegistrationDate", reg_date);
            //InsertTag(document, application, "NeedHostel", "0");
            //InsertTag(document, application, "StatusID", "4");
        }

        //private FbDataReader GetReader(string sql_text)
        //{
        //    FbConnection fb_conn = CreateFbConnection();
        //    //проверка состояния соединения (активно или не активно)
        //    if (fb_conn.State == System.Data.ConnectionState.Closed)
        //        fb_conn.Open();
        //    FbTransaction fbt = fb_conn.BeginTransaction(); //стартуем транзакцию; стартовать транзакцию можно только для открытой базы (т.е. мутод Open() уже был вызван ранее, иначе ошибка)
        //    FbCommand SelectSQL = new FbCommand(sql_text, fb_conn); //задаем запрос на выборку
        //    SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
        //    FbDataReader reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()
        //    return reader;
        //}

        private XmlNode AddApplications(XmlDocument document, XmlNode parent)
        {
            XmlNode applications = InsertTag(document, parent, "Applications", "");
            FbConnection fb_applications = CreateFbConnection();
            //проверка состояния соединения (активно или не активно)
            if (fb_applications.State == System.Data.ConnectionState.Closed)
                fb_applications.Open();
            FbTransaction fbt = fb_applications.BeginTransaction(); //стартуем транзакцию; стартовать транзакцию можно только для открытой базы (т.е. мутод Open() уже был вызван ранее, иначе ошибка)
            string sql_text = "SELECT af.* FROM ABIT_FAK af LEFT JOIN ABIT a on a.NOM_AB=af.NOM_AB WHERE af.STATUS_Z not in (3,6) AND a.ZABR not in (1,2) AND a.DOK_IN_PK=1 AND af.nom_af < 1000 ORDER BY af.NOM_AF";
            FbCommand SelectSQL = new FbCommand(sql_text, fb_applications); //задаем запрос на выборку
            SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
            FbDataReader applicationsReader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()
            string select_result = ""; //в эту переменную будем складывать результат запроса Select
            //textBoxResult.Clear();

            try
            {
                while (applicationsReader.Read()) //пока не прочли все данные выполняем...
                {
                    // select_result = select_result + reader.GetInt32(0).ToString() + ", " + reader.GetString(1) + "\n";
                    select_result = select_result + applicationsReader.GetString(0) + " " + applicationsReader.GetString(1) + " " + applicationsReader.GetString(2) + "\n";
                    AddApplication(document, applications, applicationsReader.GetInt32(0), applicationsReader.GetInt32(1));
                    //textBoxResult.Text += reader.GetString(0) + " " + reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3) + "\n";
                }
            }
            finally
            {
                //всегда необходимо вызывать метод Close(), когда чтение данных завершено
                applicationsReader.Close();
                fb_applications.Close(); //закрываем соединение, т.к. оно нам больше не нужно
            }

            //MessageBox.Show(select_result); //выводим результат запроса
            SelectSQL.Dispose(); //в документации написано, что ОЧЕНЬ рекомендуется убивать объекты этого типа, если они больше не нужны            
            return applications;
        }

        public void CreatePackageData(string pathToXml)
        {
            //Создаём сам XML-файл
            XmlTextWriter textWritter = new XmlTextWriter(pathToXml, Encoding.UTF8);
            textWritter.WriteStartDocument();//Создаём в файле заголовок XML-документа
            textWritter.WriteStartElement("Root");//Создём корень
            textWritter.WriteEndElement();//Закрываем тег
            textWritter.Close();//Закрываем XmlTextWriter

            //Заносим данные
            XmlDocument document = new XmlDocument();//Для занесения данных мы будем использовать класс XmlDocument
            document.Load(pathToXml);//Загружаем наш файл
            XmlNode PackageData = document.CreateElement("PackageData");//Создаём XML-запись
            document.DocumentElement.AppendChild(PackageData); // указываем родителя           

            AddAuthData(document);//Вносим данные аутентификации
            XmlNode Applications = AddApplications(document, PackageData);//Создаём тег с данными заявлений


            document.Save(pathToXml);//Сохраняем
        }



        public void createXML(string pathToXml)
        {
            //string pathToXml = "C:\\Users\\Александр\\Dropbox\\Программы\\FirebirdTest\\sample.xml";
            //Создаём сам XML-файл
            XmlTextWriter textWritter = new XmlTextWriter(pathToXml, Encoding.UTF8);
            textWritter.WriteStartDocument();//Создаём в файле заголовок XML-документа
            textWritter.WriteStartElement("Root");//Создём корень
            textWritter.WriteEndElement();//Закрываем тег
            textWritter.Close();//Закрываем XmlTextWriter
            //Заносим данные
            XmlDocument document = new XmlDocument();//Для занесения данных мы будем использовать класс XmlDocument
            document.Load(pathToXml);//Загружаем наш файл
            XmlNode element = document.CreateElement("element");//Создаём XML-запись
            document.DocumentElement.AppendChild(element); // указываем родителя
            XmlAttribute attribute = document.CreateAttribute("number"); // создаём атрибут
            attribute.Value = "1"; // устанавливаем значение атрибута
            element.Attributes.Append(attribute); // добавляем атрибут
            //Добавляем в запись данные
            XmlNode subElement1 = document.CreateElement("subElement1"); // даём имя
            subElement1.InnerText = "Hello"; // и значение
            element.AppendChild(subElement1); // и указываем кому принадлежит

            XmlNode subElement2 = document.CreateElement("subElement2");
            subElement2.InnerText = "Dear";
            element.AppendChild(subElement2);

            XmlNode subElement3 = document.CreateElement("subElement3");
            subElement3.InnerText = "Habr";
            element.AppendChild(subElement3);

            AddAuthData(document);

            document.Save(pathToXml);//Сохраняем
        }
    }
}
