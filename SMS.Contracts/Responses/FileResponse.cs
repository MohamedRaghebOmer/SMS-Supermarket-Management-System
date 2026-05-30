namespace SMS.Contracts.Responses
{
    public sealed record FileResponse
    {
        public byte[] Bytes { get; init; } = [];
        public string FileExtension { get; init; } = string.Empty;
        public string ContentType { get; init; } = string.Empty;
    }
}
