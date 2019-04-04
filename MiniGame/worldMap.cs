using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using RC_Framework;


namespace MiniGame
{
    class WorldMap : RC_GameStateParent
    {
        Camera mainCamera;
        public override void LoadContent()
        {
            Game1.texWorldMap = Util.texFromFile(graphicsDevice, Game1.dir + "FantasyWorldMap_2.png");
            mainCamera = new Camera();

        }
        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            horse.setWidthHeight(1568 / 8 * 0.3f, texHorseRun.Height * 0.3f);
            k = Keyboard.GetState();
            if (!started)
            {
                started = true;
                StartMovement(8);
            }

            if (k.IsKeyDown(Keys.Down))
            {
                keyDown = true;
                horse.setPosY(horse.getPosY() + movementSpeed - 2);
            }

            if (k.IsKeyDown(Keys.Up))
            {
                keyDown = true;
                horse.setPosY(horse.getPosY() - movementSpeed + 2);
            }

            if (k.IsKeyDown(Keys.Left))
            {
                keyDown = true;
                horse.setFlip(SpriteEffects.FlipHorizontally);
                horse.setPosX(horse.getPosX() - movementSpeed + 2);
            }

            if (k.IsKeyDown(Keys.Right))
            {
                keyDown = true;
                horse.setFlip(SpriteEffects.None);
                horse.setPosX(horse.getPosX() + movementSpeed - 2);
            }
            if (!keyDown)
            {
                horse.setAnimationSequence(anim, 0, 7, 0);
            }
            else
            {
                horse.setAnimationSequence(anim, 0, 7, 8);
            }
            //387 330
            if (curPos.X >= -155 && curPos.X <= -75 && curPos.Y >= 330 && curPos.Y <= 387)
            {
                horse.setPos(xx, yy);
                started = false;
                horse.setFlip(SpriteEffects.None);
                level = 0;
            }

            horseRun.animationTick(gameTime);
            mainCamera.Follow(horse);

        }
    }
}
