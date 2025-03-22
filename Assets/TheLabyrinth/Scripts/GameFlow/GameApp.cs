namespace KarenKrill.TheLabyrinth.GameFlow
{
    using Abstractions;

    public class GameApp : IGameApp
    {
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}