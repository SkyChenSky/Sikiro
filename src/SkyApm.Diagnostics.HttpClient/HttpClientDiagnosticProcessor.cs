/*
 * Licensed to the SkyAPM under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The SkyAPM licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System;
using System.Net.Http;
using Newtonsoft.Json;
using SkyApm.Common;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;

namespace SkyApm.Diagnostics.HttpClient
{
    public class HttpClientTracingDiagnosticProcessor : ITracingDiagnosticProcessor
    {
        public string ListenerName { get; } = "HttpHandlerDiagnosticListener";

        //private readonly IContextCarrierFactory _contextCarrierFactory;
        private readonly ITracingContext _tracingContext;
        private readonly IExitSegmentContextAccessor _contextAccessor;
        private SegmentContext _segmentContext;

        public HttpClientTracingDiagnosticProcessor(ITracingContext tracingContext,
            IExitSegmentContextAccessor contextAccessor)
        {
            _tracingContext = tracingContext;
            _contextAccessor = contextAccessor;
        }

        [DiagnosticName("System.Net.Http.Request")]
        public void HttpRequest([Property(Name = "Request")] HttpRequestMessage request)
        {
            _segmentContext = _tracingContext.CreateExitSegmentContext(request.RequestUri.ToString(),
                $"{request.RequestUri.Host}:{request.RequestUri.Port}",
                new HttpClientICarrierHeaderCollection(request));
            var context = _segmentContext;

            if (request.Content != null)
            {
                var requestStr = request.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                context.Span.AddTag(TagsExtension.REQUEST, requestStr);
            }
            context.Span.SpanLayer = SpanLayer.HTTP;
            context.Span.Component = Components.HTTPCLIENT;
            context.Span.AddTag(Tags.URL, request.RequestUri.ToString());
            context.Span.AddTag(Tags.HTTP_METHOD, request.Method.ToString());
            context.Span.AddTag(TagsExtension.HEADERS, JsonConvert.SerializeObject(request.Headers));
        }

        [DiagnosticName("System.Net.Http.Response")]
        public void HttpResponse([Property(Name = "Response")] HttpResponseMessage response)
        {
            var context = _segmentContext;
            if (context == null)
            {
                return;
            }

            if (response != null)
            {
                var statusCode = (int)response.StatusCode;
                if (statusCode >= 400)
                {
                    context.Span.ErrorOccurred();
                }

                if (response.Content != null)
                {
                    var responseStr = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    context.Span.AddTag(TagsExtension.RESPONSE, responseStr);
                }

                context.Span.AddTag(Tags.STATUS_CODE, statusCode);
            }

            _tracingContext.Release(context);
        }

        [DiagnosticName("System.Net.Http.Exception")]
        public void HttpException([Property(Name = "Request")] HttpRequestMessage request,
            [Property(Name = "Exception")] Exception exception)
        {
            _segmentContext?.Span?.ErrorOccurred(exception);
        }
    }
}