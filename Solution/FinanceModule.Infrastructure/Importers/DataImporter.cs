using FinanceModule.Domain.Models;
using System.Collections.Generic;

namespace FinanceModule.Infrastructure.Importers
{
    public abstract class DataImporter
    {
        public void Import(string filePath)
        {
            var data = ReadFile(filePath);
            ParseData(data);
        }

        protected abstract string ReadFile(string filePath);
        protected abstract void ParseData(string data);
    }
}