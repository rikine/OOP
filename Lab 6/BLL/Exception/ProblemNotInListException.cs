[System.Serializable]
public class ProblemNotInListException : System.Exception
{
    public ProblemNotInListException() { }
    public ProblemNotInListException(string message) : base(message) { }
    public ProblemNotInListException(string message, System.Exception inner) : base(message, inner) { }
    protected ProblemNotInListException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}