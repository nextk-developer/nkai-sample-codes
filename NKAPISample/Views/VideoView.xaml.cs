using NKAPISample.ViewModels;
using NKAPIService.API.VideoAnalysisSetting.Models;
using NKMeta;
using PredefineConstant;
using PredefineConstant.Enum.Analysis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

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
        private IMetaData _ReceivedDataSource;
        private List<Point> _points = new();
        private List<ROIDot> _currentRange = new();
        private DrawingType _currentDrawingType;
        private Shape _currentShape;

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
            else if (e.PropertyName == "IsDrawingMode") // rectangle, polygon, line등의 영역 생성 버튼을 눌렀을 때.
            {
                if (ViewModel.IsDrawingMode)
                {
                    _currentDrawingType = ViewModel.CurrentDrawingType;
                    _currentRange.Clear();
                    drawRange.Children.Clear();
                    drawComplete.Children.Clear();
                    switch (_currentDrawingType)
                    {
                        case DrawingType.All:
                            gridAllRange.Visibility = Visibility.Visible;
                            Task.Run(async () =>
                            {
                                await Task.Delay(1500);
                                Dispatcher.Invoke( () =>  gridAllRange.Visibility = Visibility.Collapsed);
                            });

                            _currentRange = new List<ROIDot>
                            {
                                new ROIDot { X = 0, Y = 0},
                                new ROIDot { X = 1.0, Y = 0},
                                new ROIDot { X = 1.0, Y = 1},
                                new ROIDot { X = 0, Y = 1},
                            };
                            ViewModel.SetRange(_currentRange);

                            break;

                        case DrawingType.Rect:
                        case DrawingType.Line:
                        case DrawingType.Polygon:
                        case DrawingType.MultiLine:
                            drawRange.Visibility = Visibility.Visible;
                            break;

                        default:
                            drawRange.Visibility = Visibility.Collapsed;
                            break;
                    }
                }
            }
            else if(e.PropertyName == "RoiList") // get roi 했을때 현재 roi 영역 목록을 화면에 표시하기 위함
            {
                if (ViewModel.RoiList == null)
                    return;

                _points.Clear();
                drawComplete.Children.Clear();
                foreach (var roi in ViewModel.RoiList)
                {
                    if (roi == null)
                        continue;

                    switch (roi.RoiType)
                    {
                        case DrawingType.All:
                            if (roi.RoiDots == null)
                                continue;

                            var point1 = new Point(roi.RoiDots[0].X * drawComplete.ActualWidth, roi.RoiDots[0].Y * drawComplete.ActualHeight);
                            DrawRectangle(point1);

                            var point2 = new Point(roi.RoiDots[2].X * drawComplete.ActualWidth, roi.RoiDots[2].Y * drawComplete.ActualHeight);
                            DrawRectangle(point2);

                            break;
                        case DrawingType.Rect:
                            if (roi.RoiDots == null)
                                continue;

                            var p1 = new Point(roi.RoiDots[0].X * drawComplete.ActualWidth, roi.RoiDots[0].Y * drawComplete.ActualHeight);
                            DrawRectangle(p1);

                            var p2 = new Point(roi.RoiDots[2].X * drawComplete.ActualWidth, roi.RoiDots[2].Y * drawComplete.ActualHeight);
                            DrawRectangle(p2);

                            break;
                        case DrawingType.MultiLine:
                            if (roi.RoiDots == null)
                                continue;

                            foreach (var dot in roi.RoiDots)
                            {
                                var p = new Point(dot.X * drawComplete.ActualWidth, dot.Y * drawComplete.ActualHeight);
                                _points.Add(p);
                            }

                            CompleteMultiLine(_points.Last());
                            break;
                        case DrawingType.Line:
                            if (roi.RoiDots == null)
                                continue;
                            foreach (var dot in roi.RoiDots)
                            {
                                var p = new Point(dot.X * drawComplete.ActualWidth, dot.Y * drawComplete.ActualHeight);
                                DrawLine(p);
                            }
                            break;
                        case DrawingType.Polygon:
                            if (roi.RoiDots == null)
                                continue;

                            foreach (var dot in roi.RoiDots)
                            {
                                if (_points.Count() == roi.RoiDots.Count() - 1)
                                    break;

                                var p = new Point(dot.X * drawComplete.ActualWidth, dot.Y * drawComplete.ActualHeight);
                                _points.Add(p);
                            }

                            ROIDot lastPolygonDot = roi.RoiDots.Last();
                            var last = new Point(lastPolygonDot.X * drawComplete.ActualWidth, lastPolygonDot.Y * drawComplete.ActualHeight);
                            CompletePolygon(last);
                            break;
                    }
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
                var fillColor = new SolidColorBrush(color) { Opacity = 0.3 };

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

                TextBlock label = new()
                {
                    Text = $"{obj.ObjectID}({obj.EventID}) / [{obj.ClassID},{obj.ObjectProb:0.0}] [{obj.EventType},{obj.AbnormalScore:0.0},{obj.StayTime:0.0}]",
                    Margin = new Thickness(rect.X, rect.Y, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };
                drawCanvas.Children.Add(shape);
                drawCanvas.Children.Add(label);
            }
        }

        private System.Drawing.Rectangle getActualRectangle(EventInfo obj, Int32Rect dirtyRect)
        {
            var rect = new System.Drawing.Rectangle((int)(obj.ImageRect.X * dirtyRect.Width), (int)(obj.ImageRect.Y * dirtyRect.Height),
                                                            (int)(obj.ImageRect.Width * dirtyRect.Width), (int)(obj.ImageRect.Height * dirtyRect.Height));
            return rect;
        }

        private void drawRange_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this);
            if (p == null) return;
            switch (_currentDrawingType)
            {
                case DrawingType.Rect:
                    DrawRectangle(p);
                    break;
                case DrawingType.Polygon:
                    if (e.RightButton == MouseButtonState.Pressed || (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2))
                    {
                        drawComplete.Children.Clear();
                        CompletePolygon(p);
                    }
                    else if (e.LeftButton == MouseButtonState.Pressed)
                        DrawPolygon(p);
                    
                    break;
                case DrawingType.Line:
                    DrawLine(p);
                    break;
                case DrawingType.MultiLine:
                    if (e.RightButton == MouseButtonState.Pressed || (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2))
                    {
                        drawComplete.Children.Clear();
                        CompleteMultiLine(p);
                    }
                    else if (e.LeftButton == MouseButtonState.Pressed)
                        DrawMultiLine(p);
                    break;

            }

        }


        private void CompletePolygon(Point p)
        {
            _points.Add(p);
            if (_points.Count() > 2)
            {

                var polygonPoints = new PointCollection();
                foreach (var pp in _points)
                {
                    var polygonPoint = new Point(pp.X, pp.Y);
                    polygonPoints.Add(polygonPoint);
                }


                _currentShape = new Polygon() {
                    Points = polygonPoints, 
                    HorizontalAlignment = HorizontalAlignment.Left, 
                    VerticalAlignment = VerticalAlignment.Top,
                    Fill = Brushes.Blue,
                    Opacity = 0.4,
                    Stroke = Brushes.DarkBlue,
                    StrokeThickness = 2
                };

                drawComplete.Children.Add(_currentShape);

                var dots = new List<ROIDot>();
                foreach(Point point in _points)
                {
                    dots.Add(new ROIDot() { X = point.X / drawRange.ActualWidth, Y = point.Y / drawRange.ActualHeight });
                }
                ViewModel.SetRange(dots);
                drawRange.Visibility=Visibility.Collapsed;
            }

            _points.Clear();
        }
        

        private void DrawMultiLine(Point p)
        {
            if (_points.Count() % 2 == 1)
            {
                Point prevPoint = _points.Last();
                var line = new Line()
                {
                    X1 = prevPoint.X,
                    Y1 = prevPoint.Y,
                    X2 = p.X,
                    Y2 = p.Y,
                    StrokeThickness = 5,
                    Stroke = Brushes.DarkGreen,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };
                _points.Add(p);
                drawComplete.Children.Add(line);
            }

            else
                _points.Add(p);           
        }


        private void CompleteMultiLine(Point p)
        {
            List<Point> pointResult = new();
            if(_points.Count() % 2 == 1)
                _points.Add(p);
            
            for(int i = 0; i < _points.Count(); i++)
            {
                if(i%2 == 1)
                {
                    Point prevPoint = _points[i - 1];
                    var line = new Line()
                    {
                        X1 = prevPoint.X,
                        Y1 = prevPoint.Y,
                        X2 = _points[i].X,
                        Y2 = _points[i].Y,
                        StrokeThickness = 5,
                        Stroke = Brushes.DarkBlue,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    drawComplete.Children.Add(line);
                    pointResult.Add(prevPoint);
                    pointResult.Add(_points[i]);
                }
            }
            
            var dots = new List<ROIDot>();
            foreach (Point point in pointResult)
            {
                dots.Add(new ROIDot() { X = point.X / drawRange.ActualWidth, Y = point.Y / drawRange.ActualHeight });
            }
            ViewModel.SetRange(dots);
            drawRange.Visibility = Visibility.Collapsed;
            _points.Clear();
        }

        private void DrawLine(Point p)
        {
            if(_points.Count() == 1)
            {
                Point prevPoint = _points.Last();
                var line = new Line()
                {
                    X1 = prevPoint.X,
                    Y1 = prevPoint.Y,
                    X2 = p.X,
                    Y2 = p.Y,
                    StrokeThickness = 3,
                    Stroke = Brushes.DarkBlue,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };

                drawComplete.Children.Add(line);
                var dotList = new List<ROIDot>();
                dotList.Add(new ROIDot() { X = prevPoint.X / drawRange.ActualWidth, Y = prevPoint.Y / drawRange.ActualHeight });
                dotList.Add(new ROIDot() { X = p.X / drawRange.ActualWidth, Y = p.Y / drawRange.ActualHeight });
                ViewModel.SetRange(dotList);
                drawRange.Visibility = Visibility.Collapsed;
                _points.Clear();
            }
            else
            {
                _points.Add(p);
            }
        }

        private void DrawPolygon(Point p)
        {
            if (_points.Count == 0)
            {
                _points.Add(p);
                return;
            }

            Point prevPoint = _points.Last();
            var line = new Line()
            {
                X1 = prevPoint.X,
                Y1 = prevPoint.Y,
                X2 = p.X,
                Y2 = p.Y,
                StrokeThickness = 3,
                Stroke = Brushes.DarkGreen,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            _points.Add(p);
            drawComplete.Children.Add(line);

        }

        private void DrawRectangle(Point p)
        {
            if (_points.Count >= 2)
            {
                _points.Clear();
                drawComplete.Children.Clear();
            }

            _points.Add(p);

            if(_points.Count == 1)
            {
                _currentShape = new Rectangle()
                {
                    Fill = Brushes.Blue,
                    Opacity = 0.4,
                    Stroke = Brushes.DarkBlue,
                    StrokeThickness = 2
                };
            }
            else if(_points.Count == 2 && _currentShape != null)
            {
                double width = Math.Abs(_points.First().X - _points.Last().X);
                double height = Math.Abs(_points.First().Y - _points.Last().Y);

                _currentShape.Width = width;
                _currentShape.Height = height;
                
                double xMargin = Math.Min(_points.First().X, _points.Last().X);
                double yMargin = Math.Min(_points.First().Y, _points.Last().Y);

                _currentShape.Margin = new Thickness(xMargin, yMargin, 0, 0);
                _currentShape.HorizontalAlignment = HorizontalAlignment.Left;
                _currentShape.VerticalAlignment = VerticalAlignment.Top;
                drawComplete.Children.Add(_currentShape);

                var dotList = new List<ROIDot>();
                dotList.Add(new ROIDot() { X = xMargin/drawRange.ActualWidth, Y = yMargin/drawRange.ActualHeight });
                dotList.Add(new ROIDot() { X = Math.Max(_points.First().X, _points.Last().X) / drawRange.ActualWidth, Y= yMargin / drawRange.ActualHeight });
                dotList.Add(new ROIDot() { X = Math.Max(_points.First().X, _points.Last().X) / drawRange.ActualWidth, Y = Math.Max(_points.First().Y, _points.Last().Y) / drawRange.ActualHeight });
                dotList.Add(new ROIDot() { X = xMargin / drawRange.ActualWidth, Y = Math.Max(_points.First().Y, _points.Last().Y) / drawRange.ActualHeight });
                ViewModel.SetRange(dotList);
                drawRange.Visibility = Visibility.Collapsed;
                _points.Clear();
            }

            
        }

    }
}
