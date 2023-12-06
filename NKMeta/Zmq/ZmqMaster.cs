using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using PredefineConstant;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NKMeta.Zmq
{
    public delegate void ReceivedStatistics(Dictionary<DateTime, List<StatisticsMeta>> smByDate);
    public delegate void ReceivedRecordDate(Dictionary<string, List<string>> media, Dictionary<string, List<string>> meta);
    public delegate void ReceivedSensor(SensorMeta sensorMeta);
    public class ZmqMaster
    {
        const string StatisticsTopic = "statistics";
        const string RecordDataTopic = "record-date";
        const string SensorTopic = "sensor";


        public string TargetHost { get; }
        public int TargetPort { get; }
        public int LinkPort { get; }
        public DateTime LastRecivedTime { get; private set; }
        public DateTime LinkLastRecivedTime { get; private set; }
        private List<Task> Workers { get; set; } = new();

        private readonly CancellationTokenSource _cts;

        public bool IsConnected { get; set; } = false;

        public event ReceivedStatistics OnReceivedStatistics;
        public event ReceivedRecordDate OnReceivedRecordDate;
        public event ReceivedSensor OnReceivedSensor;

        public ZmqMaster(string host, int port)
        {
            TargetHost = host;
            TargetPort = port;
            LinkPort = port + 1;
            _cts = new CancellationTokenSource();
        }

        public void StartTask()
        {
            const int default_delay_ms = 10;
            Workers.Add(Task.Run(() =>
            {
                try
                {
                    while (!_cts.IsCancellationRequested)
                    {
                        using (var subSocket = new SubscriberSocket())
                        {
                            subSocket.Connect($"tcp://{TargetHost}:{TargetPort}");
                            subSocket.Subscribe(StatisticsTopic);
                            subSocket.Subscribe(RecordDataTopic);

                            LastRecivedTime = DateTime.UtcNow;

                            try
                            {
                                while (!_cts.IsCancellationRequested)
                                {
                                    if (subSocket.TryReceiveFrameString(out string receivedTopic))
                                    {
                                        if (subSocket.TryReceiveFrameString(out string receivedMsg))
                                        {
                                            if (receivedTopic.Equals(StatisticsTopic))
                                            {
                                                OnReceivedStatistics?.Invoke(JsonConvert.DeserializeObject<Dictionary<DateTime, List<StatisticsMeta>>>(receivedMsg));
                                            }
                                            else if (receivedTopic.Equals(RecordDataTopic))
                                            {
                                                var tmp = JsonConvert.DeserializeObject<RecordDate>(receivedMsg);
                                                OnReceivedRecordDate?.Invoke(tmp.MediaRecordDays, tmp.MetaRecordDays);
                                            }

                                            LastRecivedTime = DateTime.UtcNow;
                                            IsConnected = true;
                                        }
                                    }

                                    // 마지막 수신 시간과 3초 이상 차이나면 재연결 
                                    if ((DateTime.UtcNow - LastRecivedTime).TotalSeconds > 3)
                                    {
                                        IsConnected = false;
                                        Console.WriteLine("Retry Connect ZmqMaster Main Task");
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
            }));

            Workers.Add(Task.Run(() =>
            {
                try
                {
                    while (!_cts.IsCancellationRequested)
                    {
                        using (var subSocket = new SubscriberSocket())
                        {
                            subSocket.Connect($"tcp://{TargetHost}:{LinkPort}");
                            subSocket.Subscribe(SensorTopic);

                            LinkLastRecivedTime = DateTime.UtcNow;

                            try
                            {
                                while (!_cts.IsCancellationRequested)
                                {
                                    if (subSocket.TryReceiveFrameString(out string receivedTopic))
                                    {
                                        if (subSocket.TryReceiveFrameString(out string receivedMsg))
                                        {
                                            var tmp = JsonConvert.DeserializeObject<SensorMeta>(receivedMsg);
                                            if (tmp != null && tmp.Sensors != null)
                                                OnReceivedSensor?.Invoke(tmp);

                                            LinkLastRecivedTime = DateTime.UtcNow;
                                        }
                                    }

                                    // 마지막 수신 시간과 3초 이상 차이나면 재연결 
                                    if ((DateTime.UtcNow - LinkLastRecivedTime).TotalSeconds > 3)
                                    {
                                        Console.WriteLine("Retry Connect Link Task");
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
            }));
        }

        public void StopTask()
        {
            if (_cts == null) return;

            _cts.Cancel();
            int timeoutMs = 1000 * 5;
            if (!Task.WaitAll(Workers.ToArray(), timeoutMs))
                Debug.WriteLine($"${nameof(ZmqMaster)} Stop task Timeout");
        }
    }

    public class RecordDate
    {
        [JsonProperty("mediaRecord")]
        public Dictionary<string, List<string>> MediaRecordDays { get; set; }
        [JsonProperty("metaRecord")]
        public Dictionary<string, List<string>> MetaRecordDays { get; set; }
    }
}