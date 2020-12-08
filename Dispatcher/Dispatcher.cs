using System;
using System.Collections.Generic;
using System.Threading;
using static Dispatcher.ActorBase;

namespace Dispatcher
{
    public class Dispatcher
    {
        private class ActionWrapper 
        {
            private readonly Dispatcher _dispatcher;
            private readonly string _actorName;
            private readonly Action _action;

            public ActionWrapper(Dispatcher dispatcher, string actorName, Action action)
            {
                _dispatcher = dispatcher;
                _actorName = actorName;
                _action = action;
            }

            public void DoAction()
            {
                _action?.Invoke();
                Queue<Action> queue;

                lock (_dispatcher._actorsActions[_actorName])
                {
                    queue = _dispatcher._actorsActions[_actorName];

                    foreach (var pending in queue)
                        pending?.Invoke();

                    queue.Clear();
                }

                lock (_dispatcher._actorRunningState)
                {
                    _dispatcher._actorRunningState[_actorName] = false;
                }
            }

            public void DoPendingAction()
            {
                _action?.Invoke();
            }
        }

        Dictionary<string, ActorBase> _actors;
        Dictionary<string, Queue<Action>> _actorsActions;
        Dictionary<string, bool> _actorRunningState;
        List<Thread> _threads = new List<Thread>();

        public Dispatcher()
        {
            _actors = new Dictionary<string, ActorBase>();
            _actorsActions = new Dictionary<string, Queue<Action>>();
            _actorRunningState = new Dictionary<string, bool>();
        }

        public string CreateActor(string[] parameters)
        {
            //first type, second increment value if exists
            ActorBase actor;
            var name = new Random().Next().ToString();

            switch ((CountType)int.Parse(parameters[0]))
            {
                case CountType.Default:
                    actor = new Actor(name);
                    break;
                case CountType.Step:
                    actor = new StepActor(name, int.Parse(parameters[1]));
                    break;
                default:
                    throw new InvalidOperationException();
            }

            _actors.Add(name, actor);

            lock (_actorsActions)
            { 
                _actorsActions.Add(name, new Queue<Action>());
            }

            lock (_actorRunningState)
            { 
                _actorRunningState.Add(name, false);
            }

            Console.WriteLine($"actor created with name {name}");
            return name;
        }

        public void InvokeAction(string actorName, string operationType) 
        {
            ActionWrapper action;

            var actor = _actors[actorName];

            if (actor == null)
            { 
                Console.WriteLine("Invalid Actor Name");
                return;
            }

            switch ((OperationType)int.Parse(operationType))
            {
                case OperationType.Increment:
                    action = new ActionWrapper(this, actorName, actor.Increment);
                    break;
                case OperationType.Decrement:
                    action = new ActionWrapper(this, actorName, actor.Decrement);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            var actorState = false;

            lock (_actorRunningState)
            {
                actorState = _actorRunningState[actorName];
            }

            if (actorState) //running
            {
                lock (_actorsActions[actorName])
                {
                    _actorsActions[actorName].Enqueue(action.DoPendingAction);
                }
            }
            else 
            {
                lock (_actorRunningState)
                {
                    _actorRunningState[actorName] = true;
                }
                var thread = new Thread(() => action.DoAction());
                thread.Start();
                _threads.Add(thread);
            }

            Console.WriteLine($"Added {(OperationType)int.Parse(operationType)} to actor {actorName}");
        }

        public void Wait() => _threads.ForEach(a => a.Join());

    }
}
