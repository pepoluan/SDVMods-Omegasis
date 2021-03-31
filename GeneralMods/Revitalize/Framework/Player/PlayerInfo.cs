using Revitalize.Framework.Player.Managers;

namespace Revitalize.Framework.Player
{
    public class PlayerInfo
    {
        public MagicManager magicManager;
        public bool justPlacedACustomObject;

        public PlayerInfo()
        {
            this.magicManager = new MagicManager();
        }

        public void update()
        {
        }
    }
}
