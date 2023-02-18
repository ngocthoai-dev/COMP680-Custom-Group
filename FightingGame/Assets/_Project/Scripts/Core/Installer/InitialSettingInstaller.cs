using UnityEngine;
using Zenject;

namespace Core
{
    [CreateAssetMenu(fileName = "InitialSetting", menuName = "Configs/InitialConfig", order = 1)]
    public class InitialSettingInstaller : ScriptableObjectInstaller<InitialSettingInstaller>
    {
        public GameStore.Setting GameSetting;
        public GameStore.Atlas Atlas;

        public override void InstallBindings()
        {
            Container.BindInstance(GameSetting);
            Container.BindInstance(Atlas);
        }
    }
}