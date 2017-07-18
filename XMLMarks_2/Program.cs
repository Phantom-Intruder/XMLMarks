using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace XMLMarks
{
    static class Program
    {
        static void print(string printValue)
        {
            Console.WriteLine(printValue);
        }
        static void Main(string[] args)
        {
            print("How many student records do you want to add");
            //TODO: Add exceptions            
            int numberOfTimes = Convert.ToInt32(Console.ReadLine());

            int i = 0;
            string dataInTag;
            string tag;
            string[] listOfTags = { "FirstName", "LastName", "Mathematics", "Chemistry", "Physics", "English", "GeneralKnowledge" };

            if (File.Exists("student.xml") == false)
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.NewLineOnAttributes = true;
                using (XmlWriter xmlWriter = XmlWriter.Create("student.xml", xmlWriterSettings))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("StudentList");
                    while (i != numberOfTimes)
                    {
                        xmlWriter.WriteStartElement("Student");
                        for (int j = 0; j <= 6; j++)
                        {
                            string TagData = Console.ReadLine();
                            elementWriterMethod(xmlWriter, TagData, listOfTags[j]);

                        }
                        xmlWriter.WriteEndElement();

                        i++;
                    }
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                    xmlWriter.Flush();
                    xmlWriter.Close();
                }
            }
            else
            {
                XDocument xDocument = XDocument.Load("student.xml");
                XElement root = xDocument.Element("StudentList");
                IEnumerable<XElement> rows = root.Descendants("Student");
                XElement firstRow = rows.First();
                while (i != numberOfTimes)
                {
                    string[] tagDataArray = new string[7];
                    for (int j = 0; j <= 6; j++)
                    {
                        tagDataArray[j] = Console.ReadLine();
                    }

                    firstRow.AddBeforeSelf(
               new XElement("Student",
               new XElement(listOfTags[0], tagDataArray[0]),
               new XElement(listOfTags[1], tagDataArray[1]),
               new XElement(listOfTags[2], tagDataArray[2]),
               new XElement(listOfTags[3], tagDataArray[3]),
               new XElement(listOfTags[4], tagDataArray[4]),
               new XElement(listOfTags[5], tagDataArray[5]),
               new XElement(listOfTags[6], tagDataArray[6])
               ));
                    xDocument.Save("student.xml");

                    i++;

                }
             
            }

            writeDataToConsole(listOfTags);

        }
        public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T value)
        {
            foreach (var cur in e)
            {
                yield return cur;
            }
            yield return value;
        }
        private static void writeDataToConsole(string[] headers)
        {
            print("Student Information");
            print("");
            print("---------------------------------------------------------------------------------------------------------------");
            print("      First Name     Last Name   Mathematics      Chemistry      Physics       English      General Knowledge");
            print("---------------------------------------------------------------------------------------------------------------");
            // XDocument doc = XDocument.Load("student.xml");
            XmlReader xmlReader = XmlReader.Create("student.xml");
          
            while (xmlReader.Read())
            {

                if (xmlReader.NodeType.Equals(XmlNodeType.Text))
                {
                    Console.Write(String.Format("|{0,13}",xmlReader.Value));

                }

                if (xmlReader.Name.Equals("Student"))
                {

                    Console.WriteLine();

                }

            }
            Console.ReadKey();
        }


        public static void Print2DArray(string[][] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i][j] + "\t");
                }
                Console.WriteLine();
            }
        }
        private static string[] loadDataToArray(IEnumerable<XElement> firstNameInFile)
        {
            int i = 0;

            string[] firstNameArray = new string[10];

            foreach (var fName in firstNameInFile)
            {
                firstNameArray[i] = (fName.Value);
                i++;
            }
            return firstNameArray;
        }

        private static void elementWriterMethod(XmlWriter xmlWriter, string dataInTag, string tag)
        {
            xmlWriter.WriteElementString(tag, dataInTag);

        }
    }
}
