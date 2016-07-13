# SyrupPay Token

시럽페이 서비스에서 가맹점 인증 및 데이터 교환을 위한 규격을 정의하며 전송 구간에 대한 암호화 및 무결성 보장을 위한 토큰을 생성, 관리하는 기능을 수행합니다.

## Supported Framework
.NET Framework 2.0, 3.5, 4.0, 4.5

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
                    .WithExtraMerchantUserId("핸드폰과 같이 회원 별 추가 ID 체계가 존재할 경우 입력")	//Optional
                    .WithSsoCredential("SSO 를 발급 받았을 경우 입력")	//Optional
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
						)	//Optional
					.WithLanguageForDisplay(PayConfigurer<SyrupPayTokenBuilder>.Language.KO)
					.WithAmount(50000)
					.WithCurrency(PayConfigurer<SyrupPayTokenBuilder>.Currency.KRW)
					.WithShippingAddress(new PayConfigurer<SyrupPayTokenBuilder>.ShippingAddress("137-332", "서초구 잠원동 하나아파트", "1동 1호", "서울", "", "kr"))	//Optional
					.WithDeliveryPhoneNumber("01011112222")		//Optional
					.WithDeliveryName("배송 수신자")			//Optional
					.WithInstallmentPerCardInformation(new PayConfigurer<SyrupPayTokenBuilder>.CardInstallmentInformation("카드구분 코드", "할부정보. ex. NN1;NN2;YY3;YY4;YY5;NH6"))	//Optional
					.WithBeAbleToExchangeToCash(false)			//Optional
					.WithPayableRuleWithCard(PayConfigurer<SyrupPayTokenBuilder>.PayableLocaleRule.ONLY_ALLOWED_KOR)	//Optional
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

### 참고 사항
#### 이용하고자 하는 시럽페이 서비스 기능이 복합적인 경우 중첩하여 사용 가능하다.
##### 상황 1. 시럽페이 가입 여부를 모르는 상태에서 결제 하고자 하는 경우 (회원가입, 로그인, 결제 가능 토큰)
```C#
var token = new SyrupPayTokenBuilder().Of("가맹점 ID")
			.SignUp()
				.WithMerchantUserId("syrup_order_user_id")
				.WithExtraMerchantUserId("핸드폰과 같이 회원 별 추가 ID 체계가 존재할 경우 입력") // Optional
			.And()
			.Pay()
				.WithOrderIdOfMerchant("가맹점에서 관리하는 주문 ID") // 가맹점 Transaction Id = mctTransAuthId
				.WithProductTitle("제품명")
				.WithProductUrls(
					"http://deal.11st.co.kr/product/SellerProductDetail.tmall?method=getSellerProductDetail&prdNo=1122841340",
					"http://deal.11st.co.kr/product/SellerProductDetail.tmall?method=getSellerProductDetail&prdNo=1265508741"
					) // Optional
				.WithLanguageForDisplay(PayConfigurer<SyrupPayTokenBuilder>.Language.KO)
				.WithAmount(50000)
				.WithCurrency(PayConfigurer<SyrupPayTokenBuilder>.Currency.KRW)
				.WithShippingAddress(new PayConfigurer<SyrupPayTokenBuilder>.ShippingAddress("137-332", "서초구 잠원동 하나아파트", "1동 1호", "서울", "", "kr")) // Optional
				.WithDeliveryPhoneNumber("01011112222") // Optional
				.WithDeliveryName("배송 수신자") // Optional
				.WithInstallmentPerCardInformation(new PayConfigurer<SyrupPayTokenBuilder>.CardInstallmentInformation("카드구분 코드", "할부정보. ex. NN1;NN2;YY3;YY4;YY5;NH6")) // Optional
				.WithBeAbleToExchangeToCash(false)
				.WithPayableRuleWithCard(PayConfigurer<SyrupPayTokenBuilder>.PayableLocaleRule.ONLY_NOT_ALLOED_USA)
			.And()
			.GenerateTokenBy(key);
```
##### 상황 2. 시럽페이 가입 여부를 모르는 상태에서 결제 하고자 하는 경우 (회원가입, 로그인, 결제 가능 토큰)
```C#
var token = new SyrupPayTokenBuilder().Of("가맹점 ID")
                .Login()
                    .WithMerchantUserId("가맹점의 회원 ID 또는 식별자")
                    .WithExtraMerchantUserId("핸드폰과 같이 회원 별 추가 ID 체계가 존재할 경우 입력") // Optional
                    .WithSsoCredential("발급 받은 SSO")	//Optainal
                .And()
                .Pay()
                    .WithOrderIdOfMerchant("가맹점에서 관리하는 주문 ID") // 가맹점 Transaction Id = mctTransAuthId
                    .WithProductTitle("제품명")
                    .WithProductUrls(
                        "http://deal.11st.co.kr/product/SellerProductDetail.tmall?method=getSellerProductDetail&prdNo=1122841340",
                        "http://deal.11st.co.kr/product/SellerProductDetail.tmall?method=getSellerProductDetail&prdNo=1265508741"
                        ) // Optional
                    .WithLanguageForDisplay(PayConfigurer<SyrupPayTokenBuilder>.Language.KO)
                    .WithAmount(50000)
                    .WithCurrency(PayConfigurer<SyrupPayTokenBuilder>.Currency.KRW)
                    .WithShippingAddress(new PayConfigurer<SyrupPayTokenBuilder>.ShippingAddress("137-332", "서초구 잠원동 하나아파트", "1동 1호", "서울", "", "kr")) // Optional
                    .WithDeliveryPhoneNumber("01011112222") // Optional
                    .WithDeliveryName("배송 수신자") // Optional
                    .WithInstallmentPerCardInformation(new PayConfigurer<SyrupPayTokenBuilder>.CardInstallmentInformation("카드구분 코드", "할부정보. ex. NN1;NN2;YY3;YY4;YY5;NH6")) // Optional
                    .WithBeAbleToExchangeToCash(false) // Optional
                    .WithPayableRuleWithCard(PayConfigurer<SyrupPayTokenBuilder>.PayableLocaleRule.ONLY_ALLOWED_KOR) // Optional
					.WithMerchantDefinedValue("{\"id_1\": \"value\",\"id_2\": 2\"}")
                .And()
                .GenerateTokenBy(key);
```

## 참고 사항
### 시럽페이 사용자 연동을 위한 Syrup Pay Token 세팅
Syrup Pay 사용자에 대한 정보를 조회하여 Syrup Pay 수동 로그인 시 ID 자동 입력과 같은 추가적인 기능을 수행할 수 있도록 매칭이 되는 정보를 설정하고 토큰을 생성합니다.
```C#
var token = new SyrupPayTokenBuilder().Of("가맹점 ID")
    .Login()
        .WithMerchantUserId("가맹점의 회원 ID 또는 식별자")
        .WithExtraMerchantUserId("핸드폰과 같이 회원 별 추가 ID 체계가 존재할 경우 입력")
        .WithSsoCredential("SSO 를 발급 받았을 경우 입력")
    .And()
    .MapToSyrupPayUser()
        .WithType(MapToSyrupPayUserConfigurer<SyrupPayTokenBuilder>.MappingType.CI_MAPPED_KEY)
        .WithValue("4987234")
    .And()
    .GenerateTokenBy(key);
```

### token의 결과
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJodHRwczovL3BheS5zeXJ1cC5jby5rciIsInR5cCI6Impvc2UiLCJpc3MiOiLqsIDrp7nsoJAiLCJleHAiOjE0NDExNjg2NjYsImlhdCI6MTQ0MTE2ODA2NiwianRpIjoiOGQyNGU3NTMtNmZjNS00YmMwLWI4MzktMmVlNTBhYjQ3MGEwIiwibmJmIjowLCJsb2dpbkluZm8iOnsibWN0VXNlcklkIjoi6rCA66e57KCQ7J2YIO2ajOybkCBJRCDrmJDripQg7Iud67OE7J6QIiwiZXh0cmFVc2VySWQiOiLtlbjrk5ztj7Dqs7wg6rCZ7J20IO2ajOybkCDrs4Qg7LaU6rCAIElEIOyytOqzhOqwgCDsobTsnqztlaAg6rK97JqwIOyeheugpSIsInNzb0NyZWRlbnRpYWwiOiJTU08g66W8IOuwnOq4iSDrsJvslZjsnYQg6rK97JqwIOyeheugpSJ9LCJ1c2VySW5mb01hcHBlciI6eyJtYXBwaW5nVHlwZSI6IkNJX01BUFBFRF9LRVkiLCJtYXBwaW5nVmFsdWUiOiI0OTg3MjM0In19.edroOd5__uGm_GU8u9YPwY7Dxkv9Qr7JOtXJuU5KBwY
```

### token의 내용
```json
{
  "aud": "https://pay.syrup.co.kr",
  "typ": "jose",
  "iss": "가맹점",
  "exp": 1441168666,
  "iat": 1441168066,
  "jti": "8d24e753-6fc5-4bc0-b839-2ee50ab470a0",
  "loginInfo": {
    "mctUserId": "가맹점의 회원 ID 또는 식별자",
    "extraUserId": "핸드폰과 같이 회원 별 추가 ID 체계가 존재할 경우 입력",
    "ssoCredential": "SSO 를 발급 받았을 경우 입력"
  },
  "userInfoMapper": {
    "mappingType": "CI_MAPPED_KEY",
    "mappingValue": "4987234"
  }
}
```

## License

The gem is available as open source under the terms of the [MIT License](http://opensource.org/licenses/MIT).

