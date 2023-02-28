using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoursesPFE
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=dbcours;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;

        private void login_Load(object sender, EventArgs e)
        {
            picSingup.Visible = false;
            btnISing.Visible = false;
            txtPass.UseSystemPasswordChar = true;
            txtpassword.UseSystemPasswordChar = true;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            picSingup.Visible = true;
            btnISing.Visible = true;
            picLogin.Visible = false;
            btnILogin.Visible = false;
        }
        private void addprofile()
        {
            cmd.Connection = con;
            // insert image
            MemoryStream ms = new MemoryStream();
            bunifuPictureBox1.Image.Save(ms, bunifuPictureBox1.Image.RawFormat);
            byte[] img = ms.ToArray();

            con.Open();
            cmd.CommandText = "insert into profil values(@b,@c,@d,@e,@f,@g,@h,null)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@b", txtnom.Text);
            cmd.Parameters.AddWithValue("@c", txtpren.Text);
            cmd.Parameters.AddWithValue("@d", img);
            cmd.Parameters.AddWithValue("@e", txtemail.Text);
            cmd.Parameters.AddWithValue("@f", txtpassword.Text);
            cmd.Parameters.AddWithValue("@g", txttele.Text);
            cmd.Parameters.AddWithValue("@h", combocountry.SelectedItem);
            cmd.ExecuteNonQuery();
            con.Close();

            con.Open();
            cmd.CommandText = "select idProfile from profil where idProfile=(select max(idProfile) from profil)";
            var id = cmd.ExecuteScalar();
            con.Close();

            con.Open();
            cmd.CommandText = "insert into logins values(@j,@k,@l,@m,@n)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@j", id);
            cmd.Parameters.AddWithValue("@k", txtnom.Text);
            cmd.Parameters.AddWithValue("@l", txtemail.Text);
            cmd.Parameters.AddWithValue("@m", txtpassword.Text);
            cmd.Parameters.AddWithValue("@n", "visiteur");
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from profil";
            dr = cmd.ExecuteReader();
            bool test1 = true;
            while (dr.Read())
            {
                if (txtemail.Text.Equals(dr["email"].ToString()))
                {
                    test1 = false;
                }
            }
            dr.Close();
            con.Close();

            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            bool regextext = false;
            if (Regex.IsMatch(txtemail.Text, pattern))
            {
                regextext = true;
            }
            if (txtemail.Text!="" && txtpassword.Text != "")
            {
                test1 = true;
            }
            else
            {
                test1 = false;
            }
            if (regextext == false)
            {
                string str = "Email not correct";
                Form ff = new alert2(str, alert2.enumType.Error);
                ff.Show();
            }
            else
            {
                if(test1 == true)
                {
                    addprofile();
                }
                else
                {
                    string str = "Email already exists";
                    Form ff = new alert2(str, alert2.enumType.Error);
                    ff.Show();
                }
            }
        }

        private void btnISing_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnILogin_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select idLogin from logins where email=@a and passwords=@b";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@a", txtUser.Text);
            cmd.Parameters.AddWithValue("@b", txtPass.Text);
            var idLogin = cmd.ExecuteScalar();
            con.Close();

            con.Open();
            cmd.CommandText = "update profil set actif=1 where email=@a and passwords=@b";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@a", txtUser.Text);
            cmd.Parameters.AddWithValue("@b", txtPass.Text);
            cmd.ExecuteNonQuery();
            con.Close();

            con.Open();
            cmd.CommandText = "select * from logins";
            dr = cmd.ExecuteReader();

            bool test = true;
            while (dr.Read())
            {
                if (txtUser.Text.Equals(dr["email"].ToString()) && txtPass.Text.Equals(dr["passwords"].ToString()) && dr["role"].ToString() == "visiteur")
                {
                    test = true;
                    Form f1 = new Visiteur2((int)idLogin);
                    f1.Show();
                    this.Hide();
                    break;
                }
                else if (txtUser.Text.Equals(dr["email"].ToString()) && txtPass.Text.Equals(dr["passwords"].ToString()) && dr["role"].ToString() == "admin")
                {
                    test = true;
                    Form f = new admin2((int)idLogin);
                    f.Show();
                    this.Hide();
                    break;
                }
                else
                {
                    test = false;
                }
            }
            dr.Close();
            con.Close();
            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            if (Regex.IsMatch(txtUser.Text, pattern))
            {
                test = true;
            }
            if (test == false)
            {
                //MessageBox.Show("Your Password or Username are not correct");
                string str = "UserName or Password not correct";
                Form ff = new alert2(str, alert2.enumType.Error);
                ff.Show();
            }
        }

        private void bunifuPictureBox1_Click(object sender, EventArgs e)
        {
            //addimage
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "|*.png; *.jpg; *.jfif";
            op.ShowDialog();
            string cheminImage = op.FileName;
            if (cheminImage != "")
                bunifuPictureBox1.Image = Image.FromFile(cheminImage);
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            picSingup.Visible = false;
            btnISing.Visible = false;
            picLogin.Visible = true;
            btnILogin.Visible = true;
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            if(txtPass.UseSystemPasswordChar == true)
            {
                iconButton2.IconChar = IconChar.Eye;
                txtPass.UseSystemPasswordChar = false;
            }
            else
            {
                iconButton2.IconChar = IconChar.EyeSlash;
                txtPass.UseSystemPasswordChar = true;
            }
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            if (txtpassword.UseSystemPasswordChar == true)
            {
                iconButton3.IconChar = IconChar.Eye;
                txtpassword.UseSystemPasswordChar = false;
            }
            else
            {
                iconButton3.IconChar = IconChar.EyeSlash;
                txtpassword.UseSystemPasswordChar = true;
            }
        }

        private void txtnum_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
