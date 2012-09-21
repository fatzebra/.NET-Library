using System;
using NUnit.Framework;
using FatZebra;

namespace FatZebra.Tests
{
	[TestFixture()]
	public class Test
	{
		[TestFixtureSetUp()]
		public void Init() 
		{
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

