using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VAMetaService;

namespace NKClientQuickSample
{
    public partial class NKClientQuickSampleForm : Form
    {
        private DateTime _vaMetaDate;
        private Client.RpcClient    _rpcClient;
        private Client.RestClient   _restClient;
        private Video.CaptureFrame  _captureFrame;
        private FrameMetaData       _currentVAMeta;
        private string              _currentDrawChannelID;

        private Pen _event_pen = new Pen(Brushes.Red, 8);
        private Pen _object_pen = new Pen(Brushes.Green, 6);
        private Pen _area_pen = new Pen(Brushes.Yellow, 6);
        private Font _draw_font = new Font("Arial", 40, FontStyle.Bold);
        private SolidBrush _solid_brush = new SolidBrush(Color.White);

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

            this.Invoke(new Action(delegate ()
            {
                try
                {
                    using (Graphics grap = Graphics.FromImage(e))
                    {
                        if (_currentVAMeta != null)
                        {
                            if (_currentVAMeta.ObjectList != null)
                            {
                                foreach (var obj in _currentVAMeta.ObjectList)
                                {
                                    int x = (int)(e.Width * obj.Box.X);
                                    int y = (int)(e.Height * obj.Box.Y);
                                    int w = (int)(e.Width * obj.Box.Width);
                                    int h = (int)(e.Height * obj.Box.Height);
                                    grap.DrawRectangle(_object_pen, new Rectangle(x, y, w, h));
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
                                    grap.DrawRectangle(_event_pen, new Rectangle(x, y, w, h));
                                    grap.DrawString(segment.Label.ToString() + $"\r\n(ID:{evt.Id})",
                                        _draw_font, _solid_brush, x, y, new StringFormat());
                                }
                            }
                        }
                        if ((DateTime.Now - _vaMetaDate).TotalMilliseconds > 2000)
                        {
                            _currentVAMeta = null;
                        }

                        //if (_listDrawRoi != null && 
                        //    grap != null &&
                        //    pbDrawBox.Image != null)
                        //{
                        //    foreach (var drawRoi in _listDrawRoi)
                        //    {
                        //        grap.DrawRectangle(_area_pen,
                        //            new Rectangle(
                        //                (int)(drawRoi[0].X * pbDrawBox.Image.Width), (int)(drawRoi[0].Y * pbDrawBox.Image.Height),
                        //                (int)(drawRoi[2].X * pbDrawBox.Image.Width) - 10, (int)(drawRoi[2].Y * pbDrawBox.Image.Height)));
                        //    }
                        //}
                    }
                }
                catch
                {
                    Console.WriteLine("Failed Load Imagee");
                }

                pbDrawBox.Image = e;
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
            _vaMetaDate = DateTime.Now;
            tbTCbaseUri.Text = "http://192.168.0.36:9000";
            tbTCNodeIp.Text = "192.168.0.36";
            //tbTCbaseUri.Text = "http://172.16.0.153:9000";
            //tbTCNodeIp.Text = "nextk.synology.me";
            tbTChttpPort.Text = "8880";
            tbTCRpcPort.Text = "33300";

            //_listDrawRoi = new List<List<Test.RoiDot>>();
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
            string URI = Newtonsoft.Json.JsonConvert.DeserializeObject<Test.ResponseChannelInfo>(response).inputUri;
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
                    _vaMetaDate = DateTime.Now;

                    if (response.EventList != null)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.AppendLine($"[{DateTime.Now.ToString("yy/MM/dd HH:mm:ss:ff")}]");
                        foreach (EventInfo ei in response.EventList)
                        {
                            builder.AppendLine($"Object ID:{ei.Id}({ei.Segmentation.Label}/{ei.State})");
                            lbClassInfo.Text = $"Class: {ei.Segmentation.Label}({ei.State})";
                            lbBoxInfo.Text = $"Box: {ei.Segmentation.Box.Width}x{ei.Segmentation.Box.Height}";
                            if (ei.State != State.Continue)
                            {
                                System.Threading.Tasks.Task.Run(() =>
                                {
                                    byte[] orgBytes = Convert.FromBase64String(ei.Base64JpgImage);
                                    using (var ms = new System.IO.MemoryStream(orgBytes))
                                    {
                                        Bitmap image = new Bitmap(ms);
                                        this.Invoke(new Action(delegate ()
                                        {
                                            pbThumbnail.Image = image;
                                        }));
                                    }
                                });
                            }
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

        private void TestAPIGetRoi()
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.ROI_GET)}";
            rtbPayload.Text = Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestROI
            {
                nodeId = tbLastNodeId.Text,
                channelId = tbLastChannelId.Text,
                roiId = "??????",
            }, Newtonsoft.Json.Formatting.Indented);
        }
        private void TestAPIGetRoiList()
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.ROI_LIST)}";
            rtbPayload.Text = Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestROI
            {
                nodeId = tbLastNodeId.Text,
                channelId = tbLastChannelId.Text
            }, Newtonsoft.Json.Formatting.Indented);
        }

        private void TestAPIAddLink()
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.LINK_CREATE)}";
            rtbPayload.Text = Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestLink
            {
                nodeId = tbLastNodeId.Text,
                channelId = tbLastChannelId.Text,
                roiLink = new List<string>()
            }, Newtonsoft.Json.Formatting.Indented);
        }
        private void TestAPIVaStart()
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.VA_COMMAND_START)}";
            rtbPayload.Text = Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestVaStart
            {
                nodeId = tbLastNodeId.Text,
                channelIds = new List<string> { tbLastChannelId.Text },
                operation = Test.VAOperations.VA_START
            }, Newtonsoft.Json.Formatting.Indented);
        }
        private void TestAPISystemStatus()
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.SYSTEM_STATUS)}";
            rtbPayload.Text = Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestCompute
            {
                nodeId = tbLastNodeId.Text
            }, Newtonsoft.Json.Formatting.Indented);
        }

        #region Button & Radio
        private void btnAPIConnection_Click(object sender, EventArgs e)
        {
            SendAPI(tbUri.Text, rtbPayload.Text);
        }
        private void rbAddCompute_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.NODE_CREATE)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.NODE_CREATE, tbTCNodeIp.Text, tbTChttpPort.Text);
        }
        private void rbRemoveCompute_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.NODE_REMOVE)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.NODE_REMOVE, tbLastNodeId.Text);
        }
        private void rbGetCompute_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.NODE_GET)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.NODE_GET, tbLastNodeId.Text);
        }
        private void rbListCompute_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.NODE_LIST)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.NODE_LIST, "");
        }
        private void rbAddChannel_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.CH_REGIST)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.CH_REGIST, tbLastNodeId.Text, "TEST",
                "rtsp://admin:enter2424@192.168.0.154/Streaming/Channels/101");
        }
        private void rbRemoveChannel_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.CH_REMOVE)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.CH_REMOVE, tbLastNodeId.Text, tbLastChannelId.Text);
        }
        private void rbGetChannel_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.CH_GET)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.CH_GET, tbLastNodeId.Text, tbLastChannelId.Text);
        }

        private void rbListChannel_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.CH_LIST)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.CH_LIST, tbLastNodeId.Text);
        }
        private void rbAddROI_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.ROI_CREATE)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.ROI_CREATE, tbLastNodeId.Text, tbLastChannelId.Text);
        }
        private void rbRemoveROI_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.ROI_REMOVE)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.ROI_REMOVE, tbLastNodeId.Text, tbLastChannelId.Text);
        }
        private void rbGetRoi_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.ROI_GET)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.ROI_GET, tbLastNodeId.Text, tbLastChannelId.Text);
        }
        private void rbGetRoiList_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.ROI_LIST)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.ROI_LIST, tbLastNodeId.Text, tbLastChannelId.Text);
        }
        private void rbAddLink_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.LINK_CREATE)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.LINK_CREATE, tbLastNodeId.Text, tbLastChannelId.Text);
        }
        private void rbVaStart_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.VA_COMMAND_START)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.VA_COMMAND_START, tbLastNodeId.Text, tbLastChannelId.Text);
        }
        private void rbGetSystemInfo_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.SYSTEM_STATUS)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.SYSTEM_STATUS, tbLastNodeId.Text);
        }
        
        #endregion

        private void textbox_KeyPress_OnlyNumberic(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

    }
}
