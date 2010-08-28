﻿namespace GoreUpdater.Manager
{
    partial class AddServerForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblTypeHelp = new System.Windows.Forms.Label();
            this.lblHostHelp = new System.Windows.Forms.Label();
            this.lblUserHelp = new System.Windows.Forms.Label();
            this.lblPasswordHelp = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection type:";
            // 
            // cmbType
            // 
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(105, 12);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(129, 21);
            this.cmbType.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Host:";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(74, 39);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(160, 20);
            this.txtHost.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "User:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Password:";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(74, 65);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(160, 20);
            this.txtUser.TabIndex = 6;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(74, 91);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(160, 20);
            this.txtPassword.TabIndex = 7;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(100, 117);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(64, 23);
            this.btnTest.TabIndex = 8;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(170, 117);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(64, 23);
            this.btnCreate.TabIndex = 9;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(30, 117);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblTypeHelp
            // 
            this.lblTypeHelp.AutoSize = true;
            this.lblTypeHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblTypeHelp.Location = new System.Drawing.Point(240, 12);
            this.lblTypeHelp.Name = "lblTypeHelp";
            this.lblTypeHelp.Size = new System.Drawing.Size(13, 13);
            this.lblTypeHelp.TabIndex = 11;
            this.lblTypeHelp.Text = "?";
            this.lblTypeHelp.Click += new System.EventHandler(this.lblTypeHelp_Click);
            // 
            // lblHostHelp
            // 
            this.lblHostHelp.AutoSize = true;
            this.lblHostHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblHostHelp.Location = new System.Drawing.Point(240, 39);
            this.lblHostHelp.Name = "lblHostHelp";
            this.lblHostHelp.Size = new System.Drawing.Size(13, 13);
            this.lblHostHelp.TabIndex = 12;
            this.lblHostHelp.Text = "?";
            this.lblHostHelp.Click += new System.EventHandler(this.lblHostHelp_Click);
            // 
            // lblUserHelp
            // 
            this.lblUserHelp.AutoSize = true;
            this.lblUserHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblUserHelp.Location = new System.Drawing.Point(240, 65);
            this.lblUserHelp.Name = "lblUserHelp";
            this.lblUserHelp.Size = new System.Drawing.Size(13, 13);
            this.lblUserHelp.TabIndex = 13;
            this.lblUserHelp.Text = "?";
            this.lblUserHelp.Click += new System.EventHandler(this.lblUserHelp_Click);
            // 
            // lblPasswordHelp
            // 
            this.lblPasswordHelp.AutoSize = true;
            this.lblPasswordHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblPasswordHelp.Location = new System.Drawing.Point(240, 91);
            this.lblPasswordHelp.Name = "lblPasswordHelp";
            this.lblPasswordHelp.Size = new System.Drawing.Size(13, 13);
            this.lblPasswordHelp.TabIndex = 14;
            this.lblPasswordHelp.Text = "?";
            this.lblPasswordHelp.Click += new System.EventHandler(this.lblPasswordHelp_Click);
            // 
            // AddServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 153);
            this.Controls.Add(this.lblPasswordHelp);
            this.Controls.Add(this.lblUserHelp);
            this.Controls.Add(this.lblHostHelp);
            this.Controls.Add(this.lblTypeHelp);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AddServerForm";
            this.Text = "Add Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblTypeHelp;
        private System.Windows.Forms.Label lblHostHelp;
        private System.Windows.Forms.Label lblUserHelp;
        private System.Windows.Forms.Label lblPasswordHelp;
    }
}