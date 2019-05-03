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
    public partial class ManageStudentGroups : Form
    {
        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";

        public ManageStudentGroups()
        {
            InitializeComponent();
            DisplayGroups();
        }

        public void DisplayGroups()
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
          /*  dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh(); */
            SqlCommand cmd1;         
            string display = "SELECT Id AS [Group Number] FROM [Group]";
            cmd1 = new SqlCommand(display, con);
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd1;
            DataTable groupdt = new DataTable();
            sda.Fill(groupdt);
            dataGridView1.DataSource = groupdt;

            DataGridViewButtonColumn addButton;
            addButton = new DataGridViewButtonColumn();
            addButton.HeaderText = "Add Students";
            addButton.Text = "Add Students";
            addButton.UseColumnTextForButtonValue = true;
            addButton.Width = 80;
            dataGridView1.Columns.Add(addButton);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
            }

            DataGridViewButtonColumn viewButton;
            viewButton = new DataGridViewButtonColumn();
            viewButton.HeaderText = "View Students";
            viewButton.Text = "View Students";
            viewButton.UseColumnTextForButtonValue = true;
            viewButton.Width = 80;
            dataGridView1.Columns.Add(viewButton);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();

            int currentRow = int.Parse(e.RowIndex.ToString());
            int currentColumnIndex = int.Parse(e.ColumnIndex.ToString());
            int gid = Convert.ToInt32(dataGridView1.Rows[currentRow].Cells[2].Value.ToString());
            if (currentColumnIndex == 0)
            {
                string count = "SELECT COUNT(*), GroupId, Status FROM GroupStudent Group By Status, GroupId Having COUNT(*) = 4 AND Status = @statval AND GroupId = @gidval";
                SqlCommand cmd3 = new SqlCommand(count, con);

                string statusval = string.Format("SELECT Id FROM Lookup WHERE Value = '{0}'", "Active");
                SqlCommand cmd4 = new SqlCommand(statusval, con);
                int statid1 = (Int32)cmd4.ExecuteScalar();

                cmd3.Parameters.AddWithValue("@statval", statid1);
                cmd3.Parameters.AddWithValue("@gidval", gid);

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd3;
                DataTable countdt = new DataTable();
                sda.Fill(countdt);

                if (countdt.Rows.Count > 0)
                {
                    MessageBox.Show("Group already contains 4 active memebers");
                }

                else
                {
                    ManageGroups_Students f3 = new ManageGroups_Students(gid);
                    this.Close();
                    f3.Show();
                }
                   
            }

            if (currentColumnIndex == 1)
            {
                ManageStudentGroups1 f4 = new ManageStudentGroups1(gid);
                this.Close();
                f4.Show();
            }

        }


        private void AddGroups_Click(object sender, EventArgs e)
        {
           
        }

        private void ManageStudentGroups_Load(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AddGroups_Click_1(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            SqlCommand cmd1;
            string group = "INSERT INTO [Group](Created_On) values(@Created_On)";
            cmd1 = new SqlCommand(group, con);
            cmd1.Parameters.AddWithValue("@Created_On", DateTime.Today);
            cmd1.ExecuteNonQuery();
            con.Close();
            MessageBox.Show(" Group Created");
            ManageStudentGroups form = new ManageStudentGroups();
            this.Close();
            form.Show();

        }

        private void Back_Click(object sender, EventArgs e)
        {
            Initial form = new Initial();
            this.Close();
            form.Show();
        }
    }
}

