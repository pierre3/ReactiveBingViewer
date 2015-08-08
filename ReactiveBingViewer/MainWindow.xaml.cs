using System.Windows;
using ReactiveBingViewer.ViewModels;
using System;

namespace ReactiveBingViewer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.Closed += (_, __) => { 
                var vm = this.DataContext as IDisposable;
                if (vm != null)
                {
                    vm.Dispose();
                }
            };
        }

    }
}
