using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    public class baseClass
    {
        public baseClass(int c, char s)
        {
            _c = c;
            _s = s;
        }
        public int _c;
        public char _s;
        public virtual void SayHi()
        {
            Console.WriteLine("base hi");
        }
    }

    public interface Itest
    {
        void SayHi();

    }

    abstract class Atest2
    {
        public abstract void SayHi();
    }

    class test2 : Atest2
    {
        public override void SayHi()
        {
            Console.WriteLine("test2 hi");
        }
    }

    class test : Itest
    {
        public void SayHi()
        {
            Console.WriteLine("test hi");
        }
    }
    public class childClass : baseClass
    {
        public childClass(int d, char s,int c,char bs):base(c,bs)
        {
            _d = d;
            _s = s;
        }
        public int _d;
       
        public new char _s;

        public new void SayHi()
        {
            Console.WriteLine("child hi");
        }
    }
    public class Program
    {

        private async static Task Say()
        {
            var t = await TestAsync();
            var t1 = await TestAsync1();
            Thread.Sleep(1000);
            Console.WriteLine(DateTime.Now + "main thread" + "threadId:{0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine(DateTime.Now + t + DateTime.Now + "threadId:{0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine(DateTime.Now + t1 + DateTime.Now + "threadId:{0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Say end ,threadId:{0}", Thread.CurrentThread.ManagedThreadId);
        }

        private async static Task<string> TestAsync()
        {
            Console.WriteLine(DateTime.Now + "who ami :{0}", Thread.CurrentThread.ManagedThreadId);
            string content = await Task.Run<string>(() =>
            {
                Console.WriteLine(DateTime.Now + "child hello " + "threadId:{0}", Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(4000);
                Console.WriteLine(DateTime.Now + "xxxxxxxxxxxxxxxxxxxxxx " + "threadId:{0}", Thread.CurrentThread.ManagedThreadId);
                return "hello Task";
            });
            Console.WriteLine(DateTime.Now + "testasync finish" + "threadId:{0}", Thread.CurrentThread.ManagedThreadId);
            return content;
        }

        private async static Task<string> TestAsync1()
        {
            Console.WriteLine(DateTime.Now + "who ami1 :{0}", Thread.CurrentThread.ManagedThreadId);
            string content = await Task.Run<string>(() =>
            {
                Console.WriteLine(DateTime.Now + "child hello1 " + "threadId:{0}", Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(12000);
                Console.WriteLine(DateTime.Now + "sssssssssssssssssssssssss " + "threadId:{0}", Thread.CurrentThread.ManagedThreadId);
                return "hello Task1";
            });
            Console.WriteLine(DateTime.Now + "testasync1 finish" + "threadId:{0}", Thread.CurrentThread.ManagedThreadId);
            return content;
        }



        public static void Main(string[] args)
        {
            //Console.WriteLine("main thread id is {0}", Thread.CurrentThread.ManagedThreadId);
            //Say();
            //Console.WriteLine(DateTime.Now + "main process" + "threadId:{0}", Thread.CurrentThread.ManagedThreadId);
            baseClass obj1 = new childClass(1,'1',2,'2');
            obj1.SayHi();

            Console.ReadKey();
        }
    }
}
