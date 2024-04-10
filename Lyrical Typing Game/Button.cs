using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace RPG_Battler
{
    public class Button 
    {
        public Vector2 position;
        public Vector2 size;
        private Texture2D texture;
        private Color defaultColor;
        private Color colorOnSelect;
        private Action onActivate;
        private bool isPreviousMouseLeftPressed = false;

        public bool Selected { get; set; }

        /// <summary>
        /// A clickable button
        /// </summary>
        /// <param name="position">Position in pixels</param>
        /// <param name="size">Size multiplier.</param>
        /// <param name="texture"></param>
        /// <param name="defaultColor">Color blended with texture colors.</param>
        /// <param name="colorOnSelect">Color blended with texture colors.</param>
        /// <param name="onActivate">Function called when clicked.</param>
        public Button(Vector2 position, Vector2 size, Texture2D texture, Color defaultColor, Color colorOnSelect, Action onActivate)
        {
            this.position = position;
            this.size = size;
            this.texture = texture;
            this.defaultColor = defaultColor;
            this.colorOnSelect = colorOnSelect;
            this.onActivate = onActivate;
        }

        public void Update(MouseState mouseState)
        {
            if (inButton(mouseState))
            {
                Selected = true;
            }
            else
            {
                Selected = false;
            }

            onClick(mouseState);
        }

        private bool inButton(MouseState mouseState)
        {
            return mouseState.X < position.X + texture.Width * size.X &&
                mouseState.X > position.X &&
                mouseState.Y < position.Y + texture.Height * size.Y &&
                mouseState.Y > position.Y;
        }

        private void onClick(MouseState mouseState)
        {
            if (inButton(mouseState) && mouseState.LeftButton == ButtonState.Pressed && !isPreviousMouseLeftPressed)
            {
                onActivate();
            }
            isPreviousMouseLeftPressed = mouseState.LeftButton == ButtonState.Pressed;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(texture, position, null, Selected ? colorOnSelect : defaultColor, 0, Vector2.Zero, size, SpriteEffects.None, 0);
        }
    }
}