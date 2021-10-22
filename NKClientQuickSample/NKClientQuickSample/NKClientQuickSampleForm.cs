using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VAMetaService;

namespace NKClientQuickSample
{
    public partial class NKClientQuickSampleForm : Form
    {
        private Client.RpcClient _rpcClient;
        private Client.RestClient _restClient;
        private Video.CaptureFrame _captureFrame;
        private FrameMetaData _currentVAMeta;
        private string _currentDrawChannelID;
        public NKClientQuickSampleForm()
        {
            InitializeComponent();

            SetDefualt();
            CreateClients();
            ChangedRpcSetting(tbTCNodeIp.Text, Convert.ToInt32(tbTCRpcPort.Text));
        }
        private void ReceivedDrawFrame(object sender, System.Drawing.Bitmap e)
        {
            if (e == null) return;
            if (_currentVAMeta == null) return;

                this.Invoke(new Action(delegate ()
            {
                using (Graphics grap = Graphics.FromImage(e))
                {
                    if (_currentVAMeta != null)
                    {
                        Pen eventPen = new Pen(Brushes.Red, 8);
                        Pen ObjectPen = new Pen(Brushes.Green, 6);
                        Font ff = new Font("Arial", 40, FontStyle.Bold);
                        SolidBrush sb = new SolidBrush(Color.White);
                        if (_currentVAMeta.ObjectList != null)
                        {
                            foreach (var obj in _currentVAMeta.ObjectList)
                            {
                                int x = (int)(e.Width * obj.Box.X);
                                int y = (int)(e.Height * obj.Box.Y);
                                int w = (int)(e.Width * obj.Box.Width);
                                int h = (int)(e.Height * obj.Box.Height);
                                Rectangle drawBox = new Rectangle(x, y, w, h);
                                grap.DrawRectangle(ObjectPen, drawBox);
                            }
                        }
                        if (_currentVAMeta.EventList != null)
                        {
                            foreach (var evt in _currentVAMeta.EventList)
                            {
                                var segment = evt.Segmentation;
                                int x = (int)(e.Width * segment.Box.X);
                                int y = (int)(e.Height * segment.Box.Y);
                                int w = (int)(e.Width * segment.Box.Width);
                                int h = (int)(e.Height * segment.Box.Height);
                                Rectangle drawBox = new Rectangle(x, y, w, h);
                                grap.DrawRectangle(eventPen, drawBox);
                                string putDrawString = segment.Label.ToString() + $"\r\n(ID:{evt.Id})";
                                grap.DrawString(putDrawString, ff, sb, x, y, new StringFormat());
                            }
                        }
                    }

                    pbDrawBox.Image = e;
                }
            }));
        }

        #region Client
        private void CreateClients()
        {
            //rest api
            _restClient = new Client.RestClient();
            _restClient.ResponseAPIHandler += ReceivedApiResponse;
            _restClient.ResponseUidHandler += ReceivedApiGetUID;

            //rpc
            _rpcClient = new Client.RpcClient();
            _rpcClient.ResponseMetaHandler += ReceivedStreamingMeta;

            //draw 
            _captureFrame = new Video.CaptureFrame();
            _captureFrame.ResponseDrawFrameHandler += ReceivedDrawFrame;
        }
        private void SetDefualt()
        {
            tbTCbaseUri.Text = "http://192.168.0.36:9000";
            tbTCNodeIp.Text = "192.168.0.36";
            //tbTCbaseUri.Text = "http://172.16.0.153:9000";
            //tbTCNodeIp.Text = "nextk.synology.me";
            tbTChttpPort.Text = "8880";
            tbTCRpcPort.Text = "33300";
        }
        private void ChangedRpcSetting(string host, int port)
        {
            if (_rpcClient.IsChangedCheck(host, port))
            {
                _rpcClient.Start(host, port);
            }
        }
        private void SendAPI(string uri, string payload)
        {
            try
            {
                //rpc 정보가 변경된 경우
                ChangedRpcSetting(tbTCNodeIp.Text, Convert.ToInt32(tbTCRpcPort.Text));
                //Send API
                _restClient.RequestTo(uri, payload);
            }
            catch (Exception e)
            {

            }
        }

        private void ReceivedApiGetUID(object target, string uid)
        {
            //마지막 생성된 Id 표출을 위함
            this.Invoke(new Action(delegate ()
            {
                string target_name = (string)target;
                if (target_name == "nodeId")
                {
                    tbLastNodeId.Text = uid;
                }
                else if (target_name == "channelId")
                {
                    SetChannelUID(uid);
                    tbLastChannelId.Text = uid;
                    lbDrawFrameChannelId.Text = uid;
                }
                else if (target_name == "roiId")
                {
                    tbLastRoiId.Text = uid;
                }
            }));
        }
        private string GetChannelURI(string ChannelUID)
        {
            string get_command  = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.CH_GET)}";
            string get_json     = Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestChannel
            {
                nodeId = tbLastNodeId.Text,
                channelId = ChannelUID
            }, Newtonsoft.Json.Formatting.Indented);

            return  _restClient.GetRequestAsync(get_command, get_json);
        }
        private void SetChannelUID(string ChannelUID)
        {
            string response = GetChannelURI(ChannelUID);
            string URI = Newtonsoft.Json.JsonConvert.DeserializeObject<Test.ResponseChannelInfo>(response).intputUri;
            if (URI != null)
            {
                //카메라 생성시마다 영상 변경
                if (_captureFrame != null)
                {
                    //_captureFrame.Stop();
                    _captureFrame.StartAndUpdate(URI);
                }
                _currentDrawChannelID = ChannelUID;
            }
        }
        private void ReceivedApiResponse(object sender, string response)
        {
            //API 응답 표출
            this.Invoke(new Action(delegate ()
            {
                rtbResponse.Text = $"[{DateTime.Now.ToString()}]\r\n" + response;
            }));
        }
        private void ReceivedStreamingMeta(object sender, FrameMetaData response)
        {
            this.Invoke(new Action(delegate ()
            {
                if (response.ChannelId == _currentDrawChannelID)
                {
                    _currentVAMeta = response;

                    if (response.EventList != null)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.AppendLine($"[{DateTime.Now.ToString("yy/MM/dd HH:mm:ss:ff")}]");
                        foreach (EventInfo ei in response.EventList)
                        {
                            builder.AppendLine($"Object ID:{ei.Id}({ei.Segmentation.Label}/{ei.State.ToString()})");
                        }
                        rtbRpcResponse.Text = builder.ToString();
                    }
                }
                else
                {
                    if (response.EventList != null)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.AppendLine($"[{response.ChannelId} >>> {DateTime.Now.ToString("yy/MM/dd HH:mm:ss:ff")}]");
                        foreach (EventInfo ei in response.EventList)
                        {
                            builder.AppendLine($"Object ID:{ei.Id}({ei.Segmentation.Label}/{ei.State.ToString()})");
                        }
                        rtbRpcOtherResponse.Text = builder.ToString();
                    }
                    //Console.WriteLine(response.ChannelId);
                }
            }));
        }
        #endregion

        #region 컴퓨팅 노드 API
        private void TestAPICreateComputeNode()
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.NODE_CREATE)}";
            rtbPayload.Text = Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestAddCompute
            {
                host = tbTCNodeIp.Text,
                httpPort = Convert.ToInt32(tbTChttpPort.Text),
                rpcPort = Convert.ToInt32(tbTCRpcPort.Text),
                nodeName = "TEST_COMPUTE_NODE"
            }, Newtonsoft.Json.Formatting.Indented);
        }
        private void TestAPIRemoveComputeNode()
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.NODE_REMOVE)}";
            rtbPayload.Text = Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestCompute
            {
                nodeId = tbLastNodeId.Text
            }, Newtonsoft.Json.Formatting.Indented);
        }
        private void TestAPIGetComputeNode()
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.NODE_GET)}";
            rtbPayload.Text = Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestCompute
            {
                nodeId = tbLastNodeId.Text
            }, Newtonsoft.Json.Formatting.Indented);
        }

        #endregion
        #region 채널 API
        private void TestAPIRegistChannel()
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.CH_REGIST)}";
            rtbPayload.Text = Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestAddChannel
            {
                nodeId = tbLastNodeId.Text,
                channelName = "TEST_CHANNEL_NAME",
                //rtsp test
                inputUri = "rtsp://admin:enter2424@192.168.0.71/stream1"
                //video test
                //inputUri = "falldown_1.mp4",
                //inputType = "video",
                //inputUri = "rtsp://nextk.synology.me/vod/fa_test"
            }, Newtonsoft.Json.Formatting.Indented);
        }
        private void TestAPIRemoveChannel()
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.CH_REMOVE)}";
            rtbPayload.Text = Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestChannel
            {
                nodeId = tbLastNodeId.Text,
                channelId = tbLastChannelId.Text
            }, Newtonsoft.Json.Formatting.Indented);
        }
        private void TestAPIGetChannel()
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.CH_GET)}";
            rtbPayload.Text = Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestChannel
            {
                nodeId = tbLastNodeId.Text,
                channelId = tbLastChannelId.Text
            }, Newtonsoft.Json.Formatting.Indented);
        }
        #endregion
        #region ROI API
        private void TestAPICreateRoi()
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.ROI_CREATE)}";
            rtbPayload.Text = Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestAddROI
            {
                nodeId = tbLastNodeId.Text,
                channelId = tbLastChannelId.Text,
                roiName = "ROI_NAME",
                description = "Loitering Event",
                roiDots = new List<Test.RoiDot>
                {
                    new Test.RoiDot { X = 0, Y = 0},
                    new Test.RoiDot { X = 1, Y = 0},
                    new Test.RoiDot { X = 1, Y = 1},
                    new Test.RoiDot { X = 0, Y = 1},
                }
            }, Newtonsoft.Json.Formatting.Indented);
        }
        private void TestAPIRemoveRoi()
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.ROI_REMOVE)}";
            rtbPayload.Text = Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestROI
            {
                nodeId = tbLastNodeId.Text,
                channelId = tbLastChannelId.Text,
                roiId = tbLastRoiId.Text,
            }, Newtonsoft.Json.Formatting.Indented);
        }
        private void TestAPIGetRoi()
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.ROI_GET)}";
            rtbPayload.Text = Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestROI
            {
                nodeId = tbLastNodeId.Text,
                channelId = tbLastChannelId.Text,
                roiId = tbLastRoiId.Text,
            }, Newtonsoft.Json.Formatting.Indented);
        }
        #endregion

        #region Button & Radio
        private void btnAPIConnection_Click(object sender, EventArgs e)
        {
            SendAPI(tbUri.Text, rtbPayload.Text);
        }
        private void rbAddCompute_CheckedChanged(object sender, EventArgs e)
        {
            TestAPICreateComputeNode();
        }
        private void rbRemoveCompute_CheckedChanged(object sender, EventArgs e)
        {
            TestAPIRemoveComputeNode();
        }
        private void rbGetCompute_CheckedChanged(object sender, EventArgs e)
        {
            TestAPIGetComputeNode();
        }
        private void rbAddChannel_CheckedChanged(object sender, EventArgs e)
        {
            TestAPIRegistChannel();
        }
        private void rbRemoveChannel_CheckedChanged(object sender, EventArgs e)
        {
            TestAPIRemoveChannel();
        }
        private void rbGetChannel_CheckedChanged(object sender, EventArgs e)
        {
            TestAPIGetChannel();
        }
        private void rbAddROI_CheckedChanged(object sender, EventArgs e)
        {
            TestAPICreateRoi();
        }
        private void rbRemoveROI_CheckedChanged(object sender, EventArgs e)
        {
            TestAPIRemoveRoi();
        }
        private void rbGetRoi_CheckedChanged(object sender, EventArgs e)
        {
            TestAPIGetRoi();
        }
        #endregion

        private void textbox_KeyPress_OnlyNumberic(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
