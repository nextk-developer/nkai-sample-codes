
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using FlyleafLib;
using FlyleafLib.MediaPlayer;
using NKAPISample.Models;
using NKMeta;
using PredefineConstant;
using PredefineConstant.Enum.Analysis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;

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


        private bool _IsDrawingMode = false;

        public bool IsDrawingMode { get => _IsDrawingMode; set => SetProperty(ref _IsDrawingMode, value); }
        private List<RoiPoint> _currentRange = new();


        public DrawingType CurrentDrawingType { get; private set; }
        private List<RoiModel> _RoiList;
        public List<RoiModel> RoiList { get => _RoiList; private set => SetProperty(ref _RoiList, value); }

        public VideoViewModel()
        {
            _MainVM = Ioc.Default.GetRequiredService<MainViewModel>();
            _MainVM.VAStopped = Stop;
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
            _Player.PlaybackStopped += _Player_PlaybackStopped;
            Player.OpenAsync(url);
            
        }

        private void _Player_PlaybackStopped(object sender, PlaybackStoppedArgs e)
        {
            var player = sender as Player;
            if (player == null || !player.CanPlay) return;

            RetryConnection(player, _MainVM.CurrentNode.CurrentChannel.MediaUrl);
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
                _MainVM.SetMetadataLog(pair.Key.EventStatus, pair.Key.ClassID, pair.Key.EventID, pair.Key.EventType, pair.Value);   
            }
        }

        internal List<RoiPoint> GetRange()
        {
            return _currentRange;
        }

        internal void SetDrawingMode(DrawingType type)
        {
            CurrentDrawingType = type;
            if (_IsDrawingMode)
                _IsDrawingMode = false;

            IsDrawingMode = true;
        }

        internal void SetRange(List<RoiPoint> currentRange)
        {
            IsDrawingMode = false;
            _currentRange = currentRange;
        }

        internal void ClearRange()
        {
            _currentRange.Clear();
        }



        internal void AddRoiRange(List<RoiModel> items)
        {
            RoiList = items;
        }
    }
}
