using Simulator;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace PL
{

    enum UpdateType {UPDATECLOCK,UPDATESTART,UPDATEEND }
    /// <summary>
    /// Interaction logic for SimulatorWindow.xaml
    /// </summary>
    public partial class SimulatorWindow : Window
    {

      

        //ProgressBar ProgressBar;
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
                        this.txtClockNow.Text = DateTime.Now.ToString();
                        break;
                    }
                case UpdateType.UPDATESTART:
                    {
                        idd.Text = (((EventStatusArgs)e.UserState).OrderId).ToString();
                        this.txtClockNow.Text = (((EventStatusArgs)e.UserState).start).ToString();
                        this.txtClockWill.Text = (((EventStatusArgs)e.UserState).finish).ToString();
                        this.txtStatusNow.Text = (((EventStatusArgs)e.UserState).now).ToString();
                        this.txtStatusWill.Text = (((EventStatusArgs)e.UserState).will).ToString();
                        //MyArgsSimulator.OrderId = ((EventStatusArgs)e.UserState).OrderId;
                        //MyArgsSimulator.start = ((EventStatusArgs)e.UserState).start;
                        //MyArgsSimulator.finish= ((EventStatusArgs)e.UserState).finish;
                        //MyArgsSimulator.now= ((EventStatusArgs)e.UserState).now;    
                        //MyArgsSimulator.will= ((EventStatusArgs)e.UserState).will;
                        break;
                    }
                case UpdateType.UPDATEEND:
                    {

                        break;
                    }
                default:
                    break;
            }
        }

        private void Bw_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("tanks");
            this.Close();
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
            bw.ReportProgress((int)UpdateType.UPDATEEND, e);
        }

        private void Simulator_reportStart(object? sender, EventStatusArgs e)
        {
            bw.ReportProgress((int)UpdateType.UPDATESTART, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Simulator.Simulator.Deactive();
           
        }
    }
}
