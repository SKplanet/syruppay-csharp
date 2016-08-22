Option Explicit

Dim objJOSEDeserilaize
Dim sJsonData

Dim sKey

sKey = "시럽페이가 발급한 가맹점 Secret"

set objJOSEDeserilaize = Server.CreateObject("SyrupPay.JoseDeserializer")
sJsonData = objJOSEDeserilaize.fromJose(sKey, "Syrup Pay가 생성한 jwt기반의 value")

'WScript.Echo sJsonData

Set objJOSEDeserilaize = Nothing

