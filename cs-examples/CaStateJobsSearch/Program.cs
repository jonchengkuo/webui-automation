using System;
using System.IO;
using WebUI.Automation.Framework;

namespace WebUI.Automation.Examples.CaStateJobsSearch
{
    /// <summary>
    /// This example browses the California state government jobs web site
    /// (https://jobs.ca.gov/), extracts available job vacancies, and writes
    /// the output into HTML files under a folder named after today's date.
    /// 
    /// <P>To run the program, modify the jobTitles field to put in the
    /// desired, EXACT job titles, save the change, then, from the Visual Studio
    /// top menu, select Debug > Start Without Debugging.</P>
    /// </summary>
    class Program
    {
        static BrowserType browserType = BrowserType.IE;

        static bool areExactJobTitles = true;

        // A list of exact job titles to search for.
        static string[] jobTitles = new string[] {
            "SYSTEMS SOFTWARE SPECIALIST I (TECHNICAL)"
        };

        static void Main(string[] args)
        {
            try
            {
                string dirName = GetAndCreateOutputDir();
                // Launch the search app and search each job title.
                using (var searchApp = new CaStateJobsSearchApp())
                {
                    searchApp.Launch(browserType);
                    foreach (string jobTitle in jobTitles)
                    {
                        String outputFileName = dirName + @"\" + jobTitle + ".html";
                        Console.WriteLine($"Start searching jobs for '{jobTitle}' and writing the results to {outputFileName}.");
                        searchApp.DoSearch(jobTitle, areExactJobTitles, outputFileName);
                        Console.WriteLine($"Finish searching jobs for '{jobTitle}'.");
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        static string GetAndCreateOutputDir()
        {
            // Use today's date as the subdir name.
            DateTime today = new DateTime();
            string dirName = today.ToString("MM-dd");

            // Create the subdir if it's needed.
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            return dirName;
        }
    }
}
