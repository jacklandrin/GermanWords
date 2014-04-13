using System.IO;
using System.Threading.Tasks;

namespace System.Net
{
	public static class WP8AsyncWebClientExtensions
	{
		public static async Task<string> DownloadStringTaskAsync(this WebClient client, string address, object usertoken = null)
		{
            var result = await client.DownloadStringTaskAsync(new Uri(address), usertoken);
            return result;
		}
		public static  Task<string> DownloadStringTaskAsync(this WebClient client, Uri address, object usertoken = null)
		{
			var tcs = new TaskCompletionSource<string>();
			try
			{
				client.DownloadStringCompleted += (s, e) =>
													  {
														  if (e.Error == null)
														  {
															  tcs.TrySetResult(e.Result);
														  }
														  else
														  {
															  tcs.TrySetException(e.Error);
														  }
													  };


				if (usertoken != null)
				{
					 client.DownloadStringAsync(address, usertoken);
				}
				else
				{
					 client.DownloadStringAsync(address);
				}
			}
			catch (Exception ex)
			{
				tcs.TrySetException(ex);
			}

			if (tcs.Task.Exception != null)
			{
				throw tcs.Task.Exception;
			}

			return tcs.Task;
		}

		public static Task<Stream> OpenReadTaskAsync(this WebClient client, string address, object usertoken = null)
		{
			return client.OpenReadTaskAsync(new Uri(address), usertoken);
		}
		public static Task<Stream> OpenReadTaskAsync(this WebClient client, Uri address, object usertoken = null)
		{
			var tcs = new TaskCompletionSource<Stream>();
			try
			{
				client.OpenReadCompleted += (s, e) =>
												{
													if (e.Error == null)
													{
														tcs.TrySetResult(e.Result);
													}
													else
													{
														tcs.TrySetException(e.Error);
													}
												};

				if (usertoken != null)
				{
					client.OpenReadAsync(address, usertoken);
				}
				else
				{
					client.OpenReadAsync(address);
				}
			}
			catch (Exception ex)
			{
				tcs.TrySetException(ex);
			}

			if (tcs.Task.Exception != null)
			{
				throw tcs.Task.Exception;
			}

			return tcs.Task;
		}

		public static Task<Stream> OpenWriteTaskAsync(this WebClient client, string address, string method = null, object usertoken = null)
		{
			return client.OpenWriteTaskAsync(new Uri(address), method, usertoken);
		}
		public static Task<Stream> OpenWriteTaskAsync(this WebClient client, Uri address, string method = null, object usertoken = null)
		{
			var tcs = new TaskCompletionSource<Stream>();
			try
			{
				client.OpenWriteCompleted += (s, e) =>
				{
					if (e.Error == null)
					{
						tcs.TrySetResult(e.Result);
					}
					else
					{
						tcs.TrySetException(e.Error);
					}
				};

				if (usertoken != null)
				{
					client.OpenWriteAsync(address, method, usertoken);
				}
				else if (method != null)
				{
					client.OpenWriteAsync(address, method);
				}
				else
				{
					client.OpenWriteAsync(address);
				}
			}
			catch (Exception ex)
			{
				tcs.TrySetException(ex);
			}

			if (tcs.Task.Exception != null)
			{
				throw tcs.Task.Exception;
			}

			return tcs.Task;
		}

		public static Task<string> OpenWriteTaskAsync(this WebClient client, string address, string data, string method = null,
			object usertoken = null)
		{
			return client.OpenWriteTaskAsync(new Uri(address), data, method, usertoken);
		}
		public static Task<string> OpenWriteTaskAsync(this WebClient client, Uri address, string data, string method = null,
			object usertoken = null)
		{
			var tcs = new TaskCompletionSource<string>();
			try
			{
				client.UploadStringCompleted += (s, e) =>
				{
					if (e.Error == null)
					{
						tcs.TrySetResult(e.Result);
					}
					else
					{
						tcs.TrySetException(e.Error);
					}
				};

				if (usertoken != null)
				{
					client.UploadStringAsync(address, method, data, usertoken);
				}
				else if (method != null)
				{
					client.UploadStringAsync(address, method, data);
				}
				else
				{
					client.UploadStringAsync(address, data);
				}
			}
			catch (Exception ex)
			{
				tcs.TrySetException(ex);
			}

			if (tcs.Task.Exception != null)
			{
				throw tcs.Task.Exception;
			}

			return tcs.Task;
		}

		public static Task<string> UploadStringTaskAsync(this WebClient client, string address, string data, string method = "POST", object usertoken = null)
		{
			return client.UploadStringTaskAsync(new Uri(address), data, method, usertoken);
		}
		public static Task<string> UploadStringTaskAsync(this WebClient client, Uri address, string data, string method = "POST", object usertoken = null)
		{
			var tcs = new TaskCompletionSource<string>();
			try
			{
				client.UploadStringCompleted += (s, e) =>
				{
					if (e.Error == null)
					{
						tcs.TrySetResult(e.Result);
					}
					else
					{
						tcs.TrySetException(e.Error);
					}
				};


				if (usertoken != null)
				{
					client.UploadStringAsync(address, method, data, usertoken);
				}
				else
				{
					client.UploadStringAsync(address, method, data);
				}
			}
			catch (Exception ex)
			{
				tcs.TrySetException(ex);
			}

			if (tcs.Task.Exception != null)
			{
				throw tcs.Task.Exception;
			}

			return tcs.Task;
		}
	}
}
