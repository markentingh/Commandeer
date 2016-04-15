using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;

namespace ConsoleControlBrowser
{
    public partial class Console : UserControl
    {

        public TextEditor Editor;
        public Process Process;

        private int selStart = 0;

        public Console()
        {
            InitializeComponent();

            //host the WPF control (ICSharpCode.AvalonEdit) as a console
            var host = new ElementHost();
            Editor = new TextEditor();
            host.Dock = DockStyle.Fill;
            host.Location = new System.Drawing.Point(0, 0);
            host.Child = Editor;
            Controls.Add(host);

            //configure console
            Editor.SyntaxHighlighting = null;
            Editor.KeyDown += Editor_KeyDown;

            //configure context menu for console
            var menu = new System.Windows.Controls.ContextMenu();
            var newItem = new System.Windows.Controls.MenuItem();
            newItem.Click += MenuItem_Copy_Click;
            newItem.Header = "Copy";
            menu.Items.Add(newItem);
            Editor.ContextMenu = menu;


            //stylize console
            Editor.Background = Brushes.Black;
            Editor.Foreground = Brushes.White;
            Editor.FontFamily = new FontFamily("Courier New");
            Editor.FontSize = 12;
            Editor.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Disabled;
            Editor.Padding = new System.Windows.Thickness(7);
        }

        #region "Textbox"
        private void WriteOutput(string output, Brush color)
        {
            Editor.Document.Insert(Editor.Text.Length, output + "\n");
            Editor.TextArea.TextView.LineTransformers.Add(new LineColorizer(Editor.Document.LineCount - 1, color));
            Editor.CaretOffset = Editor.Text.Length;
            Editor.ScrollToEnd();
            selStart = Editor.Text.Length;
        }
        #endregion

        #region "Context Menu"
        private void MenuItem_Copy_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(Editor.TextArea.Selection.Length > 0)
            {
                Clipboard.SetText(Editor.SelectedText);
            }
        }
        #endregion

        #region "Process"

        public void Start(string commands = "")
        {
            //start bash (cygwin)
            Process = new Process();
            Process.StartInfo.FileName = "C:\\cygwin\\bin\\bash.exe";
            Process.StartInfo.Arguments = "--login -i " + (commands != "" ? " -c " + commands : "");
            Process.StartInfo.WorkingDirectory = "C:\\cygwin\\bin";

            Process.StartInfo.EnvironmentVariables["CYGWIN"] = "tty";

            Process.StartInfo.RedirectStandardError = true;
            Process.StartInfo.RedirectStandardInput = true;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.CreateNoWindow = true;
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.ErrorDialog = false;

            Process.Start();

            DataReceivedEventHandler errorEventHandler = new DataReceivedEventHandler(ErrorDataReceived);
            DataReceivedEventHandler outEventHandler = new DataReceivedEventHandler(OutDataReceived);
            Process.OutputDataReceived += outEventHandler;
            Process.ErrorDataReceived += errorEventHandler;
            Process.BeginErrorReadLine();
            Process.BeginOutputReadLine();
        }

        public void End()
        {
            Process.Close();
        }

        #endregion

        #region "Events"

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                WriteOutput(e.Data, Brushes.Red);
            }));
            
        }

        private void OutDataReceived(object sender, DataReceivedEventArgs e)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                WriteOutput(e.Data, Brushes.White);
            }));
        }

        private void Editor_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
        }

        

        #endregion
    }
}
