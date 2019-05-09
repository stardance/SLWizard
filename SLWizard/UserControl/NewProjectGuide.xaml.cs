using Microsoft.Win32;
using SL.Utils.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SLWizard.UserControl
{
    /// <summary>
    /// NewProjectGuide.xaml 的交互逻辑
    /// </summary>
    public partial class NewProjectGuide : Window
    {
        public string recentDirectory { get; set; } = string.Empty;


        public delegate void SubmitProject(ArchiveProject project);

        /// <summary>
        /// 当提交新建项目时触发的事件
        /// </summary>
        public event SubmitProject OnSubmitProject;

        public NewProjectGuide()
        {
            InitializeComponent();
        }

        public NewProjectGuide(string recentDir)
        {
            InitializeComponent();
            recentDirectory = recentDir;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(Title.Text))
            {
                MessageBox.Show("标题不允许为空!");
                return;
            }
            int index = FilePosition.Text.LastIndexOf('\\');
            string fileName = FilePosition.Text.Substring(index + 1);
            OnSubmitProject?.Invoke(new ArchiveProject
            {
                FilePath = FilePosition.Text,
                FileName = fileName,
                ProjectName = Title.Text,
                LastModified =DateTime.Now.ToString("s")
            });
            this.Close();
        }

        private void SelectFileDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            this.FilePosition.Text = dialog.FileName;
        }
    }
}
