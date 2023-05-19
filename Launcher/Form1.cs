using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Configuration;
using System.Security;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Launcher
{
    public partial class Form1 : Form
    {
        private bool passwordVisible = false;
        public DateTime starttime;
        public int serverhours;
        public string apppath = "";
        public string sswebsite = "";
        public string radiusapp = "";
        private Timer countdownTimer;

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public Form1()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
            countdownTimer = new Timer();
            countdownTimer.Interval = 250;
            countdownTimer.Tick += CountdownTimer_Tick;
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan remainingTime = starttime.AddHours(serverhours) - DateTime.Now;
            if (remainingTime.TotalSeconds <= 0)
            {
                countdownTimer.Stop();
                lbTimer.Text = "00:00";
            }
            else
            {
                lbTimer.Text = remainingTime.ToString(@"hh\:mm\:ss");
            }
        }

        private void TogglePasswordVisibility()
        {
            passwordVisible = !passwordVisible;
            txtPassword.UseSystemPasswordChar = !passwordVisible;
        }

        private void buttonTogglePassword_Click(object sender, EventArgs e)
        {
            TogglePasswordVisibility();
        }

        private void btnWebsite_Click(object sender, EventArgs e)
        {
            Process.Start(sswebsite);
        }

        private void btnApp_Click(object sender, EventArgs e)
        {
            if (radiusapp != "")
            {
                radiusapp = System.IO.Path.GetFileNameWithoutExtension(radiusapp);
                Process[] processes = Process.GetProcessesByName(radiusapp);
                if (processes.Length > 0)
                {
                    try
                    {
                        IntPtr hWnd = processes[0].MainWindowHandle;
                        SetForegroundWindow(hWnd);
                    }
                    catch
                    {//
                    }
                }
                else
                {
                    Process.Start(radiusapp);
                }
            }

            //Run AS
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = apppath,
                UserName = txtUsername.Text,
                Domain = Environment.UserDomainName,
                UseShellExecute = false
            };
                string mypass = txtPassword.Text;

        SecureString securePassword = new SecureString();
            foreach (char c in mypass)
            {
                securePassword.AppendChar(c);
            }
        startInfo.Password = securePassword;
            Process.Start(startInfo);
        }

        private void btnTimer_Click(object sender, EventArgs e)
        {
            starttime = DateTime.Now;
            countdownTimer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Center the form on the screen
            this.StartPosition = FormStartPosition.Manual;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            int formWidth = this.Width;
            int formHeight = this.Height;
            int left = (screenWidth - formWidth) / 2;
            int top = (screenHeight - formHeight) / 2;
            this.Location = new Point(left, top);

            txtUsername.Text = ConfigurationManager.AppSettings["Username"];
            txtPassword.Text = ConfigurationManager.AppSettings["Password"];
            DateTime.TryParse(ConfigurationManager.AppSettings["StartTime"], out starttime);
            int.TryParse(ConfigurationManager.AppSettings["ServerHours"], out serverhours);
            apppath = ConfigurationManager.AppSettings["apppath"];
            sswebsite = ConfigurationManager.AppSettings["sswebsite"];
            radiusapp = ConfigurationManager.AppSettings["radiusapp"];
            
            if (sswebsite == "")
            {
                btnWebsite.Visible = false;
                btnTimer.Visible = false;
                lbTimer.Visible = false;
                this.Height = this.Height - 100;
            }

            if (apppath == "")
            {
                btnApp.Visible = false;
            }

            if (DateTime.Now < starttime.AddHours(serverhours))
            {
                countdownTimer.Start();
                TimeSpan remainingTime = starttime.AddHours(serverhours) - DateTime.Now;
                lbTimer.Text = remainingTime.ToString(@"hh\:mm\:ss");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save values to app.config
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["Username"].Value = txtUsername.Text;
            config.AppSettings.Settings["Password"].Value = txtPassword.Text;
            config.AppSettings.Settings["StartTime"].Value = starttime.ToString("yyyy-MM-dd HH:mm:ss");
            config.AppSettings.Settings["apppath"].Value = apppath;
            config.AppSettings.Settings["sswebsite"].Value = sswebsite;
            config.AppSettings.Settings["radiusapp"].Value = radiusapp;
            config.AppSettings.Settings["serverhours"].Value = serverhours.ToString();
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}