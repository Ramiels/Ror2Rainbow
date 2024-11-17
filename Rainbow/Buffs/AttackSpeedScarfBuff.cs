using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace Rainbow.Buffs
{
	internal class AttackSpeedScarfBuff : BuffBase
	{
		public override Sprite IconSprite => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
		public override string Name => "AttackSpeedScarf";
		public override bool CanStack => true;

		// Initialize the buff
		public override void Init()
		{
			Hooks();
			CreateBuff();
		}
		// Hook shit
		public override void Hooks()
		{
			R2API.RecalculateStatsAPI.GetStatCoefficients += (sender, args) =>{
				if (sender && sender.HasBuff(Buffs.BuffBase.BuffDefs["AttackSpeedScarf"])) {
					int count = sender.GetBuffCount(Buffs.BuffBase.BuffDefs["AttackSpeedScarf"]);
					if (count > 0)
					{
						args.attackSpeedMultAdd += 0.07f * count;
					}
				}
			};
		}
	}
}