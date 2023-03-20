﻿using AutoMapper;
using ChatRoom.Application.Abstractions.Infrastructure.Repositories;
using ChatRoom.Application.Abstractions.Services.ChatRoom;
using ChatRoom.Application.Abstractions.Services.ChatRoomLog;
using ChatRoom.Application.Services.ChatRoom;
using ChatRoom.Domain.Events;
using ChatRoom.Domain.Model;
using FluentAssertions;
using Xunit;
using Moq;
using ChatRoom.Application.Services.ChatRoomLog.Queries;
using Microsoft.Extensions.Logging;

namespace ChatRoom.UnitTests.Application.ChatRoom;

public class ChatRoomTests : IClassFixture<ChatRoomDataStoreFixture>
{
    private readonly Mock<IChatRoomRepository<Domain.Model.ChatRoom>> _mockChatRoomRepository;
    private readonly Mock<IChatRoomRepositoryFactory> _mockChatRoomRepositoryFactory;
    private readonly Mock<IRepository<Participant>> _mockParticipantRepository;
    private readonly Mock<IChatRoomLogService> _mockChatLogService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly IChatRoomService _chatRoomService;
    private readonly ChatRoomDataStoreFixture _fixture;

    public ChatRoomTests(ChatRoomDataStoreFixture fixture)
    {
        _fixture = fixture;
        _mockChatRoomRepository = new Mock<IChatRoomRepository<Domain.Model.ChatRoom>>();
        _mockParticipantRepository = new Mock<IRepository<Participant>>();
        _mockChatRoomRepositoryFactory  = new Mock<IChatRoomRepositoryFactory>();
        _mockChatLogService = new Mock<IChatRoomLogService>();
        _mockMapper = new Mock<IMapper>();

        //mocks
        _mockChatRoomRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(fixture.Rooms);

        _mockParticipantRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(fixture.Participants);

        _mockChatRoomRepositoryFactory.Setup(x => x.Repository<IChatRoomRepository<Domain.Model.ChatRoom>>())
            .Returns(_mockChatRoomRepository.Object);

        _mockChatRoomRepositoryFactory.Setup(x => x.Repository<IRepository<Participant>>())
            .Returns(_mockParticipantRepository.Object);

        _mockChatLogService.Setup(x => x.LogEventAsync(It.IsAny<ChatEvent>(), It.IsAny<CancellationToken>()));

        _chatRoomService = new ChatRoomService(It.IsAny<ILogger<ChatRoomService>>(), _mockChatRoomRepositoryFactory.Object,
            _mockChatLogService.Object, _mockMapper.Object);
    }

    [Theory(Skip = "some issues with EF attach previously detached entities")]
    [InlineData(true)]
    [InlineData(false)]
    public async Task AddRemoveParticipantToTheRoomAsyncTest(bool add)
    {
        var testRoom = _fixture.Rooms[0];
        var testParticipant = _fixture.Participants[0];

        if (add)
        {
            testParticipant.ChatRoomId.Should().BeNull();
        }
        else
        {
            testParticipant.AddToTheRoom(testRoom.Id);
        }

        _mockParticipantRepository.Setup(x => x.GetAsync(testParticipant.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(testParticipant);

        _mockChatRoomRepository.Setup(x => x.AddOrUpdateAsync(testRoom, It.IsAny<CancellationToken>()));

        if (add)
        {
            await _chatRoomService.AddParticipantAsync(testParticipant.Id, testParticipant.Id, It.IsAny<CancellationToken>());

            testParticipant.ChatRoomId.Should().Be(testRoom.Id);
        }
        else
        {
            await _chatRoomService.RemoveParticipantAsync(testParticipant.Id, testParticipant.Id, It.IsAny<CancellationToken>());

            testParticipant.ChatRoomId.Should().BeNull();
        }
    }
}