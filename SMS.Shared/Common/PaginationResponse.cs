namespace SMS.Shared.Common
{
    public class PaginationResponse<T>
    {
        public static readonly string TotalCountParameterName = "TotalCount";


        public IReadOnlyList<T> Items { get; set; }
        /// <summary>
        /// Total count of items across all pages in the datasource (not just the current page).
        /// </summary>
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
