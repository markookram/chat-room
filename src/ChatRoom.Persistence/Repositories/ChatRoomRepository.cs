﻿using ChatRoom.Application.Abstractions.Infrastructure.Persistence;
using ChatRoom.Application.Abstractions.Infrastructure.Repositories;

namespace ChatRoom.Persistence.Repositories;

/// <summary>
/// Implements chat-room repo.
/// </summary>
public class ChatRoomRepository : IRepository<Domain.Model.ChatRoom>
{
    private readonly IDataStore _dataStore;

    public ChatRoomRepository(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }


    async Task<IList<Domain.Model.ChatRoom>> IRepository<Domain.Model.ChatRoom>.GetAllAsync(CancellationToken cancellationToken)
    {
        return (await _dataStore.GetDataAsync<Domain.Model.ChatRoom>(cancellationToken)).ToList();
    }

    async Task<Domain.Model.ChatRoom?> IRepository<Domain.Model.ChatRoom>.GetAsync(int id, CancellationToken cancellationToken)
    {
        return (await _dataStore.GetDataAsync<Domain.Model.ChatRoom>(id, cancellationToken));
    }

}