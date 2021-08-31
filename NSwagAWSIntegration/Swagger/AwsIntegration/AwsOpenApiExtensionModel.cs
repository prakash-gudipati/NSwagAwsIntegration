using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSwagAWSIntegration.Swagger.AwsIntegration
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AwsOpenApiExtensionModel
    {
        public string ConnectionId { get; set; }

        public string HttpMethod { get; set; }

        public string Uri { get; set; }

        public Dictionary<string, string> RequestParameters { get; set; }

        public Dictionary<string, Dictionary<string, string>> Responses { get; set; }

        public string PassthroughBehavior { get; set; }

        public string ConnectionType { get; set; }

        public string Type{ get; set; }
    }
}
