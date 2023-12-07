using FlyleafLib;
using FlyleafLib.MediaPlayer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKAPISample.Models
{
    public enum StreamingType { Main, Sub }
    public enum ConnectedStatus { Connected, Waiting, Disconnected }
    public class MediaComponent
    {

        private object DisposeLock = new();
        private bool IsDispose = false;

        private ConcurrentDictionary<int, Player> _ReferencePlayers { get; } = new();
        public IReadOnlyList<Player> ReferencePlayers
        {
            get
            {
                return _ReferencePlayers.Values.ToList();
            }
        }

        public ConnectedStatus IsConnected => !ReferencePlayers.Any() ? ConnectedStatus.Waiting
            : ReferencePlayers.First().IsPlaying ? ConnectedStatus.Connected : ConnectedStatus.Disconnected;

        public Player MediaSource
        {
            get
            {
                var player = Build(URL, IsLive);

                _ReferencePlayers[player.PlayerId] = player;

                player.OpenCompleted += Player_OpenCompleted;
                player.PlaybackStopped += Player_PlaybackStopped;
                return player;
            }
        }

        private void Player_PlaybackStopped(object sender, PlaybackStoppedArgs e)
        {
            
        }

        private void Player_OpenCompleted(object sender, OpenCompletedArgs e)
        {
            
        }

        public bool IsLive { get; }
        public string URL { get; }
        public ChannelComponent ChannelComponent { get; }
        public StreamingType StreamingType { get; }
        public MediaComponent(ChannelComponent channelComponent, StreamingType st, string url, bool isLive = true)
        {
            ChannelComponent = channelComponent;
            StreamingType = st;
            this.URL = url;
            this.IsLive = isLive;
        }


        private static Player Build(string url, bool live)
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
            if (live)
                config.Player.MaxLatency = TimeSpan.FromMilliseconds(500).Ticks;
            config.Decoder.ShowCorrupted = true;


            config.Audio.Enabled = false;
            config.Subtitles.Enabled = false;
            config.Video.AspectRatio = AspectRatio.Fill;

            var player = new Player(config) { };

            player.OpenAsync(url);

            return player;
        }


        public void Dispose()
        {
            lock (DisposeLock)
                IsDispose = true;

            _ReferencePlayers.Values.ToList().ForEach(p => p.Dispose());
            _ReferencePlayers.Clear();
        }
    }
}
