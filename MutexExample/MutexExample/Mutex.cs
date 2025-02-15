using System.Threading;

namespace MutexExample
{
    // 此類別提供全域唯一的 Mutex 來保護 Critical Section
    public static class MutexHelper
    {
        public static Mutex Mutex = new Mutex();
    }
}
