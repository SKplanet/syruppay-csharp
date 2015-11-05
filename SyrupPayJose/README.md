# JOSE for SyrupPay

C#로 구현한 JOSE(Javascript Object Signing and Encryption) - [RFC 7516](https://tools.ietf.org/html/rfc7516), [RFC 7515](https://tools.ietf.org/html/rfc7515) 규격입니다. 
JOSE 규격은 SyrupPay 결제 데이터 암복호화 및 AccessToken 발행 등에 사용되며 SyrupPay 서비스의 가맹점에 배포하기 위한 목적으로 라이브러리가 구현되었습니다.

## Required
.NET Framework 4.0 or later

## Installation
### NuGet
`Install-Package SyrupPayJose`

## Usage
### JWE
```C#
using SyrupPayJose;
using SyrupPayJose.Jwa;

//암호화 할 데이터
var payload = "Test";
//kid : SyrupPay가 발급하는 iss
var kid = "Sample";
//SyrupPay가 발급하는 secret
//JsonWebAlgorithm.A128KW 인 경우 key bytes size가 16bytes
//JsonWebAlgorithm.A256KW 인 경우 key bytes size가 32bytes
var key = "12345678901234561234567890123456";

/*
 * JWE header 규격
 * alg : key wrap encryption algorithm. 아래 Supported JOSE encryption algorithms 참조
 * enc : content encryption algorithm. 아래 Supported JOSE encryption algorithms 참조
 */
//1. encryption
var jweToken = new Jose().Configuration(
                JoseBuilders.JsonEncryptionCompactSerializationBuilder()
                    .Header(new JoseHeader(JsonWebAlgorithm.A256KW, JsonWebAlgorithm.A128CBC_HS256, kid))
                    .Payload(payload)
                    .Key(key)
                ).Serialization();

//2. verify and decryption
var result = new Jose().Configuration(
                JoseBuilders.JsonEncryptionCompactDeserializationBuilder()
                    .SerializedSource(jweToken)
                    .Key(key)
                ).Deserialization();
```

### JWS
```C#
using SyrupPayJose;
using SyrupPayJose.Jwa;

//암호화 할 데이터
var payload = "Test";
//kid : SyrupPay가 발급하는 iss
var kid = "Sample";
//SyrupPay가 발급하는 secret
var key = "12345678901234561234567890123456";

/*
 * JWS header 규격
 * alg : signature algorithm. 아래 Supported JOSE encryption algorithms 참조
 */
//1. sign
var jwsToken = new Jose().Configuration(
                JoseBuilders.JsonSignatureCompactSerializationBuilder()
                    .Header(new JoseHeader(JsonWebAlgorithm.HS256, JsonWebAlgorithm.NONE, kid))
                    .Payload(payload)
                    .Key(key)
                ).Serialization();

//2. verify
var result = new Jose().Configuration(
                JoseBuilders.JsonSignatureCompactDeserializationBuilder()
                    .SerializedSource(jwsToken)
                    .Key(key)
                ).Deserialization();
```

## Supported JOSE encryption algorithms

### "alg" (Algorithm) Header Parameter Values For JWE
alg Param Value|Key Management Algorithm
------|------
A128KW|AES Key Wrap with default initial value using 128 bit key
A256KW|AES Key Wrap with default initial value using 256 bit key

### "enc" (Encryption Algorithm) Header Parameter Values for JWE
enc Param Value|Content Encryption Algorithm
-------------|------
A128CBC-HS256|AES_128_CBC_HMAC_SHA_256 authenticated encryption algorithm

### "alg" (Algorithm) Header Parameter Values for JWS
alg Param Value|Digital Signature or MAC Algorithm
-----|-------
HS256|HMAC using SHA-256

## License

The gem is available as open source under the terms of the [MIT License](http://opensource.org/licenses/MIT).

