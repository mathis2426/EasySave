using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Reflection;
using ControllerModel.Jobs;
using ControllerModel.LanguagesHelper;

namespace program
{
    public class Program
    {
        private readonly static LanguageManager languageManager = new LanguageManager();
        private readonly static JobManager jobManager = new JobManager();
        static void Main(string[] args)
        {
            // If specified args
            if (args.Length == 1)
            {
                if (args[0] == "AllJob")
                {
                    jobManager.LaunchBackup(0);
                    Console.WriteLine(languageManager.ResManager.GetString("All_job_executed"));
                }
                else
                {
                    jobManager.LaunchBackupCommandLine(args[0]);
                    Console.WriteLine(languageManager.ResManager.GetString("job_executed"));
                }

                
                Environment.Exit(0);
            }

            // Application Tagline
            string[] logoLines = new string[]
         {
            @"  ______                 _____                 ",
            @"|  ____|               / ____|                ",
            @"| |__   __ _ ___ _   _| (___   __ ___   _____ ",
            @"|  __| / _` / __| | | |\___ \ / _` \ \ / / _ \",
            @"| |___| (_| \__ \ |_| |____) | (_| |\ V /  __/",
            @"|______\__,_|___/\__, |_____/ \__,_| \_/ \___|",
            @"                  __/ |                       ",
            @"                 |___/                        "
         };

            ConsoleColor[] colors = new ConsoleColor[]
            {
            ConsoleColor.Red,
            ConsoleColor.Yellow,
            ConsoleColor.Green,
            ConsoleColor.Cyan,
            ConsoleColor.Blue,
            ConsoleColor.Magenta,
            };

            int cycles = 8;
            int delay = 80;

            for (int step = 0; step < cycles; step++)
            {
                Console.Clear();

                for (int i = 0; i < logoLines.Length; i++)
                {
                    int colorIndex = (step + i) % colors.Length;
                    Console.ForegroundColor = colors[colorIndex];
                    Console.WriteLine(logoLines[i]);
                }

                Thread.Sleep(delay);
            }

            Console.ResetColor();

            // Application Description
            Console.WriteLine(languageManager.ResManager.GetString("welcome"));
            Console.WriteLine(languageManager.ResManager.GetString("description1"));
            Console.WriteLine(languageManager.ResManager.GetString("description2"));
            Console.WriteLine(languageManager.ResManager.GetString("continue"));
            Console.ReadKey();
            Console.Clear();

            bool isValidInput = false;
            while (isValidInput == false)
            {
                // Main Menu
                Console.WriteLine(languageManager.ResManager.GetString("main_menu"));
                Console.WriteLine(languageManager.ResManager.GetString("option1"));
                Console.WriteLine(languageManager.ResManager.GetString("option2"));
                Console.WriteLine(languageManager.ResManager.GetString("option3"));
                Console.WriteLine(languageManager.ResManager.GetString("option4"));
                Console.WriteLine(languageManager.ResManager.GetString("option5"));
                Console.Write(languageManager.ResManager.GetString("select_option"));

                switch (Console.ReadLine())
                {
                    case "1":

                        Console.Clear();
                        Console.WriteLine(languageManager.ResManager.GetString("create_job"));
                        CreateJob();

                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine(languageManager.ResManager.GetString("delete_job"));
                        DeleteJob();
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine(languageManager.ResManager.GetString("execute_job"));
                        LaunchJob();
                        break;
                    case "4":
                        Console.Clear();
                        Console.WriteLine(languageManager.ResManager.GetString("change_language"));
                        ChangeLanguage();
                        break;
                    case "5":
                        Console.Clear();
                        isValidInput = true;
                        Console.WriteLine(languageManager.ResManager.GetString("exiting"));
                        Environment.Exit(0);
                        break;
                    case "6":
                        Console.Clear();
                        string process = Console.ReadLine();
                        jobManager.SetBlockingProcess(process);

                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine(languageManager.ResManager.GetString("invalid_option"));
                        break;
                }
            }
        }
        public static void CreateJob()
        {
            if (jobManager.JobList.Count() == 5)
            {
                Console.WriteLine(languageManager.ResManager.GetString("max_jobs"));
                Console.WriteLine(languageManager.ResManager.GetString("press_return"));
                Console.ReadKey();
                Console.Clear();
                return;
            }
            bool isValid = false;
            while (!isValid)
            {
                Console.WriteLine(languageManager.ResManager.GetString("enter_name"));
                string name = Console.ReadLine();
                Console.WriteLine(languageManager.ResManager.GetString("enter_source"));
                string sourcePath = Console.ReadLine();
                Console.WriteLine(languageManager.ResManager.GetString("enter_target"));
                string targetPath = Console.ReadLine();
                Console.WriteLine(languageManager.ResManager.GetString("select_type"));
                string typeInput = Console.ReadLine();
                JobType type = JobType.Full;
                if (typeInput == "1")
                {
                    type = JobType.Full;
                    isValid = true;
                }
                else if (typeInput == "2")
                {
                    type = JobType.Differential;
                    isValid = true;
                }
                else
                {
                    Console.WriteLine(languageManager.ResManager.GetString("invalid_input"));
                    isValid = false;
                }
                if (isValid)
                {
                    jobManager.JobCreation(name, sourcePath, targetPath, type);
                    Console.WriteLine(languageManager.ResManager.GetString("job_created"));
                    Console.WriteLine(languageManager.ResManager.GetString("continue"));
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
        public static void DeleteJob()
        {
            if (jobManager.JobList.Count() == 0)
            {
                Console.WriteLine(languageManager.ResManager.GetString("no_jobs"));
                Console.WriteLine(languageManager.ResManager.GetString("press_return"));
                Console.ReadKey();
                Console.Clear();
                return;
            }
            bool isValid = false;
            while (!isValid)
            {
                // Display the list of jobs
                Console.WriteLine(languageManager.ResManager.GetString("list_jobs"));
                int jobIndex = 0;
                jobManager.JobList.ForEach(job => {
                    Console.WriteLine($"{jobIndex}: {languageManager.ResManager.GetString("job_name")}: {job.Name} | {languageManager.ResManager.GetString("job_source")}: {job.SourcePath} | {languageManager.ResManager.GetString("job_target")}: {job.TargetPath} | {languageManager.ResManager.GetString("job_type")}: {job.Type}");
                    jobIndex++;
                }
                );
                Console.WriteLine(languageManager.ResManager.GetString("enter_job_number_delete"));
                int jobNum = int.Parse(Console.ReadLine());
                if (jobNum < 0 || jobNum >= jobManager.JobList.Count())
                {
                    Console.WriteLine(languageManager.ResManager.GetString("invalid_job"));
                }
                else
                {
                    isValid = true;
                    jobManager.JobDeletion(jobNum);
                }

            }
            Console.WriteLine(languageManager.ResManager.GetString("job_deleted"));
            Console.WriteLine(languageManager.ResManager.GetString("press_return"));
            Console.ReadKey();
            Console.Clear();
        }
        public static void LaunchJob()
        {
            // Display the list of jobs
            Console.WriteLine(languageManager.ResManager.GetString("list_jobs"));
            Console.WriteLine($"0: {languageManager.ResManager.GetString("Execute_All_Job")}");
            int jobIndex = 0;
            jobManager.JobList.ForEach(job =>
            {
                jobIndex++;
                Console.WriteLine($"{jobIndex}: {languageManager.ResManager.GetString("job_name")}: {job.Name} | {languageManager.ResManager.GetString("job_source")}: {job.SourcePath} | {languageManager.ResManager.GetString("job_target")}: {job.TargetPath} | {languageManager.ResManager.GetString("job_type")}: {job.Type}");
            }
            );
            Console.WriteLine(languageManager.ResManager.GetString("enter_job_number_execute"));
            int jobNum = -1;
            while (jobNum < 0 || jobNum > jobManager.JobList.Count())
            {

                string input = Console.ReadLine();
                if (int.TryParse(input, out jobNum) && jobNum >= 0 && jobNum <= jobManager.JobList.Count())
                {
                    
                    int jobExit = jobManager.LaunchBackup(jobNum);
                    if (jobExit == 0)
                    {
                        Console.WriteLine(languageManager.ResManager.GetString("job_executed"));
                    }
                    else
                    {
                        Console.WriteLine(languageManager.ResManager.GetString("job_invalid"));
                    }
                    break;
                }
                Console.WriteLine(languageManager.ResManager.GetString("invalid_job"));
            }
            
            Console.WriteLine(languageManager.ResManager.GetString("press_return"));
            Console.ReadKey();
            Console.Clear();
        }
        public static void ChangeLanguage()
        {
            Console.WriteLine(languageManager.ResManager.GetString("select_language"));
            Console.WriteLine(languageManager.ResManager.GetString("language_en"));
            Console.WriteLine(languageManager.ResManager.GetString("language_fr"));

            string lg = Console.ReadLine();
            languageManager.SetLanguage(lg);
            Console.WriteLine(languageManager.ResManager.GetString("language_sucessfull"));
            Console.WriteLine(languageManager.ResManager.GetString("press_return"));
            Console.ReadKey();
            Console.Clear();
        }
    }
}