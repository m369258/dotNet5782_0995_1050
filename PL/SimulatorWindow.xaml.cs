using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for SimulatorWindow.xaml
    /// </summary>
    public partial class SimulatorWindow : Window
    {
        BackgroundWorker bw;
        public SimulatorWindow()
        {
            InitializeComponent();
            bw=new BackgroundWorker();
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.ProgressChanged += Bw_ProgressChanged;
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.RunWorkerAsync();
        }

        private void Bw_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            this.txtClock.Text = DateTime.Now.ToString();
        }

        private void Bw_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Bw_DoWork(object? sender, DoWorkEventArgs e)
        {
            Simulator.Simulator.reportStart += Simulator_reportStart;
            Simulator.Simulator.reportEnd += Simulator_reportEnd;
            Simulator.Simulator.reportEndSim += Simulator_reportEndSim;
            Simulator.Simulator.Active();
            while(!bw.WorkerSupportsCancellation)
            {
                Thread.Sleep(1000);
                //bw.ReportProgress();

            }
        }

        private void Simulator_reportEndSim(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Simulator_reportEnd(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Simulator_reportStart(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bw.CancelAsync();
        }
    }
}
