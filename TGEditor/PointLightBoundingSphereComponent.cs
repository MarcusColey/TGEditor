using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PloobsEngine.Components;
using Microsoft.Xna.Framework;

namespace TGEditor
{
    /// <summary>
    /// Draw a bounding sphere with depth
    /// </summary>
    public class PointLightBoundingSphereComponent : IComponent
    {
        Matrix view, projection;
        public PointLightBoundingSphereComponent(Matrix view, Matrix projection)
        {
            this.view = view;
            this.projection = projection;
            DebugShapeRenderer.Initialize(EngineStart.engine.GraphicsDevice);
        }

        public override string getMyName()
        {
            string name = "PointLightBoundingSphere";
            return name;
        }

        protected override void PosWithDepthDraw(PloobsEngine.SceneControl.RenderHelper render, Microsoft.Xna.Framework.GameTime gt, ref Microsoft.Xna.Framework.Matrix activeView, ref Microsoft.Xna.Framework.Matrix activeProjection)
        {
            DebugShapeRenderer.Draw(gt, activeView, activeProjection);  // DO NOT DELETE - This call draws all debugshape primitves ( For some reason )
            base.PosWithDepthDraw(render, gt, ref activeView, ref activeProjection);
        }

        public override ComponentType ComponentType
        {
            get { return PloobsEngine.Components.ComponentType.POS_WITHDEPTH_DRAWABLE; }
        }
    }
}
