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
        private string _currentDrawChannelID;
        private string _currentParameter;
        private Test.ResponseRoiList _drawRoiList;

        private Pen _event_pen = new Pen(Brushes.Red, 8);
        private Pen _object_pen = new Pen(Brushes.Green, 6);
        private Pen _area_pen = new Pen(Brushes.Yellow, 6);
        private Font _draw_font = new Font("Arial", 5, FontStyle.Bold);
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
                        if (_drawRoiList != null && _drawRoiList.rois != null)
                        {
                            foreach (var roi in _drawRoiList.rois)
                            {
                                List<PointF> polygonPoint = new List<PointF>();
                                foreach(var dot in roi.roiDots)
                                {
                                    polygonPoint.Add(new PointF((float)(e.Width * dot.x), (float)(e.Height * dot.y)));
                                }
                                grap.DrawPolygon(_area_pen, polygonPoint.ToArray());
                            }
                        }

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
                                }
                            }
                        }
                        if ((DateTime.Now - _vaMetaDate).TotalMilliseconds > 2000)
                        {
                            _currentVAMeta = null;
                            pbRpcSignal.BackColor = Color.Gray;
                        }
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
            _restClient.ResponseDirectDrawHandler += ReceivedDrawDirectResponse;
            _restClient.ResponseUidHandler += ReceivedApiGetUID;

            //rpc
            _rpcClient = new Client.RpcClient();
            _rpcClient.ResponseMetaHandler += ReceivedStreamingMeta;

            //draw 
            _captureFrame = new Video.CaptureFrame();
            _captureFrame.ResponseDrawFrameHandler += ReceivedDrawFrame;
        }
        private void ReceivedDrawDirectResponse(object sender, string e)
        {
            try
            {
                _drawRoiList = Newtonsoft.Json.JsonConvert.DeserializeObject<Test.ResponseRoiList>(e);
            }
            catch
            {
                _drawRoiList = null;
            }
        }
        private void SetDefualt()
        {
            _vaMetaDate = DateTime.Now;
            tbTCbaseUri.Text = "http://192.168.0.36:9000";
            tbTCNodeIp.Text = "192.168.0.36";
            //tbTCbaseUri.Text = "http://172.16.0.153:9000";
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
        private void SendAPI(string uri, string payload, bool useResponse = false)
        {
            try
            {
                //rpc 정보가 변경된 경우
                ChangedRpcSetting(tbTCNodeIp.Text, Convert.ToInt32(tbTCRpcPort.Text));
                //Send API
                _restClient.RequestTo(uri, payload, useResponse);
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
        private void DrawCaptureImge(State state, ObjectType type, 
            double x, double y, double w, double h, string base64Jpg, 
            double width, double height)
        {
            if (base64Jpg == null || base64Jpg.Length == 0) return;

            System.Threading.Tasks.Task.Run(() =>
            {
                byte[] orgBytes = Convert.FromBase64String(base64Jpg);
                using (var ms = new System.IO.MemoryStream(orgBytes))
                {
                    this.Invoke(new Action(delegate ()
                    {
                        Bitmap image = new Bitmap(ms);
                        using (Graphics grap = Graphics.FromImage(image))
                        {
                            grap.DrawRectangle(Pens.Red, new Rectangle((int)(x * width), (int)(y * height), (int)(w * width), (int)(h * height)));
                            if (_currentParameter != null)
                            {
                                grap.DrawString($"{_currentParameter}", _draw_font, _solid_brush, 0, 0, new StringFormat());
                            }
                        }
                        pbThumbnail.Image = image;
                        lbClassInfo.Text = $"Class: {type}({state})";
                        lbBoxInfo.Text = $"Box: {width}x{height}";
                    }));
                }
            });
        }
        private void ReceivedStreamingMeta(object sender, FrameMetaData response)
        {
            this.Invoke(new Action(delegate ()
            {
                if (response.ChannelId == _currentDrawChannelID &&
                    response.EventList != null)
                {
                    if(pbRpcSignal.BackColor == Color.Gray)
                        pbRpcSignal.BackColor = Color.Green;

                    _currentVAMeta = response;
                    _vaMetaDate = DateTime.Now;
                    foreach (EventInfo ei in response.EventList)
                    {
                        if (ei.JpegImage == null) continue;

                        _currentParameter = null;
                        if (ei.EventParam != null && ei.EventParam.Parameters.Count() > 0)
                        {
                            _currentParameter = ei.EventParam.Parameters[0];
                        }

                        DrawCaptureImge(ei.State, ei.Segmentation.Label,
                            ei.JpegImage.ObjectBox.X, ei.JpegImage.ObjectBox.Y, ei.JpegImage.ObjectBox.Width, ei.JpegImage.ObjectBox.Height,
                            ei.JpegImage.Base64Image, ei.JpegImage.ImageWidth, ei.JpegImage.ImageHeight);
                    }
                }
            }));
        }
        #endregion
        private void textbox_KeyPress_OnlyNumberic(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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
                //"rtsp://192.168.0.70:554/live/dotonbori.stream");
                //"rtsp://192.168.0.70:554/vod/its2");
                "rtsp://192.168.0.70:554/vod/fa_test");
                //"rtsp://192.168.0.70:554/vod/face_gogo");
                //"rtsp://192.168.0.70:554/vod/pertest_3m_5m_con");


            if (_drawRoiList != null)
                _drawRoiList = null;
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
        private void rbVaStop_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.VA_COMMAND_STOP)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.VA_COMMAND_STOP, tbLastNodeId.Text, tbLastChannelId.Text);
        }
        private void rbGetSystemInfo_CheckedChanged(object sender, EventArgs e)
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.SYSTEM_STATUS)}";
            rtbPayload.Text = Test.TestCase.GetFormat(Test.Path.SYSTEM_STATUS, tbLastNodeId.Text);
        }
        
        #endregion

        private void btnUpdateArea_Click(object sender, EventArgs e)
        {
            string url      = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.ROI_LIST)}";
            string payload  = Test.TestCase.GetFormat(Test.Path.ROI_LIST, tbLastNodeId.Text, tbLastChannelId.Text);
            SendAPI(url, payload, true);
        }
    }
}
