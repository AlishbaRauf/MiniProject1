using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace WindowsFormsApp1

{
    public partial class GenerateReports : Form
    {

        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";

        public GenerateReports()
        {
            InitializeComponent();
        }

        private void GenerateReports_Load(object sender, EventArgs e)
        {

        }

        private void Report1_Click(object sender, EventArgs e)
        {
            string select = "SELECT Project.Id AS [Project Id], Project.Title AS [Project Title],  ProjectAdvisor.AdvisorId AS [AdvisorId], Student.RegistrationNo FROM ((((Project JOIN ProjectAdvisor ON Project.Id = ProjectAdvisor.ProjectId) JOIN GroupProject ON Project.Id = GroupProject.ProjectId) JOIN [Group] ON [Group].Id = GroupProject.GroupId) JOIN GroupStudent ON [Group].Id = GroupStudent.GroupId) JOIN Student ON Student.Id = GroupStudent.StudentId ORDER BY Project.Id";
            SqlConnection con = new SqlConnection(conURL);
            SqlCommand cmd = new SqlCommand(select, con);
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd;
            DataTable stdata = new DataTable();
            sda.Fill(stdata);
            createPDF(stdata, "Report#1.pdf");
        }

        private void Report2_Click(object sender, EventArgs e)
        {
            
        }

        public void createPDF(DataTable dataTable, string destinationpath)
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationpath, FileMode.Create));
            document.Open();

            PdfPTable table = new PdfPTable(dataTable.Columns.Count);
            table.WidthPercentage = 100;

            //Set the names of columns in the pdf file
            for (int k = 0; k < dataTable.Columns.Count; k++)
            {
                PdfPCell cell = new PdfPCell(new Phrase(dataTable.Columns[k].ColumnName));

                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(51, 102, 102);

                table.AddCell(cell);
            }

            //Add the values of DataTable in the pdf file which is generated
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(dataTable.Rows[i][j].ToString()));

                    //Align the cell in the center of column
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;

                    table.AddCell(cell);
                }
            }

            document.Add(table);
            document.Close();
            MessageBox.Show("Report Generated");
        }

        private void Back_Click(object sender, EventArgs e)
        {
            Initial form = new Initial();
            this.Close();
            form.Show();
        }

        private void Report2_Click_1(object sender, EventArgs e)
        {
            string select = "SELECT [Group].Id AS [Group Id], Evaluation.Name,Evaluation.TotalMarks, GroupEvaluation.ObtainedMarks, student.RegistrationNo  FROM  (((GroupEvaluation JOIN Evaluation ON  GroupEvaluation.EvaluationId = Evaluation.Id) JOIN [Group] ON [Group].Id = GroupEvaluation.GroupId) JOIN GroupStudent ON GroupStudent.GroupId = [Group].Id) JOIN Student ON Student.Id = GroupStudent.StudentId ";
            SqlConnection con = new SqlConnection(conURL);
            SqlCommand cmd = new SqlCommand(select, con);
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd;
            DataTable stdata = new DataTable();
            sda.Fill(stdata);
            createPDF(stdata, "Report#2.pdf");

        }
    }
}

        
