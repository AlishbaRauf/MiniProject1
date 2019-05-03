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
    public partial class AddAdvisor : Form
    {
        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";
        public int flag;

        public AddAdvisor()
        {
            InitializeComponent();
            Gender.DropDownStyle = ComboBoxStyle.DropDownList;
            Designation.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public AddAdvisor(int i)
        {
            InitializeComponent();
            flag = i;
            UpdateAdvisor();
        }

        public void UpdateAdvisor()
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();         
            SqlCommand cmd;
            string read = "SELECT FirstName,LastName,Contact,Email,DateOfBirth,Gender,Designation,Salary FROM Person JOIN Advisor ON Person.Id = Advisor.Id WHERE Person.Id = @flag1";
            cmd = new SqlCommand(read, con);
            cmd.Parameters.AddWithValue("@flag1", flag);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable advisorTable = new DataTable();
            adapter.Fill(advisorTable);
            FirstName.Text = advisorTable.Rows[0]["FirstName"].ToString();
            LastName.Text = advisorTable.Rows[0]["LastName"].ToString();
            Contact.Text = advisorTable.Rows[0]["Contact"].ToString();
            Email.Text = advisorTable.Rows[0]["Email"].ToString();
            Salary.Text = advisorTable.Rows[0]["Salary"].ToString();
            if (!String.IsNullOrEmpty(advisorTable.Rows[0]["DateOfBirth"].ToString()))
            {
                DOB.Text = advisorTable.Rows[0].Field<DateTime>("DateOfBirth").ToString("yyyy-MM-dd");
            }
            else
            {
                DOB.Text = advisorTable.Rows[0]["DateOfBirth"].ToString();
            }

            string gen = string.Format("SELECT Value FROM Lookup WHERE Id = '{0}'", advisorTable.Rows[0]["Gender"]);
            SqlCommand cmd2 = new SqlCommand(gen, con);
            string gendervalue = (string)cmd2.ExecuteScalar();
            Gender.Text = gendervalue;
            string des = string.Format("SELECT Value FROM Lookup WHERE Id = '{0}'", advisorTable.Rows[0]["Designation"]);
            SqlCommand cmd3 = new SqlCommand(des, con);
            string desvalue = (string)cmd3.ExecuteScalar();
            Designation.Text = desvalue;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            if (flag > 0)
            {
                bool j = myvalidations();
                if (j == true)
                {

                    SqlCommand cmd1;
                    SqlCommand cmd3;
                    string updateperson = "Update Person set FirstName=@FirstName, LastName=@LastName, Contact=@Contact, Email=@Email, DateOfBirth=@DateOfBirth, Gender=@Gender Where Person.Id=@flag1";
                    string updateadvisor = "Update Advisor set Designation = @Designation, Salary = @Salary Where Advisor.Id = @flag2";
                    cmd1 = new SqlCommand(updateperson, con);
                    cmd3 = new SqlCommand(updateadvisor, con);
                    cmd1.Parameters.AddWithValue("@flag1", flag);
                    cmd3.Parameters.AddWithValue("@flag2", flag);
                    
                    string des = string.Format("SELECT Id FROM Lookup WHERE Value = '{0}'", Designation.Text);
                    SqlCommand cmd4 = new SqlCommand(des, con);
                    int desid = (Int32)cmd4.ExecuteScalar();
                    cmd3.Parameters.AddWithValue("@Designation", desid);

                    if (String.IsNullOrEmpty(Salary.Text))
                    {
                        cmd3.Parameters.AddWithValue("@Salary", DBNull.Value);
                    }
                    else
                    {
                        cmd3.Parameters.AddWithValue("@Salary", Salary.Text);
                    }

                    if (String.IsNullOrEmpty(FirstName.Text))
                    {
                        cmd1.Parameters.AddWithValue("@FirstName", DBNull.Value);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@FirstName", FirstName.Text);
                    }

                    if (String.IsNullOrEmpty(LastName.Text))
                    {
                        cmd1.Parameters.AddWithValue("@LastName", DBNull.Value);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@LastName", LastName.Text);
                    }

                    if (String.IsNullOrEmpty(Contact.Text))
                    {
                        cmd1.Parameters.AddWithValue("@Contact", DBNull.Value);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@Contact", Contact.Text);
                    }

                    cmd1.Parameters.AddWithValue("@Email", Email.Text);

                    if (String.IsNullOrEmpty(DOB.Text))
                    {
                        cmd1.Parameters.AddWithValue("@DateOfBirth", DBNull.Value);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(DOB.Text));
                    }

                    if (String.IsNullOrEmpty(Gender.Text))
                    {
                        cmd1.Parameters.AddWithValue("@Gender", DBNull.Value);
                    }
                    else
                    {
                        string gen = string.Format("SELECT Id FROM Lookup WHERE Value = '{0}'", Gender.Text);
                        SqlCommand cmd2 = new SqlCommand(gen, con);
                        int gid = (Int32)cmd2.ExecuteScalar();
                        cmd1.Parameters.AddWithValue("@Gender", gid);
                    }



                    cmd1.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Advisor Details Successfully Updated ");
                    ManageAdvisor f3 = new ManageAdvisor();
                    f3.Show();
                    this.Hide();

                }

            }

            else
            {
                bool j = myvalidations();
                if (j == true)
                {
                  
                    SqlCommand cmd;
                    SqlCommand cmd1;
                    string s = "INSERT INTO Person(FirstName,LastName,Contact,Email,DateOfBirth,Gender) values(@FirstName,@LastName,@Contact,@Email,@DateOfBirth,@Gender)";
                    string advisor = "INSERT INTO Advisor(Id,Designation, Salary) values(@Id,@Designation, @Salary)";
                    cmd = new SqlCommand(s, con);
                    cmd1 = new SqlCommand(advisor, con);

                    cmd.Parameters.AddWithValue("@FirstName", FirstName.Text);
                    if (String.IsNullOrEmpty(LastName.Text))
                    {
                        cmd.Parameters.AddWithValue("@LastName", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@LastName", LastName.Text);
                    }
                    if (String.IsNullOrEmpty(Contact.Text))
                    {
                        cmd.Parameters.AddWithValue("@Contact", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Contact", Contact.Text);
                    }
                    if (String.IsNullOrEmpty(Email.Text))
                    {
                        cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Email", Email.Text);
                    }

                    if (String.IsNullOrEmpty(DOB.Text))
                    {
                        cmd.Parameters.AddWithValue("@DateOfBirth", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(DOB.Text));
                    }

                    if (String.IsNullOrEmpty(Gender.Text))
                    {
                        cmd.Parameters.AddWithValue("@Gender", DBNull.Value);
                    }
                    else
                    {
                        string gen = string.Format("SELECT Id FROM Lookup WHERE Value = '{0}'", Gender.Text);
                        SqlCommand cmd2 = new SqlCommand(gen, con);
                        int gid = (Int32)cmd2.ExecuteScalar();
                        cmd.Parameters.AddWithValue("@Gender", gid);
                    }


                    int i = cmd.ExecuteNonQuery();

                    string personid = string.Format("SELECT max(Id) FROM Person");
                    SqlCommand cmd3 = new SqlCommand(personid, con);
                    int pid = (Int32)cmd3.ExecuteScalar();
                    cmd1.Parameters.AddWithValue("@Id", pid);

                    if (String.IsNullOrEmpty(Salary.Text))
                    {
                        cmd1.Parameters.AddWithValue("@Salary", DBNull.Value);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@Salary", Convert.ToInt64(Salary.Text));
                    }

            
                    if (String.IsNullOrEmpty(Designation.Text))
                    {
                        cmd1.Parameters.AddWithValue("@Designation", DBNull.Value);
                    }
                    else
                    
                    {
                        string des = string.Format("SELECT Id FROM Lookup WHERE Value = '{0}'", Designation.Text);
                        SqlCommand cmd4 = new SqlCommand(des, con);
                        int desid = (Int32)cmd4.ExecuteScalar();
                        cmd1.Parameters.AddWithValue("@Designation", desid);
                    }


                    cmd1.ExecuteNonQuery();
                    con.Close();
                    clearfields();
                    MessageBox.Show(i + " Row(s) Inserted ");
                }
            }
        }

        public bool myvalidations()
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            int i = 0;


            if (String.IsNullOrEmpty(Designation.Text))
            {
                errorProvider9.SetError(Designation, "Required Field");
                i = 1;
            }
            else
            {
                errorProvider9.Clear();
            }

            if (String.IsNullOrEmpty(FirstName.Text))
            {
                errorProvider1.SetError(FirstName, "Required Field");
                i = 1;
            }

            else
            {
                errorProvider1.Clear();
            }

            if (String.IsNullOrEmpty(Email.Text))
            {
                errorProvider2.SetError(Email, "Required Field");
                i = 1;
            }

            else
            {
                errorProvider2.Clear();
            }



            if (!String.IsNullOrEmpty(FirstName.Text) && !Regex.Match(FirstName.Text, "^[a-zA-Z][a-zA-Z ]+[a-zA-Z]*$").Success)
            {
               
                errorProvider3.SetError(FirstName, "Must contain alphabets or spaces only");
                i = 1;
            }

            else
            {
                errorProvider3.Clear();
            }

            if (!String.IsNullOrEmpty(LastName.Text) && !Regex.Match(LastName.Text, "^[a-zA-Z][a-zA-Z ]+[a-zA-Z]*$").Success)
            {
                errorProvider4.SetError(LastName, "Must contain alphabets or spaces only");
                i = 1;
            }

            else
            {
                errorProvider4.Clear();
            }

            if (!String.IsNullOrEmpty(Contact.Text) && !Regex.Match(Contact.Text, @"^((\+92)|(0092))-{0,1}\d{3}-{0,1}\d{7}$|^\d{11}$|^\d{4}-\d{7}$").Success)
            {
                errorProvider5.SetError(Contact, "Invalid phone number");
                i = 1;
            }

            else
            {
                errorProvider5.Clear();
            }


            string findemail = string.Format("SELECT Id FROM Person WHERE Email = @val");
            SqlCommand cmd1 = new SqlCommand(findemail, con);
            cmd1.Parameters.AddWithValue("@val", Email.Text);
            cmd1.ExecuteScalar();
            SqlDataAdapter adapter1 = new SqlDataAdapter(cmd1);
            DataTable findemailTable = new DataTable();
            adapter1.Fill(findemailTable);
            if (!String.IsNullOrEmpty(Email.Text) && !Regex.Match(Email.Text, @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$").Success || !String.IsNullOrEmpty(Email.Text) && findemailTable.Rows.Count > 0)
            {
                if (flag > 0)
                {
                    if (!String.IsNullOrEmpty(Email.Text) && findemailTable.Rows.Count > 0 && findemailTable.Rows[0]["Id"].ToString() != flag.ToString())
                    {
                        errorProvider6.SetError(Email, "Email already exists");
                        i = 1;
                    }

                    else if (!String.IsNullOrEmpty(Email.Text) && !Regex.Match(Email.Text, @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$").Success)
                    {
                        errorProvider6.SetError(Email, "Invalid email");
                        i = 1;
                    }

                }
                else
                {
                    if (!String.IsNullOrEmpty(Email.Text) && findemailTable.Rows.Count > 0)
                    {
                        errorProvider6.SetError(Email, "Email already exists");
                        i = 1;
                    }

                    else if (!String.IsNullOrEmpty(Email.Text) && !Regex.Match(Email.Text, @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$").Success)
                    {
                        errorProvider6.SetError(Email, "Invalid email");
                        i = 1;
                    }
                }
               
            }

            else
            {
                errorProvider6.Clear();
            }

            /* if (!String.IsNullOrEmpty(DOB.Text) && !Regex.Match(DOB.Text, "^([0-9]{4})-([0-9]{1,2})-([0-9]{1,2})").Success)
               {
                   errorProvider7.SetError(DOB, "Invalid DOB");
                   i = 1;
               }

               else
               {
                   errorProvider7.Clear();
               } */

            if (!String.IsNullOrEmpty(Salary.Text) && !Regex.Match(Salary.Text,@"^\d{1,18}$").Success)
            {
                errorProvider8.SetError(Salary, "Must contain numerics of length no more than 18 digits");
                i = 1;
            }

            else
            {
                errorProvider8.Clear();
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

      

        private void AddAdvisor_Load(object sender, EventArgs e)
        {

        }

        /*private void AddAdvisor_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("succeeded");
            Initial init = Program.getInstance();
            init.Show();
        }*/

        public void clearfields()
        {
            FirstName.Clear();
            LastName.Clear();
            Contact.Clear();
            Email.Clear();
            Contact.Clear();
            Salary.Clear();
            Designation.SelectedIndex = -1;
            Gender.SelectedIndex = -1;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Initial form = new Initial();
            this.Close();
            form.Show();
        }

        private void Cancel_Click_1(object sender, EventArgs e)
        {
            Initial form = new Initial();
            this.Close();
            form.Show();

        }
    }
}
