namespace LogicAndSetTheoryApplication
{
    partial class LogicForm
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
            this.propositionTbx = new System.Windows.Forms.TextBox();
            this.parseBtn = new System.Windows.Forms.Button();
            this.infixTbxLb = new System.Windows.Forms.Label();
            this.uniqueVariablesLb = new System.Windows.Forms.Label();
            this.viewTreeBtn = new System.Windows.Forms.Button();
            this.infixTbx = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // propositionTbx
            // 
            this.propositionTbx.Location = new System.Drawing.Point(198, 23);
            this.propositionTbx.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.propositionTbx.Name = "propositionTbx";
            this.propositionTbx.Size = new System.Drawing.Size(690, 31);
            this.propositionTbx.TabIndex = 0;
            // 
            // parseBtn
            // 
            this.parseBtn.Location = new System.Drawing.Point(24, 23);
            this.parseBtn.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.parseBtn.Name = "parseBtn";
            this.parseBtn.Size = new System.Drawing.Size(150, 44);
            this.parseBtn.TabIndex = 1;
            this.parseBtn.Text = "Parse";
            this.parseBtn.UseVisualStyleBackColor = true;
            this.parseBtn.Click += new System.EventHandler(this.parseBtn_Click);
            // 
            // infixTbxLb
            // 
            this.infixTbxLb.AutoSize = true;
            this.infixTbxLb.Location = new System.Drawing.Point(24, 79);
            this.infixTbxLb.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.infixTbxLb.Name = "infixTbxLb";
            this.infixTbxLb.Size = new System.Drawing.Size(176, 25);
            this.infixTbxLb.TabIndex = 2;
            this.infixTbxLb.Text = "Expression infix: ";
            // 
            // uniqueVariablesLb
            // 
            this.uniqueVariablesLb.AutoSize = true;
            this.uniqueVariablesLb.Location = new System.Drawing.Point(24, 137);
            this.uniqueVariablesLb.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.uniqueVariablesLb.Name = "uniqueVariablesLb";
            this.uniqueVariablesLb.Size = new System.Drawing.Size(182, 25);
            this.uniqueVariablesLb.TabIndex = 3;
            this.uniqueVariablesLb.Text = "Unique Variables:";
            // 
            // viewTreeBtn
            // 
            this.viewTreeBtn.Location = new System.Drawing.Point(904, 23);
            this.viewTreeBtn.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.viewTreeBtn.Name = "viewTreeBtn";
            this.viewTreeBtn.Size = new System.Drawing.Size(150, 44);
            this.viewTreeBtn.TabIndex = 4;
            this.viewTreeBtn.Text = "View Tree";
            this.viewTreeBtn.UseVisualStyleBackColor = true;
            this.viewTreeBtn.Click += new System.EventHandler(this.viewTreeBtn_Click);
            // 
            // infixTbx
            // 
            this.infixTbx.Location = new System.Drawing.Point(198, 73);
            this.infixTbx.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.infixTbx.Name = "infixTbx";
            this.infixTbx.Size = new System.Drawing.Size(690, 31);
            this.infixTbx.TabIndex = 5;
            // 
            // LogicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1930, 989);
            this.Controls.Add(this.infixTbx);
            this.Controls.Add(this.viewTreeBtn);
            this.Controls.Add(this.uniqueVariablesLb);
            this.Controls.Add(this.infixTbxLb);
            this.Controls.Add(this.parseBtn);
            this.Controls.Add(this.propositionTbx);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "LogicForm";
            this.Text = "Logic Application";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox propositionTbx;
        private System.Windows.Forms.Button parseBtn;
        private System.Windows.Forms.Label infixTbxLb;
        private System.Windows.Forms.Label uniqueVariablesLb;
        private System.Windows.Forms.Button viewTreeBtn;
        private System.Windows.Forms.TextBox infixTbx;
    }
}

