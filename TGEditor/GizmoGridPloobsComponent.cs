using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PloobsEngine.Components;
using Microsoft.Xna.Framework.Graphics;
using PloobsEngine;
using PloobsEngine.Engine;
using Microsoft.Xna.Framework;


namespace TGEditor
{
    /// <summary>
    /// Draws a grid UNDER objects in the scene
    /// </summary>
    public class GizmoGridPloobsComponent : IComponent
    {
        // Refer to class summary for info about whats going on, the rest is fairly self explanitory

        GridComponent gridComponent;

        public bool Enabled
        {
            get { return gridComponent.Enabled; }

            set { gridComponent.Enabled = value; }
        }

        public GizmoGridPloobsComponent(GridComponent gridComponent)
            : base()
        {
            this.gridComponent = gridComponent;
        }

        protected override void PosWithDepthDraw(PloobsEngine.SceneControl.RenderHelper render, GameTime gt, ref Matrix activeView, ref Matrix activeProjection)
        {
            gridComponent.Draw3D();

            base.PosWithDepthDraw(render, gt, ref activeView, ref activeProjection);
        }

        public override string getMyName()
        {
            string name = "GizmoGridComponent";
            return name; 
        }

        public override ComponentType ComponentType
        {
            get { return PloobsEngine.Components.ComponentType.POS_WITHDEPTH_DRAWABLE; }
        }


    }
}
