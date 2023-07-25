namespace PredefineConstant.Enum.ComputingNode
{
    public enum APIGatewayType { Command, Monitoring }

    public class APIGatewayDetail
    {
        public string UID { get; set; }
        public string APIGatewayIP { get; set; }
        public int APIGatewayPort { get; set; }
        public APIGatewayType APIGatewayType { get; set; }
    }
}
