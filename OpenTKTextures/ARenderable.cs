using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace OpenTKTextures
{
    public abstract class ARenderable
    {
        protected readonly int Program;
        private  int AmbientShader;
        private bool ExistAmbient = false;
        protected readonly int VertexArray;
        protected readonly int Buffer;
        protected readonly int VerticeCount;
        private float positionX = 0;
        private float positionY = 0;
        private float positionZ = 0;

        public float PositionX { get => positionX; set => positionX = value; }
        public float PositionY { get => positionY; set => positionY = value; }
        public float PositionZ { get => positionZ; set => positionZ = value; }

        protected ARenderable(int program, int vertexCount)
        {
            Program = program;
            VerticeCount = vertexCount;
            VertexArray = GL.GenVertexArray();
            Buffer = GL.GenBuffer();

            GL.BindVertexArray(VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer);
        }
        public void SetAmbientShader(int ambientShader)
        {
            AmbientShader = ambientShader;
            ExistAmbient = true;
        }
        public virtual void Bind()
        {
            if(ExistAmbient)
                GL.UseProgram(AmbientShader);
            GL.UseProgram(Program);
            GL.BindVertexArray(VertexArray);
        }
        public virtual void Render()
        {
            GL.DrawArrays(PrimitiveType.Triangles, 0, VerticeCount);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GL.DeleteVertexArray(VertexArray);
                GL.DeleteBuffer(Buffer);
            }
        }
        public void NotMove()
        {
            var t2 = Matrix4.CreateTranslation(
                  PositionX,
                   PositionY,
                   PositionZ);
           
            var modelView = t2;
            GL.UniformMatrix4(21, false, ref modelView);
        }
        public void MoveRotate(float speedX, float speedY, float speedZ, double _time, float _z)
        {
                var k = (float)(_time * (0.05f + (0.1)));
                var t2 = Matrix4.CreateTranslation(
                   PositionX,
                    PositionY,
                    _z);
                var r1 = Matrix4.CreateRotationX(k * speedX);
                var r2 = Matrix4.CreateRotationY(k * speedY);
                var r3 = Matrix4.CreateRotationZ(k * speedZ);
                var modelView = r1 * r2 * r3 * t2;
                GL.UniformMatrix4(21, false, ref modelView);
        }
        public void MoveRotate2(float speedX, float speedY, float speedZ, double _time, float _z,float target_x,float target_y)
        {
                var k = (float)(_time * (0.05f + (0.1)));
           

            var t2 = Matrix4.CreateTranslation(
                  (float)((this.PositionX - target_x) * Math.Cos(k * 5f) + (this.PositionY - target_y) * Math.Sin(k * 5f) + target_x) * speedX,
                   (float)((-1) * (this.PositionX - target_x) * Math.Sin(k * 5f) + (this.PositionY - target_y) * Math.Cos(k * 5f) + target_y) * speedY,
                    _z);
                var r1 = Matrix4.CreateRotationX(k * speedX);
                var r2 = Matrix4.CreateRotationY(k * speedY);
                var r3 = Matrix4.CreateRotationZ(k * speedZ);
                var modelView = r1 * r2 * r3 * t2;
                GL.UniformMatrix4(21, false, ref modelView);
            this.positionX = (float)((this.PositionX - target_x) * Math.Cos(k * 5f) + (this.PositionY - target_y) * Math.Sin(k * 5f) + target_x);
            this.positionY = (float)((-1) * (this.PositionX - target_x) * Math.Sin(k * 5f) + (this.PositionY - target_y) * Math.Cos(k * 5f) + target_y);
            Console.WriteLine("x:{0} , y: {1}",this.PositionX,this.PositionY);
        }
        public void SetPosition(float x,float y,float z = 0)
        {
            this.PositionX = x;
            this.PositionY = y;
            this.PositionZ = z;
        }
    }
}
