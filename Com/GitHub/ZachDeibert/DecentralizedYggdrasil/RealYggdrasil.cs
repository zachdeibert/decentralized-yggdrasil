using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil {
	public class RealYggdrasil {
		private Dictionary<string, HashSet<string>> RealIPs;

		public IEnumerable<string> Resolve(string host) {
			if (RealIPs.ContainsKey(host)) {
				return RealIPs[host];
			} else {
				Console.Error.WriteLine("Forgot to resolve {0} before overriding it.", host);
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
			Exception e = new WebException("Unknown host");
			foreach (string host in Resolve(uri.Host)) {
				UriBuilder proxyUri = new UriBuilder(uri);
				proxyUri.Host = host;
				HttpWebRequest req = WebRequest.CreateHttp(proxyUri.Uri);
				req.Host = uri.Host;
				if (requestBuilder != null) {
					requestBuilder(req);
				}
				try {
					return (HttpWebResponse) req.GetResponse();
				} catch (WebException ex) {
					if (ex.Response == null) {
						throw;
					} else {
						return (HttpWebResponse) ex.Response;
					}
				} catch (Exception ex) {
					e = ex;
				}
			}
			throw e;
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
				if (status >= HttpStatusCode.OK && status < HttpStatusCode.MultipleChoices) {
					using (Stream stream = res.GetResponseStream()) {
						using (TextReader reader = new StreamReader(stream)) {
							try {
								string json = reader.ReadToEnd();
								if (json != null && json.Length > 0) {
									return JsonConvert.DeserializeObject<T>(json);
								}
							} catch {
							}
						}
					}
				} else {
					Console.Error.WriteLine("Request failed with code {0} for {1}", status, uriString);
				}
				return default(T);
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

