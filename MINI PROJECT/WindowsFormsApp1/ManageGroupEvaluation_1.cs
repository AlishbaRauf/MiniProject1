using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ManageGroupEvaluation_1 : Form
    {
        public int flag;
        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";

        public ManageGroupEvaluation_1()
        {
            InitializeComponent();
            Evaluationid.DropDownStyle = ComboBoxStyle.DropDownList;
            DisplayEvaluations();
        }

        public ManageGroupEvaluation_1(int i)
        { 
            InitializeComponent();
            flag = i;
            Evaluationid.DropDownStyle = ComboBoxStyle.DropDownList;
            DisplayEvaluations();
        } 

        public void DisplayEvaluations()
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            SqlCommand cmd1;
            string display = "SELECT * FROM Evaluation WHERE Evaluation.Id NOT IN (SELECT EvaluationId FROM GroupEvaluation WHERE GroupId = @val)";
            cmd1 = new SqlCommand(display, con);
            cmd1.Parameters.AddWithValue("@val", flag);
            cmd1.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd1;
            DataTable evdt = new DataTable();
            sda.Fill(evdt);
            dataGridView1.DataSource = evdt;

            foreach (DataRow row in evdt.Rows)
            {
                Evaluationid.Items.Add(row[0]);
            }

            /*SqlCommand cmd2;
            string display1 = "SELECT Id AS [Group Number],COUNT(*) AS [Number of Students] FROM [Group] JOIN GroupStudent ON [Group].Id = GroupStudent.GroupId GROUP BY [Group].Id,Status HAVING Status = @val1";
            cmd2 = new SqlCommand(display1, con);
            cmd2.Parameters.AddWithValue("@val1", 3);
            cmd2.ExecuteNonQuery();
            SqlDataAdapter sda1 = new SqlDataAdapter();
            sda1.SelectCommand = cmd2;
            DataTable groupdt = new DataTable();
            sda1.Fill(groupdt);

            foreach (DataRow row in groupdt.Rows)
            {
                groupid.Items.Add(row[0]);
            } */


            /* DataGridViewButtonColumn selectButton;
             selectButton = new DataGridViewButtonColumn();
             selectButton.HeaderText = "Select";
             selectButton.Text = "Select";
             selectButton.UseColumnTextForButtonValue = true;
             selectButton.Width = 80;
             dataGridView1.Columns.Add(selectButton);
             for (int i = 0; i < dataGridView1.Rows.Count; i++)
             {
                 dataGridView1.Rows[i].Cells[0].ReadOnly = true;
             } */
        }

       /* private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRow = int.Parse(e.RowIndex.ToString());
            int currentColumnIndex = int.Parse(e.ColumnIndex.ToString());
            int evid = Convert.ToInt32(dataGridView1.Rows[currentRow].Cells[1].Value.ToString());
            if (currentColumnIndex == 0)
            {
                Evaluationid.Text = evid.ToString();
            }
        } */


        private void Back_Click(object sender, EventArgs e)
        {
            ManageGroupEvaluation form = new ManageGroupEvaluation();
            this.Close();
            form.Show();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            bool i = myvalidations();
            if (i == true)
            {
                SqlConnection con = new SqlConnection(conURL);
                con.Open();
                SqlCommand cmd1;
                string insertgroupev = "INSERT INTO GroupEvaluation(GroupId, EvaluationId, ObtainedMarks, EvaluationDate) values(@GroupId, @EvaluationId, @ObtainedMarks, @EvaluationDate)";
                cmd1 = new SqlCommand(insertgroupev, con);
                cmd1.Parameters.AddWithValue("@GroupId", flag);
                cmd1.Parameters.AddWithValue("@EvaluationId", Evaluationid.Text);
                cmd1.Parameters.AddWithValue("@ObtainedMarks", ObtainedMarks.Text);
                cmd1.Parameters.AddWithValue("@EvaluationDate", DateTime.Today);
                cmd1.ExecuteNonQuery();
                MessageBox.Show("Record Inserted");
                ManageGroupEvaluation form1 = new ManageGroupEvaluation();
                this.Close();
                form1.Show();
            }
            
        }

        public bool myvalidations()
        {
            int i = 0;


            if (String.IsNullOrEmpty(ObtainedMarks.Text))
            {
                errorProvider1.SetError(ObtainedMarks, "Required Field");
                i = 1;
            }
            else
            {
                errorProvider1.Clear();
            }

            if (!String.IsNullOrEmpty(ObtainedMarks.Text) && !Regex.Match(ObtainedMarks.Text, @"^\d+$").Success)
            {
                errorProvider2.SetError(ObtainedMarks, "Must contain digits only");
                i = 1;
            }
            else
            {
                errorProvider2.Clear();
            }

            if (String.IsNullOrEmpty(Evaluationid.Text))
            {
                errorProvider3.SetError(Evaluationid, "Required Field");
                i = 1;
            }
            else
            {
                errorProvider3.Clear();
            }

            /*if (String.IsNullOrEmpty(groupid.Text))
            {
                errorProvider4.SetError(groupid, "Required Field");
                i = 1;
            }
            else
            {
                errorProvider4.Clear();
            }*/


            if (i == 1)
            {
                return false;
            }
            else
            {
                return true;
            }


        }

        private void ManageGroupEvaluation_1_Load(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void Evaluationid_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
