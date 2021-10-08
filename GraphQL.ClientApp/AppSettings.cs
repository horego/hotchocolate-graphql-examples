using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL.ClientApp
{
    class AppSettings
    {
        public Uri EndpointAddress => new Uri("http://localhost:8444/graphql");
    }
}
