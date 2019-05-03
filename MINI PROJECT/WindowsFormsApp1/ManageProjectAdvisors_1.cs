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
    public partial class ManageProjectAdvisors_1 : Form
    {

        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";
        public int flag;

        public ManageProjectAdvisors_1()
        {
            InitializeComponent();
        }

        public ManageProjectAdvisors_1(int i)
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
            string display = "SELECT FirstName, LastName ,Lookup.Value AS [Designation], Email FROM (Advisor JOIN Person ON Advisor.Id = Person.Id)  JOIN Lookup ON Advisor.Designation = Lookup.Id WHERE Advisor.Id NOT IN (SELECT AdvisorId FROM ProjectAdvisor WHERE ProjectId = @val)";
            cmd1 = new SqlCommand(display, con);

            cmd1.Parameters.AddWithValue("@val", flag);
            cmd1.ExecuteNonQuery();
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
                dataGridView1.Rows[i].Cells[0].ReadOnly = true;
            }
        }

        private void ManageProjectAdvisors_1_Load(object sender, EventArgs e)
        {

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
                bool j = myvalidations();
                if (j == true)
                {
                    if (advisor_role.Text == "Main Advisor")
                    {
                        string display1 = "SELECT * FROM ProjectAdvisor WHERE AdvisorRole = @val1 AND ProjectId = @val2";
                        SqlCommand cmd3 = new SqlCommand(display1, con);

                        string role = string.Format("SELECT Id FROM Lookup WHERE Value = '{0}'", "Main Advisor");
                        SqlCommand cmd4 = new SqlCommand(role, con);
                        int roleid1 = (Int32)cmd4.ExecuteScalar();
                        cmd3.Parameters.AddWithValue("@val1", roleid1);

                        cmd3.Parameters.AddWithValue("@val2", flag);
                        cmd3.ExecuteNonQuery();
                        SqlDataAdapter sda1 = new SqlDataAdapter();
                        sda1.SelectCommand = cmd3;
                        DataTable roledt = new DataTable();
                        sda1.Fill(roledt);
                        if (roledt.Rows.Count > 0)
                        {
                            MessageBox.Show("Project can not have more than one main advisor");
                        }
                        else
                        {
                            string groupst = "INSERT INTO ProjectAdvisor(AdvisorId, ProjectId, AdvisorRole, AssignmentDate) values(@AdvisorId, @ProjectId, @AdvisorRole, @AssignmentDate)";
                            SqlCommand cmd = new SqlCommand(groupst, con);
                            cmd.Parameters.AddWithValue("@ProjectId", flag);

                            string email = string.Format("SELECT Id FROM Person WHERE Email = '{0}'", advemail);
                            SqlCommand cmd1 = new SqlCommand(email, con);
                            int advid = (Int32)cmd1.ExecuteScalar();
                            cmd.Parameters.AddWithValue("@AdvisorId", advid);

                            string status = string.Format("SELECT Id FROM Lookup WHERE Value = '{0}'", advisor_role.Text);
                            SqlCommand cmd2 = new SqlCommand(status, con);
                            int roleid = (Int32)cmd2.ExecuteScalar();
                            cmd.Parameters.AddWithValue("@AdvisorRole", roleid);

                            cmd.Parameters.AddWithValue("@AssignmentDate", DateTime.Today);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Advisor Assigned to Project");
                            ManageProjectAdvisors_1 form = new ManageProjectAdvisors_1(flag);
                            this.Close();
                            form.Show();
                        }
                    }

                    else

                    {
                        string groupst = "INSERT INTO ProjectAdvisor(AdvisorId, ProjectId, AdvisorRole, AssignmentDate) values(@AdvisorId, @ProjectId, @AdvisorRole, @AssignmentDate)";
                        SqlCommand cmd = new SqlCommand(groupst, con);
                        cmd.Parameters.AddWithValue("@ProjectId", flag);

                        string email = string.Format("SELECT Id FROM Person WHERE Email = '{0}'", advemail);
                        SqlCommand cmd1 = new SqlCommand(email, con);
                        int advid = (Int32)cmd1.ExecuteScalar();
                        cmd.Parameters.AddWithValue("@AdvisorId", advid);

                        string status = string.Format("SELECT Id FROM Lookup WHERE Value = '{0}'", advisor_role.Text);
                        SqlCommand cmd2 = new SqlCommand(status, con);
                        int roleid = (Int32)cmd2.ExecuteScalar();
                        cmd.Parameters.AddWithValue("@AdvisorRole", roleid);

                        cmd.Parameters.AddWithValue("@AssignmentDate", DateTime.Today);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Advisor Assigned to Project");
                        ManageProjectAdvisors_1 form = new ManageProjectAdvisors_1(flag);
                        this.Close();
                        form.Show();

                    }

                }

            }
        }

        public bool myvalidations()
        {
            int i = 0;


            if (String.IsNullOrEmpty(advisor_role.Text))
            {
                errorProvider1.SetError(advisor_role, "Please select an advsior role");
                i = 1;
            }
            else
            {
                errorProvider1.Clear();
            }

            if (i == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Back_Click(object sender, EventArgs e)
        {
            ManageProjectAdvisors form1 = new ManageProjectAdvisors();
            this.Close();
            form1.Show();
        }
    }
}
