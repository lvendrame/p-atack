using System;
using System.Collections.Generic;
using System.Text;
using LFVMath.Basic;

namespace LFVGame.Items
{
	public class ItemHolySkul: Item
	{

		public ItemHolySkul(double addEnergy)
		{
			this.SpriteImage = Resource.holySkul;
			this.Heigth = this.SpriteImage.Height;
			this.Width = this.SpriteImage.Width;
			this.dblAddEnergy = addEnergy;
		}

		private double dblAddEnergy;
		public double AddEnergy
		{
			get { return dblAddEnergy; }
			set { dblAddEnergy = value; }
		}

		public override void ApplyChanges(double miliseconds, LFVGame.Ships.UserSpaceShip ssUser)
		{
			ssUser.Energy += this.dblAddEnergy;
		}
	}
}
