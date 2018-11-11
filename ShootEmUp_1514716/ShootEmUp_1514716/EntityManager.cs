using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp_1514716
{
    static class EntityManager
    {
        static List<Entity> entities = new List<Entity>();
        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();
        public static int Count { get { return entities.Count; } }

        public static object entity { get; private set; }

        static List<Enemy> enemies = new List<Enemy>();
        static List<Bullet> bullets = new List<Bullet>();

        
        private static void AddEntity(Entity entity)
        {
            entities.Add(entity);
            if (entity is Bullet)
                bullets.Add(entity as Bullet);
            else if (entity is Enemy)
                enemies.Add(entity as Enemy);
        }

        public static void Add(Entity entity)
        {
            if (!isUpdating)
                entities.Add(entity);
            if (entity is Bullet)
                bullets.Add(entity as Bullet);
            if (entity is Enemy)
               enemies.Add(entity as Enemy);
            else
                addedEntities.Add(entity);
        }
        private static bool IsColliding(Entity a, Entity b)
        {
            float radius = a.Radius + b.Radius;
            return !a.IsExpired && !b.IsExpired &&
            Vector2.DistanceSquared(a.Position, b.Position) < radius * radius;
        }
        
        
        public static void Update()
        {
            bullets = bullets.Where(x => !x.IsExpired).ToList();
            enemies = enemies.Where(x => !x.IsExpired).ToList();

            HandleCollisions();
            isUpdating = true;

            foreach (var entity in entities)  
            entity.Update();
            isUpdating = false;
            foreach (var entity in addedEntities)
                entities.Add(entity);
            //entities.AddEntity(entity);
            addedEntities.Clear();
            // remove any expired entities .
            entities = entities.Where(x => !x.IsExpired).ToList();
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
                entity.Draw(spriteBatch);
        }

        static void HandleCollisions()
        {
            // handle collisions between enemies
            for (int i = 0; i < enemies.Count; i++)
                for (int j = i + 1; j < enemies.Count; j++)
                {
                    if (IsColliding(enemies[i], enemies[j]))
                    {
                        enemies[i].HandleCollision(enemies[j]);
                        enemies[j].HandleCollision(enemies[i]);
                    }
                }
            // handle collisions between bullets and enemies
            for (int i = 0; i < enemies.Count; i++)
                for (int j = 0; j < bullets.Count; j++)
                {
                    if (IsColliding(enemies[i], bullets[j]))
                    {
                        
                    enemies[i].WasShot();
                        bullets[j].IsExpired = true;
                    }
                }
            // handle collisions between the player and enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsActive && IsColliding(PlayerShip.Instance,
                enemies[i]))
                {
                    PlayerShip.Instance.Kill();
                    // X does not exist in current context
                    //enemies.ForEach(x = > x.WasShot()); 
                    break;
                }
            }
        }


    }
}
