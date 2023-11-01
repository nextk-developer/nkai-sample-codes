using NKAPIService.API.Channel;
using NKAPIService.API.ComputingNode;

namespace NK_API_Test.SingleApiTests
{
    internal class Test_Record_days : TestBase
    {
        internal async Task Test(int repeat)
        {
            for (int i = 0; i < repeat; i++)
            {
                await Console.Out.WriteLineAsync($"Test {i}");

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
