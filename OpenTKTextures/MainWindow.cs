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
using static OpenTKTextures.ColoredRenderObject;

namespace OpenTKTextures
{

    public class MainWindow : GameWindow
    {

        private readonly List<ARenderable> _renderObjects = new List<ARenderable>();
        private double _time;
        private Color4 _backColor = new Color4(0.0f, 0.0f, 0.0f, 1.0f);
        private Matrix4 _projectionMatrix;
        private float _z = -2.7f;
        private float _fov = 60f;
        private ShaderProgram _texturedProgram;
        private ShaderProgram _solidProgram;
        private ShaderProgram _ambientProgram;
        private bool _renderAll = true;
        private bool _rotateSingle = false;
        private ARenderable background,sun,earth,moon,mars, mercury, venus,cube, lightCube;
        private int lightColorLoc;
        private int objectColor;
        private int posLight;
        private int viewPosLoc;
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
            _solidProgram = new ShaderProgram();
            _solidProgram.AddShader(ShaderType.VertexShader, @"Components\Shaders\Light\Phong\Ambient\vertexShader.vert");
            _solidProgram.AddShader(ShaderType.FragmentShader, @"Components\Shaders\Light\Phong\Ambient\fragmentShader.frag");
            _solidProgram.Link();

            //_texturedProgram = new ShaderProgram();
            //_texturedProgram.AddShader(ShaderType.VertexShader, @"Components\Shaders\Textures\vertexShader.vert");
            //_texturedProgram.AddShader(ShaderType.FragmentShader, @"Components\Shaders\Textures\fragmentShader.frag");
            //_texturedProgram.Link();

            _ambientProgram = new ShaderProgram();
            _ambientProgram.AddShader(ShaderType.VertexShader, @"Components\Shaders\Light\Basic\vertexShader.vert");
            _ambientProgram.AddShader(ShaderType.FragmentShader, @"Components\Shaders\Light\Basic\fragmentShader.frag");
            _ambientProgram.Link();

           

            //background = new MipMapGeneratedRenderObject(ObjectFactory.CreateTexturedCube(6, 1, 1), _texturedProgram.Id, @"Components\Textures\stars.jpg",8);
            //moon = new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3, 0.006f), _texturedProgram.Id, @"Components\Textures\moonmap1k.jpg", 8);
            //earth = new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3, 0.012f), _texturedProgram.Id, @"Components\Textures\earth.jpg", 8);
            //sun = new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3, 0.2f), _texturedProgram.Id, @"Components\Textures\sun.jpg", 8);
            //mercury = new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3,0.006f), _texturedProgram.Id, @"Components\Textures\mercury.jpg", 8);
            //mars = new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3, 0.008f), _texturedProgram.Id, @"Components\Textures\mars.jpg", 8);
            //venus = new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3, 0.011f), _texturedProgram.Id, @"Components\Textures\venus.jpg", 8);
            cube = new ColoredRenderObject(ObjectFactory.CreateSolidCube(0.4f,Color4.Indigo), _solidProgram.Id);
            cube.SetPosition(0, 0, _z);
            lightCube = new LightSource(ObjectFactory.CreateSolidCube(0.2f, Color4.White), _ambientProgram.Id);
            lightCube.SetPosition(2, 1, _z);
            //cube.SetAmbientShader(_ambientProgram.Id);
            _renderObjects.Add(cube);
            _renderObjects.Add(lightCube);
            CursorVisible = true;

            objectColor = GL.GetUniformLocation(_solidProgram.Id, "objectColor");
            lightColorLoc = GL.GetUniformLocation(_solidProgram.Id, "lightColor");
            posLight = GL.GetUniformLocation(_solidProgram.Id, "lightPos");
            viewPosLoc = GL.GetUniformLocation(_solidProgram.Id, "viewPos");

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            GL.PointSize(3);
            GL.Enable(EnableCap.DepthTest);
            cloth.initScene();
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
            _time += e.Time;
            Title = $" Отображение примитивов (Vsync: {VSync}) FPS: {1f / e.Time:0}";

            GL.ClearColor(_backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);



            //moon.SetPosition(4f, 4f);
            //earth.SetPosition(4f, 4f);
            //venus.SetPosition(-1.8f, -1.8f);
            //mercury.SetPosition(-0.4f, 0.6f);
            //mars.SetPosition(-1.4f, -1.5f);
            //sun.SetPosition(-0.1f, -0.1f);
            //background.SetPosition(0,0,_z + 2);


            //foreach (var renderObject in _renderObjects)
            //{

            //background.Bind();
            //background.NotMove();
            //background.Render();

            //sun.Bind();
            //sun.MoveRotate(0.2f, 0.2f, 0.1f, _time, _z);
            //sun.Render();

            //GL.UniformMatrix4(20, false, ref _projectionMatrix);

            //earth.Bind();
            // earth.MoveRotate2(0.2f, 0.2f, 1f, _time, _z, sun.PositionX, sun.PositionY);
            // earth.Render();


            //moon.Bind();
            //moon.MoveRotate2(0.1f, 0.2f, 0f, _time, _z, earth.PositionX, earth.PositionY);
            //moon.Render();

            //venus.Bind();
            //venus.MoveRotate2(0.2f, 0.2f, 0f, _time, _z, sun.PositionX, sun.PositionY);
            //venus.Render();

            //mercury.Bind();
            //mercury.MoveRotate2(0.3f, 0.2f, 0f, _time, _z, sun.PositionX, sun.PositionY);
            //mercury.Render();

            //mars.Bind();
            //mars.MoveRotate2(0.4f, 0.2f, 0f, _time, _z, sun.PositionX, sun.PositionY);
            //mars.Render();
            var k7 = (float)(_time * (0.05f + (0.1)));
            var t27 = Matrix4.CreateTranslation(
                (float)(Math.Sin(k7 * 5f) * (2f)),
                (float)(Math.Cos(k7 * 5f) * (2f)), _z);
            cloth.Update((float)e.Time);
            cloth.Render(t27, _projectionMatrix);

            lightCube.Bind();
            
            GL.UniformMatrix4(20, false, ref _projectionMatrix);
            var k = (float)(_time * (0.05f + (0.1)));
            var t22 = Matrix4.CreateTranslation(
                (float)(Math.Sin(k * 5f) * (2f)),
                (float)(Math.Cos(k * 5f) * (2f)), _z);
            lightCube.PositionX = (float)(Math.Sin(k * 5f) * (2f));
            lightCube.PositionY = (float)(Math.Cos(k * 5f) * (2f));
            lightCube.PositionZ = _z;
            var r12 = Matrix4.CreateRotationX(0);
            var r22 = Matrix4.CreateRotationY(k * 13.0f);
            var r32 = Matrix4.CreateRotationZ(k * 3.0f);
            var modelView2 = t22;
            GL.UniformMatrix4(21, false, ref modelView2);
            lightCube.Render();

           

            cube.Bind();
            GL.UniformMatrix4(20, false, ref _projectionMatrix);
            GL.Uniform3(lightColorLoc, new Vector3(1.0f, 1.0f, 1.0f));
            GL.Uniform3(objectColor, new Vector3(1.0f, 0.5f, 0.31f));
            GL.Uniform3(posLight, new Vector3(lightCube.PositionX, lightCube.PositionY, lightCube.PositionZ));
            GL.Uniform3(viewPosLoc, new Vector3(0f,0f,_z - 2));
            var t2 = Matrix4.CreateTranslation(
                  cube.PositionX,
                  cube.PositionY, cube.PositionZ);
            var r1 = Matrix4.CreateRotationX(90);
            var r2 = Matrix4.CreateRotationY(20);
            var r3 = Matrix4.CreateRotationZ(0);
            var modelView = r1 * r2 * r3 * t2;
            GL.UniformMatrix4(21, false, ref modelView);
            cube.Render();

            //GL.UniformMatrix4(20, false, ref _projectionMatrix);

            //moon.Bind();
            //moon.MoveRotate2(0.1f, 0.01f, 0f, _time, _z, earth.PositionX, earth.PositionY);
            //moon.Render();



            //}
    
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
