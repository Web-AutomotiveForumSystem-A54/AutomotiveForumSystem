﻿namespace AutomotiveForumSystem.Exceptions
{
    public class DuplicateEntityException : ApplicationException
    {
        public DuplicateEntityException(string? message) : base(message) { }
    }
}
