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
    public partial class ManageProjectAdvisors : Form
    {
        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";

        public ManageProjectAdvisors()
        {
            InitializeComponent();
            DisplayProjects();
        }

        public void DisplayProjects()
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            SqlCommand cmd1;
            string display = "SELECT Id AS [Project Number], Title AS [Project Title] FROM Project";
            cmd1 = new SqlCommand(display, con);
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd1;
            DataTable groupdt = new DataTable();
            sda.Fill(groupdt);
            dataGridView1.DataSource = groupdt;

            DataGridViewButtonColumn addButton;
            addButton = new DataGridViewButtonColumn();
            addButton.HeaderText = "Assign Advisors";
            addButton.Text = "Assign Advisors";
            addButton.UseColumnTextForButtonValue = true;
            addButton.Width = 100;
            dataGridView1.Columns.Add(addButton);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
            }

            DataGridViewButtonColumn viewButton;
            viewButton = new DataGridViewButtonColumn();
            viewButton.HeaderText = "View Advisors";
            viewButton.Text = "View Advisors";
            viewButton.UseColumnTextForButtonValue = true;
            viewButton.Width = 100;
            dataGridView1.Columns.Add(viewButton);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
            }
        }

        private void ManageProjectAdvisors_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();

            int currentRow = int.Parse(e.RowIndex.ToString());
            int currentColumnIndex = int.Parse(e.ColumnIndex.ToString());
            int prid = Convert.ToInt32(dataGridView1.Rows[currentRow].Cells[2].Value.ToString());
            if (currentColumnIndex == 0)
            {
                ManageProjectAdvisors_1 form = new ManageProjectAdvisors_1(prid);
                this.Close();
                form.Show();
            }

            if (currentColumnIndex == 1)
            {
                ManageProjectAdvisors_2 form1 = new ManageProjectAdvisors_2(prid);
                this.Close();
                form1.Show();
            }


        }

        private void Back_Click(object sender, EventArgs e)
        {
            Initial Form3 = new Initial();
            this.Close();
            Form3.Show();
        }
    }
}
