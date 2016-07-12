﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using SyrupPayJose;
using SyrupPayToken.Claims;
using SyrupPayToken;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Sockets;

namespace SyrupPay.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            string apiKey = "";     //API Authorization
            string encKey = "";     //JWS, JWE 암복호화 키
            string iss = "";        //시럽페이 발급 ID

            //거래 승인 요청 예제
            Tuple<int, Dictionary<string, object>> approvalResponse = RequestPaymentApproval(apiKey, encKey, iss);
            HandleResponse(approvalResponse);

            //거래 취소 요청 예제
            Tuple<int, Dictionary<string, object>> cancelResponse = RequestPaymentCancel(apiKey, encKey, iss);
            HandleResponse(cancelResponse);
        }

        public static void HandleResponse(Tuple<int, Dictionary<string, object>> response)
        {
            //응답 규격에 따른 결과 처리
            Dictionary<string, object> result = null;
            if (response.Item1 == 200)
            {
                result = response.Item2;
            }
            else
            {
                //response null일 경우 처리. ex) 암복호화 오류 등.
                if (response.Item2 != null)
                    result = response.Item2;
            }
        }

        public static Tuple<int, Dictionary<string, object>> RequestPaymentApproval(string apiKey, string encKey, string iss)
        {
            string url = "{baseAPIURL}/v1/api-basic/payment/approval";

            string payload = GetApprovalPayload(encKey, iss);
            string authorization = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{0}:{1}", iss, apiKey)));
            Tuple<int, Dictionary<string, object>> result = Request(url, encKey, authorization, payload);

            return result;
        }

        public static Tuple<int, Dictionary<string, object>> RequestPaymentCancel(string apiKey, string encKey, string iss)
        {
            string url = "{baseAPIURL}/v1/api-basic/payment/refund";

            string payload = GetCancelPayload(encKey, iss);
            string authorization = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{0}:{1}", iss, apiKey)));
            Tuple<int, Dictionary<string, object>> result = Request(url, encKey, authorization, payload);

            return result;
        }

        public static string GetApprovalPayload(string encKey, string iss, bool isJose = true)
        {
            //배포된 규격서의 거래승인요청 전문에 따라 구성
            var approvalPayload = new Dictionary<string, object>();
            approvalPayload["mctRequestId"] = "";
            approvalPayload["mctRequestTime"] = 0;
            approvalPayload["mctTransAuthId"] = "";
            approvalPayload["mctPaymentCode"] = "";
            approvalPayload["paymentAmt"] = 0;
            approvalPayload["taxFreeAmt"] = 0;
            approvalPayload["ocTransAuthId"] = "";
            approvalPayload["tranAuthValue"] = "";

            var submallInfo = new Dictionary<string, object>();
            submallInfo["id"] = "";
            submallInfo["officeName"] = "";
            submallInfo["businessRegNumber"] = "";
            submallInfo["lineNumber"] = "";
            submallInfo["address"] = "";
            submallInfo["owner"] = "";
            approvalPayload["submallInfo"] = submallInfo;

            var mctAdditionalProductInfo = new Dictionary<string, object>();
            mctAdditionalProductInfo["unitType"] = "";
            mctAdditionalProductInfo["unitsPurchased"] = 0;
            mctAdditionalProductInfo["pricePerUnit"] = 0;
            mctAdditionalProductInfo["currencyCode"] = "";
            mctAdditionalProductInfo["unitDimension"] = "";
            approvalPayload["mctAdditionalProductInfo"] = mctAdditionalProductInfo;

            //using Newtonsoft.Json package
            var payload = JsonConvert.SerializeObject(approvalPayload);

            if (isJose)
            {
                payload = new Jose().Configuration(
                JoseBuilders.JsonEncryptionCompactSerializationBuilder()
                    .Header(new JoseHeader(SyrupPayJose.Jwa.JsonWebAlgorithm.A256KW, SyrupPayJose.Jwa.JsonWebAlgorithm.A128CBC_HS256, iss))
                    .Payload(payload)
                    .Key(encKey)
                ).Serialization();
            }

            return payload;
        }

        public static string GetCancelPayload(string encKey, string iss, bool isJose = true)
        {
            //배포된 규격서의 취소요청 전문에 따라 구성
            var cancelPayload = new Dictionary<string, object>();
            cancelPayload["mctRequestId"] = "";
            cancelPayload["mctRequestTime"] = 0;
            cancelPayload["ocTransApproveNo"] = "";
            cancelPayload["refundType"] = "ALL";
            cancelPayload["refundPaymentAmt"] = 0;
            cancelPayload["refundTaxFreeAmt"] = 0;

            //using Newtonsoft.Json package
            var payload = JsonConvert.SerializeObject(cancelPayload);

            if (isJose)
            {
                payload = new Jose().Configuration(
                JoseBuilders.JsonEncryptionCompactSerializationBuilder()
                    .Header(new JoseHeader(SyrupPayJose.Jwa.JsonWebAlgorithm.A256KW, SyrupPayJose.Jwa.JsonWebAlgorithm.A128CBC_HS256, iss))
                    .Payload(payload)
                    .Key(encKey)
                ).Serialization();
            }

            return payload;
        }

        public static string GetMCTTAToken(string encKey, string iss)
        {
            ///시럽페이 Token 라이브러리를 이용한 Token 생성
            return new SyrupPayTokenBuilder().Of(iss)
               .Login()
                   .WithMerchantUserId("loginInfo.mctUserId")
                   .WithExtraMerchantUserId("loginInfo.extraUserId")
                   .WithSsoCredential("loginInfo.SSOCredential")
                   .WithDeviceIdentifier("loginInfo.deviceIdentifier")
               .And()
               .MapToSyrupPayUser()
                   .WithType(MapToSyrupPayUserConfigurer<SyrupPayTokenBuilder>.MappingType.CI_HASH)
                   //.WithType(MapToSyrupPayUserConfigurer<SyrupPayTokenBuilder>.MappingType.CI_MAPPED_KEY)
                   .WithValue("userInfoMapper.mappingValue")
               .And()
               .Pay()
                    .WithOrderIdOfMerchant("transactionInfo.mctTransAuthId")
                    .WithMerchantDefinedValue("transactionInfo.mctDefinedValue")
                    .WithProductTitle("transactionInfo.paymentInfo.productTitle")
                    .WithProductUrls("https://www.sample.com")
                    .WithLanguageForDisplay(PayConfigurer<SyrupPayTokenBuilder>.Language.KO)    //transactionInfo.paymentInfo.lang
                    .WithCurrency(PayConfigurer<SyrupPayTokenBuilder>.Currency.KRW)             //transactionInfo.paymentInfo.currencyCode
                    .WithAmount(1000)                                                           //transactionInfo.paymentInfo.paymentAmt
                                                                                                //transactionInfo.paymentInfo.shippingAddress
                    .WithShippingAddress(new PayConfigurer<SyrupPayTokenBuilder>.ShippingAddress("Zipcode", "Main Address", "Detail Address", "City", "", "kr"))
                    .WithDeliveryPhoneNumber("transactionInfo.paymentInfo.deliveryPhoneNumber")
                    .WithDeliveryName("transactionInfo.paymentInfo.deliveryName")
                    .WithInstallmentPerCardInformation(
                        new PayConfigurer<SyrupPayTokenBuilder>.CardInstallmentInformation("11", "NN1;NN2;YY3;YY4;YY5;NH6"),
                        new PayConfigurer<SyrupPayTokenBuilder>.CardInstallmentInformation("22", "NN1;NN2;YY3;YY4;YY5;NH6")
                    )  //transaction.paymentInfo.cardInfoList
                    .WithBeAbleToExchangeToCash(false)  //transaction.paymentInfo.isExchangeable
                                                        //transaction.paymentRestrictions.cardIssuerRegion
                    .WithPayableRuleWithCard(PayConfigurer<SyrupPayTokenBuilder>.PayableLocaleRule.ONLY_ALLOWED_KOR)
               //.WithPayableRuleWithCard(PayConfigurer<SyrupPayTokenBuilder>.PayableLocaleRule.ONLY_ALLOWED_USA)
               //.WithPayableRuleWithCard(PayConfigurer<SyrupPayTokenBuilder>.PayableLocaleRule.ONLY_NOT_ALLOED_KOR)
               //.WithPayableRuleWithCard(PayConfigurer<SyrupPayTokenBuilder>.PayableLocaleRule.ONLY_NOT_ALLOED_USA)
               .And()
               .Subscription()
                    .WithAutoPaymentId("subscription.autoPaymentId")
                    //subscription.registrationRestrictions.matchedUser
                    .WithMatchedUser(PayConfigurer<SyrupPayTokenBuilder>.MatchedUser.CI_MATCHED_ONLY)
                .And()
               .GenerateTokenBy(encKey);
        }

#if NET40 || NET45
        public static Tuple<int, Dictionary<string, object>> Request(string url, string encKey, string authorization, string requestBody, bool isJose = true)
        {
            byte[] b = Encoding.UTF8.GetBytes(requestBody);

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            httpWebRequest.Method = "POST";
            if (isJose)
            {
                httpWebRequest.ContentType = "application/jose;charset=utf-8";
                httpWebRequest.Accept = "application/jose;charset=utf-8";
            }
            else
            {
                httpWebRequest.ContentType = "application/json;charset=utf-8";
                httpWebRequest.Accept = "application/json;charset=utf-8";
            }
            
            httpWebRequest.Headers.Add("Authorization", authorization);
            httpWebRequest.ContentLength = b.Length;

            using (var requestStream = httpWebRequest.GetRequestStream())
                requestStream.Write(b, 0, b.Length);

            string contentType = null, responseBody = null;
            int statusCode = (int) HttpStatusCode.OK;
            Dictionary<string, object> payload = null;

            try
            {
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    contentType = httpWebResponse.ContentType.ToLower();

                    using (var resposneStream = httpWebResponse.GetResponseStream())
                    using (var streamReader = new StreamReader(resposneStream, Encoding.UTF8))
                    {
                        responseBody = streamReader.ReadToEnd();
                    }
                }
            } catch (WebException e)
            {
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)e.Response)
                {
                    contentType = httpWebResponse.ContentType.ToLower();
                    statusCode = (int)httpWebResponse.StatusCode;
                    Console.WriteLine("Content-Type: " + contentType);
                    Console.WriteLine("Status Code: " + statusCode);

                    using (var resposneStream = httpWebResponse.GetResponseStream())
                    using (var streamReader = new StreamReader(resposneStream, Encoding.UTF8))
                    {
                        responseBody = streamReader.ReadToEnd();
                    }
                }
            }
            finally
            {
                if (!String.IsNullOrEmpty(responseBody))
                {
                    if (contentType.IndexOf("jose") > 0)
                    {
                        responseBody = new Jose().Configuration(
                        JoseBuilders.CompactDeserializationBuilder()
                            .SerializedSource(responseBody)
                            .Key(encKey)
                        ).Deserialization();
                    }

                    payload = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);
                }
            }

            return Tuple.Create(statusCode, payload);
        }
    }
#else
        class HttpResult
        {
            private int statusCode;
            private string contentType;
            private string responseBody;

            public int StatusCode
            {
                get { return statusCode; }
                set { statusCode = value; }
            }

            public string ContentType
            {
                get { return contentType; }
                set { contentType = value; }
            }

            public string ResponseBody
            {
                get { return responseBody; }
                set { responseBody = value; }
            }
        }

        static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        static Tuple<int, Dictionary<string, object>> Request(string url, string encKey, string authorization, string requestBody, bool isJose = true)
        {
            HttpResult httpResult = new HttpResult();
            Dictionary<string, object> payload = null;

            try
            {
                using (TcpClient client = new TcpClient(new Uri(url).Host, 443))
                {
                    using (SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null))
                    {
                        sslStream.AuthenticateAsClient(String.Format("https://{0}/", new Uri(url).Host));
                        string requestData = GetRequestData(url, authorization, requestBody, isJose);
                        byte[] message = Encoding.UTF8.GetBytes(requestData);
                        sslStream.Write(message, 0, message.Length);
                        sslStream.Flush();

                        httpResult = GetResponse(sslStream);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                if (!Object.ReferenceEquals(null, httpResult) && !String.IsNullOrEmpty(httpResult.ResponseBody))
                {
                    if (httpResult.ContentType.ToLower().IndexOf("jose") > 0)
                    {
                        Debug.WriteLine("JOSE Response: " + httpResult.ResponseBody);
                        httpResult.ResponseBody = new Jose().Configuration(
                        JoseBuilders.CompactDeserializationBuilder()
                            .SerializedSource(httpResult.ResponseBody)
                            .Key(encKey)
                        ).Deserialization();
                    }

                    payload = JsonConvert.DeserializeObject<Dictionary<string, object>>(httpResult.ResponseBody);
                }
            }

            Debug.WriteLine(String.Format("statusCode: {0}, Content-Type: {1}, ResponseBody: {2}",
                httpResult.StatusCode, httpResult.ContentType, httpResult.ResponseBody));

            return Tuple.Create(httpResult.StatusCode, payload);
        }

        static string GetRequestData(string url, string authorization, string requestBody, bool isJose = true)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("POST {0} HTTP/1.1", new Uri(url).LocalPath));
            if (isJose)
            {
                sb.AppendLine("Content-Type: application/jose; charset=utf-8");
                sb.AppendLine("Accept: application/jose;charset=utf-8");
            }
            else
            {
                sb.AppendLine("Content-Type: application/json; charset=utf-8");
                sb.AppendLine("Accept: application/json;charset=utf-8");
            }

            sb.AppendLine(String.Format("Authorization: {0}", authorization));
            sb.AppendLine(String.Format("Host: {0}", new Uri(url).Host));
            sb.AppendLine(String.Format("Content-Length: {0}", Encoding.UTF8.GetBytes(requestBody).Length));
            sb.AppendLine("");
            sb.AppendLine(requestBody);

            return sb.ToString();
        }

        static HttpResult GetResponse(SslStream sslStream)
        {
            HttpResult httpResult = new HttpResult();
            byte[] buffer = new byte[4096];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;
            do
            {
                bytes = sslStream.Read(buffer, 0, buffer.Length);
                Decoder decoder = Encoding.UTF8.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);
                if (messageData.ToString().IndexOf("<EOF>") != -1)
                {
                    break;
                }
            } while (bytes != 0);

            string[] headers = messageData.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            httpResult.StatusCode = Int32.Parse(headers[0].Split(' ')[1]);

            for (int i = 1; i < headers.Length; i++)
            {
                //header end
                if (String.IsNullOrEmpty(headers[i]))
                {
                    break;
                }

                string[] token = headers[i].Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                if (!String.IsNullOrEmpty(token[0]) && !String.IsNullOrEmpty(token[1]))
                {
                    if ((token[0].Equals("content-type", StringComparison.OrdinalIgnoreCase)))
                    {
                        httpResult.ContentType = token[1];
                    }
                }
            }

            string[] headerAndBody = messageData.ToString().Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None);
            if (headerAndBody.Length == 2 && !String.IsNullOrEmpty(headerAndBody[1]))
            {
                httpResult.ResponseBody = headerAndBody[1];
            }

            return httpResult;
        }
    }
#endif

#if NET20 || NET35
        public class Tuple<T1>
    {
        public Tuple(T1 item1)
        {
            Item1 = item1;
        }

        public T1 Item1 { get; set; }
    }

    public class Tuple<T1, T2> : Tuple<T1>
    {
        public Tuple(T1 item1, T2 item2) : base(item1)
        {
            Item2 = item2;
        }

        public T2 Item2 { get; set; }
    }

    public class Tuple<T1, T2, T3> : Tuple<T1, T2>
    {
        public Tuple(T1 item1, T2 item2, T3 item3) : base(item1, item2)
        {
            Item3 = item3;
        }

        public T3 Item3 { get; set; }
    }

    public static class Tuple
    {
        public static Tuple<T1> Create<T1>(T1 item1)
        {
            return new Tuple<T1>(item1);
        }

        public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
        {
            return new Tuple<T1, T2>(item1, item2);
        }

        public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
        {
            return new Tuple<T1, T2, T3>(item1, item2, item3);
        }
    }
#endif

}
