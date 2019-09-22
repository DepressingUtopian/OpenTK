using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLCurs
{
    public class Cube
    {
        float scale = 1.0f;
        
        RenderObject.Vertex[] vertices =
           {
              new RenderObject.Vertex(new Vector4(-0.25f, 0.25f, 0.5f, 1-0f), Color4.HotPink),
              new RenderObject.Vertex(new Vector4( 0.0f, -0.25f, 0.5f, 1-0f), Color4.HotPink),
              new RenderObject.Vertex(new Vector4( 0.25f, 0.25f, 0.5f, 1-0f), Color4.HotPink),
           };
        public Cube()
        {
            
        }
        public void ScaleVertices()
        {
            foreach (RenderObject.Vertex vertex in vertices)
            {
          
            }
        }
    }
  
}
