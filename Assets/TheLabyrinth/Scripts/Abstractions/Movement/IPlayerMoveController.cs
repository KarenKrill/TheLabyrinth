using KarenKrill.Strategies.Abstractions;

namespace TheLabyrinth.Movement.Abstractions
{
    public interface IPlayerMoveController : IStrategical<IMoveStrategy>
    {
        IMoveStrategy MoveStrategy { get; set; }
    }
}
