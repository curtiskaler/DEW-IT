namespace B7.Processing
{
    public static class StepAndResultExtensions
    {
        /// <summary> Adds an object to the end of the <see cref="List{StepAndResult}"/>. </summary>
        public static void Add(this List<StepAndResult> list, IProcessStep step, IResult result)
        {
            if (list == null) { throw new ArgumentNullException(nameof(list)); }
            if (step == null) { throw new ArgumentNullException(nameof(step)); }
            if (result == null) { throw new ArgumentNullException(nameof(result)); }
            list.Add(new StepAndResult(step, result));
        }
    }
}

