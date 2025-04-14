namespace KarenKrill.TheLabyrinth.Movement.Abstractions
{
    public interface IPhysicMoveStrategy : IMoveStrategy
    {
        bool IsGrounded { get; }
    }
}
