using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using XMLMarks_2;

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
            int numberOfStudents = 0;
            numberOfStudents = getNumberOfStudents(numberOfStudents);

            int i = 0;

            string[] listOfTags = { "FirstName", "LastName", "Mathematics", "Chemistry", "Physics", "English", "GeneralKnowledge" };

            if (File.Exists("student.xml") == false)
            {
                XmlWriterSettings xmlWriterSettings = readyXmlFileForWritingAndReading();
                using (XmlWriter xmlWriter = XmlWriter.Create("student.xml", xmlWriterSettings))
                {
                    i = openFileAndListTheTagsAndData(numberOfStudents, i, listOfTags, xmlWriter);
                }
            }
            else
            {
                XDocument xDocument;
                XElement firstRow;
                readyFileForAppending(out xDocument, out firstRow);
                while (i != numberOfStudents)
                {
                    appendDataAndSaveDocument(listOfTags, xDocument, firstRow);

                    i++;

                }

            }

            writeDataToConsole(listOfTags);

        }

        private static int getNumberOfStudents(int numberOfStudents)
        {
            try
            {
                numberOfStudents = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException e)
            {
                print("Please enter a number");
                getNumberOfStudents(numberOfStudents);
            }

            return numberOfStudents;
        }

        private static void appendDataAndSaveDocument(string[] listOfTags, XDocument xDocument, XElement firstRow)
        {
            string[] tagDataArray = new string[7];
            for (int j = 0; j <= 6; j++)
            {
                getInputFromUser(tagDataArray, j, listOfTags);
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
        }

        private static void getInputFromUser(string[] tagDataArray, int j, string[] listOfTags)
        {
            try {
                print("Enter "+ listOfTags[j]);
                if (j <= 1)
                {
                    tagDataArray[j] = Console.ReadLine();
                }
                else
                {
                    int data = Convert.ToInt32(Console.ReadLine());
                    if ((data < 0)||(data > 100))
                    {
                        throw new MarksOutOfRangeException();
                    }
                    tagDataArray[j] = data.ToString();
                }
            }
            catch (FormatException e)
            {
                print("Please enter only numbers for marks");
                j = 0;
                getInputFromUser(tagDataArray, j, listOfTags);
            }catch (MarksOutOfRangeException m)
            {
                print("Please enter marks only within 0 - 100");
                getInputFromUser(tagDataArray, j, listOfTags);
            }

        }

        private static void readyFileForAppending(out XDocument xDocument, out XElement firstRow)
        {
            xDocument = XDocument.Load("student.xml");
            XElement root = xDocument.Element("StudentList");
            IEnumerable<XElement> rows = root.Descendants("Student");
            firstRow = rows.First();
        }

        private static XmlWriterSettings readyXmlFileForWritingAndReading()
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.NewLineOnAttributes = true;
            return xmlWriterSettings;
        }

        private static int openFileAndListTheTagsAndData(int numberOfTimes, int i, string[] listOfTags, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("StudentList");
            while (i != numberOfTimes)
            {
                matchTagsWithTagData(listOfTags, xmlWriter);

                i++;
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            xmlWriter.Close();
            return i;
        }

        private static void matchTagsWithTagData(string[] listOfTags, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("Student");
            for (int j = 0; j <= 6; j++)
            {
                string TagData = Console.ReadLine();
                elementWriterMethod(xmlWriter, TagData, listOfTags[j]);

            }
            xmlWriter.WriteEndElement();
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
                    Console.Write(String.Format("|{0,13}", xmlReader.Value));

                }

                if (xmlReader.Name.Equals("Student"))
                {

                    Console.WriteLine();

                }

            }
            Console.ReadKey();
        }

        private static void elementWriterMethod(XmlWriter xmlWriter, string dataInTag, string tag)
        {
            xmlWriter.WriteElementString(tag, dataInTag);

        }
    }
}
