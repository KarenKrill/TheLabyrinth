namespace KarenKrill.Common.UI.Views.Abstractions
{
    public interface IUserInterfaceFactory
    {
        public UIViewType Create<UIViewType>()
            where UIViewType : class;
    }
}