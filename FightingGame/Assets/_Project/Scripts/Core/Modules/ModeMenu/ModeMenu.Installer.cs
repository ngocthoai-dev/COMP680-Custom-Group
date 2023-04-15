using Zenject;

namespace Core.Module
{
    public partial class ModeMenu
    {
        public class Installer : Installer<Installer>
        {
            public override void InstallBindings()
            {
                Container.BindInterfacesTo<ModeMenu>().AsSingle();
            }
        }
    }
}