Option Explicit

Dim objMctTAToken
Dim apiKey, encKey, iss

apiKey = "syrupPay_API_Key"     '시럽페이가 발행하는 API Key
encKey = "syrupPay_Secret"      '시럽페이가 발급하는 Secret
iss = "syrupPay_merchantID"     '시럽페이 발급하는 가맹점 ID

Set objMctTAToken = CreateObject("SyrupPay.MctTAToken")
With objMctTAToken
.Iss = "syruppay"
.MctUserId = "merchantId"
'.ExtraUserId = "extraUserId"       'optional
'.ImplicitSSOSeed = "seed"          'optional
.SSOCredential = "SSO"             'optional
.isNotApplicableSso
'.DeviceIdentifier = "deviceId"     'optional
'.MappingType = "CI_HASH"           'optional
'.MappingValue = "CI"               'optional
.MctTransAuthId = "authId"
'.MctDefinedValue = "definedValue"  'optional
.ProductTitle = "title"
.ProductUrl = "http://sample.com"
.ProductUrl = "http://productUrl.com"
.Lang = "ko"
.CurrencyCode = "KRW"
.PaymentAmt = 1000
'.ShippingAddress = "a2:kr|137-332|서울 서초구 잠원동 하나아파트|1동 1호||"    'optional
'.DeliveryPhoneNumber = "01041110194"           'optional
'.DeliveryName = "Test"                         'optional
'.AddCardInfo "11", "NN1;NN2;YY3;YY4;YY5;NH6"   'optional
'.AddCardInfo "22", "YY2;NN2;YY3;YY4;YY5;NH6"   'optional
'.IsExchangeable = true                         'optional
.CardIssuerRegion = "ALLOWED:kor"
'.PaymentInfoMatchedUser = "CI_MATCHED_ONLY"    'optional
'.AutoPaymentId = "Auto"                        'optional
.SubscriptionPromotionCode = "12345678901234567890123456789012"
'.AutoPaymentMatchedUser = "CI_MATCHED_ONLY"    'optional
End With

WScript.Echo objMctTAToken.ToJson()
WScript.Echo objMctTAToken.Serialzie(encKey)

Set objMctTAToken = Nothing

