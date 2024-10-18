using R2API;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rainbow.Items
{
	public class DamageAndArmor : ItemBase
	{
		public override string ItemLangTokenName	=> "DamageAndArmor";
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
			RecalculateStatsAPI.GetStatCoefficients += (sender, args) => {
                if (sender && sender.inventory) {
                    int count = sender.inventory.GetItemCount(ItemDef);
                    if (count > 0) {
                        args.armorAdd += count * 6;
                        args.damageMultAdd += count * 0.06f;
                    }
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
