namespace TinyCompiler
{
    partial class Form1
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
            this.errorListTextBox = new System.Windows.Forms.TextBox();
            this.codeTextBox = new System.Windows.Forms.TextBox();
            this.Compile = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.errorList = new System.Windows.Forms.Label();
            this.tinyCode = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // errorListTextBox
            // 
            this.errorListTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorListTextBox.Location = new System.Drawing.Point(618, 496);
            this.errorListTextBox.Multiline = true;
            this.errorListTextBox.Name = "errorListTextBox";
            this.errorListTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorListTextBox.Size = new System.Drawing.Size(484, 178);
            this.errorListTextBox.TabIndex = 0;
            // 
            // codeTextBox
            // 
            this.codeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.codeTextBox.Location = new System.Drawing.Point(12, 46);
            this.codeTextBox.Multiline = true;
            this.codeTextBox.Name = "codeTextBox";
            this.codeTextBox.Size = new System.Drawing.Size(376, 411);
            this.codeTextBox.TabIndex = 1;
            // 
            // Compile
            // 
            this.Compile.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Compile.Location = new System.Drawing.Point(116, 546);
            this.Compile.Name = "Compile";
            this.Compile.Size = new System.Drawing.Size(144, 51);
            this.Compile.TabIndex = 2;
            this.Compile.Text = "Compile";
            this.Compile.UseVisualStyleBackColor = true;
            this.Compile.Click += new System.EventHandler(this.Compile_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(417, 46);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(476, 411);
            this.dataGridView1.TabIndex = 3;
            // 
            // errorList
            // 
            this.errorList.AutoSize = true;
            this.errorList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorList.Location = new System.Drawing.Point(614, 468);
            this.errorList.Name = "errorList";
            this.errorList.Size = new System.Drawing.Size(89, 25);
            this.errorList.TabIndex = 4;
            this.errorList.Text = "Error List";
            // 
            // tinyCode
            // 
            this.tinyCode.AutoSize = true;
            this.tinyCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tinyCode.Location = new System.Drawing.Point(12, 9);
            this.tinyCode.Name = "tinyCode";
            this.tinyCode.Size = new System.Drawing.Size(103, 25);
            this.tinyCode.TabIndex = 5;
            this.tinyCode.Text = "Tiny Code";
            // 
            // treeView1
            // 
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.Location = new System.Drawing.Point(916, 46);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(472, 411);
            this.treeView1.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 683);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.tinyCode);
            this.Controls.Add(this.errorList);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.Compile);
            this.Controls.Add(this.codeTextBox);
            this.Controls.Add(this.errorListTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox errorListTextBox;
        private System.Windows.Forms.TextBox codeTextBox;
        private System.Windows.Forms.Button Compile;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label errorList;
        private System.Windows.Forms.Label tinyCode;
        private System.Windows.Forms.TreeView treeView1;
    }
}

