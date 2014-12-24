using System;
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
            //syntaxVisualizer.Changed
        }

        private bool m_selectionChanging;

        private void CodeEditor_OnTextChanged(object sender, EventArgs e)
        {
            var tree = CSharpSyntaxTree.ParseText(codeEditor.Text);
            syntaxVisualizer.DisplaySyntaxTree(tree);
        }
    }
}
