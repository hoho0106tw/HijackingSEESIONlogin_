namespace HijackingSEESIONlogin_
{
    public partial class Form1 : Form
    {
        private WebView2SessionHelper sessionHelper = new WebView2SessionHelper();

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // 初始化 WebView2
            await sessionHelper.InitializeAsync(webView21);

            // 導向登入頁
            sessionHelper.Navigate("https://example.com/login");
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            // 假設你已經登入網站
            string fileUrl = "https://example.com/files/report.pdf";
            string savePath = @"C:\Temp\report.pdf";

            await sessionHelper.DownloadFileAsync(fileUrl, savePath);

            MessageBox.Show("下載完成！");
        }
    }
}
