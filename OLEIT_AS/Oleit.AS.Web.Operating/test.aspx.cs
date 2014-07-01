using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.IO;
using System.Web.Services;
using System.Data;
namespace Accounting_System
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string aa = "1111";
            switch (aa)
            {
                case"1111":
                    aa = "2222";
                    break;
                case"2222":
                    aa="3333";
                    break;
            }
            if (!IsPostBack)
            {
                //DataTable dt = new DataTable();
                //dt.Columns.Add("Role1");
                //dt.Columns.Add("Role2");
                //DataRow row = new DataRow();
                //row["Role1"] = "AAA";
                //row["Role2"] = "BBB";
                //dt.Rows.Add(row);
                //GridView1.DataSource = dt;
                //GridView1.DataBind();
          
            }
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchCustomers(string prefixText, int count)
        {

            List<string> customers = new List<string>();
            customers.Add("Taipei");
            customers.Add("Table");
            customers.Add("Ttaaa");
            customers.Add("Tlodkdj");
            customers.Add("Taadd");
            customers.Add("Kaohsiung");
            customers.Add("Kabbg");
            return customers;

        }
        [WebMethod]
        public String GetName()
        {
            
                return "餵公子吃餅";
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            XLWorkbook workbook = new XLWorkbook();
            workbook.Worksheets.Add("Sample").Cell(1, 1).SetValue("Hello World");

            // Prepare the response
            HttpResponse httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"HelloWorld.xlsx\"");

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();
        }
    }
}