using System;
using System.Collections.Generic;
using System.Text;
using LFVMath.Basic;
using LFVGame.Ships;
using LFVGame.Items;
using LFVGame.Bullets;
using System.IO;

namespace LFVGame.Stages
{
    public class StageOne : GenericStage
    {

        public override void LoadComponents()
        {
            base.LoadComponents();
            //this.Load(@"C:\Documents and Settings\luis.digisystem\My Documents\Luis\Projetos\Engine\LFVGame\Resources\Stage01z.ppm");
            this.Load(Path.Combine(ContentManager.GetPath(ContentManager.PathType.Maps), "Stage01z.ppm"));
        }

        int stage = -2;
        public override void Update(double elapsedTime)
        {
            base.Update(elapsedTime);
            this.VerifyStage();

            if (this.IsStopedMove && this.Enemies.Count == 0)
                this.IsFinish = true;
        }

        private void VerifyStage()
        {
            if (stage == -2)
            {
                Vector2D pos = new Vector2D(150, this.Position.Y - 200);
                EnemySpaceShip enemy = new EnemySpaceShip(pos);
                enemy.Velocity = pos - this.User.Position;
                enemy.Velocity = enemy.Velocity.GetNormal() * 50;
                this.Enemies.Add(enemy);
                stage++;
            }
            else if (stage == -1)
            {
                Vector2D pos = new Vector2D(150, this.Position.Y - 200);
                EnemySpaceShip enemy = new EnemySpaceShip(pos);
                enemy.Velocity = pos - this.User.Position;
                enemy.Velocity = enemy.Velocity.GetNormal() * 200;
                this.Enemies.Add(enemy);
                stage++;
            }
            else if (stage == 0)
            {
                if (this.Position.Y <= 2000)
                {
                    Random rd = new Random();
                    for (int i = 1; i < 11; i++)
                    {
                        EnemySpaceShip enemy = new EnemySpaceShip(new LFVMath.Basic.Vector2D(i * 50, 1800));
                        if (rd.Next(5) < 2)
                        {
                            enemy.Item = new ItemBullet(BulletType.Laser, 5, new Vector2D(0, -200), new Vector2D(10,0), true, 5, 0.50);
                        }
                        this.Enemies.Add(enemy);
                    }
                    stage++;
                }
            }
            else if (stage == 1)
            {
                if (this.Position.Y <= 1300)
                {
                    Random rd = new Random();
                    for (int i = 1; i < 14; i++)
                    {
                        EnemySpaceShip enemy = new EnemySpaceShip(new LFVMath.Basic.Vector2D(i * 50, 1200));
                        if (rd.Next(5) < 2)
                        {
                            enemy.Item = new ItemHolySkul(100);
                        }
                        this.Enemies.Add(enemy);
                    }
                    stage++;
                }
            }
            else if (stage == 2)
            {
                if (this.Position.Y <= 800)
                {
                    for (int i = 4; i < 14; i++)
                    {
                        EnemySpaceShip enemy = new EnemySpaceShip(new LFVMath.Basic.Vector2D(i * 50, 700));
                        enemy.BulletSettings.IsParalel = true;
                        enemy.BulletSettings.Quantity = 3;
                        enemy.BulletSettings.Increment.X = 4;
                        enemy.BulletSettings.Increment.Y = 3;
                        this.Enemies.Add(enemy);
                    }
                    stage++;
                }
            }
            else if (stage == 3)
            {
                if (this.Position.Y <= 100)
                {
                    for (int i = 4; i < 14; i++)
                    {
                        Vector2D pos = new LFVMath.Basic.Vector2D(i * 50, 0);
                        EnemySpaceShip enemy = new EnemySpaceShip(pos);
                        enemy.Velocity = this.User.Position - pos;
                        enemy.Velocity = enemy.Velocity.GetNormal() * 200;
                        this.Enemies.Add(enemy);                        
                    }
                    stage++;
                }
            }
            else if (stage == 4)
            {
                if (this.Position.Y <= 100 && Enemies.Count <= 2)
                {
                    EnemySpaceShip enemy = new EnemySpaceShip(new LFVMath.Basic.Vector2D(350, 0));
                    enemy.BulletSettings.Force = 5;
                    enemy.SpriteImage = StaticImages.Sprites[GameSprites.AirPlane_Green.GetSpriteIndex(SpritePositionEnum.DOWN, ObjectStatus.Normal)];
                    enemy.BulletSettings.Quantity = 5;
                    enemy.BulletSettings.Increment.X = 20;
                    enemy.BulletSettings.Interval = 0.75;
                    enemy.TimeAcummulator.MaxTime = 0.75;
                    enemy.BulletSettings.NominalVelocity = 350;
                    enemy.Energy = 300;
                    enemy.Velocity.Y = 120;
                    enemy.Velocity.X = 100;
                    this.Enemies.Add(enemy);

                    stage++;
                }
            }
        }
    }
}
