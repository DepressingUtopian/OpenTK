
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenTKTextures;

namespace OpenTKCloth
{
    class MainWindow : GameWindow
    {

        private readonly List<ARenderable> _renderObjects = new List<ARenderable>();
        private double _time;
        private Color4 _backColor = new Color4(1.0f, 0.2f, 0.0f, 1.0f);
        private Matrix4 _projectionMatrix;
        private float _z = -2.7f;
        private float _fov = 60f;
        private ClothSimulation cloth;


        public MainWindow() : base(1820, // initial width
                980, // initial height
                GraphicsMode.Default,
                "",  // initial title
                GameWindowFlags.Default,
                DisplayDevice.Default,
                4, // OpenGL major version
                5, // OpenGL minor version
                GraphicsContextFlags.ForwardCompatible)
        {
            Title += ": OpenGL Version: " + GL.GetString(StringName.Version);
            cloth = new ClothSimulation(@"Components\Textures\cloth.png", Width, Height);
        }
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            CreateProjection();
        }
        protected override void OnLoad(EventArgs e)
        {

            Debug.WriteLine("OnLoad");
            VSync = VSyncMode.Off;
            CreateProjection();
            CursorVisible = true;
          
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
          //  GL.PointSize(3);
            GL.Enable(EnableCap.DepthTest);
            Debug.WriteLine("OnLoad .. done");
            cloth.initScene();

        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            HandleKeyboard(e.Time);
        }
        private void HandleKeyboard(double dt)
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            if (keyState.IsKeyDown(Key.M))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
            }
            if (keyState.IsKeyDown(Key.Comma))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            if (keyState.IsKeyDown(Key.Period))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
            if (keyState.IsKeyDown(Key.J))
            {
                _fov = 40f;
                CreateProjection();
            }
            if (keyState.IsKeyDown(Key.K))
            {
                _fov = 50f;
                CreateProjection();
            }
            if (keyState.IsKeyDown(Key.L))
            {
                _fov = 60f;
                CreateProjection();
            }

            if (keyState.IsKeyDown(Key.W))
            {
                _z += 0.2f * (float)dt;
            }
            if (keyState.IsKeyDown(Key.S))
            {
                _z -= 0.2f * (float)dt;
            }
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _time += e.Time;
            Title = $" Отображение примитивов (Vsync: {VSync}) FPS: {1f / e.Time:0}";
            GL.ClearColor(_backColor);
            cloth.Update((float)e.Time);
            cloth.Render();
            SwapBuffers();
        }


        public override void Exit()
        {
            Debug.WriteLine("Exit called");
            foreach (var obj in _renderObjects)
                obj.Dispose();

            base.Exit();
        }
        private void CreateProjection()
        {

            var aspectRatio = (float)Width / Height;
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                _fov * ((float)Math.PI / 180f),
                aspectRatio,
                0.1f,
                4000f);
        }
 
    }
}
