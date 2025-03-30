using System.Collections.Generic;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
namespace ChatApplication
{

    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters != null)
            {
                foreach (var param in context.ApiDescription.ParameterDescriptions)
                {
                    if (param.Type == typeof(IFormFile))
                    {
                        operation.RequestBody = new OpenApiRequestBody
                        {
                            Content = new Dictionary<string, OpenApiMediaType>
                            {
                                ["multipart/form-data"] = new OpenApiMediaType
                                {
                                    Schema = new OpenApiSchema
                                    {
                                        Type = "object",
                                        Properties =
                                            {
                                                [param.Name] = new OpenApiSchema { Type = "string", Format = "binary" }
                                            },
                                        Required = new HashSet<string> { param.Name }
                                    }
                                }
                            }
                        };
                        operation.Parameters.Clear(); // Remove from query params
                    }
                }
            }
        }
    }

}
