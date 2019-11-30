using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKWater
{
    public class Water
    {
        private const int N = 80;
        private float K = 0.06f;
        private float DT = 0.1f;
        private int offs = 0;
        float[] a;
        public Point[,] points;
        Random random = new Random();

        public Water()
        {
            points = new Point[N,N];
            a = new float[N * N * 12];
            InitPoints();
        }
        public OpenTKPrimitives.RenderObject.Vertex[] GenWater(Color4 color)
        {
            List<OpenTKPrimitives.RenderObject.Vertex> vertices = new List<OpenTKPrimitives.RenderObject.Vertex>();
            Timer();
            foreach(var point in points)
            {
                OpenTKPrimitives.RenderObject.Vertex vertex = new OpenTKPrimitives.RenderObject.Vertex(new Vector4(point.x, point.y, point.z, 1.0f), color);
                vertices.Add(vertex);
            }
            return vertices.ToArray();
        }

        public void InitPoints()
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    points[i, j] = new Point();
                    points[i, j].x = 1.0f * j / N;
                    points[i, j].y = 1.0f * i / N;
                    points[i, j].z = 0;
                    points[i, j].vz = 0;
                }
        }
        private void Push()
        {
            if (random.NextDouble() * 500 > 10)
                return;
            int x0 = (int)(random.NextDouble() * N / 2 + 1);
            int y0 = (int)(random.NextDouble() * N / 2 + 1);
            for (int y = y0 - 5; y < y0 + 5; y++)
            {
                if (y < 1 || (y >= N - 1))
                    continue;
                for (int x = x0 - 5; x < x0 + 5; x++)
                {
                    if ((x < 1) || (x >= N - 1)) continue;
                    points[x, y].z = 10.0f / N - (float)(Math.Sqrt(Sqr(y - y0) + Sqr(x -
                   x0)) * 1.0 / N);


                }
            }
        }
        public void Push(double xc,double yc)
        {
            if (random.NextDouble() * 500 > 10)
                return;
            int x0 = (int)xc;
            int y0 = (int)yc;
            for (int y = y0 - 5; y < y0 + 5; y++)
            {
                if (y < 1 || (y >= N - 1))
                    continue;
                for (int x = x0 - 5; x < x0 + 5; x++)
                {
                    if ((x < 1) || (x >= N - 1)) continue;
                    points[x, y].z = 10.0f / N - (float)(Math.Sqrt(Sqr(y - y0) + Sqr(x -
                   x0)) * 1.0 / N);


                }
            }
        }
        private void display()
        {
            offs = 0;
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N - 1; j++)
                {
                    a[N * i * 3 * 2 + j * 3 * 2 + 0] = 1.0f * j / N;
                    a[N * i * 3 * 2 + j * 3 * 2 + 1] = 1.0f * i / N;
                    a[N * i * 3 * 2 + j * 3 * 2 + 2] = 1.0f * points[i, j].z;
                    a[N * i * 3 * 2 + j * 3 * 2 + 3] = 1.0f * (j + 1) / N;
                    a[N * i * 3 * 2 + j * 3 * 2 + 4] = 1.0f * i / N;
                    a[N * i * 3 * 2 + j * 3 * 2 + 5] = 1.0f * points[i, j + 1].z;
                    offs += 6;
                }
            for (int i = 0; i < N - 1; i++)
                for (int j = 0; j < N; j++)
                {
                    a[offs + N * i * 3 * 2 + j * 3 * 2 + 0] = 1.0f * j / N;
                    a[offs + N * i * 3 * 2 + j * 3 * 2 + 1] = 1.0f * i / N;
                    a[offs + N * i * 3 * 2 + j * 3 * 2 + 2] = 1.0f * points[i, j].z;
                    a[offs + N * i * 3 * 2 + j * 3 * 2 + 3] = 1.0f * j / N;
                    a[offs + N * i * 3 * 2 + j * 3 * 2 + 4] = 1.0f * (i + 1) / N;
                    a[offs + N * i * 3 * 2 + j * 3 * 2 + 5] = 1.0f * points[i + 1, j].z;
                }
        }
        public void Timer()
        {
            int[] dx = { -1, 0, 1, 0 };
            int[] dy = { 0, 1, 0, -1 };
            Push();
            for (int y = 1; y < N - 1; ++y)
            {
                for (int x = 1; x < N - 1; ++x)
                {

                    Point p0 = points[x, y];
                    for (int i = 0; i < 4; ++i)
                    {
                        Point p1 = points[x + dx[i], y + dy[i]];
                        float d = (float)Math.Sqrt(Sqr(p0.x - p1.x) + Sqr(p0.y - p1.y)
                       + Sqr(p0.z - p1.z));
                        p0.vz += K * (p1.z - p0.z) / d * DT;
                        p0.vz *= 0.99f;
                    }
                }
            }
            for (int y = 1; y < N - 1; ++y)
                for (int x = 1; x < N - 1; ++x)
                {
                    Point p0 = points[x, y];
                    p0.z += p0.vz;
                }
            display();
        }
        private float Sqr(float x)
        {
            return x * x;
        }
    }
}
