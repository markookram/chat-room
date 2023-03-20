﻿using ChatRoom.Domain.Events.Enum;

namespace ChatRoom.Domain.Events;

public class ParticipantHighFived : ChatEvent
{
    public ParticipantHighFived()
    {
    }

    public ParticipantHighFived(int participantId, string participantName, int chatRoomId)
    :base(EventType.PariticipantHighFived, participantId, participantName, chatRoomId)
    {
        CreatedOn = DateTime.Now;
    }

    public override string ToString()
    {
        return $"{ParticipantName} high-fives: {ToParticipantName}.";
    }

    public override string Describe(params string[] prms)
    {
        if (!prms.Any() || !int.TryParse(prms[0], out int count)) return "high-fived other people.";
        return $"{count} high-fived other people.";

    }
}