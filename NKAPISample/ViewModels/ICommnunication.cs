using CommunityToolkit.Mvvm.ComponentModel;
using NKAPIService.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace NKAPISample.ViewModels
{
    internal interface ICommnunication
    {
        public string HostURL { get; set; }
        public string PostURL { get; set; }
        public string RequestResult { get; set; }
        public string ResponseResult { get; set; }
        public string NodeID { get; set; }
        public string ChannelID { get; set; }

        /// <summary>
        /// Create Request Send
        /// </summary>
        public void CreateObject();

        /// <summary>
        /// Get Request Send
        /// </summary>
        public void GetObject();


        /// <summary>
        /// Remove Request Send
        /// </summary>
        public void RemoveObject();

        void SetPostURL(IRequest req);
        void SetRequestResult(IRequest req);
        Task SetResponseResult(IRequest req);
        Task<ResponseBase> GetResponse(IRequest req);
    }
}
