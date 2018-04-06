using System;
using NUnit.Framework;
using FatZebra;

namespace FatZebra.Tests
{
	[TestFixture()]
	public class Test
	{
		[OneTimeSetUp()]
		public void Init() 
		{
//            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            FatZebra.Gateway.Username = "TEST";
			FatZebra.Gateway.Token = "TEST";
			Gateway.SandboxMode = true;
			Gateway.TestMode = true;
		}

		[Test()]
		public void PingShouldBeSuccessful ()
		{
			Assert.IsTrue(Gateway.Ping());
		}
	}
}

