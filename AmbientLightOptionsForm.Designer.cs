namespace TGEditor
{
    partial class AmbientLightOptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RedLabel = new System.Windows.Forms.Label();
            this.BlueLabel = new System.Windows.Forms.Label();
            this.GreenLabel = new System.Windows.Forms.Label();
            this.RedValueTexBox = new System.Windows.Forms.TextBox();
            this.GreenValueTextBox = new System.Windows.Forms.TextBox();
            this.BlueValueTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // RedLabel
            // 
            this.RedLabel.AutoSize = true;
            this.RedLabel.Location = new System.Drawing.Point(40, 25);
            this.RedLabel.Name = "RedLabel";
            this.RedLabel.Size = new System.Drawing.Size(27, 13);
            this.RedLabel.TabIndex = 0;
            this.RedLabel.Text = "Red";
            // 
            // BlueLabel
            // 
            this.BlueLabel.AutoSize = true;
            this.BlueLabel.Location = new System.Drawing.Point(40, 57);
            this.BlueLabel.Name = "BlueLabel";
            this.BlueLabel.Size = new System.Drawing.Size(36, 13);
            this.BlueLabel.TabIndex = 1;
            this.BlueLabel.Text = "Green";
            // 
            // GreenLabel
            // 
            this.GreenLabel.AutoSize = true;
            this.GreenLabel.Location = new System.Drawing.Point(40, 89);
            this.GreenLabel.Name = "GreenLabel";
            this.GreenLabel.Size = new System.Drawing.Size(28, 13);
            this.GreenLabel.TabIndex = 2;
            this.GreenLabel.Text = "Blue";
            // 
            // RedValueTexBox
            // 
            this.RedValueTexBox.Location = new System.Drawing.Point(117, 22);
            this.RedValueTexBox.Name = "RedValueTexBox";
            this.RedValueTexBox.Size = new System.Drawing.Size(100, 20);
            this.RedValueTexBox.TabIndex = 3;
            this.RedValueTexBox.TextChanged += new System.EventHandler(this.RedValueTexBox_TextChanged);
            // 
            // GreenValueTextBox
            // 
            this.GreenValueTextBox.Location = new System.Drawing.Point(117, 54);
            this.GreenValueTextBox.Name = "GreenValueTextBox";
            this.GreenValueTextBox.Size = new System.Drawing.Size(100, 20);
            this.GreenValueTextBox.TabIndex = 4;
            this.GreenValueTextBox.TextChanged += new System.EventHandler(this.GreenValueTextBox_TextChanged);
            // 
            // BlueValueTextBox
            // 
            this.BlueValueTextBox.Location = new System.Drawing.Point(117, 86);
            this.BlueValueTextBox.Name = "BlueValueTextBox";
            this.BlueValueTextBox.Size = new System.Drawing.Size(100, 20);
            this.BlueValueTextBox.TabIndex = 5;
            this.BlueValueTextBox.TextChanged += new System.EventHandler(this.BlueValueTextBox_TextChanged);
            // 
            // AmbientLightOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 134);
            this.Controls.Add(this.BlueValueTextBox);
            this.Controls.Add(this.GreenValueTextBox);
            this.Controls.Add(this.RedValueTexBox);
            this.Controls.Add(this.GreenLabel);
            this.Controls.Add(this.BlueLabel);
            this.Controls.Add(this.RedLabel);
            this.Name = "AmbientLightOptionsForm";
            this.Text = "AmbientLightOptionsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label RedLabel;
        private System.Windows.Forms.Label BlueLabel;
        private System.Windows.Forms.Label GreenLabel;
        private System.Windows.Forms.TextBox RedValueTexBox;
        private System.Windows.Forms.TextBox GreenValueTextBox;
        private System.Windows.Forms.TextBox BlueValueTextBox;
    }
}