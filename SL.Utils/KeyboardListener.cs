using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SL.Utils
{
    public class KeyboardListener
    {
        KeyboardHook k_hook;

        public KeyboardListener()
        {
            k_hook = new KeyboardHook();
            //k_hook.KeyPressEvent += K_hook_KeyPressEvent;
            k_hook.KeyDownEvent += new System.Windows.Forms.KeyEventHandler(hook_KeyDown);
            //安装键盘钩子
            k_hook.Start();
        }

        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            //判断按下的键（Alt + A） 
            if (e.Control && e.KeyCode == Keys.Z)
            {
                System.Windows.Forms.MessageBox.Show("ddd");
            }
        }

        private void K_hook_KeyPressEvent(object sender, KeyPressEventArgs e)
        {
           
        }


    }
}
