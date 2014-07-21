using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FirebirdTest
{
    class xmlPackageGenerator
    {

        private XmlNode InsertTag(XmlDocument document, XmlNode parent, string nodeName, string node_value)
        {
            if (node_value.Length == 0)
            {
                XmlNode Node = document.CreateElement(nodeName);// даём имя
                document.DocumentElement.AppendChild(Node);// и указываем кому принадлежит
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
            //XmlNode AuthData = InsertTag(document, document.DocumentElement, "AuthData", "");
            XmlNode AuthData = document.CreateElement("AuthData"); // даём имя
            //subElement1.InnerText = "Hello"; // и значение
            document.DocumentElement.AppendChild(AuthData); // и указываем кому принадлежит

            InsertTag(document, AuthData, "Login", "at@vsma.ac.ru");
            //XmlNode Login = document.CreateElement("Login"); // даём имя
            //Login.InnerText = "at@vsma.ac.ru"; // и значение
            //AuthData.AppendChild(Login); // и указываем кому принадлежит

            InsertTag(document, AuthData, "Pass", "BRalQhv");
            //XmlNode Pass = document.CreateElement("Pass"); // даём имя
            //Pass.InnerText = "BRalQhv"; // и значение
            //AuthData.AppendChild(Pass); // и указываем кому принадлежит         
        }

        private void AddApplication(XmlDocument document, XmlNode parent)
        {
            XmlNode application = InsertTag(document, parent, "application", "");
            InsertTag(document, application, "UID", "23");
            InsertTag(document, application, "ApplicationNumber", "24");
            InsertTag(document, application, "NeedHostel", "0");
            InsertTag(document, application, "StatusID", "4");
        }

        private XmlNode AddApplications(XmlDocument document, XmlNode parent)
        {
            XmlNode applications = InsertTag(document, parent, "Applications", "");
            AddApplication(document, applications);
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
