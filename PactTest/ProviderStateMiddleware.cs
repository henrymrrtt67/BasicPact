﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Newtonsoft.Json;

namespace PactTest
{
    public class ProviderStateMiddleware
    {
        private const string ConsumerName = "Event API Consumer";
        private readonly Func<IDictionary<string, object>, Task> m_next;
        private readonly IDictionary<string, Action> _providerStates;

        public ProviderStateMiddleware(Func<IDictionary<string, object>, Task> next)
        {
            m_next = next;
            _providerStates = new Dictionary<string, Action>
            {
                {
                    "There is a something with id 'tester'",
                    AddTesterIfItDoesntExist
                }
            };

        }

        public void AddTesterIfItDoesntExist()
        {
            return;
        }


        public async Task Invoke(IDictionary<string, object> environment)
        {
            IOwinContext context = new OwinContext(environment);

            if (context.Request.Path.Value == "/provider-states")
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                if(context.Request.Method == HttpMethod.Post.ToString() &&
                    context.Request.Body != null)
                {
                    var jsonRequestBody = string.Empty;
                    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                    {
                        jsonRequestBody = reader.ReadToEnd();
                    }

                    var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

                    if(providerState != null &&
                       !string.IsNullOrEmpty(providerState.State) &&
                       providerState.Consumer == ConsumerName)
                    {
                        _providerStates[providerState.State].Invoke();
                    }

                    await context.Response.WriteAsync(String.Empty);
                }
            }
            else
            {
                await m_next.Invoke(environment);
            }
        }
    }
}
