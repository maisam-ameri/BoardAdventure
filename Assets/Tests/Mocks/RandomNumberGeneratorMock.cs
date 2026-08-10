using System;
using Core.Abstractions;

namespace Tests.Mocks
{
    public class RandomNumberGeneratorMock : IRandomNumberGenerator
    {
        public byte GenerateNumber()
        {
            var rnd = new Random();
            return (byte) rnd.Next(1, 7);
        }
    }
}