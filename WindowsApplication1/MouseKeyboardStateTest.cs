using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace MouseKeyboardStateTest
{
  public partial class MouseKeyboardStateTest : Form
  {
    public MouseKeyboardStateTest()
    {
      InitializeComponent();
    }

    private void CheckBtn_Click(object sender, EventArgs e)
    {
      _MousePosLabel.Text = Control.MousePosition.X + ", " + Control.MousePosition.Y;
          
      _MouseButtonsLabel.Text = Control.MouseButtons.ToString();

      _ModifierKeysLabel.Text = Control.ModifierKeys.ToString();

      StringBuilder text = new StringBuilder();
      foreach (Keys k in Enum.GetValues(typeof(Keys)))
      {
        if (Keyboard.IsKeyDown(k))
        {
          if (text.Length != 0)
            text.Append(", ");

          text.Append(k);
        }
      }
      _KeysPressedLabel.Text = text.ToString();

      text = new StringBuilder();
      foreach (Keys k in Enum.GetValues(typeof(Keys)))
      {
        if (Keyboard.IsKeyToggled(k))
        {
          if (text.Length != 0)
            text.Append(", ");

          text.Append(k);
        }
      }
      _KeysToggledLabel.Text = text.ToString();
    }
  }
}
