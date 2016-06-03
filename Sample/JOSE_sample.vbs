Option Explicit

Dim objJoseSerialize, objJoseDeserilaize 
Dim sJweToken, sJwsToken
Dim sPayload

Dim sIss, sKey

sIss = "sample"
sKey = "12345678901234567890123456789012"

'JWE Encryption
set objJoseSerialize = CreateObject("SyrupPay.JoseSerializer")
sJweToken = objJoseSerialize.toJwe(sIss, "A256KW", "A128CBC-HS256", sKey, "apple")
WScript.Echo "JWE : " & sJweToken

'JWE Decryption
set objJoseDeserilaize = CreateObject("SyrupPay.JoseDeserializer")
sPayload = objJoseDeserilaize.fromJose(sKey, sJweToken)
WScript.Echo "JWE SRC : " & sPayload

'JWS Sign
sJwsToken = objJoseSerialize.toJws(sIss, "HS256", sKey, "apple")
WScript.Echo "JWS : " & sJwsToken

'JWS Verify
sPayload = objJoseDeserilaize.fromJose(skey, sJwsToken)
WScript.Echo "JWS SRC : " & sPayload

Set objJoseSerialize = Nothing
Set objJoseDeserilaize = Nothing

