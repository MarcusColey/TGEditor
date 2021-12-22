using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using PloobsEngine.Modelo;
using PloobsEngine.Physics.Bepu;
using PloobsEngine.Material;
using PloobsEngine.Light;
using PloobsEngine.SceneControl;
using Microsoft.Xna.Framework;
using PloobsEngine.Physics;

namespace TGEditor
{
    public class TGSceneLoader
    {

        ArrayList objectList;
        public TGSceneLoader(ArrayList objectList, bool importScene = false)
        {
            this.objectList = objectList;

            foreach (object[] sceneObject in objectList)
            {
                if((string)sceneObject[0] == "isModel")
                {
                    IModelo model = new SimpleModel(EngineStart.mainScreen.GraphicFactory, (string)sceneObject[1]);
                    DeferredNormalShader shader = new DeferredNormalShader();
                    DeferredMaterial material = new DeferredMaterial(shader);
                    BoxObject modelPhycisObject = new BoxObject((Vector3)sceneObject[2], model.GetModelRadius(), model.GetModelRadius(), model.GetModelRadius(), 10f, (Vector3)sceneObject[3], (Matrix)sceneObject[4], MaterialDescription.DefaultBepuMaterial());
                    modelPhycisObject.isMotionLess = true;
                    IObject modelObject = new IObject(material, model, modelPhycisObject);

                    SceneEntity modelEntity;
                    Engine.Entities.Add(modelEntity = new SceneEntity(model, (string)sceneObject[1], EngineStart.mainScreen, false));
                    modelEntity.position = (Vector3)sceneObject[2];
                    modelEntity.scale = (Vector3)sceneObject[3];
                    modelEntity.rotation = (Matrix)sceneObject[4];

                    if (importScene)
                        GizmoComponent.Selection.Add(modelEntity);

                }
                if ((string)sceneObject[0] == "isPointLight")
                {
                    PointLightPE pointLight = new PointLightPE((Vector3)sceneObject[1], (Color)sceneObject[2], (float)sceneObject[3], (float)sceneObject[4]);
                    SceneEntity pointLightEntity;
                    Engine.Entities.Add(pointLightEntity = new SceneEntity(pointLight, EngineStart.mainScreen));

                    if (importScene)
                        GizmoComponent.Selection.Add(pointLightEntity);
                }
                if ((string)sceneObject[0] == "isSpotLight")
                {
                    SpotLightPE spotLight = new SpotLightPE((Vector3)sceneObject[1], (Vector3)sceneObject[2], 50, (float)sceneObject[4], (Color)sceneObject[3], (float)Math.Cos(Math.PI / 7), (float)sceneObject[5]);
                    SceneEntity spotLightEntity;
                    Engine.Entities.Add(spotLightEntity = new SceneEntity(spotLight, EngineStart.mainScreen));

                    if (importScene)
                        GizmoComponent.Selection.Add(spotLightEntity);
                }
                if ((string)sceneObject[0] == "isPlane")
                {
                    SimpleModel model = new SimpleModel(EngineStart.mainScreen.GraphicFactory, "Models/box");
                    //model.SetTexture((Texture2D)sceneObject[6], TextureType.DIFFUSE);
                    model.SetTexture(EngineStart.mainScreen.GraphicFactory.CreateTexture2DColor(1, 1, Color.Red), TextureType.DIFFUSE);
                    DeferredNormalShader shader = new DeferredNormalShader();
                    DeferredMaterial material = new DeferredMaterial(shader);
                    Vector3 boxXYZ = (Vector3)sceneObject[4];
                    Vector3 scale = (Vector3)sceneObject[3];
                    BoxObject modelPhysObject = new BoxObject(Vector3.Zero, 1, 1, 1, 10f, new Vector3(scale.X, 1, scale.Z), Matrix.Identity, MaterialDescription.DefaultBepuMaterial());
                    modelPhysObject.isMotionLess = true;
                    IObject modelObject = new IObject(material, model, modelPhysObject);

                    SceneEntity planeEntity;
                    Engine.Entities.Add(planeEntity = new SceneEntity(modelPhysObject, model, boxXYZ, scale, EngineStart.mainScreen));

                    if (importScene)
                        GizmoComponent.Selection.Add(planeEntity);

                }
                if((string)sceneObject[0] == "isCollideObject")
                {
                    IModelo model = new SimpleModel(EngineStart.mainScreen.GraphicFactory, (string)sceneObject[1]);
                    DeferredNormalShader shader = new DeferredNormalShader();
                    DeferredMaterial material = new DeferredMaterial(shader);
                    Vector3 dimensions = (Vector3)sceneObject[3];
                    BoxObject modelPhysObject = new BoxObject((Vector3)sceneObject[2], dimensions.X, dimensions.Y, dimensions.Z, 10f, Vector3.One, Matrix.Identity, MaterialDescription.DefaultBepuMaterial());
                    modelPhysObject.isMotionLess = true;
                    IObject modelObject = new IObject(material, model, modelPhysObject);
                    material.IsVisible = false;

                    SceneEntity collideObjectEntity;
                    Engine.Entities.Add(collideObjectEntity = new SceneEntity(modelPhysObject, (BoundingBox)modelPhysObject.BoundingBox, dimensions, EngineStart.mainScreen));

                    if (importScene)
                        GizmoComponent.Selection.Add(collideObjectEntity);
                    
                }

            }

        }

    }
}
