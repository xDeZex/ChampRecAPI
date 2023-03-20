namespace testapi.Exceptions;

public class HMException: Exception{
    public int HTTPCode {get; set;}
    public HMException()
    {
    }

    public HMException(string message)
        : base(message)
    {
    }

    public HMException(string message, Exception inner)
        : base(message, inner)
    {
    }

    
}