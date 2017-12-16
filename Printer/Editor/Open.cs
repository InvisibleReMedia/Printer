using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Editor
{
    internal partial class Open : Form
    {

        #region Private Fields

        private string _directorySource;
        private string _fileName;
        private int columnSorter;

        #endregion

        public Open(string dir)
        {
            this._directorySource = dir;
            InitializeComponent();
        }

        public string FileName
        {
            get { return this._fileName; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ReadDirectories(DirectoryInfo dir, List<ListViewItem> list)
        {
            foreach(FileInfo fi in dir.GetFiles("*.prt"))
            {
                try
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = fi.Name;
                    Printer.PrinterObject po = Printer.PrinterObject.Load(fi.FullName);
                    lvi.SubItems.Add(po.Version);
                    lvi.SubItems.Add(po.Revision.ToString());
                    list.Add(lvi);
                }
                catch { }
            }
            foreach(DirectoryInfo di in dir.GetDirectories())
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = di.Name;
                list.Add(lvi);
                this.ReadDirectories(di, list);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            List<ListViewItem> list = new List<ListViewItem>();
            this.lvFiles.Items.Clear();
            DirectoryInfo di = new DirectoryInfo(this._directorySource);

            this.ReadDirectories(di, list);

            list.Sort(new Comparison<ListViewItem>(delegate(ListViewItem l1, ListViewItem l2)
            {
                int res = 0;
                try
                {
                    if (this.columnSorter == 0)
                    {
                        res = String.Compare(l1.Text, l2.Text);
                    }
                    else if (this.columnSorter == 1)
                    {
                        res = String.Compare(l1.SubItems[1].Text, l2.SubItems[1].Text);
                    }
                    else if (this.columnSorter == 2)
                    {
                        res = String.Compare(l1.SubItems[2].Text, l2.SubItems[2].Text);
                    }
                }
                catch { }
                return res;
            }));
            foreach (ListViewItem item in list)
            {
                this.lvFiles.Items.Add(item);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            this._fileName = this.lvFiles.SelectedItems[0].Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lvFiles.SelectedIndices.Count > 0)
            {
                this.btnOpen.Enabled = true;
                this.btnNew.Enabled = true;
            }
            else
            {
                this.btnOpen.Enabled = false;
                this.btnNew.Enabled = false;
            }
        }

        private void Open_Load(object sender, EventArgs e)
        {
            this.btnRefresh_Click(sender, e);
        }

        private void lvFiles_DoubleClick(object sender, EventArgs e)
        {
            this.btnOpen_Click(sender, e);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            this._fileName = "new.prt";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void lvFiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.columnSorter = e.Column;
            this.btnRefresh_Click(sender, new EventArgs());
        }

        private void lvFiles_Click(object sender, EventArgs e)
        {
            this.lvFiles_DoubleClick(sender, e);
        }
    }
}