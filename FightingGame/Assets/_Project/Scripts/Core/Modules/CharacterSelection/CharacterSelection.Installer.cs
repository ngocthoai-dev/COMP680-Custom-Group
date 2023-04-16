using Zenject;

namespace Core.Module
{
    public partial class CharacterSelection
    {
        public class Installer : Installer<Installer>
        {
            public override void InstallBindings()
            {
                Container.BindInterfacesTo<CharacterSelection>().AsSingle();
            }
        }
    }
}