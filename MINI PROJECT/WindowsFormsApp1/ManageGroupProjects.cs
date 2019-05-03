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
    public partial class ManageGroupProjects : Form
    {
        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";

        public ManageGroupProjects()
        {
            InitializeComponent();
            DisplayGroups();
        }

        public void DisplayGroups()
        {
            
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            SqlCommand cmd1;
            string display = "SELECT Id AS [Group Number],COUNT(*) AS [Number of Students] FROM [Group] JOIN GroupStudent ON [Group].Id = GroupStudent.GroupId GROUP BY [Group].Id,Status HAVING Status = @val1 AND [Group].Id NOT IN (SELECT GroupId FROM GroupProject)";
            cmd1 = new SqlCommand(display, con);
            cmd1.Parameters.AddWithValue("@val1", 3);
            cmd1.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd1;
            DataTable groupdt = new DataTable();
            sda.Fill(groupdt);
            dataGridView1.DataSource = groupdt;

            DataGridViewButtonColumn assignButton;
            assignButton = new DataGridViewButtonColumn();
            assignButton.HeaderText = "Assign Project";
            assignButton.Text = "Assign Project";
            assignButton.UseColumnTextForButtonValue = true;
            assignButton.Width = 80;
            dataGridView1.Columns.Add(assignButton);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
            }

            string display1 = "SELECT GroupProject.GroupId AS [Group Number],Title AS [Project Title], Project.Id AS [Project No] FROM GroupProject JOIN Project ON GroupProject.ProjectId = Project.Id";
            SqlCommand cmd2 = new SqlCommand(display1, con);
            cmd2.ExecuteNonQuery();
            SqlDataAdapter sda1 = new SqlDataAdapter();
            sda1.SelectCommand = cmd2;
            DataTable projectdt = new DataTable();
            sda1.Fill(projectdt);
            dataGridView2.DataSource = projectdt;

            DataGridViewButtonColumn deleteButton;
            deleteButton = new DataGridViewButtonColumn();
            deleteButton.HeaderText = "Delete";
            deleteButton.Text = "Delete";
            deleteButton.UseColumnTextForButtonValue = true;
            deleteButton.Width = 80;
            dataGridView2.Columns.Add(deleteButton);
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                dataGridView2.Rows[i].Cells[0].ReadOnly = true;
            }
        }


        private void ManageGroupProjects_Load(object sender, EventArgs e)
        {

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
            if (currentColumnIndex == 0)
            {
                ManageGroupProject_1 f3 = new ManageGroupProject_1(gid);
                this.Close();
                f3.Show();
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
           
            int currentRow = int.Parse(e.RowIndex.ToString());
            int currentColumnIndex = int.Parse(e.ColumnIndex.ToString());
            int gid = Convert.ToInt32(dataGridView2.Rows[currentRow].Cells[1].Value.ToString());
            int projid = Convert.ToInt32(dataGridView2.Rows[currentRow].Cells[3].Value.ToString());
            if (currentColumnIndex == 0)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this item ??",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    string delete = "DELETE FROM GroupProject WHERE ProjectId = @val1 AND GroupId = @val2";
                    SqlCommand cmd1 = new SqlCommand(delete, con);
                    cmd1.Parameters.AddWithValue("@val1", projid);
                    cmd1.Parameters.AddWithValue("@val2", gid);
                    cmd1.ExecuteNonQuery();
                    MessageBox.Show("Record deleted succesfully");
                    ManageGroupProjects form = new ManageGroupProjects();
                    this.Close();
                    form.Show();
                }
            }

        }
    }
}
