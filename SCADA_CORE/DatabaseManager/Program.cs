using DatabaseManager.DatabaseServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DatabaseManager
{
    class Program
    {
        static DatabaseServiceReference.DatabaseManagerServiceClient dbClient = new DatabaseServiceReference.DatabaseManagerServiceClient();
      
        static void Main(string[] args)
        {

            unAuthMenu();
        }
        static void unAuthMenu()
        {
            Console.WriteLine("=============================");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1.Login");
            Console.WriteLine("2.Register");
            unAuthMenuResponse();
        }


        static void mainMenu()
        {
            Console.WriteLine("=============================");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Add Tag");
            Console.WriteLine("2. Remove Tag");
            Console.WriteLine("3. Switch scan mode");
            Console.WriteLine("4. Get output values");
            Console.WriteLine("5. Change output value");
            Console.WriteLine("6. Add an alarm");
            Console.WriteLine("7. Remove an alarm");
            Console.WriteLine("q. Log out");
            MainMenuResponse();
        }

        static void unAuthMenuResponse()
        {
            Console.WriteLine("=============================");
            Console.WriteLine("Enter your choice:");
            string response = Console.ReadLine();
            if (response == "1")
            {
                Console.WriteLine("Input your username:");
                string username = Console.ReadLine();
                Console.WriteLine("Input your password:");
                string password = Console.ReadLine();
                bool sucss1 = dbClient.Login(username, password);
                if (sucss1)
                {
                    Console.WriteLine("Successfully logged in!");
                    dbClient.LoadXml();
                    mainMenu();
                }
                else
                {
                    Console.WriteLine("Wrong Username or password!");
                    unAuthMenu();
                }
            }
            else if (response == "2")
            {
                Console.WriteLine("=============================");
                Console.WriteLine("Input your username:");
                string username = Console.ReadLine();
                Console.WriteLine("Input your password:");
                string password = Console.ReadLine();
                bool sucs = dbClient.Register(username, password);
                if (sucs)
                {
                    Console.WriteLine("Successfully registered!");
                    unAuthMenu();
                }
                else
                {
                    Console.WriteLine("Successfully registered!");
                    unAuthMenu();
                }
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
            unAuthMenu();
        }
        static void MainMenuResponse()
        {
            Console.WriteLine("=============================");
            Console.WriteLine("Enter your choice:");
            string response = Console.ReadLine();

            if (response == "1")
            {
                AddTagMenu();
            }
            else if (response == "2")
            {
                RemoveTagMenu();
            }
            else if (response == "3")
            {
                 SwitchScanMenu();
            }
            else if (response == "4")
            {
                GetOutputValues();
            }
            else if (response == "5")
            {
                ChangeOutputValueMenu();
            }
            else if (response == "6")
            {
                AddAlarm();
            }
            else if (response == "7")
            {
                RemoveAlarm();
            }
            else if (response == "q")
            {
                LogOut();
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
            mainMenu();
        }
        private static void AddAlarm()
        {
            Console.WriteLine("=============================");
            Console.WriteLine("Choose an ANALOG TAG to add an alarm to: ");
            string tagId = Console.ReadLine();
            Console.WriteLine("What priority is the alarm (1, 2 or 3) : ");
            int priority = int.Parse(Console.ReadLine());
            Console.WriteLine("Type  (high/low) : ");
            string type = Console.ReadLine();

            Console.WriteLine(dbClient.AddAlarmForAnalog(tagId, type, priority));
            mainMenu();
        }

        private static void RemoveAlarm()
        {
            Console.WriteLine("=============================");
            Console.WriteLine("Choose a TAG to remove alarms from: ");
            string tagId = Console.ReadLine();
            Console.WriteLine(dbClient.RemoveAlarm(tagId));
            mainMenu();
        }

       

        private static void LogOut()
        {
            Console.WriteLine("Successfully logged out !");
            Console.WriteLine("=============================");
            unAuthMenu();
        }

        private static void GetOutputValues()
        {
            Console.WriteLine("=============================");
            Dictionary<string, double> OutputValues = dbClient.GetOutputValues();
            Console.WriteLine("All output values are :");
            foreach (KeyValuePair<string, double> TagInfo in OutputValues)
            {
                
                Console.WriteLine($"{TagInfo.Key} has the value of: {TagInfo.Value}");
                Console.WriteLine("-----------------------------------------------");
            }
            Console.WriteLine("=============================");
            mainMenu();
        }

        static void RemoveTagMenu()
        {
            Console.WriteLine("Enter tag id to remove:");

            RemoveTagMenuResponse();
        }
        static void RemoveTagMenuResponse()
        {
            var response = Console.ReadLine();

            Console.WriteLine(dbClient.RemoveTag(response));
            mainMenu();
        }

        static void SwitchScanMenu()
        {
            Console.WriteLine("Enter tag id to switch:");

            SwitchScanMenuResponse();
        }
        static void SwitchScanMenuResponse()
        {
            var response = Console.ReadLine();

            Console.WriteLine(dbClient.SwitchScanMode(response));
            mainMenu();
        }

        static void ChangeOutputValueMenu()
        {
            Console.WriteLine("Enter tag id to switch:");

            ChangeOutputValueResponse();
        }
        static void ChangeOutputValueResponse()
        {
            var response = Console.ReadLine();

            Console.WriteLine("Enter new value:");
            double newValue = Double.Parse(Console.ReadLine());
            Console.WriteLine(dbClient.ChangeOutputValue(response, newValue));
            mainMenu();
        }

        static void AddTagMenu()
        {
            string response = null;

            do
            {
                Console.WriteLine("Choose an option");
                Console.WriteLine("1.Add Analog OUTPUT  1");
                Console.WriteLine("2.Add Analog OUTPUT  2");
                Console.WriteLine("3.Add Digital OUTPUT  1");
                Console.WriteLine("4.Add Digital OUTPUT  2");
                Console.WriteLine("5.Add Analog INPUT Tag S ");
                Console.WriteLine("6.Add Analog INPUT Tag C ");
                Console.WriteLine("7.Add Analog INPUT Tag R ");
                Console.WriteLine("8.Add Digital INPUT Tag S ");
                Console.WriteLine("9.Add Digital INPUT Tag C ");
                Console.WriteLine("10.Add Digital INPUT Tag R ");
                Console.WriteLine("q.Return");

                response = AddTagMenuResponse(); ;
            }
            while (response != "q");
            mainMenu();
        }

        static string AddTagMenuResponse()
        {
            Console.WriteLine("Enter your choice:");
            var response = Console.ReadLine();

            if (response == "1")
            {
                Console.WriteLine(dbClient.AddAnalogOutputTag("aoTag1", "description", "asd", 2500,  -1000, 1000));
            }
            else if (response == "2")
            {
                Console.WriteLine(dbClient.AddAnalogOutputTag("aoTag2", "description", "asd", 2000, -1000, 1000));
            }
            else if (response == "3")
            {
                Console.WriteLine(dbClient.AddDigitalOutputTag("doTag3", "description", "xzc", 1000));
            }
            else if (response == "4")
            {
                Console.WriteLine(dbClient.AddDigitalOutputTag("doTag4", "description", "wer", 3000));
            }
            else if (response == "5")
            {
                Console.WriteLine(dbClient.AddAnalogInputTag("aiTag5", "description", "Sim driver", "R", 1000, true, -100, 100, "units"));
            }
            else if (response == "6")
            {
                Console.WriteLine(dbClient.AddAnalogInputTag("aiTag6", "description", "Sim driver", "R", 1000, true, -100, 100, "bigunits"));
            }
            else if (response == "7")
            {
                Console.WriteLine(dbClient.AddAnalogInputTag("aiTag7", "description", "Sim driver", "R", 1000, true, -100, 100, "biggestunits"));
            }
            else if (response == "8")
            {
                Console.WriteLine(dbClient.AddDigitalInputTag("diTag8", "description", "Sim driver", "R", 1000, true));
            }
            else if (response == "9")
            {
                Console.WriteLine(dbClient.AddDigitalInputTag("diTag9", "description", "Sim driver", "R", 1000, true));
            }
            else if (response == "10")
            {
                Console.WriteLine(dbClient.AddDigitalInputTag("diTag10", "description", "Sim driver", "R", 1000, true));
            }
            else if (response == "q")
            {
                Console.WriteLine("Returning...");
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }

            return response;
        }


    }
}
