﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace Carting.API.Extensions
{
    public static class ConfigureApiVersioning
    {
        public static void AddApiVersioningConfigured(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                // ReportApiVersions will return the "api-supported-versions" and "api-deprecated-versions" headers.

                options.ReportApiVersions = true;

                // Set a default version when it's not provided,

                // e.g., for backward compatibility when applying versioning on existing APIs

                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();

                // Combine (or not) API Versioning Mechanisms:

                /*
                options.ApiVersionReader = ApiVersionReader.Combine(
                    // The Default versioning mechanism which reads the API version from the "api-version" Query String paramater.

                    new QueryStringApiVersionReader("api-version"),
                    // Use the following, if you would like to specify the version as a custom HTTP Header.

                    new HeaderApiVersionReader("Accept-Version"),
                    // Use the following, if you would like to specify the version as a Media Type Header.

                    new MediaTypeApiVersionReader("api-version")
                );
                */
            });

            // Here, we will add another service, e.g., to support versioning on our documentation.
            services.AddVersionedApiExplorer(options => {
                // instruct how to read versions
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        }
    }
}
