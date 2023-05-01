using Core.Business;
using Core.Gameplay;
using Core.GGPO;
using Core.SO;

namespace Core.EventSignal
{
    public class GameActionSignal<TModel>
        where TModel : IModuleContextModel
    {
        public TModel NewModel { get; private set; }
        public GameAction Action { get; private set; }

        public GameActionSignal(GameAction action, TModel newModel)
        {
            if (newModel == null)
                throw new GameActionModelIsNull();

            Action = action;
            NewModel = newModel;
        }

        public class GameActionModelIsNull : System.Exception
        { }
    }

    public enum GameAction
    {
    }

    public class OnSyncBattleHUD
    {
        public NetworkCharacter[] NetworkCharacters { get; private set; }

        public OnSyncBattleHUD(NetworkCharacter[] networkCharacters)
        {
            NetworkCharacters = networkCharacters;
        }
    }

    public class OnEndBattle
    {
        public bool IsWin { get; private set; }

        public OnEndBattle(bool isWin)
        {
            IsWin = isWin;
        }
    }
}