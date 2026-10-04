using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Arknights_PC_Recruit_Helper.src
{
    public class ImageFinder
    {

        public static System.Drawing.Point? FindSubImage(Bitmap screenB, Bitmap screenA, double threshold = 0.90)
        {
            // 1. Конвертируем Bitmap из System.Drawing в матрицы Mat для OpenCV
            using (Mat matB = screenB.ToMat())
            using (Mat matA = screenA.ToMat())
            // 2. Создаем матрицу для сохранения результатов сравнения
            using (Mat result = new Mat())
            {
                // 3. Выполняем поиск по шаблону. 
                // CcoeffNormed (нормализованный коэффициент корреляции) — наиболее точный метод для скриншотов
                Cv2.MatchTemplate(matB, matA, result, TemplateMatchModes.CCoeffNormed);

                // 4. Находим координаты с максимальным совпадением
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

        public static List<string> getCurrentTags(string currentScreen, string tagsPath)
        {
            string tagPath;
            List<string> currentTags = new List<string>();
            int cycleCount = 0;
            Bitmap bitmapB = new Bitmap(currentScreen); // Где искать
            Bitmap bitmapA;

            foreach (string tag in RecruitTags.allTags)
            {
                cycleCount++;
                tagPath = $"{tagsPath}{tag}.png";

                try
                {
                    bitmapA = new Bitmap(tagPath); // Что искать
                    System.Drawing.Point? coordinates = ImageFinder.FindSubImage(bitmapB, bitmapA);
                    if (coordinates.HasValue)
                    {
                        currentTags.Add(tag);                        
                    }
                    
                    //освобождаем данные bitmap чтобы можно было удалять скрины 
                    if ((currentTags.Count == RecruitTags.maxTagsOnScreen) || (cycleCount == RecruitTags.allTags.Length))
                    {
                        bitmapB.Dispose();
                        bitmapA.Dispose();
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