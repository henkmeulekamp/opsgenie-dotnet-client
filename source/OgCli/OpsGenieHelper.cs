using System.Xml;
using OpsGenieApi;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace OpsGenieCli
{
    internal class OpsGenieHelper
    {     
        public static OpsGenieClient CreateOpsGenieClient(OpsGenieClientConfig config)
        {
            return new OpsGenieClient(config, new MyPreferedJsonizer());
        }
        
        public static OpsGenieClientConfig GetOpsGenieConfig(string configPath)
        {
            var serializer = new XmlSerializer(typeof(OpsGenieClientConfig));
            return (OpsGenieClientConfig)serializer.Deserialize(new XmlTextReader(configPath));
        }


    }
}