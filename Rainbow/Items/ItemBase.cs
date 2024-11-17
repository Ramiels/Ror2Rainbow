using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;
using System.Collections.Generic;

namespace Rainbow.Items
{
	public abstract class ItemBase
	{
		// Localization data
		public abstract string ItemLangTokenName { get; }
		// Item Data
		public abstract ItemTier Tier { get; }
		public virtual ItemTag[] ItemTags { get; } = [];
		public virtual bool CanRemove { get; } = true;
		public virtual bool Hidden { get; } = false;
        public ItemDef ItemDef;
		public static Dictionary<string, ItemDef> ItemDefs = new Dictionary<string, ItemDef>();
        // Graphics data
        public abstract GameObject ItemModel { get; }
		public abstract Sprite ItemIcon { get; }
		public abstract ItemDisplayRuleDict CreateItemDisplayRules();
		// Init func
		public abstract void Init();


		// Create item
		protected void CreateItem()
		{
			// Create the itemdef and populate it with data
			var ItemDef						= ScriptableObject.CreateInstance<ItemDef>();
			ItemDef.name				= ItemLangTokenName;
			ItemDef.nameToken			= "ITEM_" + ItemLangTokenName + "_NAME";
			ItemDef.pickupToken			= "ITEM_" + ItemLangTokenName + "_PICKUP";
			ItemDef.descriptionToken	= "ITEM_" + ItemLangTokenName + "_DESC";
			ItemDef.loreToken			= "ITEM_" + ItemLangTokenName + "_LORE";
			ItemDef.pickupModelPrefab	= ItemModel;
			ItemDef.pickupIconSprite	= ItemIcon;
			ItemDef.tier				= Tier;
			ItemDef.hidden				= false;
			ItemDef.canRemove			= CanRemove;
			ItemDef.tags				= ItemTags;
			// Gross haxx, but possibly necessary
			#pragma warning disable CS0618 // Type or member is obsolete
			ItemDef.deprecatedTier		= Tier;
			#pragma warning restore CS0618 // Type or member is obsolete

			var itemDisplayRuleDict		= CreateItemDisplayRules();
			// Add the item with our data
			ItemAPI.Add(new CustomItem(ItemDef, itemDisplayRuleDict));

			ItemDefs.Add(ItemLangTokenName, ItemDef);

		}
		// Hooks
		public abstract void Hooks();
		// Item Count funcs
		public int GetCount(CharacterBody body)
		{
			if (!body || !body.inventory) { return 0; }
			return body.inventory.GetItemCount(ItemDef);
		}
		public int GetCount(CharacterMaster master)
		{
			if (!master || !master.inventory) { return 0; }
			return master.inventory.GetItemCount(ItemDef);
		}
	}
}
