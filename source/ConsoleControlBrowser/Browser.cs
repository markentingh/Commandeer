using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConsoleControlBrowser
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
            newTab.BackColor = Color.Black;
            newTab.Padding = new Padding(7);
            Tabs.TabPages.Add(newTab);
            Tabs.SelectedTab = newTab;

            //add new console
            var newConsole = new ConsoleControl.ConsoleControl();
            newConsole.Dock = DockStyle.Fill;
            newConsole.Location = new Point(0, 0);
            newConsole.BorderStyle = BorderStyle.None;
            newConsole.Padding = new Padding(0);

            //customize console
            var font = new Font(new FontFamily("Courier New"), 10);
            newConsole.Font = font;

            newTab.Controls.Add(newConsole);

            //start cmd process for console
            newConsole.StartProcess("c:\\cygwin\\bin\\bash.exe", "--login -i ", "c:\\cygwin\\bin\\");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewTab();
        }
    }
}
