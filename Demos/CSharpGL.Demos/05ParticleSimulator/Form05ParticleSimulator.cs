﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace CSharpGL.Demos
{
    public partial class Form05ParticleSimulator : Form
    {

        private Camera camera;
        private SatelliteManipulater rotator;
        private RendererBase renderer;

        public Form05ParticleSimulator()
        {
            InitializeComponent();

            this.glCanvas1.OpenGLDraw += glCanvas1_OpenGLDraw;
            //this.glCanvas1.MouseDown += glCanvas1_MouseDown;
            //this.glCanvas1.MouseMove += glCanvas1_MouseMove;
            //this.glCanvas1.MouseUp += glCanvas1_MouseUp;
            //this.glCanvas1.MouseWheel += glCanvas1_MouseWheel;
            this.glCanvas1.Resize += glCanvas1_Resize;

            Application.Idle += Application_Idle;
            OpenGL.ClearColor(0, 0, 0, 0);
        }

        void Application_Idle(object sender, EventArgs e)
        {
            this.Text = string.Format("{0} - FPS: {1}", this.GetType().Name, this.glCanvas1.FPS.ToShortString());
        }

        private void glCanvas1_OpenGLDraw(object sender, PaintEventArgs e)
        {
            OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);

            RenderEventArg arg = new RenderEventArg(RenderModes.Render, this.glCanvas1.ClientRectangle, this.camera);
            IRenderable renderer = this.renderer;
            if (renderer != null)
            {
                renderer.Render(arg);
            }

            // Cross cursor shows where the mouse is.
            //GL.DrawText(this.lastmousePositionX - offset.X,
            //    this.glCanvas1.Height - (this.lastmousePositionY + offset.Y) - 1,
            //    Color.Red, "Courier New", crossCursorSize, "o");
        }


        private const float crossCursorSize = 40.0f;

        private Point offset = new Point(13, 11);

        //void glCanvas1_MouseWheel(object sender, MouseEventArgs e)
        //{
        //    ICamera camera = this.camera;
        //    if (camera != null)
        //    {
        //        camera.MouseWheel(e.Delta);
        //    }
        //}

        private void glCanvas1_Resize(object sender, EventArgs e)
        {
            if (camera != null)
            {
                camera.Resize(this.glCanvas1.Width, this.glCanvas1.Height);
            }
        }

    }
}
