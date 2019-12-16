using System;

namespace Zac.Direct.Tests.Model
{
	public class ErrorEventMonitor
	{
		public ErrorEventMonitor()
		{
			IsErrorRaised = false;
		}

		public bool IsErrorRaised { get; set; }

		public void Monitor(IRaiseErrors repo)
		{
			repo.OnError += OnError;
		}

		private void OnError(object sender, ErrorEventArgs args)
		{
			//Here I can insert error logs, rethrow the error, or ignore everything
			IsErrorRaised = true;
		}

	}
}
