namespace SyrupPayToken.jwt
{
    public interface IJwtToken
    {
        string Iss { get; }
        string GetIss();

        string Sub { get; }
        string GetSub();

        string Aud { get; }
        string GetAud();

        long Exp { get; }
        long GetExp();

        long Nbf { get; }
        long GetNbf();

        long Iat { get; }
        long GetIat();

        string Jti { get; }
        string GetJti();
    }
}
