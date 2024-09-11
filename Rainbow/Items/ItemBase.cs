using R2API;
using RoR2;
using UnityEngine;
using BepInEx.Configuration;

namespace Rainbow.Items
{
	public abstract class ItemBase
	{
		// Localization data
		public abstract string ItemLangTokenName { get; }
		public abstract string ItemName { get; }
		public abstract string ItemPickupDesc { get; }
		public abstract string ItemFullDescription { get; }
		public abstract string ItemLore { get; }
		// Create localizations
		protected void CreateLang()
		{
			LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_NAME", ItemName);
			Log.Debug("ITEM_" + ItemLangTokenName + "_NAME");
			LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_PICKUP", ItemPickupDesc);
			Log.Debug("ITEM_" + ItemLangTokenName + "_PICKUP");
			LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_DESCRIPTION", ItemFullDescription);
			Log.Debug("ITEM_" + ItemLangTokenName + "_DESCRIPTION");
			LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_LORE", ItemLore);
			Log.Debug("ITEM_" + ItemLangTokenName + "_LORE");
		}
		// Item Data
		public abstract ItemTier Tier { get; }
		public virtual ItemTag[] ItemTags { get; } = [];
		public virtual bool CanRemove { get; } = true;
		public virtual bool Hidden { get; } = false;
		public ItemDef ItemDef;
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
			ItemDef						= ScriptableObject.CreateInstance<ItemDef>();
			ItemDef.name				= ItemLangTokenName;
			ItemDef.nameToken			= "ITEM_" + ItemLangTokenName + "_NAME";
			ItemDef.pickupToken			= "ITEM_" + ItemLangTokenName + "_PICKUP";
			ItemDef.descriptionToken	= "ITEM_" + ItemLangTokenName + "_DESCRIPTION";
			ItemDef.loreToken			= "ITEM_" + ItemLangTokenName + "_LORE";
			ItemDef.pickupModelPrefab	= ItemModel;
			ItemDef.pickupIconSprite	= ItemIcon;
			ItemDef.hidden				= false;
			ItemDef.canRemove			= CanRemove;
			ItemDef.tier				= Tier;
			ItemDef.tags				= ItemTags;
			var itemDisplayRuleDict		= CreateItemDisplayRules();
			// Add the item with our data
			ItemAPI.Add(new CustomItem(ItemDef, itemDisplayRuleDict));
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
