# JOSE for SyrupPay

C#로 구현한 JOSE(Javascript Object Signing and Encryption) - [RFC 7516](https://tools.ietf.org/html/rfc7516), [RFC 7515](https://tools.ietf.org/html/rfc7515) 규격입니다. 
JOSE 규격은 SyrupPay 결제 데이터 암복호화 및 AccessToken 발행 등에 사용되며 SyrupPay 서비스의 가맹점에 배포하기 위한 목적으로 라이브러리가 구현되었습니다.

자세한 사항은 [SyrupPay JOSE for c#](https://github.com/SyrupPay/jose_csharp/tree/master/SyrupPayJose)을 참고하시기 바랍니다.

# SyrupPay Token

시럽페이 서비스에서 가맹점 인증 및 데이터 교환을 위한 규격을 정의하며 전송 구간에 대한 암호화 및 무결성 보장을 위한 토큰을 생성, 관리하는 기능을 수행합니다.

자세한 사항은 [SyrupPay Token for c#](https://github.com/SyrupPay/jose_csharp/tree/master/SyrupPayToken)을 참고하시기 바랍니다.