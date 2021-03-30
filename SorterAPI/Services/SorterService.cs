using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SorterAPI.Services
{
    public class SorterService : ISorterService
    {
        public string LoadLatestFile()
        {
            var dir = new DirectoryInfo(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\Files")));

            if (!Directory.Exists(dir.FullName))
                return null;

            var file = dir.GetFiles().OrderByDescending(x => x.LastWriteTime).FirstOrDefault();

            if (file == null)
                return null;
            
            using (StreamReader sr = File.OpenText(file.FullName))
            {
                return sr.ReadToEnd();
            }
        }

        public void SortAndSave(string numbers)
        {
            List<string> strNumbers = numbers.Trim().Split(" ").ToList();

            List<int> intNumbers = new List<int>();
            foreach(var num in strNumbers)
            {
                if (int.TryParse(num, out int result))
                    intNumbers.Add(result);
            }

            List<int> numbers2 = new List<int>(intNumbers); // we make copies so that we wouldnt try to sort sequence thats already sorted
            List<int> numbers3 = new List<int>(intNumbers);

            var ElapsedQuick = MeasureTime(QuickSort, intNumbers);
            var ElapsedBubble = MeasureTime(BubbleSort, numbers2);
            var ElapsedInsertion = MeasureTime(InsertionSort, numbers3);


            string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\Files"));

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            string fileName = Path.Combine(path, "Result_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss") + ".txt");

            while (true)
            {
                if (!File.Exists(fileName))
                {
                    using (StreamWriter sw = File.CreateText(fileName))
                    {
                        foreach (int number in intNumbers)
                        {
                            sw.Write(number + " ");
                        }
                        sw.WriteLine("\nQuickSort took " + ElapsedQuick + " ms");
                        sw.WriteLine("BubbleSort took " + ElapsedBubble + " ms");
                        sw.WriteLine("InsertionSort took " + ElapsedInsertion + " ms");
                    }
                    break;
                }
                else
                {
                    fileName = fileName[0..^4];
                    fileName += "-" + DateTime.Now.Millisecond + ".txt";
                }
            }
        }

        protected static double MeasureTime(Action<List<int>> sort, List<int> param)
        {
            var start = DateTime.Now;
            sort(param);
            return (DateTime.Now - start).TotalMilliseconds;
        }

        // arr - array in which we swap elements, x & y - indexes of swapped elements
        protected static void Swap(List<int> arr, int x, int y)
        {
            if (x >= arr.Count || y >= arr.Count)
                return;

            var tmp = arr[x];
            arr[x] = arr[y];
            arr[y] = tmp;
        }

        protected static void InsertionSort(List<int> numbers)
        {
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (numbers[j - 1] > numbers[j])
                        Swap(numbers, j - 1, j);
                }
            }
        }

        protected static void BubbleSort(List<int> numbers)
        {
            for (int i = 0; i < numbers.Count; i++)
            {
                for (int j = 0; j < numbers.Count - 1; j++)
                {
                    if (numbers[j] > numbers[j + 1])
                        Swap(numbers, j, j + 1);
                }
            }
        }

        protected static void QuickSort(List<int> numbers)
        {
            if (numbers == null || numbers.Count <= 1)
                return;

            QuickSort(numbers, 0, numbers.Count - 1);
        }

        private static void QuickSort(List<int> numbers, int start, int end) // not entirely mine
        {
            static void Partition(List<int> numbers, int start, int end, ref int i, ref int j)
            {
                i = start - 1;
                j = end;
                int p = start - 1, q = end;
                int v = numbers[end];

                while (true)
                {
                    while (numbers[++i] < v) ;

                    while (v < numbers[--j])
                        if (j == start)
                            break;

                    if (i >= j)
                        break;

                    Swap(numbers, i, j);

                    if (numbers[i] == v)
                    {
                        p++;
                        Swap(numbers, p, i);
                    }

                    if (numbers[j] == v)
                    {
                        q--;
                        Swap(numbers, j, q);
                    }
                }

                Swap(numbers, i, end);

                j = i - 1;
                for (int k = start; k < p; k++, j--)
                    Swap(numbers, k, j);

                i++;
                for (int k = end - 1; k > q; k--, i++)
                {
                    Swap(numbers, i, k);
                }
            }

            if (start >= end)
                return;

            int i = 0, j = 0;

            Partition(numbers, start, end, ref i, ref j);

            QuickSort(numbers, start, j);
            QuickSort(numbers, i, end);
        }
    }
}
