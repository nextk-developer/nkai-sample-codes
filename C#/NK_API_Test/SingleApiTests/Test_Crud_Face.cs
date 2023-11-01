using NKAPIService.API.ComputingNode;
using NKAPIService.API.VideoAnalysisSetting;
using System.Drawing;

namespace NK_API_Test.SingleApiTests
{
    internal class Test_Crud_Face : TestBase
    {
        internal async Task TestOnebyOne(int repeatCount)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                await Console.Out.WriteLineAsync($"Test {i}");
                var firstCN = await service.Requset(new RequestGetComputingNode()
                {

                }) as ResponseGetComputingNode;


                var bitmap = Bitmap.FromFile(@"D:\Nextk\FaceImages\1.png");

                // 비트맵을 바이트 배열로 변환합니다.
                byte[] byteArray;

                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    byteArray = stream.ToArray();
                }


                var uuid = Guid.NewGuid().ToString().GetHashCode().ToString("x");
                var responseRegitFace = await service.Requset(new RequestRegisterFaceDB()
                {
                    NodeId = firstCN.Node.NodeId,
                    UuId = uuid,
                    UserId = i.ToString(),
                    UserName = i.ToString(),
                    Gender = PredefineConstant.Enum.Analysis.Gender.Male,
                    Memo = i.ToString(),
                    UserAge = i,
                    Identifier = PredefineConstant.Enum.Analysis.Identifier.White,
                    FaceImages = new List<string>() { Convert.ToBase64String(byteArray) }
                });

                if (responseRegitFace.Code == NKAPIService.API.ErrorCode.SUCCESS)
                {

                    var responseDeleteFace = await service.Requset(new RequestUnRegisterFaceDB()
                    {
                        NodeId = firstCN.Node.NodeId,
                        UuId = uuid,
                    });
                }
            }
        }

        internal async Task TestNbyN(int repeatCount, int addTestCount)
        {
            for (int r = 0; r < repeatCount; r++)
            {
                await Console.Out.WriteLineAsync($"Test {r}");
                string nodeId = string.Empty;
                List<string> uuids = new();

                var firstCN = await service.Requset(new RequestGetComputingNode()
                {

                }) as ResponseGetComputingNode;

                nodeId = firstCN.Node.NodeId;


                var bitmap = Bitmap.FromFile(@"D:\Nextk\FaceImages\1.png");

                // 비트맵을 바이트 배열로 변환합니다.
                byte[] byteArray;

                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    byteArray = stream.ToArray();
                }

                for (int i = 0; i < addTestCount; i++)
                {
                    await Console.Out.WriteLineAsync($"add Test {i}");
                    var uuid = Guid.NewGuid().ToString().GetHashCode().ToString("x");
                    var responseRegitFace = await service.Requset(new RequestRegisterFaceDB()
                    {
                        NodeId = firstCN.Node.NodeId,
                        UuId = uuid,
                        UserId = i.ToString(),
                        UserName = i.ToString(),
                        Gender = PredefineConstant.Enum.Analysis.Gender.Male,
                        Memo = i.ToString(),
                        UserAge = i,
                        Identifier = PredefineConstant.Enum.Analysis.Identifier.White,
                        FaceImages = new List<string>() { Convert.ToBase64String(byteArray) }
                    });

                    if (responseRegitFace.Code == NKAPIService.API.ErrorCode.SUCCESS)
                    {
                        uuids.Add(uuid);
                    }
                }

                for (int i = 0; i < uuids.Count; i++)
                {
                    var uuid = uuids[i];
                    await Console.Out.WriteLineAsync($"remove Test {i}");
                    var responseDeleteFace = await service.Requset(new RequestUnRegisterFaceDB()
                    {
                        NodeId = nodeId,
                        UuId = uuid,
                    });
                }
            }
        }
    }
}
