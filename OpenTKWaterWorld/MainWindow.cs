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
using OpenTKPrimitives;
using OpenTKTextures;
using static OpenTKPrimitives.RenderObject;



namespace OpenTKWater
{

    public class MainWindow : GameWindow
    {

        private float _z = -2.7f;
        private float _fov = 60f;
        private ShaderProgram _solidProgram;

        private OpenTKPrimitives.RenderObject render;
        private OpenTKPrimitives.RenderObject render2;
        Matrix4 _projectionMatrix;
        Water water = new Water();
    
        
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
            _solidProgram = new ShaderProgram();
            _solidProgram.AddShader(ShaderType.VertexShader, @"Components\Shaders\Solid\vertexShader.vert");
            _solidProgram.AddShader(ShaderType.FragmentShader, @"Components\Shaders\Solid\fragmentShader.frag");
            _solidProgram.Link();
            render2 = new RenderObject(OpenTKPrimitives.ObjectFactory.CreateSolidCube(0.2f, Color4.HotPink));

            render = new OpenTKPrimitives.RenderObject(water.GenWater(Color4.Red));


      
           // GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            GL.PointSize(3);
            GL.Enable(EnableCap.DepthTest);
            Debug.WriteLine("OnLoad .. done");

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
           
            Title = $" Отображение примитивов (Vsync: {VSync}) FPS: {1f / e.Time:0}";


            GL.ClearColor(0,0,0,1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
         

   

        //render.Timer();
        render2.Bind();
            GL.UniformMatrix4(20, false, ref _projectionMatrix);
            //render = new OpenTKPrimitives.RenderObject(water.GenWater(Color4.Red));

            Matrix4 pos = Matrix4.CreateTranslation(-1f, -1f,0f);
            Matrix4 rotate = Matrix4.CreateRotationX(60);
            Matrix4 rotate2 = Matrix4.CreateRotationY(40);
            Matrix4 scale = Matrix4.CreateScale(2f,4f,0);

            var modelView = pos * rotate * rotate2;
            GL.UniformMatrix4(21, false, ref modelView);
            render2.Render();


            //earth.SetPosition(4f, 4f);
            //venus.SetPosition(-1.8f, -1.8f);
            //mercury.SetPosition(-0.4f, 0.6f);
            //mars.SetPosition(-1.4f, -1.5f);
            //sun.SetPosition(-0.1f, -0.1f);


            SwapBuffers();
        }
        

        public override void Exit()
        {
            Debug.WriteLine("Exit called");
    

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
