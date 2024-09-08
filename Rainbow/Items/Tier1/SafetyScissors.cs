using R2API;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rainbow.Items
{
	public class SafetyScissors : ItemBase
	{
		public override string ItemName				=> "Safety Scissors";
		public override string ItemLangTokenName	=> "SAFETY_SCISSORS";
		public override string ItemPickupDesc		=> "Slightly increase armor and damage.";
		public override string ItemFullDescription	=> "Increase armor by <style=cIsUtility>6</style> <style=cStack>(+6 per stack)</style> and damage by <style=cIsDamage>6%</style> <style=cStack>(+6% per stack)</style>.";
		public override string ItemLore				=> "Placeholder! Lore will come later <3";
		public override ItemTier Tier				=> ItemTier.Tier1;
		public override GameObject ItemModel		=> Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();
		public override Sprite ItemIcon				=> Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
		public override ItemDisplayRuleDict CreateItemDisplayRules() {
			throw new NotImplementedException();
		}

		// Hook shit
		public override void Hooks() {
			RecalculateStatsAPI.GetStatCoefficients += (sender, args) => {
                if (sender && sender.inventory) {
                    int count = sender.inventory.GetItemCount(ItemDef);
                    if (count > 0) {
                        args.armorAdd += count * 0.06f;
                        args.damageMultAdd += count * 0.06f;
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
