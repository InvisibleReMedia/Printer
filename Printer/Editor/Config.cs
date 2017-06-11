using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Printer;

namespace Editor
{
    /// <summary>
    /// Configuration form
    /// </summary>
    public partial class Config : Form
    {

        /// <summary>
        /// Configuration object
        /// </summary>
        private Configuration conf;

        /// <summary>
        /// Constructor
        /// </summary>
        public Config()
        {
            InitializeComponent();
            this.conf = new Configuration();
        }

        /// <summary>
        /// Gets configuration object
        /// </summary>
        public Configuration Defines
        {
            get
            {
                return this.conf;
            }
            set
            {
                this.conf = value;
            }
        }

        /// <summary>
        /// When Ok clicked
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// When Cancel clicked
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.txtName.Text))
            {
                bool found = false;
                this.conf.Add(this.txtName.Text, this.txtValue.Text);
                for (int index = 0; index < configs.Items.Count; ++index)
                {
                    if (configs.Items[index].ToString() == this.txtName.Text)
                    {
                        configs.Items.RemoveAt(index);
                        configs.Items.Insert(index, this.txtName.Text);
                        found = true;
                        break;
                    }
                }
                if (!found)
                    configs.Items.Add(this.txtName.Text);
                FunLab.IsDirty = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.txtName.Text))
            {
                this.conf.Edit(this.txtName.Text, this.txtValue.Text);
                for (int index = 0; index < configs.Items.Count; ++index)
                {
                    if (configs.Items[index].ToString() == this.txtName.Text)
                    {
                        configs.Items.RemoveAt(index);
                        configs.Items.Insert(index, this.txtName.Text);
                        break;
                    }
                }
                FunLab.IsDirty = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.txtName.Text))
            {
                this.conf.Delete(this.txtName.Text);
                for (int index = 0; index < configs.Items.Count; ++index)
                {
                    if (configs.Items[index].ToString() == this.txtName.Text)
                    {
                        configs.Items.RemoveAt(index);
                        break;
                    }
                }
                FunLab.IsDirty = true;
            }
        }

        private void configs_DoubleClick(object sender, EventArgs e)
        {
            if (this.configs.SelectedIndex != -1)
            {
                IEnumerable<string> keys = this.conf.Find(this.configs.SelectedItem.ToString());
                string s = keys.FirstOrDefault();
                this.txtName.Text = s;
                this.txtValue.Text = this.conf[s];
            }
        }

        private void Config_Load(object sender, EventArgs e)
        {
            FunLab.FillConfigs(this.configs, this.conf);
        }
    }
}
