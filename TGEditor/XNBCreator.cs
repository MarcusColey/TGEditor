using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PloobsEngine.AssetLoader;
using System.Threading;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace TGEditor
{
    /// <summary>
    /// Creates .xnb content at runtime
    /// </summary>
    public class XNBCreator : ContentBuilder
    {
        ContentBuilder contentBuilder;
        string extension;

        // Stores a path to the normal map that is used for a given model
        string normalMapFileName;
        public static string NormalMap
        {
            get;
            set;
        }
   
        public XNBCreator(List<string> sourcePaths, bool loadTexture = false)
        {
            contentBuilder = new ContentBuilder();
            

            foreach (string filepath in sourcePaths)
            {
                // Sorts through all the files in th folder and stores any of use eg. files with tga/bmp extensions that has specific endings namely '_N' or '_D'
                extension = Path.GetExtension(filepath);
                if(extension == ".tga" || extension == ".bmp" || extension == ".jpg" || extension == ".png") // File is a texture
                {
                    contentBuilder.Add(filepath, Path.GetFileName(filepath.Remove(filepath.Length - 4)), "TextureImporter", "TextureProcessor");
                    if (filepath.ElementAt(filepath.Length - 6) == '_' && filepath.ElementAt(filepath.Length - 5) == 'N')
                    {
                       normalMapFileName = Path.GetFileName(filepath.Remove(filepath.Length - 4));
                    }
                }
                /*
                 * Note to artists creating models for this editor and XNA in general
                 * When exporting .fbx files from 3ds max the only options that need to be checked are
                 *  Geometry:
                 *  TurboSmooth
                 *  Convert deforming dummies to bones
                 *  preserve edge orientation
                 * And make sure under Fbx file format the version is 2010 or higher and the type is binary
                 */
                else if (extension == ".fbx" || extension == ".FBX") // File is a model
                {
                    contentBuilder.Add(filepath, Path.GetFileName(filepath.Remove(filepath.Length - 4)), "FbxImporter", "ModelProcessor");
                }

                else if (extension == ".wav")
                {
                    contentBuilder.Add(filepath, Path.GetFileNameWithoutExtension(filepath), "WavImporter", "SoundEffectProcessor");
                }
            }
            contentBuilder.Build();
            // After the content is built it is stored in a tempory folder were it will be copied and placed in the editors content directory
            string[] builtContent = Directory.GetFiles(contentBuilder.OutputDirectory, "*.xnb");
            for (int i = 0; i < builtContent.Length; i++)
            {
                string fileName = Path.GetFileName(builtContent[i]);
                if (!loadTexture)
                    File.Copy(builtContent[i], Path.Combine(EngineStart.engine.Content.RootDirectory, "Models/" + fileName));
                else if (loadTexture)
                    File.Copy(builtContent[i], Path.Combine(EngineStart.engine.Content.RootDirectory, "Textures/" + fileName));
                else if (extension == ".wav")
                    File.Copy(builtContent[i], Path.Combine(EngineStart.engine.Content.RootDirectory, "Audio/" + fileName));

                if (fileName.ElementAt(fileName.Length - 6) == '_' && fileName.ElementAt(fileName.Length - 5) == 'N')
                {
                    NormalMap = "Models/" + normalMapFileName;
                }
            }
        }
    }
}
