using System;
using System.Threading;

namespace Dispatcher
{
    public class Actor : ActorBase
    {
        public Actor(string name) : base(name)
        {
            _counter = new DefaultCount(this);
        }

        public override void Decrement()
        {
            Console.WriteLine($"{_name} decrementing...");
            _counter.Decrement();
            Thread.Sleep(5000);
            Console.WriteLine($"{_name} decremented, count now = {_count}");
        }

        public override void Increment()
        {
            Console.WriteLine($"{_name} incrementing...");
            _counter.Increment();
            Thread.Sleep(5000);
            Console.WriteLine($"{_name} incremented, count now = {_count}");
        }
    }
}
