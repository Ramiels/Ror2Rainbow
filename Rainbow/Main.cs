using BepInEx;
using R2API;
using RoR2;
using R2API.Utils;
using UnityEngine;
using System.Reflection;
using Rainbow.Items;
using System.Collections.Generic;
using System.Linq;
using System;
[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace Rainbow
{
    // Does.. something
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

        public const string PluginGUID		= PluginAuthor + "." + PluginName;
        public const string PluginAuthor	= "CopiDev";
        public const string PluginName		= "Rainbow";
        public const string PluginVersion	= "0.1.0";

		public List<ItemBase> Items = new();
		public static System.Random Rand = new();

		public void Awake()
        {
            Log.Init(Logger); // Init our logging class so that we can properly log for debugging

			Log.Info("----------------------ITEMS--------------------");
			// Some code sorcery I divined and conjured into my codebase.
			var ItemTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ItemBase)));
			foreach (var itemType in ItemTypes)	{
				ItemBase item = (ItemBase)System.Activator.CreateInstance(itemType);
				item.Init();
				//Log.Info("Item: " + item.ItemName + " Initialized!");
			}

			// Set our RNG per run
			Run.onRunStartGlobal += (run) => {
				Rand = new System.Random((int)(Run.instance.seed % int.MaxValue));
				// Log.Info("Run started with seed " + Run.instance.seed);
			};
		}
	}
}
