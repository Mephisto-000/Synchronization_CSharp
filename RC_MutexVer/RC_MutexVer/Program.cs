using System;
using System.Threading;

namespace MutexSolutionExample
{
    class Program
    {
        // 共享資源：計數器
        static int sharedData = 0;
        // 每個執行緒的迭代次數
        static int iterations = 20;
        // 用於保護 sharedData 的 Mutex 物件
        static Mutex mutex = new Mutex();

        static void Main(string[] args)
        {
            Thread[] threads = new Thread[4];

            // 建立 4 個執行緒，共同執行 IncrementWithLock 方法
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(IncrementWithLock);
                threads[i].Name = $"Thread {i + 1}";
                threads[i].Start();
            }

            // 等待所有執行緒完成
            foreach (var thread in threads)
            {
                thread.Join();
            }

            // 所有執行緒正確同步後，最終值應為 4 * iterations
            Console.WriteLine($"\n最終 sharedData 值：{sharedData} (預期：{4 * iterations})");
        }

        static void IncrementWithLock()
        {
            for (int i = 0; i < iterations; i++)
            {
                // 進入臨界區前先取得 Mutex
                mutex.WaitOne();

                // 讀取共享資源
                int temp = sharedData;
                Console.WriteLine($"{Thread.CurrentThread.Name} 讀取 sharedData = {temp}");

                // 模擬計算：加 1
                temp = temp + 1;

                // 模擬延遲（在臨界區中也能確保操作完整性）
                Thread.Sleep(10);

                // 將計算後的值寫回共享資源
                sharedData = temp;
                Console.WriteLine($"{Thread.CurrentThread.Name} 寫入 sharedData = {sharedData}");

                // 離開臨界區，釋放 Mutex
                mutex.ReleaseMutex();
            }
        }
    }
}
