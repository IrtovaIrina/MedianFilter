using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static System.Windows.Forms.LinkLabel;

namespace Recognizer
{
    internal static class MedianFilterTask
    {
        /* 
		 * Для борьбы с пиксельным шумом, подобным тому, что на изображении,
		 * обычно применяют медианный фильтр, в котором цвет каждого пикселя, 
		 * заменяется на медиану всех цветов в некоторой окрестности пикселя.
		 * https://en.wikipedia.org/wiki/Median_filter
		 * 
		 * Используйте окно размером 3х3 для не граничных пикселей,
		 * Окно размером 2х2 для угловых и 3х2 или 2х3 для граничных.
		 */
        public static double[,] MedianFilter(double[,] original)
        {
            var lines = original.GetLength(0);
            var colums = original.GetLength(1);
            var originalWithFrame = new double[lines + 2, colums + 2];
            MakeZerosFrame(original,originalWithFrame,lines,colums);
            var medianFilter = new double[lines, colums];
            var neighboringPixels = new List<double>();
            for (int x = 1; x <= lines; x++)
            {
                for (int y = 1; y <= colums; y++)
                {
                    neighboringPixels.Clear();
                    MakeMedianFilter(original, x, y, neighboringPixels,medianFilter, 0);
                }
            }
            return original;
        }
        public static void MakeZerosFrame(double[,] original, double[,] originalWithFrame,double lines,double colums)
        {
            for (int x = 1; x <= lines; x++)
            {
                for (int y = 1; y <= colums; y++)
                {
                    originalWithFrame[x, y] = original[x - 1, y - 1];
                }
            }
        }

        public static void MakeMedianFilter(double[,] original, int x, int y, List<double> neighboringPixels, double[,] medianFilter, int v)
        {
            for (int i = x - 1; i <= x + 1; i++) 
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    neighboringPixels.Add(original[i, j]);
                }
            }
            neighboringPixels.RemoveAll(s => s.Equals(null));
            neighboringPixels.Sort();
            if (neighboringPixels.Count % 2 == 1) medianFilter[x, y] = neighboringPixels[neighboringPixels.Count / 2];
            else medianFilter[x, y] = neighboringPixels[1] + neighboringPixels[2] / 2;
        }
    }
}