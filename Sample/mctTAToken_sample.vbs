Option Explicit

Dim objMctTAToken
Dim apiKey, encKey, iss

apiKey = "syrupPay_API_Key"     '�÷����̰� �����ϴ� API Key
encKey = "syrupPay_Secret"      '�÷����̰� �߱��ϴ� Secret
iss = "syrupPay_merchantID"     '�÷����� �߱��ϴ� ������ ID

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
'.ShippingAddress = "a2:kr|137-332|���� ���ʱ� ����� �ϳ�����Ʈ|1�� 1ȣ||"    'optional
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

