using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Cw2
{
    class Program
    {
        static void Main(string[] args)
        {
            string path;
            if (args[0] == null)
            {
                path = "data.csv";
            }
            else
            {
                path = args[0];
            }

            string resultPath;
            if (args[1] == null)
            {
                resultPath = "result.xml";
            }
            else
            {
                resultPath = args[1];
            }

            //tworzymy plik do logów, lub go nadpisujemy, gdy już istnieje
            string logFile = "log.txt";
            DateTime dateTime = DateTime.Now;
            StreamWriter streamWriter = File.CreateText(logFile);
            using (streamWriter)
            {
                streamWriter.WriteLine("Cw2 Log file. Created: " + dateTime + "\n");
            }

            //listy
            List<string> IdList = new List<string>();
            List<Student> students = new List<Student>();
            Dictionary<string, int> studies = new Dictionary<string, int>();

            try
            {
                using (StreamReader streamReader = File.OpenText(path))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        bool next = true;
                        for (int i = 0; i < line.Length - 1; i++)
                        {
                            if (line[i].Equals(line[i + 1]) && line[i].Equals(','))
                            {
                                using (streamWriter = new StreamWriter(logFile, true))
                                {
                                    streamWriter.WriteLine("Niekompletne dane:\t" + line);
                                    next = false;
                                }
                            }
                        }

                        if (next)
                        {
                            bool next2 = true;
                            string[] tmp = line.Split(",");
                            for (int i = 0; i < IdList.Count; i++)
                            {
                                if (IdList[i].Equals(tmp[4]))
                                {
                                    using (streamWriter = new StreamWriter(logFile, true))
                                    {
                                        streamWriter.WriteLine("Duplikacja danych:\t" + line);
                                        next2 = false;
                                    }
                                }
                            }

                            if (next2)
                            {
                                Student student = new Student(tmp[0], tmp[1], tmp[2], tmp[3], tmp[4], tmp[5], tmp[6],
                                    tmp[7], tmp[8]);
                                IdList.Add(student.getId());
                                students.Add(student);
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                using (streamWriter = new StreamWriter(logFile, true))
                {
                    streamWriter.WriteLine("FileNotFoundException: Plik " + path + " nie istnieje");
                }

                Console.WriteLine("Plik " + path + " nie istnieje\n" + e);
                //throw;
            }
            catch (ArgumentException e)
            {
                using (streamWriter = new StreamWriter(logFile, true))
                {
                    streamWriter.WriteLine("ArgumentException: Podana ścieżka jest niepoprawna\n");
                }

                Console.WriteLine("ArgumentException: Podana ścieżka jest niepoprawna\n" + e);
                //throw;
            }
            catch (DirectoryNotFoundException e)
            {
                using (streamWriter = new StreamWriter(logFile, true))
                {
                    streamWriter.WriteLine("DirectoryNotFoundException: Podana ścieżka jest niepoprawna\n");
                }

                Console.WriteLine("DirectoryNotFoundException: Podana ścieżka jest niepoprawna\n" + e);
                //throw;
            }
            catch (Exception ex)
            {
                using (streamWriter = new StreamWriter(logFile, true))
                {
                    streamWriter.WriteLine(ex);
                }

                throw;
            }

            foreach (Student student in students)
            {
                if (!studies.ContainsKey(student.getKierunek())) studies.Add(student.getKierunek(), 1);
                else studies[student.getKierunek()]++;
            }


            var date = DateTime.Now.ToString("dd.MM.yyyy");
            

            //dodawanie studentów do pliku wynikowego
            XDocument xDocument = new XDocument(new XElement("uczelnia", new XAttribute("createdAt", date),
                new XAttribute("author", "Oskar Marszałek"), new XElement("studenci",
                    from student1 in students
                    select new XElement("student", new XAttribute("indexNumber", "s" + student1.getId()),
                        new XElement("fname", student1.getImie()), new XElement("lname", student1.getNazwisko()),
                        new XElement("birthdate", student1.getData()), new XElement("email", student1.getMail()),
                        new XElement("mothersName", student1.getImieRodzica1()),
                        new XElement("fathersName", student1.getImieRodzica2()),
                        new XElement("studies", new XElement("name", student1.getKierunek()),
                            new XElement("mode", student1.getTryb())))), new XElement("activeStudies",
                    from stud in studies
                    select new XElement("studies", new XAttribute("name", stud.Key),
                        new XAttribute("numberOfStudents", stud.Value)))));
            xDocument.Save(resultPath);
        }
    }
}