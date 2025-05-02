namespace KarenKrill.TheLabyrinth.Movement.Abstractions
{
    using Strategies.Abstractions;
    public interface IPlayerMoveController : IStrategical<IMoveStrategy>
    {
        IMoveStrategy MoveStrategy { get; set; }
    }
}
