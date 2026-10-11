using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Collections.Concurrent;
using System.Diagnostics;

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

                if (File.Exists(tagPath))
                {
                    _tagsCache[tag] = new Mat(tagPath, ImreadModes.Grayscale);
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


        //Find an imageA in the screenshot with threshold accuracy
        public static bool FindSubImage(Mat matB, Mat matA, double threshold = 0.90)
        {
            using Mat result = new();
            Cv2.MatchTemplate(matB, matA, result, TemplateMatchModes.CCoeffNormed);
            Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out _);

            return maxVal >= threshold;
        }

        //Search throught the screenshoot and return list of current tags
        public static List<string> GetCurrentTags(string currentScreen, string tagsPath)
        {
            InitializeTagsCache(tagsPath);

            // ConcurrentBag to be Thread-Safe
            var foundTags = new ConcurrentBag<string>();
            using Bitmap bitmapB = new(currentScreen);
            using Mat colorMatB = bitmapB.ToMat();
            using Mat matB = new();
            Cv2.CvtColor(colorMatB, matB, ColorConversionCodes.BGR2GRAY);
            Parallel.ForEach(RecruitTags.allTags, (tag, state) =>
            {
                if (foundTags.Count >= RecruitTags.maxTagsOnScreen)
                {
                    state.Stop();
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
            return [.. foundTags];
        }      
    }
} 