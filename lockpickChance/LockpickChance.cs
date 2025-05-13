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

        private void DebugLog(string message)
        {
            if (Configuration.Instance.Logging)
            {
                Logger.Log(message);
            }
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
                        resultMessage = $"<b><color=#00FFFF>Lockpicking </color><color=green>Succeeded</color><color=#00FFFF>, Success chance {entry.LockpickChance}%!</color></b>";
                        UnturnedChat.Say(uPlayer, resultMessage, true);

                        DebugLog($"{uPlayer.CharacterName} succeeded at lockpicking with Stealy [{itemId}]. Roll: {roll:F2}. Chance: {entry.LockpickChance}.");
                    }
                    else
                    {
                        allow = false;
                        resultMessage = $"<b><color=#00FFFF>Lockpicking </color><color=red>Failed</color><color=#00FFFF>, Success chance {entry.LockpickChance}%!</color></b>";
                        UnturnedChat.Say(uPlayer, resultMessage, true);

                        bool itemFound = false;
                        for (byte page = 0; page < 10; page++) 
                        {
                            for (byte index = 0; index < 30; index++) 
                            {
                                var itemJar = uPlayer.Player.inventory.getItem(page, index);

                                if (itemJar != null && itemJar.item != null && itemJar.item.id == itemId) 
                                {
                                    uPlayer.Player.inventory.removeItem(page, index);
                                    itemFound = true;

                                    DebugLog($"{uPlayer.CharacterName} failed lockpicking with Stealy [{itemId}]. Roll: {roll:F2}. Chance: {entry.LockpickChance}. Item removed from inventory.");
                                }
                            }

                            if (itemFound)
                                break;
                        }

                        if (!itemFound)
                        {
                            DebugLog($"Failed to find Stealy Wheely [{itemId}] in {uPlayer.CharacterName}'s inventory.");
                        }
                    }

                    return;
                }
            }

            allow = false;
            UnturnedChat.Say(uPlayer, "This item cannot be used to pick locks!", Color.red);
        }
    }
}
