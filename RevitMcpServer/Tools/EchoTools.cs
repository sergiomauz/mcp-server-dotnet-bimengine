using System.ComponentModel;
using ModelContextProtocol.Server;


namespace RevitMcpServer.Tools
{
    [McpServerToolType]
    public static class EchoTools
    {
        [McpServerTool, Description("Echoes the message back to the client.")]
        public static string EchoMom(string message) => $"hello mom! {message}";

        [McpServerTool, Description("Echoes the message back to the client.")]
        public static string EchoDad(string message) => $"hello dad! {message}";
    }
}
