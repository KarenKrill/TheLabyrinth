using KarenKrill.Core.UI.Views;

namespace KarenKrill.Core.UI.Presenters
{
    public interface IPresenter
    {
        void Enable();
        void Disable();
    }
    public interface IPresenter<T> where T : IUserInterfaceView
    {
        T View { get; }
        void Enable();
        void Disable();
    }
}