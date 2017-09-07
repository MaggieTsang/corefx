// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text;
using Microsoft.Xunit.Performance;
using Xunit;

namespace System.IO.Tests
{
    public class Perf_Directory : FileSystemTest
    {
        //[Benchmark]
        public void GetCurrentDirectory()
        {
            foreach (var iteration in Benchmark.Iterations)
                using (iteration.StartMeasurement())
                    for (int i = 0; i < 20000; i++)
                    {
                        Directory.GetCurrentDirectory(); Directory.GetCurrentDirectory(); Directory.GetCurrentDirectory();
                        Directory.GetCurrentDirectory(); Directory.GetCurrentDirectory(); Directory.GetCurrentDirectory();
                        Directory.GetCurrentDirectory(); Directory.GetCurrentDirectory(); Directory.GetCurrentDirectory();
                    }
        }

        [Benchmark(InnerIterationCount = 5)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void CreateDirectory(int levels)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                // Setup
                string testSubdirectory = GetTestSubdirectory(levels);

                // Actual perf testing
                using (iteration.StartMeasurement())
                    for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                        Directory.CreateDirectory(Path.Combine(GetTestFilePath(), testSubdirectory));
            }
        }

        [Benchmark(InnerIterationCount = 5)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void DeleteDirectory(int levels)
        {
            foreach (var iteration in Benchmark.Iterations)
            {
                // Setup
                string testSubdirectory = GetTestSubdirectory(levels);

                HashSet<string> roots = new HashSet<string>();

                for (int i = 0; i < Benchmark.InnerIterationCount; i++)
                {
                    string root = GetTestFilePath();
                    roots.Add(root);
                    Directory.CreateDirectory(Path.Combine(root, testSubdirectory));
                }

                // Actual perf testing
                using (iteration.StartMeasurement())
                {
                    foreach (string root in roots)
                    {
                        Directory.Delete(root, recursive: true);
                    }
                }
            }
        }

        //[Benchmark]
        public void Exists()
        {
            // Setup
            string testFile = GetTestFilePath();
            Directory.CreateDirectory(testFile);

            foreach (var iteration in Benchmark.Iterations)
                using (iteration.StartMeasurement())
                    for (int i = 0; i < 20000; i++)
                    {
                        Directory.Exists(testFile); Directory.Exists(testFile); Directory.Exists(testFile);
                        Directory.Exists(testFile); Directory.Exists(testFile); Directory.Exists(testFile);
                        Directory.Exists(testFile); Directory.Exists(testFile); Directory.Exists(testFile);
                    }

            // Teardown
            Directory.Delete(testFile);
        }

        private string GetTestSubdirectory(int levelsDeep)
        {
            StringBuilder sb = new StringBuilder(levelsDeep * 2 + 200);

            sb.Append("a");
            for (int i = 0; i < levelsDeep - 1; i++)
            {
                sb.Append(Path.DirectorySeparatorChar + "a");
            }

            return sb.ToString();
        }


    }
}
