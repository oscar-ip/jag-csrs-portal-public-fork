namespace Csrs.Interfaces.Dynamics
{
    [Serializable]
    public class InvalidIdException : Exception
    {
        public string? Value { get; init; }
        
        public InvalidIdException(string message, string value) : base(message) 
        {
            Value = value;
        }        
    }
}
