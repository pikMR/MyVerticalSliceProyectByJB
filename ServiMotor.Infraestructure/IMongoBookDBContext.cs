using System;
using MongoDB.Driver;

namespace ServiMotor.Infraestructure
{
    public interface IMongoBookDBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
