using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.Dataviz.WinForms;
using Bunifu.Framework.UI;
using Bunifu.UI.WinForm.BunifuShadowPanel;
using System.Runtime.InteropServices;

namespace CoursesPFE
{
    public partial class Visiteur2 : Form
    {
        int idlog;
        public Visiteur2(int id)
        {
            InitializeComponent();
            cmd.Connection = con;
            idlog = id;
            //Form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
        }

        //Drag Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void moveImageBox(object sender)
        {
            Guna2Button b = (Guna2Button)sender;
            pictureBox1.Location = new Point(b.Location.X + 27, b.Location.Y - 25);
            pictureBox1.SendToBack();
        }
        private void guna2Button1_CheckedChanged(object sender, EventArgs e)
        {
            moveImageBox(sender);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.pause();
            Application.Exit();
            con.Open();
            cmd.CommandText = "update profil set actif=0 where idProfile=@a";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@a", idlog);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Maximized;
            else
                WindowState = FormWindowState.Normal;
        }

        private void guna2Button10_Click(object sender, EventArgs e)
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
            AffVideos.DataSource = dt;
            AffVideos.Columns[0].Visible = false;
            AffVideos.Columns[2].Visible = false;
            AffVideos.Columns[3].Visible = false;
            AffVideos.Columns[4].Visible = false;
            AffVideos.Columns[5].Visible = false;
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
                player.URL = newfaleName;
                //System.Diagnostics.Process.Start(newfaleName);
            }
            con.Close();
        }

        private void affichCourse()
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from courses";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
            //dataGridView1.Columns[2].Visible = false;
            dr.Close();
            con.Close();
        }

        private void affichageActif()
        {
            con.Open();
            cmd.CommandText = "select count(actif) from profil where actif=1";
            string actif = cmd.ExecuteScalar().ToString();
            Numactif.Text = actif;
            con.Close();
        }

        private void AffichageProfiles()
        {
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from profil where idProfile="+idlog;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var data = (Byte[])(dr[3]);
                var stream = new MemoryStream(data);
                pictureBox4.Image = Image.FromStream(stream);
                picdashboard.Image = Image.FromStream(stream);
                FullName.Text = dr[1].ToString() + " " + dr[2].ToString();
                lblname.Text = dr[1].ToString() + " " + dr[2].ToString();
                firstnamelbl.Text = dr[1].ToString();
                lastnamelbl.Text = dr[2].ToString();
                lblemail.Text = dr[4].ToString();
                lblpassword.Text = dr[5].ToString();
                lbltele.Text = "+212 - " + dr[6].ToString();
                label10.Text = dr[6].ToString();
                lblcountry.Text = dr[7].ToString();
            }
            con.Close();
        }
        void buttonaffich()
        {
            cmd.CommandText = "select role from logins where idLogin=@a";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@a", idlog);
            con.Open();
            
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr["role"].ToString() == "visiteur")
                {
                    guna2Button8.Visible = false;
                    guna2Button5.Visible = true;
                }else if (dr["role"].ToString() == "admin")
                {
                    guna2Button8.Visible = true;
                    guna2Button5.Visible = false;
                }
            }
            dr.Close();
            con.Close();
        }
        string idcourse;
        private void Visiteur2_Load(object sender, EventArgs e)
        {
            buttonaffich();
            panel22.Visible = false;
            panel23.Visible = false;
            affichCourse();
            affichVedio();
            AffichageProfiles();
            afficherVideoI();
            affichageActif();
            label2.Text = dataGridView1.Rows[0].Cells[1].Value.ToString();
            label1.Text = dataGridView1.Rows[1].Cells[3].Value.ToString();
            for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
            {
                Panel p = addlabel1(i);
                flowLayoutPanel1.Controls.Add(p);
            }
            for (int i = 0; i <= affvediosI.Rows.Count - 1; i++)
            {
                Panel p = addPanelVideos(i);
                flowLayoutPanel2.Controls.Add(p);
            }
        }
         Label addlabel(int i)
         {
            Label l = new Label();
            l.Name = "label" + i.ToString();
            l.Text = "label" + i.ToString();
            l.ForeColor = Color.White;
            l.BackColor = Color.Green;
            l.Font = new Font("Serif", 24, FontStyle.Bold);
            l.Width = 170;
            l.Height = 80;
            l.TextAlign = ContentAlignment.MiddleCenter;
            l.Margin = new Padding(5);

            return l;
        }
        Panel addPanelVideos(int i)
        {
            //title Course
            Label l11 = new Label();
            l11.AutoSize = true;
            l11.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            l11.ForeColor = System.Drawing.Color.White;
            l11.Location = new System.Drawing.Point(116, 15);
            l11.Name = "label11";
            l11.Size = new System.Drawing.Size(67, 19);
            l11.TabIndex = 0;
            l11.Text = affvediosI.Rows[i].Cells[1].Value.ToString(); ;

            //header
            Panel p15 = new Panel();
            p15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            p15.Controls.Add(l11);
            p15.Dock = System.Windows.Forms.DockStyle.Top;
            p15.Location = new System.Drawing.Point(0, 0);
            p15.Name = "panel15";
            p15.Size = new System.Drawing.Size(299, 49);
            p15.TabIndex = 0;

            //videos number
            Label l12 = new Label();
            l12.Dock = System.Windows.Forms.DockStyle.Top;
            l12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            l12.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            l12.ForeColor = System.Drawing.Color.Black;
            l12.Location = new System.Drawing.Point(0, 0);
            l12.Name = "label12";
            l12.Size = new System.Drawing.Size(61, 63);
            l12.TabIndex = 6;
            l12.Text = "V"+ i.ToString();
            l12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            //favorite
            PictureBox pic5 = new PictureBox();
            pic5.Dock = System.Windows.Forms.DockStyle.Fill;
            pic5.Image = pictureBox6.Image;
            pic5.Location = new System.Drawing.Point(0, 63);
            pic5.Name = "pictureBox5";
            pic5.Size = new System.Drawing.Size(61, 41);
            pic5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            pic5.TabIndex = 7;
            pic5.TabStop = false;

            //Left panel
            Panel p18 = new Panel();
            p18.Controls.Add(pic5);
            p18.Controls.Add(l12);
            p18.BackColor = System.Drawing.Color.Gainsboro;
            p18.Dock = System.Windows.Forms.DockStyle.Left;
            p18.Location = new System.Drawing.Point(0, 49);
            p18.Name = "panel18";
            p18.Size = new System.Drawing.Size(61, 104);
            p18.TabIndex = 2;

            //title vedios
            Label lm = new Label();
            lm.Dock = System.Windows.Forms.DockStyle.Fill;
            lm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lm.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lm.ForeColor = System.Drawing.Color.Black;
            lm.Location = new System.Drawing.Point(61, 49);
            lm.Name = "LblMessg";
            lm.Size = new System.Drawing.Size(238, 104);
            lm.TabIndex = 5;
            lm.Text = affvediosI.Rows[i].Cells[2].Value.ToString();
            lm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            //container
            Panel p14 = new Panel();
            p14.BackColor = System.Drawing.Color.White;
            p14.Controls.Add(lm);
            p14.Controls.Add(p18);
            p14.Controls.Add(p15);
            p14.Location = new System.Drawing.Point(0, 0);
            p14.Name = "panel14";
            p14.Size = new System.Drawing.Size(299, 153);
            p14.TabIndex = 30;

            //shadow
            Panel p = new Panel();
            p.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            p.Controls.Add(p14);
            p.Location = new System.Drawing.Point(354, 3);
            p.Name = "panel16";
            p.Size = new System.Drawing.Size(298, 157);
            p.TabIndex = 1;

            return p;
        }

        Panel addlabel1(int i)
        {
            Label li = new Label();
            li.AutoSize = true;
            li.Font = new System.Drawing.Font("Century Gothic", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            li.ForeColor = System.Drawing.Color.White;
            li.Location = new System.Drawing.Point(96, 15);
            li.Name = "label" + i.ToString();
            li.Size = new System.Drawing.Size(63, 25);
            li.TabIndex = 0;
            li.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
            li.Visible = false;
            idcourse = li.Text;

            //label description
            Label l2 = new Label();
            l2.Dock = System.Windows.Forms.DockStyle.Fill;
            l2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            l2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            l2.ForeColor = System.Drawing.Color.White;
            l2.Location = new System.Drawing.Point(17, 0);
            l2.Name = "label2";
            l2.Size = new System.Drawing.Size(217, 143);
            l2.TabIndex = 3;
            l2.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();

            //panel left space 
            Panel p11 = new Panel();
            p11.Dock = System.Windows.Forms.DockStyle.Left;
            p11.Location = new System.Drawing.Point(0, 0);
            p11.Name = "panel11";
            p11.Size = new System.Drawing.Size(17, 143);
            p11.TabIndex = 4;

            //button view courses
            Button b1 = new Button();
            b1.BackColor = System.Drawing.Color.DarkCyan;
            b1.Cursor = System.Windows.Forms.Cursors.Hand;
            b1.Dock = System.Windows.Forms.DockStyle.Bottom;
            b1.FlatAppearance.BorderSize = 0;
            b1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            b1.Font = new System.Drawing.Font("Century Gothic", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Regular))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            b1.ForeColor = System.Drawing.Color.White;
            b1.Location = new System.Drawing.Point(0, 143);
            b1.Name = idcourse;
            b1.Size = new System.Drawing.Size(234, 35);
            b1.TabIndex = 5;
            b1.Text = "View Course";
            b1.UseVisualStyleBackColor = false;
            b1.Click += new System.EventHandler(this.button1_Click);
            
            //if (b1.Name[i].ToString() == li.Name[i].ToString())
            //{
            //    
            //}

            //panel container bottom
            Panel p2 = new Panel();
            p2.Controls.Add(l2);
            p2.Controls.Add(p11);
            p2.Controls.Add(b1);
            p2.Dock = System.Windows.Forms.DockStyle.Fill;
            p2.Location = new System.Drawing.Point(0, 82);
            p2.Name = "panel10";
            p2.Size = new System.Drawing.Size(234, 178);
            p2.TabIndex = 6;

            //label title
            Label l1 = new Label();
            l1.AutoSize = true;
            l1.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            l1.ForeColor = System.Drawing.Color.White;
            l1.Location = new System.Drawing.Point(96, 15);
            l1.Name = "label1";
            l1.Size = new System.Drawing.Size(63, 25);
            l1.TabIndex = 0;
            l1.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();

            //photo courses
            PictureBox pc = new PictureBox();
            var data = (Byte[])(dataGridView1.Rows[i].Cells[2].Value);
            var stream = new MemoryStream(data);
            pc.Image = Image.FromStream(stream);
            pc.Location = new System.Drawing.Point(7, 3);
            pc.Name = "pictureBox3";
            pc.Size = new System.Drawing.Size(83, 76);
            pc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pc.TabIndex = 1;
            pc.TabStop = false;

            //label teacher
            Label l3 = new Label();
            l3.AutoSize = true;
            l3.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            l3.ForeColor = System.Drawing.Color.White;
            l3.Location = new System.Drawing.Point(98, 38);
            l3.Name = "label3";
            l3.Size = new System.Drawing.Size(116, 17);
            l3.TabIndex = 2;
            l3.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();

            //seperator
            Panel p12 = new Panel();
            p12.BackColor = System.Drawing.Color.Silver;
            p12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            p12.Location = new System.Drawing.Point(17, 88);
            p12.Name = "panel12";
            p12.Size = new System.Drawing.Size(200, 1);
            p12.TabIndex = 2;

            //conteiner top
            Panel p3 = new Panel();
            p3.Controls.Add(li);
            p3.Controls.Add(p12);
            p3.Controls.Add(l3);
            p3.Controls.Add(pc);
            p3.Controls.Add(l1);
            p3.Dock = System.Windows.Forms.DockStyle.Top;
            p3.Location = new System.Drawing.Point(0, 0);
            p3.Name = "panel9";
            p3.Size = new System.Drawing.Size(234, 96);
            p3.TabIndex = 5;

            //container
            Panel p = new Panel();
            p.BackColor = System.Drawing.Color.Teal;
            p.Controls.Add(p2);
            p.Controls.Add(p3);
            p.Location = new System.Drawing.Point(3, 3);
            p.Name = "panel8" + i.ToString();
            p.Size = new System.Drawing.Size(234, 260);
            p.TabIndex = 0;
            p.Name = idcourse;
            return p;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            pause();
            parrentPage.SetPage(0);
        }

        void pause()
        {
            player.Ctlcontrols.pause();
            iconButton2.IconChar = FontAwesome.Sharp.IconChar.PlayCircle;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            pause();
            parrentPage.SetPage(4);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            pause();
            parrentPage.SetPage(1);
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.pause();
            iconButton2.IconChar = FontAwesome.Sharp.IconChar.PauseCircle;
            parrentPage.SetPage(2);
        }
        private void guna2Button6_Click(object sender, EventArgs e)
        {
            pause();
            parrentPage.SetPage(3);
        }

        private void iconButton6_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.pause();
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.play();
            if( player.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                player.Ctlcontrols.play();
            }
            else if (player.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                player.Ctlcontrols.pause();
            }
        }

        private void sliderVol_Scroll(object sender, ScrollEventArgs e)
        {
            player.settings.volume = sliderVol.Value;
        }

        private void sliderVol_ValueChanged(object sender, EventArgs e)
        {
            player.settings.volume = sliderVol.Value;
        }

        private void AffVideos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var selected = AffVideos.SelectedRows;
            foreach (var row in selected)
            {
                int id = (int)((DataGridViewRow)row).Cells[0].Value;
                openFile(id);
                timer1.Start();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Button b1 = new Button();
            //Label l = new Label();
            //for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
            //{
            //    //if (b1.Name[i].ToString() == l.Name[i].ToString())
            //    //{
            //        idcourse = l.Text.ToString();
            //    //}
            //}

            Label li = new Label();
            label4.Text = (sender as Button).Name;
            Guna2Button b = guna2Button4;
            b.Checked = true;
            pictureBox1.Location = new Point(b.Location.X + 27, b.Location.Y - 25);
            pictureBox1.SendToBack();
            parrentPage.SetPage(2);
        }

        public enum enumType
        {
            Seccess,
            Warning,
            Error,
            Info
        }

        private void guna2Button13_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.Connection = con;
                con.Open();

                MemoryStream ms = new MemoryStream();
                profileimage.Image.Save(ms, profileimage.Image.RawFormat);
                byte[] img = ms.ToArray();

                cmd.CommandText = "Update profil set  nom=@b,prenom=@c,photos=@d,email=@e,passwords=@f,tele=@g,pays=@h where idProfile=@a";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", idlog);
                cmd.Parameters.AddWithValue("@b", firstname.Text);
                cmd.Parameters.AddWithValue("@c", lastname.Text);
                cmd.Parameters.AddWithValue("@d", img);
                cmd.Parameters.AddWithValue("@e", email.Text);
                cmd.Parameters.AddWithValue("@f", password.Text);
                cmd.Parameters.AddWithValue("@g", tele.Text);
                cmd.Parameters.AddWithValue("@h", combocountry.SelectedItem);
                string str = "okey broooooo";
                Form ff = new alert2(str, alert2.enumType.Seccess);
                ff.Show();
                cmd.ExecuteNonQuery();
                con.Close();
                AffichageProfiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                string str = ex.Message;
                Form ff = new alert2(str, alert2.enumType.Error);
                ff.Show();
            }
            finally { con.Close(); }
            AffichageProfiles();
        }

        private void profileimage_Click(object sender, EventArgs e)
        {
            //addimage
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "|*.png; *.jpg; *.jfif";
            op.ShowDialog();
            string cheminImage = op.FileName;
            if (cheminImage != "")
                profileimage.Image = Image.FromFile(cheminImage);
        }

        private void guna2CustomGradientPanel8_Click(object sender, EventArgs e)
        {
            profileimage.Image = pictureBox4.Image;
            firstname.Text = firstnamelbl.Text;
            lastname.Text = lastnamelbl.Text;
            email.Text = lblemail.Text;
            password.Text = lblpassword.Text;
            tele.Text = label10.Text;
            combocountry.SelectedItem = lblcountry.Text;
        }
        private void afficherVideoI()
        {
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select photos,langage,titreC from Element e join vidinscrit v on e.idElem=v.idVid join courses c on e.idCourse=c.idCourse where idProfil=@a order by langage";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@a", idlog);
            dr = cmd.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Clear();
            dt1.Load(dr);
            affvediosI.DataSource = dt1;
            dr.Close();
            con.Close();
        }

        private void iconButton2_Click_1(object sender, EventArgs e)
        {
            //player.Ctlcontrols.play();
            if (player.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                player.Ctlcontrols.play();
                iconButton2.IconChar = FontAwesome.Sharp.IconChar.PauseCircle;
            }
            else if (player.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                player.Ctlcontrols.pause();
                iconButton2.IconChar = FontAwesome.Sharp.IconChar.PlayCircle;
            }
        }

        private void iconButton3_Click_1(object sender, EventArgs e)
        {
            player.Ctlcontrols.pause();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (player.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                TrackBar.Maximum = (int)player.Ctlcontrols.currentItem.duration;
                TrackBar.Value = (int)player.Ctlcontrols.currentPosition;
            }
            label13.Text = player.Ctlcontrols.currentPositionString;
            label14.Text = player.Ctlcontrols.currentItem.durationString.ToString();
        }

        private void TrackBar_ValueChanged_1(object sender, EventArgs e)
        {
            player.settings.playCount = TrackBar.Value;
        }

        private void TrackBar_Scroll_1(object sender, ScrollEventArgs e)
        {
            player.Ctlcontrols.currentPosition = (int)TrackBar.Value;
        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            if (panel22.Visible == false)
            {
                panel22.Visible = true;
            }
            else
            {
                panel22.Visible = false;
            }
        }

        private void sliderVol_Scroll_1(object sender, ScrollEventArgs e)
        {
            player.settings.volume = sliderVol.Value;
            if (sliderVol.Value <= 50)
            {
                iconButton1.IconChar = FontAwesome.Sharp.IconChar.VolumeDown;
            }
            if(sliderVol.Value >= 50)
            {
                iconButton1.IconChar = FontAwesome.Sharp.IconChar.VolumeUp;
            }
            if (sliderVol.Value == 0)
            {
                iconButton1.IconChar = FontAwesome.Sharp.IconChar.VolumeOff;
            }
        }

        private void player_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            if(panel22.Visible == true)
            {
                panel22.Visible = false;
            }
            else
            {
                if (player.playState == WMPLib.WMPPlayState.wmppsPaused)
                {
                    player.Ctlcontrols.play();
                    iconButton2.IconChar = FontAwesome.Sharp.IconChar.PauseCircle;
                }
                else if (player.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    player.Ctlcontrols.pause();
                    iconButton2.IconChar = FontAwesome.Sharp.IconChar.PlayCircle;
                }
            }
            
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            if (panel23.Visible == false)
            {
                panel23.Visible = true;
            }
            else
                panel23.Visible = false;
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            pause();
            Form f = new admin2(idlog);
            f.Show();
            this.Hide();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            player.settings.mute.ToString();
        }
    }
}
