using System;
using System.Collections.Generic;
using System.Text;
using LFVMath.Basic;
using LFVGame.Stages;

namespace LFVGame.Bullets
{
    public struct BulletSettings
    {
        private int fint_Quantity;
        public int Quantity
        {
            get { return fint_Quantity; }
            set { fint_Quantity = value; }
        }

        private bool fbln_IsParalel;
        public bool IsParalel
        {
            get { return fbln_IsParalel; }
            set { fbln_IsParalel = value; }
        }

        public Vector2D Increment;

        public Vector2D InitialVelocity;

        private double fdbl_Force;
        public double Force
        {
            get { return fdbl_Force; }
            set { fdbl_Force = value; }
        }

        private double fdbl_Energy;
        public double Energy
        {
            get { return fdbl_Energy; }
            set { fdbl_Energy = value; }
        }

        private BulletType fblt_Type;
        public BulletType Type
        {
            get { return fblt_Type; }
            set { fblt_Type = value; }
        }

        private double fint_Interval;
        public double Interval
        {
            get { return fint_Interval; }
            set { fint_Interval = value; }
        }

        private double fdblNominalVelocity;
        public double NominalVelocity
        {
            get { return fdblNominalVelocity; }
            set { fdblNominalVelocity = value; }
        }

        public void AddBullets(List<Bullet> lstBullets, Vector2D v2dPosition, GenericStage stage, bool isUserBullet)
        {
            Vector2D velocity = this.InitialVelocity;
            Vector2D incPos = this.Increment;
            if (this.fint_Quantity % 2 == 0)
            {
                velocity.X -= incPos.X;
                incPos.X += this.Increment.X;
            }

            if (fbln_IsParalel)
            {
                #region fbln_IsParalel
                Vector2D finalPosition = v2dPosition;
                for (int i = 0; i < this.fint_Quantity; i++)
                {
                    lstBullets.Add(new Bullet(finalPosition, this.fblt_Type, this.fdbl_Force, this.InitialVelocity, stage, isUserBullet));

                    if (finalPosition.X <= v2dPosition.X)
                    {
                        finalPosition.X += incPos.X;
                        finalPosition.Y += incPos.Y;
                    }
                    else
                    {
                        finalPosition.X -= incPos.X;
                    }
                    incPos.X += this.Increment.X;
                } 
                #endregion
            }
            else
            {
                #region else
                for (int i = 0; i < this.fint_Quantity; i++)
                {
                    lstBullets.Add(new Bullet(v2dPosition, this.fblt_Type, this.fdbl_Force, velocity, stage, isUserBullet));

                    if (velocity.X <= this.InitialVelocity.X)
                    {
                        velocity.X += incPos.X;
                    }
                    else
                    {
                        velocity.X -= incPos.X;
                    }

                    incPos.X += this.Increment.X;
                }
                #endregion
            }
        }

        public void AddBullets<TB>(List<Bullet> lstBullets, Vector2D v2dPosition) where TB : Bullet, new()
        {
        }

    }
}
