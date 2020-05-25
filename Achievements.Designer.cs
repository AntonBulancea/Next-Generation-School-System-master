namespace Next_Generation_School_System_by_Anton {
    partial class Achievements {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
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
        private void InitializeComponent() {
            this.test1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.test = new System.Windows.Forms.Label();
            this.mark_test = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // test1
            // 
            this.test1.AutoSize = true;
            this.test1.Font = new System.Drawing.Font("Arial Narrow", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.test1.Location = new System.Drawing.Point(12, 9);
            this.test1.Name = "test1";
            this.test1.Size = new System.Drawing.Size(206, 52);
            this.test1.TabIndex = 0;
            this.test1.Text = "Achivents:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(15, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(773, 378);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(7, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(760, 350);
            this.panel1.TabIndex = 0;
            // 
            // test
            // 
            this.test.AutoSize = true;
            this.test.Font = new System.Drawing.Font("Arial Narrow", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.test.Location = new System.Drawing.Point(224, 37);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(91, 23);
            this.test.TabIndex = 2;
            this.test.Text = "Achivents:";
            this.test.Visible = false;
            // 
            // mark_test
            // 
            this.mark_test.AutoSize = true;
            this.mark_test.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mark_test.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.mark_test.Location = new System.Drawing.Point(321, 34);
            this.mark_test.Name = "mark_test";
            this.mark_test.Size = new System.Drawing.Size(23, 20);
            this.mark_test.TabIndex = 3;
            this.mark_test.Text = "10";
            this.mark_test.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 15.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Crimson;
            this.label1.Location = new System.Drawing.Point(224, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 31);
            this.label1.TabIndex = 4;
            // 
            // Achievements
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mark_test);
            this.Controls.Add(this.test);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.test1);
            this.Name = "Achievements";
            this.Text = "Achievements";
            this.Load += new System.EventHandler(this.Achievements_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label test1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label test;
        private System.Windows.Forms.Label mark_test;
        private System.Windows.Forms.Label label1;
    }
}