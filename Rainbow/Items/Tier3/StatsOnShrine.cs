using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnityEngine.UIElements.TextureRegistry;

namespace Rainbow.Items
{
	public class StatsOnShrine : ItemBase
	{
		public override string ItemLangTokenName	=> "StatsOnShrine";
		public override ItemTier Tier				=> ItemTier.Tier3;
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

			// Track accrued stats
			CharacterMaster.onStartGlobal += (obj) => {
				//obj.inventory?.gameObject.AddComponent<Statistics>();
			};

			// Set the accrued stats
			RecalculateStatsAPI.GetStatCoefficients += (sender, args) => {
				if (sender && sender.inventory) {
					int count = sender.inventory.GetItemCount(ItemDef);
					if (count > 0) {
						var stats = 0;
						args.healthMultAdd		+= stats;
						args.moveSpeedMultAdd	+= stats;
						args.damageMultAdd		+= stats;
						args.attackSpeedMultAdd	+= stats;
						args.critAdd			+= stats;
						args.armorAdd			+= stats;
						args.healthMultAdd		+= stats;
					}
				}
			};

			// Grant stats on shrine complete
			On.RoR2.PurchaseInteraction.OnInteractionBegin += (orig, self, activator) => {
				orig(self, activator); //so the original function still runs
				if (self.GetComponent<RoR2.PurchaseInteraction>().isShrine) {
					Log.Info("HEY SHITASS!! YOU FOUND A SHRINE!!");
					// Add 0.015f+(count * 0.015f) to the accrued stats
					// Tweak numbers unless I find out how to make this only work when the shrine is completed
				}
			};
		}

		// Initialize the item
		public override void Init() {
			CreateItem();
			Hooks();
		}
	}
}
