namespace s30844_net01;

public class PersonalComputer : Device
{
    public string OS { get; set; }


    public override void TurnOn()
    {
        if (OS == null)
        {
            throw new EmptySystemException("No OS provided");
        }else
        {
            IsTurnedOn = true;
        }
    }
}

public class EmptySystemException : Exception
{
    public EmptySystemException(string noOsProvided)
    {
        throw new NotImplementedException();
    }
}