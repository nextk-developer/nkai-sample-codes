using FlyleafLib.MediaPlayer;
using NKAPISample.Controls;
using NKAPISample.ViewModels;
using NKMeta;
using PredefineConstant;
using PredefineConstant.Enum.Analysis.EventType;
using PredefineConstant.Enum.Analysis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Shapes;
using System.Net;

namespace NKAPISample.Views
{
    /// <summary>
    /// Interaction logic for VideoView.xaml
    /// </summary>
    public partial class VideoView : UserControl
    {
        private ConcurrentQueue<List<EventInfo>> _detectedQueue = new();
        private CancellationTokenSource _updateMetaCancellation = new();
        public VideoViewModel ViewModel => DataContext as VideoViewModel;
        private DateTime _lastUpdateDetectionTime = DateTime.MinValue;
        private IMetaData _ReceivedDataSource;

        public VideoView()
        {
            InitializeComponent();
            
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ReceivedDataSource")
            {
                _ReceivedDataSource = ViewModel.ReceivedDataSource;
                if (_ReceivedDataSource == null)
                {
                    _updateMetaCancellation.Cancel();
                    drawCanvas.Children.Clear();
                }
                else
                {
                    StartUpdateMetaTask();
                    _ReceivedDataSource.OnReceivedMetaData += OnReceivedData;
                }
                
            }
        }

        private void ViewModel_PropertyChanging(object sender, System.ComponentModel.PropertyChangingEventArgs e)
        {
            if (e.PropertyName == "ReceivedDataSource")
            {
                if (_ReceivedDataSource != null)
                    _ReceivedDataSource.OnReceivedMetaData -= OnReceivedData;

            }
        }

        private void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanging += ViewModel_PropertyChanging;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }


        private void StartUpdateMetaTask()
        {
            _updateMetaCancellation = new();
            Task.Factory.StartNew(async () =>
            {
                if (_updateMetaCancellation != null)
                {
                    while (!_updateMetaCancellation.IsCancellationRequested)
                    {
                        try
                        {
                            var foundDetection = UpdateDetection();
                            if (!foundDetection)
                                await Task.Delay(25, _updateMetaCancellation.Token);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }


        private bool UpdateDetection()
        {
            var found = _detectedQueue.TryDequeue(out var detectedList);
            if (found)
            {
                try
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        if (ViewModel == null || ViewModel.Player == null) return;
                        if (!ViewModel.Player.CanPlay) return;

                        SetRectangle(detectedList);

                        var beginOrEndEvents = detectedList.Where(x => x.EventStatus != Progress.Continue);
                        if (beginOrEndEvents.Any())
                            this.Dispatcher.Invoke(() =>
                            {
                                var dirtyRect = new Int32Rect(0, 0, ViewModel.Player.renderer.ControlWidth, ViewModel.Player.renderer.ControlHeight);
                                Dictionary<EventInfo, System.Drawing.Rectangle> positionPair = new();
                                foreach (var detected in beginOrEndEvents)
                                {
                                    var rect = getActualRectangle(detected, dirtyRect);
                                    positionPair.Add(detected, rect);
                                }
                                ViewModel?.AlertEventFromEdgeServer(positionPair);
                            });
                    });
                    _lastUpdateDetectionTime = DateTime.UtcNow;
                }
                catch (Exception ee)
                {
                }
            }
            return found;
        }


        private void OnReceivedData(object sender, ObjectMeta om)
        {
            if (om.EventList != null)
            {
                var detectedList = om.EventList;
                if (detectedList != null && detectedList.Any())
                {
                    _detectedQueue.Enqueue(detectedList);
                }

            }

        }


        private void SetRectangle(List<EventInfo> detectedList)
        {
            var dirtyRect = new Int32Rect(0, 0, ViewModel.Player.renderer.ControlWidth, ViewModel.Player.renderer.ControlHeight);
            var firstDetectedList = detectedList.Where(x => !x.IsDetected && x.EventStatus != Progress.End);
            drawCanvas.Children.Clear();

            foreach (var obj in firstDetectedList)
            {
                var rect = getActualRectangle(obj, dirtyRect);

                if (rect.Width <= 0 || rect.X < 0 || rect.Y < 0 || rect.Height <= 0) continue;

                System.Windows.Media.Color color = System.Windows.Media.Color.FromRgb(255, 20, 84);
                var strokeColor = new SolidColorBrush(color);
                var fillColor = new SolidColorBrush(color) { Opacity = 0.5 };

                Rectangle shape = new()
                {
                    StrokeThickness = 1,
                    Width = rect.Width,
                    Height = rect.Height,
                    Fill = fillColor,
                    Stroke = strokeColor,
                    Margin = new Thickness(rect.X, rect.Y, 0, 0),
                    HorizontalAlignment=HorizontalAlignment.Left,
                    VerticalAlignment=VerticalAlignment.Top
                };

                Debug.WriteLine($"{obj.ObjectID}({obj.EventID}) / [{obj.ClassID},{obj.ObjectProb:0.0}] [{obj.EventType},{obj.AbnormalScore:0.0},{obj.StayTime:0.0}]");
                Debug.WriteLine($"X: {rect.X}, Y: {rect.Y}, Width: {rect.Width}, Height: {rect.Height}");
                drawCanvas.Children.Add(shape);
            }
        }

        private System.Drawing.Rectangle getActualRectangle(EventInfo obj, Int32Rect dirtyRect)
        {
            var rect = new System.Drawing.Rectangle((int)(obj.ImageRect.X * dirtyRect.Width), (int)(obj.ImageRect.Y * dirtyRect.Height),
                                                            (int)(obj.ImageRect.Width * dirtyRect.Width), (int)(obj.ImageRect.Height * dirtyRect.Height));
            return rect;
        }
    }
}
