﻿using ChatRoom.Application.Abstractions.Queries;
#pragma warning disable CS8618

namespace ChatRoom.Mvc.Models.ChatRoomLog;

public class ChatRoomLogRequest
{
#pragma warning disable CS8618
    public IQueryParams Params { get; set; }
#pragma warning restore CS8618

    public string? ChatRoomName { get; set; }
}