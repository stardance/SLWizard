using GalaSoft.MvvmLight.Command;
using SL.Utils;
using SL.Utils.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SLWizard.UserControl
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : Window, INotifyPropertyChanged
    {

        private IniFiles inireader;
        private Dictionary<string,int> modifierKeys;

        public Dictionary<string, int> ModifierKeys
        {
            get { return modifierKeys; }
            set
            {
                this.modifierKeys = value;
                RaisePropertyChanged(nameof(ModifierKeys));
            }
        }

        private Dictionary<string, int> keys;

        public Dictionary<string,int> DicKeys
        {
            get{ return keys; }
            set
            {
                this.keys = value;
                RaisePropertyChanged(nameof(DicKeys));

            }
        }

        private KeyValuePair<string,int> selectMKey;


        public KeyValuePair<string, int> BackupMKey
        {
            get { return selectMKey; }
            set
            {
                selectMKey = value;
                RaisePropertyChanged(nameof(BackupMKey));
            }
        }

        private KeyValuePair<string, int> selectedKey;

        public KeyValuePair<string, int> BackupKey

        {
            get { return selectedKey; }
            set
            {
                selectedKey = value;
                RaisePropertyChanged(nameof(BackupKey));

            }
        }


        private KeyValuePair<string, int> storeMKey;

        public KeyValuePair<string, int> RestoreMKey
        {
            get { return storeMKey; }
            set
            {
                storeMKey = value;
                RaisePropertyChanged(nameof(RestoreMKey));
            }
        }

        private KeyValuePair<string, int> storeKey;

        public KeyValuePair<string, int> RestoreKey
        {
            get { return storeKey; }
            set
            {
                storeKey = value;
                RaisePropertyChanged(nameof(RestoreKey));
            }
        }

        private bool autocheckNewItem;

        public bool AutoCheckNewItem
        {
            get { return autocheckNewItem; }
            set { autocheckNewItem = value; }
        }



        public event PropertyChangedEventHandler PropertyChanged;



        public Settings()
        {
            InitializeComponent();
            #region Initialize Dictionary
            modifierKeys = new Dictionary<string, int>();
            modifierKeys.Add(Keys.Alt.ToString(), (int)Keys.Alt);
            modifierKeys.Add(Keys.Shift.ToString(), (int)Keys.Shift);
            modifierKeys.Add(Keys.Control.ToString(), (int)Keys.Control);
            keys = new Dictionary<string, int>();
            keys.Add(Keys.A.ToString(), (int)Keys.A);
            keys.Add(Keys.B.ToString(), (int)Keys.B);
            keys.Add(Keys.C.ToString(), (int)Keys.C);
            keys.Add(Keys.D.ToString(), (int)Keys.D);
            keys.Add(Keys.E.ToString(), (int)Keys.E);
            keys.Add(Keys.F.ToString(), (int)Keys.F);
            keys.Add(Keys.G.ToString(), (int)Keys.G);
            keys.Add(Keys.H.ToString(), (int)Keys.H);
            keys.Add(Keys.I.ToString(), (int)Keys.I);
            keys.Add(Keys.J.ToString(), (int)Keys.J);
            keys.Add(Keys.K.ToString(), (int)Keys.K);
            keys.Add(Keys.L.ToString(), (int)Keys.L);
            keys.Add(Keys.M.ToString(), (int)Keys.M);
            keys.Add(Keys.N.ToString(), (int)Keys.N);
            keys.Add(Keys.O.ToString(), (int)Keys.O);
            keys.Add(Keys.P.ToString(), (int)Keys.P);
            keys.Add(Keys.Q.ToString(), (int)Keys.Q);
            keys.Add(Keys.R.ToString(), (int)Keys.R);
            keys.Add(Keys.S.ToString(), (int)Keys.S);
            keys.Add(Keys.T.ToString(), (int)Keys.T);
            keys.Add(Keys.U.ToString(), (int)Keys.U);
            keys.Add(Keys.V.ToString(), (int)Keys.V);
            keys.Add(Keys.W.ToString(), (int)Keys.W);
            keys.Add(Keys.X.ToString(), (int)Keys.X);
            keys.Add(Keys.Y.ToString(), (int)Keys.Y);
            keys.Add(Keys.Z.ToString(), (int)Keys.Z); 
            #endregion
            this.DataContext = this;
            inireader = new IniFiles("Settings.ini");
            int backupMKey = Convert.ToInt32(inireader.ReadString("HotKey", "BackupModifierKey","0"));
            int backupKey = Convert.ToInt32(inireader.ReadString("HotKey", "BackupKey", "0"));
            int restoreMKey = Convert.ToInt32(inireader.ReadString("HotKey", "RestoreModifierKey", "0"));
            int restoreKey = Convert.ToInt32(inireader.ReadString("HotKey", "RestoreKey", "0"));
            AutoCheckNewItem = inireader.ReadString("Misc", "AutoCheckNewItem", "0") == "1";
            string backupMKeyName = Enum.GetName(typeof(Keys), backupMKey);
            string backupKeyName = Enum.GetName(typeof(Keys), backupKey);
            string restoreMKeyName = Enum.GetName(typeof(Keys), restoreMKey);
            string restoreKeyName = Enum.GetName(typeof(Keys), restoreKey);
            BackupMKey = new KeyValuePair<string, int>(backupMKeyName, backupMKey);
            BackupKey = new KeyValuePair<string, int>(backupKeyName, backupKey);
            RestoreMKey = new KeyValuePair<string, int>(restoreMKeyName,restoreMKey);
            RestoreKey = new KeyValuePair<string, int>(restoreKeyName, restoreKey);
        }

        private void linkDmsite_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
        }


        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }

        private RelayCommand saveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ?? new RelayCommand(()=>
                {
                    inireader.WriteString("HotKey", "BackupModifierKey", BackupMKey.Value.ToString());
                    inireader.WriteString("HotKey", "BackupKey", BackupKey.Value.ToString());
                    inireader.WriteString("HotKey", "RestoreModifierKey", RestoreMKey.Value.ToString());
                    inireader.WriteString("HotKey", "RestoreKey", RestoreKey.Value.ToString());
                    inireader.WriteString("Misc", "AutoCheckNewItem", AutoCheckNewItem ? "1" : "0");
                    EventAggregatorHost.Aggregator.SendMessage<SysMessage>(new SysMessage("保存成功！"));
                });
            }
        }


        private RelayCommand<ComboBoxItem> languageChangedCommand;

        public RelayCommand<ComboBoxItem> LanguageChangedCommand
        {
            get
            {
                return languageChangedCommand ?? new RelayCommand<ComboBoxItem>((p)=>
                {
                    var name = p.Content;
                    switch (name)
                    {
                        case "中文":
                            inireader.WriteString("Misc", "Language", "zh-CN");
                            break;
                        case "English":
                            inireader.WriteString("Misc", "Language", "en-US");
                            break;
                    }
                });
            }
        }


    }
}
