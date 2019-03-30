using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using Serilog.Sinks.InMemory;

namespace LeadPrototype
{
    public static class ReaderFactory
    {
        private static readonly ILogger Logger;
        private static readonly List<Type> Readers;

        static ReaderFactory()
        {
            Logger = new LoggerConfiguration()
                .WriteTo.InMemory()
                .CreateLogger();
            Readers = GetReaders();
        }
        public static List<Type> GetReaders()
        {
            var interfaceType = typeof(IReader);
            var readers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();
            return readers;
        }
        public static IReader CreateReader(IReaderSettings readerSettings)
        {
            foreach (var reader in Readers)
            {
                if (reader.GetConstructors().Any(constructorInfo => constructorInfo.GetParameters().Any(p => p.ParameterType == readerSettings.GetType())))
                {
                    return (IReader)Activator.CreateInstance(reader, Logger, readerSettings);
                }
            }      
            return null;
        }
    }
}