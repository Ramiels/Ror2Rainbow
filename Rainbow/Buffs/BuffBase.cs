using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UnityEngine;

namespace Rainbow.Buffs
{
	public abstract class BuffBase
	{
		public virtual bool IsDebuff { get; } = false;
		public virtual Color32 BuffColor { get; } = UnityEngine.Color.green;
		public virtual Sprite IconSprite { get; }
		public virtual bool IsCooldown { get; } = false;


		public static BuffDef BuffDef;
		// Init func
		public abstract void Init();

		// Create item
		protected void CreateBuff()
		{
			// Create the buffdef and populate it with data
			BuffDef = ScriptableObject.CreateInstance<BuffDef>();
			BuffDef.isDebuff = IsDebuff;
			BuffDef.buffColor = BuffColor;
			BuffDef.iconSprite = IconSprite;
			BuffDef.isCooldown = IsCooldown;
			// Add the buff
			R2API.ContentAddition.AddBuffDef(BuffDef);
		}
	}
}