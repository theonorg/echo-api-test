using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using echo_api.Models;
using Xunit;

namespace echo_api.tests;

public class EchoAPITests
{
    private readonly EchoApiApplication _app;
    public EchoAPITests() {
        _app = new EchoApiApplication();
    }

    [InlineData("teste")]
    [InlineData("TESTE")]
    [InlineData("aptiv")]
    [Theory]
    public async Task EchoMessage(string message)
    {
        var client = _app.CreateClient();
        var response = await client.GetAsync($"/echo/{message}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = response.Content.ReadFromJsonAsync<string>();
        Assert.Equal(message, responseString.Result);
    }

    [Fact]
    public async Task GetEchoHistory()
    {
        var messages = new List<string> { 
            "message1", 
            "message2", 
            "message3" 
        };

        var expectedLog = new List<string> { 
            "Echoing message: message1", 
            "Echoing message: message2", 
            "Echoing message: message3" 
        };
        var client = _app.CreateClient();
        var response = await client.DeleteAsync($"/log");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        response = await client.GetAsync($"/echo/{messages[0]}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await client.GetAsync($"/echo/{messages[1]}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await client.GetAsync($"/echo/{messages[2]}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await client.GetAsync($"/log");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var logList = response.Content.ReadFromJsonAsync<List<EchoHistory>>().Result;
        Assert.Equal(3, logList!.Count);

        Assert.Equal(expectedLog[0], logList[0].Message);
        Assert.Equal(expectedLog[1], logList[1].Message);
        Assert.Equal(expectedLog[2], logList[2].Message);
    }

    [Fact]
    public async Task ClearEchoHistory()
    {
        var messages = new List<string> { 
            "message1", 
            "message2", 
            "message3" 
        };

        var client = _app.CreateClient();
        var response = await client.DeleteAsync($"/log");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        response = await client.GetAsync($"/echo/{messages[0]}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await client.GetAsync($"/echo/{messages[1]}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await client.GetAsync($"/echo/{messages[2]}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await client.DeleteAsync($"/log");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var deletedRecords = response.Content.ReadFromJsonAsync<int>().Result;
        Assert.Equal(3, deletedRecords);
    }

}