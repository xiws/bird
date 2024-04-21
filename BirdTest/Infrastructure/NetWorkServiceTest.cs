﻿using Infrastructure.Network;

namespace BirdTest.Infrastructure;

public class NetWorkServiceTest
{
    private NetWorkService _service = null;
    
    [SetUp]
    public void Setup()
    {
        _service = new NetWorkService();
    }
    
    [Test]
    public void Ping()
    {
        Assert.Greater(_service.TryPing("www.baidu.com"), 0);
    }
}