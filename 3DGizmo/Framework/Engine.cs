using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PloobsEngine.Components;

namespace TGEditor
{
    public class Engine : IComponent
    {
        public override string getMyName()
        {
            string name = "GizmoEnginePloobsComponent";
            return name;
        }

        public override ComponentType ComponentType
        {
            get { return ComponentType.POS_WITHDEPTH_DRAWABLE; }
        }

        public static List<SceneEntity> Entities = new List<SceneEntity>();

        public static Matrix View;
        public static Matrix Projection;
        public static Vector3 CameraPosition;

        public static GraphicsDevice Graphics;

        public static void SetupEngine(GraphicsDevice graphics)
        {
            Graphics = graphics;
        }

        public static void Update()
        {
            foreach (SceneEntity entity in Entities)
            {
                entity.Update();
            }
        }

        public static void Draw()
        {
            //foreach (SceneEntity entity in Entities)
            //{
            //    entity.Draw();
            //}
        }

        protected override void PosWithDepthDraw(PloobsEngine.SceneControl.RenderHelper render, GameTime gt, ref Matrix activeView, ref Matrix activeProjection)
        {
            foreach (SceneEntity entity in Entities)
            {
                entity.Draw();
            }

            base.PosWithDepthDraw(render, gt, ref activeView, ref activeProjection);
        }
    }
}
