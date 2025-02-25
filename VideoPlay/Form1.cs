using System.Windows.Forms;

namespace VideoPlay
{
    public partial class Form1 : Form
    {
        public string URLpath ;
        bool useWebView;
        public Form1(Uri url)
        {
            Environment.SetEnvironmentVariable("WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS", "--autoplay-policy=no-user-gesture-required");


            InitializeComponent();
            if (url.Scheme == "youtube")
            {
                URLpath = $"https://www.youtube.com/embed/{url.LocalPath[1..]}?autoplay=1";
                this.axWindowsMediaPlayer1.Visible = false;
                useWebView = true;
            }
            else
            {
                this.webView21.Visible = false;

                URLpath = url.ToString();
                useWebView = false;
            }

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            var screen = Screen.AllScreens.FirstOrDefault((screen) => !screen.Primary);
            if (screen != null)
            {
                this.Bounds = screen.Bounds;
            }

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            if (useWebView)
            {
                webView21.Source = new Uri(URLpath);
                webView21.NavigationCompleted += (e,f) => { this.InvokeOnClick(webView21,null); };
            }
            else
            {
                axWindowsMediaPlayer1.URL = URLpath;
                axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.uiMode = "none";
            }
                
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void axWindowsMediaPlayer1_KeyDownEvent(object sender, AxWMPLib._WMPOCXEvents_KeyDownEvent e)
        {
            if (e.nKeyCode == (short)Keys.F12)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }
    }
}
