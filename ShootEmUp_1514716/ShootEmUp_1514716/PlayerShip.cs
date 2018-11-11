using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp_1514716
{
    class PlayerShip : Entity
    {
        /* Bullet cooldown */
        const int cooldownFrames = 6;
        int cooldownRemaining = 0;
        static Random rand = new Random();
        /* Bullet cooldown */


        /*Respawn*/
        int framesUntilRespawn = 0;
        public bool IsDead { get { return framesUntilRespawn > 0; } }
        /**/
        private static PlayerShip instance;
        public static PlayerShip Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerShip();
                return instance;
            }
        }
        private PlayerShip()
        {
            image = GameRoot.Player;
            Position = GameRoot.ScreenSize / 2;
            Radius = 10;
        }

        public override void Update()
        {
            // ship logic goes here

            /*Player Death*/
            if (IsDead)
            {
                EnemySpawner.Reset();
                framesUntilRespawn --;
                return;
            }
            /**/

            /* Player Movement logic*/
            const float speed = 8;
            Velocity = speed * Input.GetMovementDirection();
            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size
            / 2);
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            /*Player shooting cooldown logic*/
            var aim = Input.GetAimDirection();
            if (aim.LengthSquared() > 0 && cooldownRemaining <= 0)
            {
                cooldownRemaining = cooldownFrames;
                float aimAngle = aim.ToAngle();
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0,
                aimAngle);
                float randomSpread = rand.NextFloat(-0.04f, 0.04f) +
                rand.NextFloat(-0.04f, 0.04f);
                Vector2 vel = 8f * new Vector2((float)Math.Cos(aimAngle +
                randomSpread), (float)Math.Sin(aimAngle + randomSpread));
                Vector2 offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
                EntityManager.Add(new Bullet(Position + offset, vel));
                offset = Vector2.Transform(new Vector2(25, 8), aimQuat);
                EntityManager.Add(new Bullet(Position + offset, vel));
            }
            if (cooldownRemaining > 0)
                cooldownRemaining --;
        }

        /*Override for player death*/
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDead)
                base.Draw(spriteBatch);
        }
        /*  */
        public void Kill()
        {
            framesUntilRespawn = 60;
        }


    }
}
