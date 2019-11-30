using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTextures
{
    public struct ColoredVertex
    {
        public const int Size = (4 + 4 + 3) * 4; // size of struct in bytes

        private readonly Vector4 _position;
        private readonly Color4 _color;
        private readonly Vector3 _normal;

        public ColoredVertex(Vector4 position, Color4 color, Vector3 normal)
        {
            _position = position;
            _color = color;
            _normal = normal;
        }
    }
}
