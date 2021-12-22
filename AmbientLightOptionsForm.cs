using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PloobsEngine.Light;

namespace TGEditor
{
    public partial class AmbientLightOptionsForm : Form
    {
        static int red, green, blue;
        static Microsoft.Xna.Framework.Color lightColor;

        public AmbientLightOptionsForm()
        {
            InitializeComponent();
            /*if (red != 0 && green != 0 && blue != 0)
            {
                RedValueTexBox.Text = red.ToString();
                GreenValueTextBox.Text = green.ToString();
                BlueValueTextBox.Text = blue.ToString();
            }
            else
            {
                RedValueTexBox.Text = string.Empty;
                GreenValueTextBox.Text = string.Empty;
                BlueValueTextBox.Text = string.Empty;
            }*/
        }

        private void RedValueTexBox_TextChanged(object sender, EventArgs e)
        {
            foreach (DirectionalLightPE light in EngineStart.mainScreen.World.Lights)
            {
                try
                {
                    red = (int)byte.Parse(RedValueTexBox.Text);
                }
                catch (FormatException formatException)
                {
                    EngineStart.desc.Logger.Log(formatException.ToString() + "AmbientLight option text box triggerd a format error, ususlaly nothing to worry about", PloobsEngine.Engine.Logger.LogLevel.Warning);
                }
                    
                light.Color = ChangeColor(red, green , blue);
                RedValueTexBox.Text = red.ToString();
            }
        }

        private void GreenValueTextBox_TextChanged(object sender, EventArgs e)
        {
            foreach (DirectionalLightPE light in EngineStart.mainScreen.World.Lights)
            {
                try
                {
                    green = (int)byte.Parse(GreenValueTextBox.Text);
                }
                catch (FormatException formatException)
                {
                    EngineStart.desc.Logger.Log(formatException.ToString() + "AmbientLight option text box triggerd a format error, ususlaly nothing to worry about", PloobsEngine.Engine.Logger.LogLevel.Warning);
                }

                light.Color = ChangeColor(red, green, blue);
                GreenValueTextBox.Text = green.ToString();
            }
        }

        private void BlueValueTextBox_TextChanged(object sender, EventArgs e)
        {
            foreach (DirectionalLightPE light in EngineStart.mainScreen.World.Lights)
            {
                try
                {
                    blue = (int)byte.Parse(BlueValueTextBox.Text);
                }
                catch(FormatException formatException)
                {
                    EngineStart.desc.Logger.Log(formatException.ToString() + "AmbientLight option text box triggerd a format error, ususlaly nothing to worry about", PloobsEngine.Engine.Logger.LogLevel.Warning);
                }

                light.Color = ChangeColor(red, green, blue);
                BlueValueTextBox.Text = blue.ToString();
            }
        }

        Microsoft.Xna.Framework.Color ChangeColor(int r, int g, int b)
        {
            lightColor = new Microsoft.Xna.Framework.Color(red, green, blue);
            return lightColor;
        }
    }
}
