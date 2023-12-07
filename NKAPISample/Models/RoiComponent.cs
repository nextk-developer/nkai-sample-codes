using PredefineConstant.Enum.Analysis.EventType;
using PredefineConstant.Enum.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NKAPIService.API.VideoAnalysisSetting.Models;
using PredefineConstant.Model;

namespace NKAPISample.Models
{
    public class RoiComponent
    {
        public RoiComponent()
        {
        }

        public RoiComponent(string nodeID, string channelID, string roiName, RoiNumber roiNumber, string uID, IntegrationEventType eventType, ObjectType objectType, ROI rOI, EventFilter eventFilter)
        {
            NodeID = nodeID;
            ChannelID = channelID;
            RoiName = roiName;
            RoiNumber = roiNumber;
            UID = uID;
            EventType = eventType;
            ObjectType = objectType;
            ROI = rOI;
            EventFilter = eventFilter;
        }
        #region Properties
        public string NodeID { get; private set; }
        public string ChannelID { get; private set; }
        public string RoiName { get; private set; }
        public RoiNumber RoiNumber { get; private set; }
        public string UID { get; private set; }
        public IntegrationEventType EventType { get; private set; }
        public ObjectType ObjectType { get; private set; }
        public ROI ROI { get; private set; }
        public EventFilter EventFilter { get; private set; }
        public Dictionary<string, string> Params { get; set; }
        #endregion
    }
}
