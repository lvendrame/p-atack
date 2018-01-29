namespace MouseKeyboardStateTest
{
  partial class MouseKeyboardStateTest
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
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this._MousePosLabel = new System.Windows.Forms.Label();
      this._MouseButtonsLabel = new System.Windows.Forms.Label();
      this._KeysPressedLabel = new System.Windows.Forms.Label();
      this._CheckBtn = new System.Windows.Forms.Button();
      this.label4 = new System.Windows.Forms.Label();
      this._ModifierKeysLabel = new System.Windows.Forms.Label();
      this._KeysToggledLabel = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(106, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Current Mouse Point:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(13, 41);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(118, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Current Mouse Buttons:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(13, 95);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(111, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Current Keys Pressed:";
      // 
      // _MousePosLabel
      // 
      this._MousePosLabel.AutoSize = true;
      this._MousePosLabel.Location = new System.Drawing.Point(137, 13);
      this._MousePosLabel.Name = "_MousePosLabel";
      this._MousePosLabel.Size = new System.Drawing.Size(0, 13);
      this._MousePosLabel.TabIndex = 3;
      // 
      // _MouseButtonsLabel
      // 
      this._MouseButtonsLabel.AutoSize = true;
      this._MouseButtonsLabel.Location = new System.Drawing.Point(137, 41);
      this._MouseButtonsLabel.Name = "_MouseButtonsLabel";
      this._MouseButtonsLabel.Size = new System.Drawing.Size(0, 13);
      this._MouseButtonsLabel.TabIndex = 4;
      // 
      // _KeysPressedLabel
      // 
      this._KeysPressedLabel.AutoSize = true;
      this._KeysPressedLabel.Location = new System.Drawing.Point(137, 95);
      this._KeysPressedLabel.Name = "_KeysPressedLabel";
      this._KeysPressedLabel.Size = new System.Drawing.Size(0, 13);
      this._KeysPressedLabel.TabIndex = 5;
      // 
      // _CheckBtn
      // 
      this._CheckBtn.Location = new System.Drawing.Point(103, 156);
      this._CheckBtn.Name = "_CheckBtn";
      this._CheckBtn.Size = new System.Drawing.Size(75, 23);
      this._CheckBtn.TabIndex = 6;
      this._CheckBtn.Text = "Check Now";
      this._CheckBtn.UseVisualStyleBackColor = true;
      this._CheckBtn.Click += new System.EventHandler(this.CheckBtn_Click);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(13, 69);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(110, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "Current Modifier Keys:";
      // 
      // _ModifierKeysLabel
      // 
      this._ModifierKeysLabel.AutoSize = true;
      this._ModifierKeysLabel.Location = new System.Drawing.Point(137, 69);
      this._ModifierKeysLabel.Name = "_ModifierKeysLabel";
      this._ModifierKeysLabel.Size = new System.Drawing.Size(0, 13);
      this._ModifierKeysLabel.TabIndex = 8;
      // 
      // _KeysToggledLabel
      // 
      this._KeysToggledLabel.AutoSize = true;
      this._KeysToggledLabel.Location = new System.Drawing.Point(137, 120);
      this._KeysToggledLabel.Name = "_KeysToggledLabel";
      this._KeysToggledLabel.Size = new System.Drawing.Size(0, 13);
      this._KeysToggledLabel.TabIndex = 10;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(13, 120);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(112, 13);
      this.label6.TabIndex = 9;
      this.label6.Text = "Current Keys Toggled:";
      // 
      // MouseKeyboardStateTest
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(292, 191);
      this.Controls.Add(this._KeysToggledLabel);
      this.Controls.Add(this.label6);
      this.Controls.Add(this._ModifierKeysLabel);
      this.Controls.Add(this.label4);
      this.Controls.Add(this._CheckBtn);
      this.Controls.Add(this._KeysPressedLabel);
      this.Controls.Add(this._MouseButtonsLabel);
      this.Controls.Add(this._MousePosLabel);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Name = "MouseKeyboardStateTest";
      this.Text = "Mouse And Keyboard State Test";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label _MousePosLabel;
    private System.Windows.Forms.Label _MouseButtonsLabel;
    private System.Windows.Forms.Label _KeysPressedLabel;
    private System.Windows.Forms.Button _CheckBtn;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label _ModifierKeysLabel;
    private System.Windows.Forms.Label _KeysToggledLabel;
    private System.Windows.Forms.Label label6;
  }
}

