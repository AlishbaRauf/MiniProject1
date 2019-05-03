using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class AddProject : Form
    {

        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";
        public int flag;
        public AddProject()
        {
            InitializeComponent();
        }

        public AddProject(int i)
        {
            InitializeComponent();
            flag = i;
            UpdateProject();
        }

        public void UpdateProject()
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            SqlCommand cmd;
            string read = "SELECT Title,Description FROM Project WHERE Project.Id = @flag1";
            cmd = new SqlCommand(read, con);
            cmd.Parameters.AddWithValue("@flag1", flag);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable projecttable = new DataTable();
            adapter.Fill(projecttable);
            Title.Text = projecttable.Rows[0]["Title"].ToString();
            Description.Text = projecttable.Rows[0]["Description"].ToString();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            if (flag > 0)
            {
                bool j = myvalidations();
                if (j == true)
                {

                    SqlCommand cmd1;
                    string updateproject = "Update Project set Title=@Title,Description=@Description WHERE Project.Id= @flag1";
                    cmd1 = new SqlCommand(updateproject, con);
                    cmd1.Parameters.AddWithValue("@flag1", flag);
                    cmd1.Parameters.AddWithValue("@Title", Title.Text);

                    if (String.IsNullOrEmpty(Description.Text))
                    {
                        cmd1.Parameters.AddWithValue("@Description", DBNull.Value);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@Description", Description.Text);
                    }

                    cmd1.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Project Details Successfully Updated ");
                    ManageProject f3 = new ManageProject();
                    f3.Show();
                    this.Close();

                }
            }

            else
            {
                bool j = myvalidations();
                if (j == true)
                {

                    SqlCommand cmd1;
                    string insertpr = "INSERT INTO Project(Title,Description) values(@Title,@Description)";
                    cmd1 = new SqlCommand(insertpr, con);
                    cmd1.Parameters.AddWithValue("@Title", Title.Text);

                    if (String.IsNullOrEmpty(Description.Text))
                    {
                        cmd1.Parameters.AddWithValue("@Description", DBNull.Value);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@Description", Description.Text);
                    }
                    int i = cmd1.ExecuteNonQuery();
                    con.Close();
                    clearfields();
                    MessageBox.Show(i + " Row(s) Inserted ");

                }
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Initial form = new Initial();
            form.Show();
            this.Close();
        }

        public void clearfields()
        {
            Description.Clear();
            Title.Clear();
        }

        public bool myvalidations()
        {
            int i = 0;

            if (String.IsNullOrEmpty(Title.Text))
            {
                errorProvider1.SetError(Title, "Required Field");
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
        /*private void AddProject_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("succeeded");
            Initial init = Program.getInstance();
            init.Show();
        }*/
        private void AddProject_Load(object sender, EventArgs e)
        {

        }
    }
    
}
