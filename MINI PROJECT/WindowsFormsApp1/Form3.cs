using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";
        public Form3()
        {
            InitializeComponent();
            DisplayStudents();

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
        }

        public void DisplayStudents()
        {
            SqlConnection con = new SqlConnection(conURL);
            /*try
            {
                con.Open();
                MessageBox.Show("Connection Open ! ");
                //con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            } */
            
            SqlCommand cmd1;
            string display = "SELECT FirstName,LastName,RegistrationNo,Contact,Email,DateOfBirth FROM Person JOIN Student ON Person.Id = Student.Id";
            cmd1 = new SqlCommand(display, con);
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd1;
            DataTable studentdt = new DataTable();
            sda.Fill(studentdt);
            dataGridView1.DataSource = studentdt;

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
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            int currentRow = int.Parse(e.RowIndex.ToString());
            int currentColumnIndex = int.Parse(e.ColumnIndex.ToString());
            string email = string.Format("SELECT Id FROM Person WHERE Email = '{0}'", dataGridView1.Rows[currentRow].Cells[6].Value.ToString());
            SqlCommand cmd3 = new SqlCommand(email, con);
            int id = (Int32)cmd3.ExecuteScalar();
            if (currentColumnIndex == 0)
            {
                Form2 f2 = new Form2(id);
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
                    string deletestudent = "DELETE FROM Student Where Id = @num";
                    string deletegroupst = "DELETE FROM GroupStudent Where StudentId = @num";
                    cmd1 = new SqlCommand(deletestudent, con);
                    cmd2 = new SqlCommand(deleteperson, con);
                    cmd4 = new SqlCommand(deletegroupst, con);
                    cmd1.Parameters.AddWithValue("@num", id);
                    cmd2.Parameters.AddWithValue("@num", id);
                    cmd4.Parameters.AddWithValue("@num", id);
                    cmd4.ExecuteNonQuery();
                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();                  
                    con.Close();
                    MessageBox.Show("Record deleted succesfully");
                    Form3 f3 = new Form3();
                    this.Close();
                    f3.Show();
                }

            }


        }
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Back_Click(object sender, EventArgs e)
        {
            Initial form = new Initial();
            form.Show();
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form3_Load_1(object sender, EventArgs e)
        {

        }

        private void Back_Click_1(object sender, EventArgs e)
        {
            Initial form = new Initial();
            form.Show();
            this.Close();

        }
    }

       
}


