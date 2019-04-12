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
    class City : RC_GameStateParent
    {
        Sprite3 city = null;
        Sprite3 scroll = null;
        public override void LoadContent()
        {
            city = new Sprite3(true, Game1.texCity, 0, 0);
            city.setWidthHeight(800, 600);
            scroll = new Sprite3(true, Game1.texScroll, 200, 50);
            scroll.setWidthHeight(400, 500);
        }
        public override void Update(GameTime gameTime)
        {
            
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            city.Draw(spriteBatch);
            scroll.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
