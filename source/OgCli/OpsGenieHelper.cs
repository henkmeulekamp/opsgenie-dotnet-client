using System.Xml;
using System.Xml.Serialization;
using OpsGenieApi;

namespace OpsGenieCli
{
    internal class OpsGenieHelper
    {     
        public static OpsGenieClient CreateOpsGenieClient(OpsGenieClientConfig config)
        {
            return new OpsGenieClient(config);
        }
        
        public static OpsGenieClientConfig GetOpsGenieConfig(string configPath)
        {
            var serializer = new XmlSerializer(typeof(OpsGenieClientConfig));
            return (OpsGenieClientConfig)serializer.Deserialize(new XmlTextReader(configPath));
        }

    }
}