namespace KarenKrill.Core.UI
{
    public interface IUserInterfaceFactory
    {
        public UIViewType Create<UIViewType>()
            where UIViewType : class;
    }
}