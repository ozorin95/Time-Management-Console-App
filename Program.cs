using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace TimeManagementApp
{
    class Program
    {
        static string GetProjectDirectory()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            if (basePath.Contains("bin") && basePath.Contains("Debug"))
            {
                return Path.GetFullPath(Path.Combine(basePath, @"..\..\..\"));
            }
            return basePath;
        }

        static string activitiesFile = Path.Combine(GetProjectDirectory(), "activities.csv");
        /// <summary>
        /// Hlavní vstupní bod aplikace. Obsahuje úvodní animaci, hlavní smyčku menu a ukončovací sekvenci.
        /// </summary>
        static void Main(string[] args)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            string logo = @"
  _______ _                 __  __                                   
 |__   __(_)               |  \/  |                                  
    | |   _ _ __ ___   ___ | \  / | __ _ _ __   __ _  __ _  ___ _ __ 
    | |  | | '_ ` _ \ / _ \| |\/| |/ _` | '_ \ / _` |/ _` |/ _ \ '__|
    | |  | | | | | | |  __/| |  | | (_| | | | | (_| | (_| |  __/ |   
    |_|  |_|_| |_| |_|\___||_|  |_|\__,_|_| |_|\__,_|\__, |\___|_|   
                                                      __/ |          
                                                     |___/           
";
            Console.WriteLine(logo);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("                  Loading system components...\n");


            Console.ForegroundColor = ConsoleColor.Green;

            ///Přidávání progress bar
            int totalTime = 2500;
            int steps = 25;
            int sleepTime = totalTime / steps; //přidame obdélník každých 200 ms

            for (int i = 0; i <= steps; i++)
            {
                int percent = (i * 100) / steps;


                string bar = new string('█', i) + new string('-', steps - i); // kreslíme progress bar


                Console.Write($"\r                  [{bar}] {percent}%");

                System.Threading.Thread.Sleep(sleepTime);
            }


            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("                  System Ready! Press ENTER to begin.");
            Console.ResetColor();

            Console.ReadLine();

            bool isRunning = true;

            // Hlavní nekonečná smyčka aplikace
            while (isRunning)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=== PERSONAL TIME ASSISTANT ===");
                Console.ResetColor();

                Console.WriteLine("1. Add activity (study, entertainment, etc.)");
                Console.WriteLine("2. Day analysis and advice");
                Console.WriteLine("3. Reset all data");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option (1-4): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddActivity();
                        break;
                    case "2":
                        ShowAnalysis();
                        break;
                    case "3":
                        ResetData();
                        break;
                    case "4":
                        isRunning = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error! Please select 1-4. Press Enter to continue...");
                        Console.ResetColor();
                        Console.ReadLine();
                        break;
                }
            }

            Console.Clear();
            Print("=== SYSTEM SHUTDOWN ===", ConsoleColor.Cyan);
            Print("Saving user data...\n", ConsoleColor.DarkGray);

            int exitSteps = 10;
            int exitSleep = 800 / exitSteps;

            for (int i = 0; i <= exitSteps; i++)
            {
                string bar = new string('█', i) + new string('-', exitSteps - i);
                Console.Write($"\rShutting down: [{bar}] {i * 10}%");
                Thread.Sleep(exitSleep);
            }

            Console.WriteLine("\n");
            Print("All data saved safely. Goodbye!", ConsoleColor.Green);
            Thread.Sleep(500);
        }

        /// <summary>
        /// Pomocná metoda pro zjednodušení výpisu barevného textu s odřádkováním.
        /// </summary>
        static void Print(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        /// Pomocná metoda pro výpis barevného textu na stejný řádek (pro zadávání vstupu).
        /// </summary>
        static void PrintInline(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }


        /// <summary>
        /// Přidání vlastní aktivity (Název -> Kategorie -> Hodiny).
        /// </summary>
        static void AddActivity()
        {
            while (true)
            {
                Console.Clear();
                Print("=== ADD ACTIVITY ===", ConsoleColor.Cyan);
                Print("(Press ENTER on an empty line to return to menu)\n", ConsoleColor.DarkGray);

                // 1. Název aktivity
                PrintInline("Enter activity name (e.g., Essay, Running): ", ConsoleColor.White);
                string activityName = Console.ReadLine();
                if (string.IsNullOrEmpty(activityName)) return; // Krok zpět

                // 2. Vlastní kategorie
                PrintInline("Which category does it belong to? (e.g., School, Sport): ", ConsoleColor.Yellow);
                string category = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(category)) category = "Misc"; // Výchozí hodnota

                // 3. Počet hodin
                PrintInline("How many hours did you spend on it? ", ConsoleColor.White);
                string inputHours = Console.ReadLine();
                if (string.IsNullOrEmpty(inputHours)) return;

                double hours;
                while (!double.TryParse(inputHours, out hours) || hours <= 0 || hours > 24)
                {
                    Print("Error! Please enter a valid number (0.1 - 24).", ConsoleColor.Red);
                    PrintInline("Hours: ", ConsoleColor.Yellow);
                    inputHours = Console.ReadLine();
                    if (string.IsNullOrEmpty(inputHours)) return;
                }

               
                    string record = $"{category};{activityName};{hours}\n";
                    File.AppendAllText(activitiesFile, record);

                    Print("\nSaved! Press Enter to add another...", ConsoleColor.Green);
                    Console.ReadLine();
                
            }
        }

        static void ShowAnalysis()
        {
            Console.Clear();
            Print("=== DAILY ANALYSIS ===", ConsoleColor.Cyan);

            if (!File.Exists(activitiesFile))
            {
                Print("No data found yet. Please add an activity first!", ConsoleColor.Yellow);
                Console.ReadLine();
                return;
            }

            string[] records = File.ReadAllLines(activitiesFile);

            // Slovníky pro dynamické shromažďování dat
            Dictionary<string, double> totals = new Dictionary<string, double>();
            Dictionary<string, List<string>> details = new Dictionary<string, List<string>>();
            double totalHours = 0;

            // Načítání a transformace dat
            foreach (string line in records)
            {
                string[] parts = line.Split(';');
                if (parts.Length < 3) continue; // Ochrana proti starým/poškozeným datům

                string cat = parts[0].Trim();
                string name = parts[1].Trim();

                if (double.TryParse(parts[2], out double hrs))
                {
                    totalHours += hrs;

                    if (!totals.ContainsKey(cat))
                    {
                        totals[cat] = 0;
                        details[cat] = new List<string>();
                    }

                    totals[cat] += hrs;
                    details[cat].Add($"  - {name}: {hrs} h");
                }
            }

            // SANITY CHECK (Logická kontrola 24h)
            if (totalHours > 24)
            {
                Print("\n=== CRITICAL DATA ERROR ===", ConsoleColor.Red);
                Print($"Total logged hours: {totalHours} h", ConsoleColor.White);
                Print("A day only has 24 hours! Reset old data.", ConsoleColor.Yellow);

                PrintInline("\nDo you want to RESET data IMMEDIATELY? (Y/N): ", ConsoleColor.Cyan);
                if (Console.ReadLine().ToUpper() == "Y")
                {
                    File.Delete(activitiesFile);
                    Print("Data deleted! You can start with a clean slate.", ConsoleColor.Green);
                }
                Console.ReadLine();
                return; // Ukončíme analýzu
            }

            // DYNAMICKÝ VÝPIS KATEGORIÍ 
            Console.WriteLine("-----------------------------");
            foreach (var cat in totals.Keys)
            {
                Print($"[ {cat.ToUpper()} ] (Total: {totals[cat]} h)", ConsoleColor.Cyan);
                foreach (var detail in details[cat])
                {
                    Console.WriteLine(detail);
                }
                Console.WriteLine();
            }

            // Vizuální progress bar
            int filledBoxes = (int)Math.Round(totalHours);
            int emptyBoxes = 24 - filledBoxes;
            string progressBar = new string('█', filledBoxes) + new string('-', emptyBoxes);
            Print($"[{progressBar}] Used {totalHours}/24 hours", ConsoleColor.DarkGray);
            Console.WriteLine("-----------------------------");

            // Smart Advice
            Print("\n=== HEALTH AND PRODUCTIVITY ADVICE ===", ConsoleColor.Yellow);

            double studyHours = 0, sleepHours = 0, screenHours = 0, outdoorHours = 0;

            // Lingvistická analýza zadaných kategorií (podpora pro CZ i EN slova)
            foreach (var cat in totals.Keys)
            {
                string lowerCat = cat.ToLower();
                if (lowerCat.Contains("stud") || lowerCat.Contains("school") || lowerCat.Contains("work") || lowerCat.Contains("učen"))
                    studyHours += totals[cat];

                if (lowerCat.Contains("sleep") || lowerCat.Contains("spán") || lowerCat.Contains("span"))
                    sleepHours += totals[cat];

                if (lowerCat.Contains("screen") || lowerCat.Contains("game") || lowerCat.Contains("film") || lowerCat.Contains("pc"))
                    screenHours += totals[cat];

                if (lowerCat.Contains("out") || lowerCat.Contains("sport") || lowerCat.Contains("ven"))
                    outdoorHours += totals[cat];
            }

            // Vyhodnocení rad
            if (studyHours < 3)
                Print("• Study: You spent less than 3 hours on study/work today. Try to improve this tomorrow.", ConsoleColor.Yellow);

            if (sleepHours < 7)
                Print("• Sleep: You have less than 7 hours of sleep! This can negatively affect your memory and focus.", ConsoleColor.Red);
            else if (sleepHours >= 7)
                Print("• Sleep: Excellent! You have a healthy sleep schedule.", ConsoleColor.Green);

            if (screenHours > 6)
                Print("• Eyes: You spend a lot of time in front of a screen.", ConsoleColor.Magenta);

            if (outdoorHours < 1)
                Print("• Health: You haven't been outside today. Go for at least a 30-minute walk tomorrow.", ConsoleColor.Yellow);

            if (studyHours == 0 && sleepHours == 0 && screenHours == 0 && outdoorHours == 0)
                Print("• Tip: Use words like 'School', 'Sleep', or 'Sport' in your categories to get smart advice.", ConsoleColor.DarkGray);

            Print("\nPress Enter to return to menu...", ConsoleColor.DarkGray);
            Console.ReadLine();
        }

        /// <summary>
        /// Smaže soubor s historií aktivit.
        /// </summary>
        static void ResetData()
        {
            Console.Clear();
            Print("=== RESET DATA ===", ConsoleColor.Red);
            PrintInline("Are you sure you want to delete ALL history? (Y/N): ", ConsoleColor.White);

            if (Console.ReadLine().ToUpper() == "Y")
            {
                if (File.Exists(activitiesFile))
                {
                    File.Delete(activitiesFile);
                    Print("History was successfully deleted.", ConsoleColor.Green);
                }
                else
                {
                    Print("No data to delete.", ConsoleColor.Yellow);
                }
            }
            Console.ReadLine();
        }
    }
}
