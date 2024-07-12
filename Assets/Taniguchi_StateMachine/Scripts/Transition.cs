namespace Platformer
{
    public class Transition : ITransition
    {
        public MyState To { get; }

        public IPredicate Condition { get; }

        public Transition(MyState to,IPredicate condition)
        {
            To = to;
            Condition = condition;
        }
    }
}
