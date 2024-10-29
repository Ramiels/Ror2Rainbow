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


		// Initialize the buff
		public override void Init()
		{
			CreateBuff();
		}

	}
}