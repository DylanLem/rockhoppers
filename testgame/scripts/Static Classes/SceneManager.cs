using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Rockhoppers.scripts
{
    static class SceneManager
    {
        public static UIElement helpScreen;

        public static List<Entity> entityList = new List<Entity>();

        public static List<Entity> queuedEntities = new List<Entity>();

        public static List<Entity> culledEntities = new List<Entity>();

        public static List<Player> players = new List<Player>();

        public static Vector2 camPos { get => players[0].playerEntity.WorldPosition; }


        public static float coolDown = 0.105f;
        public static float coolDownTimer = 0.0f;

        public static bool has_loaded;



        public static void InitializeScene()
        {
            helpScreen = new UIElement("helpscreen");
            helpScreen.TextureScale = Vector2.One;
            helpScreen.ScreenPosition = Game1.ScreenSize - helpScreen.textureSize;
            helpScreen.Hide(true);

            Player player = new Player();
            Entity ship = new Ship("chassis_1")
            {
                ScreenPosition = Game1.ScreenSize / 2f,
                WorldPosition = new Vector2(100000f, 100000f)
            };

            player.Set_Entity(ship);
            player.Create_UI();

            Background background = new Background("stars")
            {
                parent_player = player
            };

            has_loaded = true;

            players.Add(player);

            player.playerRadar.ParentShip = (Ship)ship;

            RocketLauncher rocker = new RocketLauncher();
            rocker.WorldPosition = player.playerEntity.WorldPosition + new Vector2(100);
            rocker.ParentShip = null;

            UIText help = new UIText("fontMain");
            help.ScreenPosition = Game1.ScreenSize / 2;
            help.text = "PRESS H FOR HELP :)";
            help.TextureScale *= 0.5f;
            help.color = Color.SlateGray;
            help.deleteQueued = true;
            help.deleteDelay = 4f;
        }


        public static void UpdateScene(GameTime gameTime)
        {
          

            coolDownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Random rnd = new Random();

            if(Input.kbState.IsKeyDown(Keys.G) && coolDownTimer >= coolDown)
            {


                Ship enemy = new Ship("ship2")
                {
                    WorldPosition = players[0].playerEntity.WorldPosition + new Vector2(rnd.Next(-10000, 10000), rnd.Next(-10000, 10000))
                    
                    
                };
                enemy.spriteDepth += rnd.Next(-100,100) / 1000;


                coolDownTimer = 0;
            }

            if (Input.kbState.IsKeyDown(Keys.H) && Input.TryInput(Keys.H))
            {
                
                helpScreen.Hide(! helpScreen.isHidden);
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
