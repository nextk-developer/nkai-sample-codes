using PredefineConstant.Enum.ExternalLinks;

namespace PredefineConstant.Model.RemoteLink
{
    public class RemoteConnection
    {
        public string TargetName { get; set; }
        public string TargetIP { get; set; }
        public int TargetPort { get; set; }
        public string TargetUser { get; set; }
        public string TargetPass { get; set; }
        public ExternalType ExternalType { get; set; }
        public string Argument { get; set; }
        public string WithCamera { get; set; }
    }
}
