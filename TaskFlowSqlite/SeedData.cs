using TaskFlowModels.Models;

namespace TaskFlowSqlite
{
    internal class SeedData
    {
        public static IEnumerable<ListItem> GetSeedListData()
        {
            return new List<ListItem>
            {
                new ListItem
                {
                    ListItemId = 1,
                    Name = "All Tasks",
                    IsDeletable = false,
                },
                new ListItem
                {
                    ListItemId = 2,
                    Name = "Starred",
                    IsDeletable = false,
                }
            };
        }
    }
}
