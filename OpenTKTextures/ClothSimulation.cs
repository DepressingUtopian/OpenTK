using OpenTK;
using OpenTKTextures;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using System.Linq;
using System.Drawing;

namespace OpenTKTextures
{

    class ClothSimulation
    {
        private ShaderProgram renderProg, computeProg, normalProg;
        private uint clothVao;
        private uint numElements;
        private const int PRIM_RESTART = 0xffffff;
        private Vector2 nParticles;
        private Vector2 clothSize;
        private float time, deltaT, speed;
        private uint readBuf;
        private uint[] posBufs = new uint[2];
        private uint[] velBufs = new uint[2];
        private uint normBuf, elBuf, tcBuf;
        private string filePath;
        private Matrix4 model;
        private Matrix4 view;
        private Matrix4 projection;
        private int width = 0;
        private int height = 0;

        private int _minMipmapLevel = 0;
        private int _maxMipmapLevel = 3;
        private int _texture = 0;

        public ShaderProgram RenderProg { get => renderProg; set => renderProg = value; }
        public ShaderProgram ComputeProg { get => computeProg; set => computeProg = value; }
        public ShaderProgram NormalProg { get => normalProg; set => normalProg = value; }
        public uint ClothVao { get => clothVao; set => clothVao = value; }
        public uint NumElements { get => numElements; set => numElements = value; }

        private void compileAndLinkShader()
        {
            ComputeProg = new ShaderProgram();
            RenderProg = new ShaderProgram();
            normalProg = new ShaderProgram();

            ComputeProg.AddShader(ShaderType.ComputeShader, @"Components\Shaders\Compute\cloth.cs");
            NormalProg.AddShader(ShaderType.ComputeShader, @"Components\Shaders\Compute\normal_cloth.cs");
            RenderProg.AddShader(ShaderType.FragmentShader, @"Components\Shaders\Light\abs.frag");
            RenderProg.AddShader(ShaderType.VertexShader, @"Components\Shaders\Light\abs.vec");

            ComputeProg.Link();
            NormalProg.Link();
            RenderProg.Link();
        }
        private void initBuffers()
        {
            // Initial transform
            Matrix4 transf = Matrix4.CreateTranslation(0, clothSize.Y, 0);
            Matrix4 rotX;
            Matrix4 trans2;

            Matrix4.CreateRotationX(-80.0f, out rotX);
            transf = transf * rotX;
            Matrix4.CreateTranslation(0, -clothSize.Y, 0, out trans2);
            transf =  transf * trans2 ;

            // Initial positions of the particles
            List<float> initPos = new List<float>();
            List<float> initVel = new List<float>();
            List<uint> el = new List<uint>();

            for (int i = 0; i < (int)nParticles.X * (int)nParticles.Y * 4; i++)
                initVel.Add(0.0f);

            List<float> initTc = new List<float>();
            float dx = (float)(clothSize.X / (nParticles.X - 1));
            float dy = (float)(clothSize.Y / (nParticles.Y - 1));
            float ds = (float)(1.0f / (nParticles.X - 1));
            float dt = (float)(1.0f / (nParticles.Y - 1));
            Vector4 p = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            for (int i = 0; i < (int)nParticles.Y; i++)
            {
                for (int j = 0; j < (int)nParticles.Y; j++)
                {
                    p.X = dx * j;
                    p.Y = dy * i;
                    p.Z = 0.0f;
                    p = transf * p;
                    initPos.Add(p.X);
                    initPos.Add(p.Y);
                    initPos.Add(p.Z);
                    initPos.Add(1.0f);

                    initTc.Add(ds * j);
                    initTc.Add(dt * i);
                }
            }

            // Every row is one triangle strip

            for (int row = 0; row < nParticles.Y - 1; row++)
            {
                for (int col = 0; col < nParticles.X; col++)
                {
                    el.Add((uint)((row + 1) * nParticles.X + (col)));
                    el.Add((uint)((row) * nParticles.X + (col)));
                }
                el.Add(PRIM_RESTART);
            }

            // We need buffers for position (2), element index,
            // velocity (2), normal, and texture coordinates.
            uint[] bufs = new uint[7];
            GL.GenBuffers(7, bufs);
            posBufs[0] = bufs[0];
            posBufs[1] = bufs[1];
            velBufs[0] = bufs[2];
            velBufs[1] = bufs[3];
            normBuf = bufs[4];
            elBuf = bufs[5];
            tcBuf = bufs[6];

            uint parts = (uint)(nParticles.X * nParticles.Y);

            // The buffers for positions
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 0, posBufs[0]);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, (int)(parts * 4 * sizeof(float)), initPos.ToArray(), BufferUsageHint.DynamicDraw);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 1, posBufs[1]);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, (int)(parts * 4 * sizeof(float)), IntPtr.Zero, BufferUsageHint.DynamicDraw);

            // Velocities
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 2, velBufs[0]);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, (int)(parts * 4 * sizeof(float)), initVel.ToArray(), BufferUsageHint.DynamicCopy);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 3, velBufs[1]);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, (int)(parts * 4 * sizeof(float)), IntPtr.Zero, BufferUsageHint.DynamicCopy);

            // Normal buffer
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 4, normBuf);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, (int)(parts * 4 * sizeof(float)), IntPtr.Zero, BufferUsageHint.DynamicCopy);

            // Element indicies
            GL.BindBuffer(BufferTarget.ArrayBuffer, elBuf);
            GL.BufferData(BufferTarget.ArrayBuffer, (int)(el.Count * sizeof(uint)), el.ToArray(), BufferUsageHint.DynamicCopy);

            // Texture coordinates
            GL.BindBuffer(BufferTarget.ArrayBuffer, tcBuf);
            GL.BufferData(BufferTarget.ArrayBuffer, initTc.Count * sizeof(float), initTc.ToArray(), BufferUsageHint.StaticDraw);

            NumElements = (uint)(el.Count);

            // Set up the VAO
            GL.GenVertexArrays(1, out clothVao);
            GL.BindVertexArray(clothVao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, posBufs[0]);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, normBuf);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, tcBuf);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(2);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elBuf);
            GL.BindVertexArray(0);

            InitTextures(filePath);
        }
        private void setMatrices()
        {
            GL.UseProgram(renderProg.Id);
            var info = GL.GetShaderInfoLog(renderProg.Id);
            if (!string.IsNullOrWhiteSpace(info))

                Console.WriteLine($"GL.CompileShader had info log: {info}");
            //view = Matrix4.LookAt(new Vector3(3, 2, 5), new Vector3(2, 1, 0), new Vector3(0, 1, 0));
            Matrix4 mv = view;
            var norm = new Matrix3(new Vector3(mv.Column0), new Vector3(mv.Column1), new Vector3(mv.Column2));
            var loc1 = GL.GetUniformLocation(RenderProg.Id, "ModelViewMatrix");
            var loc2 = GL.GetUniformLocation(RenderProg.Id, "NormalMatrix");
            var loc3 = GL.GetUniformLocation(RenderProg.Id, "MVP");

            var res = projection * mv;
            GL.UniformMatrix4(loc1, false, ref mv);
            GL.UniformMatrix3(loc2, false, ref norm);
            GL.UniformMatrix4(loc3, false, ref res);
        }
        public ClothSimulation(string texturePath,int width,int height)
        {
            this.ClothVao = 0;
            this.NumElements = 0;
            this.nParticles = new Vector2(40, 40);
            this.clothSize = new Vector2(4.0f, 3.0f);
            this.time = 0;
            this.deltaT = 0;
            this.speed = 200.0f;
            this.readBuf = 0;
            this.filePath = texturePath;
            this.width = width;
            this.height = height;
        }
        public void initScene()
        {
            GL.Enable(EnableCap.PrimitiveRestart);
            GL.PrimitiveRestartIndex(PRIM_RESTART);

            compileAndLinkShader();
            initBuffers();
            projection = Matrix4.CreatePerspectiveFieldOfView(50 * ((float)Math.PI / 180f), (float)(width / height), 1.0f, 4000f);
            var pos = new Vector4() { X = 0, Y = 0, Z = 0, W = 1.0f };
            GL.UseProgram(RenderProg.Id);
            var loc1 = GL.GetUniformLocation(RenderProg.Id, "LightPosition");
            var loc2 = GL.GetUniformLocation(RenderProg.Id, "LightIntensity");
            var loc3 = GL.GetUniformLocation(RenderProg.Id, "Kd");
            var loc4 = GL.GetUniformLocation(RenderProg.Id, "Ka");
            var loc5 = GL.GetUniformLocation(RenderProg.Id, "Ks");
            var loc6 = GL.GetUniformLocation(RenderProg.Id, "Shininess");

            GL.Uniform4(loc1, ref pos);
            GL.Uniform3(loc2, new Vector3(1f, 1f, 1f));
            GL.Uniform3(loc3, new Vector3(0.8f, 0.8f, 0.8f));
            GL.Uniform3(loc4, new Vector3(0.2f, 0.2f, 0.2f));
            GL.Uniform3(loc5, new Vector3(0.2f, 0.2f, 0.2f));
            GL.Uniform1(loc6, 80.0f);

            GL.UseProgram(ComputeProg.Id);
            var loc7 = GL.GetUniformLocation(ComputeProg.Id, "RestLengthHoriz");
            var loc8 = GL.GetUniformLocation(ComputeProg.Id, "RestLengthVert");
            var loc9 = GL.GetUniformLocation(ComputeProg.Id, "RestLengthDiag");

            float dx = clothSize.X / (nParticles.X - 1);
            float dy = clothSize.X / (nParticles.X - 1);
            GL.Uniform1(loc7, dx);
            GL.Uniform1(loc8, dy);
            GL.Uniform1(loc9, Math.Sqrt(dx * dx + dy * dy));


            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _texture);
        }
        public void Update(float t)
        {
            if (time == 0.0f)
            {
                deltaT = 0.0f;
            }
            else
            {
                deltaT = t - time;
            }
            time = t;
        }
        public void Render(Matrix4 _view,Matrix4 _prog)
        {
            GL.UseProgram(ComputeProg.Id);
            var info = GL.GetShaderInfoLog(ComputeProg.Id);
            if (!string.IsNullOrWhiteSpace(info))
                Console.WriteLine($"GL.CompileShader had info log: {info}");
            for (int i = 0; i < 1000; i++)
            {
                GL.DispatchCompute((int)nParticles.X / 10, (int)nParticles.Y / 10, 1);
                GL.MemoryBarrier(MemoryBarrierFlags.ShaderStorageBarrierBit);

                readBuf = 1 - readBuf;
                GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 0, posBufs[readBuf]);
                GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 1, posBufs[1 - readBuf]);
                GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 2, velBufs[readBuf]);
                GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 3, velBufs[1 - readBuf]);
            }
            GL.UseProgram(NormalProg.Id);
            info = GL.GetShaderInfoLog(NormalProg.Id);
            if (!string.IsNullOrWhiteSpace(info))
                Console.WriteLine($"GL.CompileShader had info log: {info}");

            GL.DispatchCompute((int)nParticles.X / 10, (int)nParticles.Y / 10, 1);
            GL.MemoryBarrier(MemoryBarrierFlags.ShaderStorageBarrierBit);

            GL.UseProgram(RenderProg.Id);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //view = Matrix4.LookAt(new Vector3(3, 2, 5), new Vector3(2, 1, 0), new Vector3(0, 1, 0));
            model = Matrix4.Identity;
            view = _view;
            projection = _prog;
            setMatrices();

            GL.BindVertexArray(ClothVao);
            GL.BindTexture(TextureTarget.Texture2D, _texture);

            GL.DrawElements(BeginMode.TriangleStrip, (int)NumElements, DrawElementsType.UnsignedInt, 0);
          //  GL.BindVertexArray(0);
        }
        public void Resize(int w, int h)
        {
            GL.Viewport(0, 0, w, h);
            width = w;
            height = h;
        }
        private void InitTextures(string filename)
        {
            int width, height;
            var data = LoadTexture(filename, out width, out height);
            GL.CreateTextures(TextureTarget.Texture2D, 1, out _texture);
            GL.TextureStorage2D(
                _texture,
                _maxMipmapLevel,             // levels of mipmapping
                SizedInternalFormat.Rgba32f, // format of texture
                width,
                height);

            GL.BindTexture(TextureTarget.Texture2D, _texture);
            GL.TextureSubImage2D(_texture,
                0,                  // this is level 0
                0,                  // x offset
                0,                  // y offset
                width,
                height,
                PixelFormat.Rgba,
                PixelType.Float,
                data);

            GL.GenerateTextureMipmap(_texture);
            GL.TextureParameterI(_texture, TextureParameterName.TextureBaseLevel, ref _minMipmapLevel);
            GL.TextureParameterI(_texture, TextureParameterName.TextureMaxLevel, ref _maxMipmapLevel);
            var textureMinFilter = (int)TextureMinFilter.LinearMipmapLinear;
            GL.TextureParameterI(_texture, TextureParameterName.TextureMinFilter, ref textureMinFilter);
            var textureMagFilter = (int)TextureMinFilter.Linear;
            GL.TextureParameterI(_texture, TextureParameterName.TextureMagFilter, ref textureMagFilter);
        }
        private float[] LoadTexture(string filename, out int width, out int height)
        {
            float[] r;
            using (var bmp = (Bitmap)Image.FromFile(filename))
            {
                width = bmp.Width;
                height = bmp.Height;
                r = new float[width * height * 4];
                int index = 0;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var pixel = bmp.GetPixel(x, y);
                        r[index++] = pixel.R / 255f;
                        r[index++] = pixel.G / 255f;
                        r[index++] = pixel.B / 255f;
                        r[index++] = pixel.A / 255f;
                    }
                }
            }
            return r;
        }

        private static void ExtractMipmapLevel(int yOffset, MipLevel mipLevel, int xOffset, Bitmap bmp, int index)
        {
            var width = xOffset + mipLevel.Width;
            var height = yOffset + mipLevel.Height;
            for (int y = yOffset; y < height; y++)
            {
                for (int x = xOffset; x < width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    mipLevel.Data[index++] = pixel.R / 255f;
                    mipLevel.Data[index++] = pixel.G / 255f;
                    mipLevel.Data[index++] = pixel.B / 255f;
                    mipLevel.Data[index++] = pixel.A / 255f;
                }
            }
        }

        public void BindTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, _texture);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                GL.DeleteTexture(_texture);
            }
        }
    } 
}
