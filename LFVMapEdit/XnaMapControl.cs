using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;
using System.Security;

namespace LFVMapEdit
{
    public class XnaMapControl : Game
    {


        private IntPtr drawSurface;

        private GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        public XnaMapControl(IntPtr drawSurface)
        {
            this.drawSurface = drawSurface;
            graphics = new GraphicsDeviceManager(this);

            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            System.Windows.Forms.Control.FromHandle((this.Window.Handle)).VisibleChanged += new EventHandler(XnaMapControl_VisibleChanged);
        }

        void XnaMapControl_VisibleChanged(object sender, EventArgs e)
        {
            if (Control.FromHandle((this.Window.Handle)).Visible == true)
                Control.FromHandle((this.Window.Handle)).Visible = false;
        }

        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawSurface;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            basicEffect = new BasicEffect(graphics.GraphicsDevice, null);

            this.vertexDeclaration = new VertexDeclaration(this.GraphicsDevice, VertexPositionColor.VertexElements);

            this.basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, 0, 0, 1);
            
            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime)
        {

            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);


            this.GraphicsDevice.VertexDeclaration = this.vertexDeclaration;
            this.basicEffect.Begin();

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                this.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList,
                       cells, 0, cells.Length/2);

                pass.End();
            }

            this.basicEffect.End();


            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            spriteBatch.End();

            base.Draw(gameTime);
        }
           
        protected override void Update(GameTime gameTime)
        {           
            base.Update(gameTime);

            if (updateCells)
            {
                this.UpdateCells();
                updateCells = false;
            }
        }

        private void UpdateCells()
        {
            List<VertexPositionColor> lst = new List<VertexPositionColor>();

            for (int x = 0; x <= this.Width; x += fint_TileWidth)
            {
                lst.Add(new VertexPositionColor(new Vector3(x, 0, 0), Color.Black));
                lst.Add(new VertexPositionColor(new Vector3(x, this.Height, 0), Color.Black));                
            }

            for (int y = 0; y <= this.Height; y += fint_TileHeight)
            {
                lst.Add(new VertexPositionColor(new Vector3(0, y, 0), Color.White));
                lst.Add(new VertexPositionColor(new Vector3(this.Width, y, 0), Color.White));
            }

            cells = lst.ToArray();
        }

        bool updateCells = true;
        public int Width
        {
            get { return this.GraphicsDevice.Viewport.Width; }
            //set { this.GraphicsDevice.Viewport.Width = value; }
        }

        public int Height
        {
            get { return this.GraphicsDevice.Viewport.Height; }
            //set { this.GraphicsDevice.Viewport.Viewport.Height = value; }
        }

        private int fint_TileWidth = 16;
        public int TileWidth
        {
            get { return fint_TileWidth; }
            set { fint_TileWidth = value; }
        }

        private int fint_TileHeight = 16;
        public int TileHeight
        {
            get { return fint_TileHeight; }
            set { fint_TileHeight = value; }
        }

        VertexDeclaration vertexDeclaration;
        VertexPositionColor[] cells;
        BasicEffect basicEffect;

    }
}
