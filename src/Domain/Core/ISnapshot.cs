﻿namespace Domain.Core;

public interface ISnapshot
{
    byte[] CreateSnapshot();
}