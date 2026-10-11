using Arknights_PC_Recruit_Helper.src;
using System.Drawing.Imaging;

namespace Arknights_PC_Recruit_Helper
{
    public partial class Form1 : Form
    {
        private static string screenshotPath = "../res/screenshot/";
        private static string tagsPath = "../res/tags/";
        private static string currentPicture = "";

        public Form1() 
        {            
            InitializeComponent();
            TopMost = true;
            TopLevel = true;
            Scan.Enabled = false;
            ImageFinder.InitializeTagsCache(tagsPath);
        }

        private void Screenshot_Click(object sender, EventArgs e)
        {
            MakeScreenshot();
            Scan.Enabled = true;
            listBox1.BackColor = Color.White;
        }

        private void Scan_Click(object sender, EventArgs e)
        {
            Scan.Enabled = false;

            var currentTags = ImageFinder.GetCurrentTags(currentPicture, tagsPath);
            string resultTagsCombinations = RecruitTags.GetBestTags(currentTags);

            if (resultTagsCombinations == "")
                resultTagsCombinations = "Only 3★";

            listBox1.Items.Clear();
            listBox1.Items.Add($"{currentTags.Count} tags found:");
            if (currentTags.Count < RecruitTags.maxTagsOnScreen)
                listBox1.BackColor = Color.Red;

            foreach (var str in currentTags)
            {
                listBox1.Items.Add(str);
            }
            listBox1.Items.Add("-----------------------------------");
            foreach (var str in resultTagsCombinations.Split('\n'))
            {
                listBox1.Items.Add(str);
            }
        }

        private static void MakeScreenshot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using var bitmap = new Bitmap(bounds.Width, bounds.Height);
            using var g = Graphics.FromImage(bitmap);

            g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            currentPicture = Path.Combine(screenshotPath, $"{DateTime.Now.ToString("yyyy-MM-dd_HH\\hmm_ss")}.jpg");
            bitmap.Save(currentPicture, ImageFormat.Jpeg);
            bitmap.Dispose();            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ImageFinder.DisposeCache();
            foreach (string file in Directory.GetFiles(screenshotPath))
            {
                File.Delete(file);
            }
        }
    }
}
