using System;
using System.Collections.Generic;
using System.Text;
using IocTests;

namespace XLabs.IOC.NUnit.Shared.Services
{
	public class MyServiceWithConstructorParam : IMyService
	{
		public MyServiceWithConstructorParam() { }

		public MyServiceWithConstructorParam(int age)
		{
			Age = age;
		}

		public int Age { get; private set; }

		#region Implementation of IMyService

		public IService Service { get; set; }

		#endregion
	}
}
