Option Explicit

Dim objMctTAToken
Dim token

Set objMctTAToken = Server.CreateObject("SyrupPay.MctTAToken")
With objMctTAToken
.Iss = "SyrupPayr가 발급한 가맹점 ID"
.MctUserId = "merchantId"
.SSOCredential = "SyrupPay로 부터 발급받은 SSO"             'optional
End With

'WScript.Echo objMctTAToken.ToJson()
token = objMctTAToken.Serialzie("Syrup Pay에서 발급한 가맹점 Secret")

Set objMctTAToken = Nothing
