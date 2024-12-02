using Hall;

namespace Game.Console2048
{
    public class EntryCreator : GameEntryCreator<GamePack>
    {
        protected override GamePack CreateGamePack()
        {
            return new GamePack();
        }
    }
}