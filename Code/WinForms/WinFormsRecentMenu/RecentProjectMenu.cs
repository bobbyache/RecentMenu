using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CygX1.UI.WinForms.RecentFileMenu
{
    public class RecentProjectMenu
    {
        public delegate void RecentProjectOpenedHandler(object sender, RecentProjectEventArgs e);
        public event RecentProjectOpenedHandler RecentProjectOpened;

        private ToolStripMenuItem menuItem;
        private IRecentFiles recentFiles;

        public string CurrentlyOpenedFile { get; set; }

        public RecentProjectMenu(ToolStripMenuItem menuItem, IRecentFiles recentFiles)
        {
            this.menuItem = menuItem;
            this.recentFiles = recentFiles;
            this.recentFiles.LoadList();

            this.menuItem.DropDownItemClicked += menuItem_DropDownItemClicked;
            this.menuItem.DropDownOpening += menuItem_DropDownOpening;
            createDummyRecentItem();
        }

        ~RecentProjectMenu()
        {
            this.menuItem.DropDownItemClicked -= menuItem_DropDownItemClicked;
            this.menuItem.DropDownOpening -= menuItem_DropDownOpening;
        }

        public void Notify(string filePath)
        {
            this.recentFiles.Add(filePath);
        }

        public void Remove(string filePath)
        {
            this.recentFiles.Remove(filePath);
        }

        private void menuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (RecentProjectOpened != null)
                RecentProjectOpened(this, new RecentProjectEventArgs(e.ClickedItem.Tag as RecentFile));
        }

        private void menuItem_DropDownOpening(object sender, EventArgs e)
        {
            this.menuItem.DropDownItems.Clear();

            if (this.recentFiles.FileList.Count > 0)
            {
                foreach (RecentFile recentFile in recentFiles.FileList)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem();
                    item.Text = recentFile.GetDisplayText(150, Directory.GetCurrentDirectory());
                    item.Tag = recentFile;
                    item.ToolTipText = recentFile.FullPath;
                    this.menuItem.DropDownItems.Add(item);
                    if (recentFile.FullPath == this.CurrentlyOpenedFile)
                        item.Enabled = false;
                }
            }
            else
            {
                createDummyRecentItem();
            }
        }

        private ToolStripMenuItem createDummyRecentItem()
        {
            this.menuItem.DropDownItems.Clear();

            ToolStripMenuItem dummyItem = new ToolStripMenuItem("{empty}");
            dummyItem.Enabled = false;
            this.menuItem.DropDownItems.Add(dummyItem);
            return dummyItem;
        }
    }
}
