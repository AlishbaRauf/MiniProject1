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
    public partial class ManageGroups_Students : Form
    {
        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";
        public int flag;

        public ManageGroups_Students()
        {
            InitializeComponent();
        }

        public ManageGroups_Students(int i)
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
            string display = "SELECT FirstName, LastName ,RegistrationNo, Contact FROM Student JOIN Person ON Student.Id = Person.Id WHERE Student.Id NOT IN (SELECT StudentId FROM GroupStudent WHERE GroupId = @val2)"; //AND Status = @val4 UNION SELECT StudentId FROM GroupStudent WHERE Status = @val1)";
            cmd1 = new SqlCommand(display, con);

            /*string status = string.Format("SELECT Id FROM Lookup WHERE Value = @val3");
            SqlCommand cmd3 = new SqlCommand(status, con);
            cmd3.Parameters.AddWithValue("@val3", "Active");
            int lookupid = (Int32)cmd3.ExecuteScalar();*/

            /*string status1 = string.Format("SELECT Id FROM Lookup WHERE Value = @val5");
            SqlCommand cmd4 = new SqlCommand(status1, con);
            cmd4.Parameters.AddWithValue("@val5", "InActive");
            int lookupid1 = (Int32)cmd4.ExecuteScalar(); */

            //cmd1.Parameters.AddWithValue("@val4", lookupid1);
            //cmd1.Parameters.AddWithValue("@val1", lookupid);
            cmd1.Parameters.AddWithValue("@val2", flag);
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd1;
            DataTable sdt = new DataTable();
            sda.Fill(sdt);
            dataGridView1.DataSource = sdt;

            DataGridViewButtonColumn addButton;
            addButton = new DataGridViewButtonColumn();
            addButton.HeaderText = "Add";
            addButton.Text = "Add";
            addButton.UseColumnTextForButtonValue = true;
            addButton.Width = 80;
            dataGridView1.Columns.Add(addButton);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[4].ReadOnly = true;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            int currentRow = int.Parse(e.RowIndex.ToString());
            int currentColumnIndex = int.Parse(e.ColumnIndex.ToString());
            string regno = dataGridView1.Rows[currentRow].Cells[3].Value.ToString();
            if (currentColumnIndex == 0)
            {
                string count = "SELECT COUNT(*), GroupId, Status FROM GroupStudent Group By Status, GroupId Having COUNT(*) = 4 AND Status = @statval AND GroupId = @gidval";
                SqlCommand cmd3 = new SqlCommand(count, con);

                string statusval = string.Format("SELECT Id FROM Lookup WHERE Value = '{0}'", "Active");
                SqlCommand cmd4 = new SqlCommand(statusval, con);
                int statid1 = (Int32)cmd4.ExecuteScalar();

                cmd3.Parameters.AddWithValue("@statval", statid1);
                cmd3.Parameters.AddWithValue("@gidval", flag);

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd3;
                DataTable countdt = new DataTable();
                sda.Fill(countdt);

                if (countdt.Rows.Count > 0)
                {
                    MessageBox.Show("Cannot add more than 4 active memebers");
                }

                else
                {
                    string groupst = "INSERT INTO GroupStudent(GroupId, StudentId, Status, AssignmentDate) values(@GroupId,@StudentId, @Status, @AssignmentDate)";
                    SqlCommand cmd = new SqlCommand(groupst, con);
                    cmd.Parameters.AddWithValue("@GroupId", flag);

                    string reg = string.Format("SELECT Id FROM Student WHERE RegistrationNo = '{0}'", regno);
                    SqlCommand cmd1 = new SqlCommand(reg, con);
                    int stid = (Int32)cmd1.ExecuteScalar();
                    cmd.Parameters.AddWithValue("@StudentId", stid);

                    string status = string.Format("SELECT Id FROM Lookup WHERE Value = '{0}'", "Active");
                    SqlCommand cmd2 = new SqlCommand(status, con);
                    int statid = (Int32)cmd2.ExecuteScalar();
                    cmd.Parameters.AddWithValue("@Status", statid);

                    string status1 = string.Format("SELECT Id FROM Lookup WHERE Value = '{0}'", "InActive");
                    SqlCommand cmd6 = new SqlCommand(status1, con);
                    int statid6 = (Int32)cmd6.ExecuteScalar();

                    string update = "UPDATE GroupStudent set Status = @val5 WHERE StudentId = @val6";
                    SqlCommand cmd5 = new SqlCommand(update, con);
                    cmd5.Parameters.AddWithValue("@val5", statid6);
                    cmd5.Parameters.AddWithValue("@val6", stid );
                    cmd5.ExecuteNonQuery();

                    

                    cmd.Parameters.AddWithValue("@AssignmentDate", DateTime.Today);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Added to Group");
                    ManageGroups_Students form = new ManageGroups_Students(flag);
                    this.Close();
                    form.Show();
                }   
                
            }
        }

        private void ManageGroups_Students_Load(object sender, EventArgs e)
        {

        }

        private void Back_Click(object sender, EventArgs e)
        {
            ManageStudentGroups form = new ManageStudentGroups();
            this.Close();
            form.Show();
        }
    }
}





