using PloobsEngine.Engine;
using PloobsEngine.SceneControl;
using PloobsEngine.Engine.Logger;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TGEditor
{
    /// <summary>
    /// The editors entry point
    /// </summary>
    public class EngineStart
    {
        public static Editor editorForm;
        static GraphicsDeviceManager graphics;
        public static EngineStuff engine;
        public static InitialEngineDescription desc;
        public static DeferredScreen mainScreen;
        public static bool isDebug;
        public static StorableList ImportedModelsList; // Used to display the list of mdels that have been imported into the editor
        SimpleLogger logger;

        public EngineStart()
        {
            desc = InitialEngineDescription.Default();
            ///We are using the simplest parameters to work in all computers
            ///Check the Advanced Demos to know how to change those
            ///optional parameters, the default is good for most situations
            //desc.UseVerticalSyncronization = true;
            //desc.isFixedGameTime = true;
            //desc.isMultiSampling = true; ///Only works on forward rendering
            //desc.useMipMapWhenPossible = true;
            logger = new SimpleLogger();
            desc.Logger = logger;
            desc.UnhandledException_Handler = UnhandledException;
            desc.BackBufferWidth = 1093;
            desc.BackBufferHeight = 874;
            isDebug = true;
     
            ///start the engine
            using (engine = new EngineStuff(ref desc, LoadScreen))
            {
                engine.Content.RootDirectory = "TGEditor-CContent"; // //TODO: Change this since test content is not the permanent//
                InitializeImportedModelsListBox();
                editorForm = new Editor();
                editorForm.Size = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height - 40);
                graphics = (GraphicsDeviceManager)((IGraphicsDeviceManager)engine.Services.GetService(typeof(IGraphicsDeviceManager)));
                editorForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(editorForm_FormClosing);
                graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
                //editorForm.MaximumSize = new System.Drawing.Size(editorForm.Width, editorForm.Height);
                //editorForm.MinimumSize = new System.Drawing.Size(250, 250);
                System.Drawing.Size formStartsize = editorForm.Size;
                //formStartsize.Width /= 2;
                //formStartsize.Height /= 2;
                editorForm.Size = formStartsize;
                //editorForm.SplitContainer.Width = editorForm.Width - 74; TODO-Panel
                //editorForm.SplitContainer.Height = editorForm.Height - 120;
                editorForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                engine.Run();
            }
        }

        void editorForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            // Displays a text box asking if the user would like to save/exit
            if (!Editor.hasSavedSession || Editor.changesHaveBeenMade)
            {
                DialogResult messageBox = MessageBox.Show("Would you like to save " + "'" + Path.GetFileName(Editor.saveName) + "'" + " ?", "Save?", MessageBoxButtons.YesNoCancel);
                if (messageBox == DialogResult.Yes)
                {
                    Stream stream = null;
                    IFormatter formatter = new BinaryFormatter();

                    if (Path.GetFileName(Editor.saveName) == "Untitled")
                        stream = new FileStream(engine.Content.RootDirectory + "/Saves/untitled", FileMode.Create, FileAccess.Write, FileShare.None);
                    else
                        stream = new FileStream(Editor.saveName, FileMode.Create, FileAccess.Write, FileShare.None);

                    formatter.Serialize(stream, new SceneSerializer(Engine.Entities));
                }
                else if (messageBox == DialogResult.Cancel)
                    e.Cancel = true;
            }
            else
            {
                DialogResult messageBox = MessageBox.Show("Are you sure you want to exit?", "Exit?", MessageBoxButtons.YesNo);
                if (messageBox == DialogResult.No)
                    e.Cancel = true;
            }

            // Creates a text file that stores error logs once the form closes
            StorableList storableList = new StorableList(Editor.listboxModelsList);
            using(StreamWriter writer = new StreamWriter(engine.Content.RootDirectory + "/bin/logger.txt", true))
            {
                foreach(string log in logger.logList)
                {
                    writer.WriteLine(log);
                }
            }

            if (e.Cancel == false)
                engine.Exit();
            
        }

        void InitializeImportedModelsListBox() // Deserializes the file that the list of imported models is in; So the list of models can be displayed in the listbox to the right of the form
        {
            // This all saves a list of the models that have been loaded into the editor for easy access
            IFormatter formatter;
            Stream stream;

            formatter = new BinaryFormatter();
            try
            {
                stream = new FileStream(engine.Content.RootDirectory + "/bin/TestList", FileMode.Open, FileAccess.Read, FileShare.None);

                ImportedModelsList = (StorableList)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (FileNotFoundException fNFException)
            {
                logger.Log("Exception:" + fNFException.ToString() + " - Could not create loaded models list", LogLevel.RecoverableError);
            }
            catch (DirectoryNotFoundException directoryException)
            {
                logger.Log("Exception: " + directoryException.ToString() + "loaded models list has not been created, Supsected first time launch", LogLevel.Warning);
            }
        }

        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = editorForm.XnaMainDisplayPanel.Handle;
        }


        static void LoadScreen(ScreenManager manager)
        {
            ///add the title screen
            manager.AddScreen(mainScreen = new DeferredScreen(graphics, editorForm));
        }

        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ///handle unhandled excetption here (log, send to a server ....)
            Console.WriteLine("Exception: " + e.ToString());
        }
    }

    /// <summary>
    /// Custom log class
    /// </summary>
    class SimpleLogger : ILogger
    {
        #region ILogger Members

        public List<string> logList = new List<string>();
        public override void Log(string Message, LogLevel logLevel)
        {
            ///handle messages logs
            Console.WriteLine(Message + "  -  " + logLevel.ToString());
            logList.Add(Message + " - " + logLevel.ToString());
        }

        #endregion
    }
}




