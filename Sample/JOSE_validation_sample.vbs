Option Explicit

Dim objJOSEDeserilaize
Dim sJsonData

Dim sKey

sKey = "�÷����̰� �߱��� ������ Secret"

set objJOSEDeserilaize = Server.CreateObject("SyrupPay.JoseDeserializer")
sJsonData = objJOSEDeserilaize.fromJose(sKey, "Syrup Pay�� ������ jwt����� value")

'WScript.Echo sJsonData

Set objJOSEDeserilaize = Nothing

