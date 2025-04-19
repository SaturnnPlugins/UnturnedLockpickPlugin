using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Rocket.Core.Logging;
using Logger = Rocket.Core.Logging.Logger;
using Rocket.Unturned.Chat;

namespace lockpickChance
{
    public class StealyChance : RocketPlugin<LockpickChanceConfiguration>
    {
        public static StealyChance Instance;

        protected override void Load()
        {
            Instance = this;
            VehicleManager.onVehicleLockpicked += OnVehicleLockpicked;
            Logger.Log("StealyChance plugin loaded.");
        }

        protected override void Unload()
        {
            VehicleManager.onVehicleLockpicked -= OnVehicleLockpicked;
            Logger.Log("StealyChance plugin unloaded.");
        }

        private void OnVehicleLockpicked(InteractableVehicle vehicle, Player instigatingPlayer, ref bool allow)
        {
            var uPlayer = UnturnedPlayer.FromPlayer(instigatingPlayer);
            ushort itemId = uPlayer.Player.equipment.itemID;

            foreach (var entry in Configuration.Instance.Stealies)
            {
                if (entry.ItemId == itemId)
                {
                    float roll = Random.Range(0f, 100f);
                    string resultMessage;

                    if (roll <= entry.LockpickChance)
                    {
                        allow = true;
                        resultMessage = $"Lockpicking Succeeded, Success chance {entry.LockpickChance}%!";
                        UnturnedChat.Say(uPlayer, resultMessage, Color.green);
                        if (Configuration.Instance.Logging)
                            Logger.Log($"{uPlayer.CharacterName} succeeded at lockpicking with Stealy [{itemId}]. Roll: {roll:F2}. Chance: {entry.LockpickChance}.");
                    }
                    else
                    {
                        allow = false;
                        resultMessage = $"Lockpicking Failed, Success chance {entry.LockpickChance}%!";
                        UnturnedChat.Say(uPlayer, resultMessage, Color.red);
                        if (Configuration.Instance.Logging)
                            Logger.Log($"{uPlayer.CharacterName} failed lockpicking with Stealy [{itemId}]. Roll: {roll:F2}. Chance: {entry.LockpickChance}.");
                    }

                    return;
                }
            }

            allow = false;
            UnturnedChat.Say(uPlayer, "This item cannot be used to pick locks!", Color.red);
        }
    }
}
