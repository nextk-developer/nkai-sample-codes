
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using FlyleafLib;
using FlyleafLib.MediaPlayer;
using Newtonsoft.Json;
using NKAPISample.Controls;
using NKAPISample.Models;
using NKAPISample.Views;
using NKAPIService.API.VideoAnalysisSetting.Models;
using NKMeta;
using PredefineConstant;
using PredefineConstant.Enum.Analysis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;
using Vortice.Mathematics;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace NKAPISample.ViewModels
{
    public partial class VideoViewModel : ObservableObject
    {
        private Player _Player;
        private MainViewModel _MainVM;
        public ChannelViewModel ChannelComponent { get; }
        private bool _IsInfo;
        private ConcurrentQueue<List<EventInfo>> _detectedQueue = new();
        public bool IsInfo { get => _IsInfo; set => SetProperty(ref _IsInfo, value); }
        public Player Player { get => _Player; set => SetProperty(ref _Player, value); }
        private IMetaData _ReceivedDataSource;

        public IMetaData ReceivedDataSource { get => _ReceivedDataSource; set => SetProperty(ref _ReceivedDataSource, value); }


        private bool _IsDrawingMode;

        public bool IsDrawingMode { get => _IsDrawingMode; set => SetProperty(ref _IsDrawingMode, value); }

        public VideoViewModel()
        {
            _MainVM = Ioc.Default.GetRequiredService<MainViewModel>();
            _MainVM.VAStopped = Stop;
            _MainVM.ROIEventTypeSelected = SetDrawingMode;
        }

        private void SetDrawingMode(DrawingType drawingType)
        {
            if (drawingType == DrawingType.Clear)
                IsDrawingMode = false;
            
        }

        internal void VideoStart(string url)
        {
            if (_Player == null)
            {
                InitializePlayer(url);
            }
            else
            {
                Player.OpenAsync(url);
            }
        }

        internal void VAStart(ChannelComponent cc)
        {
            ReceivedDataSource = cc.ObjectMetaClient;
        }
        
        private void InitializePlayer(string url)
        {
            var config = new Config();

            config.Demuxer.BufferDuration = TimeSpan.FromMilliseconds(0).Ticks; // Reduces RAM as the demuxer will not buffer large number of packets
            config.Decoder.MaxVideoFrames = 2; // Reduces VRAM as video decoder will not keep large queues in VRAM (should be tested for smooth video playback, especially for 4K)
            config.Decoder.VideoThreads = 2; // Reduces VRAM/GPU (should be tested for smooth video playback, especially for 4K)
                                             // Consider using lower quality streams on normal screen and higher quality on fullscreen (if available)
            config.Demuxer.MaxErrors = 1;
            config.Demuxer.ReadTimeout = TimeSpan.FromSeconds(3).Ticks;
            config.Demuxer.OpenTimeout = TimeSpan.FromSeconds(3).Ticks;
            config.Demuxer.CloseTimeout = TimeSpan.FromSeconds(3).Ticks;
            config.Demuxer.SeekTimeout = TimeSpan.FromSeconds(3).Ticks;
            config.Player.MaxLatency = TimeSpan.FromMilliseconds(500).Ticks;
            config.Decoder.ShowCorrupted = true;


            config.Audio.Enabled = false;
            config.Subtitles.Enabled = false;
            config.Video.AspectRatio = AspectRatio.Fill;
            config.Video.BackgroundColor = System.Windows.Media.Colors.DarkGray;
            config.Player.AutoPlay = true;
            Player = new Player(config);
            _Player.OpenCompleted += _Player_OpenCompleted;
            Player.OpenAsync(url);
            
        }


        private void _Player_OpenCompleted(object sender, OpenCompletedArgs e)
        {
            var player = sender as Player;
            if (player == null) return;
            
            if (!e.Success)
            {
                RetryConnection(player, e.Url);
            }
        }

        private void RetryConnection(Player player, string url)
        {
            player.OpenAsync(url);
        }

        internal void Stop()
        {
            ReceivedDataSource = null;
        }


        internal void AlertEventFromEdgeServer(Dictionary<EventInfo, System.Drawing.Rectangle> positionPair)
        {
            foreach(var pair in positionPair)
            {
                _MainVM.SetMetadataLog(pair.Key.EventStatus, pair.Key.ClassID, pair.Key.EventID, pair.Key.EventType, pair.Key.RoiInfo.RoiName, pair.Value);   
            }
        }

        internal List<ROIDot> GetRange(DrawingType type)
        {
            if (type == DrawingType.All)
            {
                return new List<ROIDot>
                {
                new ROIDot { X = 0, Y = 0},
                new ROIDot { X = 1.0, Y = 0},
                new ROIDot { X = 1.0, Y = 1},
                new ROIDot { X = 0, Y = 1},
                };
            }


            // 임시
            return new List<ROIDot>
                {
                new ROIDot { X = 0, Y = 0},
                new ROIDot { X = 1.0, Y = 0},
                new ROIDot { X = 1.0, Y = 1},
                new ROIDot { X = 0, Y = 1},
                };
        }
    }
}
