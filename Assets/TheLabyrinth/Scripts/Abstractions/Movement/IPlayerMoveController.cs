namespace KarenKrill.TheLabyrinth.Movement.Abstractions
{
    using Common.Strategies.Abstractions;
    public interface IPlayerMoveController : IStrategical<IMoveStrategy>
    {
        IMoveStrategy MoveStrategy { get; set; }
    }
}
