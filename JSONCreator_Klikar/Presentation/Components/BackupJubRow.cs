namespace JSONCreator.Presentation.Components
{
    public class BackupJubRow
    {
        public int Id { get; }
        public string Name { get; }
        public int RetentionCount { get; }
        public int RetentionSize { get; }

        public BackupJubRow(int id, string name, int retentionCount, int retentionSize)
        {
            Id = id;
            Name = name;
            RetentionCount = retentionCount;
            RetentionSize = retentionSize;
        }

        public override string ToString()
        {
            return $"{Name,-30} zálohy: {RetentionCount,-5} velikost: {RetentionSize} GB";
        }
    }
}
