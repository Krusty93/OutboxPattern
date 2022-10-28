using System;
using System.Globalization;

namespace OutboxPattern.FunctionApp.Helpers;

public static class DateTimeExtensions
{
    private static readonly DateTime s_epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static long ToUnixTimestamp(this DateTime self) =>
        (long)(self - s_epoch).TotalSeconds;

    public static DateTime AsUnixTimestamp(this long self) =>
        s_epoch.AddTicks(self);

    public static string SerializeDateTime(DateTime dateTime) =>
        new DateTimeOffset(dateTime).ToUniversalTime().ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);

    public static DateTime DeserializeDateTime(string dateTime)
    {
        var isValidUnixValue = long.TryParse(dateTime, out var unixTime);
        if (!isValidUnixValue)
            throw new InvalidOperationException($"Invalid date time: '{dateTime}'");

        return AsUnixTimestamp(unixTime);
    }
}
