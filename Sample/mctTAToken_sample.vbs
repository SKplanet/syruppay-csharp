Option Explicit

Dim lUnixtime
Dim objMctTAToken

lUnixtime = DateDiff("s", "01/01/1970 00:00:00", Now())

Set objMctTAToken = CreateObject("SyrupPay.MctTAToken")
With objMctTAToken
.Iss = "syruppay"
.Iat = lUnixtime
.MctUserId = "merchantId"
.ExtraUserId = "extraUserId"
.ImplicitSSOSeed = "seed"
.SSOCredential = "SSO"
.DeviceIdentifier = "deviceId"
.MappingType = "CI_HASH"
.MappingValue = "CI"
.MctTransAuthId = "authId"
.MctDefinedValue = "definedValue"
.ProductTitle = "title"
.ProductUrl = "http://sample.com"
.ProductUrl = "http://productUrl.com"
.Lang = "ko"
.CurrencyCode = "KRW"
.PaymentAmt = 1000
.ShippingAddress = "a2:kr|137-332|서울 서초구 잠원동 하나아파트|1동 1호||"
.DeliveryPhoneNumber = "01041110194"
.DeliveryName = "Test"
.AddCardInfo "11", "NN1;NN2;YY3;YY4;YY5;NH6"
.AddCardInfo "22", "YY2;NN2;YY3;YY4;YY5;NH6"
.IsExchangeable = true
.CardIssuerRegion = "ALLOWED:kor"
.PaymentInfoMatchedUser = "CI_MATCHED_ONLY"
.AutoPaymentId = "Auto"
.AutoPaymentMatchedUser = "CI_MATCHED_ONLY"
End With

WScript.Echo objMctTAToken.ToJson()
WScript.Echo objMctTAToken.Serialzie("12345678901234567890123456789012")

Set objMctTAToken = Nothing
