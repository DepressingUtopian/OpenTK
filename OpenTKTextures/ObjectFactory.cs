using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenTKTextures.ColoredRenderObject;

namespace OpenTKTextures
{
    public class ObjectFactory
    {
        public static ColoredVertex[] CreateSolidCube(float side, Color4 color)
        {
            side = side / 2f; // half side - and other half
            ColoredVertex[] vertices =
            {
                new ColoredVertex(new Vector4(-side, -side, -side, 1.0f),   color,new Vector3(  0.0f,  0.0f, -1.0f)),
                new ColoredVertex(new Vector4(-side, -side, side, 1.0f),    color,new Vector3(  0.0f,  0.0f, -1.0f)),
                new ColoredVertex(new Vector4(-side, side, -side, 1.0f),    color,new Vector3(  0.0f,  0.0f, -1.0f)),
                new ColoredVertex(new Vector4(-side, side, -side, 1.0f),    color,new Vector3(  0.0f,  0.0f, -1.0f)),
                new ColoredVertex(new Vector4(-side, -side, side, 1.0f),    color,new Vector3(  0.0f,  0.0f, -1.0f)),
                new ColoredVertex(new Vector4(-side, side, side, 1.0f),     color,new Vector3(  0.0f,  0.0f, -1.0f)),

                new ColoredVertex(new Vector4(side, -side, -side, 1.0f),    color,new Vector3(  0.0f,  0.0f,  1.0f)),
                new ColoredVertex(new Vector4(side, side, -side, 1.0f),     color,new Vector3(  0.0f,  0.0f,  1.0f)),
                new ColoredVertex(new Vector4(side, -side, side, 1.0f),     color,new Vector3(  0.0f,  0.0f,  1.0f)),
                new ColoredVertex(new Vector4(side, -side, side, 1.0f),     color,new Vector3(  0.0f,  0.0f,  1.0f)),
                new ColoredVertex(new Vector4(side, side, -side, 1.0f),     color,new Vector3(  0.0f,  0.0f,  1.0f)),
                new ColoredVertex(new Vector4(side, side, side, 1.0f),      color,new Vector3(  0.0f,  0.0f,  1.0f)),

                new ColoredVertex(new Vector4(-side, -side, -side, 1.0f),   color,new Vector3(  -1.0f,  0.0f,  0.0f)),
                new ColoredVertex(new Vector4(side, -side, -side, 1.0f),    color,new Vector3(  -1.0f,  0.0f,  0.0f)),
                new ColoredVertex(new Vector4(-side, -side, side, 1.0f),    color,new Vector3(  -1.0f,  0.0f,  0.0f)),
                new ColoredVertex(new Vector4(-side, -side, side, 1.0f),    color,new Vector3(  -1.0f,  0.0f,  0.0f)),
                new ColoredVertex(new Vector4(side, -side, -side, 1.0f),    color,new Vector3(  -1.0f,  0.0f,  0.0f)),
                new ColoredVertex(new Vector4(side, -side, side, 1.0f),     color,new Vector3(  -1.0f,  0.0f,  0.0f)),

                new ColoredVertex(new Vector4(-side, side, -side, 1.0f),    color,new Vector3( 1.0f,  0.0f,  0.0f)),
                new ColoredVertex(new Vector4(-side, side, side, 1.0f),     color,new Vector3( 1.0f,  0.0f,  0.0f)),
                new ColoredVertex(new Vector4(side, side, -side, 1.0f),     color,new Vector3( 1.0f,  0.0f,  0.0f)),
                new ColoredVertex(new Vector4(side, side, -side, 1.0f),     color,new Vector3( 1.0f,  0.0f,  0.0f)),
                new ColoredVertex(new Vector4(-side, side, side, 1.0f),     color,new Vector3( 1.0f,  0.0f,  0.0f)),
                new ColoredVertex(new Vector4(side, side, side, 1.0f),      color,new Vector3( 1.0f,  0.0f,  0.0f)),

                new ColoredVertex(new Vector4(-side, -side, -side, 1.0f),   color,new Vector3(  0.0f, -1.0f,  0.0f)),
                new ColoredVertex(new Vector4(-side, side, -side, 1.0f),    color,new Vector3(  0.0f, -1.0f,  0.0f)),
                new ColoredVertex(new Vector4(side, -side, -side, 1.0f),    color,new Vector3(  0.0f, -1.0f,  0.0f)),
                new ColoredVertex(new Vector4(side, -side, -side, 1.0f),    color,new Vector3(  0.0f, -1.0f,  0.0f)),
                new ColoredVertex(new Vector4(-side, side, -side, 1.0f),    color,new Vector3(  0.0f, -1.0f,  0.0f)),
                new ColoredVertex(new Vector4(side, side, -side, 1.0f),     color,new Vector3(  0.0f, -1.0f,  0.0f)),

                new ColoredVertex(new Vector4(-side, -side, side, 1.0f),    color,new Vector3(  0.0f,  1.0f,  0.0f)),
                new ColoredVertex(new Vector4(side, -side, side, 1.0f),     color,new Vector3(  0.0f,  1.0f,  0.0f)),
                new ColoredVertex(new Vector4(-side, side, side, 1.0f),     color,new Vector3(  0.0f,  1.0f,  0.0f)),
                new ColoredVertex(new Vector4(-side, side, side, 1.0f),     color,new Vector3(  0.0f,  1.0f,  0.0f)),
                new ColoredVertex(new Vector4(side, -side, side, 1.0f),     color,new Vector3(  0.0f,  1.0f,  0.0f)),
                new ColoredVertex(new Vector4(side, side, side, 1.0f),      color,new Vector3(  0.0f,  1.0f,  0.0f)),
            };
            return vertices;
        }

        public static TexturedVertex[] CreateTexturedCube(float side, float textureWidth, float textureHeight)
        {
            float h = textureHeight;
            float w = textureWidth;
            side = side / 2f; // half side - and other half

            TexturedVertex[] vertices =
            {
                new TexturedVertex(new Vector4(-side, -side, -side, 1.0f),   new Vector2(0, h)),
                new TexturedVertex(new Vector4(-side, -side, side, 1.0f),    new Vector2(w, h)),
                new TexturedVertex(new Vector4(-side, side, -side, 1.0f),    new Vector2(0, 0)),
                new TexturedVertex(new Vector4(-side, side, -side, 1.0f),    new Vector2(0, 0)),
                new TexturedVertex(new Vector4(-side, -side, side, 1.0f),    new Vector2(w, h)),
                new TexturedVertex(new Vector4(-side, side, side, 1.0f),     new Vector2(w, 0)),

                new TexturedVertex(new Vector4(side, -side, -side, 1.0f),    new Vector2(w, 0)),
                new TexturedVertex(new Vector4(side, side, -side, 1.0f),     new Vector2(0, 0)),
                new TexturedVertex(new Vector4(side, -side, side, 1.0f),     new Vector2(w, h)),
                new TexturedVertex(new Vector4(side, -side, side, 1.0f),     new Vector2(w, h)),
                new TexturedVertex(new Vector4(side, side, -side, 1.0f),     new Vector2(0, 0)),
                new TexturedVertex(new Vector4(side, side, side, 1.0f),      new Vector2(0, h)),

                new TexturedVertex(new Vector4(-side, -side, -side, 1.0f),   new Vector2(w, 0)),
                new TexturedVertex(new Vector4(side, -side, -side, 1.0f),    new Vector2(0, 0)),
                new TexturedVertex(new Vector4(-side, -side, side, 1.0f),    new Vector2(w, h)),
                new TexturedVertex(new Vector4(-side, -side, side, 1.0f),    new Vector2(w, h)),
                new TexturedVertex(new Vector4(side, -side, -side, 1.0f),    new Vector2(0, 0)),
                new TexturedVertex(new Vector4(side, -side, side, 1.0f),     new Vector2(0, h)),

                new TexturedVertex(new Vector4(-side, side, -side, 1.0f),    new Vector2(w, 0)),
                new TexturedVertex(new Vector4(-side, side, side, 1.0f),     new Vector2(0, 0)),
                new TexturedVertex(new Vector4(side, side, -side, 1.0f),     new Vector2(w, h)),
                new TexturedVertex(new Vector4(side, side, -side, 1.0f),     new Vector2(w, h)),
                new TexturedVertex(new Vector4(-side, side, side, 1.0f),     new Vector2(0, 0)),
                new TexturedVertex(new Vector4(side, side, side, 1.0f),      new Vector2(0, h)),

                new TexturedVertex(new Vector4(-side, -side, -side, 1.0f),   new Vector2(0, h)),
                new TexturedVertex(new Vector4(-side, side, -side, 1.0f),    new Vector2(w, h)),
                new TexturedVertex(new Vector4(side, -side, -side, 1.0f),    new Vector2(0, 0)),
                new TexturedVertex(new Vector4(side, -side, -side, 1.0f),    new Vector2(0, 0)),
                new TexturedVertex(new Vector4(-side, side, -side, 1.0f),    new Vector2(w, h)),
                new TexturedVertex(new Vector4(side, side, -side, 1.0f),     new Vector2(w, 0)),

                new TexturedVertex(new Vector4(-side, -side, side, 1.0f),    new Vector2(0, h)),
                new TexturedVertex(new Vector4(side, -side, side, 1.0f),     new Vector2(w, h)),
                new TexturedVertex(new Vector4(-side, side, side, 1.0f),     new Vector2(0, 0)),
                new TexturedVertex(new Vector4(-side, side, side, 1.0f),     new Vector2(0, 0)),
                new TexturedVertex(new Vector4(side, -side, side, 1.0f),     new Vector2(w, h)),
                new TexturedVertex(new Vector4(side, side, side, 1.0f),      new Vector2(w, 0)),
            };
            return vertices;
        }
        public static TexturedVertex[] CreateTexturedRectangle(float x,float y,float z, float textureWidth, float textureHeight)
        {
            float height = textureHeight;
            float width = textureWidth;
           

            TexturedVertex[] vertices =
            {
                new TexturedVertex(new Vector4(-x, -y, -z, 1.0f),   new Vector2(0, height)),
                new TexturedVertex(new Vector4(-x, -y, z, 1.0f),    new Vector2(width, height)),
                new TexturedVertex(new Vector4(-x, y, -z, 1.0f),    new Vector2(0, 0)),
                new TexturedVertex(new Vector4(-x, y, -z, 1.0f),    new Vector2(0, 0)),
                new TexturedVertex(new Vector4(-x, -y, z, 1.0f),    new Vector2(width, height)),
                new TexturedVertex(new Vector4(-x, y, z, 1.0f),     new Vector2(width, 0)),

                new TexturedVertex(new Vector4(x, -y, -z, 1.0f),    new Vector2(width, 0)),
                new TexturedVertex(new Vector4(x, y, -z, 1.0f),     new Vector2(0, 0)),
                new TexturedVertex(new Vector4(x, -y, z, 1.0f),     new Vector2(width, height)),
                new TexturedVertex(new Vector4(x, -y, z, 1.0f),     new Vector2(width, height)),
                new TexturedVertex(new Vector4(x, y, -z, 1.0f),     new Vector2(0, 0)),
                new TexturedVertex(new Vector4(x, y, z, 1.0f),      new Vector2(0, height)),

                new TexturedVertex(new Vector4(-x, -y, -z, 1.0f),   new Vector2(width, 0)),
                new TexturedVertex(new Vector4(x, -y, -z, 1.0f),    new Vector2(0, 0)),
                new TexturedVertex(new Vector4(-x, -y, z, 1.0f),    new Vector2(width, height)),
                new TexturedVertex(new Vector4(-x, -y, z, 1.0f),    new Vector2(width, height)),
                new TexturedVertex(new Vector4(x, -y, -z, 1.0f),    new Vector2(0, 0)),
                new TexturedVertex(new Vector4(x, -y, z, 1.0f),     new Vector2(0, height)),

                new TexturedVertex(new Vector4(-x, y, -z, 1.0f),    new Vector2(width, 0)),
                new TexturedVertex(new Vector4(-x, y, z, 1.0f),     new Vector2(0, 0)),
                new TexturedVertex(new Vector4(x, y, -z, 1.0f),     new Vector2(width, height)),
                new TexturedVertex(new Vector4(x, y, -z, 1.0f),     new Vector2(width, height)),
                new TexturedVertex(new Vector4(-x, y, z, 1.0f),     new Vector2(width, 0)),
                new TexturedVertex(new Vector4(x, y, z, 1.0f),      new Vector2(width, height)),

                new TexturedVertex(new Vector4(-x, -y, -z, 1.0f),   new Vector2(0, height)),
                new TexturedVertex(new Vector4(-x, y, -z, 1.0f),    new Vector2(width, height)),
                new TexturedVertex(new Vector4(x, -y, -z, 1.0f),    new Vector2(0, 0)),
                new TexturedVertex(new Vector4(x, -y, -z, 1.0f),    new Vector2(0, 0)),
                new TexturedVertex(new Vector4(-x, y, -z, 1.0f),    new Vector2(width, height)),
                new TexturedVertex(new Vector4(x, y, -z, 1.0f),     new Vector2(width, 0)),

                new TexturedVertex(new Vector4(-x, -y, z, 1.0f),    new Vector2(0, height)),
                new TexturedVertex(new Vector4(x, -y, z, 1.0f),     new Vector2(width, height)),
                new TexturedVertex(new Vector4(-x, y, z, 1.0f),     new Vector2(0, 0)),
                new TexturedVertex(new Vector4(-x, y, z, 1.0f),     new Vector2(0, 0)),
                new TexturedVertex(new Vector4(x, -y, z, 1.0f),     new Vector2(width, height)),
                new TexturedVertex(new Vector4(x, y, z, 1.0f),      new Vector2(width, 0)),
            };
            return vertices;
        }
        public static TexturedVertex[] CreateTexturedCube6(float side, float textureWidth, float textureHeight)
        {
            float h = textureHeight;
            side = side / 2f; // half side - and other half
            var tx10 = 0f;
            var tx11 = textureWidth / 6f;

            var tx20 = textureWidth / 6f;
            var tx21 = (textureWidth / 6f) * 2f;

            var tx30 = (textureWidth / 6f) * 2f;
            var tx31 = (textureWidth / 6f) * 3f;

            var tx40 = (textureWidth / 6f) * 3f;
            var tx41 = (textureWidth / 6f) * 4f;

            var tx50 = (textureWidth / 6f) * 4f;
            var tx51 = (textureWidth / 6f) * 5f;

            var tx60 = (textureWidth / 6f) * 5f;
            var tx61 = (textureWidth / 6f) * 6f;

            TexturedVertex[] vertices =
            {
                new TexturedVertex(new Vector4(-side, -side, -side, 1.0f),  new Vector2(tx10, h)),
                new TexturedVertex(new Vector4(-side, -side, side, 1.0f),   new Vector2(tx11, h)),
                new TexturedVertex(new Vector4(-side, side, -side, 1.0f),   new Vector2(tx10, 0)),
                new TexturedVertex(new Vector4(-side, side, -side, 1.0f),   new Vector2(tx10, 0)),
                new TexturedVertex(new Vector4(-side, -side, side, 1.0f),   new Vector2(tx11, h)),
                new TexturedVertex(new Vector4(-side, side, side, 1.0f),    new Vector2(tx11, 0)),

                new TexturedVertex(new Vector4(side, -side, -side, 1.0f),   new Vector2(tx21, 0)),
                new TexturedVertex(new Vector4(side, side, -side, 1.0f),    new Vector2(tx20, 0)),
                new TexturedVertex(new Vector4(side, -side, side, 1.0f),    new Vector2(tx21, h)),
                new TexturedVertex(new Vector4(side, -side, side, 1.0f),    new Vector2(tx21, h)),
                new TexturedVertex(new Vector4(side, side, -side, 1.0f),    new Vector2(tx20, 0)),
                new TexturedVertex(new Vector4(side, side, side, 1.0f),     new Vector2(tx20, h)),

                new TexturedVertex(new Vector4(-side, -side, -side, 1.0f),  new Vector2(tx31, 0)),
                new TexturedVertex(new Vector4(side, -side, -side, 1.0f),   new Vector2(tx30, 0)),
                new TexturedVertex(new Vector4(-side, -side, side, 1.0f),   new Vector2(tx31, h)),
                new TexturedVertex(new Vector4(-side, -side, side, 1.0f),   new Vector2(tx31, h)),
                new TexturedVertex(new Vector4(side, -side, -side, 1.0f),   new Vector2(tx30, 0)),
                new TexturedVertex(new Vector4(side, -side, side, 1.0f),    new Vector2(tx30, h)),

                new TexturedVertex(new Vector4(-side, side, -side, 1.0f),   new Vector2(tx41, 0)),
                new TexturedVertex(new Vector4(-side, side, side, 1.0f),    new Vector2(tx40, 0)),
                new TexturedVertex(new Vector4(side, side, -side, 1.0f),    new Vector2(tx41, h)),
                new TexturedVertex(new Vector4(side, side, -side, 1.0f),    new Vector2(tx41, h)),
                new TexturedVertex(new Vector4(-side, side, side, 1.0f),    new Vector2(tx40, 0)),
                new TexturedVertex(new Vector4(side, side, side, 1.0f),     new Vector2(tx40, h)),

                new TexturedVertex(new Vector4(-side, -side, -side, 1.0f),  new Vector2(tx50, h)),
                new TexturedVertex(new Vector4(-side, side, -side, 1.0f),   new Vector2(tx51, h)),
                new TexturedVertex(new Vector4(side, -side, -side, 1.0f),   new Vector2(tx50, 0)),
                new TexturedVertex(new Vector4(side, -side, -side, 1.0f),   new Vector2(tx50, 0)),
                new TexturedVertex(new Vector4(-side, side, -side, 1.0f),   new Vector2(tx51, h)),
                new TexturedVertex(new Vector4(side, side, -side, 1.0f),    new Vector2(tx51, 0)),

                new TexturedVertex(new Vector4(-side, -side, side, 1.0f),   new Vector2(tx60, h)),
                new TexturedVertex(new Vector4(side, -side, side, 1.0f),    new Vector2(tx61, h)),
                new TexturedVertex(new Vector4(-side, side, side, 1.0f),    new Vector2(tx60, 0)),
                new TexturedVertex(new Vector4(-side, side, side, 1.0f),    new Vector2(tx60, 0)),
                new TexturedVertex(new Vector4(side, -side, side, 1.0f),    new Vector2(tx61, h)),
                new TexturedVertex(new Vector4(side, side, side, 1.0f),     new Vector2(tx61, 0)),

            };
            return vertices;
        }
    }
}

