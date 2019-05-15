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

        private int backupMKey = 0;

        private int backupKey = 0;

        private int restoreMKey = 0;

        private int restoreKey = 0;

        public KeyboardListener()
        {
            k_hook = new KeyboardHook();
            //k_hook.KeyPressEvent += K_hook_KeyPressEvent;
            k_hook.KeyDownEvent += new System.Windows.Forms.KeyEventHandler(hook_KeyDown);

            var inireader = new IniFiles("Settings.ini");
            backupMKey = Convert.ToInt32(inireader.ReadString("HotKey", "BackupModifierKey", "0"));
            backupKey = Convert.ToInt32(inireader.ReadString("HotKey", "BackupKey", "0"));
            restoreMKey = Convert.ToInt32(inireader.ReadString("HotKey", "RestoreModifierKey", "0"));
            restoreKey = Convert.ToInt32(inireader.ReadString("HotKey", "RestoreKey", "0"));
            //安装键盘钩子
            k_hook.Start();
        }

        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            //backup action
            if (backupMKey != 0 && backupKey != 0)
            {
                if (e.KeyValue == backupKey && (int)Control.ModifierKeys == backupMKey)
                {
                    EventAggregatorHost.Aggregator.SendMessage<BackupMessage>(default(BackupMessage));
                } 
            }

            //restore action
            if (restoreMKey != 0 && restoreKey != 0)
            {
                if (e.KeyValue == restoreKey && (int)Control.ModifierKeys == restoreMKey)
                {
                    EventAggregatorHost.Aggregator.SendMessage<RestoreMessage>(default(RestoreMessage));
                } 
            }

        }

        private void K_hook_KeyPressEvent(object sender, KeyPressEventArgs e)
        {
           
        }


    }
}
