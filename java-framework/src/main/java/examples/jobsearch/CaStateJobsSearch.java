package examples.jobsearch;

import static java.util.concurrent.TimeUnit.SECONDS;
import static webui.automation.browser.BrowserType.FIREFOX;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Date;
import java.util.List;

import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.NoSuchElementException;

import webui.automation.browser.Browser;
import webui.automation.elements.*;
import webui.automation.framework.BaseElement;
import webui.automation.framework.BasePage;
import webui.automation.framework.WebUI;

/**
 * This example browses the California state government jobs web site
 * (https://jobs.ca.gov/), extracts available job vacancies, and writes
 * the output into HTML files under a folder named after today's date.
 * 
 * <P>To run the program, modify the jobTitles field to put in the
 * desired job titles, save the change, right-click the program and
 * then select Run As > Java Application.</P>
 * 
 * <P>It will open a Firefox browser, browser through available job
 * vacancies for each specified job title, and then write the vacancy
 * details into HTML files in a directory named after todays' date
 * (e.g., 10-28).</P>
 */
public class CaStateJobsSearch {

    static final Browser browser = WebUI.getDefaultBrowser();

    static SimpleHtmlWriter htmlWriter;
    
    public static void main( String[] args ) {

        String[] jobTitles = new String[] {
            "SYSTEMS SOFTWARE SPECIALIST I (TECHNICAL)"
            //"SYSTEMS SOFTWARE SPECIALIST II (TECHNICAL)",
            //"SYSTEMS SOFTWARE SPECIALIST III (TECHNICAL)",
            //"STAFF INFORMATION SYSTEMS ANALYST (SPECIALIST)",
            //"SENIOR INFORMATION SYSTEMS ANALYST (SPECIALIST)",
            //"STAFF PROGRAMMER ANALYST (SPECIALIST)",
            //"SENIOR PROGRAMMER ANALYST (SPECIALIST)"
        };

        // Use today's date as the subdir name.
        SimpleDateFormat format = new SimpleDateFormat("MM-dd");
        String dirName = format.format(new Date());
        
        // Make a subdir.
        File dir = new File(dirName);
        if (!dir.exists()) {
            dir.mkdir();
        }

        // Open browser and search each job title.
        browser.open(FIREFOX);
        try {
            for(String jobTitle : jobTitles) {
                System.out.println("Start searching jobs for " + jobTitle);
                File outputFile = new File(dir, jobTitle + ".html");
                doSearch(jobTitle, outputFile);
                System.out.println("Finish searching jobs for " + jobTitle);
                System.out.println();
            }
        } finally {
            browser.close();
        }
    }

    private static void doSearch(String jobTitle, File outputFile) {
        JobSearchPage searchPage = new JobSearchPage();
        try {
            browser.navigateTo(searchPage.url);
            searchPage.waitUntilAvailable();
            ExamsAndJobVacanciesSearchResultsPage searchResultsPage = searchPage.doSearch(jobTitle, true);
            searchResultsPage.waitUntilAvailable();
            System.out.println("Got 'Exams & Job Vacancies Search Results' page.");
                
            System.out.println("Open output file: " + outputFile.getPath() + "\\" + outputFile.getName());
            htmlWriter = new SimpleHtmlWriter(new FileOutputStream(outputFile));
            try {
                htmlWriter.beginHtml();
                htmlWriter.beginTable();
                searchResultsPage.loopThroughJobTitles(htmlWriter);
            } finally {
                htmlWriter.endTable().endHtml().close();
            }

        } catch (IOException ioe) {
            System.out.println(ioe.getMessage());
        }
    }

    static class JobSearchPage extends BasePage<JobSearchPage> {
        public String url = "https://jobs.ca.gov/";
        public TextField jobTitleTextField = new TextField(By.id("cphMainContent_JobSearch_keyword"));
        public CheckBox jobTitleSearchOnlyCheckbox = new CheckBox(By.id("cphMainContent_JobSearch_cbTitleSearch"));
        public TextField departmentTextField = new TextField(By.id("cphMainContent_JobSearch_ddlDepartments1"));
        public Button searchButton = new Button(By.id("cphMainContent_JobSearch_ibtnSearch3"));

        public JobSearchPage() {
            super.setKeyElement(jobTitleTextField);
        }

        public ExamsAndJobVacanciesSearchResultsPage doSearch(String jobTitle, boolean titleSearchOnly) {
            this.jobTitleTextField.setText(jobTitle);
            if (titleSearchOnly) {
                this.jobTitleSearchOnlyCheckbox.check();
            }
            this.searchButton.click();
            return new ExamsAndJobVacanciesSearchResultsPage();
        }
    }
    
    /**
     * This page is the main search result page.
     * It has title "Exams & Job Vacancies Search Results" and
     * has a list of Occupation Categories below the title.
     */
    static class ExamsAndJobVacanciesSearchResultsPage extends BasePage<ExamsAndJobVacanciesSearchResultsPage> {

        private Table table = new Table(By.id("cphMainContent_OccGrid"));

        public ExamsAndJobVacanciesSearchResultsPage() {
            super.setKeyElement(table);
        }
        
        public void loopThroughJobTitles(SimpleHtmlWriter htmlWriter) {
            int rowCount = this.table.getRowCount();
            System.out.println("This page has " + rowCount + " rows.");
            for (int rowIdx = 0; rowIdx < rowCount; rowIdx++) {
                try {
                    String vacanciesLinkId = "cphMainContent_OccGrid_lbVacacnyCount_" + rowIdx;
                    TextLink vacanciesLink = new TextLink(By.id(vacanciesLinkId));
                    if (vacanciesLink.exists(0)) {
                        String vacanciesTitle = this.table.getCellText(rowIdx, 0);
                        System.out.println();
                        System.out.println("Clicking the '" + vacanciesTitle + "' vacancies link on row " + rowIdx + " (id=" + vacanciesLinkId + ")");
                        vacanciesLink.click();

                        new JobVacancySearchResultsPage()
                            .waitUntilAvailable()
                            .loopThroughJobVacancies(htmlWriter);

                        System.out.println("Navigate back to the previous (Exams & Job Vacancies Search Results) page");
                        browser.navigateBack();
                        waitUntilAvailable();
                    }
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        }
    }

    /**
     * This page is displayed when clicking a "## Vacancies" link in the
     * <b>Exams & Job Vacancies Search Results</b> page.
     */
    static class JobVacancySearchResultsPage extends BasePage<JobVacancySearchResultsPage> {
        private Table table = new Table(By.id("cphMainContent_grdVacancy"));
        
        public JobVacancySearchResultsPage() {
            super.setKeyElement(table);
        }
        
        public void loopThroughJobVacancies(SimpleHtmlWriter htmlWriter) {
            
            // Find the list of pages at the table foot.
            List<WebElement> resultPages = this.table.findElements(By.cssSelector("tfoot table td"));
            int numOfPages = resultPages.size();
            if (resultPages.size() == 0) {
                numOfPages = 1;
                System.out.println("This 'Job Vacancy Search Results' has only 1 page.");
            } else {
                numOfPages = resultPages.size();
                System.out.println("This 'Job Vacancy Search Results' has " + numOfPages + " pages.");
            }

            for(int pageIdx = 0; pageIdx < numOfPages; pageIdx++) {
                if (pageIdx > 0) {
                    System.out.println("Clicking link to page " + (pageIdx+1));
                    try {
                        gotoPage(pageIdx);
                    } catch (NoSuchElementException e) {
                        System.out.println("Failed to go into page " + (pageIdx+1) + " due to exception; skip this page.");
                        e.printStackTrace();
                        System.out.println();
                    }
                }
                System.out.println();
                System.out.println("Start parsing page " + (pageIdx+1));
                int rowCount = this.table.getRowCount();
                System.out.println("Page " + (pageIdx+1) + " lists " + rowCount + " vacancies.");

                for (int rowIdx = 0; rowIdx < rowCount; rowIdx++) {
                    try {
                        List<String> textList = this.table.getCellTexts(rowIdx);
                        String jobTitle = textList.get(0);
                        String salary = textList.get(1);
                        String jobType = textList.get(2);
                        String deptAndLocation = textList.get(3);
                        String postedDate = textList.get(4);
                        String deadline = textList.get(5);

                        System.out.println("Clicking vacancy " + (rowIdx+1) + " on page " + (pageIdx+1));
                        String postingLinkId = "cphMainContent_grdVacancy_hypJobTitle_" + rowIdx;
                        TextLink postingLink = new TextLink(By.id(postingLinkId));
                        postingLink.click();

                        JobDescriptionSection jobDesc = new JobDescriptionSection();
                        jobDesc.waitUntilPresent(10);

                        Text jobCtrlNumberText = new Text(By.id("lblDetailsJobControlNumber"));
                        String jobCtrlNumber = jobCtrlNumberText.getText();
                        Text workingTitleText = new Text(By.id("lblWorkingTitle"));
                        if (workingTitleText.exists()) {
                            jobTitle += "<br/>" + workingTitleText.getText();
                        }
                        //Text jobClassText = new Text(By.id("lblPrimaryClassification"));
                        //jobTitle += jobClassText.getText();

                        //Text deptText = new Text(By.id("lblDepartmentName"));
                        //String deptName = deptText.getText();
                        
                        //Text filingDateText = new Text(By.id("lblFinalFilingDate"));
                        //String filingDate = filingDateText.getText();
                        
                        System.out.println("Writing " + jobCtrlNumber + ": " + jobTitle);
                        List<String> basicInfo = Arrays.asList(jobCtrlNumber, jobTitle, salary, jobType, deptAndLocation, postedDate, deadline);
                        htmlWriter.writeTableRow(basicInfo);

                        List<String> jobDescTexts = jobDesc.getTexts();
                        htmlWriter.out.print("<tr><td colspan=\"" + basicInfo.size() + "\">");
                        for(String desc : jobDescTexts) {
                            htmlWriter.out.println("<p>" + desc + "</p>");
                        }
                        htmlWriter.out.println("</td></tr>");

                        System.out.println("Navigate back to the previous (Job Vacancy Search Results) page");
                        browser.navigateBack();
                        waitUntilAvailable();
                        // Because the Back button always goes back the first page,
                        // if this row is not the last row, we need to go into the desired page again.
                        /*if (pageIdx > 0 && i < rowCount) {
                            System.out.println("Clicking link to page " + (pageIdx+1));
                            try {
                                vacanciesResultPage.gotoPage(pageIdx);
                            } catch (NoSuchElementException e) {
                                System.out.println("Failed to go back to page " + (pageIdx+1) + " due to exception:");
                                e.printStackTrace();
                                System.out.println();
                            }
                        }*/
                    } catch (NoSuchElementException e) {
                        e.printStackTrace();
                        //browser.takeScreenShot();
                    }
                }
            }
        }

        public void gotoPage(int pageIdx) {
            // If the page to look into is not page #1 (pageIdx=0), 
            // we need to click the page number link at the bottom of the table.
            try {
                WebElement pageLinkElement = this.table.findElement(
                    By.cssSelector("tfoot table tbody tr td:nth-child(" + (pageIdx+1) + ") a"));
                pageLinkElement.click();
                // Make sure we have a minimum wait.
                // TODO: Check the non-link page number instead.
                browser.sleep(3, SECONDS);
            } catch (NoSuchElementException e) {
                System.out.println("Cannot find link to page " + (pageIdx+1));
                e.printStackTrace();
                //browser.takeScreenShot();
            }
            waitUntilAvailable();
        }
    }

    static class JobDescriptionSection extends BaseElement {
        public JobDescriptionSection() {
            super(By.id("pnlJobDescription"));
        }

        public List<String> getTexts() {
            WebElement webElement = getWebElement();
            List<String> result = new ArrayList<String>();
            List<WebElement> textElements = webElement.findElements(By.xpath("p/span"));
            for(WebElement textElem : textElements) {
                result.add(textElem.getText());
            }
            List<WebElement> listElements = webElement.findElements(By.xpath("ul/li"));
            for(WebElement listElem : listElements) {
                result.add(listElem.getText());
            }
            if (textElements.size() == 0 && listElements.size() == 0) {
                List<WebElement> allElements = webElement.findElements(By.xpath("*"));
                for(WebElement elem : allElements) {
                    result.add(elem.getText());
                }
            }
            List<WebElement> linkElements = webElement.findElements(By.tagName("a"));
            for(WebElement linkElem : linkElements) {
                String link = linkElem.getAttribute("href");
                result.add("<a href=\"" + link + "\">" + link + "</a>");
            }
            return result;
        }
    }

}
