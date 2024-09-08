using R2API;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rainbow.Items
{
	public class SafetyScissors : ItemBase
	{
		public override string ItemName				=> "SAFETY_SCISSORS_NAME";
		public override string ItemLangTokenName	=> "SAFETY_SCISSORS";
		public override string ItemPickupDesc		=> "SAFETY_SCISSORS_PICKUP";
		public override string ItemFullDescription	=> "SAFETY_SCISSORS_DESC";
		public override string ItemLore				=> "SAFETY_SCISSORS_LORE";
		public override ItemTier Tier				=> ItemTier.Tier1;
		public override GameObject ItemModel		=> Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();
		public override Sprite ItemIcon				=> Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
		public override ItemDisplayRuleDict CreateItemDisplayRules() {
			// One day...
			var rules = new ItemDisplayRuleDict();
			rules.Add("mdlCommandoDualies", new RoR2.ItemDisplayRule[] {
				new RoR2.ItemDisplayRule {
					ruleType = ItemDisplayRuleType.ParentedPrefab,
					followerPrefab = ItemModel,
					childName = "Chest",
					localPos = new Vector3(0F, 0.17296F, 0.20893F),
					localAngles = new Vector3(80.00002F, 180F, 180F),
					localScale = new Vector3(0.08412F, 0.06451F, 0.06451F)
				}
			});
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
			CreateLang();
			CreateItem();
			Hooks();
		}
	}
}
