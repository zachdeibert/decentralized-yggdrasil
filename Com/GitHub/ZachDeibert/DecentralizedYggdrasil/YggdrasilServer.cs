using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil {
	public class YggdrasilServer {
		private HttpListener Listener;
		private List<IApi> Apis;

		private void CheckRequest(HttpListenerRequest req) {
			if (req.HttpMethod == "POST" && req.Headers.Get("Content-Type") != "application/json") {
				throw new StandardErrorException(StandardErrors.InvalidType, 415, "Unsupported Media Type");
			}
		}

		private object Handle(Uri uri, string method, string data) {
			foreach (IApi api in Apis) {
				if (api.IsAcceptable(uri)) {
					if (method != api.Method) {
						throw new StandardErrorException(StandardErrors.InvalidMethod, 405, "Method Not Allowed");
					}
					return api.Run(JsonConvert.DeserializeObject(data, api.ParamType), uri);
				}
			}
			throw new StandardErrorException(StandardErrors.InvalidURL, 404, "Not Found");
		}

		private void RequestCallback(IAsyncResult iar) {
			HttpListenerContext ctx = Listener.EndGetContext(iar);
			if (Listener.IsListening) {
				Listener.BeginGetContext(RequestCallback, null);
			}
			if (ctx != null) {
				HttpListenerRequest req = ctx.Request;
				HttpListenerResponse res = ctx.Response;
				try {
					object response = null;
					try {
						CheckRequest(req);
						byte[] data = new byte[req.ContentLength64];
						req.InputStream.Read(data, 0, data.Length);
						response = Handle(req.Url, req.HttpMethod, Encoding.UTF8.GetString(data));
					} catch (StandardErrorException ex) {
						response = ex.Error;
						res.StatusCode = ex.StatusCode;
						res.StatusDescription = ex.StatusDescription;
					} catch (Exception ex) {
						response = new Error(ex);
						res.StatusCode = 500;
						res.StatusDescription = "Internal Server Error";
					}
					byte[] raw = response == null ? new byte[0] : Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
					if (raw.Length == 0) {
						res.StatusCode = 204;
						res.StatusDescription = "No Content";
					}
					res.ContentLength64 = raw.Length;
					res.ContentType = "application/json";
					res.OutputStream.Write(raw, 0, raw.Length);
				} catch (Exception ex) {
					Console.Error.WriteLine(ex);
				} finally {
					res.OutputStream.Close();
				}
			}
		}

		public void Start() {
			Apis = new List<IApi>();
			foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()) {
				if (typeof(IApi).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface) {
					Apis.Add((IApi) t.GetConstructor(new Type[0]).Invoke(new object[0]));
				}
			}
			Listener.Start();
			Listener.BeginGetContext(RequestCallback, null);
		}

		public void Stop() {
			Listener.Stop();
		}

		public YggdrasilServer(int port) {
			Listener = new HttpListener();
			Listener.Prefixes.Add(string.Format("http://+:{0}/", port));
		}
	}
}

