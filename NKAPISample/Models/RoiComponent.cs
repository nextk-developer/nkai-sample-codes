using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Analysis.EventType;
using PredefineConstant.Model;
using System.Collections.Generic;

namespace NKAPISample.Models
{
    public class RoiComponent
    {
        public RoiComponent()
        {
        }


        public RoiComponent(string nodeID, string channelID, string roiName, string uID, IntegrationEventType eventType, ObjectType objectType, RoiModel roi)
        {
            NodeID = nodeID;
            ChannelID = channelID;
            RoiName = roiName;
            UID = uID;
            EventType = eventType;
            ObjectType = objectType;
            Roi = roi;
        }
        #region Properties
        public string NodeID { get; private set; }
        public string ChannelID { get; private set; }
        public string RoiName { get; private set; }
        public string UID { get; private set; }
        public IntegrationEventType EventType { get; private set; }
        public ObjectType ObjectType { get; private set; }
        public RoiModel Roi { get; private set; }
        public Dictionary<string, string> Params { get; set; }
        #endregion
    }
}
