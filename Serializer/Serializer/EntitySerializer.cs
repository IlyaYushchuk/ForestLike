using Serializer.Entities;

using System.Text.Json;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace Serializer;

public static class EntitySerializer
{
    public static string SerializeUser(User user)
    {
        return JsonSerializer.Serialize<User>(user);
    }
    public static User DeserializeUser(string UserString)
    {
        return JsonSerializer.Deserialize<User>(UserString);
    }

    public static string SerializeRecords(IEnumerable<Record> records)
    {
        return JsonSerializer.Serialize<IEnumerable<Record>>(records);
    }
    public static IEnumerable<Record> DeserializeRecords(string RecordsString)
    {
        return JsonSerializer.Deserialize<IEnumerable<Record>>(RecordsString);
    }
    public static string SerializeRecord(Record record)
    {
        return JsonSerializer.Serialize<Record>(record);
    }
    public static Record DeserializeRecord(string RecordString)
    {
        return JsonSerializer.Deserialize<Record>(RecordString);
    }
   
    public static string SerializeCooperativeRequest(CooperativeTimerRequest request)
    {
        return JsonSerializer.Serialize<CooperativeTimerRequest>(request);
    }
    public static CooperativeTimerRequest DeserializeCooperativeRequest(string CooperativeRequestString)
    {
        return JsonSerializer.Deserialize<CooperativeTimerRequest>(CooperativeRequestString);
    }
    public static string SerializeNameList(IEnumerable<string> names)
    {
        return JsonSerializer.Serialize<IEnumerable<string>>(names);
    }
    public static IEnumerable<string> DeserializeNameList(string names)
    {
        return JsonSerializer.Deserialize<IEnumerable<string>>(names);
    }
    public static string SerializeToXmlString(object obj)
    {
        // Создаем XmlSerializer для типа объекта
        XmlSerializer serializer = new XmlSerializer(obj.GetType());
        // Создаем StringWriter для записи сериализованных данных в строку
        StringWriter stringWriter = new StringWriter();

        XmlWriterSettings settings = new XmlWriterSettings();
        settings.OmitXmlDeclaration = true;

        using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
        {
            serializer.Serialize(xmlWriter, obj);
        }

        // Возвращаем XML-строку
        return stringWriter.ToString();
    }
    public static T DeserializeFromXmlString<T>(string xmlString)
    {
        // Создаем XmlSerializer для типа объекта T
        XmlSerializer serializer = new XmlSerializer(typeof(T));

        // Создаем StringReader для чтения XML строки
        using (StringReader stringReader = new StringReader(xmlString))
        {
            // Десериализуем XML строку в объект
            return (T)serializer.Deserialize(stringReader);
        }
    }
}
