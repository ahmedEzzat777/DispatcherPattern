namespace Dispatcher
{
    public abstract class ActorBase
    {
        public enum CountType
        {
            Default,
            Step
        }

        public enum OperationType 
        { 
            Increment,
            Decrement
        }

        protected class DefaultCount
        {
            protected readonly ActorBase _actor;

            public DefaultCount(ActorBase actor)
            {
                _actor = actor;
            }

            public virtual void Increment()
            {
                _actor._count++;
            }

            public virtual void Decrement()
            {
                _actor._count--;
            }
        }

        protected class StepCount : DefaultCount
        {
            private readonly int _step;

            public StepCount(ActorBase actor, int step) :base(actor)
            {
                _step = step;
            }

            public override void Increment()
            {
                _actor._count += _step;
            }

            public override void Decrement()
            {
                _actor._count -= _step;
            }
        }

        protected string _name;
        protected int _count;
        protected DefaultCount _counter;

        public ActorBase(string name)
        {
            _name = name;
            _count = 0;
        }

        public abstract void Increment();
        public abstract void Decrement();
        public int GetCount() => _count;
    }
}
