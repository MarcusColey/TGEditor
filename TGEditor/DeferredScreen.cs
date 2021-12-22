using Microsoft.Xna.Framework;
using PloobsEngine.Cameras;
using PloobsEngine.Light;
using PloobsEngine.Material;
using PloobsEngine.Modelo;
using PloobsEngine.Physics;
using PloobsEngine.Physics.Bepu;
using PloobsEngine.SceneControl;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using PloobsEngine.Engine;
using Microsoft.Xna.Framework.Content;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System;
using System.Collections.Generic;
using PloobsEngine.Features.DebugDraw;

namespace TGEditor
{
    /// <summary>
    /// Basic Deferred Scene    
    /// </summary>
    public class DeferredScreen : IScene
    {
        #region Fields
        // Default/Manditory
        Editor editorForm;
        GraphicsDeviceManager graphics;
        Form initialWindow;
        EngineStuff engine;
        public static EditorCamera camera;
        GizmoGridPloobsComponent gizmoGridPloobsCom;

        //Gizmo fields
        Engine gizmoEngine;
        public static InputState gizmoInput;
        public static GizmoComponent gizmoComponent;
        public static GridComponent gizmoGridComponent;
        StringBuilder helpTextBuilder;
        string helpText;
        SpriteFont helpTextFont;

        //Framerate counter variables
        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        // Ray that represents the mouses position in world space
        public static Ray mouseRay;

        // Check booleans - eg. Wheather to draw the grid, if true draw grid
        public static bool drawBox;
        public static bool drawGrid;
        public static bool deleteObject;
        public static bool isOverXnaDisplayPanel;
        public static bool isOverPropertiesPanel;

        PointLightBoundingSphereComponent drawableSphere;
        /*bool hasSelectedPoint;
        bool hasCreatedBase;
        bool collideBoxCreation;
        Vector3 boxStartPosition;
        BoundingBox boundingBox;*/
        Quad quad;

        public List<BoundingBox> collideCallbackList = new List<BoundingBox>();

        RenderHelper renderHelper;
        bool hasSetRenderHelper;

        //Fields for the form - These ensure the picking ray is drawn correctly, and other stuff
        Viewport XnaDisplayPanelViewport;
        RenderTarget2D DisplayPanelRenderTarget;

        public static DecalComponent decalComponent;

        #endregion
        
        // Default constructor
        public DeferredScreen(GraphicsDeviceManager graphics, Editor editor)
        {
            this.graphics = graphics;
            this.editorForm = editor;
        }

        // Initializes all neccesary things
        protected override void InitScreen(PloobsEngine.Engine.GraphicInfo GraphicInfo, PloobsEngine.Engine.EngineStuff engine)
        {
            this.engine = engine;
            Engine.SetupEngine(this.graphics.GraphicsDevice); // Engine for the gizmo component
     
            initialWindow = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(engine.Window.Handle);
            initialWindow.GotFocus += new System.EventHandler(initialWindow_GotFocus);
            editorForm.Resize += new System.EventHandler(Panel_Resize);        // This might be the cause of erros in the release version
            editorForm.XnaMainDisplayPanel.MouseEnter += new EventHandler(XnaMainDisplayPanel_MouseEnter);
            editorForm.XnaMainDisplayPanel.MouseLeave += new EventHandler(XnaMainDisplayPanel_MouseLeave);
            editorForm.PropertiesPanel.Click += new System.EventHandler(PropertiesPanel_Click);
            //editorForm.TopMost = true;
            editorForm.Show();
            gizmoInput = new InputState(this.graphics.GraphicsDevice);

            XnaDisplayPanelViewport = new Viewport();
            XnaDisplayPanelViewport.X = editorForm.XnaMainDisplayPanel.Bounds.X;
            XnaDisplayPanelViewport.Y = editorForm.XnaMainDisplayPanel.Bounds.Y;
            XnaDisplayPanelViewport.Width = editorForm.XnaMainDisplayPanel.Width;
            XnaDisplayPanelViewport.Height = editorForm.XnaMainDisplayPanel.Height + editorForm.MainMenuStrip.Height;
            XnaDisplayPanelViewport.MaxDepth = engine.GraphicsDevice.Viewport.MaxDepth;
            XnaDisplayPanelViewport.MinDepth = engine.GraphicsDevice.Viewport.MinDepth;

            base.InitScreen(GraphicInfo, engine);
        }

        
        protected override void SetWorldAndRenderTechnich(out IRenderTechnic renderTech, out IWorld world)
        {
            ///create the world using bepu as physic api and a simple culler implementation
            ///IT DOES NOT USE PARTICLE SYSTEMS (see the complete constructor, see the ParticleDemo to know how to add particle support)
            BepuPhysicWorld physicWorld = new BepuPhysicWorld();
            world = new IWorld(physicWorld, new OctreeCuller(12000, 18, 5, Vector3.Zero), null, true);
            physicWorld.isDebugDraw = true;
        
            ///Create the deferred description
            DeferredRenderTechnicInitDescription desc = DeferredRenderTechnicInitDescription.Default();
            ///Some custom parameter, this one allow light saturation. (and also is a pre requisite to use hdr)
            desc.UseFloatingBufferForLightMap = true;
            ///set background color, default is black
            desc.BackGroundColor = Color.CornflowerBlue;
            ///create the deferred technich
            renderTech = new DeferredRenderTechnic(desc);
        }
        
        // All content is loaded here
        protected override void LoadContent(PloobsEngine.Engine.GraphicInfo GraphicInfo, PloobsEngine.Engine.GraphicFactory factory, IContentManager contentManager)
        {
            contentManager.ContentManager.RootDirectory = "TGEditor-CContent";
            decalComponent = new DecalComponent();
            engine.AddComponent(decalComponent);
            ///must be called before all
            base.LoadContent(GraphicInfo, factory, contentManager);
            
         
            gizmoComponent = new GizmoComponent(new ContentManager(engine.Services), this.graphics.GraphicsDevice, editorForm, camera); // instantiate the gizmo component
            gizmoComponent.Initialize();
            gizmoGridComponent = new GridComponent(this.graphics.GraphicsDevice, 30);
            gizmoGridPloobsCom = new GizmoGridPloobsComponent(gizmoGridComponent);
            engine.AddComponent(gizmoGridPloobsCom);

            helpTextBuilder = new StringBuilder();
            helpTextBuilder.AppendLine();
            helpTextBuilder.AppendLine();
            helpTextBuilder.AppendLine();
            helpTextBuilder.AppendLine("Hotkeys:");

            helpTextBuilder.AppendLine("Q,W,E,R to switch Transformation Modes");
            helpTextBuilder.AppendLine("Spacebar = Switch space (Local/World)");
            helpTextBuilder.AppendLine("Hold LeftShift = Precision Mode");
            helpTextBuilder.AppendLine("? = Toggle Snapping");
            helpTextBuilder.AppendLine("P - ? = Switch PivotTypes");
            helpTextBuilder.AppendLine("LeftControl = Add to selection");
            helpTextBuilder.AppendLine("LeftAlt = Remove from selection");

            helpText = helpTextBuilder.ToString();
            helpTextFont = contentManager.ContentManager.Load<SpriteFont>("3DGizmoContent/gizmoFont");

            //Loads and displays a model (If uncommented)
            #region PloobsModelLoadingSample
            ///Create a simple object
            ///Geomtric Info and textures (this model automaticaly loads the texture)
            //SimpleModel simpleModel = new SimpleModel(factory, "Models/Pallet_GameReady_Complete");
            ///Physic info (postion, rotation and scale are set here)
            //boxPhysObj = new BoxObject(Vector3.Zero, simpleModel.GetModelRadius(), simpleModel.GetModelRadius(), simpleModel.GetModelRadius(), 10, Vector3.One, Matrix.Identity, MaterialDescription.DefaultBepuMaterial());
            //IPhysicObject physObj = boxPhysObj;
            //physObj.isMotionLess = true;
            ///Shader info (must be a deferred type -- BECAUSE OF THE RENDER TECHNICH)
            //DeferredNormalShader shader = new DeferredNormalShader();
            ///Material info (must be a deferred type also)
            //DeferredMaterial fmaterial = new DeferredMaterial(shader);
            ///The object itself
            //IObject obj = new IObject(fmaterial, simpleModel, physObj);
            ///Add to the world
            //this.World.AddObject(obj);
            #endregion

            ///Add some directional lights
            #region Lights
            DirectionalLightPE ld1 = new DirectionalLightPE(Vector3.Left, Color.White);
            DirectionalLightPE ld2 = new DirectionalLightPE(Vector3.Right, Color.White);
            DirectionalLightPE ld3 = new DirectionalLightPE(Vector3.Backward, Color.White);
            DirectionalLightPE ld4 = new DirectionalLightPE(Vector3.Forward, Color.White);
            DirectionalLightPE ld5 = new DirectionalLightPE(Vector3.Down, Color.White);
            float li = 0.4f;
            ld1.LightIntensity = li;
            ld2.LightIntensity = li;
            ld3.LightIntensity = li;
            ld4.LightIntensity = li;
            ld5.LightIntensity = li;
            ld5.ProjMatrix = Matrix.Identity;
            ld5.ViewMatrix = Matrix.Identity;
            this.World.AddLight(ld1);
            this.World.AddLight(ld2);
            this.World.AddLight(ld3);
            this.World.AddLight(ld4);
            this.World.AddLight(ld5);
            #endregion

            ///Add a post effect
            this.RenderTechnic.AddPostEffect(new AntiAliasingPostEffect());

            ///add a camera
            this.World.CameraManager.AddCamera(camera = new EditorCamera(engine, editorForm, new Vector3(225, 225, 225), Vector3.Zero, Vector3.Up));
           
            // This component handles all the bounding object with 'debug shape renderer' class and draws them with depth
            drawableSphere = new PointLightBoundingSphereComponent(camera.view, camera.projection);
            engine.AddComponent(drawableSphere);
            
            //Engine for the gizmo component
            gizmoEngine = new Engine();
            engine.AddComponent(gizmoEngine);

            
            Texture2D texture = engine.Content.Load<Texture2D>("Textures/GarbageTexture_GameReady");
            Matrix view = camera.view;
            Matrix projection = camera.projection;
            Decal shit = new Decal(texture, Matrix.CreateLookAt(
                    new Vector3(500, 500, 700),
                    new Vector3(-200, 50, -10),
                    Vector3.Up
                    ), Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, camera.AspectRatio, 1, 2000));
            decalComponent.Decals.Add(shit);
        }

        // Converts mouse position to a position in world space
        Ray ConvertMouseToRay(Vector2 mousePosition)
        { 
            /*Viewport vp;
            vp = new Viewport(editorForm.XnaMainDisplayPanel.Location.X, editorForm.XnaMainDisplayPanel.Location.Y, editorForm.XnaMainDisplayPanel.Width, editorForm.XnaMainDisplayPanel.Height);
            vp.MinDepth = 0;
            vp.MaxDepth = 1;
            try
            {
                engine.GraphicsDevice.Viewport = vp;
            }
            catch (ArgumentException viewportArgsException)
            {
                EngineStart.desc.Logger.Log("Exception: " + viewportArgsException.ToString() + " Something is wrong with the viewport, \n most likely a portion of the viewport is out of parenet bounds",
                    PloobsEngine.Engine.Logger.LogLevel.FatalError);
            }*/
            
            Vector3 nearPoint = new Vector3((float)mousePosition.X, (float)mousePosition.Y, 0);
            Vector3 farPoint = new Vector3((float)mousePosition.X, (float)mousePosition.Y, 1);

            nearPoint = engine.GraphicsDevice.Viewport.Unproject(nearPoint,
                camera.Projection,
                camera.View,
                Matrix.Identity);
            farPoint = engine
                .GraphicsDevice.Viewport.Unproject(farPoint,
                camera.Projection,
                camera.View,
                Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }

        public static MouseState currMouse;
        protected override void Update(GameTime gameTime)
        {
            //editorForm.Panel2.Focus(); // Focus's panel2 so mouse wheel scrolling works properly after panel2 is out of focus
                                                            // ***
            Mouse.WindowHandle = engine.GraphicsDevice.PresentationParameters.DeviceWindowHandle;  //DO NOT DELETE, THIS IS STATIC, AND IS USED IN GIZMOCOMPONENT CLASS
            currMouse = Mouse.GetState();                   // ***
            // Draw ray that represents mouse position in world space ONLY is it is in debug mode - This has to be changed manually
            if(EngineStart.isDebug)
                mouseRay = ConvertMouseToRay(new Vector2(currMouse.X + 3, currMouse.Y + 1));
            
            // Set up the camera for the gizmo component
            Engine.CameraPosition = camera.cameraPosition;
            Engine.View = camera.View;
            Engine.Projection = camera.Projection;
   
            gizmoInput.Update(gameTime);

            Engine.Update();
            
            gizmoComponent.HandleInput(gizmoInput);
            gizmoComponent.Update(gameTime);

            editorForm.UpdateForm(gameTime, renderHelper);

            // Move the buttons at the bottom of the form as a group
            for (int i = 0; i < Editor.bottomFormButtons.Count; i++)                                                          //
            {                                                                                                           // ***
                if (editorForm.Width == 874 && editorForm.Height == 720 || editorForm.Size == editorForm.MaximumSize)   // Sets the buttons back to default POS. when form is maximized
                    Editor.bottomFormButtons[i].Location = new System.Drawing.Point(Editor.bottomFormButtons[i].Location.X, 633);   // ***
            }                                                                                                           //

            //Framerate counter
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }

            base.Update(gameTime);
        }

        // Used to turn the grid on ad off
        public static KeyboardState currKeyboard;
        public static KeyboardState oldKeyboard;
        
        protected override void Draw(GameTime gameTime, RenderHelper render)
        {
            ///must be called before    
            base.Draw(gameTime, render);

            /*SimpleModel model = new SimpleModel(this.GraphicFactory, "3DGizmoContent/box");
            BoxObject modelPhysObject = new BoxObject(Vector3.Zero, 100, 100, 100, 10f, Vector3.One, Matrix.Identity, MaterialDescription.DefaultBepuMaterial());
            modelPhysObject.isMotionLess = true;
            BoxLight desc = new BoxLight(Color.Green, Color.White, Color.Red);
            BoxLightShader shader = new BoxLightShader(desc);
            
            DeferredMaterial material = new DeferredMaterial(shader);
            IObject modelObject = new IObject(material, model, modelPhysObject);
            shader.Initialize(this.GraphicInfo, this.GraphicFactory, modelObject);*/

            /*this.World.AddObject(modelObject);
            BoundingBox bb;
            DebugShapeRenderer.AddBoundingBox(bb = new BoundingBox(modelPhysObject.BoundingBox.Value.Min, modelPhysObject.BoundingBox.Value.Max), Color.Red);*/
            

            if(EngineStart.isDebug)
                RayRenderer.Render(mouseRay, 5000, engine.GraphicsDevice, camera.view, camera.projection, Color.Red);

            // Checks wheather certain keys are perssed or not
            currKeyboard = Keyboard.GetState();
            drawGrid = KeypressTest(Microsoft.Xna.Framework.Input.Keys.G, currKeyboard, oldKeyboard);
            drawBox = KeypressTest(Microsoft.Xna.Framework.Input.Keys.B, currKeyboard, oldKeyboard);
            deleteObject = KeypressTest(Microsoft.Xna.Framework.Input.Keys.Delete, currKeyboard, oldKeyboard);
            
            if (drawGrid)
            {
                gizmoGridPloobsCom.Enabled = !gizmoGridPloobsCom.Enabled;
            }

            // Handles the removal of objects when the 'delete' key is pressed
            if (GizmoComponent.Selection.Count > 0) // Deletes the currently selected object
            {
                if (deleteObject)
                {
                    foreach (SceneEntity selectedObject in GizmoComponent.Selection)
                    {
                        if(!selectedObject.IsPointlightEntity && !selectedObject.IsCollideObject)
                        {
                            //this.World.RemoveObject(selectedObject.modelObject);
                            Engine.Entities.Remove(selectedObject);
                            this.World.RemoveObject(selectedObject.modelObject);
                            Editor.changesHaveBeenMade = true;
                        }
                        else if(selectedObject.IsPointlightEntity)
                        {                           
                            Engine.Entities.Remove(selectedObject);
                            this.World.RemoveLight(selectedObject.PointLightObject);
                            GizmoComponent.Selection.Remove(selectedObject);
                            Editor.changesHaveBeenMade = true;
                        }
                        else if (selectedObject.IsCollideObject)
                        {                            
                            Engine.Entities.Remove(selectedObject);
                            this.World.RemoveObject(selectedObject.modelObject);
                            GizmoComponent.Selection.Remove(selectedObject);
                            Editor.changesHaveBeenMade = true;
                        }

                    }
                }
            }
           
            SpriteBatch spriteBatch = new SpriteBatch(this.graphics.GraphicsDevice);
          
            Engine.Draw();

            gizmoComponent.Draw3D();
            gizmoComponent.DrawUI(spriteBatch);

            frameCounter++;
            string fps = string.Format("fps: {0}", frameRate);


            // Draws Editor/GizmoComponoent help text/Framerate and more
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            spriteBatch.DrawString(helpTextFont, helpText, new Vector2(5), Color.White);

            //Framerate counter text
            spriteBatch.DrawString(helpTextFont, fps, new Vector2(10, 10), Color.Black);
            spriteBatch.DrawString(helpTextFont, fps, new Vector2(10, 10), Color.White);


            spriteBatch.End();

            //SimpleModel shit = new SimpleModel(this.GraphicFactory, "3DGizmocontent/box");
            //TriangleMeshObject shitmesh = new TriangleMeshObject(shit, Vector3.Zero, Matrix.Identity, Vector3.One, MaterialDescription.DefaultBepuMaterial());
       
            ///Draw some text to the screen
            int height = editorForm.XnaMainDisplayPanel.Height + editorForm.MainMenuStrip.Height; // DOTHIS!
            render.RenderTextComplete("TGEditor" + Mouse.GetState().X.ToString() 
                + Mouse.GetState().Y.ToString() +"\n" 
                + engine.GraphicsDevice.Viewport.Bounds.ToString() + "\n"
                + editorForm.XnaMainDisplayPanel.Width.ToString() + " - " 
                + height.ToString() + "\n"
                //+ (shitmesh.BoundingBox.Value.Max - shitmesh.BoundingBox.Value.Min).ToString()
                , new Vector2(GraphicInfo.Viewport.Width - 315, 15), Color.White, Matrix.Identity);

            //DebugShapeRenderer.AddBoundingFrustum(camera.BoundingFrustum, Color.Green);

            /*if (true)
            {
                if (gizmoInput.Mouse.IsButtonDown(MouseButtons.Left) && collideBoxCreation != true)
                {
                    Plane gridPlane = new Plane(new Vector4(0, 1, 0, 0));
                    if (mouseRay.Intersects(gridPlane).HasValue)
                    {
                        Vector3 intersectionPoint = mouseRay.Position + mouseRay.Direction * mouseRay.Intersects(gridPlane).Value;
                        BoundingSphere sphere = new BoundingSphere(intersectionPoint, 10f);
                        DebugShapeRenderer.AddBoundingSphere(sphere, Color.Red);

                        if (hasSelectedPoint != true)
                        {
                            boxStartPosition = intersectionPoint;
                            hasSelectedPoint = true;
                        }

                        boundingBox = new BoundingBox(boxStartPosition, intersectionPoint);
                        DebugShapeRenderer.AddBoundingBox(boundingBox, Color.Green);
                    }
                    hasCreatedBase = true;
                }
                else if(gizmoInput.Mouse.IsButtonUp(MouseButtons.Left) && hasCreatedBase)
                {
                    collideBoxCreation = true;
                    Plane cameraDirectionInvert = new Plane(Vector3.Negate(camera.GetCameraDirection), 0);

                    Vector3 intersectionPoint = Vector3.Zero;
                    
                    if (mouseRay.Intersects(cameraDirectionInvert).HasValue)
                        intersectionPoint = mouseRay.Position + mouseRay.Direction * mouseRay.Intersects(cameraDirectionInvert).Value;

                     BoundingSphere boundingSphere = new BoundingSphere(intersectionPoint, 10f);
                        DebugShapeRenderer.AddBoundingSphere(boundingSphere, Color.Red);

                    if (intersectionPoint.Y <= 0)
                        intersectionPoint.Y = 0;
                        
                    DebugShapeRenderer.AddBoundingBox(boundingBox = new BoundingBox(boundingBox.Min, new Vector3(boundingBox.Max.X, intersectionPoint.Y, boundingBox.Max.Z)), Color.Red);
                }
                else if(gizmoInput.Mouse.IsButtonDown(MouseButtons.Left) && hasCreatedBase)
                {
                    collideCallbackList.Add(boundingBox);
                    hasCreatedBase = false;
                    collideBoxCreation = false;
                    hasSelectedPoint = false;
                    Thread.Sleep(100);
                }
            }

            foreach (BoundingBox boundingBox in collideCallbackList)
            {
                DebugShapeRenderer.AddBoundingBox(new BoundingBox(boundingBox.Min, boundingBox.Max), Color.Purple);
            }*/

            /*quad = new Quad(Vector3.Zero, Vector3.UnitY, Vector3.Right, 100, 100);
            BasicEffect quadEffect = new BasicEffect(engine.GraphicsDevice);
            quadEffect.View = camera.view;
            quadEffect.Projection = camera.Projection;
            quadEffect.World = Matrix.Identity;
            quadEffect.LightingEnabled = true;
            //quadEffect.EnableDefaultLighting();
            quadEffect.TextureEnabled = true;
            Texture2D texture = engine.Content.Load<Texture2D>("Textures/AtticFloor_Planks_GmaeReady_700");
            quadEffect.Texture = texture;

            foreach (EffectPass pass in quadEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                engine.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, quad.Vertices, 0, 4, quad.Indexes, 0, 2);
            }*/
            
            // **Do not delete!**
            //try
            //{
            //    engine.GraphicsDevice.Viewport = new Viewport(0, 0, editorForm.XnaMainDisplayPanel.Width, editorForm.XnaMainDisplayPanel.Height); // This makes sure the picking ray is in the correct position!
            //}
            //catch (ArgumentException viewportException)
            //{
            //    EngineStart.desc.Logger.Log(viewportException.ToString() + "Window was minimized or the viewport was out of the parent screen/window bounds", PloobsEngine.Engine.Logger.LogLevel.Warning);
            //}
            // **Do not delete!**

            // Rasterstate is set to non so plane object can be drawn properly, Find a fix!!!
            //engine.GraphicsDevice.RasterizerState = RasterizerState.CullNone; // - TODO: Find a fiz for the raster state - null is very memory consuming
            //this.World.AddLight(new BoxLight());
            oldKeyboard = currKeyboard;
        }

        void Panel_Resize(object sender, System.EventArgs e)      // May not need this after all - This might be the cause if errors in release version
        {
            int formSizeDeltaY = editorForm.Height - editorForm.ClientRectangle.Height;
            int formSizeDeltaX = editorForm.Width - editorForm.ClientRectangle.Width;
            //int panelDelta = editorForm.PropertiesPanelHolder.Width - editorForm.PropertiesPanelHolder.ClientRectangle.Width;

            //Changes the position of the form buttons according to the size of the window
            for (int i = 0; i < Editor.bottomFormButtons.Count; i++)
            {
                System.Drawing.Point buttonLocation = Editor.bottomFormButtons[i].Location;
                buttonLocation = new System.Drawing.Point(buttonLocation.X, (editorForm.MainMenuStrip.Height + editorForm.XnaMainDisplayPanel.Height) + 10); // Fix location X!
                Editor.bottomFormButtons[i].Location = buttonLocation;
            }

            //editorForm.SplitContainer.Width = editorForm.Width - editorForm.ListBox.Width - 85;
            //editorForm.SplitContainer.Height = editorForm.Height - 120;
            camera.AspectRatio = (float)editorForm.XnaMainDisplayPanel.Width / (float)editorForm.XnaMainDisplayPanel.Height;  //May be causing an error in release version
            //editorForm.PropertiesPanel.Width = editorForm.PropertiesPanelHolder.Width;
            //editorForm.PropertiesPanel.Height = editorForm.PropertiesPanelHolder.Height;
            //editorForm.XnaMainDisplayPanel.Width = editorForm.Width - 50;
            //editorForm.XnaMainDisplayPanel.Height = editorForm.Height - 50;
        }

        void initialWindow_GotFocus(object sender, System.EventArgs e)
        {
            initialWindow.Visible = false;
            initialWindow.TopMost = false;
        }

        void XnaMainDisplayPanel_MouseLeave(object sender, EventArgs e)
        {
            isOverXnaDisplayPanel = false;
        }

        void XnaMainDisplayPanel_MouseEnter(object sender, EventArgs e)
        {
            isOverXnaDisplayPanel = true;
            editorForm.XnaMainDisplayPanel.Focus();
        }


        void PropertiesPanel_Click(object sender, System.EventArgs e)
        {
            isOverPropertiesPanel = true;
        }

        // Detects unique key presses
        public static bool KeypressTest(Microsoft.Xna.Framework.Input.Keys theKey, KeyboardState currKeyboard, KeyboardState oldKeyboard)
        {
            if (currKeyboard.IsKeyUp(theKey) && oldKeyboard.IsKeyDown(theKey))
                return true;
            else
                return false;
        }

        void ResetRenderState()
        {

            engine.GraphicsDevice.DepthStencilState = DepthStencilState.None;

            engine.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            engine.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            engine.GraphicsDevice.BlendState = BlendState.Opaque;

            engine.GraphicsDevice.DepthStencilState = DepthStencilState.None;

        }

        /// <summary>
        /// Clean up the resources you used
        /// Just CLEAN the global resources (those that are global but you dont want to share, like Entity mapper, MessageSystem,Skybox)
        /// You will see how they work in the examples
        /// </summary>
        /// <param name="engine"></param>
        protected override void CleanUp(PloobsEngine.Engine.EngineStuff engine)
        {
            base.CleanUp(engine);
        }
    }
}

