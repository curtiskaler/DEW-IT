namespace B7.Processing
{
    public static class ResultExtensions
    {
        public static bool IsFailure(this IResult result)
        {
            if (result == null) { throw new ArgumentNullException(nameof(result)); }
            return result.Code == ResultCode.FAILURE;
        }

        public static bool IsSuccess(this IResult result)
        {
            if (result == null) { throw new ArgumentNullException(nameof(result)); }
            return result.Code == ResultCode.SUCCESS;
        }

        public static bool IsSkipped(this IResult result)
        {
            if (result == null) { throw new ArgumentNullException(nameof(result)); }
            return result.Code == ResultCode.SKIP;
        }

        public static List<Failure> GetFailures(this IEnumerable<IResult> results)
        {
            if (results == null) { throw new ArgumentNullException(nameof(results)); }

            return results
                .Where(it => it.Code == ResultCode.FAILURE)
                .Select(it => (it as Failure)!)
                .ToList();
        }
    }
}

