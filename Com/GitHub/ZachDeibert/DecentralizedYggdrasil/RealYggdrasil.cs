using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil {
	public class RealYggdrasil {
		private Dictionary<string, HashSet<string>> RealIPs;

		public IEnumerable<string> Resolve(string host) {
			if (RealIPs.ContainsKey(host)) {
				return RealIPs[host];
			} else {
				return Dns.GetHostAddresses(host).Select(i => i.ToString());
			}
		}

		public void ResolveAOT(string host) {
			IPAddress[] addrs = Dns.GetHostAddresses(host);
			if (addrs.Length > 0) {
				HashSet<string> ips;
				if (RealIPs.ContainsKey(host)) {
					ips = RealIPs[host];
				} else {
					ips = RealIPs[host] = new HashSet<string>();
				}
				foreach (IPAddress addr in addrs) {
					ips.Add(addr.ToString());
				}
			}
		}

		public HttpWebResponse Connect(string uriString, Action<HttpWebRequest> requestBuilder = null) {
			Uri uri = new Uri(uriString);
			if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) {
				throw new UriFormatException("Invalid URI scheme");
			}
			Exception ex = new WebException("Unknown host");
			foreach (string host in Resolve(uri.Host)) {
				UriBuilder proxyUri = new UriBuilder(uri);
				proxyUri.Host = host;
				HttpWebRequest req = WebRequest.CreateHttp(proxyUri.Uri);
				if (requestBuilder != null) {
					requestBuilder(req);
				}
				try {
					return (HttpWebResponse) req.GetResponse();
				} catch (Exception e) {
					ex = e;
				}
			}
			throw ex;
		}

		public T Request<T>(string uriString, object request, out HttpStatusCode status) {
			using (HttpWebResponse res = Connect(uriString, req => {
				if (request != null) {
					req.Method = "POST";
					req.ContentType = "application/json";
					using (Stream stream = req.GetRequestStream()) {
						using (TextWriter writer = new StreamWriter(stream)) {
							writer.Write(JsonConvert.SerializeObject(request));
						}
					}
				}
			})) {
				status = res.StatusCode;
				using (Stream stream = res.GetResponseStream()) {
					using (TextReader reader = new StreamReader(stream)) {
						try {
							string json = reader.ReadToEnd();
							if (json != null) {
								return JsonConvert.DeserializeObject<T>(json);
							}
						} catch {
						}
						return default(T);
					}
				}
			}
		}

		public T Request<T>(string uriString, object request) {
			HttpStatusCode status;
			return Request<T>(uriString, request, out status);
		}

		public RealYggdrasil() {
			RealIPs = new Dictionary<string, HashSet<string>>();
			foreach (string host in HostsFile.Hosts) {
				ResolveAOT(host);
			}
		}
	}
}

