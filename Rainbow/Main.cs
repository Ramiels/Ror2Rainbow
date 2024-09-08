using BepInEx;
using R2API;
using RoR2;
using R2API.Utils;
using UnityEngine;
using System.Reflection;
using Rainbow.Items;
using System.Collections.Generic;
using System.Linq;

namespace Rainbow
{
    // Add Dependecies I think?
    [BepInDependency(ItemAPI.PluginGUID)]
    [BepInDependency(LanguageAPI.PluginGUID)]
    [BepInDependency(RecalculateStatsAPI.PluginGUID)]
    // Plugin Data
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    // Network Compat Data
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]

    public class Main : BaseUnityPlugin
    {

        /* PluginVersion formatting! Don't be a dumbfuck and forget to update this!!
        MAJOR version when you either complete your development milestones, or make extreme changes to your mod.
        MINOR version when you are adding minor features (like another item for instance)
        PATCH version increment when you're patching bugs, doing small tweaks, polishing assets, etc.
        */

        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "CopiDev";
        public const string PluginName = "Rainbow";
        public const string PluginVersion = "0.1.0";



		public List<ItemBase> Items = new List<ItemBase>();


		public void Awake()
        {
            Log.Init(Logger); // Init our logging class so that we can properly log for debugging

			Log.Info("----------------------ITEMS--------------------");
			//Item Initialization
			// Some code sorcery I divined and conjured into my codebase.
			var ItemTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ItemBase)));
			foreach (var itemType in ItemTypes)
			{
				ItemBase item = (ItemBase)System.Activator.CreateInstance(itemType);
				Log.Info("Item: " + item.ItemName + " Pre-Init!");
				item.Init();
				Log.Info("Item: " + item.ItemName + " Initialized!");
			}

        }

		// The Update() method is run on every frame of the game.
		private void Update()
		{
			/*
			// This if statement checks if the player has currently pressed F2.
			if (Input.GetKeyDown(KeyCode.F2))
			{
				// Get the player body to use a position:
				var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

				// And then drop our defined item in front of the player.

				Log.Info($"Player pressed F2. Spawning our custom item at coordinates {transform.position}");

				var ItemTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ItemBase)));
				foreach (var itemType in ItemTypes)
				{
					ItemBase item = (ItemBase)System.Activator.CreateInstance(itemType);
					item.Init();
					PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(item.ItemDef.itemIndex), transform.position, transform.forward * 20f);
					Log.Info("Item: " + item.ItemName + " Initialized!");
				}

			}*/
		}
	}
}
