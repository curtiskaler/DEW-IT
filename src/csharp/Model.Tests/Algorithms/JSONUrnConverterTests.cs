using System.Text;
using DewIt.Model.Algorithms;
using DewIt.Model.DataTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DewIt.Model.Tests.Algorithms
{
    internal class JSONUrnConverterTests
    {

        [TestCase("example", "a123,z456", null, null, null)]
        [TestCase("example", "a123,z456", null, null, "789")]
        [TestCase("example", "a123,z456", null, "xyz", "789")]
        [TestCase("example", "a123,z456", "abc", null, "789")]
        [TestCase("example", "a123,z456", null, "xyz", null)]
        [TestCase("example", "a123,z456", "abc", "xyz", null)]
        [TestCase("example", "a123,z456", "abc", null, null)]
        [TestCase("example", "a123,z456", "abc", "xyz", "789")]
        public void Types_AreSerializableToJSON(string NID, string NSS, string? RComponent, string? QComponent,
            string? FComponent)
        {
            var urnString = URNTestHelper.ResolveString(NID, NSS, RComponent, QComponent, FComponent);
            var urn = new URN(urnString);
            var options = new JsonSerializerOptions
                { WriteIndented = true, PropertyNameCaseInsensitive = true, Converters = { new JsonURNConverter() } };

            string jsonString = JsonSerializer.Serialize(urn, options);
            var deserialized = JsonSerializer.Deserialize<URN>(jsonString, options);

            Assert.Multiple(() =>
            {
                Assert.That(deserialized, Is.Not.Null);
                Assert.That(deserialized!.AssignedName, Is.EqualTo($"{URN.URNScheme}:{NID}:{NSS}"));
                Assert.That(deserialized.Scheme, Is.EqualTo(URN.URNScheme));
                Assert.That(deserialized.NID, Is.EqualTo(NID));
                Assert.That(deserialized.NSS, Is.EqualTo(NSS));
                Assert.That(deserialized.RComponent, Is.EqualTo(RComponent ?? ""));
                Assert.That(deserialized.QComponent, Is.EqualTo(QComponent ?? ""));
                Assert.That(deserialized.FComponent, Is.EqualTo(FComponent ?? ""));
                Assert.That(deserialized.NameString, Is.EqualTo(urnString));
            });
        }


        [TestCase("[\r\n \"malformed-wrong-braces\": \"urn:example:abc123\" \r\n]")]
        [TestCase("{\r\n \"namestring\": null \r\n}")]
        [TestCase("{\r\n \"wrong-key\": \"urn:example:abc123\" \r\n}")]
        [TestCase("{\r\n \"malformed-no-end\": \"urn:example:abc123\" \r\n")]
        public void JSON_Read_ThrowsWhenURNIsMalformed(string urnString)
        {
            var converter = new JsonURNConverter();
            var utf8JsonReader = new Utf8JsonReader(Encoding.UTF8.GetBytes(urnString), false, new JsonReaderState(new JsonReaderOptions()));
            
            Exception? ex = null;
            try
            {
                utf8JsonReader.Read();
                converter.Read(ref utf8JsonReader, typeof(URN), new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull});
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.That(ex, Is.Not.Null);
        }
    }


}
