using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;


namespace Commandeer
{
    public partial class Console : UserControl
    {
        public TextEditor Editor;
        public Process Process;
        private StreamWriter inputWriter;
        private List<LineColorizer> coloredLines = new List<LineColorizer>();

        private int selStart = 0;
        private string lastInput = "";
        private int _maxLines = 200;

        public int MaxLines
        {
            get { return _maxLines; }
            set { _maxLines = value; }
        }

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
            Editor.TextArea.Focus();

            //configure console
            Editor.SyntaxHighlighting = null;
            //Editor.KeyDown += Editor_KeyDown;
            Editor.PreviewKeyDown += Editor_KeyDown;

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
            var len = Editor.Text.Length;
            var lineStart = len == 0 ? 1 : Editor.Document.LineCount + 1;
            var lineEnd = 1;
            var lineOffset = 0;
            Editor.Document.Insert(len, (len == 0 ? "" : "\n") + output.Trim());
            Thread.Sleep(10);
            lineEnd = Editor.Document.LineCount;
            lineOffset = lineEnd - MaxLines;
            for (var x = lineStart; x <= lineEnd; x++)
            {
                //color all lines of text in the output
                coloredLines.Add(new LineColorizer(x, color));
                Editor.TextArea.TextView.LineTransformers.Add(coloredLines.Last());
            }

            //remove extra lines if limit reached
            if(lineEnd > MaxLines)
            {
                var line = Editor.Document.Lines[lineOffset];
                var i = 0;
                Editor.Document.Remove(0, line.Offset + line.Length + 1);

                //fix line colors
                for (var x = 0; x < coloredLines.Count - (lineEnd - lineStart); x++)
                {
                    coloredLines[x].LineNumber -= (lineOffset + 1);
                    if(coloredLines[x].LineNumber < 0)
                    {
                        coloredLines[x].deleted = true;
                    }
                    else
                    {
                        Editor.TextArea.TextView.LineTransformers[x] = coloredLines[x];
                    }
                }
                while(coloredLines.Count > 0)
                {
                    if(coloredLines[i].deleted == true)
                    {
                        coloredLines.RemoveAt(0);
                        Editor.TextArea.TextView.LineTransformers.RemoveAt(0);
                    }
                    else
                    {
                        break;
                    } 
                }

                Editor.InvalidateArrange();
            }
            

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
            Thread.Sleep(200);
            Process = new Process();
            Process.StartInfo.FileName = "C:\\cygwin\\bin\\bash.exe";
            Process.StartInfo.Arguments = "--login -i " + (commands != "" ? " -c " + commands : "");
            Process.StartInfo.WorkingDirectory = "C:\\cygwin\\bin";
            //Process.StartInfo.FileName = "cmd";
            //Process.StartInfo.Arguments = " -c c:\\cygwin\\bin\\bash.exe --login -i " + commands;

            Process.StartInfo.RedirectStandardError = true;
            Process.StartInfo.RedirectStandardInput = true;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.CreateNoWindow = true;
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.ErrorDialog = false;
            Process.EnableRaisingEvents = true;

            Process.Start();

            //setup inputs & outputs for process
            inputWriter = Process.StandardInput; //process writer

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

        private void OutDataReceived(object sender, DataReceivedEventArgs e)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                var output = e.Data;
                var color = Brushes.White;

                //transform output if needed
                if (output.IndexOf(lastInput) == 0)
                {
                    output = output.Substring(lastInput.Length + 1);
                }

                //change font color if needed
                if (lastInput == "ls" || lastInput.IndexOf("ls ") == 0)
                {
                    color = Brushes.LightSeaGreen;
                }

                WriteOutput(output, color);
            }));
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                var output = e.Data;
                var color = Brushes.Red;
                var nogo = false;
                var i = new int[] { 0, 0, 0 };
                var firstfew = new int[] { 0, 1, 2 };

                if (output.Length <= 2) { nogo = true; }
                if(lastInput != "" && firstfew.Contains(output.IndexOf(lastInput))) { nogo = true; }

                //transform output if needed
                if (nogo == true)
                {
                }
                else if (output.IndexOf("bash: ") == 0)
                {
                    //bash message
                    if (output.IndexOf("bash: cannot set terminal process group") == 0)
                    {
                        nogo = true;
                    }
                    else if (output.IndexOf("bash: no job control in this shell") == 0)
                    {
                        nogo = true;
                    }
                }
                else if (firstfew.Contains(output.IndexOf("]0;")))
                {
                    //beginning of input header
                    nogo = true;

                }
                else if (firstfew.Contains(output.IndexOf("[32m")))
                {
                    //end of input header
                    i[0] = output.IndexOf("[32m");
                    i[1] = output.IndexOf("[33m");
                    i[2] = output.IndexOf("[0m");
                    if (i[0] >= 0 && i[1] > i[0] && i[2] > i[1])
                    {
                        output = output.Substring(i[0] + 4, i[1] - i[0] - 5) + output.Substring(i[1] + 4, i[2] - i[1] - 5);
                        WriteOutput(output, Brushes.Orange);
                        WriteOutput("$", Brushes.White);
                        nogo = true;
                    }
                }

                //finally, write the output
                if (nogo == false) { WriteOutput(output, color); }
            }));
        }
        #endregion

        #region "Events"
       
        private void Editor_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(Editor.CaretOffset == Editor.Text.Length)
            {
                if(e.Key == Key.Return)
                {
                    var input = Editor.Text.Substring(selStart, Editor.Text.Length - selStart).Replace("\n","").Replace("\r","");
                    lastInput = input;
                    inputWriter.Write(input + "\n");
                    inputWriter.Flush();
                    e.Handled = true;
                }else if(e.Key == Key.Back)
                {
                    if(Editor.Text.Length <= selStart)
                    {
                        e.Handled = true;
                    }
                }else if(e.Key == Key.Tab)
                {
                    //TODO: auto-complete command

                    e.Handled = true;
                }
            }
        }
        #endregion
    }
}
