using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SL.Utils.Entities
{
    [Serializable]
    [XmlRoot(ElementName = "Saves")]
    public class ArchiveData:ObservableObject
    {

        [XmlArray(ElementName = "Projects")]
        public List<ArchiveProject> Projects { get; set; }
        [XmlIgnore]
        private ObservableCollection<ArchiveProject> obList;
        [XmlIgnore]
        public ObservableCollection<ArchiveProject> ObList
        {
            get { return obList; }
            set
            {
                obList = value;
                RaisePropertyChanged("ObList");
            }
        }




        public ArchiveData()
        {
            Projects = new List<ArchiveProject>();
            ObList = new ObservableCollection<ArchiveProject>();
        }
    }


    [Serializable]
    [XmlType(TypeName = "ArchiveProject")]
    public class ArchiveProject:ObservableObject
    {
        [XmlIgnore]
        private bool isSelected;
        [XmlElement(ElementName = "IsSelected")]
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        [XmlElement(ElementName = "ProjectName")]
        public string ProjectName { get; set; }
        [XmlElement(ElementName = "LastModified")]
        public string LastModified { get; set; }
        [XmlElement(ElementName = "FilePath")]
        public string FilePath { get; set; }
        [XmlElement(ElementName = "FileName")]
        public string FileName { get; set; }

        [XmlArray(ElementName = "Items")]
        public List<ArchiveItem> Items{ get;set;}

        [XmlIgnore]
        private ObservableCollection<ArchiveItem> obList;

        [XmlIgnore]
        public ObservableCollection<ArchiveItem> ObList
        {
            get { return obList; }
            set
            {
                obList = value;
                RaisePropertyChanged("ObList");
            }
        }



        public ArchiveProject()
        {
            Items = new List<ArchiveItem>();
            ObList = new ObservableCollection<ArchiveItem>();
        }
    }


    [Serializable]
    [XmlType(TypeName = "ArchiveItem")]
    public class ArchiveItem:ObservableObject
    {
        [XmlIgnore]
        private bool isSelected;
        [XmlElement(ElementName = "IsSelected")]
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        [XmlElement(ElementName = "Serial")]
        public int Serial { get; set; }

        [XmlElement(ElementName = "Guid")]
        public string Guid { get; set; }

        [XmlElement(ElementName = "CreateTime")]
        public string CreateTime { get; set; }
        [XmlElement(ElementName = "AbsolutePath")]
        public string AbsolutePath { get; set; }
        [XmlElement(ElementName = "Note")]
        public string Note { get; set; }
    }
}
