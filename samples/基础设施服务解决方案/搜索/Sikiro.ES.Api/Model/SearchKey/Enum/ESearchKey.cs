namespace Sikiro.ES.Api.Model.SearchKey.Enum
{
    public static class ESearchKey
    {
        public enum EntityType
        {
            Comic = 0,
            Novel = 1,
            Album = 2,
            ChatNovel = 3,
            AllNovel = 4,
            FanNovel = 5,
            Maskword = 6,
        }

        public enum Sort
        {
            Weight = 1,
            ActiveDate = 2
        }
    }
}
