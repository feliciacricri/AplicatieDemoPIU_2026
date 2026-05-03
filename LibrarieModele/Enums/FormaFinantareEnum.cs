namespace LibrarieModele.Enums
{
    public static class FormaFinantareEnum
    {
        public const string CuTaxa = "cu taxa";
        public const string Buget = "buget";
        public const string CuBursa = "cu bursa";

        public static IEnumerable<string> Toate => new[] { CuTaxa, Buget, CuBursa };
    }
}
