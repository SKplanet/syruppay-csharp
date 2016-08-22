Option Explicit

Dim objMctTAToken
Dim token

Set objMctTAToken = Server.CreateObject("SyrupPay.MctTAToken")
With objMctTAToken
.Iss = "가맹점 ID"
.MctUserId = "가맹점의 회원 ID 또는 식별자"
.SSOCredential = "발급 받은 SSO"             'optional
.MctTransAuthId = "가맹점에서 생성하는 주문 ID"
.ProductTitle = "제품명"
.ProductUrl = "http://deal.11st.co.kr/product/prdNo=1122841340"         'optional
.ProductUrl = "http://deal.11st.co.kr/product/prdNo=1265508741"         'optional
.Lang = "ko"
.CurrencyCode = "KRW"
.PaymentAmt = 50000
.ShippingAddress = "a2:kr|137-332|서울 서초구 잠원동 하나아파트|1동 1호||"     'optional
.DeliveryPhoneNumber = "01011112222"            'optional
.DeliveryName = "배송 수신자"                     'optional
.AddCardInfo "카드구분 코드", "할부정보"            'optional
.IsExchangeable = false                          'optional
.PaymentType = "MOBILE;BANK"
End With

'WScript.Echo objMctTAToken.ToJson()
token = objMctTAToken.Serialzie("시럽페이가 발급한 가맹점 Secret")

Set objMctTAToken = Nothing

