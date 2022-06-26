namespace DewIt.Model.DataTypes
{
    /// <summary> The priority of an issue, so that agents can react accordingly, </summary>
    public abstract class Priority : Enumeration<Priority>
    {
        public string Color { get; init; }

        /// <summary> The 'unset' priority. Color: gray. </summary>
        public static readonly Priority NOT_SPECIFIED = new Instance(0, nameof(NOT_SPECIFIED), "#C2C2C2" /* gray */);

        /// <summary> Minor issue or easily worked around. Color: blue </summary>
        public static readonly Priority MINOR = new Instance(1, nameof(MINOR), "#0090FF" /* blueish */);

        /// <summary> Has the potential to affect progress. Color: yellow-green. </summary>
        public static readonly Priority NORMAL = new Instance(2, nameof(NORMAL), "#00FF90" /* yellow-green */);

        /// <summary> Has the potential to block progress. Color: orange. </summary>
        public static readonly Priority HIGH = new Instance(3, nameof(HIGH), "#FF9000" /* orange */);

        /// <summary> This will block progress. Color: a dark red. </summary>
        public static readonly Priority CRITICAL = new Instance(4, nameof(CRITICAL), "#FF0000" /* red */);

        protected Priority(int ordinal, string name, string color) : base(ordinal, name)
        {
            Color = color;
        }

        private class Instance : Priority
        {
            internal Instance(int ordinal, string name, string color) : base(ordinal, name, color)
            {
            }
        }
    }
}