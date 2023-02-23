using Simulator;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
 
namespace PL
{

    enum UpdateType {UPDATECLOCK,UPDATESTART,UPDATEEND }
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
            switch ((UpdateType)e.ProgressPercentage)
            {
                case UpdateType.UPDATECLOCK:
                    {
                        this.txtClock.Text = DateTime.Now.ToString();
                        break;
                    }
                case UpdateType.UPDATESTART:
                    {
                        idd.Content = ((EventStatusArgs)e.UserState).OrderId;
                        break;
                    }
                case UpdateType.UPDATEEND:
                    break;
                default:
                    break;
            }
        }

        private void Bw_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("kkk");
        }

        private void Bw_DoWork(object? sender, DoWorkEventArgs e)
        {
            Simulator.Simulator.ReportStart (Simulator_reportStart);
            Simulator.Simulator.ReportEnd(Simulator_reportEnd);
            Simulator.Simulator.ReportEndSim (Simulator_reportEndSim);
            Simulator.Simulator.Active();
            while(!bw.CancellationPending)
            {
                Thread.Sleep(1000);
                bw.ReportProgress((int)UpdateType.UPDATECLOCK);
            }
        }

        private void Simulator_reportEndSim(object? sender, EventArgs e)
        {
            bw.CancelAsync();   
        }

        private void Simulator_reportEnd(object? sender, EventArgs e)
        {
            bw.ReportProgress((int)UpdateType.UPDATESTART, e);
        }

        private void Simulator_reportStart(object? sender, EventStatusArgs e)
        {
            bw.ReportProgress((int)UpdateType.UPDATEEND, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Simulator.Simulator.Deactive();
           
        }
    }
}
