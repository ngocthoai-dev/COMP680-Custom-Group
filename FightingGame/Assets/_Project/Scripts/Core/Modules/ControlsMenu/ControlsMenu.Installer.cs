using Zenject;

namespace Core.Module
{
    public partial class ControlsMenu
    {
        public class Installer : Installer<Installer>
        {
            public override void InstallBindings()
            {
                Container.BindInterfacesTo<ControlsMenu>().AsSingle();
            }
        }
    }
}