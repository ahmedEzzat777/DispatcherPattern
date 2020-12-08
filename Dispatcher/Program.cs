using System;

namespace Dispatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Dispatcher system started!\nPress Any Key To Start!");
            Console.ReadKey();

            var dispatcher = new Dispatcher();

            var actorName1 = dispatcher.CreateActor(new[]{ "0" });
            dispatcher.InvokeAction(actorName1, "0");
            dispatcher.InvokeAction(actorName1, "1");

            var actorName2 = dispatcher.CreateActor(new[] { "1", "2" });

            dispatcher.InvokeAction(actorName1, "0");
            dispatcher.InvokeAction(actorName2, "0");
            dispatcher.InvokeAction(actorName2, "0");
            dispatcher.InvokeAction(actorName2, "1");

            dispatcher.Wait();
        }
    }
}
