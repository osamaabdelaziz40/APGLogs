using APGLogs.DomainHelper.Attributes;
using APGLogs.DomainHelper.Interfaces;
using APGLogs.DomainHelper.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace APGFundamentals.DomainHelper.Services
{
    public class CSVManager : ICSVManager
    {
        public async Task<ExportedCSVFile> Export<T>(List<T> data, string reportName)
        {
            Type myType = GetGenericType(data.FirstOrDefault());
            using (var stream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(stream, new UTF8Encoding(true)))
                {
                    var columns = (await GetColumns<T>()).OrderBy(o => o.ExportAttribute.Order);
                    //var columns = (await GetColumns(myType)).OrderBy(o => o.ExportAttribute.Order);
                    var columnNames = columns.Select(c => c.ExportAttribute.ExportName != null
                        ? c.ExportAttribute.ExportName
                        : c.PropertyInfo.Name);
                    streamWriter.WriteLine(string.Join(',', columnNames));

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            var values = await GetObjectValues<T>(item, columns);
                            streamWriter.WriteLine(string.Join(',', values));
                        }
                    }

                    streamWriter.Flush();
                    stream.Seek(0, SeekOrigin.Begin);

                    return new ExportedCSVFile()
                    {
                        ContentType = "text/csv",
                        FileName = $"{reportName}.csv",
                        FileBase64 = Convert.ToBase64String(stream.ToArray())
                    };
                }
            }
        }

        #region Private Methods
        private async Task<IEnumerable<ExportProperty>> GetColumns<T>()
        {
            return typeof(T).GetProperties().Select(
                property =>
                {
                    var exportAttribute = ((ExportAttribute)property.GetCustomAttributes(typeof(ExportAttribute), false).FirstOrDefault());
                    return exportAttribute == null
                        ? null
                        : new ExportProperty { PropertyInfo = property, ExportAttribute = exportAttribute };
                }).Where(p => p != null);

        }
        private async Task<List<string>> GetObjectValues<T>(T item, IEnumerable<ExportProperty> columns)
        {
            var propertyValues = new List<string>();
            foreach (var column in columns)
            {
                propertyValues.Add(await GetAttributeValue(item, column.PropertyInfo, column.ExportAttribute));
            }

            return propertyValues;
        }

        private async Task<string> GetAttributeValue<T>(T item, PropertyInfo propertyInfo, ExportAttribute attribute)
        {
            object value = propertyInfo.GetValue(item);

            if (value == null || attribute == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(attribute.Format) && value is IFormattable)
            {
                return (value as IFormattable).ToString(attribute.Format, CultureInfo.CurrentCulture);
            }

            if (!string.IsNullOrWhiteSpace(attribute.Format))
            {
                return string.Format(attribute.Format, value);
            }

            return propertyInfo.GetValue(item).ToString();
        }

        private Type GetGenericType(object obj)
        {
            if (obj != null)
            {
                Type t = obj.GetType();
                if (t.IsGenericType)
                {
                    Type[] at = t.GetGenericArguments();
                    t = at.First<Type>();
                }
                return t;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
