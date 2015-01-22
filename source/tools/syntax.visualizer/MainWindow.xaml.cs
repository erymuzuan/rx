using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Win32;
using Bespoke.Sph.SyntaxVisualizers.ViewModels;

namespace Bespoke.Sph.SyntaxVisualizers
{
    public partial class MainWindow : IView
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += this.MainWindowLoaded;
            this.Closing += this.MainWindowClosing;
            this.AllowDrop = true;
            this.Drop += this.MainWindowDrop;
        }

        void MainWindowDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                HandleFileOpen(files[0]);
            }
        }

        private void HandleFileOpen(string file)
        {

            try
            {
                this.Cursor = Cursors.Wait;
                codeEditor.Text = File.ReadAllText(file);
                this.EditedFile = file;
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        public string EditedFile { get; set; }

        void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.LastEditedCode = codeEditor.Text;
            Properties.Settings.Default.WindowHeight = this.Height;
            Properties.Settings.Default.WindowWidth = this.Width;
            Properties.Settings.Default.WindowState = (int)this.WindowState;
            Properties.Settings.Default.Save();
        }

        async void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            var appvm = (AppViewModel)this.DataContext;
            await appvm.LoadAsync(this);
            syntaxVisualizer.Selected += SyntaxVisualizerSelected;
            Delegate load = new Action(() =>
            {
                this.Height = Properties.Settings.Default.WindowHeight;
                this.Width = Properties.Settings.Default.WindowWidth;
                this.WindowState = (WindowState)Properties.Settings.Default.WindowState;
                codeEditor.Text = Properties.Settings.Default.LastEditedCode;
            });
            this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, load);

        }

        void SyntaxVisualizerSelected(object sender, RoutedEventArgs e)
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

        private void FormatCode(object sender, RoutedEventArgs e)
        {
            var vm = (AppViewModel)this.DataContext;
            vm.IsBusy = true;
            this.QueueUserWorkItem(code2 =>
            {
                var code = code2.FormatCode();
                this.Post((c) =>
                {
                    codeEditor.Text = c;
                    vm.IsBusy = false;
                }, code);

            }, codeEditor.Text);

        }

        private void MenuExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            var folder = File.Exists(this.EditedFile)
                ? Path.GetDirectoryName(this.EditedFile)
                : Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var dlg = new OpenFileDialog
            {
                RestoreDirectory = true,
                InitialDirectory = folder,
                Filter = "c sharp|*.cs;*.txt|All Files|*.*",
                Title = "Open a csharp file"
            };

            if (dlg.ShowDialog() ?? false)
            {
                HandleFileOpen(dlg.FileName);
            }
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.EditedFile))
            {
                var dlg = new SaveFileDialog
                {
                    RestoreDirectory = true,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    Filter = "c sharp|*.cs;*.txt|All Files|*.*",
                    Title = "save"
                };

                if (dlg.ShowDialog() ?? false)
                {

                    this.EditedFile = dlg.FileName;
                    File.WriteAllText(this.EditedFile, codeEditor.Text);
                }
                else
                {
                    return;
                }

            }


            try
            {
                this.Cursor = Cursors.Wait;
                File.WriteAllText(this.EditedFile, codeEditor.Text);
            }
            finally
            {

                this.Cursor = Cursors.Arrow;
            }

        }

        public DispatcherObject View
        {
            get { return this; }
            set { throw new NotImplementedException(); }
        }
    }
}
