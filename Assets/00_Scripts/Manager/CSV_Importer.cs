using System.Collections.Generic;

public class CSV_Importer
{
    public static List<Dictionary<string, object>> EXP = new List<Dictionary<string, object>>(CSVReader.Read("EXP"));
}
