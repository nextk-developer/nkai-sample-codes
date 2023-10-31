// See https://aka.ms/new-console-template for more information
using NKAPIService;
using NKAPIService.API.Channel;
using NKAPIService.API.ComputingNode;
using NKAPIService.API.VideoAnalysisSetting;
using NKMeta.Zmq;
using System.Diagnostics;

Console.WriteLine("Hello, World!");

var uri = new Uri($"http://127.0.0.1:8880");
var service = APIService.Build().SetUrl(uri);

int index = 0;
int channelCount = 4;
int roiCount = 4;

while (true)
{
    Console.WriteLine($"################# Processing count {index++} #################");
    Stopwatch stopwatch = Stopwatch.StartNew();

    var firstCN = await service.Requset(new RequestGetComputingNode()
    {

    }) as ResponseGetComputingNode;

    if (firstCN != null && firstCN.Node != null && !string.IsNullOrEmpty(firstCN.Node.NodeId))
    {
        await service.Requset(new RequestRemoveComputingNode()
        {
            NodeId = firstCN.Node.NodeId
        });
    }


    stopwatch.Restart();
    var responseCreateCN = await service.Requset(new RequestCreateComputingNode()
    {
        Host = "localhost",
        NodeName = "test",
        // 고정값
        License = "oLabps19pw07ZOcig1/eej7O6zMtn8N9iMO1mRw2OiydMdIBldoQPJyL572pP90i"
    }) as ResponseCreateComputingNode;
    Console.WriteLine($"{nameof(RequestCreateComputingNode)}: {stopwatch.ElapsedMilliseconds}ms");

    if (responseCreateCN != null)
    {
        List<string> channelId = new();

        Dictionary<string, List<string>> roi_pair = new();
        Dictionary<string, ObjectMetaClient> meta_pair = new();

        for (int i = 0; i < channelCount; i++)
        {
            // add camera
            var responseCameraRegister = await service.Requset(new RequestRegisterChannel()
            {
                NodeId = responseCreateCN.NodeId,
                ChannelName = "",
                GroupName = "",
                Description = "",
                InputUrl = "rtsp://211.198.128.30/vod/fa_test",
                InputType = NKAPIService.API.Channel.Models.InputType.SRC_IPCAM_NORMAL,
            }) as ResponseRegisterChannel;

            if (responseCameraRegister == null) continue;

            channelId.Add(responseCameraRegister.ChannelId);

            Console.WriteLine($"add : {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Restart();


            var schedule = new List<List<int>>();

            for (int w = 0; w < 7; w++)
            {
                List<int> hours = new();
                for (int h = 0; h < 24; h++)
                    hours.Add(h);

                schedule.Add(hours);
            }

            var responseVASchedule = await service.Requset(new RequestVaSchedule()
            {
                ChannelID = responseCameraRegister.ChannelId,
                NodeId = responseCreateCN.NodeId,
                Schedule = schedule
            }) as ResponseVaSchedule;

            Console.WriteLine($"va schedule : {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Restart();

            for (int j = 0; j < roiCount; j++)
            {
                var responsecreateroi = await service.Requset(new RequestCreateROI()
                {
                    NodeId = responseCreateCN.NodeId,
                    ChannelID = responseCameraRegister.ChannelId,
                    EventType = PredefineConstant.Enum.Analysis.EventType.IntegrationEventType.Intrusion,
                    ROIDots = new List<NKAPIService.API.VideoAnalysisSetting.Models.ROIDot>()
                {
                    new NKAPIService.API.VideoAnalysisSetting.Models.ROIDot(){X = 0,Y = 0 },
                    new NKAPIService.API.VideoAnalysisSetting.Models.ROIDot(){X = 1,Y = 0 },
                    new NKAPIService.API.VideoAnalysisSetting.Models.ROIDot(){X = 1,Y = 1 },
                    new NKAPIService.API.VideoAnalysisSetting.Models.ROIDot(){X = 0,Y = 1 },
                },
                    EventFilter = new NKAPIService.API.VideoAnalysisSetting.Models.EventFilter()
                    {
                        ObjectsTarget = new List<PredefineConstant.Enum.Analysis.ClassId>() { PredefineConstant.Enum.Analysis.ClassId.Person },

                    }
                }) as ResponseCreateROI;

                if (responsecreateroi != null)
                {
                    if (!roi_pair.ContainsKey(responseCameraRegister.ChannelId))
                        roi_pair.Add(responseCameraRegister.ChannelId, new List<string>());

                    roi_pair[responseCameraRegister.ChannelId].Add(responsecreateroi.ROIID);

                    if (roi_pair[responseCameraRegister.ChannelId].Count == 1)
                    {
                        var responseVA = await service.Requset(new RequestControl()
                        {
                            ChannelIDs = new() { responseCameraRegister.ChannelId },
                            NodeId = responseCreateCN.NodeId,
                            Operation = NKAPIService.API.VideoAnalysisSetting.Models.Operations.VA_START,
                        }) as ResponseControl;

                        stopwatch.Restart();
                        meta_pair.Add(responseCameraRegister.ChannelId,
                            new ObjectMetaClient(responseCreateCN.NodeId, responseCameraRegister.ChannelId, responseCameraRegister.ChannelId, $"{responseVA.SourceIp}:{responseVA.SourcePort}")
                            );
                        meta_pair[responseCameraRegister.ChannelId].StartTask();
                        Console.WriteLine($"start va: {stopwatch.ElapsedMilliseconds}ms");
                    }

                    Console.WriteLine($"create roi({responsecreateroi.ROIID}) : {stopwatch.ElapsedMilliseconds}ms");
                    stopwatch.Restart();
                }
            }
        }


        await Task.Delay(500);


        // delete camera
        stopwatch.Restart();
        foreach (var ch in channelId)
        {
            if (roi_pair.ContainsKey(ch))
            {
                foreach (var roi in roi_pair[ch])
                {
                    var responseremoveroi = await service.Requset(new RequestRemoveROI()
                    {
                        NodeId = responseCreateCN.NodeId,
                        ChannelID = ch,
                        ROIIds = new List<string>() { roi }
                    });

                    Console.WriteLine($"remove roi({roi}): {stopwatch.ElapsedMilliseconds}ms");
                    stopwatch.Restart();
                }
            }

            await service.Requset(new RequestControl()
            {
                NodeId = responseCreateCN.NodeId,
                ChannelIDs = new List<string>() { ch },
                Operation = NKAPIService.API.VideoAnalysisSetting.Models.Operations.VA_STOP,
            });
            Console.WriteLine($"stop va: {stopwatch.ElapsedMilliseconds}ms");
            if (meta_pair.ContainsKey(ch))
            {
                Console.WriteLine($"ReceivedMetaTotal: {meta_pair[ch].ReceivedMetaTotal}");
                meta_pair[ch].StopTask();
                meta_pair.Remove(ch);
            }


            stopwatch.Restart();
            await service.Requset(new RequestRemoveChannel()
            {
                ChannelId = ch,
                NodeId = responseCreateCN.NodeId,
            });



            Console.WriteLine($"del : {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Restart();
        }

        await Task.Delay(100);


        stopwatch.Restart();
        var reseponseDeleteCN = await service.Requset(new RequestRemoveComputingNode()
        {
            NodeId = responseCreateCN.NodeId
        }) as ResponseRemoveComputingNode;
        Console.WriteLine($"{nameof(RequestRemoveComputingNode)}: {stopwatch.ElapsedMilliseconds}ms");

        if (reseponseDeleteCN == null)
            ;// throw new Exception("reseponseDeleteCN == null");
    }
}



Console.WriteLine("Bye, World!");
