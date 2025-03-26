using KarenKrill.Common.UI.Views.Abstractions;

namespace KarenKrill.Common.UI.Presenters.Abstractions
{
    public interface IPresenter
    {
        void Enable();
        void Disable();
    }
    public interface IPresenter<T> : IPresenter where T : IUserInterfaceView
    {
        T View { get; }
    }
}