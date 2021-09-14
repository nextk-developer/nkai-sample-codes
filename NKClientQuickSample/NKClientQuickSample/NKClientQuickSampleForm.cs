using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NKClientQuickSample
{
    public partial class NKClientQuickSampleForm : Form
    {
        private RpcClient _rpcClient;
        private RestClient _requestAPI;
        public NKClientQuickSampleForm()
        {
            InitializeComponent();

            SetDefualt();
            CreateClients();
            ChangedRpcSetting(tbTCNodeIp.Text, Convert.ToInt32(tbTCRpcPort.Text));
        }
        private void CreateClients()
        {
            //rest api
            _requestAPI = new RestClient();
            _requestAPI.ResponseAPIHandler += ReceivedApiResponse;
            _requestAPI.ResponseLastUidHandler += ReceivedApiGetUID;

            //rpc
            _rpcClient = new RpcClient();
            _rpcClient.ResponseMetaHandler += ReceivedStreamingMeta;
        }
        private void SetDefualt()
        {
            tbTCbaseUri.Text = "http://192.168.0.36:9000";
            tbTCNodeIp.Text = "192.168.0.36";
            tbTChttpPort.Text = "8880";
            tbTCRpcPort.Text = "33300";
            rbAddCompute.Checked = true;
        }
        private void ChangedRpcSetting(string host, int port)
        {
            if (_rpcClient.IsChangedCheck(host, port))
            {
                _rpcClient.Start(host, port);
            }
        }
        private void ReceivedApiGetUID(object target, string uid)
        {
            //마지막 생성된 Id 표출을 위함
            this.Invoke(new Action(delegate ()
            {
                if ((string)target == "nodeId")
                {
                    tbLastNodeId.Text = uid;
                }
                else if ((string)target == "channelId")
                {
                    tbLastChannelId.Text = uid;
                }
            }));
        }

        private void ReceivedApiResponse(object sender, string response)
        {
            //API 응답 표출
            this.Invoke(new Action(delegate ()
            {
                rtbResponse.Text = $"[{DateTime.Now.ToString()}]\r\n" + response;
            }));
        }

        private void ReceivedStreamingMeta(object sender, string response)
        {
            this.Invoke(new Action(delegate ()
            {
                rtbRpcResponse.Text = $"[{DateTime.Now.ToString("yy/MM/dd HH:mm:ss:ff")}]\r\n" + response;
            }));
        }

        private void SendAPI(string uri, string payload)
        {
            try
            {
                //rpc 정보가 변경된 경우
                ChangedRpcSetting(tbTCNodeIp.Text, Convert.ToInt32(tbTCRpcPort.Text));
                //Send API
                _requestAPI.RequestTo(uri, payload);
            }
            catch(Exception e)
            {
                
            }
        }

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
                inputUri = "rtsp://nextk.synology.me/vod/fa_test"
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
                roiId = "????",
            }, Newtonsoft.Json.Formatting.Indented);
        }
        private void TestAPIGetRoi()
        {
            tbUri.Text = $"{tbTCbaseUri.Text}/{Test.TestCase.GetPath(Test.Path.ROI_GET)}";
            rtbPayload.Text = Newtonsoft.Json.JsonConvert.SerializeObject(new Test.RequestROI
            {
                nodeId = tbLastNodeId.Text,
                channelId = tbLastChannelId.Text,
                roiId = "????",
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
