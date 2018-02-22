using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC2016.Admin.Controllers.Common
{
    public class RegCompact
    {
        /// <summary>
        /// 一小时最多允许注册量
        /// </summary>
        static int PER_HOUR = 1;

        Queue<long> queue = new Queue<long>();

        public RegCompact()
        {
        }

        public RegCompact(DateTime dt)
        {
            queue.Enqueue(dt.Ticks);
        }

        public static RegCompact Parse(string cachestr)
        {
            RegCompact cp = new RegCompact();
            string[] arr = cachestr.Split(',');
            for (int i = 0; i < arr.Length; i++)
            {
                cp.queue.Enqueue(long.Parse(arr[i]));
            }

            return cp;
        }

        internal void update(DateTime dt)
        {
            if (queue.Count >= PER_HOUR)
                queue.Dequeue();

            queue.Enqueue(dt.Ticks);
        }


        public override string ToString()
        {
            long[] ticks = queue.ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (long t in ticks)
            {
                sb.Append(t);
                sb.Append(",");
            }

            return sb.ToString().Trim(',');

        }

        /// <summary>
        /// 是否超限，超限则返回 true
        /// </summary>
        /// <returns></returns>
        internal bool isOver()
        {
            //队列数据大于设定值，且头的时间小于一小时，则认为超限
            if (queue.Count >= PER_HOUR)
            {
                if (RegCompact.LastSeconds(DateTime.Now.Ticks, queue.Peek()) < 3600)
                    return true;
            }
            return false;


        }

        /// <summary>
        /// 时间差，按秒
        /// </summary>
        /// <param name="big"></param>
        /// <param name="small"></param>
        /// <returns></returns>
        public static long LastSeconds(long big, long small)
        {
            return (big - small) / 10000000;
        }
    }
}
