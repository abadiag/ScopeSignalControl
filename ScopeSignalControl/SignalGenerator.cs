using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeSignalControl
{
    class SignalGenerator
    {
        //public delegate void SignalHandler(double x);
        public event EventHandler<SignalEventArgs> SignalChanged;
        private Random rand = new Random();
        private double timeX = 0;
        public WaveForm waveform { get; set; }
        public int Freq { get; set; } = 100;
        public int Amplitude { get; set; } = 50;

        public SignalGenerator(TimerClass timer)
        {
            timer.dispatcherTimer.Tick += new EventHandler(OnTick);
        }

        private void OnTick(object sender, EventArgs e)
        {
            switch (waveform)
            {
                case WaveForm.Sine:
                    SinusWaveGen(timeX);
                    break;
                case WaveForm.Random:
                    SetOutputSignal(rand.Next(-100, 100));
                    break;
            }
        }

        private void SinusWaveGen(double X)
        {
            SetOutputSignal(Math.Sin(X) * Amplitude);
            timeX = timeX + Freq;
        }

        private void SetOutputSignal(double outSignalValue)
        {
            EventHandler<SignalEventArgs> handler = SignalChanged;

            SignalEventArgs signalEvent = new SignalEventArgs();
            signalEvent.value = outSignalValue;

            handler?.Invoke(null, signalEvent);
        }
    }

    class SignalEventArgs : EventArgs
    {
        public double value { get; set; }
    }
}
