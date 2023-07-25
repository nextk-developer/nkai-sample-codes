using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Utils.Extentions
{
    public static class ConcurrentQueueEx
    {
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            while (queue.TryDequeue(out T ignore)) ;
        }
    }

    public static class BlockingCollectionEx
    {
        public static void Clear<T>(this BlockingCollection<T> queue)
        {
            while (queue.TryTake(out T ignore)) ;
        }

        public static void RemoveRange<T>(this BlockingCollection<T> queue, int count)
        {
            int startIndex = 0;
            while (queue.TryTake(out T ignore))
            {
                if (++startIndex < count)
                    return;
            }
        }

        public static void RemoveItem<T>(this BlockingCollection<T> collection, T removeItem)
        {
            lock (collection)
            {
                List<T> tmp = new List<T>();

                while (collection.TryTake(out T item))
                {
                    if (!ReferenceEquals(item, removeItem))
                        tmp.Add(item);
                }

                foreach (var item in tmp)
                    collection.Add(item);
            }
        }

    }
}