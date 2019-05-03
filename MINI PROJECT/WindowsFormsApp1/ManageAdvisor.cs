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
    public partial class ManageAdvisor : Form
    {
        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";

        public ManageAdvisor()
        {
            InitializeComponent();
            DisplayAdvisor();
        }

        private void ManageAdvisor_Load(object sender, EventArgs e)
        {

        }

        public void DisplayAdvisor()
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            SqlCommand cmd1;
            string display = "SELECT FirstName, LastName, Lookup.Value, Salary, Email, Contact FROM (Person JOIN Advisor ON Person.Id = Advisor.Id) JOIN Lookup ON Advisor.Designation = Lookup.Id";
            cmd1 = new SqlCommand(display, con);
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd1;
            DataTable advisordt = new DataTable();
            sda.Fill(advisordt);
            dataGridView1.DataSource = advisordt;

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

        private void Back_Click_1(object sender, EventArgs e)
        {
            Initial form = new Initial();
            form.Show();
            this.Close();

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            int currentRow = int.Parse(e.RowIndex.ToString());
            int currentColumnIndex = int.Parse(e.ColumnIndex.ToString());
            string email = string.Format("SELECT Id FROM Person WHERE Email = '{0}'", dataGridView1.Rows[currentRow].Cells[6].Value.ToString());
            SqlCommand cmd3 = new SqlCommand(email, con);
            int id = (Int32)cmd3.ExecuteScalar();
            if (currentColumnIndex == 0)
            {
                AddAdvisor f2 = new AddAdvisor(id);
                f2.Show();
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
                    SqlCommand cmd4;
                    string deleteperson = "DELETE FROM Person Where Id = @num";
                    string deleteadvisor = "DELETE FROM Advisor Where Id = @num";
                    string deletepradvisor = "DELETE FROM ProjectAdvisor Where AdvisorId = @num";
                    cmd1 = new SqlCommand(deleteadvisor, con);
                    cmd2 = new SqlCommand(deleteperson, con);
                    cmd4 = new SqlCommand(deletepradvisor, con);
                    cmd1.Parameters.AddWithValue("@num", id);
                    cmd2.Parameters.AddWithValue("@num", id);
                    cmd4.Parameters.AddWithValue("@num", id);
                    cmd4.ExecuteNonQuery();
                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record deleted succesfully");
                    ManageAdvisor f3 = new ManageAdvisor();
                    this.Close();
                    f3.Show();
                }

            }

        }
    }
}
