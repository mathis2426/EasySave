using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ControllerModel;
using System.Resources;
using System.Reflection;

namespace program
{
    public class Program
    {

        public static ResourceManager resManager = new ResourceManager("View.Resources.Lang", Assembly.GetExecutingAssembly());

        public static JobManager jobManager = new JobManager();
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US"); 

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
            Console.WriteLine(resManager.GetString("welcome"));
            Console.WriteLine(resManager.GetString("description1"));
            Console.WriteLine(resManager.GetString("description2"));
            Console.WriteLine(resManager.GetString("continue"));
            Console.ReadKey();
            Console.Clear();

            bool isValidInput = false;
            while (isValidInput == false)
            {
                // Main Menu
                Console.WriteLine(resManager.GetString("main_menu"));
                Console.WriteLine(resManager.GetString("option1"));
                Console.WriteLine(resManager.GetString("option2"));
                Console.WriteLine(resManager.GetString("option3"));
                Console.WriteLine(resManager.GetString("option4"));
                Console.WriteLine(resManager.GetString("option5"));
                Console.Write(resManager.GetString("select_option"));

                switch (Console.ReadLine())
                {
                    case "1":

                        Console.Clear();
                        Console.WriteLine(resManager.GetString("create_job"));
                        CreateJob();

                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine(resManager.GetString("delete_job"));
                        DeleteJob();
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine(resManager.GetString("execute_job"));
                        LaunchJob();
                        break;
                    case "4":
                        Console.Clear();
                        Console.WriteLine(resManager.GetString("change_language"));

                        break;
                    case "5":
                        Console.Clear();
                        isValidInput = true;
                        Console.WriteLine(resManager.GetString("exiting"));
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine(resManager.GetString("invalid_option"));
                        break;
                }
            }
        }
        public static void CreateJob()
        {
            bool isValid = false;
            while (!isValid)
            {
                Console.WriteLine(resManager.GetString("enter_name"));
                string name = Console.ReadLine();
                Console.WriteLine(resManager.GetString("enter_source"));
                string sourcePath = Console.ReadLine();
                Console.WriteLine(resManager.GetString("enter_target"));
                string targetPath = Console.ReadLine();
                Console.WriteLine(resManager.GetString("select_type"));
                string typeInput = Console.ReadLine();
                jobType type = jobType.Full;
                if (typeInput == "1")
                {
                    type = jobType.Full;
                    isValid = true;
                }
                else if (typeInput == "2")
                {
                    type = jobType.Differential;
                    isValid = true;
                }
                else
                {
                    Console.WriteLine(resManager.GetString("invalid_input"));
                    isValid = false;
                }
                if (isValid)
                {
                    jobManager.JobCreation(name, sourcePath, targetPath, type);
                    Console.WriteLine(resManager.GetString("job_created"));
                    Console.WriteLine(resManager.GetString("continue"));
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
        public static void DeleteJob()
        {
            if (jobManager._jobList.Count() == 0)
            {
                Console.WriteLine(resManager.GetString("no_jobs"));
                Console.WriteLine(resManager.GetString("press_return"));
                Console.ReadKey();
                Console.Clear();
                return;
            }
            bool isValid = false;
            while (!isValid)
            {
                // Display the list of jobs
                Console.WriteLine(resManager.GetString("list_jobs"));
                jobManager._jobList.ForEach(job =>
                    Console.WriteLine("{0}: {1} | {2}: {3} | {4}: {5} | {6}: {7}",
                        resManager.GetString("job_name"),
                        job._name,
                        resManager.GetString("job_source"),
                        job._sourcePath,
                        resManager.GetString("job_target"),
                        job._targetPath,
                        resManager.GetString("job_type"),
                        job._type)
                );
                Console.WriteLine(resManager.GetString("enter_job_number_delete"));
                int jobNum = int.Parse(Console.ReadLine());
                if (jobNum < 0 || jobNum >= jobManager._jobList.Count())
                {
                    Console.WriteLine(resManager.GetString("invalid_job"));
                }
                else
                {
                    isValid = true;
                    jobManager.JobDeletion(jobNum);
                }

            }
            Console.WriteLine(resManager.GetString("job_deleted"));
            Console.WriteLine(resManager.GetString("press_return"));
            Console.ReadKey();
            Console.Clear();
        }
        public static void LaunchJob()
        {
            // Display the list of jobs
            Console.WriteLine(resManager.GetString("list_jobs"));
            jobManager._jobList.ForEach(job =>
                Console.WriteLine("{0}: {1} | {2}: {3} | {4}: {5} | {6}: {7}",
                    resManager.GetString("job_name"),
                    job._name,
                    resManager.GetString("job_source"),
                    job._sourcePath,
                    resManager.GetString("job_target"),
                    job._targetPath,
                    resManager.GetString("job_type"),
                    job._type)
            );
            Console.WriteLine(resManager.GetString("enter_job_number_execute"));
            int jobNum = int.Parse(Console.ReadLine());
            while (jobNum < 0 || jobNum >= jobManager._jobList.Count())
            {
                Console.WriteLine(resManager.GetString("invalid_job"));
                jobNum = int.Parse(Console.ReadLine());
            }
            jobManager.LaunchBackup(jobNum);
            Console.WriteLine(resManager.GetString("job_executed"));
            Console.WriteLine(resManager.GetString("press_return"));
            Console.ReadKey();
            Console.Clear();
        }
    }
}
