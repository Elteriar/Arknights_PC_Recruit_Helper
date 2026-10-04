using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Diagnostics;
using System.Collections.Concurrent;
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

        public static bool FindSubImage(Mat matB, Mat matA, double threshold = 0.90)
        {
            using (Mat result = new Mat())
            {
                // Выполняем поиск по шаблону. 
                // CcoeffNormed (нормализованный коэффициент корреляции) — наиболее точный метод для скриншотов
                Cv2.MatchTemplate(matB, matA, result, TemplateMatchModes.CCoeffNormed);

                // Находим координаты с максимальным совпадением
                Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out _);

                // Проверяем, превышает ли совпадение заданный порог (0.9 = 90% сходства)
                return maxVal >= threshold;
            }
        }

        public static List<string> GetCurrentTags(string currentScreen, string tagsPath)
        {
            // Гарантируем, что кэш инициализирован
            InitializeTagsCache(tagsPath);

            // Используем ConcurrentBag для безопасного добавления из разных потоков
            var foundTags = new ConcurrentBag<string>();
            using (Bitmap bitmapB = new Bitmap(currentScreen))
            using (Mat matB = bitmapB.ToMat())
            {
                // Параллельный обход всех тегов (задействует все ядра CPU)
                Parallel.ForEach(RecruitTags.allTags, (tag, state) =>
                {
                    // Проверяем, не нашли ли мы уже максимум тегов на экране
                    if (foundTags.Count >= RecruitTags.maxTagsOnScreen)
                    {
                        state.Stop(); // Останавливаем остальные потоки
                        return;
                    }

                    if (_tagsCache.TryGetValue(tag, out Mat? matA) && (matA is not null))
                    {
                        if (FindSubImage(matB, matA))
                        {
                            foundTags.Add(tag);
                        }
                    }

                });
                return [..foundTags];
            }
        }
    }
}