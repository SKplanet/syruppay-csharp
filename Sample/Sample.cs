using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using SyrupPayJose;
using SyrupPayToken.Claims;
using SyrupPayToken;
using Newtonsoft.Json;
using System.Diagnostics;

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
            string authorization = "Basic "+apiKey;
            Tuple<int, Dictionary<string, object>> result = Request(url, encKey, authorization, payload);

            return result;
        }

        public static Tuple<int, Dictionary<string, object>> RequestPaymentCancel(string apiKey, string encKey, string iss)
        {
            string url = "{baseAPIURL}/v1/api-basic/payment/refund";

            string payload = GetCancelPayload(encKey, iss);
            string authorization = "Basic " + apiKey;
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
}
