using Guna.UI2.WinForms;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoursesPFE
{
    public partial class admin2 : Form
    {
        int idlog;
        public admin2(int id)
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

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            parrentPage.SetPage(0);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            parrentPage.SetPage(1);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            parrentPage.SetPage(2);
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            parrentPage.SetPage(3);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
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

        //Fill dropDowList with Courses
        private void Remplissage()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from courses", @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=dbcours;Integrated Security=True");
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            da.Fill(dt1);
            da.Fill(dt2);
            comboIdCourse.DisplayMember = "langage";
            comboIdCourse.ValueMember = "idCourse";
            comboVideosFilter.DisplayMember = "langage";
            comboVideosFilter.ValueMember = "idCourse";
            comboIdCourse.DataSource = dt1;
            comboVideosFilter.DataSource = dt2;
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
            AffCourse.Columns[2].Visible = false;
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
            AffVedios.DataSource = dt;
            AffVedios.Columns[5].Visible = false;
            AffVedios.Columns[6].Visible = false;
            dr.Close();
            con.Close();
        }

        //private void remplissagepic()
        //{
        //    cmd.Connection = con;
        //    con.Open();
        //    cmd.CommandText = "select photos from profil where idProfile=" + idlog;
        //    dr = cmd.ExecuteReader();
        //    while (dr.Read())
        //    {
        //        var data = (Byte[])(dr[0]);
        //        var stream = new MemoryStream(data);
        //        picdashboard.Image = Image.FromStream(stream);                
        //    }
        //    con.Close();
        //}

        private void affichageActif()
        {
            con.Open();
            cmd.CommandText = "select count(actif) from profil where actif=1";
            string actif = cmd.ExecuteScalar().ToString();
            Numactif.Text = actif;
            con.Close();
        }

        private void admin2_Load(object sender, EventArgs e)
        {
            panel23.Visible = false;
            //remplissagepic();
            affichCourse();
            Remplissage();
            affichVedio();
            RemplissageIDprofile(comboidprofile);
            AffichageProfiles();
            affichageActif();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select nom, prenom, photos  from profil where idProfile=@a";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@a", idlog);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                string fullName = dr["nom"].ToString() + " " + dr["prenom"].ToString() + ",";
                FullName.Text = fullName;
                var data = (Byte[])(dr[2]);
                var stream = new MemoryStream(data);
                picdashboard.Image = Image.FromStream(stream);
            }
            dr.Close();
            con.Close();
        }

        private void AffCourse_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
        }

        private void AffCourse_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
        }
        //Update Courses
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

        private void btnADDcourse_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                //insert image
                MemoryStream ms = new MemoryStream();
                bunifuPictureBox1.Image.Save(ms, bunifuPictureBox1.Image.RawFormat);
                byte[] img = ms.ToArray();

                cmd.Connection = con;
                cmd.CommandText = "insert into courses values(@b,@c,@d,@g,@e,@f)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@b", txtDescr.Text);
                cmd.Parameters.AddWithValue("@c", img);
                cmd.Parameters.AddWithValue("@d", txtName.Text);
                cmd.Parameters.AddWithValue("@g", txtProf.Text);
                cmd.Parameters.AddWithValue("@e", DateTime.Now);
                cmd.Parameters.AddWithValue("@f", txtNumEl.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                affichCourse();
                Remplissage();
                string str = "course added seccessfuly";
                Form ff = new alert2(str, alert2.enumType.Seccess);
                ff.Show();
                con.Close();
        }
            catch (Exception ex) 
            {
                string str =ex.Message;
                Form ff = new alert2(str, alert2.enumType.Error);
                ff.Show();
            }
            finally { con.Close(); }
        }

        private void btnUPDcourse_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                //insert image
                MemoryStream ms = new MemoryStream();
                bunifuPictureBox1.Image.Save(ms, bunifuPictureBox1.Image.RawFormat);
                byte[] img = ms.ToArray();

                cmd.Connection = con;
                cmd.CommandText = "update courses set descriptions=@b,photos=@c,langage=@d,prof=@g,numElem=@e where @a=idCourse";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", txtId.Text);
                cmd.Parameters.AddWithValue("@b", txtDescr.Text);
                cmd.Parameters.AddWithValue("@c", img);
                cmd.Parameters.AddWithValue("@d", txtName.Text);
                cmd.Parameters.AddWithValue("@g", txtProf.Text);
                cmd.Parameters.AddWithValue("@e", txtNumEl.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                string str = "course updated seccessfuly";
                Form ff = new alert2(str, alert2.enumType.Seccess);
                ff.Show();
                affichCourse();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { con.Close(); }
        }

        public void deleteCourse()
        {
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete courses where idCourse=@a";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", txtId.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                affichCourse();
                Remplissage();
                string str = "course deleted seccessfuly";
                Form ff = new alert2(str, alert2.enumType.Error);
                ff.Show();
            }
            catch (Exception) { }
            finally { con.Close(); }
        }

        private void btnDELcourse_Click(object sender, EventArgs e)
        {
            deleteCourse();
            //int id = int.Parse(txtId.Text);
            //string message = "did you want to delete this course";
            //Form confirm = new confermation(id,message);
            //confirm.ShowDialog();
            //affichCourse();
            //Remplissage();
        }
        //Search Courses
        private void txtSearch_IconRightClick(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select * from courses where langage=@a";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", txtSearch.Text);
                dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Load(dr);
                AffCourse.DataSource = dt;
                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void guna2Button6_Click_1(object sender, EventArgs e)
        {
            affichCourse();
            txtId.Text = "";
            txtName.Text = "";
            txtDescr.Text = "";
            txtProf.Text = "";
            txtNumEl.Text = "";
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select * from courses where langage=@a";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", txtSearch.Text);
                dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Load(dr);
                AffCourse.DataSource = dt;
                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void guna2Button7_Click_1(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select * from courses where dateAjoute between @a and @b";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", dateDebut.Value);
                cmd.Parameters.AddWithValue("@b", dateFin.Value);
                dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Load(dr);
                AffCourse.DataSource = dt;
                dr.Close();
                con.Close();
            }
            catch (Exception)
            {
            }
        }

        private void txtLink_IconRightClick(object sender, EventArgs e)
        {

            OpenFileDialog op = new OpenFileDialog();
            op.ShowDialog();
            txtLink.Text = op.FileName;
        }

        private void AddVideos_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTitleV.Text == "")
                {
                    MessageBox.Show("You Can't Insert Empthy Values");
                }
                else
                {
                    Stream stream = File.OpenRead(txtLink.Text);
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);

                    string extn = new FileInfo(txtLink.Text).Extension;
                    string name = new FileInfo(txtLink.Text).Name;

                    cmd.Connection = con;
                    con.Open();

                    cmd.CommandText = "insert into Element(idCourse,titreC,chemain,duree,name,extension) values(@b,@c,@d,null,@f,@g)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@b", comboIdCourse.SelectedValue);
                    cmd.Parameters.AddWithValue("@c", txtTitleV.Text);
                    cmd.Parameters.AddWithValue("@d", buffer);
                    cmd.Parameters.AddWithValue("@f", name);
                    cmd.Parameters.AddWithValue("@g", extn);
                    cmd.ExecuteNonQuery();

                    con.Close();
                    affichVedio();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { con.Close(); }
        }

        private void UpdateVideos_Click(object sender, EventArgs e)
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
                cmd.Parameters.AddWithValue("@a", txtidV.Text);
                cmd.Parameters.AddWithValue("@b", comboIdCourse.SelectedValue);
                cmd.Parameters.AddWithValue("@c", txtTitleV.Text);
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

        private void DeleteVideos_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete Element where idElem=@a";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", txtidV.Text);
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

        private void AffVedios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtidV.Text = AffVedios.CurrentRow.Cells[0].Value.ToString();
            comboIdCourse.SelectedValue = AffVedios.CurrentRow.Cells[1].Value.ToString();
            txtTitleV.Text = AffVedios.CurrentRow.Cells[4].Value.ToString();
            txtLink.Text = AffVedios.CurrentRow.Cells[5].Value.ToString();
        }

        private void AffVedios_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
        }

        private void AffVedios_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
        }

        private void comboVideosFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select * from Element where idCourse=@a";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", comboVideosFilter.SelectedValue);
                dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Load(dr);
                AffVedios.DataSource = dt;
                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void guna2TextBox1_IconRightClick(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select * from Element where titreC=@a";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", txtTitreVFilter.Text);
                dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Load(dr);
                AffVedios.DataSource = dt;
                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "select e.* from Element e join courses c on e.idCourse=c.idCourse where langage=@a";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", txtNameCFilter.Text);
                dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Load(dr);
                AffVedios.DataSource = dt;
                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            affichVedio();
            txtidV.Text = "";
            txtTitleV.Text = "";
            txtLink.Text = "";
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
            
        }

        private void txtLink_TextChanged(object sender, EventArgs e)
        {

        }

        public enum enumType
        {
            Seccess,
            Warning,
            Error,
            Info
        }

        private void guna2Button14_Click(object sender, EventArgs e)
        {
            string str = "hello";
            Form f = new alert2(str,alert2.enumType.Seccess);
            f.Show();
        }

        private void AffCourse_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtId.Text = AffCourse.CurrentRow.Cells[0].Value.ToString();
            txtDescr.Text = AffCourse.CurrentRow.Cells[1].Value.ToString();
            txtName.Text = AffCourse.CurrentRow.Cells[3].Value.ToString();
            txtProf.Text = AffCourse.CurrentRow.Cells[4].Value.ToString();
            txtNumEl.Text = AffCourse.CurrentRow.Cells[6].Value.ToString();
            var data = (Byte[])(AffCourse.CurrentRow.Cells[2].Value);
            var stream = new MemoryStream(data);
            bunifuPictureBox1.Image = Image.FromStream(stream);
        }

        //Fill dropDowList with idProfile
        private void RemplissageIDprofile(Guna2ComboBox combo)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from profil", @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=dbcours;Integrated Security=True");
            DataTable dt = new DataTable();
            da.Fill(dt);
            combo.DisplayMember = "nom";
            combo.ValueMember = "idProfile";
            combo.DataSource = dt;
        }

        private void AffichageProfiles()
        {
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select p.idProfile,p.nom,prenom,photos,p.email,tele,pays,role,actif from profil p join logins l on p.idProfile=l.idLogin";
            dr = cmd.ExecuteReader();            
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Load(dr);
            dr.Close();
            affProfile.DataSource = dt;
            affProfile.Columns[0].Visible = false;
            affProfile.Columns[3].Visible = false;
            con.Close();
        }

        private void imgProfile_Click(object sender, EventArgs e)
        {
            //addimage
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "|*.png; *.jpg; *.jfif";
            op.ShowDialog();
            string cheminImage = op.FileName;
            if (cheminImage != "")
                imgProfile.Image = Image.FromFile(cheminImage);
        }
        void updateProfile()
        {
            con.Open();

            MemoryStream ms = new MemoryStream();
            imgProfile.Image.Save(ms, imgProfile.Image.RawFormat);
            byte[] img = ms.ToArray();

            cmd.CommandText = "Update profil set  nom=@b,prenom=@c,photos=@d,email=@e,passwords=@f,tele=@g,pays=@h where idProfile=@a";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@a", comboidprofile.SelectedValue);
            cmd.Parameters.AddWithValue("@b", namep.Text);
            cmd.Parameters.AddWithValue("@c", lastnamep.Text);
            cmd.Parameters.AddWithValue("@d", img);
            cmd.Parameters.AddWithValue("@e", emailp.Text);
            cmd.Parameters.AddWithValue("@f", passwordp.Text);
            cmd.Parameters.AddWithValue("@g", telep.Text);
            cmd.Parameters.AddWithValue("@h", combocountry.SelectedItem);
            string str = "Profile updated drseccessfuly";
            Form ff = new alert2(str, alert2.enumType.Seccess);
            ff.Show();
            cmd.ExecuteNonQuery();
            con.Close();
            AffichageProfiles();
        }

        void updateRole()
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "Update logins set  role=@b where idLogin=@a";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@a", comboidprofile.SelectedValue);
            cmd.Parameters.AddWithValue("@b", RoleLogin.SelectedItem);
            string str = "Role updated seccessfuly";
            Form ff = new alert2(str, alert2.enumType.Seccess);
            ff.Show();
            cmd.ExecuteNonQuery();
            con.Close();
            AffichageProfiles();
        }

        private void UpdateProfiles_Click(object sender, EventArgs e)
        {
            try
            {
                
                string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
                bool regext = false;
                bool test = false;
                if (Regex.IsMatch(emailp.Text, pattern))
                {
                    regext = true;
                }
                else
                {
                    regext = false;
                }

                if ((int)comboidprofile.SelectedValue == idlog)
                {
                    test = true;
                }
                else
                {
                    test = false;
                }
                if (regext == true)
                {
                    if (test == true)
                    {
                        updateProfile();
                    }
                    else
                    {
                        updateRole();
                    }
                }
                else
                {
                    string str = "Email not correct";
                    Form ff = new alert2(str, alert2.enumType.Error);
                    ff.Show();
                }
                AffichageProfiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { con.Close(); }
        }

        private void comboidprofile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((int)comboidprofile.SelectedValue != idlog)
            {
                namep.ReadOnly = true;
                lastnamep.ReadOnly = true;
                emailp.ReadOnly = true;
                passwordp.ReadOnly = true;
                telep.ReadOnly = true;
            }
            else
            {
                namep.ReadOnly = false;
                lastnamep.ReadOnly = false;
                emailp.ReadOnly = false;
                passwordp.ReadOnly = false;
                telep.ReadOnly = false;
            }
        }

        private void guna2Button14_Click_1(object sender, EventArgs e)
        {
            AffichageProfiles();
        }

        private void admin2_Leave(object sender, EventArgs e)
        {
            con.Open();
            cmd.CommandText = "update profil set actif=0 where idProfile=@a";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@a", idlog);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void affProfile_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            comboidprofile.SelectedValue = affProfile.CurrentRow.Cells[0].Value.ToString();
            namep.Text = affProfile.CurrentRow.Cells[1].Value.ToString();
            lastnamep.Text = affProfile.CurrentRow.Cells[2].Value.ToString();
            emailp.Text = affProfile.CurrentRow.Cells[4].Value.ToString();
            telep.Text = affProfile.CurrentRow.Cells[5].Value.ToString();
            combocountry.SelectedItem = affProfile.CurrentRow.Cells[6].Value.ToString();
            RoleLogin.SelectedItem = affProfile.CurrentRow.Cells[7].Value.ToString();
            var data = (Byte[])(affProfile.CurrentRow.Cells[3].Value);
            var stream = new MemoryStream(data);
            imgProfile.Image = Image.FromStream(stream);
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select p.idProfile,p.nom,prenom,photos,p.email,tele,pays,role,actif from profil p join logins l on p.idProfile=l.idLogin where role=@a";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@a", guna2ComboBox2.SelectedItem);
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Load(dr);
            dr.Close();
            affProfile.DataSource = dt;
            affProfile.Columns[0].Visible = false;
            affProfile.Columns[3].Visible = false;
            con.Close();
        }

        private void guna2TextBox8_IconRightClick(object sender, EventArgs e)
        {
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select p.idProfile,p.nom,prenom,photos,p.email,tele,pays,role,actif from profil p join logins l on p.idProfile=l.idLogin where p.prenom=@a";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@a", guna2TextBox8.Text);
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Load(dr);
            dr.Close();
            affProfile.DataSource = dt;
            affProfile.Columns[0].Visible = false;
            affProfile.Columns[3].Visible = false;
            con.Close();
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select p.idProfile,p.nom,prenom,photos,p.email,tele,pays,role,actif from profil p join logins l on p.idProfile=l.idLogin where pays=@a";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@a", guna2ComboBox1.SelectedItem);
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Load(dr);
            dr.Close();
            affProfile.DataSource = dt;
            affProfile.Columns[0].Visible = false;
            affProfile.Columns[3].Visible = false;
            con.Close();
        }

        private void guna2Button13_Click(object sender, EventArgs e)
        {
            if (panel23.Visible == false)
            {
                panel23.Visible = true;
            }
            else
                panel23.Visible = false;
        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
            Form f = new Visiteur2(idlog);
            f.Show();
            this.Hide();
        }
    }
}
