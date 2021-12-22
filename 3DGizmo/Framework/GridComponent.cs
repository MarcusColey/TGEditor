using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TGEditor
{
    public sealed class GridComponent
    {

        public bool Enabled = true;

        private int spacing;
        public int GridSpacing
        {
            get { return spacing; }
            set
            {
                spacing = value;
                ResetLines();
            }
        }

        private int gridSize = 512;

        /// <summary>
        /// Number of lines in total.
        /// </summary>
        private int nrOfLines;
        //public int NumberOfLines
        //{
        //    get { return nrOfLines; }
        //    set
        //    {
        //        nrOfLines = value;
        //        ResetLines();
        //    }
        //}

        private BasicEffect effect;
        private GraphicsDevice graphics;
        private VertexPositionColor[] vertexData;

        private Color lineColor = Color.DarkBlue;
        private Color highlightColor = Color.Blue;


        public GridComponent(GraphicsDevice device, int gridspacing)
        {
            effect = new BasicEffect(device);
            effect.VertexColorEnabled = true;
            effect.World = Matrix.Identity;

            graphics = device;

            spacing = gridspacing;

            ResetLines();
        }

        public void ResetLines()
        {
            // calculate nr of lines, +2 for the highlights, +12 for boundingbox
            nrOfLines = ((gridSize / spacing) * 4) + 2 + 12;

            List<VertexPositionColor> vertexList = new List<VertexPositionColor>(nrOfLines);

            // fill array
            for (int i = 1; i < (gridSize / spacing) + 1; i++)
            {
                vertexList.Add(new VertexPositionColor(new Vector3((i * spacing), 0, gridSize ), lineColor));
                vertexList.Add(new VertexPositionColor(new Vector3((i * spacing), 0, -gridSize), lineColor));

                vertexList.Add(new VertexPositionColor(new Vector3((-i * spacing), 0, gridSize), lineColor));
                vertexList.Add(new VertexPositionColor(new Vector3((-i * spacing), 0, -gridSize), lineColor));

                vertexList.Add(new VertexPositionColor(new Vector3(gridSize, 0, (i * spacing)), lineColor));
                vertexList.Add(new VertexPositionColor(new Vector3(-gridSize, 0, (i * spacing)), lineColor));

                vertexList.Add(new VertexPositionColor(new Vector3(gridSize, 0, (-i * spacing)), lineColor));
                vertexList.Add(new VertexPositionColor(new Vector3(-gridSize, 0, (-i * spacing)), lineColor));
            }

            // add highlights
            vertexList.Add(new VertexPositionColor(Vector3.Forward * gridSize, highlightColor));
            vertexList.Add(new VertexPositionColor(Vector3.Backward * gridSize, highlightColor));

            vertexList.Add(new VertexPositionColor(Vector3.Right * gridSize, highlightColor));
            vertexList.Add(new VertexPositionColor(Vector3.Left * gridSize, highlightColor));


            // add boundingbox
            BoundingBox box = new BoundingBox(new Vector3(-gridSize, -gridSize, -gridSize), new Vector3(gridSize, gridSize, gridSize));
            Vector3[] corners = new Vector3[8];

            box.GetCorners(corners);
            vertexList.Add(new VertexPositionColor(corners[0], lineColor));
            vertexList.Add(new VertexPositionColor(corners[1], lineColor));

            vertexList.Add(new VertexPositionColor(corners[0], lineColor));
            vertexList.Add(new VertexPositionColor(corners[3], lineColor));

            vertexList.Add(new VertexPositionColor(corners[0], lineColor));
            vertexList.Add(new VertexPositionColor(corners[4], lineColor));

            vertexList.Add(new VertexPositionColor(corners[1], lineColor));
            vertexList.Add(new VertexPositionColor(corners[2], lineColor));

            vertexList.Add(new VertexPositionColor(corners[1], lineColor));
            vertexList.Add(new VertexPositionColor(corners[5], lineColor));

            vertexList.Add(new VertexPositionColor(corners[2], lineColor));
            vertexList.Add(new VertexPositionColor(corners[3], lineColor));

            vertexList.Add(new VertexPositionColor(corners[2], lineColor));
            vertexList.Add(new VertexPositionColor(corners[6], lineColor));

            vertexList.Add(new VertexPositionColor(corners[3], lineColor));
            vertexList.Add(new VertexPositionColor(corners[7], lineColor));

            vertexList.Add(new VertexPositionColor(corners[4], lineColor));
            vertexList.Add(new VertexPositionColor(corners[5], lineColor));

            vertexList.Add(new VertexPositionColor(corners[4], lineColor));
            vertexList.Add(new VertexPositionColor(corners[7], lineColor));

            vertexList.Add(new VertexPositionColor(corners[5], lineColor));
            vertexList.Add(new VertexPositionColor(corners[6], lineColor));

            vertexList.Add(new VertexPositionColor(corners[6], lineColor));
            vertexList.Add(new VertexPositionColor(corners[7], lineColor));


            // convert to array for drawing
            vertexData = vertexList.ToArray();
        }

        public void Draw3D()
        {
            if (Enabled)
            {
                graphics.DepthStencilState = DepthStencilState.Default;

                effect.View = Engine.View;
                effect.Projection = Engine.Projection;

                

                effect.CurrentTechnique.Passes[0].Apply();
                EngineStart.engine.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                EngineStart.engine.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
                graphics.DrawUserPrimitives(PrimitiveType.LineList, vertexData, 0, nrOfLines);                                    
            }
        }
    }
}
