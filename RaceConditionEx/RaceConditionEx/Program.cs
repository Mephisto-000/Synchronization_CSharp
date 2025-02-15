using System;
using System.Threading;

namespace RaceConditionExample
{
    class Program
    {
        // 共享資源：計數器
        static int sharedData = 0;
        // 每個執行緒的迭代次數
        static int iterations = 20;

        static void Main(string[] args)
        {
            Thread[] threads = new Thread[4];

            // 建立 4 個執行緒，共同執行 IncrementWithoutLock 方法
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(IncrementWithoutLock);
                threads[i].Name = $"Thread {i + 1}";
                threads[i].Start();
            }

            // 等待所有執行緒完成
            foreach (var thread in threads)
            {
                thread.Join();
            }

            // 預期值應為 4 * iterations，但 Race Condition 可能導致較低結果
            Console.WriteLine($"\n最終 sharedData 值：{sharedData} (預期：{4 * iterations})");
        }

        static void IncrementWithoutLock()
        {
            for (int i = 0; i < iterations; i++)
            {
                // 讀取共享資源
                int temp = sharedData;
                Console.WriteLine($"{Thread.CurrentThread.Name} 讀取 sharedData = {temp}");

                // 模擬計算：加 1
                temp = temp + 1;

                // 模擬延遲，容易讓執行緒交錯產生競爭
                Thread.Sleep(10);

                // 將計算後的值寫回共享資源
                sharedData = temp;
                Console.WriteLine($"{Thread.CurrentThread.Name} 寫入 sharedData = {sharedData}");
            }
        }
    }
}
