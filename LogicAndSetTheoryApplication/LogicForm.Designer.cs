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
            this.truthTableLbx = new System.Windows.Forms.ListBox();
            this.hashCodeTbx = new System.Windows.Forms.TextBox();
            this.uniqueVariablesTbx = new System.Windows.Forms.TextBox();
            this.hashCodeLb = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // propositionTbx
            // 
            this.propositionTbx.Location = new System.Drawing.Point(108, 12);
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
            // infixTbxLb
            // 
            this.infixTbxLb.AutoSize = true;
            this.infixTbxLb.Location = new System.Drawing.Point(12, 41);
            this.infixTbxLb.Name = "infixTbxLb";
            this.infixTbxLb.Size = new System.Drawing.Size(85, 13);
            this.infixTbxLb.TabIndex = 2;
            this.infixTbxLb.Text = "Expression infix: ";
            // 
            // uniqueVariablesLb
            // 
            this.uniqueVariablesLb.AutoSize = true;
            this.uniqueVariablesLb.Location = new System.Drawing.Point(12, 67);
            this.uniqueVariablesLb.Name = "uniqueVariablesLb";
            this.uniqueVariablesLb.Size = new System.Drawing.Size(90, 13);
            this.uniqueVariablesLb.TabIndex = 3;
            this.uniqueVariablesLb.Text = "Unique Variables:";
            // 
            // viewTreeBtn
            // 
            this.viewTreeBtn.Location = new System.Drawing.Point(470, 10);
            this.viewTreeBtn.Name = "viewTreeBtn";
            this.viewTreeBtn.Size = new System.Drawing.Size(75, 23);
            this.viewTreeBtn.TabIndex = 4;
            this.viewTreeBtn.Text = "View Tree";
            this.viewTreeBtn.UseVisualStyleBackColor = true;
            this.viewTreeBtn.Click += new System.EventHandler(this.viewTreeBtn_Click);
            // 
            // infixTbx
            // 
            this.infixTbx.Location = new System.Drawing.Point(108, 38);
            this.infixTbx.Name = "infixTbx";
            this.infixTbx.Size = new System.Drawing.Size(347, 20);
            this.infixTbx.TabIndex = 5;
            // 
            // truthTableLbx
            // 
            this.truthTableLbx.FormattingEnabled = true;
            this.truthTableLbx.Location = new System.Drawing.Point(5, 2);
            this.truthTableLbx.Margin = new System.Windows.Forms.Padding(2);
            this.truthTableLbx.Name = "truthTableLbx";
            this.truthTableLbx.Size = new System.Drawing.Size(322, 342);
            this.truthTableLbx.TabIndex = 6;
            // 
            // hashCodeTbx
            // 
            this.hashCodeTbx.Location = new System.Drawing.Point(252, 64);
            this.hashCodeTbx.Name = "hashCodeTbx";
            this.hashCodeTbx.ReadOnly = true;
            this.hashCodeTbx.Size = new System.Drawing.Size(100, 20);
            this.hashCodeTbx.TabIndex = 7;
            // 
            // uniqueVariablesTbx
            // 
            this.uniqueVariablesTbx.Location = new System.Drawing.Point(108, 64);
            this.uniqueVariablesTbx.Name = "uniqueVariablesTbx";
            this.uniqueVariablesTbx.ReadOnly = true;
            this.uniqueVariablesTbx.Size = new System.Drawing.Size(100, 20);
            this.uniqueVariablesTbx.TabIndex = 8;
            // 
            // hashCodeLb
            // 
            this.hashCodeLb.AutoSize = true;
            this.hashCodeLb.Location = new System.Drawing.Point(214, 67);
            this.hashCodeLb.Name = "hashCodeLb";
            this.hashCodeLb.Size = new System.Drawing.Size(35, 13);
            this.hashCodeLb.TabIndex = 9;
            this.hashCodeLb.Text = "Hash:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 171);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(340, 377);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.truthTableLbx);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(332, 351);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(332, 351);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // LogicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 682);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.hashCodeLb);
            this.Controls.Add(this.uniqueVariablesTbx);
            this.Controls.Add(this.hashCodeTbx);
            this.Controls.Add(this.infixTbx);
            this.Controls.Add(this.viewTreeBtn);
            this.Controls.Add(this.uniqueVariablesLb);
            this.Controls.Add(this.infixTbxLb);
            this.Controls.Add(this.parseBtn);
            this.Controls.Add(this.propositionTbx);
            this.Name = "LogicForm";
            this.Text = "Logic Application";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
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
        private System.Windows.Forms.ListBox truthTableLbx;
        private System.Windows.Forms.TextBox hashCodeTbx;
        private System.Windows.Forms.TextBox uniqueVariablesTbx;
        private System.Windows.Forms.Label hashCodeLb;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}

