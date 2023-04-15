using Zenject;

namespace Core.Module
{
    public partial class MainMenu
    {
        public class Installer : Installer<Installer>
        {
            public override void InstallBindings()
            {
                Container.BindInterfacesTo<MainMenu>().AsSingle();
            }
        }
    }
}