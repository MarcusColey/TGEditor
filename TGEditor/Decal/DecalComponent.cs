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
    public class DecalComponent : IComponent
    {
        public DecalComponent()
            :base(int.MinValue)
        {
        }

        private Effect effect;
        public List<Decal> Decals = new List<Decal>();
        
        public override ComponentType ComponentType
        {
            get { return PloobsEngine.Components.ComponentType.POS_WITHDEPTH_DRAWABLE; }
        }

        public static readonly String MyName = "Decal";
        public override string getMyName()
        {
            return MyName;
        }

        GraphicInfo GraphicInfo;
        protected override void LoadContent(PloobsEngine.Engine.GraphicInfo GraphicInfo, PloobsEngine.Engine.GraphicFactory factory)
        {
            //effect = EngineStart.engine.Content.Load<Effect>("Effects/decal"); 
            //base.LoadContent(GraphicInfo, factory);
            //this.GraphicInfo = GraphicInfo;
        }

        protected override void PosWithDepthDraw(PloobsEngine.SceneControl.RenderHelper render, Microsoft.Xna.Framework.GameTime gt, ref Microsoft.Xna.Framework.Matrix activeView, ref Microsoft.Xna.Framework.Matrix activeProjection)
        {                        
            /*effect.Parameters["InvProj"].SetValue(Matrix.Invert(activeView * activeProjection));
            effect.Parameters["halfPixel"].SetValue(GraphicInfo.HalfPixel);
            effect.Parameters["DepthBuffer"].SetValue(render[PrincipalConstants.DephRT]);

            render.PushBlendState(BlendState.AlphaBlend);
            foreach (Decal item in Decals)
            {
                effect.Parameters["ProjectedTexture"].SetValue(item.tex);
                effect.Parameters["ProjectorViewProjection"].SetValue(item.view * item.Projection);
                render.RenderFullScreenQuadVertexPixel(effect);
            }
            render.PopBlendState();

            base.PosWithDepthDraw(render, gt, ref activeView, ref activeProjection);

            render.SetSamplerStates(GraphicInfo.SamplerState, 6);*/
        }
    }
}

