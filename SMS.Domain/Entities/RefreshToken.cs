using SMS.Shared.Guards;

namespace SMS.Domain.Entities
{
    public sealed class RefreshToken
    {
        private int _userId;
        private string _tokenHash = string.Empty;


        public Guid RefreshTokenId { get; set; }

        public int UserId
        {
            get => _userId;
            set
            {
                NumericGuard.AgainstInvalidId(value);
                _userId = value;
            }
        }

        public string TokenHash
        {
            get => _tokenHash;

            set
            {
                StringGuard.AgainstNullOrWhiteSpace(value, nameof(TokenHash));
                _tokenHash = value;
            }
        }

        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public bool IsRevoked { get; set; }


        public RefreshToken() { }

        public RefreshToken(int userId, string tokenHash, DateTime expirationDate,
            DateTime createdAt, DateTime? revokedAt, bool isRevoked)
        {
            UserId = userId;
            TokenHash = tokenHash;
            ExpirationDate = expirationDate;
            RevokedAt = revokedAt;
            CreatedAt = createdAt;
            IsRevoked = isRevoked;
        }

        public RefreshToken(Guid refreshTokenId, int userId,
            string tokenHash, DateTime expirationDate, DateTime createdAt,
            DateTime? revokedAt, bool isRevoked) : this(userId, tokenHash, expirationDate
                , createdAt, revokedAt, isRevoked)
        {
            RefreshTokenId = refreshTokenId;
        }
    }
}
