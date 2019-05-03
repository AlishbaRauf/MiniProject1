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
    public partial class ManageProjectAdvisors_2 : Form
    {
        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";
        public int flag;

        public ManageProjectAdvisors_2()
        {
            InitializeComponent();
        }

        public ManageProjectAdvisors_2(int i)
        {
            InitializeComponent();
            flag = i;
            DisplayAdvisors();
        }

        public void DisplayAdvisors()
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            SqlCommand cmd1;
            string display = "SELECT FirstName, LastName ,Lookup.Value AS [Advisor Role], Email FROM ((Advisor JOIN ProjectAdvisor ON Advisor.Id = ProjectAdvisor.AdvisorId) JOIN Lookup ON ProjectAdvisor.AdvisorRole = Lookup.Id) JOIN Person ON Advisor.Id = Person.Id WHERE ProjectAdvisor.ProjectId = @val";
            cmd1 = new SqlCommand(display, con);
            cmd1.Parameters.AddWithValue("@val", flag);
            cmd1.ExecuteNonQuery();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd1;
            DataTable sdt = new DataTable();
            sda.Fill(sdt);
            dataGridView1.DataSource = sdt;

            DataGridViewButtonColumn delButton;
            delButton = new DataGridViewButtonColumn();
            delButton.HeaderText = "Delete ";
            delButton.Text = "Delete";
            delButton.UseColumnTextForButtonValue = true;
            delButton.Width = 80;
            dataGridView1.Columns.Add(delButton);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
            }
        }



        private void ManageProjectAdvisors_2_Load(object sender, EventArgs e)
        {

        }

        private void Back_Click(object sender, EventArgs e)
        {
            ManageProjectAdvisors form = new ManageProjectAdvisors();
            this.Close();
            form.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            int currentRow = int.Parse(e.RowIndex.ToString());
            int currentColumnIndex = int.Parse(e.ColumnIndex.ToString());
            string advemail = dataGridView1.Rows[currentRow].Cells[4].Value.ToString();
            if (currentColumnIndex == 0)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this item ??",
                                    "Confirm Delete!!",
                                    MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    string delete = "DELETE FROM ProjectAdvisor Where AdvisorId = @val1 AND ProjectId = @val2";
                    SqlCommand cmd = new SqlCommand(delete, con);

                    string email = string.Format("SELECT Id FROM Person WHERE Email = '{0}'", advemail);
                    SqlCommand cmd1 = new SqlCommand(email, con);
                    int advid = (Int32)cmd1.ExecuteScalar();

                    cmd.Parameters.AddWithValue("@val1", advid);
                    cmd.Parameters.AddWithValue("@val2", flag);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record deleted succesfully");
                    ManageProjectAdvisors_2 form = new ManageProjectAdvisors_2(flag);
                    this.Close();
                    form.Show();
                }
            }
        }
    }
}
