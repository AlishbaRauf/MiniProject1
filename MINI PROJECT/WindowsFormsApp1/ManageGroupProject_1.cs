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
    public partial class ManageGroupProject_1 : Form
    {
        public int flag;
        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";

        public ManageGroupProject_1()
        {
            InitializeComponent();
        }

        public ManageGroupProject_1(int i)
        {
            InitializeComponent();
            flag = i;
            DisplayProjects();
        }

        public void DisplayProjects()
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            SqlCommand cmd1;
            string display = "SELECT Id AS [Project No], Description, Title FROM Project WHERE Project.Id NOT IN (SELECT ProjectId FROM GroupProject)";
            cmd1 = new SqlCommand(display, con);
            cmd1.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd1;
            DataTable groupdt = new DataTable();
            sda.Fill(groupdt);
            dataGridView1.DataSource = groupdt;

            DataGridViewButtonColumn assignButton;
            assignButton = new DataGridViewButtonColumn();
            assignButton.HeaderText = "Assign";
            assignButton.Text = "Assign";
            assignButton.UseColumnTextForButtonValue = true;
            assignButton.Width = 80;
            dataGridView1.Columns.Add(assignButton);
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
            int prjid = Convert.ToInt32(dataGridView1.Rows[currentRow].Cells[1].Value.ToString());
            if (currentColumnIndex == 0)
            {
                string groupproject = "INSERT INTO GroupProject(ProjectId, GroupId, AssignmentDate) values(@ProjectId, @GroupId, @AssignmentDate)";
                SqlCommand cmd1 = new SqlCommand(groupproject, con);
                cmd1.Parameters.AddWithValue("@ProjectId", prjid);
                cmd1.Parameters.AddWithValue("@GroupId", flag);
                cmd1.Parameters.AddWithValue("@AssignmentDate", DateTime.Today);
                cmd1.ExecuteNonQuery();
                MessageBox.Show("Project Assigned to Group");
                ManageGroupProjects f3 = new ManageGroupProjects();
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

        private void ManageGroupProject_1_Load(object sender, EventArgs e)
        {

        }
    }
}
