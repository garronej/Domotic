using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace BoardApplication
{
    public class ThreadSafeQueue
    {
        private Queue queue;
        private int maxSize;
        private bool closed;
        private AutoResetEvent wHandle;

        public ThreadSafeQueue(int maxSize) {
            queue = new Queue();
            this.maxSize = maxSize;
            wHandle = new AutoResetEvent(false);
        }

        public void Close() {
            Monitor.Enter(queue);
            closed = true;
            wHandle.Set();
            Monitor.Exit(queue);
        }

        public bool push(Object item) {
            Monitor.Enter(queue);
            if (closed) {
                return false;
            }
            while (queue.Count >= maxSize) {
                Monitor.Exit(queue);
                wHandle.WaitOne();
                Monitor.Enter(queue);
                if (closed) {
                    return false;
                }
            }
            queue.Enqueue(item);
            if(queue.Count==1){
                wHandle.Set();
            }
            Monitor.Exit(queue);
            return true;
            
        }

        public bool pull(out Object item) {
            Monitor.Enter (queue) ;
            while (queue.Count <= 0) {
                if (closed) {
                    item = null;
                    return false;
                }
                Monitor.Exit(queue);
                wHandle.WaitOne();
                Monitor.Enter(queue);
                    
            }
            item = queue.Dequeue();
            if (queue.Count == maxSize - 1) {
                wHandle.Set();
            }
            Monitor.Exit(queue);
            return true;
            
        }

    }
}
