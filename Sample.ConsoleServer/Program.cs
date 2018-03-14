﻿using System;
using AutoMapper;
using Sample.UserManagement.Contract;
using Sample.UserManagement.Domain;

namespace Sample.ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server has started.");
            Mapper.Initialize(cfg => cfg.CreateMap<User, UserDto>());
            new SampleServerProgram().Run();
        }
    }
}