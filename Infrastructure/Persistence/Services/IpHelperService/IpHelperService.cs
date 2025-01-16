using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Services.IpHelperService
{
    public class IpHelperService : IIpHelperService
    {
        private IHttpContextAccessor _httpContextAccessor;
        public IpHelperService(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor;
        }


        public string GetPublicIPAddress()
        {
            string result = string.Empty;
            try
            {
                var ipAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                if (ipAddress == "::1")
                {
                    //client.Headers["User-Agent"] = "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) " + "(compatible; MSIE 6.0; Windows NT 5.1; " + ".NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                    try
                    {
                        using (var client = new WebClient())
                        {
                            byte[] arr = client.DownloadData("http://checkip.amazonaws.com/");
                            string response = System.Text.Encoding.UTF8.GetString(arr);
                            result = response.Trim();
                        }
                    }
                    catch (WebException)
                    {
                        String address = string.Empty;
                        WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
                        using (WebResponse response = request.GetResponse())
                        using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                        {
                            address = stream.ReadToEnd();
                        }
                        int first = address.IndexOf("Address: ") + 9;
                        int last = address.LastIndexOf("</body>");
                        result = address.Substring(first, last - first);
                    }
                }
            }
            catch (Exception)
            {
                return "127.0.0.1";
            }
            return result;
        }

    }
}
