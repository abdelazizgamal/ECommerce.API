namespace ECommerce.APIs

{
    public record TokenDto
    (
        string AccessToken,
        DateTime Expiration,
        string TokenType = "Bearer"
    );
}
