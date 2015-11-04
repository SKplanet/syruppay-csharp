# SyrupPay Token

시럽페이 서비스에서 가맹점 인증 및 데이터 교환을 위한 규격을 정의하며 전송 구간에 대한 암호화 및 무결성 보장을 위한 토큰을 생성, 관리하는 기능을 수행합니다.

## Required
.NET Framework 4.0 or later

## Installation
### NuGet
`Install-Package SyrupPayToken`

## Usage
### 회원가입, 로그인, 설정과 같은 사용자 정보에 접근하기 위한 Syrup Pay Token 생성
```C#
using SyrupPayToken.jwt;
using SyrupPayToken.Claims;
using SyrupPayToken;

//SyrupPay 발급 secret
var key = "12345678901234561234567890123456";

//Token 생성
String token = new SyrupPayTokenBuilder().Of("SyrupPay 발급 가맹점 ID")
                .Login()
                    .WithMerchantUserId("가맹점의 회원 ID 또는 식별자")
                    .WithExtraMerchantUserId("핸드폰과 같이 회원 별 추가 ID 체계가 존재할 경우 입력")
                    .WithSsoCredential("SSO 를 발급 받았을 경우 입력")
                .And()
                .GenerateTokenBy(key);

//Token 검증
IToken<SyrupPayTokenBuilder> t = SyrupPayTokenBuilder.Verify(token, key);

```
### token 생성 결과
```
eyJhbGciOiJIUzI1NiIsImtpZCI6IlN5cnVwUGF5IOuwnOq4iSDqsIDrp7nsoJAgSUQiLCJ0eXAiOiJKV1QifQ.eyJhdWQiOiJodHRwczovL3BheS5zeXJ1cC5jby5rciIsInR5cCI6Impvc2UiLCJpc3MiOiJTeXJ1cFBheSDrsJzquIkg6rCA66e57KCQIElEIiwiZXhwIjoxNDQ2NjI1MDUxLCJpYXQiOjE0NDY2MjQ0NTEsImp0aSI6IjMyMjcwNzQwLTRiOTMtNDA3MC05YTRhLTI5ZDYwZTA0ZGIxMSIsIm5iZiI6MCwibG9naW5JbmZvIjp7Im1jdFVzZXJJZCI6IuqwgOunueygkOydmCDtmozsm5AgSUQg65iQ64qUIOyLneuzhOyekCIsImV4dHJhVXNlcklkIjoi7ZW465Oc7Y-w6rO8IOqwmeydtCDtmozsm5Ag67OEIOy2lOqwgCBJRCDssrTqs4TqsIAg7KG07J6s7ZWgIOqyveyasCDsnoXroKUiLCJTU09DcmVkZW50aWFsIjoiU1NPIOulvCDrsJzquIkg67Cb7JWY7J2EIOqyveyasCDsnoXroKUifX0.njn3KaCirax6APxoegPNH7vvexKM4G_H0rdyC5yLsyg
```

### token value
```json
{
  "aud": "https://pay.syrup.co.kr",
  "typ": "jose",
  "iss": "SyrupPay 발급 가맹점 ID",
  "exp": 1446625051,
  "iat": 1446624451,
  "jti": "32270740-4b93-4070-9a4a-29d60e04db11",
  "nbf": 0,
  "loginInfo": {
    "mctUserId": "가맹점의 회원 ID 또는 식별자",
    "extraUserId": "핸드폰과 같이 회원 별 추가 ID 체계가 존재할 경우 입력",
    "SSOCredential": "SSO 를 발급 받았을 경우 입력"
  }
}
```

### 결재 인증을 위한 Syrup Pay Token 생성
```C#
using SyrupPayToken.jwt;
using SyrupPayToken.Claims;
using SyrupPayToken;

//SyrupPay 발급 secret
var key = "12345678901234561234567890123456";

//Token 생성
String token = new SyrupPayTokenBuilder().Of("SyrupPay 발급 가맹점 ID")
				.Pay()
					.WithOrderIdOfMerchant("가맹점에서 관리하는 주문고유ID")
					.WithProductTitle("제품명")
					.WithProductUrls(
						"http://deal.11st.co.kr/product/SellerProductDetail.tmall?method=getSellerProductDetail&prdNo=1122841340",
						"http://deal.11st.co.kr/product/SellerProductDetail.tmall?method=getSellerProductDetail&prdNo=1265508741"
						)
					.WithLanguageForDisplay(PayConfigurer<SyrupPayTokenBuilder>.Language.KO)
					.WithAmount(50000)
					.WithCurrency(PayConfigurer<SyrupPayTokenBuilder>.Currency.KRW)
					.WithShippingAddress(new PayConfigurer<SyrupPayTokenBuilder>.ShippingAddress("137-332", "서초구 잠원동 하나아파트", "1동 1호", "서울", "", "kr"))
					.WithDeliveryPhoneNumber("01011112222")
					.WithDeliveryName("배송 수신자")
					.WithInstallmentPerCardInformation(new PayConfigurer<SyrupPayTokenBuilder>.CardInstallmentInformation("카드구분 코드", "할부정보. ex. NN1;NN2;YY3;YY4;YY5;NH6"))
					.WithBeAbleToExchangeToCash(false)
					.WithPayableRuleWithCard(PayConfigurer<SyrupPayTokenBuilder>.PayableLocaleRule.ONLY_ALLOWED_KOR)
				.And()
				.GenerateTokenBy(key);

//Token 검증
IToken<SyrupPayTokenBuilder> t = SyrupPayTokenBuilder.Verify(token, key);
```

### token 생성 결과
```
eyJhbGciOiJIUzI1NiIsImtpZCI6IlN5cnVwUGF5IOuwnOq4iSDqsIDrp7nsoJAgSUQiLCJ0eXAiOiJKV1QifQ.eyJhdWQiOiJodHRwczovL3BheS5zeXJ1cC5jby5rciIsInR5cCI6Impvc2UiLCJpc3MiOiJTeXJ1cFBheSDrsJzquIkg6rCA66e57KCQIElEIiwiZXhwIjoxNDQ2NjI1Mjc1LCJpYXQiOjE0NDY2MjQ2NzUsImp0aSI6ImZjMGU2ZDAzLTg1ZjAtNDU4Mi04NDcwLTUzOTcyOTNkMTVkMCIsIm5iZiI6MCwidHJhbnNhY3Rpb25JbmZvIjp7Im1jdFRyYW5zQXV0aElkIjoi6rCA66e57KCQ7JeQ7IScIOq0gOumrO2VmOuKlCDso7zrrLjqs6DsnKBJRCIsInBheW1lbnRJbmZvIjp7ImNhcmRJbmZvTGlzdCI6W3siY2FyZENvZGUiOiLsubTrk5zqtazrtoQg7L2U65OcIiwibW9udGhseUluc3RhbGxtZW50SW5mbyI6Iu2VoOu2gOygleuztC4gZXguIE5OMTtOTjI7WVkzO1lZNDtZWTU7Tkg2In1dLCJwcm9kdWN0VGl0bGUiOiLsoJztkojrqoUiLCJwcm9kdWN0VXJscyI6WyJodHRwOi8vZGVhbC4xMXN0LmNvLmtyL3Byb2R1Y3QvU2VsbGVyUHJvZHVjdERldGFpbC50bWFsbD9tZXRob2Q9Z2V0U2VsbGVyUHJvZHVjdERldGFpbCZwcmRObz0xMTIyODQxMzQwIiwiaHR0cDovL2RlYWwuMTFzdC5jby5rci9wcm9kdWN0L1NlbGxlclByb2R1Y3REZXRhaWwudG1hbGw_bWV0aG9kPWdldFNlbGxlclByb2R1Y3REZXRhaWwmcHJkTm89MTI2NTUwODc0MSJdLCJsYW5nIjoiS08iLCJjdXJyZW5jeUNvZGUiOiJLUlciLCJwYXltZW50QW10Ijo1MDAwMCwic2hpcHBpbmdBZGRyZXNzIjoia3J8MTM3LTMzMnzshJzstIjqtawg7J6g7JuQ64-ZIO2VmOuCmOyVhO2MjO2KuHwx64-ZIDHtmLh87ISc7Jq4fHwiLCJkZWxpdmVyeVBob25lTnVtYmVyIjoiMDEwMTExMTIyMjIiLCJkZWxpdmVyeU5hbWUiOiLrsLDshqEg7IiY7Iug7J6QIiwiaXNFeGNoYW5nZWFibGUiOmZhbHNlfSwicGF5bWVudFJlc3RyaWN0aW9ucyI6eyJjYXJkSXNzdWVyUmVnaW9uIjoiQUxMT1dFRDpLT1IifX19.rxNxhqKwWDxDisWFm0tFGeM-133mndperUiYeRn9ydo
```

### token value
```json
{
  "aud": "https://pay.syrup.co.kr",
  "typ": "jose",
  "iss": "SyrupPay 발급 가맹점 ID",
  "exp": 1446625275,
  "iat": 1446624675,
  "jti": "fc0e6d03-85f0-4582-8470-5397293d15d0",
  "nbf": 0,
  "transactionInfo": {
    "mctTransAuthId": "가맹점에서 관리하는 주문고유ID",
    "paymentInfo": {
      "cardInfoList": [
        {
          "cardCode": "카드구분 코드",
          "monthlyInstallmentInfo": "할부정보. ex. NN1;NN2;YY3;YY4;YY5;NH6"
        }
      ],
      "productTitle": "제품명",
      "productUrls": [
        "http://deal.11st.co.kr/product/SellerProductDetail.tmall?method=getSellerProductDetail&prdNo=1122841340",
        "http://deal.11st.co.kr/product/SellerProductDetail.tmall?method=getSellerProductDetail&prdNo=1265508741"
      ],
      "lang": "KO",
      "currencyCode": "KRW",
      "paymentAmt": 50000,
      "shippingAddress": "kr|137-332|서초구 잠원동 하나아파트|1동 1호|서울||",
      "deliveryPhoneNumber": "01011112222",
      "deliveryName": "배송 수신자",
      "isExchangeable": false
    },
    "paymentRestrictions": {
      "cardIssuerRegion": "ALLOWED:KOR"
    }
  }
}
```

### 복합기능 Token 생성
#### Usecase 1. 가맹점의 사용자가 시럽페이 회원 여부를 모르는 상태에서 결제 시도 시
#### Usecase 2. 가맹점에서 가지고 있는 사용자의 SSO 를 기반으로 자동로그인 후 결제를 하고자 하는 경우

복합기능 Token 생성에 관한 방법은 [SyrupPayToken for Java](https://github.com/skplanet/syruppay-java/tree/master/syruppay-token) 의 참고사항을 참조

## License

The gem is available as open source under the terms of the [MIT License](http://opensource.org/licenses/MIT).

