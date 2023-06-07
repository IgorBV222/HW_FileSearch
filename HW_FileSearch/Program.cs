using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HW_FileSearch
{
    internal class Program
    {
        private static string[] StrArr(string _path)
        {
            string[] _arr = new string[] { };
            try
            {
                StreamReader streamReader = new StreamReader(_path, true);
                _arr = streamReader.ReadToEnd().Split(' ');                
                streamReader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException: " + e.Message);
            }
            return _arr;
        }        
        static void SearchFile(string pathToCodes, int numberOfDaysToSearch)
        {
            string[] userID = StrArr(pathToCodes);
            string myDoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string[] allFoundFilesMyDoc = Directory.GetFiles(myDoc, "*.txt", SearchOption.AllDirectories);
            string myDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);            
            string[] allFoundFilesmyDesktop = Directory.GetFiles(myDesktop, "*.txt", SearchOption.AllDirectories);
            var allFoundFiles = allFoundFilesMyDoc.Union(allFoundFilesmyDesktop);
            foreach (string filename in allFoundFiles)
            {
                StreamReader streamReader = new StreamReader(filename);
                FileInfo fileInfo = new FileInfo(filename);
                DateTime timeOfModify = fileInfo.LastWriteTime;
                DateTime stopDate = DateTime.Now;
                DateTime startDate = stopDate.AddDays(numberOfDaysToSearch * (-1));
                if (startDate <= timeOfModify & timeOfModify < stopDate)
                {
                    try
                    {                       
                        string[] dataInFile = StrArr(filename);
                        var coincidence = userID.Intersect(dataInFile);

                        foreach (var item in coincidence)
                        {
                            Console.WriteLine(($"{item}" +
                                    $" содержится в файле:\n{filename}, который был изменён {timeOfModify.ToString("yyyy.MM.dd HH:mm:ss")}\n"));
                        }
                    }
                    catch 
                    {
                        Console.WriteLine("\nException: файлы, удовлетворяющие условию поиска не обнаружены!");
                    }
                }
                streamReader.Close();
            }
        }
        static void Main(string[] args)
        {
            string pathToCodes;
            try
            {
                if (args.Length > 0)
                {
                    pathToCodes = args[0];                    
                }
                else
                {
                    Console.WriteLine("Введите путь к файлу с идентификаторами:");
                    pathToCodes = Console.ReadLine();                    
                }                
                Console.WriteLine("Введите количество дней для поиска:");
                int numberOfDaysToSearch = Convert.ToInt32(Console.ReadLine());
                SearchFile(pathToCodes, numberOfDaysToSearch);
            }
            catch
            {
                Console.WriteLine("\nException: неверный путь к файлу!");
            }
        }
    }
}
