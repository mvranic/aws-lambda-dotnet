using Amazon.Lambda.BootstrapWrapper;
using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.Lambda.RuntimeSupport.IntegrationTests {
    public class EchoFunction : LambdaBootstrapWrapper {
        public  EchoFunction() : base() {}

        public override string CustomHandler(string input) {
            return input;
        }
    }
}
