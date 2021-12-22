using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PloobsEngine.Modelo;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using PloobsEngine.Light;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Threading;
using System.IO;
using PloobsEngine.Engine.Logger;
using PloobsEngine.Trigger;
using PloobsEngine.Physics;
using PloobsEngine.Physics.Bepu;
using PloobsEngine.Material;
using PloobsEngine.SceneControl;
using PloobsEngine.Audio;
using BEPUphysics.CollisionShapes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TGEditor
{
    public partial class Editor : Form
    {
        bool callOnce; // This allows things to be called once in methods like the update() method.
        bool snapEnabled;
        bool hasJustImported;
        bool drawCollideObjects;
        public static List<string> listboxModelsList = new List<string>();
        public static List<Control> bottomFormButtons = new List<Control>();
        public static List<Control> sideFormButtons = new List<Control>();
        public List<Control> snapControls = new List<Control>();

        //Used to draw collide object bounding boxes
        bool hasSelectedPoint;
        bool hasCreatedBase;
        bool collideBoxCreation;
        Vector3 boxStartPosition;
        BoundingBox boundingBox;
        public static List<BoundingBox> collideCallbackList = new List<BoundingBox>();

        //Used to indicate wheather to draw a plain or not
        bool drawPlane;
        bool planeHasSelectedPoint;
        bool planeCreation;
        Vector3 planeBoxStartPosition;
        BoundingBox planeBoundingBox;

        // Used for the creation of planes
        
        DeferredNormalShader defShader;
        DeferredMaterial defMaterial;

        public static string saveName = "Untitled";
        public static bool hasSavedSession;
        public static bool changesHaveBeenMade;

        public Editor()
        {
            InitializeComponent();
            
            bottomFormButtons.Add(TranslationButton);
            bottomFormButtons.Add(RotationButton);
            bottomFormButtons.Add(ScaleButton);
            bottomFormButtons.Add(UniformScaleButton);
            //bottomFormButtons.Add(SnapSpacingButton);

            sideFormButtons.Add(PointLightButton);
            sideFormButtons.Add(SpotLightButton);
            sideFormButtons.Add(EntityCollideCallbackButton);
            sideFormButtons.Add(CreatePlaneButton);
            sideFormButtons.Add(SoundButton);

            XnaDisplayPanel.Location = new System.Drawing.Point(5, menuStrip1.Height + 5);
            XnaDisplayPanel.Size = new System.Drawing.Size(this.Width - PropertiesPanel.Width - 10, this.Height + bottomFormButtons[0].Height + 10);
            //ListBoxPanel.Location = new System.Drawing.Point(this.Width, menuStrip1.Height - 5);
            //ImportedModelsListBox.Location = new System.Drawing.Point(Width + 82, menuStrip1.Height + 5); // Width of the point object is abritrary, just trying to make it farily equal to the other die of the form
            MainTabControl.Location = new System.Drawing.Point(Width + 82, menuStrip1.Height + 5);
            //TabPage listBoxPage = new TabPage("PageTest");
            //MainTabControl.TabPages.Add(listBoxPage);
            for(int i = 0; i < sideFormButtons.Count; i++)
            {
                System.Drawing.Point position;
                position = new System.Drawing.Point(MainTabControl.Location.X - sideFormButtons[i].Width - 10, menuStrip1.Height + (i * 50) + 5);
                //int yPos = (i * 5) + 10;
                sideFormButtons[i].Location = position;
            }

            PropertiesPanel.Location = new System.Drawing.Point(sideFormButtons[0].Location.X - PropertiesPanel.Width - 10, menuStrip1.Height + 5);

            for (int i = 0; i < bottomFormButtons.Count; i++)
            {
                System.Drawing.Point position;
                position = new System.Drawing.Point(0 + (i * 50) + 5, XnaDisplayPanel.Height + bottomFormButtons[i].Height + 5);
                bottomFormButtons[i].Location = position;
            }
            // Spaces the snap spacing button properly
            SnapSpacingButton.Location = new System.Drawing.Point(bottomFormButtons[3].Location.X + SnapSpacingButton.Width * 2, bottomFormButtons[3].Location.Y);

            GridSpacingLabel.Location = new System.Drawing.Point(SnapSpacingButton.Location.X + SnapSpacingButton.Width + 5, SnapSpacingButton.Location.Y);
            TranslationSnapTextBox.Location = new System.Drawing.Point(GridSpacingLabel.Location.X + GridSpacingLabel.Width + 5, GridSpacingLabel.Location.Y);
            
            ScaleLabel.Location = new System.Drawing.Point(GridSpacingLabel.Location.X + (GridSpacingLabel.Width - ScaleLabel.Width), SnapSpacingButton.Location.Y + SnapSpacingButton.Height - ScaleLabel.Height);
            ScaleSnapTextBox.Location = new System.Drawing.Point(ScaleLabel.Location.X + ScaleLabel.Width + 5, ScaleLabel.Location.Y);

            RotationLabel.Location = new System.Drawing.Point(TranslationSnapTextBox.Location.X + RotationLabel.Width + 5, TranslationSnapTextBox.Location.Y);
            RotationSnapTextBox.Location = new System.Drawing.Point(RotationLabel.Location.X + RotationLabel.Width + 5, RotationLabel.Location.Y);

            ResetSnapButton.Location = new System.Drawing.Point(RotationLabel.Location.X, RotationLabel.Location.Y + ResetSnapButton.Height + 5);

            DefaultLightButton.Location = new System.Drawing.Point(RotationSnapTextBox.Location.X + RotationSnapTextBox.Width + 55, SnapSpacingButton.Location.Y);
            
            // Initializes plane components
            defShader = new DeferredNormalShader();
            defMaterial = new DeferredMaterial(defShader);     

            try
            {
                listboxModelsList = EngineStart.ImportedModelsList.ModelList;
            }
            catch (NullReferenceException nullRefException)
            {
                EngineStart.desc.Logger.Log("Exception: " + nullRefException.ToString() + "No imported model list found \n in: " + EngineStart.engine.Content.RootDirectory + "bin/",
                    PloobsEngine.Engine.Logger.LogLevel.RecoverableError);
            }
            hasJustImported = true; // this is used to avoid raising the 'Selected Index Changed' event (ln: 335)   
            ImportedModelsListBox.DataSource = listboxModelsList;
            //ImportedModelsListBox.DataSource = listboxTest;
            //formButtons.Add(ResetSnapButtonControl); // The position of this control must be monitored seperately      
        }

        public List<string> ListBoxDataSource
        {
            get { return (List<string>)ImportedModelsListBox.DataSource; }
        }

        public Control XnaMainDisplayPanel
        {
            get { return XnaDisplayPanel; }
        }

        /*public Control PropertiesPanelHolder
        {
            get { return FarLeftPanel; }
        }*/

        /*public Control ListBoxHolderPanel
        {
            get { return ListBoxPanel; }
        }*/

        public Control PropertiesPanel
        {
            get { return propertyGrid1; }
        }

        public Control TranslationButton
        {
            get {return TranslationGizmoButton; }
        }

        public Control RotationButton
        {
            get { return RotationGizmoButton; }
        }

        public Control ScaleButton
        {
            get { return ScaleGizmoButton; }
        }

        public Control UniformScaleButton
        {
            get { return UniformScaleGizmoButton; }
        }

        public Control ResetSnapButtonControl
        {
            get { return ResetSnapButton; }
        }

        public Control MainMenuStrip
        {
            get { return menuStrip1; }
        }

        public Control ListBox
        {
            get { return ImportedModelsListBox; }
        }

        public Control DiffuseTextureTextbox
        {
            get { return DiffuseTextureTextBox; }
        }

        public object SelectedPropertyObject
        {
            get { return propertyGrid1.SelectedObject; }

            set { propertyGrid1.SelectedObject = value; }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TGEditor Created by: Marcus Coley\n © 2012");
        }

        private void modelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool hasSelectedFile = true;
            List<string> contentList = new List<string>();
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();
            try
            {
                string filepathTest = System.IO.Path.GetDirectoryName(openFile.FileName);
            }
            catch (ArgumentException argsException)
            {
                hasSelectedFile = false;
                EngineStart.desc.Logger.Log("Exception: " + argsException.ToString(), PloobsEngine.Engine.Logger.LogLevel.Info); // TODO: Re-write all log entries using this format
                EngineStart.desc.Logger.Log("   No file was selected" + LogLevel.Info.ToString(), LogLevel.Info);                // Use this as an example for the rest
            }
            if (hasSelectedFile)
            {
                string searchDirectory = System.IO.Path.GetDirectoryName(openFile.FileName);
                contentList = SearchForTextures(searchDirectory);
                string fileName = System.IO.Path.GetFileName(openFile.FileName); // File nae with extensions included
                string filePath = System.IO.Path.GetDirectoryName(openFile.FileName);
                string modelFile = Path.Combine(filePath, fileName);
                contentList.Add(modelFile);

                XNBCreator xnbCreator = new XNBCreator(contentList);

                fileName = fileName.Remove(fileName.Length - 4);
                IModelo model;
                model = new SimpleModel(EngineStart.mainScreen.GraphicFactory, "Models/" + fileName); // Dont actually have to display normal map for the sake of performance
                //model.Name = "Models/" + fileName;

                SceneEntity modelEntity = new SceneEntity(model, "Models/" + fileName, EngineStart.mainScreen, false);
                Engine.Entities.Add(modelEntity);
                listboxModelsList.Add(fileName);
                hasJustImported = true;
                ImportedModelsListBox.DataSource = null;
                hasJustImported = true;
                ImportedModelsListBox.DataSource = listboxModelsList;
            }   
        }

        List<string> SearchForTextures(string directory)
        {
            string[] fileTextures = Directory.GetFiles(directory);
            List<string> returnListOfFiles = new List<string>();

            for (int i = 0; i < fileTextures.Length; i++)
            {
                if (fileTextures[i].ElementAt<char>(fileTextures[i].Length - 6) == '_' && fileTextures[i].ElementAt<char>(fileTextures[i].Length - 5) == 'D')
                    returnListOfFiles.Add(fileTextures[i]);
                else if(fileTextures[i].ElementAt<char>(fileTextures[i].Length - 6) == '_' && fileTextures[i].ElementAt<char>(fileTextures[i].Length - 5) == 'N')
                    returnListOfFiles.Add(fileTextures[i]);
            }

            return returnListOfFiles;
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void boundingBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            boundingBoxToolStripMenuItem.Checked = !boundingBoxToolStripMenuItem.Checked;
            SceneEntity.drawBox = boundingBoxToolStripMenuItem.Checked;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EngineStart.engine.Exit();
        }

        private void TGToolsPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {
      
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TranslationGizmoButton_Click(object sender, EventArgs e)
        {
            TranslationGizmoButton.FlatStyle = FlatStyle.Standard;
            DeferredScreen.gizmoComponent.ActiveMode = GizmoMode.Translate;
        }

        private void RotationGizmoButton_Click(object sender, EventArgs e)
        {
            RotationGizmoButton.FlatStyle = FlatStyle.Standard;
            DeferredScreen.gizmoComponent.ActiveMode = GizmoMode.Rotate;
        }

        private void ScaleGizmoButton_Click(object sender, EventArgs e)
        {
            ScaleGizmoButton.FlatStyle = FlatStyle.Standard;
            DeferredScreen.gizmoComponent.ActiveMode = GizmoMode.NonUniformScale;
        }

        private void UniformScaleButton_Click(object sender, EventArgs e)
        {
            UniformScaleGizmoButton.FlatStyle = FlatStyle.Standard;
            DeferredScreen.gizmoComponent.ActiveMode = GizmoMode.UniformScale;
        }

        private void SnapSpacingButton_Click(object sender, EventArgs e)
        {
            SnapSpacingButton.FlatStyle = FlatStyle.Standard;
            snapEnabled = !snapEnabled;

            if (snapEnabled)
            {   
                GizmoComponent.TranslationSnapValue = 0.01f;
                GizmoComponent.RotationSnapValue = 0.01f;
                GizmoComponent.ScaleSnapValue = 0.01f;
            }
            else
            {
                float translationSnapValue = float.Parse(TranslationSnapTextBox.Text);
                float rotationSnapValue = float.Parse(RotationSnapTextBox.Text);
                float scaleSnapeValue = float.Parse(ScaleSnapTextBox.Text);
               
                GizmoComponent.TranslationSnapValue = translationSnapValue;
                GizmoComponent.RotationSnapValue = rotationSnapValue;
                GizmoComponent.ScaleSnapValue = scaleSnapeValue;
            }
        }

        private void PointLightButton_Click(object sender, EventArgs e)
        {
            PointLightButton.FlatStyle = FlatStyle.Standard;
            PointLightPE pointlight = new PointLightPE(Vector3.Zero, Microsoft.Xna.Framework.Color.Wheat, 100f, 5f);
            SceneEntity lightEntity = new SceneEntity(pointlight, EngineStart.mainScreen);
            pointlight.CastShadown = true;
            Engine.Entities.Add(lightEntity);
            changesHaveBeenMade = true;
        }
      
        private void SpotLightButton_Click(object sender, EventArgs e)
        {
            SpotLightButton.FlatStyle = FlatStyle.Standard;
            SpotLightPE spotlight = new SpotLightPE(new Vector3(5, 50, 5), Vector3.Down, 100, 600, Microsoft.Xna.Framework.Color.Red, (float)Math.Cos(Math.PI / 7), 2.5f);
            SceneEntity spotlightEntity = new SceneEntity(spotlight, EngineStart.mainScreen);
            spotlight.CastShadown = true;
            Engine.Entities.Add(spotlightEntity);

            /*
            Texture2D goboTexture = EngineStart.engine.Content.Load<Texture2D>("Textures/gobo_castlebase_window_large");
            Matrix view;
            Matrix projection;
            Decal shit = new Decal(goboTexture, view = Matrix.CreateLookAt(spotlight.Position, spotlight.Target, Vector3.Up), projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4 / 5, 
                                                                                                                                                            EngineStart.engine.GraphicsDevice.Viewport.AspectRatio,
                                                                                                                                                 1, 750));
            BoundingFrustum bf = new BoundingFrustum(view * projection);
            DebugShapeRenderer.AddBoundingFrustum(bf, Microsoft.Xna.Framework.Color.Red, 10);
            DeferredScreen.decalComponent.Decals.Add(shit);*/
            changesHaveBeenMade = true;
        }

        private void EntityCollideCallbackButton_Click(object sender, EventArgs e)
        {
            EntityCollideCallbackButton.FlatStyle = FlatStyle.Standard;

            drawCollideObjects = !drawCollideObjects;
            drawPlane = false;

            if (drawPlane)
                CreatePlaneButton.BackColor = SystemColors.ControlDark;
            else
                CreatePlaneButton.BackColor = SystemColors.ControlLight;

            if (drawCollideObjects)
                EntityCollideCallbackButton.BackColor = SystemColors.ControlDark;
            else
                EntityCollideCallbackButton.BackColor = SystemColors.ControlLight;
        }

        private void CreatePlaneButton_Click(object sender, EventArgs e)
        {
            CreatePlaneButton.FlatStyle = FlatStyle.Standard;

            drawPlane = !drawPlane;
            drawCollideObjects = false;

            if (drawPlane)
                CreatePlaneButton.BackColor = SystemColors.ControlDark;
            else
                CreatePlaneButton.BackColor = SystemColors.ControlLight;

            if (drawCollideObjects)
                EntityCollideCallbackButton.BackColor = SystemColors.ControlDark;
            else
                EntityCollideCallbackButton.BackColor = SystemColors.ControlLight;
        }

        public void UpdateForm(GameTime gameTime, RenderHelper renderHelper) // *** UPDATE METHOD *** //
        {
            if (callOnce == false)
            {
                this.TopMost = false;

                // Makes sure this if statement isnt called again hence 'callOnce'
                callOnce = true;
            }

            // Change the tab header text depending on kind of item is currently selected
            if (GizmoComponent.Selection.Count > 0 && GizmoComponent.Selection.Count < 2)
            {
                if (!GizmoComponent.Selection[0].IsCollideObject && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity)
                    MainTabControl.TabPages[1].Text = "Model";
                else if (GizmoComponent.Selection[0].IsPointlightEntity)
                    MainTabControl.TabPages[1].Text = "PointLight";
                else if (GizmoComponent.Selection[0].IsSpotlightEntity)
                    MainTabControl.TabPages[1].Text = "SpotLight";
                else if (GizmoComponent.Selection[0].IsPlaneObject)
                    MainTabControl.TabPages[1].Text = "Plane";
                else if (GizmoComponent.Selection[0].IsCollideObject)
                    MainTabControl.TabPages[1].Text = "CollideObject";
            }
            else
                MainTabControl.TabPages[1].Text = null;

            // The following code allows the user to draw (Click and drag) collideCallBack objects - used as triggers
            if (drawCollideObjects)
            {
                if (DeferredScreen.gizmoInput.Mouse.IsButtonDown(TGEditor.MouseButtons.Left) && collideBoxCreation != true)
                {
                    Plane gridPlane = new Plane(new Vector4(0, 1, 0, 0));
                    if (DeferredScreen.mouseRay.Intersects(gridPlane).HasValue)
                    {
                        Vector3 intersectionPoint = DeferredScreen.mouseRay.Position + DeferredScreen.mouseRay.Direction * DeferredScreen.mouseRay.Intersects(gridPlane).Value;
                        BoundingSphere sphere = new BoundingSphere(intersectionPoint, 10f);
                        DebugShapeRenderer.AddBoundingSphere(sphere, Microsoft.Xna.Framework.Color.Red);

                        if (hasSelectedPoint != true)
                        {
                            boxStartPosition = intersectionPoint;
                            hasSelectedPoint = true;
                        }

                        boundingBox = new BoundingBox(boxStartPosition, intersectionPoint);
                        DebugShapeRenderer.AddBoundingBox(boundingBox, Microsoft.Xna.Framework.Color.Red);
                    }
                    hasCreatedBase = true;
                }
                else if (DeferredScreen.gizmoInput.Mouse.IsButtonUp(TGEditor.MouseButtons.Left) && hasCreatedBase)
                {
                    collideBoxCreation = true;
                    Plane cameraDirectionInvert = new Plane(Vector3.Negate(DeferredScreen.camera.GetCameraDirection), 0);

                    Vector3 intersectionPoint = Vector3.Zero;

                    if (DeferredScreen.mouseRay.Intersects(cameraDirectionInvert).HasValue)
                        intersectionPoint = DeferredScreen.mouseRay.Position + DeferredScreen.mouseRay.Direction * DeferredScreen.mouseRay.Intersects(cameraDirectionInvert).Value;

                    BoundingSphere boundingSphere = new BoundingSphere(intersectionPoint, 10f);
                    DebugShapeRenderer.AddBoundingSphere(boundingSphere, Microsoft.Xna.Framework.Color.Red);

                    if (intersectionPoint.Y <= 0)
                        intersectionPoint.Y = 0;

                    DebugShapeRenderer.AddBoundingBox(boundingBox = new BoundingBox(boundingBox.Min, new Vector3(boundingBox.Max.X, intersectionPoint.Y, boundingBox.Max.Z)), Microsoft.Xna.Framework.Color.Red);
                }
                else if (DeferredScreen.gizmoInput.Mouse.IsButtonDown(TGEditor.MouseButtons.Left) && hasCreatedBase)
                {
                    collideCallbackList.Add(boundingBox);

                    //SimpleModel dummyModel = new SimpleModel(EngineStart.mainScreen.GraphicFactory, "3DGizmoContent/box");
                    //dummyModel.SetTexture(EngineStart.mainScreen.GraphicFactory.CreateTexture2DColor(1, 1, Microsoft.Xna.Framework.Color.Red), TextureType.DIFFUSE);

                    Vector3 boxDimensions = boundingBox.Max - boundingBox.Min;
                    Vector3 origBoundingBoxCenter = boundingBox.Max - (boundingBox.Max - boundingBox.Min) / 2;
                    BoxObject callBackBox = new BoxObject(origBoundingBoxCenter, boxDimensions.X, boxDimensions.Y, boxDimensions.Z, 10f, Vector3.One, Matrix.Identity, MaterialDescription.DefaultBepuMaterial());
                    callBackBox.isMotionLess = true;

                    //ForwardTransparenteShader trannyShader = new ForwardTransparenteShader(100); // TODO: may have to figure another way of making the cube transparent
                    //DeferredNormalShader modelShader = new DeferredNormalShader();
                    //DeferredMaterial modelMaterial = new DeferredMaterial(modelShader);
                    //ForwardMaterial forMat = new ForwardMaterial(trannyShader);
                    //IObject modelObj = new IObject(forMat, dummyModel, callBackBox); 
                    //EngineStart.mainScreen.World.AddObject(modelObj);
                    //collideCallbackList.Add((BoundingBox)callBackBox.BoundingBox);

                    Engine.Entities.Add(new SceneEntity(callBackBox, boundingBox, boxDimensions, EngineStart.mainScreen));

                    hasCreatedBase = false;
                    collideBoxCreation = false;
                    hasSelectedPoint = false;

                    Thread.Sleep(100);
                    changesHaveBeenMade = true;
                    collideCallbackList.RemoveRange(0, collideCallbackList.Count);
                }
            }
            else
                return;
            foreach (BoundingBox boundingBox in collideCallbackList)
            {
                DebugShapeRenderer.AddBoundingBox(new BoundingBox(boundingBox.Min, boundingBox.Max), Microsoft.Xna.Framework.Color.Red);
            }

            // The following code allows the user to draw planes
            if (drawPlane)
            {
                if (DeferredScreen.gizmoInput.Mouse.IsButtonDown(TGEditor.MouseButtons.Left))
                {
                    Plane gridPlane = new Plane(new Vector4(0, 1, 0, 0));
                    if (DeferredScreen.mouseRay.Intersects(gridPlane).HasValue)
                    {
                        Vector3 intersectionPoint = DeferredScreen.mouseRay.Position + DeferredScreen.mouseRay.Direction * DeferredScreen.mouseRay.Intersects(gridPlane).Value;
                        BoundingSphere sphere = new BoundingSphere(intersectionPoint, 10f);
                        DebugShapeRenderer.AddBoundingSphere(sphere, Microsoft.Xna.Framework.Color.Red);

                        if (planeHasSelectedPoint != true)
                        {
                            planeBoxStartPosition = intersectionPoint;
                            planeHasSelectedPoint = true;
                        }

                        planeBoundingBox = new BoundingBox(planeBoxStartPosition, intersectionPoint);
                        DebugShapeRenderer.AddBoundingBox(planeBoundingBox, Microsoft.Xna.Framework.Color.Red);
                    }
                }
                else if (DeferredScreen.gizmoInput.Mouse.IsButtonUp(TGEditor.MouseButtons.Left) && planeHasSelectedPoint)
                {
                    planeHasSelectedPoint = false;
                    if (planeBoundingBox.Max != null && planeBoundingBox.Min != null)
                    {
                        Vector3 planeSize = planeBoundingBox.Max - planeBoundingBox.Min;
                        Vector3 planeCenter = planeBoundingBox.Max - (planeBoundingBox.Max - planeBoundingBox.Min) / 2;
                        //System.Diagnostics.Debug.WriteLine(planeSize.ToString());

                        BoxObject planeBoxObj;
                        planeBoxObj = new BoxObject(planeCenter, planeSize.X, 0.25f, planeSize.Y, 10f, new Vector3(planeSize.X / 25, 0.01f, planeSize.Z / 25), Matrix.Identity, MaterialDescription.DefaultBepuMaterial());
                        planeBoxObj.isMotionLess = true;

                        SimpleModel planeModel = new SimpleModel(EngineStart.mainScreen.GraphicFactory, "3DGizmoContent/box");
                        IObject planeObject;

                        //RasterizerState rs = new RasterizerState();
                        //rs.CullMode = CullMode.None;
                        //EngineStart.engine.GraphicsDevice.RasterizerState = rs;

                        //planeObject = new IObject(defMaterial, planeModel = new SimpleModel(EngineStart.mainScreen.GraphicFactory, "3DGizmoContent/box"), planeBoxObj);
                        SceneEntity planeEntity = new SceneEntity(planeBoxObj, planeModel, planeSize, new Vector3(planeSize.X / 25, 0.01f, planeSize.Z / 25), EngineStart.mainScreen);
                        Engine.Entities.Add(planeEntity);
                        changesHaveBeenMade = true;
                        //planeEntity.scale = new Vector3(planeSize.X / 25, 0.1f, planeSize.Z / 25);
                        //Engine.Entities.Add(planeEntity);
                        //EngineStart.mainScreen.World.AddObject(planeObject);
                        //System.Diagnostics.Debug.WriteLine(planeSize.X / 25 + " - " + planeSize.Z / 25);
                    }
                }
            }
            else
                return;

            // Unselects all tools
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                drawPlane = false;
                drawCollideObjects = false;
            }

        }

        private void ResetSnapButton_Click(object sender, EventArgs e)
        {
            ResetSnapButton.FlatStyle = FlatStyle.Standard;
        }

        private void TranslationSnapTextBox_TextChanged(object sender, EventArgs e)
        {
            if (snapEnabled == false)
            {
                try
                {
                    float translationSnapValue = float.Parse(TranslationSnapTextBox.Text);
                    GizmoComponent.TranslationSnapValue = translationSnapValue;
                }
                catch (System.FormatException changeTranslationSnapValueException)
                {
                    EngineStart.desc.Logger.Log("Exception: " + changeTranslationSnapValueException.ToString() + "Could not parse snap value in textbox",
                        PloobsEngine.Engine.Logger.LogLevel.RecoverableError);
                }
            }
        }

        private void ScaleSnapTextBox_TextChanged(object sender, EventArgs e)
        {
            if (snapEnabled == false)
            {
                try
                {
                    float scaleSnapValue = float.Parse(ScaleSnapTextBox.Text);
                    GizmoComponent.ScaleSnapValue = scaleSnapValue;
                }
                catch (System.FormatException changeScaleSnapValueException)
                {
                    EngineStart.desc.Logger.Log("Exception: " + changeScaleSnapValueException.ToString() + "Could not parse snap value in textbox",
                        PloobsEngine.Engine.Logger.LogLevel.RecoverableError);
                }
            }
        }

        private void RotationSnapTextBox_TextChanged(object sender, EventArgs e)
        {
            if (snapEnabled == false)
            {
                try
                {
                    float rotationSnapValue = float.Parse(RotationSnapTextBox.Text);
                    GizmoComponent.RotationSnapValue = rotationSnapValue;
                }
                catch (System.FormatException changeRotationSnapException)
                {
                    EngineStart.desc.Logger.Log("Exception: " + changeRotationSnapException.ToString() + "Could not parse snap value in textbox",
                        PloobsEngine.Engine.Logger.LogLevel.RecoverableError);
                }
            }
        }

        private void ResetSnapButton_Click_1(object sender, EventArgs e)
        {
            TranslationSnapTextBox.Text = "10";
            RotationSnapTextBox.Text = "30";
            ScaleSnapTextBox.Text = "0.5";
        }

        private void splitContainer1_Panel2_Paint_1(object sender, PaintEventArgs e)
        {
         
        }

        private void ImportedModelsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hasJustImported == false)
            {     
          
                IModelo selectedModel = new SimpleModel(EngineStart.mainScreen.GraphicFactory, "Models/" + ImportedModelsListBox.SelectedItem.ToString());
                SceneEntity selectedModelEntity = new SceneEntity(selectedModel, "Models/" + ImportedModelsListBox.SelectedItem.ToString(), EngineStart.mainScreen, false);

                Engine.Entities.Add(selectedModelEntity);
                changesHaveBeenMade = true;
            }
            else
            {
                ImportedModelsListBox.ClearSelected();
                hasJustImported = false;
            }
        }

        private void FarLeftPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!hasSavedSession)
            {
                SaveFileDialog saveDialogue = new SaveFileDialog();
                saveDialogue.ShowDialog();

                saveName = saveDialogue.FileName;
                hasSavedSession = true;
                changesHaveBeenMade = false;
            }

            SceneSerializer scene = new SceneSerializer(Engine.Entities);
            Stream saveStream = null;
            IFormatter saveFormatter = new BinaryFormatter();
            try
            {
                saveFormatter.Serialize(saveStream = new FileStream(saveName, FileMode.Create, FileAccess.Write, FileShare.None), scene);
                saveStream.Close();
            }
            catch (ArgumentException argsException)
            {
                EngineStart.desc.Logger.Log(argsException.ToString() + " - " + " Save file dialogue was closed with savinga file ", LogLevel.Info);
            }
            hasSavedSession = true;
            changesHaveBeenMade = false;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog saveDialogue = new SaveFileDialog();
            saveDialogue.ShowDialog();

            saveName = saveDialogue.FileName;
          
            SceneSerializer scene = new SceneSerializer(Engine.Entities);
            Stream saveStream = null;
            IFormatter saveFormatter = new BinaryFormatter();
            try
            {
                saveFormatter.Serialize(saveStream = new FileStream(saveName, FileMode.Create, FileAccess.Write, FileShare.None), scene);
                saveStream.Close();
            }
            catch (ArgumentException argsException)
            {
                EngineStart.desc.Logger.Log(argsException.ToString() + " - " + " Save file dialogue was closed with savinga file ", LogLevel.Info);
            }
            hasSavedSession = true;
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Remove all existing objects in the scene
            Engine.Entities.RemoveRange(0, Engine.Entities.Count);
            
            for (int i = 0; i < EngineStart.mainScreen.World.Objects.Count; i++)
            {
                EngineStart.mainScreen.World.RemoveObject(EngineStart.mainScreen.World.Objects[i]);
                i--;
            }
            for(int i = 0; i < EngineStart.mainScreen.World.Lights.Count; i++)
            {
                EngineStart.mainScreen.World.RemoveLight(EngineStart.mainScreen.World.Lights[i]);
                i--;
            }

            SceneSerializer serializedScene = new SceneSerializer();

            // Open the dialogue
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();

            Stream stream = null;
            IFormatter formatter = new BinaryFormatter();

            try
            {
                stream = new FileStream(openFile.FileName, FileMode.Open, FileAccess.Read, FileShare.None);

                // Deserialize and load the actual scene
                serializedScene = (SceneSerializer)formatter.Deserialize(stream);

                TGSceneLoader sceneLoader = new TGSceneLoader(serializedScene.listOfObjects);
                saveName = openFile.FileName;
                hasSavedSession = true;
            }
            catch (ArgumentException argsException)
            {
                EngineStart.desc.Logger.Log(argsException.ToString() + "OpenFileDialogue exited withought selection", LogLevel.Info);
            }

            // Create and add defualt lighting
            DirectionalLightPE[] defaultLight = new DirectionalLightPE[6];

            DirectionalLightPE upDir = new DirectionalLightPE(Vector3.Up, Microsoft.Xna.Framework.Color.White);
            DirectionalLightPE downDir = new DirectionalLightPE(Vector3.Down, Microsoft.Xna.Framework.Color.White);
            DirectionalLightPE rightDir = new DirectionalLightPE(Vector3.Right, Microsoft.Xna.Framework.Color.White);
            DirectionalLightPE leftDir = new DirectionalLightPE(Vector3.Left, Microsoft.Xna.Framework.Color.White);
            DirectionalLightPE backDir = new DirectionalLightPE(Vector3.Backward, Microsoft.Xna.Framework.Color.White);
            DirectionalLightPE forDir = new DirectionalLightPE(Vector3.Forward, Microsoft.Xna.Framework.Color.White);
            defaultLight[0] = upDir;
            defaultLight[1] = downDir;
            defaultLight[2] = rightDir;
            defaultLight[3] = leftDir;
            defaultLight[4] = backDir;
            defaultLight[5] = forDir;

            foreach (DirectionalLightPE light in defaultLight)
            {
                light.LightIntensity = 0.4f;
                EngineStart.mainScreen.World.AddLight(light);
            }

        }

        private void importSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SceneSerializer serializedScene = new SceneSerializer();

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();

            Stream stream = null;
            IFormatter formatter = new BinaryFormatter();

            stream = new FileStream(openFile.FileName, FileMode.Open, FileAccess.Read, FileShare.None);

            serializedScene = (SceneSerializer)formatter.Deserialize(stream);

            TGSceneLoader sceneLoader = new TGSceneLoader(serializedScene.listOfObjects, true);

            hasSavedSession = false;
            
        }

        private void DefaultLightButton_Click(object sender, EventArgs e)
        {
            foreach (ILight light in EngineStart.mainScreen.World.Lights)
            {
                if (light.LightType == LightType.Deferred_Directional)
                {
                    light.Enabled = !light.Enabled;
                }
            }
        }

        private void DiffuesTextureButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();

            string fileName = Path.GetFileNameWithoutExtension(openFile.FileName);

            if (openFile.FileName != null)
            {
                bool skip = false;
                string[] searchFiles = Directory.GetFiles(EngineStart.engine.Content.RootDirectory + "/Textures", "*.xnb");

                if (searchFiles.Contains(Path.Combine(EngineStart.engine.Content.RootDirectory + "/Textures", fileName + ".xnb")))
                {
                    if (!GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsCollideObject)
                    {
                        GizmoComponent.Selection[0].DiffuseTexture = "Textures/" + fileName;
                        DiffuseTextureTextBox.Text = fileName;
                    }
                    else if (GizmoComponent.Selection[0].IsPlaneObject)
                    {
                        GizmoComponent.Selection[0].DiffuseTexture = "Textures/" + fileName;
                        DiffuseTextureTextBox.Text = fileName;
                    }
                    skip = true;
                }

                if (!skip)
                {
                    List<string> textureList = new List<string>();
                    textureList.Add(openFile.FileName);

                    XNBCreator xnbCreator = new XNBCreator(textureList, true);

                    if (!GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsCollideObject)
                    {
                        GizmoComponent.Selection[0].DiffuseTexture = "Textures/" + fileName;
                        DiffuesTextureButton.Text = fileName;
                    }
                    else if (GizmoComponent.Selection[0].IsPlaneObject)
                    {
                        GizmoComponent.Selection[0].DiffuseTexture = "Textures/" + fileName;
                        DiffuesTextureButton.Text = fileName;
                    }
                }
            }
        }

        private void SpecularTextureButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();

            string fileName = Path.GetFileNameWithoutExtension(openFile.FileName);

            if (openFile.FileName != null)
            {
                bool skip = false;
                string[] searchFiles = Directory.GetFiles(EngineStart.engine.Content.RootDirectory + "/Textures", "*.xnb");

                if (searchFiles.Contains(Path.Combine(EngineStart.engine.Content.RootDirectory + "/Textures", fileName + ".xnb")))
                {
                    if (!GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsCollideObject)
                    {
                        GizmoComponent.Selection[0].SpecularTexture = "Textures/" + fileName;
                        SpecularTextureTextBox.Text = fileName;
                    }
                    else if (GizmoComponent.Selection[0].IsPlaneObject)
                    {
                        GizmoComponent.Selection[0].SpecularTexture = "Textures/" + fileName;
                        SpecularTextureTextBox.Text = fileName;
                    }
                    skip = true;
                }

                if (!skip)
                {
                    List<string> textureList = new List<string>();
                    textureList.Add(openFile.FileName);

                    XNBCreator xnbCreator = new XNBCreator(textureList, true);

                    if (!GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsCollideObject)
                    {
                        GizmoComponent.Selection[0].SpecularTexture = "Textures/" + fileName;
                        SpecularTextureTextBox.Text = fileName;
                    }
                    else if (GizmoComponent.Selection[0].IsPlaneObject)
                    {
                        GizmoComponent.Selection[0].SpecularTexture = "Textures/" + fileName;
                        SpecularTextureTextBox.Text = fileName;
                    }
                }
            }
        }

        private void NormalTextureButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();

            string fileName = Path.GetFileNameWithoutExtension(openFile.FileName);

            if (openFile.FileName != null)
            {
                bool skip = false;
                string[] searchFiles = Directory.GetFiles(EngineStart.engine.Content.RootDirectory + "/Textures", "*.xnb");

                if (searchFiles.Contains(Path.Combine(EngineStart.engine.Content.RootDirectory + "/Textures", fileName + ".xnb")))
                {
                    if (!GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsCollideObject)
                    {
                        GizmoComponent.Selection[0].NormalTexture = "Textures/" + fileName;
                        NormalTextureTextBox.Text = fileName;
                    }
                    else if (GizmoComponent.Selection[0].IsPlaneObject)
                    {
                        GizmoComponent.Selection[0].NormalTexture = "Textures/" + fileName;
                        NormalTextureTextBox.Text = fileName;
                    }
                    skip = true;
                }

                if (!skip)
                {
                    List<string> textureList = new List<string>();
                    textureList.Add(openFile.FileName);

                    XNBCreator xnbCreator = new XNBCreator(textureList, true);

                    if (!GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsCollideObject)
                    {
                        GizmoComponent.Selection[0].NormalTexture = "Textures/" + fileName;
                        NormalTextureTextBox.Text = fileName;
                    }
                    else if (GizmoComponent.Selection[0].IsPlaneObject)
                    {
                        GizmoComponent.Selection[0].NormalTexture = "Textures/" + fileName;
                        NormalTextureTextBox.Text = fileName;
                    }
                }
            }
        }

        private void GlowTextureButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();

            string fileName = Path.GetFileNameWithoutExtension(openFile.FileName);
      
            if (openFile.FileName != null)
            {
                bool skip = false;
                string[] searchFiles = Directory.GetFiles(EngineStart.engine.Content.RootDirectory + "/Textures", "*.xnb");

                if (searchFiles.Contains(Path.Combine(EngineStart.engine.Content.RootDirectory + "/Textures", fileName + ".xnb")))
                {
                    if (!GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsCollideObject)
                    {
                        GizmoComponent.Selection[0].GlowTexture = "Textures/" + fileName;
                        GlowTextureTextBox.Text = GizmoComponent.Selection[0].DiffuseTexture;
                    }
                    else if (GizmoComponent.Selection[0].IsPlaneObject)
                    {
                        GizmoComponent.Selection[0].GlowTexture = "Textures/" + fileName;
                        GlowTextureTextBox.Text = fileName;
                    }
                    skip = true;
                }

                if (!skip)
                {
                    List<string> textureList = new List<string>();
                    textureList.Add(openFile.FileName);

                    XNBCreator xnbCreator = new XNBCreator(textureList, true);

                    if (!GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsCollideObject)
                    {
                        GizmoComponent.Selection[0].GlowTexture = "Textures/" + fileName;
                        GlowTextureTextBox.Text = fileName;
                    }
                    else if (GizmoComponent.Selection[0].IsPlaneObject)
                    {
                        GizmoComponent.Selection[0].GlowTexture = "Textures/" + fileName;
                        GlowTextureTextBox.Text = fileName;
                    }
                }
            }
        }

        private void MultiTex1Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();

            string fileName = Path.GetFileNameWithoutExtension(openFile.FileName);

            if (openFile.FileName != null)
            {
                bool skip = false;
                string[] searchFiles = Directory.GetFiles(EngineStart.engine.Content.RootDirectory + "/Textures", "*.xnb");

                if (searchFiles.Contains(Path.Combine(EngineStart.engine.Content.RootDirectory + "/Textures", fileName + ".xnb"))) 
                {
                    if (!GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsCollideObject)
                    {
                        GizmoComponent.Selection[0].MultiTex1 = "Textures/" + fileName;
                        MultiTex1TextBox.Text = fileName;
                    }
                    else if (GizmoComponent.Selection[0].IsPlaneObject)
                    {
                        GizmoComponent.Selection[0].MultiTex1 = "Textures/" + fileName;
                        MultiTex1TextBox.Text = fileName;
                    }
                    skip = true;
                }

                if (!skip)
                {
                    List<string> textureList = new List<string>();
                    textureList.Add(openFile.FileName);

                    XNBCreator xnbCreator = new XNBCreator(textureList, true);

                    if (!GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsCollideObject)
                    {
                        GizmoComponent.Selection[0].MultiTex1 = "Textures/" + fileName;
                        MultiTex1TextBox.Text = fileName;
                    }
                    else if (GizmoComponent.Selection[0].IsPlaneObject)
                    {
                        GizmoComponent.Selection[0].MultiTex1 = "Textures/" + fileName;
                        MultiTex1TextBox.Text = fileName;
                    }
                }
            }
        }

        private void MultiTex2Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();

            string fileName = Path.GetFileNameWithoutExtension(openFile.FileName);

            if (openFile.FileName != null)
            {
                bool skip = false;
                string[] searchFiles = Directory.GetFiles(EngineStart.engine.Content.RootDirectory + "/Textures", "*.xnb");

                if (searchFiles.Contains(Path.Combine(EngineStart.engine.Content.RootDirectory + "/Textures", fileName + ".xnb")))
                {
                    if (!GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsCollideObject)
                    {
                        GizmoComponent.Selection[0].MultiTex2 = "Textures/" + fileName;
                        MultiTex2TextBox.Text = fileName;
                    }
                    else if (GizmoComponent.Selection[0].IsPlaneObject)
                    {
                        GizmoComponent.Selection[0].MultiTex2 = "Textures/" + fileName;
                        MultiTex2TextBox.Text = fileName;
                    }
                    skip = true;
                }

                if (!skip)
                {
                    List<string> textureList = new List<string>();
                    textureList.Add(openFile.FileName);

                    XNBCreator xnbCreator = new XNBCreator(textureList, true);

                    if (!GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsCollideObject)
                    {
                        GizmoComponent.Selection[0].MultiTex2 = "Textures/" + fileName;
                        MultiTex2TextBox.Text = fileName;
                    }
                    else if (GizmoComponent.Selection[0].IsPlaneObject)
                    {
                        GizmoComponent.Selection[0].MultiTex2 = "Textures/" + fileName;
                        MultiTex2TextBox.Text = fileName;
                    }
                }
            }
        }

        private void MultiTex3Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();

            string fileName = Path.GetFileNameWithoutExtension(openFile.FileName);

            if (openFile.FileName != null)
            {
                bool skip = false;
                string[] searchFiles = Directory.GetFiles(EngineStart.engine.Content.RootDirectory + "/Textures", "*.xnb");

                if (searchFiles.Contains(Path.Combine(EngineStart.engine.Content.RootDirectory + "/Textures", fileName + ".xnb")))
                {
                    if (!GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsCollideObject)
                    {
                        GizmoComponent.Selection[0].MultiTex3 = "Textures/" + fileName;
                        MultiTex3TextBox.Text = fileName;
                    }
                    else if (GizmoComponent.Selection[0].IsPlaneObject)
                    {
                        GizmoComponent.Selection[0].MultiTex3 = "Textures/" + fileName;
                        MultiTex3TextBox.Text = fileName;
                    }
                    skip = true;
                }

                if (!skip)
                {
                    List<string> textureList = new List<string>();
                    textureList.Add(openFile.FileName);

                    XNBCreator xnbCreator = new XNBCreator(textureList, true);

                    if (!GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsCollideObject)
                    {
                        GizmoComponent.Selection[0].MultiTex3 = "Textures/" + fileName;
                        MultiTex3TextBox.Text = fileName;
                    }
                    else if (GizmoComponent.Selection[0].IsPlaneObject)
                    {
                        GizmoComponent.Selection[0].MultiTex3 = "Textures/" + fileName;
                        MultiTex3TextBox.Text = fileName;
                    }
                }
            }
        }

        private void MultiTex4Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();

            string fileName = Path.GetFileNameWithoutExtension(openFile.FileName);

            if (openFile.FileName != null)
            {
                bool skip = false;
                string[] searchFiles = Directory.GetFiles(EngineStart.engine.Content.RootDirectory + "/Textures", "*.xnb");

                if (searchFiles.Contains(Path.Combine(EngineStart.engine.Content.RootDirectory + "/Textures", fileName + ".xnb")))
                {
                    if (!GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsCollideObject)
                    {
                        GizmoComponent.Selection[0].MultiTex4 = "Textures/" + fileName;
                        MultiTex4TextBox.Text = fileName;
                    }
                    else if (GizmoComponent.Selection[0].IsPlaneObject)
                    {
                        GizmoComponent.Selection[0].MultiTex4 = "Textures/" + fileName;
                        MultiTex4TextBox.Text = fileName;
                    }
                    skip = true;
                }

                if (!skip)
                {
                    List<string> textureList = new List<string>();
                    textureList.Add(openFile.FileName);

                    XNBCreator xnbCreator = new XNBCreator(textureList, true);

                    if (!GizmoComponent.Selection[0].IsPointlightEntity && !GizmoComponent.Selection[0].IsSpotlightEntity && !GizmoComponent.Selection[0].IsPlaneObject && !GizmoComponent.Selection[0].IsCollideObject)
                    {
                        GizmoComponent.Selection[0].MultiTex4 = "Textures/" + fileName;
                        MultiTex4TextBox.Text = fileName;
                    }
                    else if (GizmoComponent.Selection[0].IsPlaneObject)
                    {
                        GizmoComponent.Selection[0].MultiTex4 = "Textures/" + fileName;
                        MultiTex4TextBox.Text = fileName;
                    }
                }
            }
        }

        private void ambientLightToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AmbientLightOptionsForm lightOptionsForm = new AmbientLightOptionsForm();
            lightOptionsForm.ShowDialog(this);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Remove all existing objects in the scene
            Engine.Entities.RemoveRange(0, Engine.Entities.Count);

            for (int i = 0; i < EngineStart.mainScreen.World.Objects.Count; i++)
            {
                EngineStart.mainScreen.World.RemoveObject(EngineStart.mainScreen.World.Objects[i]);
                i--;
            }
            for (int i = 0; i < EngineStart.mainScreen.World.Lights.Count; i++)
            {
                EngineStart.mainScreen.World.RemoveLight(EngineStart.mainScreen.World.Lights[i]);
                i--;
            }
        }

        private void SoundButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog(this);

            if (Path.GetExtension(openFile.FileName) == ".wav")
            {
                //File.Copy(openFile.FileName, EngineStart.engine.Content.RootDirectory + "/Audio/" + Path.GetFileName(openFile.FileName));
                List<string> audioPath = new List<string>();
                audioPath.Add(openFile.FileName);
                XNBCreator audioXNB = new XNBCreator(audioPath);

                SceneEntity audioEntity = new SceneEntity(Path.GetFileNameWithoutExtension(openFile.FileName));
                Engine.Entities.Add(audioEntity);
            }

        }
    }
}
