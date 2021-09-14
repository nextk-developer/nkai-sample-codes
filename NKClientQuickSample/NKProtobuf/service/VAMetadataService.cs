using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using VAMetaService;

namespace NKProtobuf
{
    public class VAMetadataService : VAMetaService.VAMetaService.VAMetaServiceBase
    {
        private ConcurrentDictionary<string, DateTime> _timeoutFps;
        private readonly int _interval;
        private readonly string _name;
        private bool _isrunning;
        private ConcurrentDictionary<string, ConcurrentQueue<FrameMetaData>> _dicQueueMetaMsg;
        public VAMetadataService(string name, int interval)
        {
            _name = name;
            _interval = interval;
            _dicQueueMetaMsg = new ConcurrentDictionary<string, ConcurrentQueue<FrameMetaData>>();
        }


        public int EnqueueMeta(FrameMetaData msg)
        {
            if (_isrunning)
            {
                if(_dicQueueMetaMsg.Count() == 0)
                {
                    _isrunning = false;
                }

                foreach (var queue in _dicQueueMetaMsg)
                {
                    queue.Value.Enqueue(msg);
                }
                return 0;
            }
            return -1;
        }

        public void StopTask()
        {
            _isrunning = false;
        }

        public bool IsRunning()
        {
            return _isrunning;
        }
        public override async Task GetVAMetaStream(Empty e, IServerStreamWriter<FrameMetaData> responseStream, ServerCallContext context)
        {
            string host = context.Peer;
            if(_dicQueueMetaMsg.ContainsKey(host) == false)
                _dicQueueMetaMsg.TryAdd(host, new ConcurrentQueue<FrameMetaData>());
            Console.WriteLine($"Connection gRPC Client host : {host}");

            try
            {
                _isrunning = true;
                while (true)
                {
                    if (_dicQueueMetaMsg[host].TryDequeue(out FrameMetaData msg))
                    {
#if DEBUG
                        foreach (var ev in msg.EventList)
                        {
                            Console.WriteLine($"ID : {ev.Id}, Segmentation : {ev.Segmentation}");
                        }
#endif
                        await responseStream.WriteAsync(msg);
                    }
                }
            }
            catch
            {
                if(_dicQueueMetaMsg.ContainsKey(host))
                    _dicQueueMetaMsg.Remove(host, out ConcurrentQueue<FrameMetaData> curr);

                Console.WriteLine($"Failed Streaming to gRPC Client host : {host}");
            }
            await Task.Delay(10);
        }
    }
}
