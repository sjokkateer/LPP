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
            this.truthTableTab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.simplifiedTruthTableTab = new System.Windows.Forms.TabPage();
            this.simplifiedTruthTableLbx = new System.Windows.Forms.ListBox();
            this.truthTableTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.simplifiedTruthTableTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // propositionTbx
            // 
            this.propositionTbx.Location = new System.Drawing.Point(216, 23);
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
            this.uniqueVariablesLb.Location = new System.Drawing.Point(24, 129);
            this.uniqueVariablesLb.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.uniqueVariablesLb.Name = "uniqueVariablesLb";
            this.uniqueVariablesLb.Size = new System.Drawing.Size(182, 25);
            this.uniqueVariablesLb.TabIndex = 3;
            this.uniqueVariablesLb.Text = "Unique Variables:";
            // 
            // viewTreeBtn
            // 
            this.viewTreeBtn.Location = new System.Drawing.Point(940, 19);
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
            this.infixTbx.Location = new System.Drawing.Point(216, 73);
            this.infixTbx.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.infixTbx.Name = "infixTbx";
            this.infixTbx.Size = new System.Drawing.Size(690, 31);
            this.infixTbx.TabIndex = 5;
            // 
            // truthTableLbx
            // 
            this.truthTableLbx.FormattingEnabled = true;
            this.truthTableLbx.ItemHeight = 25;
            this.truthTableLbx.Location = new System.Drawing.Point(10, 4);
            this.truthTableLbx.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.truthTableLbx.Name = "truthTableLbx";
            this.truthTableLbx.Size = new System.Drawing.Size(640, 654);
            this.truthTableLbx.TabIndex = 6;
            // 
            // hashCodeTbx
            // 
            this.hashCodeTbx.Location = new System.Drawing.Point(504, 123);
            this.hashCodeTbx.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.hashCodeTbx.Name = "hashCodeTbx";
            this.hashCodeTbx.ReadOnly = true;
            this.hashCodeTbx.Size = new System.Drawing.Size(196, 31);
            this.hashCodeTbx.TabIndex = 7;
            // 
            // uniqueVariablesTbx
            // 
            this.uniqueVariablesTbx.Location = new System.Drawing.Point(216, 123);
            this.uniqueVariablesTbx.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.uniqueVariablesTbx.Name = "uniqueVariablesTbx";
            this.uniqueVariablesTbx.ReadOnly = true;
            this.uniqueVariablesTbx.Size = new System.Drawing.Size(196, 31);
            this.uniqueVariablesTbx.TabIndex = 8;
            // 
            // hashCodeLb
            // 
            this.hashCodeLb.AutoSize = true;
            this.hashCodeLb.Location = new System.Drawing.Point(428, 129);
            this.hashCodeLb.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.hashCodeLb.Name = "hashCodeLb";
            this.hashCodeLb.Size = new System.Drawing.Size(68, 25);
            this.hashCodeLb.TabIndex = 9;
            this.hashCodeLb.Text = "Hash:";
            // 
            // truthTableTab
            // 
            this.truthTableTab.Controls.Add(this.tabPage1);
            this.truthTableTab.Controls.Add(this.simplifiedTruthTableTab);
            this.truthTableTab.Location = new System.Drawing.Point(24, 329);
            this.truthTableTab.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.truthTableTab.Name = "truthTableTab";
            this.truthTableTab.SelectedIndex = 0;
            this.truthTableTab.Size = new System.Drawing.Size(680, 725);
            this.truthTableTab.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.truthTableLbx);
            this.tabPage1.Location = new System.Drawing.Point(8, 39);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage1.Size = new System.Drawing.Size(664, 678);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Truth Table";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // simplifiedTruthTableTab
            // 
            this.simplifiedTruthTableTab.Controls.Add(this.simplifiedTruthTableLbx);
            this.simplifiedTruthTableTab.Location = new System.Drawing.Point(8, 39);
            this.simplifiedTruthTableTab.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.simplifiedTruthTableTab.Name = "simplifiedTruthTableTab";
            this.simplifiedTruthTableTab.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.simplifiedTruthTableTab.Size = new System.Drawing.Size(664, 678);
            this.simplifiedTruthTableTab.TabIndex = 1;
            this.simplifiedTruthTableTab.Text = "Simplified Table";
            this.simplifiedTruthTableTab.UseVisualStyleBackColor = true;
            // 
            // simplifiedTruthTableLbx
            // 
            this.simplifiedTruthTableLbx.FormattingEnabled = true;
            this.simplifiedTruthTableLbx.ItemHeight = 25;
            this.simplifiedTruthTableLbx.Location = new System.Drawing.Point(12, 12);
            this.simplifiedTruthTableLbx.Margin = new System.Windows.Forms.Padding(4);
            this.simplifiedTruthTableLbx.Name = "simplifiedTruthTableLbx";
            this.simplifiedTruthTableLbx.Size = new System.Drawing.Size(640, 654);
            this.simplifiedTruthTableLbx.TabIndex = 7;
            // 
            // LogicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1928, 1312);
            this.Controls.Add(this.truthTableTab);
            this.Controls.Add(this.hashCodeLb);
            this.Controls.Add(this.uniqueVariablesTbx);
            this.Controls.Add(this.hashCodeTbx);
            this.Controls.Add(this.infixTbx);
            this.Controls.Add(this.viewTreeBtn);
            this.Controls.Add(this.uniqueVariablesLb);
            this.Controls.Add(this.infixTbxLb);
            this.Controls.Add(this.parseBtn);
            this.Controls.Add(this.propositionTbx);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "LogicForm";
            this.Text = "Logic Application";
            this.truthTableTab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.simplifiedTruthTableTab.ResumeLayout(false);
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
        private System.Windows.Forms.TabControl truthTableTab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage simplifiedTruthTableTab;
        private System.Windows.Forms.ListBox simplifiedTruthTableLbx;
    }
}

