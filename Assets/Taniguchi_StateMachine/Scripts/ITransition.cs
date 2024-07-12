namespace Platformer
{
    public interface ITransition
    {
        MyState To { get; }
        IPredicate Condition { get; }
    }
}
