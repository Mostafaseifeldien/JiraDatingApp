using System.Xml;
using System.Xml.Linq;
using EventProcessor.Core.Entities;
using EventProcessor.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace EventProcessor.Infrastructure.FileParsers
{
    public class EventFileParser : IFileParser
    {
        private readonly ILogger<EventFileParser> _logger;

        public EventFileParser(ILogger<EventFileParser> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<object>> ParseEventFileAsync(Stream fileStream)
        {
            var events = new List<object>();

            try
            {
                using var reader = new StreamReader(fileStream);
                var xmlDoc = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None);
                var nsManager = CreateNamespaceManager(xmlDoc);

                foreach (var element in xmlDoc.Descendants())
                {
                    try
                    {
                        switch (element.Name.LocalName)
                        {
                            case "accountingOperation":
                                events.Add(ParseAccountingOperation(element));
                                break;
                            case "personalization":
                                events.Add(ParsePersonalization(element));
                                break;
                            case "unitReconstruction":
                                events.Add(ParseUnitReconstruction(element));
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error parsing element: {ElementName}, Xml: {Xml}", element.Name.LocalName, element.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing entire event file");
                throw;
            }

            return events;
        }

        private AccountingOperation ParseAccountingOperation(XElement element)
        {
            var aoNs = "http://www.cairo.com/cbo/data/accountingoperation";
            var headElement = element.Element(XName.Get("head", aoNs));
            return new AccountingOperation
            {
                Header = ParseEventHeader(headElement, aoNs),
                OperationType = SafeInt(element, "operationType", aoNs),
                OperationNumber = SafeInt(element, "operationNumber", aoNs),
                CardId = SafeLong(element, "cardId", aoNs),
                OperationBalance = SafeInt(element, "operationBalance", aoNs),
                CardBalance = SafeInt(element, "cardBalance", aoNs),
                TotalAmount = SafeInt(element, "totalAmount", aoNs),
                Payment = SafeInt(element, "payment", aoNs),
                ServiceNumber = SafeInt(element, "serviceNumber", aoNs),
                UserCode = SafeInt(element, "userCode", aoNs)
            };
        }

        private Personalization ParsePersonalization(XElement element)
        {
            var perNs = "http://www.cairo.com/cbo/data/personalization";
            var headElement = element.Element(XName.Get("head", perNs));
            return new Personalization
            {
                Header = ParseEventHeader(headElement, perNs),
                PersonalizationType = SafeString(element, "type", perNs),
                Data = SafeString(element, "data", perNs)
            };
        }

        private UnitReconstruction ParseUnitReconstruction(XElement element)
        {
            var unitNs = "http://www.cairo.com/cbo/data/unitreconstruction";
            var headElement = element.Element(XName.Get("head", unitNs));
            return new UnitReconstruction
            {
                Header = ParseEventHeader(headElement, unitNs),
                UnitType = SafeString(element, "type", unitNs),
                Data = SafeString(element, "data", unitNs)
            };
        }

        private EventHeader ParseEventHeader(XElement headerElement, string ns)
        {
            if (headerElement == null)
            {
                _logger.LogWarning("Missing <head> element");
                return new EventHeader
                {
                    RecordId = 0,
                    Version = 0,
                    Date = DateTime.MinValue
                };
            }

            return new EventHeader
            {
                RecordId = SafeInt(headerElement, "recordId", ns),
                Version = SafeInt(headerElement, "version", ns),
                Date = SafeDateTime(headerElement, "date", ns),
                UniversalSeq = SafeNullableInt(headerElement, "universalSeq", ns),
                DailySeq = SafeNullableInt(headerElement, "dailySeq", ns),
                ServiceProvider = SafeNullableInt(headerElement, "serviceProvider", ns),
                EquipmentType = SafeInt(headerElement, "equipmentType", ns),
                EquipmentModel = SafeInt(headerElement, "equipmentModel", ns),
                SerialNumber = SafeInt(headerElement, "serialNumber", ns),
                Line = SafeInt(headerElement, "line", ns),
                Station = SafeInt(headerElement, "station", ns),
                Hall = SafeInt(headerElement, "hall", ns),
                Position = SafeInt(headerElement, "position", ns)
            };
        }

        private XmlNamespaceManager CreateNamespaceManager(XDocument xmlDoc)
        {
            var nsManager = new XmlNamespaceManager(new NameTable());
            foreach (var attr in xmlDoc.Root.Attributes().Where(a => a.IsNamespaceDeclaration))
            {
                nsManager.AddNamespace(attr.Name.LocalName, attr.Value);
            }
            return nsManager;
        }

        // Helper functions
        private int SafeInt(XElement element, string name, string ns) =>
            (int?)element.Element(XName.Get(name, ns)) ?? 0;

        private int? SafeNullableInt(XElement element, string name, string ns) =>
            (int?)element.Element(XName.Get(name, ns));

        private long SafeLong(XElement element, string name, string ns) =>
            (long?)element.Element(XName.Get(name, ns)) ?? 0;

        private DateTime SafeDateTime(XElement element, string name, string ns) =>
            (DateTime?)element.Element(XName.Get(name, ns)) ?? DateTime.MinValue;

        private string SafeString(XElement element, string name, string ns) =>
            (string)element.Element(XName.Get(name, ns)) ?? string.Empty;
    }
}
