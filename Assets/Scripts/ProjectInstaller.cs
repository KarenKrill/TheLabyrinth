using KarenKrill.Logging;
using UnityEngine;
using Zenject;

namespace KarenKrill
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
#if DEBUG
            Container.Bind<ILogger>().To<Logger>().FromNew().AsSingle().WithArguments(new DebugLogHandler());
#else
            Container.Bind<ILogger>().To<StubLogger>().FromNew().AsSingle();
#endif
        }
    }
}