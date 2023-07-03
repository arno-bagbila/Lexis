using KiotaPosts.Client.Api.Blogs.Author.Item;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
namespace KiotaPosts.Client.Api.Blogs.Author {
    /// <summary>
    /// Builds and executes requests for operations under \api\Blogs\author
    /// </summary>
    public class AuthorRequestBuilder : BaseRequestBuilder {
        /// <summary>Gets an item from the KiotaPosts.Client.api.Blogs.author.item collection</summary>
        public WithAuthorItemRequestBuilder this[string position] { get {
            var urlTplParams = new Dictionary<string, object>(PathParameters);
            if (!string.IsNullOrWhiteSpace(position)) urlTplParams.Add("authorId", position);
            return new WithAuthorItemRequestBuilder(urlTplParams, RequestAdapter);
        } }
        /// <summary>
        /// Instantiates a new AuthorRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public AuthorRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/Blogs/author", pathParameters) {
        }
        /// <summary>
        /// Instantiates a new AuthorRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public AuthorRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/Blogs/author", rawUrl) {
        }
    }
}
