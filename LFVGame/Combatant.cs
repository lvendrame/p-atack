using System;
using System.Collections.Generic;
using System.Text;
using LFVMath.Phisics;

namespace LFVGame
{
	public class Combatant: AbstractColidable
	{
		#region ICombatant Members

        private double dblMaxEnergy;
        public double MaxEnergy
        {
            get { return dblMaxEnergy; }
            set { dblMaxEnergy = value; }
        }

		private double dblEnergy;
		public double Energy
		{
			get
			{
				return dblEnergy;
			}
			set
			{
				dblEnergy = value;
			}
		}

		private int intLifes;
		public int Lifes
		{
			get { return intLifes; }
			set { intLifes = value; }
		}

		public bool IsDead
		{
			get { return intLifes <= 0 && dblEnergy <=0; }
		}

		private double dblForce;
		public double Force
		{
			get
			{
				return dblForce;
			}
			set
			{
				dblForce = value;
			}
		}

		public virtual CombatStatus VerifyCombat(Combatant combatant)
		{
			return this.Colide(combatant) ? this.Combat(combatant) : CombatStatus.None;
		}

		public virtual CombatStatus Combat(Combatant combatant)
		{
			this.Energy -= combatant.Force;
			combatant.Energy -= this.Force;

			CombatStatus status = CombatStatus.OnlyColide;
			if (this.Energy <= 0)
			{
				status |= CombatStatus.MeDie;
				this.intLifes--;
                if (!this.IsDead)
                    this.Energy = this.MaxEnergy;
			}
			if (combatant.Energy <= 0)
			{
				status |= CombatStatus.CombatantDie;
				combatant.Lifes = combatant.Lifes - 1;
                if (!combatant.IsDead)
                    combatant.Energy = combatant.MaxEnergy;
			}

			return status;
		}

		#endregion

        public override void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    public enum CombatStatus
    {
        OnlyColide = 0,
        MeDie = 1,
        CombatantDie = 2,
        BothDie = 3,
        None = 4
    }
}
