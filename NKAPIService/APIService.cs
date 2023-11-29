using Newtonsoft.Json;
using NKAPIService.API;
using NKAPIService.API.Channel;
using NKAPIService.API.ComputingNode;
using NKAPIService.API.Statistics;
using NKAPIService.API.System;
using NKAPIService.API.VideoAnalysisSetting;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NKAPIService
{
    public class APIService
    {
        public Uri BaseUrl { get; private set; }
        private RestClient _restClient;

        private APIService()
        {

        }

        public override bool Equals(object obj)
        {
            if (obj is APIService service)
            {
                return this.BaseUrl == service.BaseUrl;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public static APIService Build()
        {
            return new APIService();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"> full uri</param>
        public APIService SetUrl(Uri baseUrl, string userName = "", string userPassword = "")
        {
            BaseUrl = baseUrl;

            if (!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(userPassword))
                _restClient = new RestClient(new RestClientOptions() { Authenticator = new HttpBasicAuthenticator(userName, userPassword), BaseUrl = baseUrl });
            else
                _restClient = new RestClient(new RestClientOptions() { BaseUrl = baseUrl });

            return this;
        }


        public APIService SetClient(RestClient restClient)
        {
            _restClient = restClient;

            return this;
        }


        private readonly Dictionary<RequestType, Func<string, ResponseBase>> _parsingMap = new()
        {
            #region CN
            { RequestType.CreateComputingNode, json => JsonConvert.DeserializeObject<ResponseCreateComputingNode>(json) },
            { RequestType.UpdateComputingNode, json => JsonConvert.DeserializeObject<ResponseBase>(json) },
            { RequestType.RemoveComputingNode, json => JsonConvert.DeserializeObject<ResponseRemoveComputingNode>(json) },
            { RequestType.GetComputingNode, json => JsonConvert.DeserializeObject<ResponseGetComputingNode>(json) },
            { RequestType.ListComputingNode, json => JsonConvert.DeserializeObject<ResponseListComputingNode>(json) },
            #endregion

            #region Channel
            { RequestType.GetChannel, json => JsonConvert.DeserializeObject<ResponseGetChannel>(json) },
            { RequestType.ListChannel, json => JsonConvert.DeserializeObject<ResponseListChannels>(json) },
            { RequestType.RegisterChannel, json => JsonConvert.DeserializeObject<ResponseRegisterChannel>(json) },
            { RequestType.RemoveChannel, json => JsonConvert.DeserializeObject<ResponseRemoveChannel>(json) },
            { RequestType.UpdateChannel, json => JsonConvert.DeserializeObject<ResponsetUpdateChannel>(json) },

            { RequestType.UpdateChannelLinkPoints, json => JsonConvert.DeserializeObject<ResponseBase>(json) },
            { RequestType.UpdateChannelLink, json => JsonConvert.DeserializeObject<ResponseBase>(json) },

            { RequestType.Playback, json => JsonConvert.DeserializeObject<ResponsePlayback>(json) },
            { RequestType.Export, json => JsonConvert.DeserializeObject<ResponseExport>(json) },
            { RequestType.Metadata, json => JsonConvert.DeserializeObject<ResponseMetadata>(json) },
            { RequestType.MetadataTimeList, json => JsonConvert.DeserializeObject<ResponseMetaTimeList>(json) },
            { RequestType.VaSchedule, json => JsonConvert.DeserializeObject<ResponseVaSchedule>(json) },
            { RequestType.RecordingSchedule, json => JsonConvert.DeserializeObject<ResponseRecordingSchedule>(json) },
            { RequestType.RecordDays, json => JsonConvert.DeserializeObject<ResponseRecordDays>(json) },
            #endregion

            #region Events
            { RequestType.CreateROI, json => JsonConvert.DeserializeObject<ResponseCreateROI>(json) },
            { RequestType.RemoveROI, json => JsonConvert.DeserializeObject<ResponseBase>(json) },
            { RequestType.UpdateROI, json => JsonConvert.DeserializeObject<ResponseBase>(json) },
            { RequestType.GetROI, json => JsonConvert.DeserializeObject<ResponseGetROI>(json) },
            { RequestType.ListROI, json => JsonConvert.DeserializeObject<ResponseListROI>(json) },
            { RequestType.Control, json => JsonConvert.DeserializeObject<ResponseControl>(json) },

            #endregion

            #region FaceDB
            { RequestType.RegisterFaceDB, json => JsonConvert.DeserializeObject<ResponseRegisterFaceDB>(json) },
            { RequestType.UpdateFaceDB, json => JsonConvert.DeserializeObject<ResponseUpdateFaceDB>(json) },
            { RequestType.UnRegisterFaceDB, json => JsonConvert.DeserializeObject<ResponseUnRegisterFaceDB>(json) },
            { RequestType.ListFaceDB, json => JsonConvert.DeserializeObject<ResponseListFaceDB>(json) },
            { RequestType.MatchingFace, json => JsonConvert.DeserializeObject<ResponseMatchingFace>(json) },
            #endregion

            #region Etc
            { RequestType.Calibrate, json => JsonConvert.DeserializeObject<ResponseCalibrate>(json) },
            { RequestType.GetStatistics, json => JsonConvert.DeserializeObject<ResponseStatistics>(json) },
            { RequestType.SystemLog, json => JsonConvert.DeserializeObject<ResponseSystemLog>(json) },
            { RequestType.System, json => JsonConvert.DeserializeObject<ResponseBase>(json) },
            { RequestType.GetModels, json => JsonConvert.DeserializeObject<ResponseGetModels>(json) },
            { RequestType.ApplyModels, json => JsonConvert.DeserializeObject<ResponseBase>(json) },
            #endregion
        };

        public async Task<ResponseBase> Requset(IRequest requestBody)
        {
            ResponseBase rb = new() { Code = ErrorCode.REQUEST_TIMEOUT };
            var req = new RestRequest()
            {
                Resource = requestBody.GetResource(),
                Method = Method.Post,
                RequestFormat = DataFormat.Json,
            };

            var body = JsonConvert.SerializeObject(requestBody);
            req.AddHeader("Content-type", "application/json; charset=utf-8");
            req.AddHeader("Accept-Encoding", "gzip");
            req.AddJsonBody(body);


            var res = await _restClient.ExecutePostAsync(req);
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                rb = _parsingMap[requestBody.RequsetType](res.Content);
            }

            return rb;
        }
    }
}
