using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        String conURL = @"Data Source = DESKTOP-QK3I2UN\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True";

        public int flag;

        public Form2()
        {
            InitializeComponent();
            Pr_Gender.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public Form2(int i)
        {
            InitializeComponent();
            flag = i;
            UpdateStudent();
        }


        public void UpdateStudent()
        {
            SqlConnection con = new SqlConnection(conURL);
            con.Open();
            SqlCommand cmd;
            string read = "SELECT FirstName,LastName,Contact,Email,DateOfBirth,Gender,RegistrationNo FROM Person JOIN Student ON Person.Id = Student.Id WHERE Person.Id = @flag1";
            cmd = new SqlCommand(read, con);
            cmd.Parameters.AddWithValue("@flag1", flag);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable studentTable = new DataTable();
            adapter.Fill(studentTable);
            Pr_FirstName.Text = studentTable.Rows[0]["FirstName"].ToString();
            Pr_LastName.Text = studentTable.Rows[0]["LastName"].ToString();
            Pr_Contact.Text = studentTable.Rows[0]["Contact"].ToString();
            Pr_Email.Text = studentTable.Rows[0]["Email"].ToString();
            St_RegNo.Text = studentTable.Rows[0]["RegistrationNo"].ToString();
            if (!String.IsNullOrEmpty(studentTable.Rows[0]["DateOfBirth"].ToString()))
            {
                Pr_DOB.Text = studentTable.Rows[0].Field<DateTime>("DateOfBirth").ToString("yyyy-MM-dd");
            }
            else
            {
                Pr_DOB.Text = studentTable.Rows[0]["DateOfBirth"].ToString();
            }

            string gen = string.Format("SELECT Value FROM Lookup WHERE Id = '{0}'", studentTable.Rows[0]["Gender"]);
            SqlCommand cmd2 = new SqlCommand(gen, con);
            string gendervalue = (string)cmd2.ExecuteScalar();
            Pr_Gender.Text = gendervalue;
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
                    string updatestudent = "Update Student set RegistrationNo = @RegistrationNo WHERE Student.Id = @flag2";
                    cmd1 = new SqlCommand(updateperson, con);
                    cmd3 = new SqlCommand(updatestudent, con);
                    cmd3.Parameters.AddWithValue("@flag2", flag);
                    cmd3.Parameters.AddWithValue("@RegistrationNo", St_RegNo.Text);

                    cmd1.Parameters.AddWithValue("@flag1", flag);

                    if (String.IsNullOrEmpty(Pr_FirstName.Text))
                    {
                        cmd1.Parameters.AddWithValue("@FirstName", DBNull.Value);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@FirstName", Pr_FirstName.Text);
                    }

                    if (String.IsNullOrEmpty(Pr_LastName.Text))
                    {
                        cmd1.Parameters.AddWithValue("@LastName", DBNull.Value);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@LastName", Pr_LastName.Text);
                    }

                    if (String.IsNullOrEmpty(Pr_Contact.Text))
                    {
                        cmd1.Parameters.AddWithValue("@Contact", DBNull.Value);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@Contact", Pr_Contact.Text);
                    }

                    if (String.IsNullOrEmpty(Pr_Email.Text))
                    {
                        cmd1.Parameters.AddWithValue("@Email", DBNull.Value);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@Email", Pr_Email.Text);
                    }

                    if (String.IsNullOrEmpty(Pr_DOB.Text))
                    {
                        cmd1.Parameters.AddWithValue("@DateOfBirth", DBNull.Value);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(Pr_DOB.Text));
                    }

                    if (String.IsNullOrEmpty(Pr_Gender.Text))
                    {
                        cmd1.Parameters.AddWithValue("@Gender", DBNull.Value);
                    }
                    else
                    {
                        string gen = string.Format("SELECT Id FROM Lookup WHERE Value = '{0}'", Pr_Gender.Text);
                        SqlCommand cmd2 = new SqlCommand(gen, con);
                        int gid = (Int32)cmd2.ExecuteScalar();
                        cmd1.Parameters.AddWithValue("@Gender", gid);
                    }

                    cmd1.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Student Details Successfully Updated ");
                    Form3 f3 = new Form3();
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
                    string student = "INSERT INTO Student(Id,RegistrationNo) values(@Id,@RegistrationNo)";
                    cmd = new SqlCommand(s, con);
                    cmd1 = new SqlCommand(student, con);

                    cmd.Parameters.AddWithValue("@FirstName", Pr_FirstName.Text);
                    if (String.IsNullOrEmpty(Pr_LastName.Text))
                    {
                        cmd.Parameters.AddWithValue("@LastName", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@LastName", Pr_LastName.Text);
                    }
                    if (String.IsNullOrEmpty(Pr_Contact.Text))
                    {
                        cmd.Parameters.AddWithValue("@Contact", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Contact", Pr_Contact.Text);
                    }
                    if (String.IsNullOrEmpty(Pr_Email.Text))
                    {
                        cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Email", Pr_Email.Text);
                    }
                    if (String.IsNullOrEmpty(Pr_DOB.Text))
                    {
                        cmd.Parameters.AddWithValue("@DateOfBirth", DBNull.Value);
                    }
                    else

                    {
                        cmd.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(Pr_DOB.Text));

                    }
                    if (String.IsNullOrEmpty(Pr_Gender.Text))
                    {
                        cmd.Parameters.AddWithValue("@Gender", DBNull.Value);
                    }
                    else
                    {
                        string gen = string.Format("SELECT Id FROM Lookup WHERE Value = '{0}'", Pr_Gender.Text);
                        SqlCommand cmd2 = new SqlCommand(gen, con);
                        int gid = (Int32)cmd2.ExecuteScalar();
                        cmd.Parameters.AddWithValue("@Gender", gid);
                    }


                    int i = cmd.ExecuteNonQuery();
                    string personid = string.Format("SELECT max(Id) FROM Person");
                    SqlCommand cmd3 = new SqlCommand(personid, con);
                    int pid = (Int32)cmd3.ExecuteScalar();
                    cmd1.Parameters.AddWithValue("@Id", pid);
                    cmd1.Parameters.AddWithValue("@RegistrationNo", St_RegNo.Text);
                    cmd.CommandType = CommandType.Text;
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

            if (String.IsNullOrEmpty(St_RegNo.Text))
            {
                errorProvider9.SetError(St_RegNo, "Required Field");
                i = 1;
            }
            else
            {
                errorProvider9.Clear();
            }

            if (String.IsNullOrEmpty(Pr_FirstName.Text))
            {
                errorProvider1.SetError(Pr_FirstName, "Required Field");
                i = 1;
            }
            else
            {
                errorProvider1.Clear();
            }

            if (String.IsNullOrEmpty(Pr_Email.Text))
            {
                errorProvider2.SetError(Pr_Email, "Required Field");
                //MessageBox.Show("First Name Required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                i = 1;
            }

            else
            {
                errorProvider2.Clear();
            }

            if (!String.IsNullOrEmpty(Pr_FirstName.Text) && !Regex.Match(Pr_FirstName.Text, "^[a-zA-Z][a-zA-Z ]+[a-zA-Z]*$").Success)
            {
                errorProvider3.SetError(Pr_FirstName, "Must contain alphabets or spaces only");
                i = 1;
            }

            else
            {
                errorProvider3.Clear();
            }

            if (!String.IsNullOrEmpty(Pr_LastName.Text) && !Regex.Match(Pr_LastName.Text, "^[a-zA-Z][a-zA-Z ]+[a-zA-Z]*$").Success)
            {
                errorProvider4.SetError(Pr_LastName, "Must contain alphabets or spaces only");
            }

            else
            {
                errorProvider4.Clear();
            }

            if (!String.IsNullOrEmpty(Pr_Contact.Text) && !Regex.Match(Pr_Contact.Text, @"^((\+92)|(0092))-{0,1}\d{3}-{0,1}\d{7}$|^\d{11}$|^\d{4}-\d{7}$").Success)
            {
                errorProvider5.SetError(Pr_Contact, "Invalid phone number");
                i = 1;
            }

            else
            {
                errorProvider5.Clear();
            }


            string findemail = string.Format("SELECT Id FROM Person WHERE Email = @val");
            SqlCommand cmd1 = new SqlCommand(findemail, con);
            cmd1.Parameters.AddWithValue("@val", Pr_Email.Text);
            cmd1.ExecuteScalar();
            SqlDataAdapter adapter1 = new SqlDataAdapter(cmd1);
            DataTable findemailTable = new DataTable();
            adapter1.Fill(findemailTable);
            if (!String.IsNullOrEmpty(Pr_Email.Text) && !Regex.Match(Pr_Email.Text, @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$").Success || !String.IsNullOrEmpty(Pr_Email.Text) && findemailTable.Rows.Count > 0)
            {
                if (flag > 0)
                {
                    if (!String.IsNullOrEmpty(Pr_Email.Text) && findemailTable.Rows.Count > 0 && findemailTable.Rows[0]["Id"].ToString() != flag.ToString())
                    {
                        errorProvider6.SetError(Pr_Email, "Email already exists");
                        i = 1;
                    }

                    else if (!String.IsNullOrEmpty(Pr_Email.Text) && !Regex.Match(Pr_Email.Text, @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$").Success)
                    {
                        errorProvider6.SetError(Pr_Email, "Invalid email");
                        i = 1;
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(Pr_Email.Text) && findemailTable.Rows.Count > 0)
                    {
                        errorProvider6.SetError(Pr_Email, "Email already exists");
                        i = 1;
                    }

                    else if (!String.IsNullOrEmpty(Pr_Email.Text) && !Regex.Match(Pr_Email.Text, @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$").Success)
                    {
                        errorProvider6.SetError(Pr_Email, "Invalid email");
                        i = 1;
                    }
                }
                    
            }

            else
            {
                errorProvider6.Clear();
            }

            /*  if (!String.IsNullOrEmpty(Pr_DOB.Text) && !Regex.Match(Pr_DOB.Text, "^([0-9]{4})-([0-9]{1,2})-([0-9]{1,2})").Success)
                {
                    errorProvider7.SetError(Pr_DOB, "Invalid DOB");
                    i = 1;
                }

                else
                {
                    errorProvider7.Clear();
                }  */


            string findreg = string.Format("SELECT Id FROM Student WHERE RegistrationNo = @val");
            SqlCommand cmd = new SqlCommand(findreg, con);
            cmd.Parameters.AddWithValue("@val", St_RegNo.Text);
            cmd.ExecuteScalar();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable findregTable = new DataTable();
            adapter.Fill(findregTable);

            if (!String.IsNullOrEmpty(St_RegNo.Text) && findregTable.Rows.Count > 0 || !String.IsNullOrEmpty(St_RegNo.Text) && !Regex.Match(St_RegNo.Text, "^([0-9]{4})-([a-zA-Z]{2,3})-([0-9]{1,3})*$").Success)
            {
                if (flag > 0)
                {
                    if (!String.IsNullOrEmpty(St_RegNo.Text) && findregTable.Rows.Count > 0 && findregTable.Rows[0]["Id"].ToString() != flag.ToString())
                    {
                        errorProvider8.SetError(St_RegNo, "RegistrationNo already exists");
                        i = 1;
                    }

                    else if (!String.IsNullOrEmpty(St_RegNo.Text) && !Regex.Match(St_RegNo.Text, "^([0-9]{4})-([a-zA-Z]{2,3})-([0-9]{1,3})*$").Success)
                    {
                        errorProvider8.SetError(St_RegNo, "RegistrationNo already exists");
                        i = 1;
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(St_RegNo.Text) && findregTable.Rows.Count > 0)
                    {
                        errorProvider8.SetError(St_RegNo, "RegistrationNo already exists");
                        i = 1;
                    }

                    else if (!String.IsNullOrEmpty(St_RegNo.Text) && !Regex.Match(St_RegNo.Text, "^([0-9]{4})-([a-zA-Z]{2,3})-([0-9]{1,3})*$").Success)
                    {
                        errorProvider8.SetError(St_RegNo, "RegistrationNo already exists");
                        i = 1;
                    }
                }
               
              
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

        public void clearfields()
        {
            Pr_FirstName.Clear();
            Pr_LastName.Clear();
            Pr_Contact.Clear();
            Pr_Email.Clear();
            Pr_Contact.Clear();
            St_RegNo.Clear();
            Pr_Gender.SelectedIndex = -1;
        }

        private void Pr_Gender_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void St_RegNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void Pr_FirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Initial form = new Initial();
            this.Close();
            form.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void Pr_LastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        /*private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("succeeded");
            Initial init = Program.getInstance();
            init.Show();
        }*/

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
