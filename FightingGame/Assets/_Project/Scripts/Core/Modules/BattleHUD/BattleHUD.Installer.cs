using Zenject;

namespace Core.Module
{
    public partial class BattleHUD
    {
        public class Installer : Installer<Installer>
        {
            public override void InstallBindings()
            {
                Container.BindInterfacesTo<BattleHUD>().AsSingle();
            }
        }
    }
}