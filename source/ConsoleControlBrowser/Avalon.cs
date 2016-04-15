using System;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Document;

namespace ConsoleControlBrowser
{
    class LineColorizer : DocumentColorizingTransformer
    {
        int lineNumber;
        Brush color;

        public LineColorizer(int lineNumber, Brush color)
        {
            this.lineNumber = lineNumber;
            this.color = color;
        }

        public int LineNumber
        {
            get { return lineNumber; }
            set
            {
                if (value < 1)
                {
                    lineNumber = 1;
                }
                else
                {
                    lineNumber = value;
                }
            }
        }

        protected override void ColorizeLine(DocumentLine line)
        {
            if (!line.IsDeleted && line.LineNumber == lineNumber)
            {
                ChangeLinePart(line.Offset, line.EndOffset, ApplyChanges);
            }
        }

        void ApplyChanges(VisualLineElement element)
        {
            // apply changes here
            element.TextRunProperties.SetForegroundBrush(color);
        }
    }
}
