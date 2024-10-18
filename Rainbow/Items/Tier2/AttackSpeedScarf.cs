﻿using R2API;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rainbow.Items
{
	public class AttackSpeedScarf : ItemBase
	{
		public override string ItemLangTokenName	=> "AttackSpeedScarf";
		public override ItemTier Tier				=> ItemTier.Tier2;
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
			GlobalEventManager.onCharacterDeathGlobal += (damageReport) => {
				DamageReport report = damageReport;

				// If a character was killed by the world, we shouldn't do anything.
				if (!report.attacker || !report.attackerBody) { return; }

				var attackerCharacterBody = report.attackerBody;

				// We need an inventory to do check for our item
				if (attackerCharacterBody.inventory) {
					var garbCount = GetCount(attackerCharacterBody);
					if (garbCount > 0 && Util.CheckRoll(50, attackerCharacterBody.master)) {
						attackerCharacterBody.AddTimedBuff(RoR2Content.Buffs.Cloak, 3 + garbCount);
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