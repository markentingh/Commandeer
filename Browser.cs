using System;
using System.Drawing;
using System.Windows.Forms;

namespace Commandeer
{
    public partial class Browser : Form
    {
        public Browser()
        {
            InitializeComponent();
            NewTab();
        }

        private void NewTab()
        {
            //add new tab
            var newTab = new TabPage();
            newTab.Text = "Console" + (Tabs.TabPages.Count > 0 ? (Tabs.TabPages.Count + 1).ToString() : "");
            newTab.BorderStyle = BorderStyle.None;
            //newTab.BackColor = Color.Black;
            newTab.Padding = new Padding(0);
            Tabs.TabPages.Add(newTab);
            Tabs.SelectedTab = newTab;

            //add new console
            var newConsole = new Console();
            newConsole.Dock = DockStyle.Fill;
            newConsole.Location = new Point(0, 0);
            newConsole.BorderStyle = BorderStyle.None;
            newConsole.Padding = new Padding(0);

            newTab.Controls.Add(newConsole);

            //start console
            newConsole.MaxLines = 500;
            newConsole.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewTab();
        }
    }
}
