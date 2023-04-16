using Zenject;

namespace Core.Module
{
    public partial class CharacterToggleMenu
    {
        public class Installer : Installer<Installer>
        {
            public override void InstallBindings()
            {
                Container.BindInterfacesTo<CharacterToggleMenu>().AsSingle();
            }
        }
    }
}