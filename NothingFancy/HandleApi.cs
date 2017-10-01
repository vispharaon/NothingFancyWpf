using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using NothingFancy.Data.Dto;

namespace NothingFancy
{
    public class HandleApi
    {
        public static TrackListDto GetListOfTracks(string queryString, int pageSize)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(Constants.BaseAddressApi),
                MaxResponseContentBufferSize = 256000
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(ConstructRequestUri(queryString, pageSize)).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Status code not correct: {response.StatusCode}! {response.RequestMessage}");
            }

            return JsonConvert.DeserializeObject<TrackListDto>(response.Content.ReadAsStringAsync().Result);
        }

        private static string ConstructRequestUri(string queryString, int pageSize)
        {
            return
                $"{Constants.TrackSearchApi}?" +
                $"q={queryString}" +
                $"&oauth_consumer_key={Constants.ApiKey}" +
                $"&country={Constants.CountryDevTag}" +
                $"&pagesize={pageSize}" +
                $"&usageTypes={Constants.UsageTypes}";
        }

        public static string GetPreviewTrackSource(int trackId)
        {
            var normalizedUrl = String.Empty;
            var normalizedRequestParameters = String.Empty;
            var authHeader = String.Empty;
            var encodedSignature = string.Empty;

            var signature = GenerateHmacsha1Signature(new Uri($"{Constants.BaseAddressPreview}{Constants.TrackClipPreview}/{trackId}"), "GET",
                out normalizedUrl, 
                out normalizedRequestParameters,
                out authHeader,
                out encodedSignature);

            return $"{normalizedUrl}?{normalizedRequestParameters}&oauth_signature={encodedSignature}";
        }
        
        private static string ConstructRequestPreviewUri(int trackId)
        {
            return $"{Constants.TrackClipPreview}" +
                   $"/{trackId}" +
                   $"?country={Constants.CountryDevTag}";
        }

        /// <summary> 
        /// Generates a signature using the specified signatureType  
        /// </summary>         
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param> 
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param> 
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param> 
        /// <returns>A base64 string of the hash value</returns> 
        public static string GenerateHmacsha1Signature(Uri url, string httpMethod,
            out string normalizedUrl, out string normalizedRequestParameters, out string authHeader, out string encodedSignature)
        {
            normalizedUrl = null;
            normalizedRequestParameters = null;
            authHeader = null;
            encodedSignature = null;

            var nonce = GenerateNonce();
            var timestamp = GenerateTimeStamp();

            string signatureBase = GenerateSignatureBase(url, Constants.ApiKey, httpMethod, timestamp,
                nonce, "HMAC-SHA1", out normalizedUrl, out normalizedRequestParameters);

            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.ASCII.GetBytes(string.Format("{0}&{1}", UrlEncode(Constants.ConsumerSecret), ""));

            string signature = GenerateSignatureUsingHash(signatureBase, hmacsha1);

            StringBuilder auth = new StringBuilder();
            auth.AppendFormat("{0}=\"{1}\", ", "oauth_consumer_key", UrlEncode(Constants.ApiKey));
            auth.AppendFormat("{0}=\"{1}\", ", "oauth_nonce", UrlEncode(nonce));
            auth.AppendFormat("{0}=\"{1}\", ", "oauth_signature", UrlEncode(signature));
            auth.AppendFormat("{0}=\"{1}\", ", "oauth_signature_method", "HMAC-SHA1");
            auth.AppendFormat("{0}=\"{1}\", ", "oauth_timestamp", timestamp);
            //auth.AppendFormat("{0}=\"{1}\", ", "oauth_token", UrlEncode(token));
            auth.AppendFormat("{0}=\"{1}\"", "oauth_version", "1.0");
            authHeader = auth.ToString();
            encodedSignature = UrlEncode(signature);

            return signature;
        }

        /// <summary> 
        /// Generate the signature value based on the given signature base and hash algorithm 
        /// </summary> 
        /// <param name="signatureBase">The signature based as produced by the GenerateSignatureBase method or by any other means</param> 
        /// <param name="hash">The hash algorithm used to perform the hashing. If the hashing algorithm requires initialization or a key it should be set prior to calling this method</param> 
        /// <returns>A base64 string of the hash value</returns> 
        public static string GenerateSignatureUsingHash(string signatureBase, HashAlgorithm hash)
        {
            return ComputeHash(hash, signatureBase);
        }

        /// <summary> 
        /// Helper function to compute a hash value 
        /// </summary> 
        /// <param name="hashAlgorithm">The hashing algorithm used. If that algorithm needs some initialization, like HMAC and its derivatives, they should be initialized prior to passing it to this function</param> 
        /// <param name="data">The data to hash</param> 
        /// <returns>a Base64 string of the hash value</returns> 
        private static string ComputeHash(HashAlgorithm hashAlgorithm, string data)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException("hashAlgorithm");
            }

            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data");
            }

            byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes(data);
            byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary> 
        /// Generate the signature base that is used to produce the signature 
        /// </summary> 
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param> 
        /// <param name="consumerKey">The consumer key</param>         
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param> 
        /// <param name="signatureType">The signature type. To use the default values use <see cref="OAuthBase.SignatureTypes">OAuthBase.SignatureTypes</see>.</param> 
        /// <returns>The signature base</returns> 
        public static string GenerateSignatureBase(Uri url, string consumerKey, string httpMethod, string timeStamp, string nonce, string signatureType, out string normalizedUrl, out string normalizedRequestParameters)
        {
            
            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException("consumerKey");
            }

            if (string.IsNullOrEmpty(httpMethod))
            {
                throw new ArgumentNullException("httpMethod");
            }

            if (string.IsNullOrEmpty(signatureType))
            {
                throw new ArgumentNullException("signatureType");
            }

            normalizedUrl = null;
            normalizedRequestParameters = null;

            List<QueryParameter> parameters = GetQueryParameters(url.Query);
            parameters.Add(new QueryParameter("oauth_version", "1.0"));
            parameters.Add(new QueryParameter("oauth_nonce", nonce));
            parameters.Add(new QueryParameter("oauth_timestamp", timeStamp));
            parameters.Add(new QueryParameter("oauth_signature_method", "HMAC-SHA1"));
            parameters.Add(new QueryParameter("oauth_consumer_key", consumerKey));
            parameters.Add(new QueryParameter("country", "ww"));

            parameters.Sort(new QueryParameterComparer());

            normalizedUrl = string.Format("{0}://{1}", url.Scheme, url.Host);
            if (!((url.Scheme == "http" && url.Port == 80) || (url.Scheme == "https" && url.Port == 443)))
            {
                normalizedUrl += ":" + url.Port;
            }
            normalizedUrl += url.AbsolutePath;
            normalizedRequestParameters = NormalizeRequestParameters(parameters);

            StringBuilder signatureBase = new StringBuilder();
            signatureBase.AppendFormat("{0}&", httpMethod.ToUpper());
            signatureBase.AppendFormat("{0}&", UrlEncode(normalizedUrl));
            signatureBase.AppendFormat("{0}", UrlEncode(normalizedRequestParameters));

            return signatureBase.ToString();
        }

        /// <summary> 
        /// Normalizes the request parameters according to the spec 
        /// </summary> 
        /// <param name="parameters">The list of parameters already sorted</param> 
        /// <returns>a string representing the normalized parameters</returns> 
        private static string NormalizeRequestParameters(IList<QueryParameter> parameters)
        {
            StringBuilder sb = new StringBuilder();
            QueryParameter p = null;
            for (int i = 0; i < parameters.Count; i++)
            {
                p = parameters[i];
                sb.AppendFormat("{0}={1}", p.Name, p.Value);

                if (i < parameters.Count - 1)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }

        /// <summary> 
        /// Internal function to cut out all non oauth query string parameters (all parameters not beginning with "oauth_") 
        /// </summary> 
        /// <param name="parameters">The query string part of the Url</param> 
        /// <returns>A list of QueryParameter each containing the parameter name and value</returns> 
        private static List<QueryParameter> GetQueryParameters(string parameters)
        {
            if (parameters.StartsWith("?"))
            {
                parameters = parameters.Remove(0, 1);
            }

            List<QueryParameter> result = new List<QueryParameter>();

            if (!string.IsNullOrEmpty(parameters))
            {
                string[] p = parameters.Split('&');
                foreach (string s in p)
                {
                    if (!string.IsNullOrEmpty(s) && !s.StartsWith("oauth_"))
                    {
                        if (s.IndexOf('=') > -1)
                        {
                            string[] temp = s.Split('=');
                            result.Add(new QueryParameter(temp[0], temp[1]));
                        }
                        else
                        {
                            result.Add(new QueryParameter(s, string.Empty));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary> 
        /// Comparer class used to perform the sorting of the query parameters 
        /// </summary> 
        protected class QueryParameterComparer : IComparer<QueryParameter>
        {

            #region IComparer<QueryParameter> Members 

            public int Compare(QueryParameter x, QueryParameter y)
            {
                if (x.Name == y.Name)
                {
                    return string.Compare(x.Value, y.Value);
                }
                else
                {
                    return string.Compare(x.Name, y.Name);
                }
            }

            #endregion
        }

        /// <summary> 
        /// Generate the timestamp for the signature         
        /// </summary> 
        /// <returns></returns> 
        private static string GenerateTimeStamp()
        {
            // Default implementation of UNIX time of the current UTC time 
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary> 
        /// Generate a nonce 
        /// </summary> 
        /// <returns></returns> 
        private static string GenerateNonce()
        {
            Random random = new Random();
            // Just a simple implementation of a random number between 123400 and 9999999 
            return random.Next(123400, 9999999).ToString();
        }

        /// <summary> 
        /// This is a different Url Encode implementation since the default .NET one outputs the percent encoding in lower case. 
        /// While this is not a problem with the percent encoding spec, it is used in upper case throughout OAuth 
        /// </summary> 
        /// <param name="value">The value to Url encode</param> 
        /// <returns>Returns a Url encoded string</returns> 
        private static string UrlEncode(string value)
        {
            string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            StringBuilder result = new StringBuilder();

            foreach (char symbol in value)
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + String.Format("{0:X2}", (int)symbol));
                }
            }

            return result.ToString();
        }

        /// <summary> 
        /// Provides an internal structure to sort the query parameter 
        /// </summary> 
        protected class QueryParameter
        {
            private string name = null;
            private string value = null;

            public QueryParameter(string name, string value)
            {
                this.name = name;
                this.value = value;
            }

            public string Name
            {
                get { return name; }
            }

            public string Value
            {
                get { return value; }
            }
        }
    }
}
