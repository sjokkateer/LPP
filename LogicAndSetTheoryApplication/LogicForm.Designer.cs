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
            this.propositionResultLb = new System.Windows.Forms.Label();
            this.uniqueVariablesLb = new System.Windows.Forms.Label();
            this.viewTreeBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // propositionTbx
            // 
            this.propositionTbx.Location = new System.Drawing.Point(99, 12);
            this.propositionTbx.Name = "propositionTbx";
            this.propositionTbx.Size = new System.Drawing.Size(347, 20);
            this.propositionTbx.TabIndex = 0;
            // 
            // parseBtn
            // 
            this.parseBtn.Location = new System.Drawing.Point(12, 12);
            this.parseBtn.Name = "parseBtn";
            this.parseBtn.Size = new System.Drawing.Size(75, 23);
            this.parseBtn.TabIndex = 1;
            this.parseBtn.Text = "Parse";
            this.parseBtn.UseVisualStyleBackColor = true;
            this.parseBtn.Click += new System.EventHandler(this.parseBtn_Click);
            // 
            // propositionResultLb
            // 
            this.propositionResultLb.AutoSize = true;
            this.propositionResultLb.Location = new System.Drawing.Point(22, 53);
            this.propositionResultLb.Name = "propositionResultLb";
            this.propositionResultLb.Size = new System.Drawing.Size(35, 13);
            this.propositionResultLb.TabIndex = 2;
            this.propositionResultLb.Text = "label1";
            // 
            // uniqueVariablesLb
            // 
            this.uniqueVariablesLb.AutoSize = true;
            this.uniqueVariablesLb.Location = new System.Drawing.Point(22, 77);
            this.uniqueVariablesLb.Name = "uniqueVariablesLb";
            this.uniqueVariablesLb.Size = new System.Drawing.Size(90, 13);
            this.uniqueVariablesLb.TabIndex = 3;
            this.uniqueVariablesLb.Text = "Unique Variables:";
            // 
            // viewTreeBtn
            // 
            this.viewTreeBtn.Location = new System.Drawing.Point(452, 12);
            this.viewTreeBtn.Name = "viewTreeBtn";
            this.viewTreeBtn.Size = new System.Drawing.Size(75, 23);
            this.viewTreeBtn.TabIndex = 4;
            this.viewTreeBtn.Text = "View Tree";
            this.viewTreeBtn.UseVisualStyleBackColor = true;
            this.viewTreeBtn.Click += new System.EventHandler(this.viewTreeBtn_Click);
            // 
            // LogicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.viewTreeBtn);
            this.Controls.Add(this.uniqueVariablesLb);
            this.Controls.Add(this.propositionResultLb);
            this.Controls.Add(this.parseBtn);
            this.Controls.Add(this.propositionTbx);
            this.Name = "LogicForm";
            this.Text = "Logic Application";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox propositionTbx;
        private System.Windows.Forms.Button parseBtn;
        private System.Windows.Forms.Label propositionResultLb;
        private System.Windows.Forms.Label uniqueVariablesLb;
        private System.Windows.Forms.Button viewTreeBtn;
    }
}

