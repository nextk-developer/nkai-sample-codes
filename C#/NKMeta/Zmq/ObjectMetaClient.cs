using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using PredefineConstant;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NKMeta.Zmq
{
    public class ObjectMetaClient : Utils.ComputeFps, IMetaData
    {
        public string NodeUID { get; }
        public string CameraUID { get; }
        public string CameraName { get; }

        private string _targetHost;

        public int VideoFps => Fps;

        public DateTime LastRecivedTime { get; private set; }

        public event EventHandler<ObjectMeta> OnReceivedMetaData;
        private readonly CancellationTokenSource _cts;

#if DEBUG
        public int ReceivedMetaTotal { get; private set; } = 0;
#endif

        public ObjectMetaClient(string nodeUID, string camerUID, string nickName, string targetHost)
        {
            NodeUID = nodeUID;
            CameraUID = camerUID;
            CameraName = nickName;
            _targetHost = targetHost;
            _cts = new CancellationTokenSource();
        }

        public void StartTask()
        {
            const int default_delay_ms = 10;
            Task.Run(() =>
            {
                try
                {
                    while (!_cts.IsCancellationRequested)
                    {
                        using (var subSocket = new SubscriberSocket())
                        {
                            subSocket.Connect($"tcp://{_targetHost}");
                            subSocket.Subscribe(CameraUID);

                            LastRecivedTime = DateTime.UtcNow;

                            try
                            {
                                while (!_cts.IsCancellationRequested)
                                {
                                    if (subSocket.TryReceiveFrameString(out string receivedTopic))
                                    {
                                        if (subSocket.TryReceiveFrameString(out string receivedMsg))
                                        {
                                            ComputedFps();
                                            var objMeta = JsonConvert.DeserializeObject<ObjectMeta>(receivedMsg);

#if DEBUG
                                            ReceivedMetaTotal++;
#endif

                                            if (!_cts.IsCancellationRequested)
                                            {
                                                OnReceivedMetaData?.Invoke(this, objMeta);
                                            }

                                            LastRecivedTime = DateTime.UtcNow;
                                        }
                                    }

                                    // 마지막 수신 시간과 3초 이상 차이나면 재연결 
                                    if ((DateTime.UtcNow - LastRecivedTime).TotalSeconds > 3)
                                    {
                                        break;
                                    }

                                    // 기본 대기 시간
                                    Task.Delay(default_delay_ms).Wait();
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                if (Debugger.IsAttached)
                    Debug.WriteLine("ZMQ client stop Task");
            });
        }

        public void StopTask()
        {
            _cts.Cancel();
        }
    }
}
