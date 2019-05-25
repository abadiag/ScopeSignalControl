using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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

namespace ScopeSignalControl
{
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>
    public partial class ScopeView : UserControl
    {
        //Main attributte input value
        public double inputValue { get; set; } = 0.0;
        //Dispacher timer allow access to UI thread from timer thread
        private DispatcherTimer dispatcherTimer = null;
        //Main array holds values
        private static double[] waveArray = null;
        //// Create a red Brush  
        private SolidColorBrush redBrush = null;

        #region Public declaration section

        //Defines the 0 X value of the scope
        public double centerXScope { get; set; } = 0;

        public int GridScale { get; private set; }

        public int RefreshTime { get; private set; }

        public int timeStep { get; private set; }

        public int Speed { get; private set; }

        public double heightScope { get; private set; }

        public double widthScope { get; private set; } 
        #endregion

        /// <summary>
        /// Contructor not needs any param
        /// </summary>
        public ScopeView()
        {
            InitializeComponent();

            SetupInitView();
            InitQueue();
        }

        /// <summary>
        /// Every tick refresh the array and Draw it on the Screen 
        /// </summary>
        private void OnTick(Object timer, EventArgs tick)
        {
            var newArray = new double[waveArray.Length];
            newArray = ShiftValuesLeft(waveArray, Speed, inputValue);
            newArray[newArray.Length - 1] = inputValue;
            waveArray = newArray;
            DrawWave(newArray, timeStep);
        }

        /// <summary>
        /// Make shift of the array values Right to Left and add the new Value
        /// </summary>
        static double[] ShiftValuesLeft(double[] myArray, int jumps, double input)
        {
            double[] tArray = new double[myArray.Length];

            for (int i = 0; i < myArray.Length; i++)
            {

                if (i < myArray.Length - jumps)
                {
                    tArray[i] = myArray[i + jumps];

                }
                else
                {
                    tArray[i] = input;
                }

            }

            return tArray;
        }

        /// <summary>
        /// Draw the array on the Canvas and Refresh view
        /// </summary>
        private void DrawWave(double[] wave, int timeStep)
        {
            scopeScreen.Children.Clear();

            for (int time = 0; time < wave.Length - 1; time += timeStep)
            {               
                scopeScreen.Children.Add(wavePart(wave, time));
                // Forcing the CommandManager to raise the RequerySuggested event
                CommandManager.InvalidateRequerySuggested();
            }

            
        }

        /// <summary>
        /// Using BezierSegment draw little part of curve betweent two
        /// consecutive points of the array
        /// </summary>
        /// <param name="wave"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private Path wavePart(double[] wave, int time)
        {

            //Set up the bezierSegment
            BezierSegment segmentBezier = new BezierSegment();
            segmentBezier.Point1 = new Point(time, wave[time] + centerXScope);
            segmentBezier.Point2 = new Point(time + timeStep, wave[time + 1] + centerXScope);
            segmentBezier.Point3 = new Point(time + timeStep + timeStep, wave[time + 2] + centerXScope);
            segmentBezier.IsSmoothJoin = true;

            // Set up the PathFigure and Path to insert the segments
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = segmentBezier.Point1;
            pathFigure.IsClosed = false;
            pathFigure.MayHaveCurves();
            pathFigure.Segments.Add(segmentBezier);

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            System.Windows.Shapes.Path path = new Path();
            path.Data = pathGeometry;

            path.StrokeThickness = 1;
            path.Stroke = redBrush;

            return path;
        }


        /// <summary>
        /// Set All Initial values of the array at 0.0
        /// </summary>
        private void InitQueue()
        {
            waveArray = new double[(int)widthScope];

            for (int val = 0; val < widthScope - 1; val++)
            {
                waveArray[val] = 0;
            }
        }

        /// <summary>
        /// Set Initial standard values 
        /// </summary>
        private void SetupInitView()
        {
            //Set initial configs
            heightScope = scopeScreen.Height + 1;
            widthScope = scopeScreen.Width + 1;
            centerXScope = heightScope / 2;

            //Set initial values
            RefreshTime = 10;
            timeStep = 2;
            Speed = 2;

            inputValue = 0.0;

            redBrush = new SolidColorBrush();
            redBrush.Color = Colors.Red;

            SetupTimer();
        }

        /// <summary>
        /// DispacherTimer allow access cross threading draw to canvas from timer thread
        /// </summary>
        private void SetupTimer()
        {
            //  DispatcherTimer setup
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(OnTick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, RefreshTime);
            dispatcherTimer.Start();
        }
    }
}