using System;
using System.Threading.Tasks;
using ProtoBuf.Grpc.Client;
using Service.TutorialPersonal.Client;

namespace TestApp
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			GrpcClientFactory.AllowUnencryptedHttp2 = true;

			Console.Write("Press enter to start");
			Console.ReadLine();

			var factory = new TutorialPersonalClientFactory("http://localhost:5001");
			var client = factory.GetTutorialPersonalService();

			//var resp = await  client.SayHelloAsync(new HelloGrpcRequest(){Name = "Alex"});
			//Console.WriteLine(resp?.Message);

			Console.WriteLine("End");
			Console.ReadLine();
		}
	}
}
