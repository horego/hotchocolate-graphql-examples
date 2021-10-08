using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL.ServerApp
{
    class AppSettings
    {
        public int ServicePort => 8444;
        public string ServiceEndpointPath => "/graphql";
    }
}
