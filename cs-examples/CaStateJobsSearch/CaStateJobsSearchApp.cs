using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using WebUI.Automation.Elements;
using WebUI.Automation.Framework;
using static WebUI.Automation.Elements.ElementFactory;

namespace WebUI.Automation.Examples.CaStateJobsSearch
{

    /// <summary>
    /// This class represents the web UI of the California state government jobs web site
    /// (https://jobs.ca.gov/). When you launch this web app and execute DoSearch for a particular
    /// job title, it will do a job search using this job title as an exact matchextracts available job vacancies, and writes
    /// the output into HTML files under a folder named after today's date.
    /// <P>This web  will open an IE browser, browser through available job
    /// vacancies for each specified job title, and then write the vacancy
    /// details into HTML files in a directory named after todays' date
    /// (e.g., 10-28).</P>
    /// </summary>
    class CaStateJobsSearchApp : BaseApp
    {
        JobSearchPage JobSearchPage = new JobSearchPage();
        ExamsAndJobVacanciesSearchResultsPage InitialJobSearchResultsPage = new ExamsAndJobVacanciesSearchResultsPage();

        SimpleHtmlWriter htmlWriter;

        // Define the CSS in the HTML head section for the table in the search result output HTML.
        static string htmlHead = @"
<head>
  <style>
    table, th, td {
      border: 1px solid black;
      border-collapse: collapse;
    }
  </style>
</head>
";

        internal CaStateJobsSearchApp()
            : base("https://jobs.ca.gov/")
        {
        }

        internal void DoSearch(string jobTitle, bool exactMatch, String outputFileName)
        {
            Browser.NavigateTo(HomeUrl);
            JobSearchPage.WaitUntilAvailable();
            JobSearchPage.DoSearch(jobTitle, exactMatch);
            InitialJobSearchResultsPage.WaitUntilAvailable();
            Console.WriteLine("Got 'Exams & Job Vacancies Search Results' page.");

            htmlWriter = new SimpleHtmlWriter(outputFileName);
            htmlWriter.HtmlHead = htmlHead;

            try
            {
                htmlWriter.BeginHtml();
                htmlWriter.BeginTable();
                InitialJobSearchResultsPage.LoopThroughJobTitles(htmlWriter);
            }
            finally
            {
                htmlWriter.EndTable().EndHtml().Close();
            }
        }

    }

    class JobSearchPage : BasePage<JobSearchPage>
    {
        TextField jobTitleTextField = new TextField(By.Id("cphMainContent_JobSearch_keyword"));
        CheckBox jobTitleSearchOnlyCheckbox = new CheckBox(By.Id("cphMainContent_JobSearch_cbTitleSearch"));
        TextField departmentTextField = new TextField(By.Id("cphMainContent_JobSearch_ddlDepartments1"));
        Button searchButton = new Button(By.Id("cphMainContent_JobSearch_ibtnSearch3"));

        public JobSearchPage()
        {
            base.Locator = jobTitleTextField.Locator;
        }

        public virtual JobSearchPage DoSearch(string jobTitle, bool titleSearchOnly)
        {
            this.jobTitleTextField.Text = jobTitle;
            if (titleSearchOnly)
            {
                this.jobTitleSearchOnlyCheckbox.Check();
            }
            this.searchButton.Click();
            return this;
        }
    }

    /// <summary>
    /// This page is the main search result page.
    /// It has title "Exams & Job Vacancies Search Results" and
    /// has a list of Occupation Categories below the title.
    /// </summary>
    class ExamsAndJobVacanciesSearchResultsPage : BasePage<ExamsAndJobVacanciesSearchResultsPage>
    {
        internal Table table = new Table(By.Id("cphMainContent_OccGrid"));

        public ExamsAndJobVacanciesSearchResultsPage()
        {
            base.Locator = table.Locator;
        }

        public virtual void LoopThroughJobTitles(SimpleHtmlWriter htmlWriter)
        {
            int rowCount = this.table.RowCount;
            Console.WriteLine("The search result page has " + rowCount + " rows.");
            for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
            {
                try
                {
                    string vacanciesLinkId = "cphMainContent_OccGrid_lbVacacnyCount_" + rowIdx;
                    TextLink vacanciesLink = new TextLink(By.Id(vacanciesLinkId));
                    if (vacanciesLink.Exists)
                    {
                        string vacanciesTitle = this.table.GetCell(rowIdx, 0).Text;
                        Console.WriteLine();
                        Console.WriteLine("Clicking the '" + vacanciesTitle + "' vacancies link on row " + rowIdx + " (id=" + vacanciesLinkId + ")");
                        vacanciesLink.Click();

                        (new JobVacancySearchResultsPage()).WaitUntilAvailable().LoopThroughJobVacancies(htmlWriter);

                        Console.WriteLine("Navigate back to the previous (Exams & Job Vacancies Search Results) page");
                        Browser.NavigateBack();
                        WaitUntilAvailable();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                }
            }
        }
    }

    /// <summary>
    /// This page is displayed when clicking a "## Vacancies" link in the
    /// <b>Exams & Job Vacancies Search Results</b> page.
    /// </summary>
    class JobVacancySearchResultsPage : BasePage<JobVacancySearchResultsPage>
    {
        internal Table table = new Table(By.Id("cphMainContent_grdVacancy"));

        public JobVacancySearchResultsPage()
        {
            base.Locator = table.Locator;
        }

        public void LoopThroughJobVacancies(SimpleHtmlWriter htmlWriter)
        {
            // Find the list of pages at the table foot.
            IReadOnlyCollection<IWebElement> resultPages = this.table.FindElements(By.CssSelector("tfoot table td"));
            int numOfPages = resultPages.Count;
            if (resultPages.Count == 0)
            {
                numOfPages = 1;
                Console.WriteLine("This 'Job Vacancy Search Results' has only 1 page.");
            }
            else
            {
                numOfPages = resultPages.Count;
                Console.WriteLine("This 'Job Vacancy Search Results' has " + numOfPages + " pages.");
            }

            for (int pageIdx = 0; pageIdx < numOfPages; pageIdx++)
            {
                if (pageIdx > 0)
                {
                    Console.WriteLine("Clicking link to page " + (pageIdx + 1));
                    try
                    {
                        GotoPage(pageIdx);
                    }
                    catch (NoSuchElementException e)
                    {
                        Console.WriteLine("Failed to go into page " + (pageIdx + 1) + " due to exception; skip this page.");
                        Console.WriteLine(e.ToString());
                        Console.Write(e.StackTrace);
                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Start parsing page " + (pageIdx + 1));
                int rowCount = this.table.RowCount;
                Console.WriteLine("Page " + (pageIdx + 1) + " lists " + rowCount + " vacancies.");

                for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
                {
                    try
                    {
                        var textList = this.table.GetRow(rowIdx).CellTexts;
                        string jobTitle = textList[0];
                        string salary = textList[1];
                        string jobType = textList[2];
                        string deptAndLocation = textList[3];
                        string postedDate = textList[4];
                        string deadline = textList[5];

                        Console.WriteLine("Clicking vacancy " + (rowIdx + 1) + " on page " + (pageIdx + 1));
                        string postingLinkId = "cphMainContent_grdVacancy_hypJobTitle_" + rowIdx;
                        TextLink postingLink = new TextLink(By.Id(postingLinkId));
                        postingLink.Click();

                        JobPostPage jobPostPage = new JobPostPage();
                        jobPostPage.WaitUntilAvailable();

                        var details = jobPostPage.GetDetails(jobTitle);
                        jobTitle = details.Item1;
                        string jobCtrlNumber = details.Item2;
                        IList<string> jobDescTexts = details.Item3;

                        Console.WriteLine("Writing " + jobCtrlNumber + ": " + jobTitle);
                        var basicInfo = new List<string> { jobCtrlNumber, jobTitle, salary, jobType, deptAndLocation, postedDate, deadline };
                        htmlWriter.WriteTableRow(basicInfo);

                        htmlWriter.Write("<tr><td colspan=\"" + basicInfo.Count + "\">");
                        foreach (string desc in jobDescTexts)
                        {
                            htmlWriter.WriteLine("<p>" + desc + "</p>");
                        }
                        htmlWriter.WriteLine("</td></tr>");

                        Console.WriteLine("Navigate back to the previous (Job Vacancy Search Results) page");
                        Browser.NavigateBack();
                        WaitUntilAvailable();
                    }
                    catch (NoSuchElementException e)
                    {
                        Console.WriteLine(e.ToString());
                        Console.Write(e.StackTrace);
                        //browser.takeScreenShot();
                    }
                }
            }
        }

        public virtual void GotoPage(int pageIdx)
        {
            // If the page to look into is not page #1 (pageIdx=0), 
            // we need to click the page number link at the bottom of the table.
            try
            {
                IWebElement pageLinkElement = this.table.FindElement(By.CssSelector("tfoot table tbody tr td:nth-child(" + (pageIdx + 1) + ") a"));
                pageLinkElement.Click();
                // Make sure we have a minimum wait.
                // TODO: Check the non-link page number instead.
                Browser.Sleep(3);
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("Cannot find link to page " + (pageIdx + 1));
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                //browser.takeScreenShot();
            }
            WaitUntilAvailable();
        }
    }

    /// <summary>
    /// This page is displayed when clicking a link to a particular job vacancy/post.
    /// </summary>
    class JobPostPage : BasePage<JobPostPage>
    {
        JobDescriptionSection jobDesc = new JobDescriptionSection(By.Id("pnlJobDescription"));

        public JobPostPage()
        {
            base.Locator = jobDesc.Locator;
        }

        public Tuple<String, String, IList<string>> GetDetails(string jobTitle)
        {
            TextElement jobCtrlNumberText = CreateTextElement(By.Id("lblDetailsJobControlNumber"));
            string jobCtrlNumber = jobCtrlNumberText.Text;
            TextElement workingTitleText = CreateTextElement(By.Id("lblWorkingTitle"));
            if (workingTitleText.Exists)
            {
                jobTitle += "<br/>" + workingTitleText.Text;
            }
            //Text jobClassText = new Text(By.id("lblPrimaryClassification"));
            //jobTitle += jobClassText.getText();

            //Text deptText = new Text(By.id("lblDepartmentName"));
            //String deptName = deptText.getText();

            //Text filingDateText = new Text(By.id("lblFinalFilingDate"));
            //String filingDate = filingDateText.getText();

            return new Tuple<String, String, IList<string>>(jobTitle, jobCtrlNumber, jobDesc.Texts);
        }
    }

    class JobDescriptionSection : BaseElement
    {
        public JobDescriptionSection(By locator) : base(locator)
        {
        }

        public IList<string> Texts
        {
            get
            {
                IWebElement webElement = this.WebElement;
                IList<string> result = new List<string>();
                IList<IWebElement> textElements = webElement.FindElements(By.XPath("p/span"));
                foreach (IWebElement textElem in textElements)
                {
                    result.Add(textElem.Text);
                }
                IList<IWebElement> listElements = webElement.FindElements(By.XPath("ul/li"));
                foreach (IWebElement listElem in listElements)
                {
                    result.Add(listElem.Text);
                }
                if (textElements.Count == 0 && listElements.Count == 0)
                {
                    IList<IWebElement> allElements = webElement.FindElements(By.XPath("*"));
                    foreach (IWebElement elem in allElements)
                    {
                        result.Add(elem.Text);
                    }
                }
                IList<IWebElement> linkElements = webElement.FindElements(By.TagName("a"));
                foreach (IWebElement linkElem in linkElements)
                {
                    string link = linkElem.GetAttribute("href");
                    result.Add("<a href=\"" + link + "\">" + link + "</a>");
                }
                return result;
            }
        }
    }

}