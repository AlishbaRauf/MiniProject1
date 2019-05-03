using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Initial : Form
    {
        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";
        

        public Initial()
        {
            InitializeComponent();
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            string students = "SELECT COUNT(*) FROM Student";
            SqlCommand cmd = new SqlCommand(students, con);
            int nostds = (Int32)cmd.ExecuteScalar();
            label9.Text = "(" + nostds.ToString() + ")" ;

            string advisors= "SELECT COUNT(*) FROM Advisor";
            SqlCommand cmd1 = new SqlCommand(advisors, con);
            int noadv = (Int32)cmd1.ExecuteScalar();
            label7.Text = "(" + noadv.ToString() + ")";

            string evaluations = "SELECT COUNT(*) FROM GroupEvaluation";
            SqlCommand cmd2 = new SqlCommand(evaluations, con);
            int noev = (Int32)cmd2.ExecuteScalar();
            label12.Text = "(" + noev.ToString() + ")";

            string projects = "SELECT COUNT(*) FROM GroupEvaluation";
            SqlCommand cmd3 = new SqlCommand(projects, con);
            int nopr = (Int32)cmd3.ExecuteScalar();
            label18.Text = "(" + nopr.ToString() + ")";


        }

        private void button1_Click(object sender, EventArgs e)
        {
            var Form2 = new Form2();
            Form2.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var Form3 = new Form3();
            Form3.Show();
            this.Hide();
        }

        private void AddEvaluation_Click(object sender, EventArgs e)
        {
            AddEvaluation E1 = new AddEvaluation();
            E1.Show();
            this.Hide();
        }

        private void ManageEvaluation_Click(object sender, EventArgs e)
        {
            ManageEvaluation E2 = new ManageEvaluation();
            E2.Show();
            this.Hide();
        }

        private void Initial_Load(object sender, EventArgs e)
        {

        }
        private void Initial_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (string.Equals((sender as Button).Name, @"CloseButton"))
            {
                MessageBox.Show("succeeded");
            }
        
        }

        private void ManageAdvisor_Click(object sender, EventArgs e)
        {
            ManageAdvisor E3 = new ManageAdvisor();
            E3.Show();
            this.Hide();
        }

        private void AddAdvisor_Click(object sender, EventArgs e)
        {
            AddAdvisor E4 = new AddAdvisor();
            E4.Show();
            this.Hide();
        }

        private void AddProject_Click(object sender, EventArgs e)
        {
            AddProject E5 = new AddProject();
            E5.Show();
            this.Hide();
        }

        private void ManageProject_Click(object sender, EventArgs e)
        {
            ManageProject E6 = new ManageProject();
            E6.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel25_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel23_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void ManageGroups_Click(object sender, EventArgs e)
        {
            ManageStudentGroups E7 = new ManageStudentGroups();
            E7.Show();
            this.Hide();

        }

        private void ManageGroupProjects_Click(object sender, EventArgs e)
        {
            ManageGroupProjects E8 = new ManageGroupProjects();
            E8.Show();
            this.Hide();
        }

        private void ManageProjectAdvisors_Click(object sender, EventArgs e)
        {
            ManageProjectAdvisors E9 = new ManageProjectAdvisors();
            E9.Show();
            this.Hide();
        }

        private void ManageGroupEvaluation_Click(object sender, EventArgs e)
        {
            ManageGroupEvaluation E10 = new ManageGroupEvaluation();
            E10.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            GenerateReports E8 = new GenerateReports();
            E8.Show();
            this.Hide();
        }
    }
}
