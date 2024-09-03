using System.Reflection;

using EFT;
using BepInEx;
using SPT.Reflection.Patching;

namespace ShoulderSwapFix
{
    [BepInPlugin("com.sgtlaggy.shoulderswapfix", "ShoulderSwapFix", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            new HandInteractionPatch().Enable();
            new InventoryPatch().Enable();
            new KnifePatch().Enable();
            new RangefinderPatch().Enable();
            new RadioTransmitterPatch().Enable();
        }
    }

    public class HandInteractionPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(LeftStanceController).GetMethod("DisableLeftStanceAnimFromHandsAction");
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
            return typeof(LeftStanceController).GetMethod("DisableLeftStanceAnimFromOpenInventory");
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
            return typeof(PortableRangeFinderController.Class1079).GetMethod("SetLeftStanceAnimOnStartOperation");
        }

        [PatchPrefix]
        private static bool Prefix(Player ___player_0)
        {
            ___player_0.MovementContext.LeftStanceController.SetLeftStanceForce(false);
            return false;
        }
    }

    public class RadioTransmitterPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(RadioTransmitterController.Class1080).GetMethod("SetLeftStanceAnimOnStartOperation");
        }

        [PatchPrefix]
        private static bool Prefix(Player ___player_0)
        {
            ___player_0.MovementContext.LeftStanceController.SetLeftStanceForce(false);
            return false;
        }
    }

    public class KnifePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(Player.KnifeController.Class1062).GetMethod("SetLeftStanceAnimOnStartOperation");
        }

        [PatchPrefix]
        private static bool Prefix(Player ___player_0)
        {
            ___player_0.MovementContext.LeftStanceController.SetLeftStanceForce(false);
            return false;
        }
    }
}
