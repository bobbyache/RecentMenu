using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CygX1.UI.WinForms.RecentFileMenu;

namespace RecentMenuTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeRecentMenu();

            this.Text = "Test - (no file)";
        }

        private RecentProjectMenu recentProjectMenu;

        private void InitializeRecentMenu()
        {
            RecentFiles recentFiles = new RecentFiles();
            recentFiles.MaxNoOfFiles = 25;
            recentFiles.RegistryPath = @"Software\CygSoft\XessGenerator";
            recentFiles.RegistrySubFolder = "Recent";
            recentFiles.MaxDisplayNameLength = 80;

            recentProjectMenu = new RecentProjectMenu(menuRecent, recentFiles);
            recentProjectMenu.RecentProjectOpened += RecentProjectMenu_RecentProjectOpened;
        }

        private void RecentProjectMenu_RecentProjectOpened(object sender, RecentProjectEventArgs e)
        {
            if (File.Exists(e.RecentFile.FullPath))
            {
                this.Text = "Test - " + e.RecentFile.FullPath;
                recentProjectMenu.Notify(e.RecentFile.FullPath);
            }
            else
            {
                //recentProjectMenu.Remove(e.RecentFile.FullPath);
                MessageBox.Show("Recent file will be removed.");
            }
        }
    }
}
