﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace ConsoleSelfHostedWCF
{
    [ServiceContract]
    public interface IHelloWorldService
    {
        [OperationContract]
        string SayHello(string name);
    }

    public class HelloWorldService : IHelloWorldService
    {
        public string SayHello(string name)
        {
            return string.Format("Hello, {0}", name);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting on port " + System.Environment.GetEnvironmentVariable("PORT"));

            // also tried + and localhost
            Uri baseAddress = new Uri("http://0.0.0.0:" + System.Environment.GetEnvironmentVariable("PORT") + "/api");
            //Uri baseAddress = new Uri("http://+:" + System.Environment.GetEnvironmentVariable("PORT") + "/api");
            //Uri baseAddress = new Uri("http://localhost:" + System.Environment.GetEnvironmentVariable("PORT") + "/api");

            // Create the ServiceHost.
            using (ServiceHost host = new ServiceHost(typeof(HelloWorldService), baseAddress))
            {
                // Enable metadata publishing.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);

                // Open the ServiceHost to start listening for messages. Since
                // no endpoints are explicitly configured, the runtime will create
                // one endpoint per base address for each service contract implemented
                // by the service.
                host.Open();

                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                // Close the ServiceHost.
                host.Close();
            }

            while (true) {
                Console.WriteLine("Alive");
                System.Threading.Thread.Sleep(5000);
            }
        }
    }
}
