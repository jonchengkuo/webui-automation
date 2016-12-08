using System.IO;
using System.Collections.Generic;

namespace WebUI.Automation.Examples.CaStateJobsSearch
{

    public class SimpleHtmlWriter
    {

        public string HtmlIndent { get; set; } = "  ";
        public string HtmlDoctype { get; set; } = "<!DOCTYPE html>";
        public string HtmlHead { get; set; } = "<head/>";

        private TextWriter writer;

        public SimpleHtmlWriter(string outputFileName)
        {
            writer = new StreamWriter(outputFileName);
        }

        public virtual void Close()
        {
            writer.Flush();
            writer.Close();
        }

        public virtual SimpleHtmlWriter BeginHtml()
        {
            writer.WriteLine(HtmlDoctype);
            writer.WriteLine("<html>");
            writer.WriteLine(HtmlHead);
            writer.WriteLine("<body>");
            return this;
        }

        public virtual SimpleHtmlWriter EndHtml()
        {
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            return this;
        }

        public virtual SimpleHtmlWriter Write(string str)
        {
            writer.Write(str);
            return this;
        }

        public virtual SimpleHtmlWriter WriteLine(string str)
        {
            writer.WriteLine(str);
            return this;
        }

        public virtual SimpleHtmlWriter BeginTable()
        {
            writer.WriteLine("<table>");
            return this;
        }

        public virtual SimpleHtmlWriter EndTable()
        {
            writer.WriteLine("</table>");
            return this;
        }

        public virtual SimpleHtmlWriter WriteTableHead(IList<string> headingList)
        {
            writer.Write(HtmlIndent);
            writer.WriteLine("<thead><tr>");
            foreach (string heading in headingList)
            {
                writer.Write(HtmlIndent);
                writer.Write(HtmlIndent);
                writer.Write("<th>");
                writer.Write(heading);
                writer.WriteLine("</th>");
            }
            writer.Write(HtmlIndent);
            writer.WriteLine("</tr></thead>");
            return this;
        }

        public virtual SimpleHtmlWriter WriteTableRow(IList<string> cellDataList)
        {
            writer.Write(HtmlIndent);
            writer.WriteLine("<tr>");
            foreach (string cellData in cellDataList)
            {
                writer.Write(HtmlIndent);
                writer.Write(HtmlIndent);
                writer.Write("<td>");
                writer.Write(cellData);
                writer.WriteLine("</td>");
            }
            writer.Write(HtmlIndent);
            writer.WriteLine("</tr>");
            return this;
        }

    }

}