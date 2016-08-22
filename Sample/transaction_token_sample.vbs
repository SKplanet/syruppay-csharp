Option Explicit

Dim objMctTAToken
Dim token

Set objMctTAToken = Server.CreateObject("SyrupPay.MctTAToken")
With objMctTAToken
.Iss = "������ ID"
.MctUserId = "�������� ȸ�� ID �Ǵ� �ĺ���"
.SSOCredential = "�߱� ���� SSO"             'optional
.MctTransAuthId = "���������� �����ϴ� �ֹ� ID"
.ProductTitle = "��ǰ��"
.ProductUrl = "http://deal.11st.co.kr/product/prdNo=1122841340"         'optional
.ProductUrl = "http://deal.11st.co.kr/product/prdNo=1265508741"         'optional
.Lang = "ko"
.CurrencyCode = "KRW"
.PaymentAmt = 50000
.ShippingAddress = "a2:kr|137-332|���� ���ʱ� ����� �ϳ�����Ʈ|1�� 1ȣ||"     'optional
.DeliveryPhoneNumber = "01011112222"            'optional
.DeliveryName = "��� ������"                     'optional
.AddCardInfo "ī�屸�� �ڵ�", "�Һ�����"            'optional
.IsExchangeable = false                          'optional
.PaymentType = "MOBILE;BANK"
End With

'WScript.Echo objMctTAToken.ToJson()
token = objMctTAToken.Serialzie("�÷����̰� �߱��� ������ Secret")

Set objMctTAToken = Nothing

