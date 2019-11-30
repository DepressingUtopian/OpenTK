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
using static OpenTKPrimitives.RenderObject;

namespace OpenTKPrimitives
{

    public class MainWindow : GameWindow
    {
        private int _program;
        private List<RenderObject> _renderObjects = new List<RenderObject>();
        private double _time;
        private Color4 _backColor = new Color4(0.1f, 0.1f, 0.3f, 1.0f);
        private Matrix4 _projectionMatrix;
        private float _z = -2.7f;
        private float _fov = 60f;
        bool IsMouseClick = true;
        float mouseX = 0;
        float mouseY = 0;
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
    
            VSync = VSyncMode.Off;
            CreateProjection();
           // _renderObjects.Add(new RenderObject(ObjectFactory.CreateSolidCube(0.2f, Color4.HotPink)));
           // _renderObjects.Add(new RenderObject(ObjectFactory.CreateSolidSquare(0.2f, Color4.BlueViolet)));
            //_renderObjects.Add(new RenderObject(IcoSphereFactory.Create(3,Color4.Aqua)));
            _renderObjects.Add(new RenderObject(water.GenWater(Color4.Red)));

            CursorVisible = true;

            _program = CreateProgram();
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            GL.Enable(EnableCap.DepthTest);

        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            HandleKeyboard(e.Time);
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButton.Left)
            {
                mouseX = (e.X - Width / 2f) / (Width / 2f);
                mouseY = -(e.Y - Height / 2f) / (Height / 2f);
                // Pass coordinates of point to a_Position
                IsMouseClick = true;



            }
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
            if (keyState.IsKeyDown(Key.Q))
            {
                var rd = Matrix4.CreateRotationX(1);
                _projectionMatrix *= rd;
            }
            if (keyState.IsKeyDown(Key.A))
            {
                var rd = Matrix4.CreateRotationY(1);
                _projectionMatrix *= rd;
            }
            if (keyState.IsKeyDown(Key.Z))
            {
                var rd = Matrix4.CreateRotationZ(1);
                _projectionMatrix *= rd;
            }

        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _time += e.Time;
            Title = $" Отображение примитивов (Vsync: {VSync}) FPS: {1f / e.Time:0}";
          
            GL.ClearColor(_backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(_program);
           
            GL.UniformMatrix4(20, false, ref _projectionMatrix);
           
            float c = 0f;
        
                _renderObjects[_renderObjects.Count - 1] = new RenderObject(water.GenWater(Color4.Aqua));
            
            foreach (var renderObject in _renderObjects)
            {
                renderObject.Bind();
               
                
                    //var k = (float)(_time * (0.05f + (0.1 * c)));
                    var t2 = Matrix4.CreateTranslation(
                        -2,
                       -2, _z - 3);
                    var r1 = Matrix4.CreateRotationY(0);
                    var s = Matrix4.CreateScale(10f,6f,1.0f);
                // var r2 = Matrix4.CreateRotationY(k * 13.0f);
                //  var r3 = Matrix4.CreateRotationZ(k * 3.0f);
                    var modelView = s * t2 * r1;
                  
                
                GL.UniformMatrix4(21, false, ref modelView);
                renderObject.Render();
                c += 0.3f;
            }
            GL.PointSize(10);
            SwapBuffers();
            
        }
        private int CompileShaders(ShaderType type, string path)
        {

            var shader = GL.CreateShader(type);
            var src = File.ReadAllText(path);
            GL.ShaderSource(shader, src);
            GL.CompileShader(shader);
            var info = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrWhiteSpace(info))
                Debug.WriteLine($"GL.CompileShader [{type}] had info log: {info}");
            return shader;
        }
        private int CreateProgram()
        {
            var program = GL.CreateProgram();
            var shaders = new List<int>();
            shaders.Add(CompileShaders(ShaderType.VertexShader, @"Components\Shaders\vertexShader.vert"));
            shaders.Add(CompileShaders(ShaderType.FragmentShader, @"Components\Shaders\fragmentShader.frag"));

            foreach (var shader in shaders)
                GL.AttachShader(program, shader);
            GL.LinkProgram(program);
            var info = GL.GetProgramInfoLog(program);
            if (!string.IsNullOrWhiteSpace(info))
                Debug.WriteLine($"GL.LinkProgram had info log: {info}");

            foreach (var shader in shaders)
            {
                GL.DetachShader(program, shader);
                GL.DeleteShader(shader);
            }
            return program;
        } 

        public override void Exit()
        {
            Debug.WriteLine("Exit called");
            foreach (var obj in _renderObjects)
                obj.Dispose();
            GL.DeleteProgram(_program);
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
