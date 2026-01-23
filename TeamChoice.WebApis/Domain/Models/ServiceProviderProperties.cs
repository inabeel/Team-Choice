namespace TeamChoice.WebApis.Domain.Models;


public class ServiceProviderProperties
{
    public TPlus TPlus { get; set; }
    public Agent Agent { get; set; }
    public SomBank SomBank { get; set; }
}

public class TPlus
{
    public string Link { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class Agent
{
    public string Link { get; set; }
    public string AgentCode { get; set; }
    public string Location { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class SomBank
{
    public string Link { get; set; }
    public string ApiKey { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
