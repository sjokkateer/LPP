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
            this.disjunctiveFormTbx = new System.Windows.Forms.TextBox();
            this.disjunctiveFormLb = new System.Windows.Forms.Label();
            this.hashesListBox = new System.Windows.Forms.ListBox();
            this.simplifiedDisjunctiveFormTbx = new System.Windows.Forms.TextBox();
            this.hashCodesListbox = new System.Windows.Forms.ListBox();
            this.simplifiedDisjunctiveFormLb = new System.Windows.Forms.Label();
            this.nandifiedTbx = new System.Windows.Forms.TextBox();
            this.nandifiedLb = new System.Windows.Forms.Label();
            this.hashCodeValidationTbx = new System.Windows.Forms.TextBox();
            this.truthTableTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.simplifiedTruthTableTab.SuspendLayout();
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
            // truthTableTab
            // 
            this.truthTableTab.Controls.Add(this.tabPage1);
            this.truthTableTab.Controls.Add(this.simplifiedTruthTableTab);
            this.truthTableTab.Location = new System.Drawing.Point(12, 171);
            this.truthTableTab.Name = "truthTableTab";
            this.truthTableTab.SelectedIndex = 0;
            this.truthTableTab.Size = new System.Drawing.Size(340, 377);
            this.truthTableTab.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.truthTableLbx);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(332, 351);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Truth Table";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // simplifiedTruthTableTab
            // 
            this.simplifiedTruthTableTab.Controls.Add(this.simplifiedTruthTableLbx);
            this.simplifiedTruthTableTab.Location = new System.Drawing.Point(4, 22);
            this.simplifiedTruthTableTab.Name = "simplifiedTruthTableTab";
            this.simplifiedTruthTableTab.Padding = new System.Windows.Forms.Padding(3);
            this.simplifiedTruthTableTab.Size = new System.Drawing.Size(332, 351);
            this.simplifiedTruthTableTab.TabIndex = 1;
            this.simplifiedTruthTableTab.Text = "Simplified Table";
            this.simplifiedTruthTableTab.UseVisualStyleBackColor = true;
            // 
            // simplifiedTruthTableLbx
            // 
            this.simplifiedTruthTableLbx.FormattingEnabled = true;
            this.simplifiedTruthTableLbx.Location = new System.Drawing.Point(5, 2);
            this.simplifiedTruthTableLbx.Margin = new System.Windows.Forms.Padding(2);
            this.simplifiedTruthTableLbx.Name = "simplifiedTruthTableLbx";
            this.simplifiedTruthTableLbx.Size = new System.Drawing.Size(322, 342);
            this.simplifiedTruthTableLbx.TabIndex = 7;
            // 
            // disjunctiveFormTbx
            // 
            this.disjunctiveFormTbx.Location = new System.Drawing.Point(108, 90);
            this.disjunctiveFormTbx.Name = "disjunctiveFormTbx";
            this.disjunctiveFormTbx.Size = new System.Drawing.Size(347, 20);
            this.disjunctiveFormTbx.TabIndex = 12;
            // 
            // disjunctiveFormLb
            // 
            this.disjunctiveFormLb.AutoSize = true;
            this.disjunctiveFormLb.Location = new System.Drawing.Point(12, 93);
            this.disjunctiveFormLb.Name = "disjunctiveFormLb";
            this.disjunctiveFormLb.Size = new System.Drawing.Size(98, 13);
            this.disjunctiveFormLb.TabIndex = 11;
            this.disjunctiveFormLb.Text = "Disjunctive Normal:";
            // 
            // hashesListBox
            // 
            this.hashesListBox.FormattingEnabled = true;
            this.hashesListBox.Location = new System.Drawing.Point(353, 195);
            this.hashesListBox.Margin = new System.Windows.Forms.Padding(2);
            this.hashesListBox.Name = "hashesListBox";
            this.hashesListBox.Size = new System.Drawing.Size(158, 342);
            this.hashesListBox.TabIndex = 13;
            // 
            // simplifiedDisjunctiveFormTbx
            // 
            this.simplifiedDisjunctiveFormTbx.Location = new System.Drawing.Point(108, 116);
            this.simplifiedDisjunctiveFormTbx.Name = "simplifiedDisjunctiveFormTbx";
            this.simplifiedDisjunctiveFormTbx.Size = new System.Drawing.Size(347, 20);
            this.simplifiedDisjunctiveFormTbx.TabIndex = 14;
            // 
            // hashCodesListbox
            // 
            this.hashCodesListbox.FormattingEnabled = true;
            this.hashCodesListbox.Location = new System.Drawing.Point(515, 195);
            this.hashCodesListbox.Margin = new System.Windows.Forms.Padding(2);
            this.hashCodesListbox.Name = "hashCodesListbox";
            this.hashCodesListbox.Size = new System.Drawing.Size(209, 342);
            this.hashCodesListbox.TabIndex = 15;
            // 
            // simplifiedDisjunctiveFormLb
            // 
            this.simplifiedDisjunctiveFormLb.AutoSize = true;
            this.simplifiedDisjunctiveFormLb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simplifiedDisjunctiveFormLb.Location = new System.Drawing.Point(12, 119);
            this.simplifiedDisjunctiveFormLb.Name = "simplifiedDisjunctiveFormLb";
            this.simplifiedDisjunctiveFormLb.Size = new System.Drawing.Size(52, 13);
            this.simplifiedDisjunctiveFormLb.TabIndex = 16;
            this.simplifiedDisjunctiveFormLb.Text = "simplified:";
            // 
            // nandifiedTbx
            // 
            this.nandifiedTbx.Location = new System.Drawing.Point(108, 145);
            this.nandifiedTbx.Name = "nandifiedTbx";
            this.nandifiedTbx.Size = new System.Drawing.Size(347, 20);
            this.nandifiedTbx.TabIndex = 18;
            // 
            // nandifiedLb
            // 
            this.nandifiedLb.AutoSize = true;
            this.nandifiedLb.Location = new System.Drawing.Point(12, 148);
            this.nandifiedLb.Name = "nandifiedLb";
            this.nandifiedLb.Size = new System.Drawing.Size(41, 13);
            this.nandifiedLb.TabIndex = 17;
            this.nandifiedLb.Text = "NAND:";
            // 
            // hashCodeValidationTbx
            // 
            this.hashCodeValidationTbx.Location = new System.Drawing.Point(515, 170);
            this.hashCodeValidationTbx.Name = "hashCodeValidationTbx";
            this.hashCodeValidationTbx.ReadOnly = true;
            this.hashCodeValidationTbx.Size = new System.Drawing.Size(209, 20);
            this.hashCodeValidationTbx.TabIndex = 19;
            // 
            // LogicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 552);
            this.Controls.Add(this.hashCodeValidationTbx);
            this.Controls.Add(this.nandifiedTbx);
            this.Controls.Add(this.nandifiedLb);
            this.Controls.Add(this.simplifiedDisjunctiveFormLb);
            this.Controls.Add(this.hashCodesListbox);
            this.Controls.Add(this.simplifiedDisjunctiveFormTbx);
            this.Controls.Add(this.hashesListBox);
            this.Controls.Add(this.disjunctiveFormTbx);
            this.Controls.Add(this.disjunctiveFormLb);
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
        private System.Windows.Forms.TextBox disjunctiveFormTbx;
        private System.Windows.Forms.Label disjunctiveFormLb;
        private System.Windows.Forms.ListBox hashesListBox;
        private System.Windows.Forms.TextBox simplifiedDisjunctiveFormTbx;
        private System.Windows.Forms.ListBox hashCodesListbox;
        private System.Windows.Forms.Label simplifiedDisjunctiveFormLb;
        private System.Windows.Forms.TextBox nandifiedTbx;
        private System.Windows.Forms.Label nandifiedLb;
        private System.Windows.Forms.TextBox hashCodeValidationTbx;
    }
}

