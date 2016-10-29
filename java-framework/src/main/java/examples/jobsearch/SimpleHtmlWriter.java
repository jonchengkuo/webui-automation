package examples.jobsearch;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.util.List;
import java.util.Properties;

public class SimpleHtmlWriter {

    public static String templateFileName = "examples/jobsearch/html_template.properties";
    public static Properties templates = new Properties();
    public static String indent = "  ";

    static {
        InputStream is = SimpleHtmlWriter.class.getClassLoader().getResourceAsStream(templateFileName);
        if (is == null) {
            throw new RuntimeException("Cannot find template resource file: " + templateFileName);
        }
        try {
            templates.load(is);
        } catch (IOException e) {
            e.printStackTrace();
            //throw new RuntimeException(e.getMessage());
        }
    }

    public PrintWriter out;

    public SimpleHtmlWriter(OutputStream out) {
        this.out = new PrintWriter(out);
    }

    public void close() {
        this.out.flush();
        this.out.close();
    }

    public SimpleHtmlWriter beginHtml() {
        out.println("<html>");
        String head = templates.getProperty("head", "<head/>");
        out.println(head);
        out.println("<body>");
        return this;
    }

    public SimpleHtmlWriter endHtml() {
        out.println("</body>");
        out.println("</html>");
        return this;
    }

    public SimpleHtmlWriter write(String str) {
        out.print(str);
        return this;
    }

    public SimpleHtmlWriter beginTable() {
        out.println("<table>");
        return this;
    }

    public SimpleHtmlWriter endTable() {
        out.println("</table>");
        return this;
    }

    public SimpleHtmlWriter writeTableHead(List<String> headingList) {
        out.print(indent);
        out.println("<thead><tr>");
        for(String heading : headingList) {
            out.print(indent);
            out.print(indent);
            out.print("<th>");
            out.print(heading);
            out.println("</th>");
        }
        out.print(indent);
        out.println("</tr></thead>");
        return this;
    }

    public SimpleHtmlWriter writeTableRow(List<String> cellDataList) {
        out.print(indent);
        out.println("<tr>");
        for(String cellData : cellDataList) {
            out.print(indent);
            out.print(indent);
            out.print("<td>");
            out.print(cellData);
            out.println("</td>");
        }
        out.print(indent);
        out.println("</tr>");
        return this;
    }

}
