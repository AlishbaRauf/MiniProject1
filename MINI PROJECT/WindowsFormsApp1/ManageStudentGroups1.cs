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
    public partial class ManageStudentGroups1 : Form
    {
        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";
        public int flag;

        public ManageStudentGroups1()
        {
            InitializeComponent();
        }

        public ManageStudentGroups1(int i)
        {
            InitializeComponent();
            flag = i;
            DisplayStudents();
        }

        public void DisplayStudents()
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            /*dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh(); */
            SqlCommand cmd1;
            string display = "SELECT FirstName, LastName ,RegistrationNo, Contact, Lookup.Value AS Status FROM ((Student JOIN Person ON Student.Id = Person.Id) JOIN GroupStudent ON GroupStudent.StudentId = Student.Id) JOIN Lookup ON GroupStudent.Status = Lookup.Id WHERE GroupId = @gid";
            cmd1 = new SqlCommand(display, con);
            cmd1.Parameters.AddWithValue("@gid", flag);
            cmd1.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd1;
            DataTable sdt = new DataTable();
            sda.Fill(sdt);
            dataGridView1.DataSource = sdt;

            /*DataGridViewButtonColumn statButton;
            statButton = new DataGridViewButtonColumn();
            statButton.HeaderText = "Change Status";
            statButton.Text = "Change Status";
            statButton.UseColumnTextForButtonValue = true;
            statButton.Width = 80;
            dataGridView1.Columns.Add(statButton);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
            }*/
        }

        /*private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            int currentRow = int.Parse(e.RowIndex.ToString());
            int currentColumnIndex = int.Parse(e.ColumnIndex.ToString());
            string regno = dataGridView1.Rows[currentRow].Cells[3].Value.ToString();
            if (currentColumnIndex == 0)
            {
                string reg = string.Format("SELECT Id FROM Student WHERE RegistrationNo = '{0}'", regno);
                SqlCommand cmd1 = new SqlCommand(reg, con);
                int stdid = (Int32)cmd1.ExecuteScalar();

                string statvalue = string.Format("SELECT Status FROM GroupStudent WHERE StudentId = @val1 AND GroupId = @val2 ", stdid, flag);
                SqlCommand cmd2 = new SqlCommand(statvalue, con);
                cmd2.Parameters.AddWithValue("@val1", stdid);
                cmd2.Parameters.AddWithValue("@val2", flag);
                int statid = (Int32)cmd2.ExecuteScalar();


                string updategroupst = "Update GroupStudent set Status = @Status Where GroupStudent.StudentId = @flag2 AND GroupStudent.GroupId = @flag3";

                string status = string.Format("SELECT Id FROM Lookup WHERE Id <> @val3 AND Category = @val4");
                SqlCommand cmd3 = new SqlCommand(status, con);
                cmd3.Parameters.AddWithValue("@val3", statid);
                cmd3.Parameters.AddWithValue("@val4", "STATUS");
                int lookupid = (Int32)cmd3.ExecuteScalar();

                SqlCommand cmd4 = new SqlCommand(updategroupst, con);
                cmd4.Parameters.AddWithValue("@flag2", stdid);
                cmd4.Parameters.AddWithValue("@flag3", flag);
                cmd4.Parameters.AddWithValue("@Status", lookupid);
                cmd4.ExecuteNonQuery();


                ManageStudentGroups1 f3 = new ManageStudentGroups1(flag);
                this.Close();
                f3.Show();
            }
        } */

        private void ManageStudentGroups1_Load(object sender, EventArgs e)
        {

        }

        private void Back_Click(object sender, EventArgs e)
        {
            ManageStudentGroups f1 = new ManageStudentGroups();
            this.Close();
            f1.Show();
        }
    }
}
