﻿using ServerManagement.Models;

namespace ServerManagement.StateStore;

public class ContainerStorage
{
    private Server _server = new();

    public Server GetServer()
    {
        return _server;
    }

    public void SetServer(Server server)
    {
        _server = server;
    }
}