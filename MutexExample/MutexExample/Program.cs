using System;
using System.Threading;

namespace MutexExample
{
    class Programs
    {
        static void Main(string[] args)
        {
            // 為主執行緒命名
            Thread.CurrentThread.Name = "主執行緒";

            Console.WriteLine($"{Thread.CurrentThread.Name} 嘗試進入 Critical Section...");
            // 主執行緒進入 Critical Section
            MutexHelper.Mutex.WaitOne();
            Console.WriteLine($"{Thread.CurrentThread.Name} 已進入 Critical Section，將占用 7 秒...");

            // 啟動副執行緒，讓其也嘗試進入 Critical Section
            Thread worker = new Thread(WorkerThread);
            worker.Name = "副執行緒";
            worker.Start();

            // 主執行緒在 Critical Section 中執行 7 秒的工作
            Thread.Sleep(7000);
            Console.WriteLine($"{Thread.CurrentThread.Name} 離開 Critical Section");
            // 主執行緒離開 Critical Section
            MutexHelper.Mutex.ReleaseMutex();

            // 等待副執行緒完成
            worker.Join();
            Console.WriteLine("程式結束");
        }

        static void WorkerThread()
        {
            // 若執行緒尚未命名則命名（通常已在建立時指定）
            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = "副執行緒";
            }

            // 等待 2 秒後開始嘗試進入 Critical Section
            Thread.Sleep(2000);
            Console.WriteLine($"\n{Thread.CurrentThread.Name}：2 秒後開始嘗試進入 Critical Section...");
            // 額外等待 1 秒，讓輸出順序更清楚
            Thread.Sleep(1000);

            while (true)
            {
                // 嘗試以非阻塞方式取得 Mutex (timeout = 0)
                if (MutexHelper.Mutex.WaitOne(0))
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name} 成功進入 Critical Section，將占用 5 秒...");
                    // 模擬在 Critical Section 中執行 5 秒的工作
                    Thread.Sleep(5000);
                    Console.WriteLine($"{Thread.CurrentThread.Name} 離開 Critical Section");
                    // 離開 Critical Section
                    MutexHelper.Mutex.ReleaseMutex();
                    break;
                }
                else
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name} 正在等待進入 Critical Section...");
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
