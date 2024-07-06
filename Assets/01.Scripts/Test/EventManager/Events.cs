public class Events
{
    public static ResourceAddEvent ResourceAddEvent = new ResourceAddEvent();
}

public class ResourceAddEvent : GameEvent
{
    public int amount;
    public string message;
}