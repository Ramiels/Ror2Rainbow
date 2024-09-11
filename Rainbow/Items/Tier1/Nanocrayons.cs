using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rainbow.Items
{
	public class Nanocrayons : ItemBase
	{
		public override string ItemLangTokenName	=> "Nanocrayons";
		public override string ItemName				=> "ITEM_Nanocrayons_NAME";
		public override string ItemPickupDesc		=> "ITEM_Nanocrayons_PICKUP";
		public override string ItemFullDescription	=> "ITEM_Nanocrayons_DESC";
		public override string ItemLore				=> "ITEM_Nanocrayons_LORE";
		public override ItemTier Tier				=> ItemTier.Tier1;
		public override ItemTag[] ItemTags			=> [];
		public override GameObject ItemModel		=> Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();
		public override Sprite ItemIcon				=> Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
		public override ItemDisplayRuleDict CreateItemDisplayRules() {
			// One day...
			var rules = new ItemDisplayRuleDict();
			return rules;
		}

		// Hook shit
		public override void Hooks() {
			// Grant +5% HP per stack
			RecalculateStatsAPI.GetStatCoefficients += (sender, args) => {
				if (sender && sender.inventory) {
					int count = sender.inventory.GetItemCount(ItemDef);
					if (count > 0) {
						args.healthMultAdd += count * 0.05f;
					}
				}
			};

			// Turn into a green item if you have 4+ stacks
			// TODO: V/S-FX
			Inventory.onServerItemGiven += (inventory, item, count) => {
				var playerCount = inventory.GetItemCount(ItemDef);
				if (item == ItemDef.itemIndex && playerCount >= 4)	{
					int remainder;
					int quotient = Math.DivRem(playerCount, 4, out remainder);
					inventory.RemoveItem(ItemDef, playerCount - remainder);
					ItemDef[] greenList = ItemCatalog.allItemDefs.Where(itemDef => itemDef.tier == ItemTier.Tier2).ToArray();
					while (quotient > 0) {
						quotient--;
						inventory.GiveItem(greenList[Main.Rand.Next(0, greenList.Length)]);
					}
				}
			};
		}

		// Initialize the item
		public override void Init() {
			CreateLang();
			CreateItem();
			Hooks();
		}
	}
}
