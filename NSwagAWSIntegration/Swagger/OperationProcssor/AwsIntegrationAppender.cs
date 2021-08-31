using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using NSwagAWSIntegration.Swagger.AwsIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSwagAWSIntegration.Swagger.OperationProcssor
{
    public class AwsIntegrationAppender : IOperationProcessor
    {
        private const string ExtensionKey = "x-amazon-apigateway-integration";

        public bool Process(OperationProcessorContext context)
        {
            if (context.OperationDescription.Operation.ExtensionData == null)
            {
                context.OperationDescription.Operation.ExtensionData = new Dictionary<string, object>();
            }

            if (context.OperationDescription.Operation.ExtensionData.ContainsKey(ExtensionKey))
            {
                // Note : Key already exists. Skipping.
                return true;
            }

            var awsExtensionData = new AwsOpenApiExtensionModel
            {
                Type = "http",
                PassthroughBehavior = "when_no_match",
                ConnectionId = "abcd123",
                Responses = new Dictionary<string, Dictionary<string, string>> {
                    { "default", new Dictionary<string, string> {
                        { "statusCode", "200" },
                    }
                    },
                },
                ConnectionType = "VPC_LINK",
                Uri = context.OperationDescription.Path,
                HttpMethod = context.OperationDescription.Method.ToUpper(),
            };

            List<OpenApiParameter> requestParameters = context.OperationDescription.Operation.Parameters.Where(x => x.Kind == OpenApiParameterKind.Query).ToList();
            if (context.OperationDescription.Method.ToLower() == "get" && requestParameters.Count > 0)
            {
                awsExtensionData.RequestParameters = new Dictionary<string, string>();
                foreach (var parameter in requestParameters)
                {
                    awsExtensionData.RequestParameters.Add($"integration.request.path.{parameter.Name}", $"method.request.path.{parameter.Name}");
                }
            }

            context.OperationDescription.Operation.ExtensionData[ExtensionKey] = awsExtensionData;

            return true;
        }
    }
}
