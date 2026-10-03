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

            this.TopLevel = true;
            this.TopMost = true;

            Scan.Enabled = false;
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

            var currentTags = ImageFinder.getCurrentTags(currentPicture, tagsPath);
            string resultTagsCombinations = RecruitTags.getBestTags(currentTags);

            //Делаем вывод результата
            if (resultTagsCombinations == "")
                resultTagsCombinations = "Only 3★";

            listBox1.Items.Clear();
            listBox1.Items.Add($"{currentTags.Count} tags found:");
            //если не распознал все теги - подстветить listboxs
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

        // метод, который делает скриншот и записывает его в файл
        private static void MakeScreenshot()
        {
            // получаем размеры окна рабочего стола
            Rectangle bounds = Screen.GetBounds(Point.Empty);

            // создаем пустое изображения размером с экран устройства
            using (var bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                // создаем объект на котором можно рисовать
                using (var g = Graphics.FromImage(bitmap))
                {
                    // перерисовываем экран на наш графический объект
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }

                // сохраняем в файл с форматом Png
                currentPicture = screenshotPath + DateTime.Now.ToString("yyyy-MM-dd_HH\\hmm_ss") + ".png";
                bitmap.Save(currentPicture, ImageFormat.Jpeg);
                bitmap.Dispose();
            }
        }

        //по закрытию приложения удалить все файлы скриншотов
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (string file in Directory.GetFiles(screenshotPath))
            {
                File.Delete(file);
            }
        }
    }
}
