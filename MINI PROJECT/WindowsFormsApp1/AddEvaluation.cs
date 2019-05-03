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
    public partial class AddEvaluation : Form
    {
        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";
        public int flag;

        public AddEvaluation()
        {
            InitializeComponent();
        }

        public AddEvaluation(int i)
        {
            InitializeComponent();
            flag = i;
            UpdateEvaluation();
        }

        public void UpdateEvaluation()
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            SqlCommand cmd;
            string read = "SELECT Name,TotalMarks,TotalWeightage FROM Evaluation WHERE Evaluation.Id = @flag1";
            cmd = new SqlCommand(read, con);
            cmd.Parameters.AddWithValue("@flag1", flag);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable evaluationtable = new DataTable();
            adapter.Fill(evaluationtable);
            Ev_Name.Text = evaluationtable.Rows[0]["Name"].ToString();
            Ev_TotalMarks.Text = evaluationtable.Rows[0]["TotalMarks"].ToString();
            Ev_TotalWeightage.Text = evaluationtable.Rows[0]["TotalWeightage"].ToString();
        }


        /*private void AddEvaluation_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("succeeded");
            Initial init = Program.getInstance();
            init.Show();
        }*/
        private void AddEvaluation_Load(object sender, EventArgs e)
        {

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
                    string updateevaluation = "Update Evaluation set  Name=@Name,TotalMarks=@TotalMarks,TotalWeightage=@TotalWeightage WHERE Evaluation.ID = @flag1";
                    cmd1 = new SqlCommand(updateevaluation, con);
                    cmd1.Parameters.AddWithValue("@flag1", flag);
                    cmd1.Parameters.AddWithValue("@Name", Ev_Name.Text);
                    cmd1.Parameters.AddWithValue("@TotalMarks", Convert.ToInt64(Ev_TotalMarks.Text));
                    cmd1.Parameters.AddWithValue("@TotalWeightage", Convert.ToInt64(Ev_TotalWeightage.Text));
                    cmd1.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Evaluation Details Successfully Updated ");
                    ManageEvaluation f3 = new ManageEvaluation();
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
                    string insertev = "INSERT INTO Evaluation(Name,TotalMarks,TotalWeightage) values(@Name,@TotalMarks,@TotalWeightage)";
                    cmd1 = new SqlCommand(insertev, con);
                    cmd1.Parameters.AddWithValue("@Name", Ev_Name.Text);
                    cmd1.Parameters.AddWithValue("@TotalMarks", Convert.ToInt64(Ev_TotalMarks.Text));
                    cmd1.Parameters.AddWithValue("@TotalWeightage", Convert.ToInt64(Ev_TotalWeightage.Text));
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
            Ev_Name.Clear();
            Ev_TotalMarks.Clear();
            Ev_TotalWeightage.Clear();
        }

        public bool myvalidations()
        {
            int i = 0;

            if (String.IsNullOrEmpty(Ev_Name.Text))
            {
                errorProvider1.SetError(Ev_Name, "Required Field");
                i = 1;
            }

            else
            {
                errorProvider1.Clear();
            }

            if (String.IsNullOrEmpty(Ev_TotalMarks.Text))
            {
                errorProvider2.SetError(Ev_TotalMarks, "Required Field");
                i = 1;
            }

            else
            {
                errorProvider2.Clear();
            }

            if (String.IsNullOrEmpty(Ev_TotalWeightage.Text))
            {
                errorProvider3.SetError(Ev_TotalWeightage, "Required Field");
                i = 1;
            }

            else
            {
                errorProvider3.Clear();
            }

            /*if (!String.IsNullOrEmpty(Ev_Name.Text) && !Regex.Match(Ev_Name.Text, "^[a-zA-Z][a-zA-Z ]+[a-zA-Z]*$").Success)
            {
                errorProvider4.SetError(Ev_Name, "Must contain alphabets or spaces only");
                i = 1;
            }

            else
            {
                errorProvider4.Clear();
            } */

            if (!String.IsNullOrEmpty(Ev_TotalMarks.Text) && !Regex.Match(Ev_TotalMarks.Text, @"^\d+$").Success)
            {
                errorProvider5.SetError(Ev_TotalMarks, "Must contain digits only");
                i = 1;
            }

            else
            {
                errorProvider5.Clear();
            }

            if (!String.IsNullOrEmpty(Ev_TotalWeightage.Text) && !Regex.Match(Ev_TotalWeightage.Text, @"^\d+$").Success)
            {
                errorProvider6.SetError(Ev_TotalWeightage, "Must contain digits only");
                i = 1;
            }

            else
            {
                errorProvider6.Clear();
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
        
}
