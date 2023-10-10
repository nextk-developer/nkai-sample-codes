using NKAPIService.API.Channel;
using NKAPIService.API.ComputingNode;

namespace NK_API_Test.SingleApiTests
{
    internal class Test_Record_days : TestBase
    {
        internal async Task RecordDays()
        {
            while (true)
            {
                var firstCN = await service.Requset(new RequestGetComputingNode()
                {

                }) as ResponseGetComputingNode;

                if (firstCN != null)
                {
                    var response = await service.Requset(new RequestRecordDays()
                    {
                        NodeId = firstCN.Node.NodeId

                    }) as ResponseRecordDays;
                }
            }
        }
    }
}
