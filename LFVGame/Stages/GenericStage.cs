using System;
using System.Collections.Generic;
using System.Text;
using LFVMath.Basic;
using System.Drawing;
using LFVGL;
using LFVGame.Ships;
using LFVGame.Bullets;
using LFVGame.Items;

namespace LFVGame.Stages
{
	public class GenericStage: Map, IGameStage
	{
		private UserSpaceShip ssUser;
		public UserSpaceShip User
		{
			get { return ssUser; }			
		}

		private Vector2D v2dNextUserPosition;
		public Vector2D NextUserPosition
		{
			get { return v2dNextUserPosition; }
			set { v2dNextUserPosition = value; }
		}
        
		private bool createBullet = false;

		#region Lists

		private List<EnemySpaceShip> lstEnemies = new List<EnemySpaceShip>();
		public List<EnemySpaceShip> Enemies
		{
			get { return lstEnemies; }
		}

		private List<Bullet> lstUserBullets = new List<Bullet>();
		public List<Bullet> UserBullets
		{
			get { return lstUserBullets; }
		}

		private List<Bullet> lstEnemiesBullets = new List<Bullet>();
		public List<Bullet> EnemiesBullets
		{
			get { return lstEnemiesBullets; }
		}
		
		private List<Explosion> lstExplosions = new List<Explosion>();
		public List<Explosion> Explosions
		{
			get { return lstExplosions; }
		}

		private List<Item> lstItem = new List<Item>();
		public List<Item> Items
		{
			get { return lstItem; }
		}

		#endregion
        
		public override void Update(double elapsedTime)
		{
			if (this.IsPaused)
				return;

			if (this.Position.Y <= 0)
				this.State = StageState.Stop;
			
			if(!this.IsStopedMove)
				this.UpdateY(elapsedTime);

            if (this.v2dNextUserPosition.Y >= this.Position.Y && this.v2dNextUserPosition.Y <= (this.Position.Y + 500))
                ssUser.Position.Y = this.v2dNextUserPosition.Y;
            else
                ssUser.Position.Y += this.Velocity.Y * elapsedTime;

            if(this.v2dNextUserPosition.X >= this.Position.X && this.v2dNextUserPosition.X <= (this.Position.X+800))
                ssUser.Position.X = this.v2dNextUserPosition.X;
			    

			#region createBullet
			ssUser.TimeAcummulator.Update(elapsedTime);
			if (createBullet)
			{
                if (ssUser.TimeAcummulator.IsOverflow)
				{
                    Vector2D posB;
                    posB.X = ssUser.Position.X + ssUser.AddPosBullet;
                    posB.Y = ssUser.Position.Y;
                    ssUser.BulletSettings.AddBullets(lstUserBullets, posB, this, true);
					createBullet = false;
				}
			} 
			#endregion

			#region enemy.Update
			for (int ie = 0; ie < lstEnemies.Count; ie++)
			{
				EnemySpaceShip enemy = lstEnemies[ie];
				switch (enemy.Update(ssUser, this, elapsedTime))
				{
					case CombatStatus.MeDie:
					case CombatStatus.BothDie:
						lstEnemies.RemoveAt(ie);
						ie--;
						if (enemy.HasItem)
							this.Items.Add(enemy.Item);
                        enemy.Dispose();
						break;
				}
			}
			#endregion

			#region lstUserBullets
			for (int i = 0; i < lstUserBullets.Count; ++i)
			{
				Bullet bullet = lstUserBullets[i];
				bullet.Update(elapsedTime);
				if (bullet.Position.Y <= this.Position.Y)
				{
					lstUserBullets.RemoveAt(i);
                    bullet.Dispose();
					--i;
					continue;
				}
				for (int j = 0; j < lstEnemies.Count; ++j)
				{
					EnemySpaceShip enemy = lstEnemies[j];
					CombatStatus cs = bullet.VerifyCombat(enemy);
					bool breakFor = false;
					switch (cs)
					{
						case CombatStatus.BothDie:
                            #region CombatStatus.BothDie
                            this.lstExplosions.Add(new Explosion(bullet.Position, ExplosionType.One));
                            lstUserBullets.RemoveAt(i);
                            bullet.Dispose();
                            --i;
                            lstEnemies.RemoveAt(j);
                            --j;
                            if (enemy.HasItem)
                            {
                                lstItem.Add(enemy.Item);
                            }
                            enemy.Dispose();
                            breakFor = true;
                            break; 
                            #endregion
						case CombatStatus.MeDie:
                            #region CombatStatus.MeDie
                            this.lstExplosions.Add(new Explosion(bullet.Position, ExplosionType.One));
                            lstUserBullets.RemoveAt(i);
                            bullet.Dispose();
                            --i;
                            breakFor = true;
                            break; 
                            #endregion
						case CombatStatus.CombatantDie:
                            #region CombatStatus.CombatantDie
                            this.lstExplosions.Add(new Explosion(bullet.Position, ExplosionType.One));
                            lstEnemies.RemoveAt(j);
                            --j;
                            if (enemy.HasItem)
                            {
                                lstItem.Add(enemy.Item);
                            }
                            enemy.Dispose();
                            break; 
                            #endregion
					}
					if (breakFor)
						break;
				}
			} 
			#endregion

			#region lstEnemiesBullets
			for (int i = 0; i < lstEnemiesBullets.Count; ++i)
			{
				Bullet enBullet = lstEnemiesBullets[i];
				enBullet.Update(elapsedTime);
				if (enBullet.Position.Y >= (this.Position.Y + 816))
				{
					lstEnemiesBullets.RemoveAt(i);
                    enBullet.Dispose();
					--i;
					continue;
				}
				if (enBullet.VerifyCombat(ssUser) != CombatStatus.None)
				{
					this.lstExplosions.Add(new Explosion(enBullet.Position, ExplosionType.Two));
					lstEnemiesBullets.RemoveAt(i);
                    enBullet.Dispose();
					--i;
					break;
				}
			}

			#endregion

			#region lstExplosions
			for (int i = 0; i < lstExplosions.Count; ++i)
			{
				Explosion exp = lstExplosions[i];
				exp.Update(elapsedTime);
				if (exp.Destroy())
				{
					lstExplosions.RemoveAt(i);
                    exp.Dispose();
					--i;
				}
			}
			#endregion

			#region lstItem
			for (int i = 0; i < lstItem.Count; ++i)
			{
				Item ib = lstItem[i];
				if(ib.Update(elapsedTime, ssUser))
				{
					lstItem.RemoveAt(i);
                    ib.Dispose();
					--i;
				}
			}
			#endregion
		}

		public override void Draw(System.Drawing.Graphics grPaint)
		{
			base.Draw(grPaint);
			this.ssUser.Draw(grPaint, this.Position);
            for (int i = 0; i < lstEnemies.Count; i++)
            {
                lstEnemies[i].Draw(grPaint, this.Position);
            }
            for (int i = 0; i < this.lstEnemiesBullets.Count; i++)
            {
                lstEnemiesBullets[i].Draw(grPaint, this.Position);
            }
            for (int i = 0; i < this.lstUserBullets.Count; i++)
            {
                lstUserBullets[i].Draw(grPaint, this.Position);
            }
            for (int i = 0; i < this.lstExplosions.Count; i++)
            {
                lstExplosions[i].Draw(grPaint, this.Position);
            }
            for (int i = 0; i < this.lstItem.Count; i++)
            {
                lstItem[i].Draw(grPaint, this.Position);
            }
            for (int i = 0; i < ssUser.Lifes; i++)
            {
                grPaint.DrawImage(Resource.NaveRoxa, 120 + (i * 35), 10);
            }
		}

		#region IGameStage Members

        private StageState enmState = StageState.Running;
        public StageState State
        {
            get { return enmState; }
            set { enmState = value; }
        }

		public bool IsPaused
		{
            get { return enmState == StageState.Pause; }
		}

		public bool IsStopedMove
		{
			get { return this.enmState == StageState.Stop; }
		}

        public bool ExitGame
        {
            get
            {
                return this.enmState == StageState.ExitGame;
            }
        }

		private bool blnLockUser = false;
		public bool LockUser
		{
			get { return blnLockUser; }
		}

		public virtual void CheckGameInputs()
		{
			WinAPIUtil.MouseButtons mb = WinAPIUtil.GetMouseButtons;
			if (mb == WinAPIUtil.MouseButtons.Left)
				createBullet = true;
		}

		short lastPauseStatus = WinAPIUtil.GetKeyState(80);
		public virtual void CheckMainInputs()
		{
			/*if (WinAPIUtil.GetKeyState(69) < 0)
				this.IsRunning = false;*/

			short currentPauseStatus = WinAPIUtil.GetKeyState(80);
			if (currentPauseStatus < 0)
			{
				currentPauseStatus = lastPauseStatus == 0 ? (short)1 : (short)0;
			}
			if (lastPauseStatus != currentPauseStatus)
			{
                if (this.IsPaused)
                    this.State = StageState.Running;
                else
                    this.State = StageState.Pause;
			}
			lastPauseStatus = currentPauseStatus;
		}

		public virtual void LoadComponents()
		{
			ssUser = new UserSpaceShip();
		}

		public virtual void Redraw(double elapsedTime)
		{
			this.blnIsDrawing = true;

			
			Image imgNew = new Bitmap(800, 600, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			using (Graphics gImg = Graphics.FromImage(imgNew))
			{
				this.Draw(gImg);
				gImg.Save();				
			}

			this.imgStageImage = imgNew;
			this.blnIsDrawing = false;
		}

		public virtual void UnloadComponents()
		{
			this.lstEnemies.Clear();
			this.lstEnemiesBullets.Clear();
			this.lstExplosions.Clear();
			this.lstUserBullets.Clear();
			this.imgStageImage = null;
		}

		bool blnIsLoad = false;
		public bool IsLoad
		{
			get { return blnIsLoad; }
		}

		bool blnIsFinish = false;
		public bool IsFinish
		{
			get { return blnIsFinish; }
			set { blnIsFinish = value; }
		}

		bool blnIsDrawing = false;
		public bool IsDrawing
		{
			get { return blnIsDrawing; }
		}

		protected Image imgStageImage = null;
		public System.Drawing.Image StageImage
		{
			get { return imgStageImage; }
		}

		#endregion

        public override void Dispose()
        {
            Util.FinalizeListItems(lstEnemies);
            Util.FinalizeListItems(lstUserBullets);
            Util.FinalizeListItems(lstEnemiesBullets);
            Util.FinalizeListItems(lstExplosions);
            Util.FinalizeListItems(lstItem);
            base.Dispose();
        }
	}
}
