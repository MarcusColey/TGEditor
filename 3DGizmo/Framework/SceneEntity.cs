using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PloobsEngine.Modelo;
using PloobsEngine;
using PloobsEngine.Physics.Bepu;
using PloobsEngine.Material;
using PloobsEngine.SceneControl;
using PloobsEngine.Physic;
using PloobsEngine.Physics;
using PloobsEngine.Engine;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;
using PloobsEngine.Light;
using System.IO;

namespace TGEditor
{
    public class SceneEntity : IObject
    {
        #region Fields
        public Vector3 position = Vector3.Zero;     // ***
        public Vector3 scale = Vector3.One;         //
                                                    //
        public Vector3 Forward = Vector3.Forward;   //
        public Vector3 Up = Vector3.Up;             //
                                                    // All used for transformations - Mostly pre existing 3DGizmo fields but some arent
        public IModelo model;                       //
        public string Name;                         //
                                                    //
        private Matrix world;                       //
        public Matrix rotation;                     // ***

        static IModelo lightBoundingBoxModel; // This is static so it deosnt have to be created every time something is imported
        PointLightPE pointLightObject; 
        Vector3 lightBoundsMin, lightBoundsMax; //Min and Max values of the points lights boundinbox
        Vector3 center; // This represents the center of pointlight/spotlight and audio object bounding boxes
        bool isMotionless = true;

        // *All used for creating a ploobs object*

        DeferredNormalShader shader;
        TriangleMeshObject boundingBoxMesh; // Used to display the boundingox of the object
        BoxObject boxObj;
        DeferredMaterial objMaterial;
        IObject modelObj;
        BoundingBox objBoundingBox;

        // *All used for creating a ploobs object*

        //Used for collideCallBack objects
        Vector3 boxDimensions;
        Vector3 graveYard; // Used to position the 'simpleModels'/Entites attached to the collide objects far away so they are no longer visible

        //Used to calculate a plane object dimensions
        Vector3 planeDimensions;
        Vector3 planeScale;

        // Used for to create spotlights
        SpotLightPE spotlightObject;
        bool isSpotlightEntity;
        float spotlightDirection;

        // Used to create audio objects
        static IModelo audioBoundingBoxModel;
        string audioFile;

        // Other fields
        DeferredScreen mainScreen;

        bool hasDrawn = false;
        public static bool drawBox;
        BasicEffect boxEffect;
      
        Editor editorForm = EngineStart.editorForm;

        string diffuseTextureName, specularTextureName, normalTextureName, glowTextureName, MultiTex1Name, MultiTex2Name, MultiTex3Name, MultiTex4Name;

        bool isPlaneObject; // True of the current scene entity instacne is a plane object

        short[] bBoxIndices = {
	    0, 1, 1, 2, 2, 3, 3, 0, // Front edges
	    4, 5, 5, 6, 6, 7, 7, 4, // Back edges
	    0, 4, 1, 5, 2, 6, 3, 7 // Side edges connecting front and back
	    };
        #endregion

        #region Properties

        public bool IsMotionless
        {
            get
            {
                if (!IsPointlightEntity && !IsSpotlightEntity && !IsPlaneObject && !IsCollideObject)
                    return isMotionless;
                else
                    return false;
            }
            set
            {
                if (!IsPointlightEntity && !IsSpotlightEntity && !IsPlaneObject && !IsCollideObject)
                    isMotionless = value;
            }
        }
      
        // True if the instance is a light entity
        [Browsable(false)]
        public bool IsPointlightEntity
        {
            get;
            protected set;
        }

        // True is the current scene entity instance is a spotlight
        [Browsable(false)]
        public bool IsSpotlightEntity
        {
            get { return isSpotlightEntity; }
        }

        // True if the instance is a collide call back object (Used as a trigger)
        [Browsable(false)]
        public bool IsCollideObject
        {
            get;
            protected set;
        }

        // True if the current scene entity instance is a plane object
        [Browsable(false)]
        public bool IsPlaneObject
        {
            get { return isPlaneObject; }
            set { isPlaneObject = value; }
        }

        
        // All used to display in the properties panel of the editor
        // This bounding box is used for picking
        [Browsable(false), Description("BoundingBox of the object"), Category("Properties")]
        public BoundingBox BoundingBox
        {
            get 
            {
                return objBoundingBox;
            }
        }

        // This 'bounding box' is used to position the entity (Mostly)
        [Browsable(false), Description("BoundingBox of the object"), Category("Properties")]
        public BoxObject BoxObject
        {
            get { return boxObj; }
        }

        // Position of the object
        [Description("Position of the object"), Category("Properties")]
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        //Scale of the object
        [Description("Scale of the object"), Category("Properties")]
        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; }
        }
    
        // The IObject of the scene entity
        [Browsable(false)]
        public IObject modelObject
        {
            get { return modelObj; }
        }

        // The current point light instance
        [Browsable(false)]
        public PointLightPE PointLightObject
        {
            get { return pointLightObject; }
        }

        // Radius of the current light entity
        [Description("Radius of the point light"), Category("Properties")]
        public float PointLightRadius
        {
            get {
                    if (IsPointlightEntity)
                        return pointLightObject.LightRadius;
                    else
                        return 0;
                }

            set {
                    if(IsPointlightEntity)
                        pointLightObject.LightRadius = value;
                }
        }

        [Description("Determines wheather the point light is enabled"), Category("Properties")]
        public bool PointLightIsEnabled
        {
            get 
            {
                if (IsPointlightEntity)
                    return pointLightObject.Enabled;
                else
                    return false;

            }
            set
            {
                if (IsPointlightEntity)
                    pointLightObject.Enabled = value;
            }
        }
        [Description("Determines wheather the light casts shadows"), Category("Properties")]
        public bool PointlightCastShadows
        {
            get
            {
                if (IsPointlightEntity)
                    return pointLightObject.CastShadown;
                else
                    return false;
            }
            set
            {
                if (IsPointlightEntity)
                    pointLightObject.CastShadown = value;
            }
        }

        // Color of the current light entity
        [Description("Color of the point light"), Category("Properties")]
        public Color PointLightColor
        {
            get {
                    if (IsPointlightEntity)
                        return pointLightObject.Color;
                    else
                        return Color.White;
                }

            set { 
                    if(IsPointlightEntity)
                        pointLightObject.Color = value; 
                }
        }

        // Intensity of the current point light entitty
        [Description("Intesity of the point light"), Category("Properties")]
        public float PointLightIntesity
        {
            get
            {
                if (IsPointlightEntity)
                    return pointLightObject.LightIntensity;
                else
                    return 0;
            }
            set
            {
                if (IsPointlightEntity)
                    pointLightObject.LightIntensity = value;
            }
        }
     
        [Browsable(false)]
        public SpotLightPE SpotlightObject
        {
            get { return spotlightObject; }
            //set { spotlightDirection = value; }
        }

        [Description("Color of the spot light"), Category("Properties")]
        public Color SpotLightColor
        {
            get 
            {
                if (IsSpotlightEntity)
                    return spotlightObject.Color;
                else
                    return new Color(0, 0, 0, 0);
            }
            set
            {
                if (IsSpotlightEntity)
                    spotlightObject.Color = value;
            }
        }

        [Description("Determines wheather the spotlight is visible"), Category("Properties")]
        public bool SpotLightEnabled
        {
            get 
            {
                if (IsSpotlightEntity)
                    return spotlightObject.Enabled;
                else
                    return false;
            }
            set
            {
                if (IsSpotlightEntity)
                    spotlightObject.Enabled = value;
            }
        }
        [Description("Determines wheather the spotlight casts shadows"), Category("Properties")]
        public bool SpotlightCastShadows
        {
            get
            {
                if (isSpotlightEntity)
                    return spotlightObject.CastShadown;
                else
                    return false;
            }
            set
            {
                if (isSpotlightEntity)
                    spotlightObject.CastShadown = value;
            }
        }

        [Description("The intensity of the spotlight"), Category("Properties")]
        public float SpotLightIntesity
        {
            get 
            {
                if (IsSpotlightEntity)
                    return spotlightObject.LightIntensity;
                else
                    return 0;
            }
            set
            {
                spotlightObject.LightIntensity = value;
            }
        }

        [Description("The radius of the spotlight"), Category("Properties")]
        public float SpotLightRadius
        {
            get 
            {
                if (IsSpotlightEntity)
                    return spotlightObject.LightRadius;
                else
                    return 0;
            }
            set
            {
                if (IsSpotlightEntity)
                    spotlightObject.LightRadius = value;
            }
        }

        [Browsable(false)]
        public Vector3 SpotLightDirection
        {
            get { return SpotlightObject.Direction; }
        }

        [Description("Determines the fall off rate"), CategoryAttribute("Properties")]
        public float SpotLightConeDecay
        {
            get {
                    if (IsSpotlightEntity)
                        return SpotlightObject.ConeDecay;
                    else
                        return 0;
                }
            set
            {
                if (IsSpotlightEntity)
                    SpotlightObject.ConeDecay = value;
            }
        }

        public bool IsModel
        {
            get;
            set;
        }

        [Browsable(false)]
        public Vector3 PlaneScale // The vector used to create a plane with the correct size when loading a saved scene
        {
            get { return planeScale; }
        }

        [Browsable(false)]
        public Vector3 PlaneDimensions // Dont confuse this with plane scale! - This is used ONLY for creating the correct bounding box for the plane
        {
            get { return planeDimensions; }
        }

        public string DiffuseTexture
        {
            get
            {
                if (!IsPointlightEntity && !IsSpotlightEntity && !IsPlaneObject && !IsCollideObject)
                    return diffuseTextureName;
                else if (isPlaneObject)
                    return diffuseTextureName;
                else
                    return null; 
            }

            set
            {
                Texture2D texture = EngineStart.engine.Content.Load<Texture2D>(value);
                model.SetTexture(texture, TextureType.DIFFUSE);
                diffuseTextureName = Path.GetFileName(value);
            }
        }

        public string NormalTexture
        {
            get
            {
                if (!IsPointlightEntity && !IsSpotlightEntity && !IsPlaneObject && !IsCollideObject)
                    return normalTextureName;
                else if (isPlaneObject)
                    return normalTextureName;
                else
                    return null;
            }

            set
            {
                Texture2D texture = EngineStart.engine.Content.Load<Texture2D>(value);
                model.SetTexture(texture, TextureType.BUMP);
                normalTextureName = Path.GetFileName(value);
            }
        }

        public string SpecularTexture
        {
            get
            {
                if (!IsPointlightEntity && !IsSpotlightEntity && !IsPlaneObject && !IsCollideObject)
                    return specularTextureName;
                else if (isPlaneObject)
                    return specularTextureName;
                else
                    return null;
            }

            set
            {
                Texture2D texture = EngineStart.engine.Content.Load<Texture2D>(value);
                model.SetTexture(texture, TextureType.SPECULAR);
                specularTextureName = Path.GetFileName(value);
            }
        }

        public string GlowTexture
        {
            get
            {
                if (!IsPointlightEntity && !IsSpotlightEntity && !IsPlaneObject && !IsCollideObject)
                    return glowTextureName;
                else if (isPlaneObject)
                    return glowTextureName;
                else
                    return null;
            }

            set
            {
                Texture2D texture = EngineStart.engine.Content.Load<Texture2D>(value);
                model.SetTexture(texture, TextureType.GLOW);
                glowTextureName = Path.GetFileName(value);
            }
        }

        public string MultiTex1
        {
            get
            {
                if (!IsPointlightEntity && !IsSpotlightEntity && !IsPlaneObject && !IsCollideObject)
                    return MultiTex1Name;
                else if (isPlaneObject)
                    return MultiTex1Name;
                else
                    return null;
            }

            set
            {
                Texture2D texture = EngineStart.engine.Content.Load<Texture2D>(value);
                model.SetTexture(texture, TextureType.MULTITEX1);
                MultiTex1Name = Path.GetFileName(value);
            }
        }

        public string MultiTex2
        {
            get
            {
                if (!IsPointlightEntity && !IsSpotlightEntity && !IsPlaneObject && !IsCollideObject)
                    return MultiTex2Name;
                else if (isPlaneObject)
                    return MultiTex2Name;
                else
                    return null;
            }

            set
            {
                Texture2D texture = EngineStart.engine.Content.Load<Texture2D>(value);
                model.SetTexture(texture, TextureType.MULTITEX2);
                MultiTex2Name = Path.GetFileName(value);
            }
        }

        public string MultiTex3
        {
            get
            {
                if (!IsPointlightEntity && !IsSpotlightEntity && !IsPlaneObject && !IsCollideObject)
                    return MultiTex3Name;
                else if (isPlaneObject)
                    return MultiTex3Name;
                else
                    return null;
            }

            set
            {
                Texture2D texture = EngineStart.engine.Content.Load<Texture2D>(value);
                model.SetTexture(texture, TextureType.MULTITEX3);
                MultiTex3Name = Path.GetFileName(value);
            }
        }

        public string MultiTex4
        {
            get
            {
                if (!IsPointlightEntity && !IsSpotlightEntity && !IsPlaneObject && !IsCollideObject)
                    return MultiTex4Name;
                else if (isPlaneObject)
                    return MultiTex4Name;
                else
                    return null;
            }

            set
            {
                Texture2D texture = EngineStart.engine.Content.Load<Texture2D>(value);
                model.SetTexture(texture, TextureType.MULTITEX4);
                MultiTex4Name = Path.GetFileName(value);
            }
        }

        public bool IsAudioObject
        {
            get;
            set;
        }

        public Vector3 AudioPosition
        {
            get;
            set;
        }

        public string Audiofile
        {
            get { return audioFile; }
            set { audioFile = value; }
        }

        public Vector3 CollideObjectDimensions
        {
            get { return boxDimensions; }
        }

        // Gets the main deffered screen instance
        [Browsable(false)]
        public DeferredScreen MainScreen
        {
            get { return mainScreen; }
        }

            
        #endregion

        #region Constructors
        // Empty/ Default constructor
        public SceneEntity()
        {
        }
       
        // Model creation
        public SceneEntity(IModelo model, string name, DeferredScreen mainScreen, bool isPlaneObject)
        {
            this.model = model;
            this.Name = name;
            this.isPlaneObject = isPlaneObject;
            this.mainScreen = mainScreen;
            rotation = Matrix.Identity;

            IObject modelObj;            
            boxObj = new BoxObject(Vector3.Zero, model.GetModelRadius(), model.GetModelRadius(), model.GetModelRadius(), 10, Vector3.One, Matrix.Identity, MaterialDescription.DefaultBepuMaterial());
            boxObj.isMotionLess = true;

            shader = new DeferredNormalShader();
            objMaterial = new DeferredMaterial(shader);
            modelObj = new IObject(objMaterial, model, boxObj);
            this.modelObj = modelObj;

            boxEffect = new BasicEffect(EngineStart.engine.GraphicsDevice);
            IsPointlightEntity = false;
            IsCollideObject = false;

            IsModel = true;
        }

        // Plane object creation
        public SceneEntity(BoxObject planeObject, SimpleModel model,Vector3 dimensions, Vector3 planeScale, DeferredScreen mainScreen)
        {
            this.model = model;
            //this.Name = name;
            //this.isPlaneObject = isPlaneObject;
            this.planeDimensions = dimensions;
            this.planeScale = planeScale;
            this.position = planeObject.Position;
            this.mainScreen = mainScreen;
            rotation = Matrix.Identity;

            IObject modelObj;
            boxObj = planeObject;
            boxObj.isMotionLess = true;
            model.SetTexture("Textures/AtticFloor_Planks_GmaeReady_700", TextureType.DIFFUSE);
          
            objBoundingBox = (BoundingBox)boxObj.BoundingBox;

            shader = new DeferredNormalShader();        
            objMaterial = new DeferredMaterial(shader);
            BasicMaterialDecorator materialDecor = new BasicMaterialDecorator(objMaterial, RasterizerState.CullNone);
            modelObj = new IObject(materialDecor, model, boxObj);
            this.modelObj = modelObj;

            boxEffect = new BasicEffect(EngineStart.engine.GraphicsDevice);
            IsPointlightEntity = false;
            IsCollideObject = false;
            mainScreen.World.AddObject(modelObj);
            planeDimensions = dimensions;
            isPlaneObject = true;
        }

        // Point light object creation
        public SceneEntity(PointLightPE pointlight, DeferredScreen mainScreen) // Used to add lights to the scene
        {
            if (lightBoundingBoxModel == null)
                lightBoundingBoxModel = new SimpleModel(mainScreen.GraphicFactory, "3DGizmoContent/LightBoundingBox");

            this.pointLightObject = pointlight;
            lightBoundsMin = new Vector3(-0.001f, -0.001f, -56.801f);
            lightBoundsMax = new Vector3(56.801f, 56.801f, 0.001f);
            center = lightBoundsMin + (lightBoundsMax - lightBoundsMin) / 2;

            position = pointlight.LightPosition;
            boxObj = new BoxObject(pointlight.LightPosition, lightBoundingBoxModel.GetModelRadius(), lightBoundingBoxModel.GetModelRadius(), lightBoundingBoxModel.GetModelRadius(), 10, Vector3.One, Matrix.Identity, MaterialDescription.DefaultBepuMaterial());
            boxObj.isMotionLess = true;
            

            mainScreen.World.AddLight(pointlight);

            isSpotlightEntity = false;
            IsPointlightEntity = true;
            IsCollideObject = false;

            boxEffect = new BasicEffect(EngineStart.engine.GraphicsDevice);
 
            if (this.mainScreen == null)
                this.mainScreen = mainScreen;
        }

        // Spot light creation
        public SceneEntity(SpotLightPE spotlight, DeferredScreen mainScreen)
        {
            if (lightBoundingBoxModel == null)
                lightBoundingBoxModel = new SimpleModel(mainScreen.GraphicFactory, "3DGizmoContent/LightBoundingBox");

            this.spotlightObject = spotlight;

            lightBoundsMin = new Vector3(-0.001f, -0.001f, -56.801f);
            lightBoundsMax = new Vector3(56.801f, 56.801f, 0.001f);
            center = lightBoundsMin + (lightBoundsMax - lightBoundsMin) / 2;

            position = spotlight.Position;
            boxObj = new BoxObject(spotlight.Position, lightBoundingBoxModel.GetModelRadius(), lightBoundingBoxModel.GetModelRadius(), lightBoundingBoxModel.GetModelRadius(), 10, Vector3.One, Matrix.Identity, MaterialDescription.DefaultBepuMaterial());
            boxObj.isMotionLess = true;

            mainScreen.World.AddLight(spotlight);

            isSpotlightEntity = true;
            IsCollideObject = false;

            boxEffect = new BasicEffect(EngineStart.engine.GraphicsDevice);

            if (this.mainScreen == null)
                this.mainScreen = mainScreen;
        }
       
        // Collide object creation
        public SceneEntity(BoxObject collideObject, BoundingBox boundingBox, Vector3 dimensions, DeferredScreen mainScreen)
        {
            //this.boxDimensions = dimensions;
            graveYard = new Vector3(99999, 99999, 99999);

            SimpleModel dummyModel = new SimpleModel(EngineStart.mainScreen.GraphicFactory, "3DGizmoContent/box");
            dummyModel.SetTexture(EngineStart.mainScreen.GraphicFactory.CreateTexture2DColor(1, 1, Microsoft.Xna.Framework.Color.Red), TextureType.DIFFUSE);

            //Vector3 boxDimensions = boundingBox.Max - boundingBox.Min;
            //Vector3 origBoundingBoxCenter = boundingBox.Max - (boundingBox.Max - boundingBox.Min) / 2;
            //BoxObject callBackBox = new BoxObject(origBoundingBoxCenter, boxDimensions.X, boxDimensions.Y, boxDimensions.Z, 10f, Vector3.One, Matrix.Identity, MaterialDescription.DefaultBepuMaterial());
            //callBackBox.isMotionLess = true;

            ForwardTransparenteShader forwardShader = new ForwardTransparenteShader(100); // TODO: may have to figure another way of making the cube transparent

            DeferredNormalShader modelShader = new DeferredNormalShader();
            DeferredMaterial modelMaterial = new DeferredMaterial(modelShader);
            modelMaterial.IsVisible = false;
            //GhostObject ghostObject = new GhostObject(collideObject.BoundingBox);
  
            ForwardMaterial forwardMaterial = new ForwardMaterial(forwardShader);
            forwardMaterial.IsVisible = false;
            IObject modelObj = new IObject(modelMaterial, dummyModel, collideObject); //TODO: Delete the model attached to the collide object bounding box somehow!
         
            EngineStart.mainScreen.World.AddObject(modelObj);

           
            boxObj = collideObject;
            objBoundingBox = (BoundingBox)boxObj.BoundingBox;
            position = boxObj.Position;
            boxDimensions = boundingBox.Max - boundingBox.Min;

            IsPointlightEntity = false;
            IsCollideObject = true;

        }

        // Audio object creation
        public SceneEntity(string audioName)
        {
            if(audioBoundingBoxModel == null)
                audioBoundingBoxModel = new SimpleModel(EngineStart.mainScreen.GraphicFactory, "3DGizmocontent/box");

            audioFile = audioName;

            BoundingBox boundingBox = new BoundingBox(new Vector3(-0.001f, -0.001f, -56.801f), new Vector3(56.801f, 56.801f, 0.001f));
            center = boundingBox.Max + (boundingBox.Max - boundingBox.Min) / 2;

            BoxObject boxObject = new BoxObject(Vector3.Zero, audioBoundingBoxModel.GetModelRadius(), audioBoundingBoxModel.GetModelRadius(), audioBoundingBoxModel.GetModelRadius(), 10f, Vector3.One, Matrix.Identity, MaterialDescription.DefaultBepuMaterial());
            boxObject.isMotionLess = true;

            boxObj = boxObject;
            boxObj.isMotionLess = true;
            boxObj.Position = Vector3.Zero;
            position = Vector3.Zero;

            IsAudioObject = true;

            position = Vector3.Zero;

            boxEffect = new BasicEffect(EngineStart.engine.GraphicsDevice);
        }

        #endregion

        #region Methods
        public void Update()
        {
            // *** Pre Existing 3DGizmo Code ***
            Vector3 up = Up;
            up.Normalize();

            Vector3 forward = Forward;
            forward.Normalize();
        
            world = Matrix.CreateScale(scale) * Matrix.CreateWorld(position, forward, up);
            // *** Pre Existing 3DGizmo Code ***

            #region Model Update Code
            if (IsModel && modelObj.PhysicObject.Scale != scale)
            {
                mainScreen.World.RemoveObject(modelObj);
                boxObj = new BoxObject(position, model.GetModelRadius(), model.GetModelRadius(), model.GetModelRadius(), 10, scale, rotation, MaterialDescription.DefaultBepuMaterial());
                boxObj.isMotionLess = true;
                modelObj = new IObject(objMaterial, model, boxObj);
                mainScreen.World.AddObject(modelObj);
            }
            boxObj.Rotation = rotation; // This must be set each frame - When objects are duplicated their rotations wont be correct without this assignment
            //boxObj.Scale = scale;
            #endregion

            #region Collide object update code
            if (IsPointlightEntity == false && IsCollideObject == true) // Collide object update code
            {

                boxObj.Position = position;
                objBoundingBox = (BoundingBox)boxObj.BoundingBox;
                //Editor.collideCallbackList.Add((BoundingBox)boxObj.BoundingBox);
                DebugShapeRenderer.AddBoundingBox((BoundingBox)boxObj.BoundingBox, Color.Red);

            }
            #endregion

            #region SpotLight Update Code
            if (isSpotlightEntity)
            {
                //boxObj.Position = position;
                //spotlightObject.Position = position;
            }
            #endregion

            #region Scaling Limits
            // The following code sets scaling limits - The user cannot scale beyond 4x the initial size
            if (scale.X < 0.35f)
                scale.X = 0.35f;
            if (scale.Y < 0.35f)
                scale.Y = 0.35f;
            if(scale.Z < 0.35f)
                scale.Z = 0.35f;

            if (scale.X > 4f)
                scale.X = 4f;
            if (scale.Y > 4f)
                scale.Y = 4f;
            if (scale.Z > 4f)
                scale.Z = 4f;
            #endregion
        }
 
        public void Draw()
        {
            #region Non-LightEntity Draw Code
            if (hasDrawn == false && IsModel)
            {
                mainScreen.World.AddObject(modelObj);
                hasDrawn = true;
            }

            if (IsModel)
            {
                                                                                                        // ***
                TriangleMeshObject boundingBoxMesh = new TriangleMeshObject(model, position, rotation,  // Used to display the bounding box of objects
                   scale, MaterialDescription.DefaultBepuMaterial());                                   // ***
                this.boundingBoxMesh = boundingBoxMesh;

                objBoundingBox = new BoundingBox(boundingBoxMesh.BoundingBox.Value.Min, boundingBoxMesh.BoundingBox.Value.Max);

                boxObj.Position = position;
            }

            if (drawBox && IsSpotlightEntity) // Spotlight object draw code
            {
               DrawBoundingBox(objBoundingBox);
            }
           
            #endregion

            #region PointLight Draw Code
            if (IsPointlightEntity)
            {
                BoundingSphere lightBoundingSphere = new BoundingSphere(position, pointLightObject.LightRadius);
                DebugShapeRenderer.AddBoundingSphere(lightBoundingSphere, Color.White);
                TriangleMeshObject lightBoundingBoxMesh = new TriangleMeshObject(lightBoundingBoxModel, position - center, Matrix.Identity,
                    Vector3.One, MaterialDescription.DefaultBepuMaterial());
                
                objBoundingBox = new BoundingBox(lightBoundingBoxMesh.BoundingBox.Value.Min, lightBoundingBoxMesh.BoundingBox.Value.Max);
               
                boxObj.Position = position;
                pointLightObject.LightPosition = lightBoundingSphere.Center;
                DebugShapeRenderer.AddBoundingSphere(new BoundingSphere(lightBoundingSphere.Center, 8f), Color.Purple);
            }

            if (IsAudioObject)
            {
                BoundingSphere audioBoundingSphere = new BoundingSphere(position, 10);
                DebugShapeRenderer.AddBoundingSphere(audioBoundingSphere, Color.Yellow);

                TriangleMeshObject audioTriengleMesh = new TriangleMeshObject(audioBoundingBoxModel, position, Matrix.Identity,
                    Vector3.One, MaterialDescription.DefaultBepuMaterial());

                objBoundingBox = new BoundingBox(audioTriengleMesh.BoundingBox.Value.Min, audioTriengleMesh.BoundingBox.Value.Max);

                boxObj.Position = position;
                AudioPosition = audioBoundingSphere.Center;

                DebugShapeRenderer.AddBoundingSphere(new BoundingSphere(audioBoundingSphere.Center, 350f), Color.White);
            }

            if (drawBox && IsAudioObject)
            {
                DrawBoundingBox(objBoundingBox);
            }

            

            //if (IsPointlightEntity && drawBox) // Delete this when you dont want bounding boxes displayed with point lights
                //DrawBoundingBox(objBoundingBox);
                //DebugShapeRenderer.AddBoundingBox(objBoundingBox, Color.White); // Can use either or method of drawing boudning objects 
            //TODO: Switch all bounding objects to debugShaperenderer for optimization? (Just a thought)        
            #endregion

            #region SpotLight Draw Code
            if (isSpotlightEntity)
            {
                // Creates a bounding frustrum that represents the spot lights direction
                //Matrix frustrumMatrix = Matrix.Multiply(spotlightObject.ViewMatrix, spotlightObject.ProjMatrix);
                //spotlightObject.Target = spotlightObject.Direction + new Vector3(25);
                BoundingFrustum lightFrustrum = new BoundingFrustum(Matrix.CreateLookAt(spotlightObject.Position, Vector3.Transform(spotlightObject.Target, rotation) + spotlightObject.Position, Vector3.Up) * spotlightObject.ProjMatrix * Matrix.CreateScale(new Vector3(5 - (spotlightObject.LightRadius / 300), 5 - (spotlightObject.LightRadius / 300), 1.0008f /*spotlightObject.ConeDecay / 99.92f*/)));
                DebugShapeRenderer.AddBoundingFrustum(lightFrustrum, Color.White);
                lightFrustrum.Matrix *= Matrix.CreateScale(0.5f);
                TriangleMeshObject lightBoundingBoxMesh = new TriangleMeshObject(lightBoundingBoxModel, position - center, Matrix.Identity,
                    Vector3.One, MaterialDescription.DefaultBepuMaterial());
                objBoundingBox = new BoundingBox(lightBoundingBoxMesh.BoundingBox.Value.Min, lightBoundingBoxMesh.BoundingBox.Value.Max);

                boxObj.Position = position;
                spotlightObject.Position = boxObj.Position;
                //spotlightObject.Direction = new Vector3(spotlightDirection, 0, 0);
            }
            #endregion

            #region PlaneObject Draw Code

            if (isPlaneObject)
            {

                TriangleMeshObject boundingBoxMesh = new TriangleMeshObject(model, position, rotation,
                   new Vector3(planeDimensions.X / 25, 0.25f, planeDimensions.Z / 25), MaterialDescription.DefaultBepuMaterial());
                objBoundingBox = new BoundingBox(boundingBoxMesh.BoundingBox.Value.Min, boundingBoxMesh.BoundingBox.Value.Max);
                boxObj.Position = position;

            }
            if (isPlaneObject && drawBox)
                DrawBoundingBox(objBoundingBox);

            #endregion
        }

        #region BoundingBox drawing helper methods
        void DrawBoundingBox(BoundingBox box) // Used to draw the bounding box of objects
        {
            Vector3[] corners = box.GetCorners();
            VertexPositionColor[] primititveList = new VertexPositionColor[corners.Length];

            // Assign the 8 box vertices
            for (int i = 0; i < corners.Length; i++)
            {
                primititveList[i] = new VertexPositionColor(corners[i], Color.Red);
            }

            boxEffect.World = Matrix.Identity;
            boxEffect.View = Engine.View;
            boxEffect.Projection = Engine.Projection;
            boxEffect.TextureEnabled = false;
            boxEffect.World = Matrix.Identity;
            
            foreach (EffectPass pass in boxEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                EngineStart.engine.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                EngineStart.engine.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
                EngineStart.engine.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, primititveList, 0, 8, bBoxIndices, 0, 12);
            }
        }

        void DrawBoundingBox(BoundingBox? box) // Used to draw the bounding box of objects using a nullable bounding box - Not sure if thi is needed anymore delete when ready (Maybe)
        {
            Vector3[] corners = box.Value.GetCorners();
            VertexPositionColor[] primititveList = new VertexPositionColor[corners.Length];

            // Assign the 8 box vertices
            for (int i = 0; i < corners.Length; i++)
            {
                primititveList[i] = new VertexPositionColor(corners[i], Color.Red);
            }

            boxEffect.World = Matrix.Identity;
            boxEffect.View = Engine.View;
            boxEffect.Projection = Engine.Projection;
            boxEffect.TextureEnabled = false;
            boxEffect.World = Matrix.Identity;

            foreach (EffectPass pass in boxEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                EngineStart.engine.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, primititveList, 0, 8, bBoxIndices, 0, 12);
            }
        }
        #endregion

        #endregion
    }
}
