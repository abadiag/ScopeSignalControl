using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace ScopeSignalControl
{
    enum WaveForm
    {
        Sine,
        Random,
        Square,
        Triangle
    }
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ScopeView scopeView = new ScopeView();
        TimerClass timerClass = new TimerClass();
        SignalGenerator signalGen;
        private bool listening = false;

        public MainWindow()
        {          
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            WaveFormCombo.Items.Add(newItem: WaveForm.Random);
            WaveFormCombo.Items.Add(newItem: WaveForm.Sine);

            mainContainer.Children.Add(scopeView);

            signalGen = new SignalGenerator(timerClass);
        }

        private void OnSignalGenerator(object sender, SignalEventArgs valueArgs)
        {
            scopeView.inputValue = valueArgs.value;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scopeView.inputValue = e.NewValue;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (listening)
            {
                signalGen.SignalChanged -= OnSignalGenerator;
            }
            else
            {
                signalGen.SignalChanged += OnSignalGenerator;
            }

            listening = !listening;    
        }

        private void WaveFormCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            signalGen.waveform = (WaveForm)WaveFormCombo.SelectedItem;
        }

        private void FreqTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
            signalGen.Freq =  Convert.ToInt32(FreqTextBox.Text);
        }

        private void AmpTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            signalGen.Amplitude = Convert.ToInt32(AmpTextBox.Text);
        }
    }
}
