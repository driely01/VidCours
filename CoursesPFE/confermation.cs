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

namespace CoursesPFE
{
    public partial class confermation : Form
    {
        int idCourse;
        string msg;
        public confermation(int id, string message)
        {
            InitializeComponent();
            idCourse = id;
            msg = message;
        }
        //Connection
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=dbcours;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;

        public void deleteCourse()
        {
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete courses where idCourse=@a";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", idCourse);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception) { }
            finally { con.Close(); }
        }

        private void confermation_Load(object sender, EventArgs e)
        {
            LblMessage.Text = msg;
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            deleteCourse();
            this.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
