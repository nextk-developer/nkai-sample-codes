﻿
using CommunityToolkit.Mvvm.ComponentModel;
using FlyleafLib;
using FlyleafLib.MediaPlayer;
using System;
using System.Numerics;
using System.Security.Policy;
using System.Windows.Shapes;

namespace NKAPISample.ViewModels
{
    public partial class VideoViewModel : ObservableObject
    {
        private Player _Player;
        private MainViewModel _MainVM;

        public Player Player { get => _Player; set => SetProperty(ref _Player, value); }

        public VideoViewModel(MainViewModel mainVM)
        {
            _MainVM = mainVM;
            //_Player = Build();
        }
        private Player Build()
        {

            var config = new Config() { };

            config.Demuxer.BufferDuration = TimeSpan.FromMilliseconds(0).Ticks; // Reduces RAM as the demuxer will not buffer large number of packets
            config.Decoder.MaxVideoFrames = 2; // Reduces VRAM as video decoder will not keep large queues in VRAM (should be tested for smooth video playback, especially for 4K)
            config.Decoder.VideoThreads = 2; // Reduces VRAM/GPU (should be tested for smooth video playback, especially for 4K)
                                             // Consider using lower quality streams on normal screen and higher quality on fullscreen (if available)
            config.Demuxer.MaxErrors = 1;
            config.Demuxer.ReadTimeout = TimeSpan.FromSeconds(3).Ticks;
            config.Demuxer.OpenTimeout = TimeSpan.FromSeconds(3).Ticks;
            config.Demuxer.CloseTimeout = TimeSpan.FromSeconds(3).Ticks;
            config.Demuxer.SeekTimeout = TimeSpan.FromSeconds(3).Ticks;                
            config.Decoder.ShowCorrupted = true;


            config.Audio.Enabled = false;
            config.Subtitles.Enabled = false;
            config.Video.AspectRatio = AspectRatio.Fill;

            var player = new Player(config) { };
            return player;
        }

        //private Player Build(string url, bool live)
        //{
        //    if (live)
        //        _Player.Config.Player.MaxLatency = TimeSpan.FromMilliseconds(500).Ticks;
        //    _Player.OpenAsync(url);
            
        //}

        internal void Start()
        {
            
        }
    }
}
