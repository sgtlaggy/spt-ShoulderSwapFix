using System;
using System.Reflection;

using EFT;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace ShoulderSwapFix
{
    [BepInPlugin("com.sgtlaggy.shoulderswapfix", "ShoulderSwapFix", "1.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> Enabled { get; set; }

        private void Awake()
        {
            Enabled = Config.Bind("General", "Enabled", true, "Keep weapon in left shoulder during most interactions.");
            Enabled.SettingChanged += TogglePatches;
            TogglePatches(Enabled, EventArgs.Empty);
        }

        private void TogglePatches(object sender, EventArgs e)
        {
            var setting = sender as ConfigEntry<bool>;
            if (setting == null)
            {
                return;
            }

            if (setting.Value)
            {
                new HandInteractionPatch().Enable();
                new InventoryPatch().Enable();
                new KnifePatch().Enable();
                new RangefinderPatch().Enable();
                new RadioTransmitterPatch().Enable();
                new GrenadePatch().Enable();
                new MedsPatch().Enable();
                new UsableItemPatch().Enable();
            }
            else
            {
                new HandInteractionPatch().Disable();
                new InventoryPatch().Disable();
                new KnifePatch().Disable();
                new RangefinderPatch().Disable();
                new RadioTransmitterPatch().Disable();
                new GrenadePatch().Disable();
                new MedsPatch().Disable();
                new UsableItemPatch().Disable();
            }
        }

        public static void DisableLeftStance(Player player)
        {
            player.MovementContext.LeftStanceController.SetLeftStanceForce(false);
        }
    }

    public class HandInteractionPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(LeftStanceController).GetMethod(nameof(LeftStanceController.DisableLeftStanceAnimFromHandsAction));
        }

        [PatchPrefix]
        private static bool Prefix()
        {
            return false;
        }
    }

    public class InventoryPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(LeftStanceController).GetMethod(nameof(LeftStanceController.DisableLeftStanceAnimFromOpenInventory));
        }

        [PatchPrefix]
        private static bool Prefix()
        {
            return false;
        }
    }

    public class RangefinderPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(PortableRangeFinderController.Class1173).GetMethod(nameof(PortableRangeFinderController.Class1173.SetLeftStanceAnimOnStartOperation));
        }

        [PatchPrefix]
        private static bool Prefix(Player ___player_0)
        {
            Plugin.DisableLeftStance(___player_0);
            return false;
        }
    }

    public class RadioTransmitterPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(RadioTransmitterController.Class1174).GetMethod(nameof(RadioTransmitterController.Class1174.SetLeftStanceAnimOnStartOperation));
        }

        [PatchPrefix]
        private static bool Prefix(Player ___player_0)
        {
            Plugin.DisableLeftStance(___player_0);
            return false;
        }
    }

    public class KnifePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(Player.KnifeController.Class1156).GetMethod(nameof(Player.KnifeController.Class1156.SetLeftStanceAnimOnStartOperation));
        }

        [PatchPrefix]
        private static bool Prefix(Player ___player_0)
        {
            Plugin.DisableLeftStance(___player_0);
            return false;
        }
    }

    public class GrenadePatch : ModulePatch
    {
        private static FieldInfo itemHandsControllerPlayer;

        protected override MethodBase GetTargetMethod()
        {
            itemHandsControllerPlayer = AccessTools.Field(typeof(Player.ItemHandsController), "_player");
            return typeof(Player.GrenadeHandsController.Class1147).GetMethod(nameof(Player.GrenadeHandsController.Class1147.SetLeftStanceAnimOnStartOperation));
        }

        [PatchPrefix]
        private static bool Prefix(Player.ItemHandsController ___gparam_0)
        {
            Player player = (Player)itemHandsControllerPlayer.GetValue(___gparam_0);
            Plugin.DisableLeftStance(player);
            return false;
        }
    }

    public class MedsPatch : ModulePatch
    {
        private static FieldInfo medsControllerPlayer;

        protected override MethodBase GetTargetMethod()
        {
            medsControllerPlayer = AccessTools.Field(typeof(Player.MedsController), "_player");
            return typeof(Player.MedsController.Class1158).GetMethod(nameof(Player.MedsController.Class1158.SetLeftStanceAnimOnStartOperation));
        }

        [PatchPrefix]
        private static bool Prefix(Player.MedsController ___medsController_0)
        {
            Player player = (Player)medsControllerPlayer.GetValue(___medsController_0);
            Plugin.DisableLeftStance(player);
            return false;
        }
    }

    public class UsableItemPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(Player.UsableItemController.Class1172).GetMethod(nameof(Player.UsableItemController.Class1172.SetLeftStanceAnimOnStartOperation));
        }

        [PatchPrefix]
        private static bool Prefix(Player ___player_0)
        {
            Plugin.DisableLeftStance(___player_0);
            return false;
        }
    }
}
