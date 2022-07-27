using System;

public enum Starsign
{
    Aires,
    Taurus,
    Gemini,
    Cancer,
    Leo,
    Virgo,
    Libra,
    Scorpio,
    Sagittarius,
    Capricorn,
    Aquarius,
    Pices
}

public class Zodiac
{
    public Starsign Starsign;
    public Element Element;
    public DateTime StartDate, EndDate;

    public static Zodiac Aires => Zodiacs[0];
    public static Zodiac Taurus => Zodiacs[1];
    public static Zodiac Gemini => Zodiacs[2];
    public static Zodiac Cancer => Zodiacs[3];
    public static Zodiac Leo => Zodiacs[4];
    public static Zodiac Virgo => Zodiacs[5];
    public static Zodiac Libra => Zodiacs[6];
    public static Zodiac Scorpio => Zodiacs[7];
    public static Zodiac Sagittarius => Zodiacs[8];
    public static Zodiac Capricorn => Zodiacs[9];
    public static Zodiac Aquarius => Zodiacs[10];
    public static Zodiac Pices => Zodiacs[11];

    public Zodiac(Starsign starsign, Element element, DateTime startDate, DateTime endDate)
    {
        Starsign = starsign;
        Element = element;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static Zodiac GetZodiac(Starsign sign)
    {
        switch (sign)
        {
            case Starsign.Aires: return Aires;
            case Starsign.Taurus: return Taurus;
            case Starsign.Gemini: return Gemini;
            case Starsign.Cancer: return Cancer;
            case Starsign.Leo: return Leo;
            case Starsign.Virgo: return Virgo;
            case Starsign.Libra: return Libra;
            case Starsign.Scorpio: return Scorpio;
            case Starsign.Sagittarius: return Sagittarius;
            case Starsign.Capricorn: return Capricorn;
            case Starsign.Aquarius: return Aquarius;
            case Starsign.Pices: return Pices;
            default: return Pices;
        }
    }

    public static Zodiac[] Zodiacs =
    {
        new Zodiac(Starsign.Aires, Element.Fire, new DateTime(2000, 3, 21), new DateTime(2000, 4, 19)),
        new Zodiac(Starsign.Taurus, Element.Earth, new DateTime(2000, 4, 20), new DateTime(2000, 5, 20)),
        new Zodiac(Starsign.Gemini, Element.Air, new DateTime(2000, 5, 21), new DateTime(2000, 6, 20)),
        new Zodiac(Starsign.Cancer, Element.Water, new DateTime(2000, 6, 21), new DateTime(2000, 7, 22)),
        new Zodiac(Starsign.Leo, Element.Fire, new DateTime(2000, 7, 23), new DateTime(2000, 8, 22)),
        new Zodiac(Starsign.Virgo, Element.Earth, new DateTime(2000, 8, 23), new DateTime(2000, 9, 22)),
        new Zodiac(Starsign.Libra, Element.Air, new DateTime(2000, 9, 23), new DateTime(2000, 10, 22)),
        new Zodiac(Starsign.Scorpio, Element.Water, new DateTime(2000, 10, 23), new DateTime(2000, 11, 21)),
        new Zodiac(Starsign.Sagittarius, Element.Fire, new DateTime(2000, 11, 22), new DateTime(2000, 12, 21)),
        new Zodiac(Starsign.Capricorn, Element.Earth, new DateTime(2000, 12, 22), new DateTime(2000, 1, 19)),
        new Zodiac(Starsign.Aquarius, Element.Air, new DateTime(2000, 1, 20), new DateTime(2000, 2, 18)),
        new Zodiac(Starsign.Pices, Element.Water, new DateTime(2000, 2, 19), new DateTime(2000, 3, 20))
    };

    public static Starsign GetStarsignFromDate(DateTime date)
    {
        date = new DateTime(2000, date.Month, date.Day);
        foreach (Zodiac z in Zodiacs)
            if (date >= z.StartDate && date <= z.EndDate)
                return z.Starsign;

        return Starsign.Pices;
    }
}