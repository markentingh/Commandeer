using System;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Document;

namespace Commandeer
{
    class LineColorizer : DocumentColorizingTransformer
    {
        public int LineNumber;
        public Brush color;
        public bool deleted = false;

        public LineColorizer(int lineNumber, Brush color)
        {
            LineNumber = lineNumber;
            this.color = color;
        }

        protected override void ColorizeLine(DocumentLine line)
        {
            if(deleted == true) { return; }
            if (!line.IsDeleted && line.LineNumber == LineNumber)
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
