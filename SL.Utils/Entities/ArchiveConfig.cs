﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SL.Utils.Entities
{
    [Serializable]
    [XmlRoot(ElementName = "Saves")]
    public class ArchiveConfig
    {
        [XmlArray(ElementName = "Projects")]
        public List<ArchiveProject> Projects { get; set; }

        public ArchiveConfig()
        {
            Projects = new List<ArchiveProject>();
        }
    }


    [Serializable]
    [XmlType(TypeName = "ArchiveProject")]
    public class ArchiveProject
    {
       

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

        public ArchiveProject()
        {
            Items = new List<ArchiveItem>();
        }
    }


    [Serializable]
    [XmlType(TypeName = "ArchiveItem")]
    public class ArchiveItem
    {
        [XmlElement(ElementName = "Serial")]
        public int Serial { get; set; }
        [XmlElement(ElementName = "CreateTime")]
        public string CreateTime { get; set; }
        [XmlElement(ElementName = "AbsolutePath")]
        public string AbsolutePath { get; set; }
        [XmlElement(ElementName = "Note")]
        public string Note { get; set; }
    }
}
