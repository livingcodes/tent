namespace Ase
{
    public interface ISqlBuilder
    {
        string BuildInsertSql();
        string BuildUpdateSql();
    }
}
