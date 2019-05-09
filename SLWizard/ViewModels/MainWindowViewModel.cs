using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SL.Utils;
using SL.Utils.Entities;
using SLWizard.UserControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SLWizard.ViewModels
{
    public class MainWindowViewModel:ViewModelBase,IListener<SysMessage>
    {
        public ArchiveConfig Entity { get; set; }

        readonly string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchiveConfig.xml");

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
        }

        private void LoadArchiveConfig()
        {
            if (!File.Exists(configPath))
            {
                Entity = new ArchiveConfig();
                XmlHelper.Write<ArchiveConfig>(configPath,Entity);
            }
            else
            {
                Entity = XmlHelper.Read<ArchiveConfig>(configPath);
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
                        Entity.Projects.Add(p);
                        string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Archive", p.ProjectName);
                        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                        XmlHelper.Write<ArchiveConfig>(configPath, Entity);
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
                    string bakPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Archive", SelectedProject.ProjectName, $"{serial}.bak");
                    File.Copy(SelectedProject.FilePath, bakPath);
                    SelectedProject.Items.Add(new ArchiveItem
                    {
                        AbsolutePath = bakPath,
                        CreateTime = DateTime.Now.ToString("s"),
                        Note = string.Empty,
                        Serial = serial
                    });
                    XmlHelper.Write<ArchiveConfig>(configPath, Entity);
                    EventAggregatorHost.Aggregator.SendMessage<SysMessage>(new SysMessage($"对文件{SelectedProject.ProjectName}的保存已完成！"));
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
                    File.Copy(SelectedItem.AbsolutePath, SelectedProject.FilePath,true);
                    EventAggregatorHost.Aggregator.SendMessage<SysMessage>(new SysMessage($"文件{SelectedProject.ProjectName}已重新载入！"));
                });
            }
        }



    }
}
