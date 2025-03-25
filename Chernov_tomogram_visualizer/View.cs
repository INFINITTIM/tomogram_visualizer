using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace Chernov_tomogram_visualizer
{
    internal class View
    {
        public void SetupView(int width, int height)
        {
            GL.ShadeModel(ShadingModel.Smooth);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Bin.X, 0, Bin.Y, -1, 1);
            GL.Viewport(0, 0, width, height);
        }

        public void DrawQuads(int layerNamber)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Begin(BeginMode.Quads);
            for (int x_coord = 0; x_coord < Bin.X; x_coord++)
            {
                for (int y_coord = 0; y_coord < Bin.Y; y_coord++)
                {
                    short value;
                    // 1 вершина
                    value = Bin.array[x_coord + y_coord * Bin.X
                        +layerNamber * Bin.Y * Bin.X];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x_coord, y_coord);
                    // 2 вершина
                    value = Bin.array[x_coord + (y_coord + 1) * Bin.X
                        + layerNamber * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x_coord, y_coord + 1);
                    // 3 вершина
                    value = Bin.array[(x_coord + 1) + (y_coord + 1) * Bin.X
                        + layerNamber * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x_coord + 1, y_coord + 1);
                    // 4 вершина
                    value = Bin.array[(x_coord + 1) + y_coord * Bin.X
                        + layerNamber * Bin.X * Bin.Y];
                    GL.Color3(TransferFunction(value));
                    GL.Vertex2(x_coord + 1, y_coord);
                }
            }
            GL.End();
        }

        public Color TransferFunction(short value)
        {
            int min = 0;
            int max = 255;
            int newVal = Clamp((value - min) * 255 / (max - min), 0, 255);
            return Color.FromArgb(255, newVal, newVal, newVal);
        }

        public int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
    }
}
