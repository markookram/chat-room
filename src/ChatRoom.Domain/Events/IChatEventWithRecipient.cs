﻿namespace ChatRoom.Domain.Events;

public interface IChatEventWithRecipient
{
    public int? ToParticipantId { get; }

    public string? ToParticipantName { get;}
}