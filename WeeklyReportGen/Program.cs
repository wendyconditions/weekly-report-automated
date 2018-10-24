using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ReadFileTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadFile();
        }

        private static void ReadFile()
        {
            try
            {
                string file = "testfile.txt";
                string fullpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + file;
                using (StreamReader sr = new StreamReader(fullpath))
                {
                    string line = sr.ReadToEnd();
                    if (line.Any())
                    {
                        string message = $"Email app started at: {DateTime.Now}";
                        WriteToFile(message);
                        SendEmail(line);
                    }
                    else
                    {
                        string message = $"The file was empty. Date: {DateTime.Now}";
                        WriteToFile(message);
                    }
                }
            }
            catch (Exception e)
            {
                string message = $"Exception: {e.Message}";
                WriteToFile(message);
            }
        }

        private static void WriteToFile(string message)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Logs\\WeeklyReport_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                using(StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(message);
                }
            }
        }

        private static void SendEmail(string text)
        {
            try
            {
                MailMessage message = new MailMessage();
                MailAddress fromMA = new MailAddress("email");
                MailAddress toMA = new MailAddress("email");

                message.From = fromMA;
                message.To.Add(toMA);
                message.Subject = "Weekly Report";
                message.Body = text;
                message.IsBodyHtml = false; // catching line breaks in text file

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = "host";

                smtpClient.Send(message);
                string successMessage = $"Email successfully sent at: {DateTime.Now}";
                WriteToFile(successMessage);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
