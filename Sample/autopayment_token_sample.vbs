Option Explicit

Dim objMctTAToken
Dim token

Set objMctTAToken = Server.CreateObject("SyrupPay.MctTAToken")
With objMctTAToken
.Iss = "가맹점 ID"
.MctUserId = "가맹점의 회원 ID 또는 식별자"
.SSOCredential = "발급 받은 SSO"             'optional
.AutoPaymentId = "시럽페이가 자동결제 등록후 전달한 자동결제 id"	'optional
End With

'WScript.Echo objMctTAToken.ToJson()
token = objMctTAToken.Serialzie("시럽페이가 발급한 가맹점 Secret")

Set objMctTAToken = Nothing

