namespace Core.Business
{
    public enum ScreenName
    {
        SessionStart = 0,
        Restart,
    }

    public enum ModuleName
    {
        Dummy,
        DummyUTKit,
        
        MainMenu,
        OptionsMenu,
        CharacterToggleMenu,
        SettingsMenu,
        ModeMenu,
        CharacterSelection,
        AboutMenu,
        ControlsMenu,
        BattleHUD,
        BattleResult
    }

    public enum ViewName
    {
        Unity
    }

    public enum BundleLoaderName
    {
        Resource,
        Addressable
    }

    public enum PoolName
    {
        Object,
        Audio
    }

    public enum DefinitionLocation
    {
        Local,
        Remote
    }
}