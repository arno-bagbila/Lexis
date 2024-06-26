using KiotaPosts.Client.Api.Blogs;
using KiotaPosts.Client.Api.Users;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
namespace KiotaPosts.Client.Api {
    /// <summary>
    /// Builds and executes requests for operations under \api
    /// </summary>
    public class ApiRequestBuilder : BaseRequestBuilder {
        /// <summary>The Blogs property</summary>
        public BlogsRequestBuilder Blogs { get =>
            new BlogsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The Users property</summary>
        public UsersRequestBuilder Users { get =>
            new UsersRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new ApiRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ApiRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api", pathParameters) {
        }
        /// <summary>
        /// Instantiates a new ApiRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ApiRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api", rawUrl) {
        }
    }
}
