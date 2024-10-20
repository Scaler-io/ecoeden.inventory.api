﻿namespace Ecoeden.Inventory.Application.Helpers;
public static class DateTimeHelper
{
    public static DateTime ConvertUtcToIst(DateTime dateTime)
    {
        if(dateTime.Kind != DateTimeKind.Utc) return dateTime;

        TimeZoneInfo zoneInfo = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime istTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, zoneInfo);

        return istTime;
    }
}
