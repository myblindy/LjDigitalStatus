using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LjDigitalStatus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region bindable properties
        const int DigitalInputCount = 16;
        public ObservableCollection<DigitalInputModel> DigitalInputs { get; } = new ObservableCollection<DigitalInputModel>();

        public DigitalInputModel ControlDigitalInput { get; } = new DigitalInputModel { Name = "IO0" };

        bool automaticMode = true;
        public bool AutomaticMode
        {
            get => automaticMode;
            set { automaticMode = value; FirePropertyChanged(nameof(AutomaticMode)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void FirePropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion

        string FileName;

        public MainWindow()
        {
            InitializeComponent();
            for (int idx = 0; idx < DigitalInputCount; ++idx)
                DigitalInputs.Add(new DigitalInputModel { Name = $"DI{idx}" });
            DataContext = this;

            var sfd = new SaveFileDialog { Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*" };
            if (!sfd.ShowDialog() ?? false)
                Environment.Exit(0);
            FileName = sfd.FileName;

            var filesync = new object();
            ControlDigitalInput.PropertyChanged += (s, e) =>
            {
                var di = (DigitalInputModel)s;
                if (e.PropertyName == nameof(di.IsOn) && di.IsOn)
                {
                    var time = DateTime.Now;
                    var state = string.Join(",", DigitalInputs.Select(w => w.IsOn ? 1 : 0));
                    ThreadPool.QueueUserWorkItem(_ => { lock (filesync) File.AppendAllText(FileName, $"{time:yyyy-MM-dd HH:mm:ss.ffff},{state}\n"); });
                }
            };

            new Thread(BackgroundThread) { Name = "Background Thread", IsBackground = true }.Start();
        }

        private void BackgroundThread()
        {
            bool first = true;
            ulong lastvals = 0;

            while (true)
                if (AutomaticMode)
                {
                    var vals = LJ.ReadDigitalInputs();

                    if (first || lastvals != vals)
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                        {
                            for (int idx = 0; idx < DigitalInputCount; ++idx)
                                DigitalInputs[idx].IsOn = (vals & (1UL << idx)) != 0;
                            ControlDigitalInput.IsOn = (vals & (1 << 16)) != 0;
                        }));
                        lastvals = vals;
                        first = false;
                    }
                }
                else
                    first = true;
        }
    }

    public class DigitalInputModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void FirePropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public string Name { get; set; }

        bool isOn;
        public bool IsOn { get => isOn; set { if (IsOn == value) return; isOn = value; FirePropertyChanged(nameof(IsOn)); } }
    }
}
