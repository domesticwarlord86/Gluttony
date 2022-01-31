namespace Gluttony
{
    partial class GluttonySettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GluttonySettings));
            this.foodDropBox = new System.Windows.Forms.ComboBox();
            this.foodLabel = new System.Windows.Forms.Label();
            this.spiritBindCheckBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // foodDropBox
            // 
            this.foodDropBox.FormattingEnabled = true;
            resources.ApplyResources(this.foodDropBox, "foodDropBox");
            this.foodDropBox.Name = "foodDropBox";
            this.foodDropBox.SelectedIndexChanged += new System.EventHandler(this.FoodDropBox_SelectedIndexChanged);
            this.foodDropBox.Click += new System.EventHandler(this.FoodDropBox_Click);
            // 
            // foodLabel
            // 
            resources.ApplyResources(this.foodLabel, "foodLabel");
            this.foodLabel.Name = "foodLabel";
            // 
            // spiritBindCheckBox1
            // 
            resources.ApplyResources(this.spiritBindCheckBox1, "spiritBindCheckBox1");
            this.spiritBindCheckBox1.Name = "spiritBindCheckBox1";
            this.spiritBindCheckBox1.UseVisualStyleBackColor = true;
            this.spiritBindCheckBox1.CheckedChanged += new System.EventHandler(this.spiritBindCheckBox1_CheckedChanged);
            // 
            // GluttonySettings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.spiritBindCheckBox1);
            this.Controls.Add(this.foodLabel);
            this.Controls.Add(this.foodDropBox);
            this.Name = "GluttonySettings";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.CheckBox spiritBindCheckBox1;

        #endregion

        private System.Windows.Forms.ComboBox foodDropBox;
        private System.Windows.Forms.Label foodLabel;
    }
}