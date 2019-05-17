using SL.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;

namespace SLWizard
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string language = new IniFiles("Settings.ini").ReadString("Misc", "Language", "zh-CN");
            if (language != "zh-CN")
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language); 
            }
        }
    }
}
