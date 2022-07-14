// See https://aka.ms/new-console-template for more information
using LMSLibrary;

Console.WriteLine("Hello, World!");

string query = $@"WITH Subs (StudentId, Name, Id) AS (
        SELECT sub.StudentId, Name, MAX(Id)
        FROM Submission sub
        INNER JOIN Student st ON sub.StudentId = st.StudentId
        WHERE CodingProblemId = %id%
        AND [TimeStamp] > '01-01-2022'
        GROUP BY sub.StudentId, Name
    ),
    Codes (StudentId, Name, Code) AS (
        SELECT s1.StudentId, Name, Code
        FROM Submission s1 
        INNER JOIN Subs s2 ON s1.Id = s2.Id
    )

SELECT c1.Name AS Student1, c2.Name AS Student2, c1.Code AS Code1, c2.Code AS Code2
FROM Codes c1
INNER JOIN Codes c2 ON c1.StudentId > c2.StudentId
";

while (true)
{
    string id = Console.ReadLine();

    var result = SQLHelper.RunSqlQuery("data source=letsusedata.database.windows.net; initial catalog=Material; persist security info=True;user id=marcelo;password=rR`jR34rpVh_>wUr;",
        query.Replace("%id%", id));

    Dictionary<double, List<string>> d = new Dictionary<double, List<string>>();
    foreach (var line in result)
    {
        HashSet<string> s1 = new HashSet<string>(line[2].ToString().Split());
        HashSet<string> s2 = new HashSet<string>(line[3].ToString().Split());

        int n1 = s1.Count;
        int n2 = s2.Count;
        int n = Math.Max(n1, n2);
        int m = s1.Intersect(s2).Count();

        double p = (1.0 * m) / n;

        if (p == 1)
            if (line[2] == line[3])
                p = 2;

        string s = line[0] + " " + line[1] + ": " + Math.Round(p, 2);
        if (d.ContainsKey(p))
            d[p].Add(s);
        else
            d.Add(p, new List<string>() { s });
    }

    double avg = d.Keys.Average();
    double sd = Math.Sqrt(d.Keys.Average(v => Math.Pow(v - avg, 2)));

    foreach (var kv in d)
        if(kv.Key > (avg + 1.5 * sd) || kv.Key > 0.95)
            foreach(var s in kv.Value)
                Console.WriteLine(s);
}


