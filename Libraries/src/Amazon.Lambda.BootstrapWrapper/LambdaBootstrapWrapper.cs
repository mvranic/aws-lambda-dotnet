using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using System;
using System.Threading.Tasks;
using Amazon.Lambda.Serialization.Json;

namespace Amazon.Lambda.BootstrapWrapper {
	/// <summary>
	/// Lambda bootstrap wrapper.
	/// Used for custum run time for langauages with ..net bridge.
	/// </summary>
	public abstract class LambdaBootstrapWrapper {
		/// <summary>
		/// Handler name.
		/// </summary>
		public const string HandlerName = "Handler";
		/// <summary>
		/// Deligate
		/// </summary>
		/// <param name="json">Input json object</param>
		/// <returns>Output JSON object</returns>
		public delegate string Handler(string json);
		/// <summary>
		/// Instance of handler.
		/// </summary>
		public Handler HandlerInstance; 

		/// <summary>
		/// Constructor.
		/// </summary>
		public LambdaBootstrapWrapper() {
			HandlerInstance = new Handler(CustomHandler);
		}

		/// <summary>
		/// Start Lambda.
		/// </summary>
		public void Start() {
			Task.Run(() => StartAsync());
		}

		/// <summary>
		/// Start Lambda.
		/// </summary>
		public async Task StartAsync() {
			// Wrap the FunctionHandler method in a form that LambdaBootstrap can work with.
			using (var handlerWrapper = HandlerWrapper.GetHandlerWrapper((Func<string, ILambdaContext, string>)BaseHandler, new JsonSerializer())) {
				// Instantiate a LambdaBootstrap and run it.
				// It will wait for invocations from AWS Lambda and call
				// the handler function for each one.
				using (var bootstrap = new LambdaBootstrap(handlerWrapper)) {
					await bootstrap.RunAsync();
				}
			}
		}

		/// <summary>
		/// Handler.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public string BaseHandler(string input, ILambdaContext context) {
			return CustomHandler(input);
		}

		/// <summary>
		/// Handler.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public abstract string CustomHandler(string input);
	}
}
