Option Explicit

Dim objMctTAToken
Dim token

Set objMctTAToken = Server.CreateObject("SyrupPay.MctTAToken")
With objMctTAToken
.Iss = "������ ID"
.MctUserId = "�������� ȸ�� ID �Ǵ� �ĺ���"
.SSOCredential = "�߱� ���� SSO"             'optional
.AutoPaymentId = "�÷����̰� �ڵ����� ����� ������ �ڵ����� id"	'optional
End With

'WScript.Echo objMctTAToken.ToJson()
token = objMctTAToken.Serialzie("�÷����̰� �߱��� ������ Secret")

Set objMctTAToken = Nothing

