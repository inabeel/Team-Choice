namespace TeamChoice.WebApis.Domain.Models.DTOs;
public class MMTLookupReq
{
    public string AccountNo { get; set; }
    public string AccountAgentCode { get; set; }
    public string Location { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string AgentCode { get; set; }
}