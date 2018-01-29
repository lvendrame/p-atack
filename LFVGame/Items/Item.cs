using System;
using System.Collections.Generic;
using System.Text;
using LFVGame.Ships;

namespace LFVGame.Items
{
	public abstract class Item: CombatantDrawable
	{
		public Item()
		{
		}

		public virtual bool Update(double miliseconds, UserSpaceShip ssUser)
		{
			base.Update(miliseconds);
			if (this.Colide(ssUser))
			{
				this.ApplyChanges(miliseconds, ssUser);
				return true;
			}
			return false;
		}

		public abstract void ApplyChanges(double miliseconds, UserSpaceShip ssUser);
	}
}
