using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Arknights_PC_Recruit_Helper.src
{
    public class ImageFinder
    {
        private static Dictionary<string, Mat> _tagsCache = [];

        public static void InitializeTagsCache(string tagsPath)
        {
            if (_tagsCache.Count > 0) return;
            string tagPath;
            
            foreach (string tag in RecruitTags.allTags)
            {
                tagPath = Path.Combine(tagsPath, $"{tag}.png");

                if(File.Exists(tagPath))
                {
                    _tagsCache[tag] = new Mat(tagPath);
                }                 
            }
        }

        public static void DisposeCache()
        {
            if (_tagsCache.Count > 0) return;
            foreach (var tag in _tagsCache)
            {
                tag.Value?.Dispose();
            }
            _tagsCache.Clear();
        }

        public static System.Drawing.Point? FindSubImage(Mat matB, string tag, double threshold = 0.90)
        {
            using (Mat result = new Mat())
            {
                // Выполняем поиск по шаблону. 
                // CcoeffNormed (нормализованный коэффициент корреляции) — наиболее точный метод для скриншотов
                Cv2.MatchTemplate(matB, _tagsCache[tag], result, TemplateMatchModes.CCoeffNormed);

                // Находим координаты с максимальным совпадением
                Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

                // Проверяем, превышает ли совпадение заданный порог (0.9 = 90% сходства)
                if (maxVal >= threshold)
                {
                    // Возвращаем верхнюю левую точку найденного скриншота А внутри Б
                    return new System.Drawing.Point(maxLoc.X, maxLoc.Y);
                }
                else
                    return null;
            }
        }

        public static List<string> GetCurrentTags(string currentScreen, string tagsPath)
        {
            string tagPath;
            List<string> currentTags = new List<string>();
            int cycleCount = 0;
            using (Bitmap bitmapB = new Bitmap(currentScreen))
            using (Mat matB = bitmapB.ToMat())
            {
                foreach (string tag in RecruitTags.allTags)
                {
                    cycleCount++;
                    tagPath = Path.Combine(tagsPath, $"{tag}.png");

                    try
                    {
                        System.Drawing.Point? coordinates = ImageFinder.FindSubImage(matB, tag);
                        if (coordinates.HasValue)
                        {
                            currentTags.Add(tag);
                        }

                        if (currentTags.Count == RecruitTags.maxTagsOnScreen)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Print($"{ex.Message} проверь {tag}.png");
                    }
                }

                return currentTags;
            }
        }
    }
}