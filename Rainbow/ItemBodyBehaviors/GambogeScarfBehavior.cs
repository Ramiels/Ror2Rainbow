using RoR2;
using RoR2.Items;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Rainbow.ItemBodyBehaviors
{
	internal class GambogeScarfBehavior : BaseItemBodyBehavior
	{
		[ItemDefAssociation(useOnServer = true, useOnClient = false)]
		// [BaseItemBodyBehavior.ItemDefAssociationAttribute(useOnServer = true, useOnClient = false)]
		 
		public static ItemDef GetItemDef() => Rainbow.Items.GambogeScarf.ItemDef;

		private bool providingBuff;

		// Handle in/out of combat behavior
		private void OnDisable()
		{
			this.SetProvidingBuff(false);
		}
		private void FixedUpdate()
		{
			Log.Debug("WORK!!! I BEG OF YOU!!");
			this.SetProvidingBuff(base.body.outOfCombat);
		}

		private void SetProvidingBuff(bool shouldProvideBuff)
		{
			if (shouldProvideBuff == this.providingBuff)
			{
				return;
			}
			this.providingBuff = shouldProvideBuff;
			if (this.providingBuff)
			{
				Log.Debug("HEY WE'RE DOING SHIT 11111111");
				base.body.AddBuff(RoR2Content.Buffs.WhipBoost);
				EffectData effectData = new EffectData();
				effectData.origin = base.body.corePosition;
				CharacterDirection characterDirection = base.body.characterDirection;
				bool flag = false;
				if (characterDirection && characterDirection.moveVector != Vector3.zero)
				{
					effectData.rotation = Util.QuaternionSafeLookRotation(characterDirection.moveVector);
					flag = true;
				}
				if (!flag)
				{
					effectData.rotation = base.body.transform.rotation;
				}
				EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/SprintActivate"), effectData, true);
				return;
			}
			base.body.RemoveBuff(RoR2Content.Buffs.WhipBoost);
		}

	}
}
