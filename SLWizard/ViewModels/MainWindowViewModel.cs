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
                    int serial = 0;
                    if (!Entity.Projects[0].Items.Any())
                    {
                        serial = 1;
                    }
                    else
                    {
                        serial = Entity.Projects[0].Items.Max(it => it.Serial) + 1;
                    }
                    string bakPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Archive", Entity.Projects[0].ProjectName, $"{serial}.bak");
                    File.Copy(Entity.Projects[0].FilePath, bakPath);
                    Entity.Projects[0].Items.Add(new ArchiveItem
                    {
                        AbsolutePath = bakPath,
                        CreateTime = DateTime.Now.ToString("s"),
                        Note = string.Empty,
                        Serial = serial
                    });
                    XmlHelper.Write<ArchiveConfig>(configPath, Entity);
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
                    File.Copy(Entity.Projects[0].Items[0].AbsolutePath,Entity.Projects[0].FilePath);
                });
            }
        }



    }
}
