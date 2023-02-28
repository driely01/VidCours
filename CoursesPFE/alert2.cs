using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoursesPFE
{
    public partial class alert2 : Form
    {
        string codde;
        enumType typp;
        public alert2(string code, enumType type)
        {
            InitializeComponent();
            codde = code;
            typp = type;
        }

        public enum enmAction
        {
            wait,
            start,
            close
        }

        public enum enumType
        {
            Seccess,
            Warning,
            Error,
            Info
        }

        private alert2.enmAction action;
        private int x, y;


        public void showAlert(string mesg, enumType type)
        {
            this.Opacity = 0.0;
            this.StartPosition = FormStartPosition.Manual;
            string fname;

            for (int i = 0; i < 10; i++)
            {
                fname = "alert2" + i.ToString();
                alert2 frm = (alert2)Application.OpenForms[fname];
                if (frm == null)
                {
                    this.Name = fname;
                    this.x = Screen.PrimaryScreen.WorkingArea.Width - this.Width + 15;
                    this.y = Screen.PrimaryScreen.WorkingArea.Height - this.Height * i;
                    this.Location = new Point(this.x, this.y - 60);
                    break;
                }
            }
            this.x = Screen.PrimaryScreen.WorkingArea.Width - base.Width - 5;

            switch (type)
            {
                case enumType.Seccess:
                    this.ImgMssg.IconChar = IconChar.CheckCircle;
                    this.BackColor = Color.Green;
                        break;
                case enumType.Error:
                    this.ImgMssg.IconChar = IconChar.RadiationAlt;
                    this.BackColor = Color.DarkRed;
                    break;
                case enumType.Warning:
                    this.ImgMssg.IconChar = IconChar.ExclamationTriangle;
                    this.BackColor = Color.DarkOrange;
                    break;
                case enumType.Info:
                    this.ImgMssg.IconChar = IconChar.InfoCircle;
                    this.BackColor = Color.RoyalBlue;
                    break;
            }

            this.LblMessg.Text = mesg;

            this.Show();
            this.action = enmAction.start;
            this.timer1.Interval = 1;
            timer1.Start();
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (this.action)
            {
                case enmAction.wait:
                    timer1.Interval = 5000;
                    action = enmAction.close;
                    break;
                case enmAction.start:
                    timer1.Interval = 1;
                    this.Opacity += 0.1;
                    if (this.x < this.Location.X)
                    {
                        this.Left--;
                    }
                    else
                    {
                        if (this.Opacity == 1.0)
                        {
                            action = enmAction.wait;
                        }
                    }
                    break;
                case enmAction.close:
                    timer1.Interval = 1;
                    this.Opacity -= 0.1;

                    this.Left -= 3;
                    if (base.Opacity == 0.0)
                    {
                        base.Close();
                    }
                    break;
            }
        }

        private void alert2_Load(object sender, EventArgs e)
        {
            LblMessg.Text = codde;
            showAlert(codde, typp);
        }
        private void iconButton1_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1;
            action = enmAction.close;
            this.Dispose();
        }
    }
}
