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
    public partial class ManageGroupEvaluation : Form
    {
        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";

        public ManageGroupEvaluation()
        {
            InitializeComponent();
            DisplayGroups();
        }

        public void DisplayGroups()
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();

            string display1 = "SELECT GroupId AS [Group Number], Evaluation.Id AS [Evaluation Number] ,Name AS [Evaluation Name],ObtainedMarks, EvaluationDate FROM GroupEvaluation JOIN Evaluation ON Evaluation.Id = GroupEvaluation.EvaluationId";
            SqlCommand cmd2 = new SqlCommand(display1, con);
            cmd2.ExecuteNonQuery();
            SqlDataAdapter sda1 = new SqlDataAdapter();
            sda1.SelectCommand = cmd2;
            DataTable evaldt = new DataTable();
            sda1.Fill(evaldt);
            dataGridView1.DataSource = evaldt;

            string display = "SELECT Id AS [Group Number] FROM [Group] JOIN GroupStudent ON [Group].Id = GroupStudent.GroupId GROUP BY [Group].Id,Status HAVING Status = @val1";
            SqlCommand cmd = new SqlCommand(display, con);
            cmd.Parameters.AddWithValue("@val1", 3);
            cmd.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd;
            DataTable projectdt = new DataTable();
            sda.Fill(projectdt);
            dataGridView2.DataSource = projectdt;

            DataGridViewButtonColumn evaluateButton;
            evaluateButton = new DataGridViewButtonColumn();
            evaluateButton.HeaderText = "Evaluate";
            evaluateButton.Text = "Evaluate";
            evaluateButton.UseColumnTextForButtonValue = true;
            evaluateButton.Width = 80;
            dataGridView2.Columns.Add(evaluateButton);
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                dataGridView2.Rows[i].Cells[0].ReadOnly = true;
            }
           

            DataGridViewButtonColumn deleteButton;
            deleteButton = new DataGridViewButtonColumn();
            deleteButton.HeaderText = "Delete";
            deleteButton.Text = "Delete";
            deleteButton.UseColumnTextForButtonValue = true;
            deleteButton.Width = 80;
            dataGridView1.Columns.Add(deleteButton);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
            }
        }


        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRow = int.Parse(e.RowIndex.ToString());
            int currentColumnIndex = int.Parse(e.ColumnIndex.ToString());
            int gid = Convert.ToInt32(dataGridView2.Rows[currentRow].Cells[1].Value.ToString());
            if (currentColumnIndex == 0)
            {
                ManageGroupEvaluation_1 f3 = new ManageGroupEvaluation_1(gid);
                this.Close();
                f3.Show();
            }
        }



        private void Back_Click(object sender, EventArgs e)
        {
            Initial form = new Initial();
            this.Close();
            form.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRow = int.Parse(e.RowIndex.ToString());
            int currentColumnIndex = int.Parse(e.ColumnIndex.ToString());
            int gid = Convert.ToInt32(dataGridView1.Rows[currentRow].Cells[1].Value.ToString());
            int evid = Convert.ToInt32(dataGridView1.Rows[currentRow].Cells[2].Value.ToString());
            /*if (currentColumnIndex == 0)
            {
                ManageGroupEvaluation_1 f4 = new ManageGroupEvaluation_1(gid);
                this.Close();
                f4.Show();
            }*/

            if (currentColumnIndex == 0)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this item ??",
                                   "Confirm Delete!!",
                                   MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    SqlConnection con = new SqlConnection(conURL);
                    con.Open();
                    string delete = "DELETE FROM GroupEvaluation Where GroupId = @val1 AND EvaluationId = @val2";
                    SqlCommand cmd = new SqlCommand(delete, con);
                    cmd.Parameters.AddWithValue("@val1", gid);
                    cmd.Parameters.AddWithValue("@val2", evid);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record deleted succesfully");
                    ManageGroupEvaluation f5 = new ManageGroupEvaluation();
                    this.Close();
                    f5.Show();
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ManageGroupEvaluation_1 f6 = new ManageGroupEvaluation_1( );
            this.Close();
            f6.Show();

        }

        private void ManageGroupEvaluation_Load(object sender, EventArgs e)
        {

        }

       
    }
}
