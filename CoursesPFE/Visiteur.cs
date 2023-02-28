using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace CoursesPFE
{
    public partial class Visiteur : Form
    {
        //field
        private IconButton currentBtn;
        private Panel leftBorderBtn;

        //Constructure
        public Visiteur()
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
            public static Color color1 = Color.FromArgb(172, 126, 241);
            public static Color color2 = Color.FromArgb(249, 118, 176);
            public static Color color3 = Color.FromArgb(253, 138, 114);
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
                currentBtn.BackColor = Color.FromArgb(37, 36, 81);
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
                currentBtn.BackColor = Color.FromArgb(31, 30, 68);
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
        }

        private void vedioBtn_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color2);
            ParrentPage.SetPage(2);
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
        //Connection
        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=dbcours;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;

        //Methods
        private void affichVedio()
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select idElem,titreC,chemain,duree,name,extension from Element";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Load(dr);
            AffVedios.DataSource = dt;
            AffVedios.Columns[0].Visible = false;
            AffVedios.Columns[2].Visible = false;
            AffVedios.Columns[3].Visible = false;
            AffVedios.Columns[4].Visible = false;
            AffVedios.Columns[5].Visible = false;
            dr.Close();
            con.Close();
        }

        private void openFile(int id)
        {
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select chemain,name,extension from Element where idElem=@a";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@a", id);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                var name = dr["name"].ToString();
                var data = (byte[])dr["chemain"];
                var extn = dr["extension"].ToString();
                var newfaleName = name.Replace(extn, DateTime.Now.ToString("ddMMyyyyhhmmss")) + extn;
                File.WriteAllBytes(newfaleName, data);
                axWindowsMediaPlayer1.URL = newfaleName;
                //System.Diagnostics.Process.Start(newfaleName);
            }
            con.Close();
        }

        private void Visiteur_Load(object sender, EventArgs e)
        {
            affichVedio();
        }

        private void AffVedios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var selected = AffVedios.SelectedRows;
            foreach (var row in selected)
            {
                int id = (int)((DataGridViewRow)row).Cells[0].Value;
                openFile(id);
            }
            //bunifuHSlider1.Maximum = axWindowsMediaPlayer1.settings.playCount;
        }

        private void bunifuVScrollBar1_Scroll(object sender, Bunifu.UI.WinForms.BunifuVScrollBar.ScrollEventArgs e)
        {
            try
            {
                AffVedios.FirstDisplayedScrollingRowIndex = AffVedios.Rows[e.Value].Index;
            }
            catch (Exception)
            {
            }
        }

        private void AffVedios_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                bunifuVScrollBar1.Maximum = AffVedios.RowCount - 1;
            }
            catch (Exception)
            {

            }
        }

        private void AffVedios_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                bunifuVScrollBar1.Maximum = AffVedios.RowCount - 1;
            }
            catch (Exception)
            {

            }
        }

        private void AffVedios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }

        private void iconButton6_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void bunifuHSlider1_MouseUp(object sender, MouseEventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = bunifuHSlider1.Value;
        }

        private void bunifuHSlider2_Scroll(object sender, Utilities.BunifuSlider.BunifuHScrollBar.ScrollEventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = bunifuHSlider2.Value;
        }

        private void bunifuHSlider2_ValueChanged(object sender, Utilities.BunifuSlider.BunifuHScrollBar.ValueChangedEventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = bunifuHSlider2.Value;
        }

        private void bunifuHSlider1_Scroll(object sender, Utilities.BunifuSlider.BunifuHScrollBar.ScrollEventArgs e)
        {
            axWindowsMediaPlayer1.settings.playCount = bunifuHSlider1.Value;
        }

        private void bunifuHSlider1_ValueChanged(object sender, Utilities.BunifuSlider.BunifuHScrollBar.ValueChangedEventArgs e)
        {
            axWindowsMediaPlayer1.settings.playCount = bunifuHSlider1.Value;
        }
    }
}
