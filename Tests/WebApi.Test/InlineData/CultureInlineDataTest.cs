namespace WebApi.Test.InlineData;

public class CultureInlineDataTest : TheoryData<string>
{
    public CultureInlineDataTest()
    {
        Add("en");
        Add("pt");
        Add("pt-PT");
        Add("fr");
    }

    public sealed override void Add(TheoryDataRow<string> row)
    {
        base.Add(row);
    }
}