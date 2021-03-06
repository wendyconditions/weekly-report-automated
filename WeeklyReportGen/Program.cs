﻿using System;
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
        
        // reading from a txt file located on user desktop
        // change file variable to whatever file you want the program to read
        private static void ReadFile()
        {
            string message;
            try
            {
                string file = "testfile.txt";
                string fullpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + file;
                using (StreamReader sr = new StreamReader(fullpath))
                {
                    string line = sr.ReadToEnd();
                    if (!line.Any())
                    {
                        message = $"The file was empty. Date: {DateTime.Now}";
                        WriteToFile(message);
                        return;
                    }
                    message = $"Email app started at: {DateTime.Now}";
                    WriteToFile(message);
                    SendEmail(line);
                }
            }
            catch (Exception e)
            {
                message = $"Exception: {e.Message}";
                WriteToFile(message);
            }
        }
        
        // Creating and/or adding application messages to a new text file in Logs folder on user desktop
        // Will write errors or success messages to file to monitor application behaviors if necessary
        private static void WriteToFile(string message)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Logs\\WeeklyReport_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            var fileAction = File.Exists(filepath) ? File.AppendText(filepath) : File.CreateText(filepath);

            using (StreamWriter sw = fileAction)
            {
                sw.WriteLine(message);
            }
        }
        
        // Configure emails to fromMA and toMA
        // Configure smtpClient Host
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
