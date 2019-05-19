using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SL.Utils;
using SL.Utils.Entities;
using SL.Utils.Message;
using SLWizard.UserControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SLWizard.ViewModels
{
    public class MainWindowViewModel:ViewModelBase,IListener<SysMessage>,IListener<SaveConfigMessage>,IListener<BackupMessage>,IListener<RestoreMessage>
    {
        public KeyboardListener keyListener { get; set; }

        public ArchiveData Entity { get; set; }

        readonly string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchiveData.xml");

        private string sysMsg;

        public string SysMsg
        {
            get { return sysMsg; }
            set { sysMsg = value;RaisePropertyChanged(nameof(SysMsg)); }
        }

        private ArchiveProject selectedProject;

        public ArchiveProject SelectedProject
        {
            get { return selectedProject; }
            set
            {
                selectedProject = value;
                RaisePropertyChanged("SelectedProject");
            }
        }


        private ArchiveItem selectedItem;

        public ArchiveItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }




        public MainWindowViewModel()
        {
            LoadArchiveConfig();
            EventAggregatorHost.Aggregator.AddListener(this);
            keyListener = new KeyboardListener();
        }

        private void LoadArchiveConfig()
        {
            if (!File.Exists(configPath))
            {
                Entity = new ArchiveData();
                SaveData();
            }
            else
            {
                Entity = XmlHelper.Read<ArchiveData>(configPath);
                Entity.Projects.ForEach(it => it.ObList = new System.Collections.ObjectModel.ObservableCollection<ArchiveItem>(it.Items));
                Entity.ObList = new System.Collections.ObjectModel.ObservableCollection<ArchiveProject>(Entity.Projects);
                if (Entity.Projects.Exists(it => it.IsSelected == true))
                {
                    SelectedProject = Entity.Projects.Single(it => it.IsSelected == true);
                    if (SelectedProject.Items.Exists(it => it.IsSelected == true))
                    {
                        SelectedItem = SelectedProject.Items.Single(it => it.IsSelected == true);
                    }
                }
            }
        }

        public void Handle(SysMessage message)
        {
            SysMsg = message.Msg;
            Task.Factory.StartNew(()=> 
            {
                Thread.Sleep(4 * 1000);
            }).ContinueWith((t)=> 
            {
                SysMsg = "就绪.";
            });
        }

        public void Handle(SaveConfigMessage message)
        {
            SaveData();
        }

        private RelayCommand  addNewProjectCommand;

        public RelayCommand AddNewProjectCommand
        {
           get
           {
                return addNewProjectCommand ?? (new RelayCommand(() =>
                {
                    NewProjectGuide guide = new NewProjectGuide();
                    guide.OnSubmitProject += (p) =>
                    {
                        Entity.ObList.Add(p);
                        string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Archive", p.ProjectName);
                        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                        SaveData();
                        EventAggregatorHost.Aggregator.SendMessage<SysMessage>(new SysMessage($" 新项目:‘{p.ProjectName}’创建完成"));
                    };
                    guide.ShowDialog();
                }));
            }
        }

        private RelayCommand saveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ?? (new RelayCommand(()=>
                {
                    if(SelectedProject == null)
                    {
                        EventAggregatorHost.Aggregator.SendMessage<SysMessage>(new SysMessage($"请先选择一个项目"));
                        return;
                    }
                    int serial = 0;
                    if (!SelectedProject.Items.Any())
                    {
                        serial = 1;
                    }
                    else
                    {
                        serial = SelectedProject.Items.Max(it => it.Serial) + 1;
                    }
                    string guid = SerialTool.NewID();
                    string bakPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Archive", SelectedProject.ProjectName, $"{guid}.bak");
                    File.Copy(SelectedProject.FilePath, bakPath);
                    var item = new ArchiveItem
                    {
                        AbsolutePath = bakPath,
                        CreateTime = DateTime.Now.ToString("s"),
                        Note = string.Empty,
                        Serial = serial,
                        Guid = guid
                    };
                    if (new IniFiles("Settings.ini").ReadString("Misc", "AutoCheckNewItem", "0") == "1")
                    {
                        SelectedProject.ObList.Where(it => it.Guid != item.Guid).ToList().ForEach(it => it.IsSelected = false);
                        item.IsSelected = true;
                    }
                    SelectedProject.ObList.Add(item);
                    SelectedItem = item;
                    
                    SaveData();
                    EventAggregatorHost.Aggregator.SendMessage<SysMessage>(new SysMessage($"对文件{SelectedProject.ProjectName}的保存已完成！"));
                    string soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sound", "Windows_Foreground.wav");
                    SoundTool.Play(soundPath);
                }));
            }
        }

        private RelayCommand loadCommand;

        public RelayCommand LoadCommand
        {
            get
            {
                return loadCommand ?? new RelayCommand(()=>
                {
                    if (SelectedItem != null && SelectedProject != null)
                    {
                        File.Copy(SelectedItem.AbsolutePath, SelectedProject.FilePath, true);
                        EventAggregatorHost.Aggregator.SendMessage<SysMessage>(new SysMessage($"文件{SelectedProject.ProjectName}已重新载入！"));
                        string soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sound", "Windows_Notify_System_Generic.wav");
                        SoundTool.Play(soundPath);
                    }
                });
            }
        }


        private RelayCommand settingCommand;

        public RelayCommand SettingCommand
        {
            get
            {
                if (settingCommand == null)
                {
                    settingCommand = new RelayCommand(() =>
                    {
                        Settings setting = new Settings();
                        
                        setting.ShowDialog();
                    });
                }
                return settingCommand;
            }

        }

        private RelayCommand<ArchiveProject> projectCheckedCommand;

        public RelayCommand<ArchiveProject> ProjectCheckedCommand
        {
            get
            {
                if (projectCheckedCommand == null)
                {
                    projectCheckedCommand = new RelayCommand<ArchiveProject>((p) =>
                    {
                        Entity.Projects.Where(it => it.ProjectName != p.ProjectName).ToList().ForEach(it => it.IsSelected = false);
                    });
                }
                return projectCheckedCommand;
            }

        }

        private RelayCommand<ArchiveItem> itemCheckCommand;

        public RelayCommand<ArchiveItem> ItemCheckedCommand
        {
            get
            {
                if (itemCheckCommand == null)
                {
                    itemCheckCommand = new RelayCommand<ArchiveItem>((p) =>
                    {
                        SelectedProject.Items.Where(it => it.Serial != p.Serial).ToList().ForEach(it => it.IsSelected = false);
                    });
                }
                return itemCheckCommand;
            }

        }


        private RelayCommand<ArchiveItem> deleteItemCommand;

        public RelayCommand<ArchiveItem> DeleteItemCommand
        {
            get
            {
                if (deleteItemCommand == null)
                {
                    deleteItemCommand = new RelayCommand<ArchiveItem>((p) =>
                    {
                        if (MessageBox.Show("确定删除此项?","警告",MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            File.Delete(SelectedItem.AbsolutePath);
                            EventAggregatorHost.Aggregator.SendMessage<SysMessage>(new SysMessage($"{SelectedItem.Note}({SelectedItem.Serial})已删除。"));

                            int deleteingSerial = SelectedItem.Serial;
                            SelectedProject.ObList.Remove(SelectedItem);
                            SelectedProject.ObList.Where(it => it.Serial > deleteingSerial).ToList().ForEach(it => it.Serial--);
                        }
                    });
                }
                return deleteItemCommand;
            }

        }

        private RelayCommand deleteProjectCommand;

        public RelayCommand DeleteProjectCommand
        {
            get
            {
                if (deleteProjectCommand == null)
                {
                    deleteProjectCommand = new RelayCommand(() =>
                    {
                        if (MessageBox.Show("确定删除此项目?", "警告", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            string archiveDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Archive", SelectedProject.ProjectName);
                            if(Directory.Exists(archiveDirectory))
                            {
                                Directory.Delete(archiveDirectory, true);
                                EventAggregatorHost.Aggregator.SendMessage<SysMessage>(new SysMessage($"项目{SelectedProject.ProjectName}已删除。"));
                                Entity.ObList.Remove(SelectedProject);
                            }
                        }
                    });
                }
                return deleteProjectCommand;
            }

        }


        private RelayCommand<ArchiveItem> dataGridSelectionChangedCommand;

        public RelayCommand<ArchiveItem> DataGridSelectionChangedCommand
        {
            get
            {
                return dataGridSelectionChangedCommand ?? new RelayCommand<ArchiveItem>((p)=>
                {
                    if (SelectedItem != null)
                    {
                        SelectedItem.IsSelected = true;
                    }
                });
            }
        }



        private void SaveData()
        {
            Entity.Projects.ForEach(it => it.Items = it.ObList.ToList());
            Entity.Projects = Entity.ObList?.ToList();
            XmlHelper.Write<ArchiveData>(configPath, Entity);
        }

        public void Handle(BackupMessage message)
        {
            SaveCommand.Execute(null);
        }

        public void Handle(RestoreMessage message)
        {
            LoadCommand.Execute(null);
        }
    }
}
