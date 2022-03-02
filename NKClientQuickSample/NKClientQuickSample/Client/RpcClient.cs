using Grpc.Core;
using System;
using System.Text;
using System.Threading.Tasks;
using VAMetaService;

namespace NKClientQuickSample.Client
{
    public class RpcClient
    {
        public string _host { get; set; }
        public int _port { get; set; }
        public event EventHandler<FrameMetaData> ResponseMetaHandler;

        private Channel _chClient;
        private bool _IsRunning;

        public bool IsChangedCheck(string host, int port)
        {
            //if(host == _host &&  port == _port)
            //{
            //    return false;
            //}

            Stop();
            return true;
        }
        public void Start(string host, int port)
        {
            _host = host;
            _port = port;
            _chClient = new Channel($"{host}:{port}", ChannelCredentials.Insecure);
            Task.Run(async () =>
            {
                while (true)
                {
                    switch (_chClient.State)
                    {
                        case ChannelState.Idle:
                        case ChannelState.Shutdown:
                            {
                                _IsRunning = false;
                                //await _chClient.ConnectAsync();
                                _chClient.ConnectAsync().Wait(50);
                            }
                            break;
                        case ChannelState.Ready:
                            if (_IsRunning == false)
                            {
                                StreamingProc();
                            }
                            break;
                        case ChannelState.Connecting:
                        default:
                            break;
                    }
                    await Task.Delay(new TimeSpan(0, 0, 0, 0, 1));
                }
            });
        }
        public void Stop()
        {
            if (_IsRunning)
                _IsRunning = false;
        }
        public bool IsRunning()
        {
            return _IsRunning;
        }
        private void StreamingProc()
        {
            _IsRunning = true;
            var VASC = new VAMetaService.VAMetaService.VAMetaServiceClient(_chClient);
            var streamCall = VASC.GetVAMetaStream(new Google.Protobuf.WellKnownTypes.Empty());

            Task.Run(async () =>
            {
                try
                {
                    while (_IsRunning)
                    {
                        if (_chClient.State != ChannelState.Ready) continue;

                        await foreach (FrameMetaData item in streamCall.ResponseStream.ReadAllAsync())
                        {
                            if (item.EventList.Count > 0 || item.ObjectList.Count > 0)
                            {
                                ResponseMetaHandler?.Invoke(this, item);
                            }
                        }
                    }
                }
                catch
                {

                }
                _IsRunning = false;
            });
        }

    }
}
