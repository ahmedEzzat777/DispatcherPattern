using System;
using System.Threading;

namespace Dispatcher
{
    class StepActor : ActorBase
    {
        public StepActor(string name, int step = 1):base(name)
        {
            _counter = new StepCount(this, step);
        }
        public override void Decrement()
        {
            Console.WriteLine($"{_name} decrementing...");
            _counter.Decrement();
            Thread.Sleep(7000);
            Console.WriteLine($"{_name} decremented, count now = {_count}");
        }

        public override void Increment()
        {
            Console.WriteLine($"{_name} incrementing...");
            _counter.Increment();
            Thread.Sleep(7000);
            Console.WriteLine($"{_name} incremented, count now = {_count}");
        }
    }
}
