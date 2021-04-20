using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Rockhoppers.scripts
{
    static class SceneManager
    {

        public static List<Entity> entityList = new List<Entity>();

        public static List<Entity> queuedEntities = new List<Entity>();

        public static List<Entity> culledEntities = new List<Entity>();

        public static List<Player> players = new List<Player>();

        public static Vector2 camPos { get => players[0].playerEntity.WorldPosition; }


        public static float coolDown = 0.25f;
        public static float coolDownTimer = 0.0f;

        public static bool has_loaded;



        public static void InitializeScene()
        {
            Player player = new Player();
            Entity ship = new Ship("ship");


            ship.ScreenPosition = Game1.ScreenSize / 2f ;
            ship.WorldPosition = new Vector2(100000f, 100000f);

            player.Set_Entity(ship);

            Background background = new Background(true, "stars");
            background.parent_player = player;

            has_loaded = true;

            players.Add(player);

            player.playerRadar.ParentShip = (Ship)ship;
        }


        public static void UpdateScene(GameTime gameTime)
        {
            coolDownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Random rnd = new Random();

            if(Input.kbState.IsKeyDown(Keys.G) && coolDownTimer >= coolDown)
            {
                

                Ship enemy = new Ship("ship2");
                enemy.WorldPosition = new Vector2(100400,100400);
                enemy.WorldPosition += new Vector2(rnd.Next(-1000, 1000), rnd.Next(-1000, 1000));
                coolDownTimer = 0;
            }

            foreach(Player player in players)
            {
                player.Update(gameTime);
            }

            foreach(Entity entity in entityList)
            {
                entity.Update(gameTime);
            }

            AddEntities();

        }


        public static void QueueEntity(Entity e)
        {
            queuedEntities.Add(e);
        }

        public static void AddEntities()
        {
            foreach(Entity e in queuedEntities)
                entityList.Add(e);

            queuedEntities = new List<Entity>();
        }


        public static void ClearEntities()
        {
            entityList = new List<Entity>();
        }

        public static void RemoveEntity(Entity e)
        {
            culledEntities.Add(e);
        }

        public static void CullEntities()
        {
            foreach(Entity e in culledEntities)
            {
                entityList.Remove(e);
            }
        }

        public static Entity GetEntity(string _uniqueId)
        {
            System.Diagnostics.Debug.WriteLine("ID: " + _uniqueId);
            foreach (Entity e in entityList)
            {
                if(e.uniqueID == Convert.ToInt32(_uniqueId))
                {
                    return e;
                }
            }
            return null;
        }
    }
}
