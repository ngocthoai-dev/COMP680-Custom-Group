using Zenject;

namespace Core.Module
{
    public partial class OptionsMenu
    {
        public class Installer : Installer<Installer>
        {
            public override void InstallBindings()
            {
                Container.BindInterfacesTo<OptionsMenu>().AsSingle();
            }
        }
    }
}