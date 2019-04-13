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
        
        public override void LoadContent()
        {
            city = new Sprite3(true, Game1.texCityScreen, 0, 0);
            city.setWidthHeight(800, 600);
            
        }
        public override void Update(GameTime gameTime)
        {
            
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            city.Draw(spriteBatch);
            
            spriteBatch.End();
        }
    }
}
