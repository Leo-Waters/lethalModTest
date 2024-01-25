using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalLib.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace TestMod
{
    [BepInPlugin(ModGUID, ModName,ModVersion)]
    public class TestModPatch : BaseUnityPlugin
    {
        const string ModGUID="lh.testmod";
        const string ModName="testmod";
        const string ModVersion="1.0.0.0";

        private readonly Harmony harmony = new Harmony(ModGUID);

        private static TestModPatch Instance;

        internal ManualLogSource logger;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            logger = BepInEx.Logging.Logger.CreateLogSource(ModGUID);

            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "dazblock");
            AssetBundle bundle = AssetBundle.LoadFromFile(assetDir);

            //items
            Item dazBlock = bundle.LoadAsset<Item>("Assets/DazBlockMod/dazblock.asset");
            Item hoggBlock = bundle.LoadAsset<Item>("Assets/DazBlockMod/hoggblock.asset");


            //register to network
            NetworkPrefabs.RegisterNetworkPrefab(dazBlock.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(hoggBlock.spawnPrefab);

            //fix audio
            Utilities.FixMixerGroups(dazBlock.spawnPrefab);
            Utilities.FixMixerGroups(hoggBlock.spawnPrefab);

            //registers as scarp
            Items.RegisterScrap(dazBlock, 1000, Levels.LevelTypes.All);
            Items.RegisterScrap(hoggBlock, 1000, Levels.LevelTypes.All);


            //TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            //node.clearPreviousText = true;
            //node.displayText = "Daz block\n\n";
            //Items.RegisterShopItem(dazBlock, null, null, node, 0);



            logger.LogInfo(ModName+" has initalized");
        }


    }
}
