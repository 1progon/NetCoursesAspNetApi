using System.Text.RegularExpressions;

namespace NetCourses.Extensions;

public static class StringExtensions
{
    private static readonly Dictionary<char, string> Dic = new()
    {
        {'а', "a"},
        {'б', "b"},
        {'в', "v"},
        {'г', "g"},
        {'д', "d"},
        {'е', "e"},
        {'ё', "e"},
        {'ж', "zh"},
        {'з', "z"},
        {'и', "i"},
        {'й', "i"},
        {'к', "k"},
        {'л', "l"},
        {'м', "m"},
        {'н', "n"},
        {'о', "o"},
        {'п', "p"},
        {'р', "r"},
        {'с', "s"},
        {'т', "t"},
        {'у', "u"},
        {'ф', "f"},
        {'х', "h"},
        {'ц', "c"},
        {'ч', "ch"},
        {'ш', "sh"},
        {'щ', "sch"},
        {'ъ', ""},
        {'ы', "y"},
        {'ь', ""},
        {'э', "e"},
        {'ю', "yu"},
        {'я', "ya"},
        {' ', "-"},
    };

    public static string ToSlug(this string str)
    {
        var words = str.Trim()
            .ToLower();


        IList<string> slugArr = new List<string>();

        foreach (var letter in words)
        {
            try
            {
                slugArr.Add(Dic[letter]);
            }
            catch (Exception)
            {
                slugArr.Add(letter.ToString());
            }
        }

        var res = string.Join("", slugArr);

        // todo add regex symbols check
        return Regex.Replace(res, "[-,.!?:;'\"]+", "-");
    }
}