using System;
using System.Globalization;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace FatZebra.JsonConverters
{
	/// <summary>
	/// Converts a <see cref="DateTime"/> to mm/yyyy format.
	/// </summary>
	public class ExpiryDateConverter : IsoDateTimeConverter
	{
		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
		/// <param name="value">The value.</param>
		/// <param name="serializer">The calling serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			string text;

			if (value is DateTime)
			{
				DateTime dateTime = (DateTime)value;
				text = dateTime.ToString("MM/yyyy");
			}
			else
			{
				throw new JsonSerializationException("Unexpected value when converting date. Expected DateTime or DateTimeOffset");
			}

			writer.WriteValue(text);
		}
	}
}