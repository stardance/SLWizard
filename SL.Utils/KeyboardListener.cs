using SL.Utils.Message;
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
            //backup action
            if (e.KeyValue == (int)Keys.A && (int)Control.ModifierKeys == (int)Keys.Alt)
            {
                EventAggregatorHost.Aggregator.SendMessage<BackupMessage>(default(BackupMessage));
            }
        
            //restore action
            if(e.KeyValue == (int)Keys.Z && (int)Control.ModifierKeys == (int)Keys.Alt)
            {
                EventAggregatorHost.Aggregator.SendMessage<RestoreMessage>(default(RestoreMessage));
            }

        }

        private void K_hook_KeyPressEvent(object sender, KeyPressEventArgs e)
        {
           
        }


    }
}
