namespace NKAPIService
{
    public enum RequestType
    {
        #region Computing Node
        CreateComputingNode,
        UpdateComputingNode,
        RemoveComputingNode,
        GetComputingNode,
        ListComputingNode,
        #endregion

        #region Channel
        GetChannel,
        ListChannel,
        RegisterChannel,
        RemoveChannel,
        UpdateChannel,
        #endregion

        #region Events
        AddOrUpdate,
        GetROI,
        UpdateROI,
        RemoveROI,
        ListROI,
        Control,

        UpdateChannelLinkPoints,
        UpdateChannelLink,
        #endregion


        #region face db
        RegisterFaceDB,
        ListFaceDB,
        UpdateFaceDB,
        UnRegisterFaceDB,
        MatchingFace,
        #endregion


        Calibrate,
        Snapshot,

        RecordingSchedule,
        VaSchedule,
        Playback,
        Export,
        MetadataTimeList,
        Metadata,
        RecordDays,

        SystemLog,
        System,
        GetModels,
        ApplyModels,

        GetStatistics
    }
}
