using System;
using Xunit;
using SorterAPI.Services;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace Tests
{
    public class SorterTests : SorterService // to access protected methods
    {
        private readonly ImmutableList<int> unsortedNumbers = ImmutableList.Create(new int[] { 2, 8, 0, 4, 5, 9, 3, 1010, 1, 0, 99999, 11, 2, 2, 2, 1337, 2, 6, 0, 0, -991, 69, 32, 42, 39, 7, 111, 1111 });
        private readonly ImmutableList<int> sortedNumbers = ImmutableList.Create(new int[] { -991, 0, 0, 0, 0, 1, 2, 2, 2, 2, 2, 3, 4, 5, 6, 7, 8, 9, 11, 32, 39, 42, 69, 111, 1010, 1111, 1337, 99999 });
        
        [Fact]
        public void TestQuickSort() // +
        {
            List<int> numbers = new List<int>(unsortedNumbers);
            QuickSort(numbers);
            Assert.Equal(numbers, sortedNumbers);
        }

        [Fact]
        public void TestInsertionSort()
        {
            List<int> numbers = new List<int>(unsortedNumbers);
            InsertionSort(numbers);
            Assert.Equal(numbers, sortedNumbers);
        }

        [Fact]
        public void TestBubbleSort() 
        {
            List<int> numbers = new List<int>(unsortedNumbers);
            BubbleSort(numbers);
            Assert.Equal(numbers, sortedNumbers);
        }

        [Fact]
        public void TestSwap()
        {
            List<int> numbers = new List<int>(new int[] { 1, 2, 3, -4, 6, 1337, 99 });
            Swap(numbers, 0, numbers.Count - 2);
            Assert.Equal(new int[] { 1337, 2, 3, -4, 6, 1, 99 }.ToList(), numbers);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello world!!! ")] // should be ignored 
        public void TestSortAndSave(string input)
        {
            //string input = "";
            foreach (var i in unsortedNumbers)
            {
                input += i + " ";
            }

            SortAndSave(input);

            DirectoryInfo info = new DirectoryInfo(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\Files")));
            var lastFile = info.GetFiles().OrderBy(x => x.LastWriteTime).First();

            using(var sr = lastFile.OpenText())
            {
                Assert.Equal("-991 0 0 0 0 1 2 2 2 2 2 3 4 5 6 7 8 9 11 32 39 42 69 111 1010 1111 1337 99999", sr.ReadLine().Trim());
            }
        }

        [Fact]
        public void TestLoadLatestFile()
        {
            string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\Files"));

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (StreamWriter sw = File.CreateText(path + @"\test.txt"))
            {
                sw.Write("Hello World 123!");
            }

            Assert.Equal("Hello World 123!", LoadLatestFile());
            File.Delete(path + @"\test.txt");
        }
    }
}
