
namespace NKClientQuickSample.Test
{
    public static class TestCase
    {
        static public string GetPath(Path path)
        {
            switch (path)
            {
                //node
                case Path.NODE_CREATE:  return "v2/va/create-computing-node";
                case Path.NODE_REMOVE:  return "v2/va/remove-computing-node";
                case Path.NODE_UPDATE:  return "v2/va/update-computing-node";
                case Path.NODE_GET:     return "v2/va/get-computing-node";
                case Path.NODE_LIST:    return "v2/va/list-computing-node";
                //channel
                case Path.CH_REGIST:    return "v2/va/register-channel";
                case Path.CH_REMOVE:    return "v2/va/remove-channel";
                case Path.CH_UPDATE:    return "v2/va/update-channel";
                case Path.CH_GET:       return "v2/va/get-channel";
                case Path.CH_LIST:      return "v2/va/list-channel";
                //roi
                case Path.ROI_CREATE:   return "v2/va/create-roi";
                case Path.ROI_REMOVE:   return "v2/va/remove-roi";
                case Path.ROI_UPDATE:   return "v2/va/update-roi";
                case Path.ROI_GET:      return "v2/va/get-roi";
                case Path.ROI_LIST:     return "v2/va/list-roi";
                //link
                case Path.LINK_CREATE:  return "v2/va/create-link";
                case Path.LINK_GET: return "v2/va/get-link";
                case Path.LINK_LIST: return "v2/va/list-link";
                case Path.LINK_REMOVE: return "v2/va/remove-link";
                case Path.LINK_UPDATE: return "v2/va/update-link";

                default:                return "";
            }
        }
    }
}
