Option Explicit

Dim objMctTAToken
Dim token

Set objMctTAToken = Server.CreateObject("SyrupPay.MctTAToken")
With objMctTAToken
.Iss = "SyrupPayr�� �߱��� ������ ID"
.MctUserId = "merchantId"
.SSOCredential = "SyrupPay�� ���� �߱޹��� SSO"             'optional
End With

'WScript.Echo objMctTAToken.ToJson()
token = objMctTAToken.Serialzie("Syrup Pay���� �߱��� ������ Secret")

Set objMctTAToken = Nothing
