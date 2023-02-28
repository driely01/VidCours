using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;
using System.Data.SqlClient;
using System.IO;

namespace CoursesPFE
{
    public partial class Admin1 : Form
    {
        //field
        private IconButton currentBtn;
        private Panel leftBorderBtn;

        //Constructure
        public Admin1()
        {
            InitializeComponent();
            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 60);
            panelMenu.Controls.Add(leftBorderBtn);
            //Form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            //this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }

        //Structs
        private struct RGBColors
        {
            public static Color color1 = Color.FromArgb(44, 212, 217);
            public static Color color2 = Color.FromArgb(25, 58, 193);
            public static Color color3 = Color.FromArgb(37, 177, 181);
            public static Color color4 = Color.FromArgb(95, 77, 221);
            public static Color color5 = Color.FromArgb(249, 88, 155);
            public static Color color6 = Color.FromArgb(24, 161, 251);
        }

        //Methods
        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();
                //Button
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.FromArgb(25, 58, 193);
                currentBtn.ForeColor = color;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;

                //Left Border Color
                leftBorderBtn.BackColor = color;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();

                //Icon Current Child Form
                iconCurrentChild.IconChar = currentBtn.IconChar;
                iconCurrentChild.IconColor = color;
            }
        }

        //Disable
        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(21, 7, 115);
                currentBtn.ForeColor = Color.Gainsboro;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.Gainsboro;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        //Reset
        private void Reset()
        {
            DisableButton();
            leftBorderBtn.Visible = false;
            iconCurrentChild.IconChar = IconChar.Home;
            iconCurrentChild.IconColor = Color.MediumPurple;
            lblChildForm.Text = "Home";
        }

        private void CourseBtn_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            ParrentPage.SetPage(1);
            lblChildForm.Text = tabPage2.Text;
        }

        private void vedioBtn_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color2);
            ParrentPage.SetPage(2);
            lblChildForm.Text = tabPage3.Text;
        }

        private void profileBtn_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color3);
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color4);
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color5);
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            Reset();
            ParrentPage.SetPage(0);
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            Reset();
            ParrentPage.SetPage(0);
        }

        //Drag Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnMax_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Maximized;
            else
                WindowState = FormWindowState.Normal;
        }

        private void BtnMin_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //addimage
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "|*.png; *.jpg; *.jfif";
            op.ShowDialog();
            string cheminImage = op.FileName;
            if (cheminImage != "")
                pictureBox1.Image = Image.FromFile(cheminImage);
        }
        //Connection
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=dbcours;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;

        //Fill dropDowList with Courses
        private void Remplissage()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from courses", @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=dbcours;Integrated Security=True");
            DataTable dt1 = new DataTable();
            da.Fill(dt1);
            drpIDCourse.DisplayMember = "langage";
            drpIDCourse.ValueMember = "idCourse";
            drpIDCourse.DataSource = dt1;
        }

        //affichage
        private void affichCourse()
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from courses";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Load(dr);
            AffCourse.DataSource = dt;
            dr.Close();
            con.Close();
        }

        private void affichVedio()
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select idElem,e.idCourse,langage,prof,titreC,chemain,duree,name,extension from Element e join courses c on e.idCourse=c.idCourse";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Load(dr);
            dtgElement.DataSource = dt;
            dtgElement.Columns[5].Visible = false;
            dtgElement.Columns[6].Visible = false;
            dr.Close();
            con.Close();
        }
        private void Admin_Load(object sender, EventArgs e)
        {
            affichCourse();
            Remplissage();
            affichVedio();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                //insert image
                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                byte[] img = ms.ToArray();
                cmd.Connection = con;
                cmd.CommandText = "insert into courses values(@a,@b,@c,@d,@g,@e,@f)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", txtID.Text);
                cmd.Parameters.AddWithValue("@b", txtDescrip.Text);
                cmd.Parameters.AddWithValue("@c", img);
                cmd.Parameters.AddWithValue("@d", txtLangage.Text);
                cmd.Parameters.AddWithValue("@g", txtProf.Text);
                cmd.Parameters.AddWithValue("@e", DateTime.Now);
                cmd.Parameters.AddWithValue("@f", txtNum.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                affichCourse();
                Remplissage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { con.Close(); }
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "delete courses where idCourse=@a";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@a", txtID.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            affichCourse();
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                //insert image
                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                byte[] img = ms.ToArray();
                cmd.Connection = con;
                cmd.CommandText = "update courses set descriptions=@b,photos=@c,langage=@d,prof=@g,numElem=@e where @a=idCourse";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", txtID.Text);
                cmd.Parameters.AddWithValue("@b", txtDescrip.Text);
                cmd.Parameters.AddWithValue("@c", img);
                cmd.Parameters.AddWithValue("@d", txtLangage.Text);
                cmd.Parameters.AddWithValue("@g", txtProf.Text);
                cmd.Parameters.AddWithValue("@e", txtNum.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                affichCourse();
                Remplissage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { con.Close(); }
        }

        private void bunifuButton5_Click(object sender, EventArgs e)
        {
            try
            {

                //Stream stream = File.OpenRead(txtLink.Text);
                //byte[] buffer = new byte[stream.Length];
                //stream.Read(buffer, 0, buffer.Length);

                string extn = new FileInfo(txtLink.Text).Extension;
                string fullname = new FileInfo(txtLink.Text).FullName;
                string name = new FileInfo(txtLink.Text).Name;

                cmd.Connection = con;
                con.Open();

                cmd.CommandText = "insert into Element(idElem,idCourse,titreC,chemain,data,duree,name,extension) values(@a,@b,@c,null,@d,null,@f,@g)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", txtIDVedio.Text);
                cmd.Parameters.AddWithValue("@b", drpIDCourse.SelectedValue);
                cmd.Parameters.AddWithValue("@c", txtTitleCourse.Text);
                cmd.Parameters.AddWithValue("@d", fullname);
                //cmd.Parameters.AddWithValue("@e", null);
                cmd.Parameters.AddWithValue("@f", name);
                cmd.Parameters.AddWithValue("@g", extn);
                cmd.ExecuteNonQuery();

                con.Close();
                affichVedio();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { con.Close(); }
        }

        private void txtLink_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.ShowDialog();
            txtLink.Text = op.FileName;
        }

        private void bunifuButton7_Click(object sender, EventArgs e)
        {
            try
            {
                Stream stream = File.OpenRead(txtLink.Text);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                string extn = new FileInfo(txtLink.Text).Extension;
                string name = new FileInfo(txtLink.Text).Name;

                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "update Element set idCourse=@b,titreC=@c,chemain=@d,duree=null,name=@f,extension=@g where idElem=@a";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", txtIDVedio.Text);
                cmd.Parameters.AddWithValue("@b", drpIDCourse.SelectedValue);
                cmd.Parameters.AddWithValue("@c", txtTitleCourse.Text);
                cmd.Parameters.AddWithValue("@d", buffer);
                cmd.Parameters.AddWithValue("@f", name);
                cmd.Parameters.AddWithValue("@g", extn);
                cmd.ExecuteNonQuery();
                con.Close();
                affichVedio();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { con.Close(); }
        }

        private void bunifuButton6_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete Element where idElem=@a";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", txtIDVedio.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                affichVedio();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { con.Close(); }
        }

        private void txtLangage_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuVScrollBar1_Scroll(object sender, Bunifu.UI.WinForms.BunifuVScrollBar.ScrollEventArgs e)
        {
            try
            {
                dtgElement.FirstDisplayedScrollingRowIndex = dtgElement.Rows[e.Value].Index;
            }
            catch (Exception)
            {
            }
        }

        private void dtgElement_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                bunifuVScrollBar1.Maximum = dtgElement.RowCount - 1;   
            }
            catch (Exception)
            {

            }
        }

        private void dtgElement_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                bunifuVScrollBar1.Maximum = dtgElement.RowCount - 1;
            }
            catch (Exception)
            {

            }
        }

        private void bunifuVScrollBar2_Scroll(object sender, Bunifu.UI.WinForms.BunifuVScrollBar.ScrollEventArgs e)
        {
            try
            {
                AffCourse.FirstDisplayedScrollingRowIndex = AffCourse.Rows[e.Value].Index;
            }
            catch (Exception)
            {
            }
        }

        private void AffCourse_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                bunifuVScrollBar1.Maximum = AffCourse.RowCount - 1;
            }
            catch (Exception)
            {

            }
        }

        private void AffCourse_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                bunifuVScrollBar1.Maximum = AffCourse.RowCount - 1;
            }
            catch (Exception)
            {

            }
        }
        public void Alert(string msg)
        {
            Form frm = new alert(msg);
            frm.Show();
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            Alert("user name or password are not correct");
        }

        private void panelTitleBar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtLink_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
