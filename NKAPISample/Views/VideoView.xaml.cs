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
        public PlayerInnerControl PlayerInnerContent { get; private set; } = new();
        private DateTime _lastUpdateDetectionTime = DateTime.MinValue;

        public VideoView()
        {
            InitializeComponent();
        }


        private void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            StartUpdateMetaTask();
        }


        #region ReceivedDataSource
        public IMetaData ReceivedDataSource
        {
            get => (IMetaData)GetValue(ReceivedDataSourceProperty);
            set => SetValue(ReceivedDataSourceProperty, value);
        }

        public static readonly DependencyProperty ReceivedDataSourceProperty = DependencyProperty.Register(nameof(ReceivedDataSource),
            typeof(IMetaData),
            typeof(VideoView),
            new FrameworkPropertyMetadata(OnReceivedDataSourceChanged));

        private static void OnReceivedDataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (VideoView)d;
            if (e.OldValue is IMetaData oldSource)
            {
                oldSource.OnReceivedMetaData -= view.OnReceivedData;
            }

            if (e.NewValue is IMetaData newSource)
            {
                newSource.OnReceivedMetaData += view.OnReceivedData;
            }
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
        #endregion


        private void StartUpdateMetaTask()
        {
            Task.Factory.StartNew(async () =>
            {
                if (_updateMetaCancellation != null)
                {
                    while (!_updateMetaCancellation.IsCancellationRequested)
                    {
                        try
                        {
                            var foundDetection = UpdateDetection();
                            //BlinkROIs();
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

                        PlayerInnerContent.drawCanvas.Children.Clear();

                        ChangeVisibleRoiInfo(ViewModel.IsInfo);
                        //DrawAbnormalThumnails(PlayerInnerContent.drawCanvas.Children, _dirtyRect, EventROIItems, detectedList, ViewModel.Player, ViewModel.IsInfo, ViewModel.IsBluring, ViewModel.CameraUID, _roiTextBlockDic);
                        //DrawKeyPoints(PlayerInnerContent.drawCanvas.Children, _dirtyRect, detectedList);
                    });
                    _lastUpdateDetectionTime = DateTime.UtcNow;
                }
                catch (Exception ee)
                {
                }
            }
            if ((DateTime.UtcNow - _lastUpdateDetectionTime).Seconds > 1)
            {
                this.Dispatcher.Invoke(() => PlayerInnerContent.drawCanvas.Children.Clear());
            }

            return found;
        }

        #region 여기 해야됨..!!!

        //internal static void DrawAbnormalThumnails(UIElementCollection canvas, Int32Rect dirtyRect, List<ViewModelROIItem> eventROIItems, List<EventInfo> Items, Player player, bool isInfo, bool isBluring, string channelId, Dictionary<string, TextBlock> roiTextBlockDic)
        //{
        //    if (Items == null) return;

        //    DefaultFontSize = (int)(dirtyRect.Height * 2 / ScreenInfo.DpiY);
        //    if (DefaultFontSize == 0) return;

        //    if (ViewMode == ViewMode.Expansion)
        //    {
        //        if (dirtyRect.Width != _expandedRectWidth)
        //        {
        //            return;
        //        }
        //    }

        //    canvas.Clear();

        //    int frameWidth = player.Video.Width;
        //    int frameHeight = player.Video.Height;

        //    foreach (var evt in Items.Where(x => !x.IsDetected && x.EventStatus != Progress.End))
        //    {
        //        var rect = new System.Drawing.Rectangle((int)(evt.ImageRect.X * dirtyRect.Width), (int)(evt.ImageRect.Y * dirtyRect.Height),
        //                                                    (int)(evt.ImageRect.Width * dirtyRect.Width), (int)(evt.ImageRect.Height * dirtyRect.Height));

        //        if (rect.Width < 0 || rect.X < 0 || rect.Y < 0 || rect.Height < 0) continue;
        //        string eventContents = default;
        //        Color color;



        //        Rectangle shape = new()
        //        {
        //            StrokeThickness = 1,
        //            Width = rect.Width,
        //            Height = rect.Height
        //        };

        //        Canvas.SetLeft(shape, rect.X);
        //        Canvas.SetTop(shape, rect.Y);

        //        if (evt.IsEvent)
        //        {
        //            if (!eventROIItems.Any()) continue;

        //            eventContents = $"{evt.ObjectID}({evt.EventID}) / [{evt.ClassID},{evt.ObjectProb:0.0}] [{evt.EventType},{evt.AbnormalScore:0.0},{evt.StayTime:0.0}]";

        //            var roi = eventROIItems.FirstOrDefault(e => e.UID == evt.RoiInfo.RoiUid);
        //            if (roi == null) continue;
        //            if (!roiTextBlockDic.ContainsKey(roi.UID)) continue;

        //            var txtBlock = roiTextBlockDic[roi.UID];

        //            float x = roi.RoiComponent.ROI.Points.Min(pt => pt.X);
        //            float y = roi.RoiComponent.ROI.Points.Min(pt => pt.Y);

        //            if (evt.EventType == IntegrationEventType.SpaceOccupancy)
        //            {
        //                txtBlock.Text = "Occupancy: " + evt.AbnormalScore.ToString("0.0") + "%";
        //            }
        //            else if (evt.EventType == IntegrationEventType.TimeOccupancy)
        //            {
        //                txtBlock.Text = "Occupancy: " + evt.AbnormalScore.ToString("0.0") + "vehicles/sec";
        //            }
        //            else if (evt.EventType == IntegrationEventType.Headaway)
        //            {
        //                txtBlock.Text = "Headway: " + evt.AbnormalScore.ToString("0.0") + "sec/vehicle";
        //            }
        //            else if (evt.EventType == IntegrationEventType.QueueLength)
        //            {
        //                txtBlock.Text = "Queue: " + evt.AbnormalScore.ToString("0.0") + "vehicles";
        //            }
        //            else if (evt.EventType == IntegrationEventType.IntervalVelocity)
        //            {
        //                if (roi != null)
        //                {
        //                    txtBlock.Text = "Velocity: " + evt.AbnormalScore.ToString("0.0") + "km/h\n";

        //                    if (!roiTextBlockDic.ContainsKey(evt.RoiInfo.RoiUid))
        //                    {
        //                        roiTextBlockDic.Add(evt.RoiInfo.RoiUid, txtBlock);
        //                    }
        //                    else
        //                    {
        //                        roiTextBlockDic[evt.RoiInfo.RoiUid] = txtBlock;
        //                    }
        //                }
        //            }
        //            else if (evt.EventType == IntegrationEventType.Direction || evt.EventType == IntegrationEventType.Straight ||
        //                evt.EventType == IntegrationEventType.RightTurn ||
        //                evt.EventType == IntegrationEventType.LeftTurn || evt.EventType == IntegrationEventType.UTurn ||
        //                evt.EventType == IntegrationEventType.ITS)
        //            {
        //                {
        //                    StringBuilder sb = new();
        //                    sb.Append("\t Cur / Min / Max / Avg\n");

        //                    if (evt.RoiInfo.RoiAggregatedDataItems.ContainsKey(IntegrationEventType.IntervalVelocity))
        //                    {
        //                        sb.Append($"VEL\t {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.IntervalVelocity].Cur?.ToString("000.0")}"
        //                                + $" / {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.IntervalVelocity].Min?.ToString("000.0")}"
        //                                + $" / {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.IntervalVelocity].Max?.ToString("000.0")}"
        //                                + $" / {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.IntervalVelocity].Avg?.ToString("000.0")}\n");
        //                    }
        //                    if (evt.RoiInfo.RoiAggregatedDataItems.ContainsKey(IntegrationEventType.TimeOccupancy))
        //                    {
        //                        sb.Append($"Ot\t {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.TimeOccupancy].Cur?.ToString("000.0")}"
        //                                + $" / {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.TimeOccupancy].Min?.ToString("000.0")}"
        //                                + $" / {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.TimeOccupancy].Max?.ToString("000.0")}"
        //                                + $" / {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.TimeOccupancy].Avg?.ToString("000.0")}\n");
        //                    }
        //                    if (evt.RoiInfo.RoiAggregatedDataItems.ContainsKey(IntegrationEventType.SpaceOccupancy))
        //                    {
        //                        sb.Append($"Os\t {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.SpaceOccupancy].Cur?.ToString("000.0")}"
        //                                + $" / {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.SpaceOccupancy].Min?.ToString("000.0")}"
        //                                + $" / {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.SpaceOccupancy].Max?.ToString("000.0")}"
        //                                + $" / {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.SpaceOccupancy].Avg?.ToString("000.0")}\n");
        //                    }
        //                    if (evt.RoiInfo.RoiAggregatedDataItems.ContainsKey(IntegrationEventType.Headaway))
        //                    {
        //                        sb.Append($"Headway {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.Headaway].Cur?.ToString("000.0")}"
        //                                + $" / {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.Headaway].Min?.ToString("000.0")}"
        //                                + $" / {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.Headaway].Max?.ToString("000.0")}"
        //                                + $" / {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.Headaway].Avg?.ToString("000.0")}\n");
        //                    }
        //                    if (evt.RoiInfo.RoiAggregatedDataItems.ContainsKey(IntegrationEventType.QueueLength))
        //                    {
        //                        sb.Append($"Queue\t {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.QueueLength].Cur?.ToString("000.0")}"
        //                                + $" / {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.QueueLength].Min?.ToString("000.0")}"
        //                                + $" / {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.QueueLength].Max?.ToString("000.0")}"
        //                                + $" / {evt.RoiInfo.RoiAggregatedDataItems[IntegrationEventType.QueueLength].Avg?.ToString("000.0")}\n");
        //                    }

        //                    sb.Append("\tCar / Truck / Bus / Etc\n");

        //                    IEnumerable<KeyValuePair<(ClassId ClassId, string RoiUId), int>> dict = ModelLive.GetCountOfObject();

        //                    var valueVeh = 0;
        //                    var valueTruck = 0;
        //                    var valueBus = 0;
        //                    var valueEtc = 0;

        //                    for (int i = 0; i < dict.Count(); i++)
        //                    {
        //                        var rank = dict.ElementAt(i);
        //                        var key = rank.Key;
        //                        var keyClassId = key.ClassId;
        //                        var keyRoiId = key.RoiUId;

        //                        if (keyRoiId == evt.RoiInfo.RoiUid)
        //                        {
        //                            if (keyClassId == ClassId.Vehicle)
        //                            {
        //                                valueVeh = rank.Value;
        //                            }
        //                            else if (keyClassId == ClassId.Vehicle_Truck)
        //                            {
        //                                valueTruck = rank.Value;
        //                            }
        //                            else if (keyClassId == ClassId.Vehicle_Bus)
        //                            {
        //                                valueBus = rank.Value;
        //                            }
        //                            else
        //                            {
        //                                valueEtc += rank.Value;
        //                            }
        //                        }
        //                    }

        //                    sb.Append($"Count\t{valueVeh.ToString("0000")} / {valueTruck.ToString("0000")} / {valueBus.ToString("0000")} / {valueEtc.ToString("0000")}\n");

        //                    txtBlock.Text = sb.ToString();
        //                }
        //            }
        //            else if (evt.EventType == IntegrationEventType.AbnormalObjectCount)
        //            {
        //                txtBlock.Text = $"{evt.ObjectID}({evt.EventID}) / {evt.ClassID} [{evt.EventType}]\n" +
        //                                           $"current object count\t: {evt.RoiInfo.RoiObjectCounting.ObjectCount}\n" +
        //                                           $"entered count total\t: {evt.RoiInfo.RoiObjectCounting.EnteredCount}\n" +
        //                                           $"exited count total\t: {evt.RoiInfo.RoiObjectCounting.ExitedCount}\n" +
        //                                           $"avg staying time\t\t: {evt.RoiInfo.AvgStayTime:0.0}";
        //            }
        //            else if (evt.EventType == IntegrationEventType.LineCrossing)
        //            {
        //                txtBlock.Text = $"{evt.ObjectID}({evt.EventID}) / {evt.ClassID} [{evt.EventType}]\n" +
        //                                           $"entered count total\t: {evt.RoiInfo.RoiObjectCounting.EnteredCount}\n" +
        //                                           $"exited count total\t: {evt.RoiInfo.RoiObjectCounting.ExitedCount}";
        //            }


        //            if (evt.EventType != IntegrationEventType.ITS)
        //            {
        //                roi.IsEvent = true;
        //                roi.LastOccuredEvent = DateTime.Now;
        //            }
        //            color = Color.FromRgb(255, 20, 84);
        //        }
        //        else
        //        {
        //            // 트래킹된 객체
        //            evt.ObjectColor ??= new ObjectColor() { R = 255, G = 255, B = 255 };

        //            eventContents = $"{evt.ObjectID} / [{evt.ClassID},{evt.ObjectProb:0.0}]";

        //            color = Color.FromRgb((byte)evt.ObjectColor.R, (byte)evt.ObjectColor.G, (byte)evt.ObjectColor.B);
        //        }

        //        if (isBluring)
        //        {
        //            var orgRect = new System.Drawing.Rectangle((int)(evt.ImageRect.X * frameWidth),
        //                                                       (int)(evt.ImageRect.Y * frameHeight),
        //                                                       (int)(evt.ImageRect.Width * frameWidth),
        //                                                       (int)(evt.ImageRect.Height * frameHeight));

        //            shape.Stroke = new SolidColorBrush(color);
        //            using var bitmap = player.TakeSnapshotToBitmap();
        //            if (bitmap != null)
        //            {
        //                using var cropBitmap = bitmap.CropAtRect(orgRect);
        //                shape.Fill = new ImageBrush(cropBitmap.ToBitmapImage());
        //                shape.Effect = new BlurEffect() { KernelType = KernelType.Gaussian, Radius = 10 };
        //            }
        //        }
        //        else
        //        {
        //            shape.Stroke = new SolidColorBrush(color);
        //            shape.Fill = new SolidColorBrush(color) { Opacity = 0.1 };
        //        }

        //        #region Drwing vechile license plate
        //        if (evt.EventDetail.Vehicle.LicenseImageRect != null && evt.EventDetail.Vehicle.LicenseImageRect.Width > 0)
        //        {
        //            var licenseImageRect = evt.EventDetail.Vehicle.LicenseImageRect;

        //            var licenseRect = new System.Drawing.Rectangle((int)(licenseImageRect.X * dirtyRect.Width), (int)(licenseImageRect.Y * dirtyRect.Height),
        //                                                    (int)(licenseImageRect.Width * dirtyRect.Width), (int)(licenseImageRect.Height * dirtyRect.Height));

        //            Rectangle licenseShape = new()
        //            {
        //                StrokeThickness = 1,
        //                Width = licenseRect.Width,
        //                Height = licenseRect.Height,
        //                Stroke = new SolidColorBrush(color),
        //                Fill = new SolidColorBrush(color) { Opacity = 0.1 }
        //            };

        //            Canvas.SetLeft(licenseShape, licenseRect.X);
        //            Canvas.SetTop(licenseShape, licenseRect.Y);

        //            canvas.Add(licenseShape);
        //        }
        //        #endregion



        //        if (isInfo)
        //        {
        //            if (eventContents != null)
        //            {
        //                if (eventContents.Contains("Occupancy") == false && eventContents.Contains("Headaway") == false
        //                    && eventContents.Contains("Queue") == false && eventContents.Contains("ITS") == false)
        //                {
        //                    TextBlock textBlock = new()
        //                    {
        //                        FontSize = DefaultFontSize,
        //                        Visibility = Visibility.Visible,
        //                        Text = eventContents,
        //                        Foreground = new SolidColorBrush(color)
        //                    };

        //                    Canvas.SetLeft(textBlock, rect.X);
        //                    Canvas.SetTop(textBlock, rect.Y);
        //                    canvas.Add(textBlock);
        //                }
        //            }
        //        }

        //        canvas.Add(shape);
        //    }
        //}

        #endregion
        private void ChangeVisibleRoiInfo(bool isInfo)
        {
            foreach (var control in PlayerInnerContent.roiContent.Children)
            {
                if (control is FrameworkElement fe)
                {
                    var visible = isInfo ? Visibility.Visible : Visibility.Hidden;
                    if (fe.Visibility != visible)
                        fe.Visibility = visible;
                }

            }
        }
    }
}
