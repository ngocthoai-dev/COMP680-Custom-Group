using Zenject;

namespace Core.Module
{
    public partial class AboutMenu
    {
        public class Installer : Installer<Installer>
        {
            public override void InstallBindings()
            {
                Container.BindInterfacesTo<AboutMenu>().AsSingle();
            }
        }
    }
}