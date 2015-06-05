using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace SQL_Server_Killer_v1
{
    class GenericFunctions
    {

        public static bool checkNode(string ip)
        {
            if (validateIP(ip))
            {
                //sql server uses tcp as connection protocol and IANA registered port for sql server is 1433
                TcpClient client = new TcpClient();
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), 1433);

                try
                {
                    client.Connect(ep);
                    client.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    GenericFunctions.logError("SQL Server Service not running publically");
                    return false;
                }
            }
            else
            {
                GenericFunctions.logError("Bad Ip Input");
                return false;
            }
        }

        public static bool validateIP(string ip)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ip);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static List<string> getPasswordFromFile()
        {
            List<string> passwords = null;
            try
            {
                using (StreamReader sr = new StreamReader("pass.txt"))
                {
                    string line = null;
                    passwords = new List<string>();
                    
                    while ((line = sr.ReadLine()) != null)
                        passwords.Add(line);

                    return passwords;
                }
            }
            catch (Exception ex)
            {
                logError(ex.Message);
                return null;
            }
        }


        public static void logError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[Error] : " + message);
        }

        public static void logNotification(string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n[INFO] : " + message);
        }

        public static void logSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[Success] : " + message);
        }


        public static void dbResponse(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n[DB Responce] : " + message);
        }


        public static void logDataTable(DataTable dataTable)
        {
            DataRowCollection dataRowCollection = dataTable.Rows;
            foreach (DataRow dataRow in dataRowCollection)
            {
                object[] rowColumns = dataRow.ItemArray;
                foreach (var column in rowColumns)
                {
                    if (!String.IsNullOrEmpty(column.ToString()))
                        dbResponse(column.ToString());
                }
            }
        }


    }
}
