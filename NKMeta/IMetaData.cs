using PredefineConstant;
using System;

namespace NKMeta
{
    public interface IMetaData
    {
        public string NodeUID { get; }
        public string CameraUID { get; }
        public string CameraName { get; }
        event EventHandler<ObjectMeta> OnReceivedMetaData;
        int VideoFps { get; }

        void StartTask();
        void StopTask();
    }
}
