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
    public partial class ManageEvaluation : Form
    {
        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";
        public ManageEvaluation()
        {
            InitializeComponent();
            DisplayEvaluation();
        }

        public void DisplayEvaluation()
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            SqlCommand cmd1;
            string display = "SELECT* FROM Evaluation";
            cmd1 = new SqlCommand(display, con);
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd1;
            DataTable evaluationdata = new DataTable();
            sda.Fill(evaluationdata);
            dataGridView1.DataSource = evaluationdata;

            DataGridViewButtonColumn editButton;
            editButton = new DataGridViewButtonColumn();
            editButton.HeaderText = "Update";
            editButton.Text = "Update";
            editButton.UseColumnTextForButtonValue = true;
            editButton.Width = 80;
            dataGridView1.Columns.Add(editButton);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
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
            con.Close();
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void Back_Click(object sender, EventArgs e)
        {
            Initial form = new Initial();
            form.Show();
            this.Close();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ManageEvaluation_Load(object sender, EventArgs e)
        {

        }

        private void Back_Click_1(object sender, EventArgs e)
        {
            Initial form = new Initial();
            form.Show();
            this.Close();

        }

        private void dataGridView1_CellContentClick_2(object sender, DataGridViewCellEventArgs e)
        {

            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            int currentRow = int.Parse(e.RowIndex.ToString());
            int currentColumnIndex = int.Parse(e.ColumnIndex.ToString());
            int id = Convert.ToInt32(dataGridView1.Rows[currentRow].Cells[2].Value.ToString());
            if (currentColumnIndex == 0)
            {
                AddEvaluation fe = new AddEvaluation(id);
                fe.Show();
                this.Hide();
            }

            if (currentColumnIndex == 1)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this item ??",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    SqlCommand cmd1;
                    SqlCommand cmd2;
                    string deleteevaluation = "DELETE FROM Evaluation Where Id = @num";
                    string deletegroupev = "DELETE FROM GroupEvaluation Where EvaluationId = @num";
                    cmd1 = new SqlCommand(deleteevaluation, con);
                    cmd2 = new SqlCommand(deletegroupev, con);
                    cmd1.Parameters.AddWithValue("@num", id);
                    cmd2.Parameters.AddWithValue("@num", id);
                    cmd2.ExecuteNonQuery();
                    cmd1.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record deleted succesfully");
                    ManageEvaluation f3 = new ManageEvaluation();
                    this.Close();
                    f3.Show();
                }

            }

        }
    }
}
