using System;
using System.Diagnostics;
using System.Windows;
using Microsoft.CodeAnalysis.CSharp;

namespace syntax.visualizer
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            syntaxVisualizer.Selected += syntaxVisualizer_Selected;
        }

        void syntaxVisualizer_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                codeEditor.SelectionStart = syntaxVisualizer.SelectedTextSpan.Start;
                codeEditor.SelectionLength = syntaxVisualizer.SelectedTextSpan.Length;
              
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Debug.WriteLine(ex);
            }
        }

       // private bool m_selectionChanging;

        private void CodeEditor_OnTextChanged(object sender, EventArgs e)
        {
            var tree = CSharpSyntaxTree.ParseText(codeEditor.Text);
            syntaxVisualizer.DisplaySyntaxTree(tree);
        }
    }
}
