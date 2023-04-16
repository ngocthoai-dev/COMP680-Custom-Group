using Zenject;

namespace Core.Module
{
    public partial class SettingsMenu
    {
        public class Installer : Installer<Installer>
        {
            public override void InstallBindings()
            {
                Container.BindInterfacesTo<SettingsMenu>().AsSingle();
            }
        }
    }
}