﻿using System.Web;
using ChatRoom.Application.Abstractions.Events.Enum;
using ChatRoom.Domain.Events.Enum;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ChatRoom.IntegrationTests.ChatRoom;

[TestCaseOrderer(
    "ChatRoom.IntegrationTests.PriorityOrderer",
    "RunTestsInOrder.XUnit")]
public class ChatRoomTests : IClassFixture<TestingWebAppFactoryFixture<Program>>,
    IClassFixture<ChatRoomDataStoreFixture>
{
    private readonly HttpClient _client;
    private readonly ChatRoomDataStoreFixture _dataFixture;
    private readonly TestingWebAppFactoryFixture<Program> _factoryFixture;

    public ChatRoomTests(TestingWebAppFactoryFixture<Program> factoryFixture,
        ChatRoomDataStoreFixture dataFixture)
    {
        _client = factoryFixture.CreateClient(new WebApplicationFactoryClientOptions()
        {
            BaseAddress = new Uri("https://localhost:44336/")
        });

        _factoryFixture = factoryFixture;
        _dataFixture = dataFixture;
    }

    [Theory, TestPriority(1)]
    [InlineData(GranularityType.All, "<textarea class=\"form-control\" rows=\"20\">11:50 PM:\tMike on Sql leaves\r\n09:48 PM:\tMike on Sql comments Oooo sorry guys I have to go on another meeting, sorryyyyy....\r\n07:46 PM:\tKate on Sql high-fives: Bob on Sql\r\n05:44 PM:\tBob on Sql comments Before we start, Kate last time forgot to tell you how I like your high-five gesture :)\r\n03:42 PM:\tAlice on Sql leaves\r\n01:40 PM:\tAlice on Sql comments Yes, sorry my headphones were muted.\r\n11:38 AM:\tKate on Sql comments Alice do you hear us?\r\n09:36 AM:\tAlice on Sql enters the room\r\n07:34 AM:\tKate on Sql enters the room\r\n08:11 AM:\tMike on Sql comments Hi...\r\n08:11 AM:\tBob on Sql comments Same to you\r\n08:10 AM:\tMike on Sql enters the room\r\n08:10 AM:\tBob on Sql enters the room</textarea>")]
    [InlineData(GranularityType.Hourly, "<textarea class=\"form-control\" rows=\"20\">11:00 PM: \r\n\r\n        Mike on Sql leaves\r\n\r\n09:00 PM: \r\n\r\n        Mike on Sql comments Oooo sorry guys I have to go on another meeting, sorryyyyy....\r\n\r\n07:00 PM: \r\n\r\n        Kate on Sql high-fives: Bob on Sql\r\n\r\n05:00 PM: \r\n\r\n        Bob on Sql comments Before we start, Kate last time forgot to tell you how I like your high-five gesture :)\r\n\r\n03:00 PM: \r\n\r\n        Alice on Sql leaves\r\n\r\n01:00 PM: \r\n\r\n        Alice on Sql comments Yes, sorry my headphones were muted.\r\n\r\n11:00 AM: \r\n\r\n        Kate on Sql comments Alice do you hear us?\r\n\r\n09:00 AM: \r\n\r\n        Alice on Sql enters the room\r\n\r\n07:00 AM: \r\n\r\n        Kate on Sql enters the room\r\n\r\n08:00 AM: \r\n\r\n        Mike on Sql comments Hi...\r\n        Bob on Sql comments Same to you\r\n        Mike on Sql enters the room\r\n        Bob on Sql enters the room\r\n\r\n</textarea>")]
    [InlineData(GranularityType.Minute, "<textarea class=\"form-control\" rows=\"20\">11:50 PM: \r\n\r\n        Mike on Sql leaves\r\n\r\n09:48 PM: \r\n\r\n        Mike on Sql comments Oooo sorry guys I have to go on another meeting, sorryyyyy....\r\n\r\n07:46 PM: \r\n\r\n        Kate on Sql high-fives: Bob on Sql\r\n\r\n05:44 PM: \r\n\r\n        Bob on Sql comments Before we start, Kate last time forgot to tell you how I like your high-five gesture :)\r\n\r\n03:42 PM: \r\n\r\n        Alice on Sql leaves\r\n\r\n01:40 PM: \r\n\r\n        Alice on Sql comments Yes, sorry my headphones were muted.\r\n\r\n11:38 AM: \r\n\r\n        Kate on Sql comments Alice do you hear us?\r\n\r\n09:36 AM: \r\n\r\n        Alice on Sql enters the room\r\n\r\n07:34 AM: \r\n\r\n        Kate on Sql enters the room\r\n\r\n08:11 AM: \r\n\r\n        Mike on Sql comments Hi...\r\n        Bob on Sql comments Same to you\r\n\r\n08:10 AM: \r\n\r\n        Mike on Sql enters the room\r\n        Bob on Sql enters the room\r\n\r\n</textarea>")]
    [InlineData(GranularityType.AggregatedByHour, "<textarea class=\"form-control\" rows=\"20\">11:00 PM: \r\n\t\t1 left\r\n\r\n09:00 PM: \r\n\t\t1 comments\r\n\r\n07:00 PM: \r\n\t\t1 person high-fived 1 other person\r\n\r\n05:00 PM: \r\n\t\t1 comments\r\n\r\n03:00 PM: \r\n\t\t1 left\r\n\r\n01:00 PM: \r\n\t\t1 comments\r\n\r\n11:00 AM: \r\n\t\t1 comments\r\n\r\n09:00 AM: \r\n\t\t1 person entered\r\n\r\n07:00 AM: \r\n\t\t1 person entered\r\n\r\n08:00 AM: \r\n\t\t2 comments\r\n\t\t2 people entered\r\n\r\n</textarea>")]
    [InlineData(GranularityType.AggregatedByMinute, "<textarea class=\"form-control\" rows=\"20\">11:50 PM: \r\n\t\t1 left\r\n\r\n09:48 PM: \r\n\t\t1 comments\r\n\r\n07:46 PM: \r\n\t\t1 person high-fived 1 other person\r\n\r\n05:44 PM: \r\n\t\t1 comments\r\n\r\n03:42 PM: \r\n\t\t1 left\r\n\r\n01:40 PM: \r\n\t\t1 comments\r\n\r\n11:38 AM: \r\n\t\t1 comments\r\n\r\n09:36 AM: \r\n\t\t1 person entered\r\n\r\n07:34 AM: \r\n\t\t1 person entered\r\n\r\n08:11 AM: \r\n\t\t2 comments\r\n\r\n08:10 AM: \r\n\t\t2 people entered\r\n\r\n</textarea>")]
    public async Task ChatRoomLog_CheckLogs_Test(GranularityType type, string expectedResult)
    {
        var testRoom = _dataFixture.Rooms[0];

        var postRequest = new HttpRequestMessage(HttpMethod.Get, "/ChatRoomLog/CheckLogs");
        var formModel = new Dictionary<string, string>
        {
            { "granularityId", $"{(int)type}" },
            { "chatRoomId", $"{testRoom.Id}" }
        };
        postRequest.Content = new FormUrlEncodedContent(formModel);

        var response = await _client.SendAsync(postRequest);

        response.EnsureSuccessStatusCode();

        var responseString = HttpUtility.HtmlDecode(await response.Content.ReadAsStringAsync());

        responseString.Should().Contain(expectedResult);
    }

    [Fact, TestPriority(2)]
    public async Task ChatRoom_WelcomeToTheLoby_Test()
    {
        var response = await _client.GetAsync("ChatRoom/Index");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        responseString.Should().Contain("Welcome to our Chat-room loby");
    }

    [Fact, TestPriority(3)]
    public async Task ChatRoom_GetIntoTheRoom_Test()
    {
        var testRoom = _dataFixture.Rooms[0]; //Sql server
        var testParticipant = _dataFixture.Participants[3]; //Alice on Sql

        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/ChatRoom/GetIntoTheRoom");
        var formModel = new Dictionary<string, string>
        {
            { "ChatRoomId", $"{testRoom.Id}" },
            { "ParticipantId", $"{testParticipant.Id}" }
        };
        postRequest.Content = new FormUrlEncodedContent(formModel);

        var chatEventsNum = _factoryFixture.EventsContext.ChatEvents.Count();

        var response = await _client.SendAsync(postRequest);

        response.EnsureSuccessStatusCode();

        var responseString = HttpUtility.HtmlDecode(await response.Content.ReadAsStringAsync());

        //Assert
        _factoryFixture.EventsContext.ChatEvents.Count().Should().Be(chatEventsNum + 1);

        responseString.Should().Contain($"Welcome '{testParticipant.Name}' in the '{testRoom.Name}' chat-room.");

        var @event = _factoryFixture.EventsContext.ChatEvents.OrderByDescending(e => e.CreatedOn).First();
        @event.Should().NotBeNull();
        @event.Type.Should().Be(EventType.ParticipantEntered);

        //Clean
        _factoryFixture.RemoveEvent(@event);
    }

    [Theory, TestPriority(4)]
    [InlineData(EventType.ParticipantCommented,  "{0} sent.")]
    [InlineData(EventType.ParticipantHighFived, "High-five successfully sent to {0}.")]
    public async Task ChatRoom_SendMessage_Test(EventType type,  string expectedResult)
    {
        var testRoom = _dataFixture.Rooms[0];
        var testParticipant = _dataFixture.Participants[0];
        var testToParticipant = _dataFixture.Participants[1];
        var comment = "Hi from test";

        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/ChatRoom/SendMessage");
        var formModel = new Dictionary<string, string>
        {
            { "roomId", $"{testRoom.Id}" },
            { "participantId", $"{testParticipant.Id}" },
            { "message", comment}
        };
        if (type == EventType.ParticipantHighFived)
        {
            formModel.Add("toParticipantId", $"{testToParticipant.Id}");
        }

        postRequest.Content = new FormUrlEncodedContent(formModel);

        var chatEventsNum = _factoryFixture.EventsContext.ChatEvents.Count();

        var response = await _client.SendAsync(postRequest);

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        //Assert
        _factoryFixture.EventsContext.ChatEvents.Count().Should().Be(chatEventsNum + 1);

        responseString.Should().Contain(string.Format(expectedResult, type == EventType.ParticipantCommented ? comment : $"{testToParticipant.Name}"));

        var @event = _factoryFixture.EventsContext.ChatEvents.OrderByDescending(e => e.CreatedOn).First();
        @event.Should().NotBeNull();
        @event.Type.Should().Be(type);

        //Clean
        _factoryFixture.RemoveEvent(@event);
    }
}