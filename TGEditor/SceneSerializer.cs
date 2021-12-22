using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PloobsEngine.Modelo;
using PloobsEngine.Physics.Bepu;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;

namespace TGEditor
{
    [Serializable]
    public struct SceneSerializer
    {
        public ArrayList listOfObjects;

        public SceneSerializer(List<SceneEntity> sceneInfo)
        {
            listOfObjects = new ArrayList();

            string modelName;

            Vector3 position;
            Vector3 scale;
            Matrix rotation;

            // Physic object variables
            Vector3 physObjPosition;
            Vector3 physObjScale;
            Matrix physObjRot; // Rotation of the physic object

            float physObjWidth, physObjHeight, physObjLength;
            float physObjMass;

            bool isMotionless;

            // Light variables
            float lightIntensity;
            float lightRadius;
            Vector3 lightDirection; // For spotlights only
            Color lightColor;
            bool lightEnabled;
            bool castShadows;

            string diffuesTexture, specularTexture, normalTexture, glowTexture, multiTex1, multiTex2, multiTex3, multiTex4;

            Vector3 collideObjectDimensions;

            //BoxObject collideObject; Not serilizable

            // Audio object variables
            string audioFileName;

            string isModel;
            string isPlane;
            string isPointLight;
            string isSpotLight;
            string isCollideObject;
            string isAudioObject;

            foreach (SceneEntity entity in sceneInfo)
            {
                if(entity.IsModel) // Serialize a model
                {
                    modelName = entity.Name;
                    position = entity.position;
                    scale = entity.scale;
                    rotation = entity.rotation;
                    isMotionless = entity.IsMotionless;

                    if (entity.DiffuseTexture != null)
                        diffuesTexture = "Textures/" + entity.DiffuseTexture;
                    else
                        diffuesTexture = null;

                    if (entity.SpecularTexture != null)
                        specularTexture = "Textures/" + entity.SpecularTexture;
                    else
                        specularTexture = null;

                    if (entity.NormalTexture != null)
                        normalTexture = "Textures/" + entity.NormalTexture;
                    else
                        normalTexture = null;

                    if (entity.GlowTexture != null)
                        glowTexture = "Textures/" + entity.GlowTexture;
                    else
                        glowTexture = null;

                    if (entity.MultiTex1 != null)
                        multiTex1 = "Textures/" + entity.MultiTex1;
                    else
                        multiTex1 = null;

                    if (entity.MultiTex2 != null)
                        multiTex2 = "Textures/" + entity.MultiTex2;
                    else
                        multiTex2 = null;

                    if (entity.MultiTex3 != null)
                        multiTex3 = "Textures/" + entity.MultiTex3;
                    else
                        multiTex3 = null;

                    if (entity.MultiTex4 != null)
                        multiTex4 = "Textures/" + entity.MultiTex4;
                    else
                        multiTex4 = null;

                    isModel = "isModel";

                    object[] modelInfo = new object[21];
                    modelInfo[0] = isModel; //  Identifies this object as a model
                    modelInfo[1] = modelName;   // The model's name(So it can be loaded)
                    modelInfo[2] = position;    // The model's position
                    modelInfo[3] = scale;   // The model's Scale
                    modelInfo[4] = rotation;    // The model's rotaion;
                    modelInfo[5] = isMotionless; // Determines wheather the object is effected by gravity

                    // All the textures that the model uses
                    modelInfo[6] = diffuesTexture;
                    modelInfo[7] = normalTexture;
                    modelInfo[8] = specularTexture;
                    modelInfo[9] = glowTexture;
                    modelInfo[10] = multiTex1;
                    modelInfo[11] = multiTex2;
                    modelInfo[12] = multiTex3;
                    modelInfo[13] = multiTex4;

                    if (entity.PhysicObject is BoxObject)
                    {
                        BoxObject boxObject = entity.PhysicObject as BoxObject;

                        physObjPosition = boxObject.Position;
                        physObjScale = boxObject.Scale;
                        physObjRot = boxObject.Rotation;

                        BEPUphysics.Entities.Prefabs.Box box = (BEPUphysics.Entities.Prefabs.Box)boxObject.Entity;
                        physObjWidth = box.Width / physObjScale.X;
                        physObjHeight = box.Height / physObjScale.Y;
                        physObjLength = box.Length / physObjScale.Z;
                        physObjMass = 10;

                        if (!boxObject.isMotionLess)
                        {
                            physObjMass = box.Mass;
                        }

                        modelInfo[14] = physObjPosition;
                        modelInfo[15] = physObjWidth;
                        modelInfo[16] = physObjHeight;
                        modelInfo[17] = physObjLength;
                        modelInfo[18] = physObjMass;
                        modelInfo[19] = physObjScale;
                        modelInfo[20] = physObjRot;

                    }
                    

                    listOfObjects.Add(modelInfo);
                }

                if (entity.IsPointlightEntity) // Serialize a pointlight
                {
                    position = entity.position; // The light's position
                    lightColor = entity.PointLightColor;    // The light's Color
                    lightRadius = entity.PointLightRadius;  // The light's radius
                    lightIntensity = entity.PointLightIntesity; // The light's intensity
                    lightEnabled = entity.PointLightIsEnabled; // Determines wheather the pointLight is visible
                    castShadows = entity.PointLightObject.CastShadown; // Determines wheather the pointlight casts shadows

                    isPointLight = "isPointLight";

                    object[] pointLightInfo = new object[7];
                    pointLightInfo[0] = isPointLight;
                    pointLightInfo[1] = position;
                    pointLightInfo[2] = lightColor;
                    pointLightInfo[3] = lightRadius;
                    pointLightInfo[4] = lightIntensity;
                    pointLightInfo[5] = lightEnabled;
                    pointLightInfo[6] = castShadows;

                    listOfObjects.Add(pointLightInfo);
                }

                if (entity.IsSpotlightEntity) // Serialize a spotlight
                {
                    position = entity.position;
                    lightDirection = entity.SpotLightDirection;
                    lightColor = entity.SpotLightColor;
                    lightRadius = entity.SpotLightRadius;
                    lightIntensity = entity.SpotLightIntesity;
                    lightEnabled = entity.SpotLightEnabled;
                    castShadows = entity.SpotlightObject.CastShadown;

                    isSpotLight = "isSpotLight";
                    
                    object[] spotLightInfo = new object[8];
                    spotLightInfo[0] = isSpotLight;
                    spotLightInfo[1] = position;
                    spotLightInfo[2] = lightDirection;
                    spotLightInfo[3] = lightColor;
                    spotLightInfo[4] = lightRadius;
                    spotLightInfo[5] = lightIntensity;
                    spotLightInfo[6] = lightEnabled;
                    spotLightInfo[7] = castShadows;

                    listOfObjects.Add(spotLightInfo);
                }

                if (entity.IsPlaneObject) // Serialize a plane object
                {
                    modelName = entity.Name;
                    position = entity.position;
                    scale = entity.PlaneScale;
                    rotation = entity.rotation;

                    if (entity.DiffuseTexture != null)
                        diffuesTexture = "Textures/" + entity.DiffuseTexture;
                    else
                        diffuesTexture = null;

                    if (entity.SpecularTexture != null)
                        specularTexture = "Textures/" + entity.SpecularTexture;
                    else
                        specularTexture = null;

                    if (entity.NormalTexture != null)
                        normalTexture = "Textures/" + entity.NormalTexture;
                    else
                        normalTexture = null;

                    if (entity.GlowTexture != null)
                        glowTexture = "Textures/" + entity.GlowTexture;
                    else
                        glowTexture = null;

                    if (entity.MultiTex1 != null)
                        multiTex1 = "Textures/" + entity.MultiTex1;
                    else
                        multiTex1 = null;

                    if (entity.MultiTex2 != null)
                        multiTex2 = "Textures/" + entity.MultiTex2;
                    else
                        multiTex2 = null;

                    if (entity.MultiTex3 != null)
                        multiTex3 = "Textures/" + entity.MultiTex3;
                    else
                        multiTex3 = null;

                    if (entity.MultiTex4 != null)
                        multiTex4 = "Textures/" + entity.MultiTex4;
                    else
                        multiTex4 = null;

                    isPlane = "isPlane";

                    object[] planeObjectInfo = new object[15];
                    planeObjectInfo[0] = isPlane;
                    planeObjectInfo[1] = modelName; // The objects model
                    planeObjectInfo[2] = position;  // The objects position
                    planeObjectInfo[3] = scale; // The object scale - Differs froma models scale in that is is used to create a plane of the proper size
                    planeObjectInfo[4] = entity.PlaneDimensions;    // Creates the planes bounding box
                    planeObjectInfo[5] = rotation;  // Rotation of the plane

                    // All the textures the the plane uses
                    planeObjectInfo[6] =  "Textures/" + diffuesTexture;
                    planeObjectInfo[7] = "Textures/" + normalTexture;
                    planeObjectInfo[8] = "Textures/" + specularTexture;
                    planeObjectInfo[9] = "Textures/" + glowTexture;
                    planeObjectInfo[10] = "Textures/" + multiTex1;
                    planeObjectInfo[11] = "Textures/" + multiTex2;
                    planeObjectInfo[12] = "Textures/" + multiTex3;
                    planeObjectInfo[13] = "Textures/" + multiTex4;

                    listOfObjects.Add(planeObjectInfo);
                }

                if (entity.IsCollideObject) // Serialize a collide object
                {
                    modelName = "Models/box";
                    position = entity.position;
                    collideObjectDimensions = entity.CollideObjectDimensions;

                    isCollideObject = "isCollideObject";

                    object[] collideObjectInfo = new object[4];
                    collideObjectInfo[0] = isCollideObject;
                    collideObjectInfo[1] = modelName;
                    collideObjectInfo[2] = position;
                    collideObjectInfo[3] = collideObjectDimensions;

                    listOfObjects.Add(collideObjectInfo);
                }

                if (entity.IsAudioObject)
                {
                    modelName = entity.Audiofile;

                    position = entity.AudioPosition;
                    audioFileName = entity.Audiofile;

                    isAudioObject = "isAudioObject";

                    object[] audioObjectInfo = new object[3];
                    audioObjectInfo[0] = isAudioObject;
                    audioObjectInfo[1] = audioFileName;
                    audioObjectInfo[2] = position;

                    listOfObjects.Add(audioObjectInfo);
                }
            }
        }
    }
}
